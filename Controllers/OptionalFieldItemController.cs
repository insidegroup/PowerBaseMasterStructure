using System;
using System.Linq;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Repository;
using CWTDesktopDatabase.ViewModels;
using System.Data.SqlClient;
using System.Xml;

namespace CWTDesktopDatabase.Controllers
{
	public class OptionalFieldItemController : Controller
	{
		//main repository
		private OptionalFieldGroupRepository optionalFieldGroupRepository = new OptionalFieldGroupRepository();
		private OptionalFieldItemRepository optionalFieldItemRepository = new OptionalFieldItemRepository();
        private OptionalFieldRepository optionalFieldRepository = new OptionalFieldRepository();

		public ActionResult List(int id, string filter, int? page, string sortField, int? sortOrder)
		{
			//Get Item From Database
			OptionalFieldGroup optionalFieldGroup = new OptionalFieldGroup();
			optionalFieldGroup = optionalFieldGroupRepository.GetGroup(id);

			//Check Exists
			if (optionalFieldGroup == null)
			{
				ViewData["ActionMethod"] = "ListGet";
				return View("RecordDoesNotExistError");
			}
			//SortField
            if (sortField != "OptionalFieldDisplayOrder" && sortField != "ProductName" && sortField != "SupplierName")
			{
				sortField = "OptionalFieldName";
			}
			ViewData["CurrentSortField"] = sortField;

			//SortOrder
			if (sortOrder == 1)
			{
				ViewData["NewSortOrder"] = 0;
				ViewData["CurrentSortOrder"] = 1;
			}
			else
			{
				ViewData["NewSortOrder"] = 1;
				ViewData["CurrentSortOrder"] = 0;
				sortOrder = 0;
			}
			OptionalFieldItemsVM optionalFieldItemsVM = new OptionalFieldItemsVM();
			optionalFieldItemsVM.OptionalFieldGroup = optionalFieldGroup;

			var paginatedResult = optionalFieldItemRepository.PageOptionalFieldItems(id, page ?? 1, sortField, sortOrder ?? 0);
			optionalFieldItemsVM.OptionalFieldItems = paginatedResult;

			//Can only a maximum of 10 items to a group
			int recordCount = paginatedResult.Count();
			if (recordCount < 10)
			{
				optionalFieldItemsVM.CanCreate = true;
			}

			//Can only edit order if more than 1 item present per group
			if (recordCount > 1)
			{
				optionalFieldItemsVM.CanEditOrder = (paginatedResult.First().CanEditOrder == true);
			}

			//Check Access Rights
			RolesRepository rolesRepository = new RolesRepository();
			if (rolesRepository.HasWriteAccessToOptionalFieldGroup(optionalFieldGroup.OptionalFieldGroupId))
			{
				optionalFieldItemsVM.HasWriteAccess = true;
			}

			return View(optionalFieldItemsVM);
		}


		public ActionResult Create(int id)
		{
			//Get Item From Database
			OptionalFieldGroup optionalFieldGroup = new OptionalFieldGroup();
			optionalFieldGroup = optionalFieldGroupRepository.GetGroup(id);

			//Check Exists
			if (optionalFieldGroup == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToOptionalFieldGroup(id))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			OptionalFieldItemVM optionalFieldItemVM = new OptionalFieldItemVM();

			OptionalFieldItem optionalFieldItem = new OptionalFieldItem();
			optionalFieldItem.OptionalFieldGroup = optionalFieldGroup;
			optionalFieldItemVM.OptionalFieldItem = optionalFieldItem;

			//Get Optional Fields
			SelectList optionalFields = new SelectList(optionalFieldRepository.GetAllOptionalFields().ToList(), "OptionalFieldId",
				"OptionalFieldName");
			optionalFieldItemVM.OptionalFields = optionalFields;

			//Populate List of Products
			ProductRepository productRepository = new ProductRepository();
			SelectList products = new SelectList(productRepository.GetAllProducts().ToList(), "ProductId", "ProductName");
			ViewData["ProductList"] = products;

			return View(optionalFieldItemVM);
		}


		// POST: /Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(OptionalFieldItemVM optionalFieldItemVM)
		{
			//Get Item From Database
			OptionalFieldGroup optionalFieldGroup = new OptionalFieldGroup();
			optionalFieldGroup = optionalFieldGroupRepository.GetGroup(optionalFieldItemVM.OptionalFieldItem.OptionalFieldGroupId);

			//Check Exists
			if (optionalFieldGroup == null)
			{
				ViewData["ActionMethod"] = "EditPost";
				return View("RecordDoesNotExistError");
			}

			//Check Access Rights
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToOptionalFieldGroup(optionalFieldItemVM.OptionalFieldItem.OptionalFieldGroupId))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel<OptionalFieldItem>(optionalFieldItemVM.OptionalFieldItem, "OptionalFieldItem");
			}
			catch
			{
				string n = "";
				foreach (ModelState modelState in ViewData.ModelState.Values)
				{
					foreach (ModelError error in modelState.Errors)
					{
						n += error.ErrorMessage;
					}
				}
				ViewData["Message"] = "ValidationError : " + n;
				return View("Error");
			}


			//Database Update
			try
			{
				optionalFieldItemRepository.Add(optionalFieldItemVM.OptionalFieldItem);
			}
			catch (SqlException ex)
			{

				//Can only a maximum of 10 items to a group
				if (ex.Message == "SQLMaximumNumberReached")
				{
					ViewData["Message"] = "Sorry, the maximum number of 10 items for this group has already been reached. Please delete an item before adding a new one."; 
					return View("Error");
				}
				
				//Non-Unique Name
				if (ex.Message == "NonUniqueName")
				{
					return View("NonUniqueNameError");
				}

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);
				
				return View("Error");
			}

			ViewData["NewSortOrder"] = 0;
			return RedirectToAction("List", new {id = optionalFieldItemVM.OptionalFieldItem.OptionalFieldGroupId});
		}

        public ActionResult SelectOptionalFieldToOrder(int id)
        {

            OptionalFieldGroup optionalFieldGroup = new OptionalFieldGroup();
            optionalFieldGroup = optionalFieldGroupRepository.GetGroup(id);

            OptionalFieldItemOrderSelectionVM optionalFieldItemOrderSelectionVM = new OptionalFieldItemOrderSelectionVM();
            
			ViewData["Products"] = new SelectList(optionalFieldItemRepository.GetOptionalFieldItemOptionalFieldTypes(id), "ProductId", "ProductName");

            optionalFieldItemOrderSelectionVM.OptionalFieldGroup = optionalFieldGroup;

            return View(optionalFieldItemOrderSelectionVM);
        }

        // POST: /SelectServicingOptionToOrder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelectOptionalFieldToOrder(OptionalFieldItemOrderSelectionVM optionalFieldItemOrderSelectionVM)
        {
            int groupId = optionalFieldItemOrderSelectionVM.OptionalFieldGroup.OptionalFieldGroupId;
			//int id = optionalFieldItemOrderSelectionVM.OptionalFieldId;
			int productId = optionalFieldItemOrderSelectionVM.ProductId;

            //Get Group
            OptionalFieldGroup optionalFieldGroup = new OptionalFieldGroup();
            optionalFieldGroup = optionalFieldGroupRepository.GetGroup(groupId);
            
            //Check Exists
            if (optionalFieldGroup == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }

            //Return
            return RedirectToAction("EditSequence", new { groupid = groupId, productId = productId });
        }

        // GET: /EditSequence
        public ActionResult EditSequence(int groupid, int productId, int? page)
        {
            //Get Group
            OptionalFieldGroup optionalFieldGroup = new OptionalFieldGroup();
            optionalFieldGroup = optionalFieldGroupRepository.GetGroup(groupid);

            //Check Exists
            if (optionalFieldGroup == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }

            //Check Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToOptionalFieldGroup(groupid))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
             OptionalFieldItemSequenceRepository optionalFieldItemSequenceRepository = new OptionalFieldItemSequenceRepository();
             OptionalFieldRepository optionalFieldRepository = new OptionalFieldRepository();

			 //Only show products where more than one item is present
             OptionalFieldItemSequencesVM optionalFieldItemSequencesVM = new OptionalFieldItemSequencesVM();
			 optionalFieldItemSequencesVM.OptionalFieldItemSequences = optionalFieldItemSequenceRepository.GetOptionalFieldItemSequences(groupid, productId, page ?? 1);
             optionalFieldItemSequencesVM.OptionalFieldGroup = optionalFieldGroup;
			 //optionalFieldItemSequencesVM.OptionalField = optionalFieldRepository.GetItem(id);
			 //optionalFieldItemSequencesVM.OptionalFieldId = id;

             ViewData["Page"] = page ?? 1;


             return View(optionalFieldItemSequencesVM);
        }

        // POST: /EditSequence
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSequence(int groupid, int page, FormCollection collection)
        {

            //Get Group
            OptionalFieldGroup optionalFieldGroup = new OptionalFieldGroup();
            optionalFieldGroup = optionalFieldGroupRepository.GetGroup(groupid);

            //Check Exists
            if (optionalFieldGroup == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }

			////Check Exists
			//OptionalField optionalField = new OptionalField();
			//OptionalFieldRepository optionalFieldRepository = new OptionalFieldRepository();
			//optionalField = optionalFieldRepository.GetItem(id);
			//if (optionalField == null)
			//{
			//	ViewData["ActionMethod"] = "EditSequencePost";
			//	return View("RecordDoesNotExistError");
			//}

            //Check Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToOptionalFieldGroup(groupid))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            string[] sequences = collection["Sequence"].Split(new char[] { ',' });

            int sequence = (page - 1 * 5) - 2;
            if (sequence < 0)
            {
                sequence = 1;
            }

            XmlDocument doc = new XmlDocument();// Create the XML Declaration, and append it to XML document
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(dec);
            XmlElement root = doc.CreateElement("SequenceXML");
            doc.AppendChild(root);

            foreach (string s in sequences)
            {
                string[] primaryKey = s.Split(new char[] { '_' });

                int servicingOptionItemId = Convert.ToInt32(primaryKey[0]);
                int versionNumber = Convert.ToInt32(primaryKey[1]);

                XmlElement xmlItem = doc.CreateElement("Item");
                root.AppendChild(xmlItem);

                XmlElement xmlSequence = doc.CreateElement("Sequence");
                xmlSequence.InnerText = sequence.ToString();
                xmlItem.AppendChild(xmlSequence);

				XmlElement xmlServicingOptionItemId = doc.CreateElement("OptionalFieldItemId");
				xmlServicingOptionItemId.InnerText = servicingOptionItemId.ToString();
				xmlItem.AppendChild(xmlServicingOptionItemId);

                XmlElement xmlVersionNumber = doc.CreateElement("VersionNumber");
                xmlVersionNumber.InnerText = versionNumber.ToString();
                xmlItem.AppendChild(xmlVersionNumber);

                sequence = sequence + 1;
            }

            try
            {
                OptionalFieldItemSequenceRepository optionalFieldItemSequenceRepository = new OptionalFieldItemSequenceRepository();
                optionalFieldItemSequenceRepository.UpdateOptionalFieldItemSequences(System.Xml.Linq.XElement.Parse(doc.OuterXml));
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/servicingOptionItem.mvc/EditSequence?page=" + page + "&groupid=" + groupid;
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List", new { id = groupid });
        }

		// GET: /View
		public ActionResult View(int id)
		{
			OptionalFieldItemVM optionalFieldItemVM = new OptionalFieldItemVM();

			//Check Exists
			OptionalFieldItem optionalFieldItem = new OptionalFieldItem();
			optionalFieldItem = optionalFieldItemRepository.GetItem(id);
			if (optionalFieldItem == null)
			{
				ViewData["ActionMethod"] = "ViewGet";
				return View("RecordDoesNotExistError");
			}

			//Get Group
			OptionalFieldGroup optionalFieldGroup = new OptionalFieldGroup();
			optionalFieldGroup = optionalFieldGroupRepository.GetGroup(optionalFieldItem.OptionalFieldGroupId);
			if (optionalFieldGroup != null)
			{
				optionalFieldItemVM.OptionalFieldGroup = optionalFieldGroup;
			}

			//Get Supplier
			if (optionalFieldItem.ProductId != null && optionalFieldItem.SupplierCode != null)
			{
				SupplierRepository supplierRepository = new SupplierRepository();
				Supplier supplier = supplierRepository.GetSupplier(optionalFieldItem.SupplierCode, (int) optionalFieldItem.ProductId);
				optionalFieldItem.SupplierName = supplier.SupplierName;
			}

			//Get Product
			if (optionalFieldItem.ProductId != null)
			{
				ProductRepository productRepository = new ProductRepository();
				Product product = productRepository.GetProduct((int) optionalFieldItem.ProductId);
				optionalFieldItem.ProductName = product.ProductName;
			}

			optionalFieldItemVM.OptionalFieldItem = optionalFieldItem;

			return View(optionalFieldItemVM);
		}

		public ActionResult Edit(int id)
		{
			//Get Item From Database
			OptionalFieldItem optionalFieldItem = new OptionalFieldItem();
			optionalFieldItem = optionalFieldItemRepository.GetItem(id);

			//Check Exists
			if (optionalFieldItem == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToOptionalFieldGroup(optionalFieldItem.OptionalFieldGroupId))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			OptionalFieldItemVM optionalFieldItemVM = new OptionalFieldItemVM();
			optionalFieldItemVM.OptionalFieldItem = optionalFieldItem;

			//Get Optional Fields
			SelectList optionalFields = new SelectList(
				optionalFieldRepository.GetAllOptionalFields().ToList(),
				"OptionalFieldId",
				"OptionalFieldName",
				optionalFieldItem.OptionalFieldId);

			optionalFieldItemVM.OptionalFields = optionalFields;

			//Get Supplier
			if (optionalFieldItem.ProductId != null && optionalFieldItem.SupplierCode != null)
			{
				SupplierRepository supplierRepository = new SupplierRepository();
				Supplier supplier = supplierRepository.GetSupplier(optionalFieldItem.SupplierCode, (int) optionalFieldItem.ProductId);
				optionalFieldItem.SupplierName = supplier.SupplierName;
			}

			//Populate List of Products
			ProductRepository productRepository = new ProductRepository();
			SelectList products = new SelectList(productRepository.GetAllProducts().ToList(),
				"ProductId",
				"ProductName",
				optionalFieldItem.ProductId);

			ViewData["ProductList"] = products;

			return View(optionalFieldItemVM);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(OptionalFieldItemVM optionalFieldItemVM)
		{

			OptionalFieldItem optionalFieldItem = new OptionalFieldItem();
			optionalFieldItem = optionalFieldItemRepository.GetItem(optionalFieldItemVM.OptionalFieldItem.OptionalFieldItemId);

			//Check Exists
			if (optionalFieldItem == null)
			{
				ViewData["ActionMethod"] = "EditPost";
				return View("RecordDoesNotExistError");
			}

			//Update  Model from Form
			try
			{
				UpdateModel<OptionalFieldItem>(optionalFieldItem, "OptionalFieldItem");
			}
			catch
			{
				string n = "";
				foreach (ModelState modelState in ViewData.ModelState.Values)
				{
					foreach (ModelError error in modelState.Errors)
					{
						n += error.ErrorMessage;
					}
				}
				ViewData["Message"] = "ValidationError : " + n;
				return View("Error");
			}

			//Database Update
			try
			{
				optionalFieldItemRepository.Update(optionalFieldItem);
			}
			catch (SqlException ex)
			{
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] =
					"There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("List", new {id = optionalFieldItem.OptionalFieldGroup.OptionalFieldGroupId});
		}

		// GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id)
		{
			//Get Item From Database
			OptionalFieldItem optionalFieldItem = new OptionalFieldItem();
			optionalFieldItem = optionalFieldItemRepository.GetItem(id);

			//Check Exists
			if (optionalFieldItem == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToOptionalFieldGroup(optionalFieldItem.OptionalFieldGroupId))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			OptionalFieldItemVM optionalFieldItemVM = new OptionalFieldItemVM();

			//Get Group
			OptionalFieldGroup optionalFieldGroup = new OptionalFieldGroup();
			optionalFieldGroup = optionalFieldGroupRepository.GetGroup(optionalFieldItem.OptionalFieldGroupId);
			if (optionalFieldGroup != null)
			{
				optionalFieldItemVM.OptionalFieldGroup = optionalFieldGroup;
			}

			//Get Supplier
			if (optionalFieldItem.ProductId != null && optionalFieldItem.SupplierCode != null)
			{
				SupplierRepository supplierRepository = new SupplierRepository();
				Supplier supplier = supplierRepository.GetSupplier(optionalFieldItem.SupplierCode, (int) optionalFieldItem.ProductId);
				optionalFieldItem.SupplierName = supplier.SupplierName;
			}

			//Get Product
			if (optionalFieldItem.ProductId != null)
			{
				ProductRepository productRepository = new ProductRepository();
				Product product = productRepository.GetProduct((int) optionalFieldItem.ProductId);
				optionalFieldItem.ProductName = product.ProductName;
			}

			optionalFieldItemVM.OptionalFieldItem = optionalFieldItem;

			return View(optionalFieldItemVM);
		}



		// POST: /Delete
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(OptionalFieldItemVM optionalFieldItemVM)
		{
			//Get Item
			OptionalFieldItem optionalFieldItem = new OptionalFieldItem();
			optionalFieldItem = optionalFieldItemRepository.GetItem(optionalFieldItemVM.OptionalFieldItem.OptionalFieldItemId);

			//Check Exists
			if (optionalFieldItem == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}


			//Delete Item
			try
			{
				optionalFieldItemRepository.Delete(optionalFieldItemVM.OptionalFieldItem);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/OptionalFieldItem.mvc/Delete/" + optionalFieldItem.OptionalFieldItemId.ToString();
					return View("VersionError");
				}
				//Generic Error
				ViewData["Message"] =
					"There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			//Return
			return RedirectToAction("List", new {id = optionalFieldItem.OptionalFieldGroup.OptionalFieldGroupId});

		}

		/*
		 * 
		 * // Sequencing
		public ActionResult EditSequenceTypeSelection(int id)
		{

			OptionalFieldGroup optionalFieldGroup = new OptionalFieldGroup();
			optionalFieldGroup = optionalFieldGroupRepository.GetGroup(id);

			ProductOptionalFieldItemOrderTypeSelectionVM productOptionalFieldItemOrderTypeSelectionVM = new ProductOptionalFieldItemOrderTypeSelectionVM();
			productOptionalFieldItemOrderTypeSelectionVM.OptionalFieldTypes = new SelectList(
																			optionalFieldItemRepository.GetOptionalFieldItemOptionalFieldTypes(id),
																			"OptionalFieldId",
																			"OptionalFieldName");

			productOptionalFieldItemOrderTypeSelectionVM.OptionalFieldGroup = optionalFieldGroup;

			return View(productOptionalFieldItemOrderTypeSelectionVM);
		}


		
		 * public ActionResult EditSequence(int id, int optionalFieldId, int? page)
		{

			//Get Item From Database
			OptionalFieldGroup group = new OptionalFieldGroup();
			group = optionalFieldGroupRepository.GetGroup(id);

			//Check Exists
			if (group == null)
			{
				ViewData["ActionMethod"] = "EditSequenceGet";
				return View("RecordDoesNotExistError");
			}
			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToOptionalFieldGroup(id))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			OptionalFieldItemSequenceRepository optionalFieldItemSequenceRepository = new OptionalFieldItemSequenceRepository();

			OptionalFieldGroupOptionalFieldTypeSequencingVM optionalFieldGroupOptionalFieldTypeSequencingVM = new OptionalFieldGroupOptionalFieldTypeSequencingVM();
			optionalFieldGroupOptionalFieldTypeSequencingVM.SequenceItems = optionalFieldItemSequenceRepository.GetOptionalFieldItemSequences(id, optionalFieldTypeId, page ?? 1);
			optionalFieldGroupOptionalFieldTypeSequencingVM.OptionalFieldGroup = group;
			optionalFieldGroupOptionalFieldTypeSequencingVM.OptionalFieldTypeId = optionalFieldTypeId;
			optionalFieldGroupOptionalFieldTypeSequencingVM.Page = page ?? 1;

			return View(optionalFieldGroupOptionalFieldTypeSequencingVM);

		}

		
		// POST: /EditSequence
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult EditSequence(OptionalFieldGroupOptionalFieldTypeSequencingVM optionalFieldGroupOptionalFieldTypeSequencingVM,
			FormCollection collection)
		{

			int optionalFieldGroupId = optionalFieldGroupOptionalFieldTypeSequencingVM.OptionalFieldGroup.OptionalFieldGroupId;
			int optionalFieldTypeId = optionalFieldGroupOptionalFieldTypeSequencingVM.OptionalFieldTypeId;
			int page = optionalFieldGroupOptionalFieldTypeSequencingVM.Page;

			//Check Exists
			OptionalFieldGroup optionalFieldGroup = new OptionalFieldGroup();
			optionalFieldGroup = optionalFieldGroupRepository.GetGroup(optionalFieldGroupId);
			if (optionalFieldGroup == null)
			{
				return View("Error");
			}
			
			//Check Access Rights
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToOptionalFieldGroup(optionalFieldGroupId))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}
			string[] sequences = collection["RCSequence"].Split(new char[] {','});

			int sequence = (page - 1*5) - 2;
			if (sequence < 0)
			{
				sequence = 1;
			}

			string xml = "<SequenceXML>";
			foreach (string s in sequences)
			{
				string[] optionalFieldItemIdPK = s.Split(new char[] {'_'});

				int optionalFieldItemId = Convert.ToInt32(optionalFieldItemIdPK[0]);
				int versionNumber = Convert.ToInt32(optionalFieldItemIdPK[1]);

				xml = xml + "<Item>";
				xml = xml + "<Sequence>" + sequence + "</Sequence>";
				xml = xml + "<OptionalFieldItemId>" + optionalFieldItemId + "</OptionalFieldItemId>";
				xml = xml + "<VersionNumber>" + versionNumber + "</VersionNumber>";
				xml = xml + "</Item>";

				sequence = sequence + 1;
			}
			xml = xml + "</SequenceXML>";

			try
			{
				OptionalFieldItemSequenceRepository optionalFieldItemSequenceRepository = new OptionalFieldItemSequenceRepository();
				optionalFieldItemSequenceRepository.UpdateOptionalFieldItemSequences(xml);
			}
			catch (SqlException ex)
			{
				//Versioning Error
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/OptionalFieldItem.mvc/EditSequence?OptionalFieldTypeId=" + optionalFieldTypeId + "&id=" +
											optionalFieldGroupId + "?page=" + page;
					return View("VersionError");
				}
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] =
					"There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");

			}

			return RedirectToAction("List", new {id = optionalFieldGroupId});
		}*/
	}
}