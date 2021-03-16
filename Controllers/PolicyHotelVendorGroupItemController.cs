using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Repository;
using System.Data.SqlClient;
using System.Xml;


namespace CWTDesktopDatabase.Controllers
{
    public class PolicyHotelVendorGroupItemController : Controller
    {
        //main repositories
        PolicyHotelVendorGroupItemRepository policyHotelVendorGroupItemRepository = new PolicyHotelVendorGroupItemRepository();
        PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();

        //GET: A list of PolicyGroupHotelVendor Items for this PolicyGroup
        public ActionResult List(int id, int? page, string sortField, int? sortOrder)
        {
            //Set Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToPolicyGroup(id))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //Parent Information
            ViewData["PolicyGroupID"] = id;
            ViewData["PolicyGroupName"] = policyGroupRepository.GetGroup(id).PolicyGroupName;


            //SortField + SortOrder settings
            if (string.IsNullOrEmpty(sortField))
            {
                sortField = "SequenceNumber";
            }
            ViewData["CurrentSortField"] = sortField;

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

            //Get data
			var paginatedView = policyHotelVendorGroupItemRepository.GetPolicyHotelVendorGroupItemsByPolicyGroup(id, page ?? 1, sortField, sortOrder ?? 0);

            //Return View
            return View(paginatedView);
        }

        // GET: /Create
        public ActionResult Create(int id)
        {
            PolicyGroup policyGroup = policyGroupRepository.GetGroup(id);
            if (policyGroup == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }
            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(id))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Populate List of PolicyLocations
            PolicyLocationRepository policyLocationRepository = new PolicyLocationRepository();
            SelectList policyLocations = new SelectList(policyLocationRepository.GetAllPolicyLocations().ToList(), "PolicyLocationId", "PolicyLocationName");
            ViewData["PolicyLocationList"] = policyLocations;

            //Populate List of PolicyHotelStatuses 
            PolicyHotelStatusRepository policyHotelStatusRepository = new PolicyHotelStatusRepository();
            SelectList policyHotelStatuses = new SelectList(policyHotelStatusRepository.GetAllPolicyHotelStatuses().ToList(), "PolicyHotelStatusId", "PolicyHotelStatusDescription");
            ViewData["PolicyHotelStatusList"] = policyHotelStatuses;

            //Populate List of Products
            ProductRepository productRepository = new ProductRepository();
            SelectList products = new SelectList(productRepository.GetAllProducts().ToList(), "ProductId", "ProductName");
            ViewData["ProductList"] = products;

            //populate new PolicyHotelVendorGroupItem with known PolicyGroup Information           
            PolicyHotelVendorGroupItem policyHotelVendorGroupItem = new PolicyHotelVendorGroupItem();
            policyHotelVendorGroupItem.PolicyGroupId = id;
            policyHotelVendorGroupItem.PolicyGroupName = policyGroup.PolicyGroupName;

            //Show 'Create' Form
            return View(policyHotelVendorGroupItem);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PolicyHotelVendorGroupItem policyHotelVendorGroupItem)
        {

            PolicyGroup policyGroup = policyGroupRepository.GetGroup(policyHotelVendorGroupItem.PolicyGroupId);
            if (policyGroup == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }
            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyHotelVendorGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update Model from Form
            try
            {
                UpdateModel(policyHotelVendorGroupItem);
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
            try
            {
                policyHotelVendorGroupItemRepository.Add(policyHotelVendorGroupItem);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }
            return RedirectToAction("List", new { id = policyHotelVendorGroupItem.PolicyGroupId });
        }


        // GET: /Edit
        public ActionResult Edit(int id)
        {
            //Get PolicyHotelVendorGroupItem
            PolicyHotelVendorGroupItem policyHotelVendorGroupItem = new PolicyHotelVendorGroupItem();
            policyHotelVendorGroupItem = policyHotelVendorGroupItemRepository.GetPolicyHotelVendorGroupItem(id);

            //Check Exists
            if (policyHotelVendorGroupItem == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyHotelVendorGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Populate List of PolicyLocations
            PolicyLocationRepository policyLocationRepository = new PolicyLocationRepository();
            SelectList policyLocations = new SelectList(policyLocationRepository.GetAllPolicyLocations().ToList(), "PolicyLocationId", "PolicyLocationName");
            ViewData["PolicyLocationList"] = policyLocations;

            //Populate List of PolicyHotelStatuses 
            PolicyHotelStatusRepository policyHotelStatusRepository = new PolicyHotelStatusRepository();
            SelectList policyHotelStatuses = new SelectList(policyHotelStatusRepository.GetAllPolicyHotelStatuses().ToList(), "PolicyHotelStatusId", "PolicyHotelStatusDescription");
            ViewData["PolicyHotelStatusList"] = policyHotelStatuses;

            //Populate List of Products
            ProductRepository productRepository = new ProductRepository();
            SelectList products = new SelectList(productRepository.GetAllProducts().ToList(), "ProductId", "ProductName");
            ViewData["ProductList"] = products;

            //Populate new PolicyHotelVendorGroupItem with known PolicyGroup Information
            PolicyGroup policyGroup = policyGroupRepository.GetGroup(policyHotelVendorGroupItem.PolicyGroupId);
            policyHotelVendorGroupItem.PolicyGroupName = policyGroup.PolicyGroupName;

            //return edit form
            policyHotelVendorGroupItemRepository.EditItemForDisplay(policyHotelVendorGroupItem);
            return View(policyHotelVendorGroupItem);
        }

        // POST /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection collection)
        {
            PolicyHotelVendorGroupItem policyHotelVendorGroupItem = new PolicyHotelVendorGroupItem();
            policyHotelVendorGroupItem = policyHotelVendorGroupItemRepository.GetPolicyHotelVendorGroupItem(id);

            //Check Exists
            if (policyHotelVendorGroupItem == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyHotelVendorGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update PolicyHotelVendorGroupItem Model From Form
            try
            {
                UpdateModel(policyHotelVendorGroupItem);
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
                policyHotelVendorGroupItemRepository.Update(policyHotelVendorGroupItem);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/PolicyHotelVendorGroupItem.mvc/Edit/" + policyHotelVendorGroupItem.PolicyHotelVendorGroupItemId;
                    return View("VersionError");
                }

                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");            
            }

            //Return to Form
            return RedirectToAction("List", new { id = policyHotelVendorGroupItem.PolicyGroupId });

        }

        // GET: /View
        public ActionResult View(int id)
        {
            //Get PolicyHotelVendorGroupItem
            PolicyHotelVendorGroupItem policyHotelVendorGroupItem = new PolicyHotelVendorGroupItem();
            policyHotelVendorGroupItem = policyHotelVendorGroupItemRepository.GetPolicyHotelVendorGroupItem(id);

            //Check Exists
            if (policyHotelVendorGroupItem == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            //populate new PolicyHotelVendorGroupItem with known PolicyGroup Information
            policyHotelVendorGroupItemRepository.EditItemForDisplay(policyHotelVendorGroupItem);

            //Show 'View' Form
            return View(policyHotelVendorGroupItem);
        }

        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id)
        {
            //Get PolicyHotelVendorGroupItem
            PolicyHotelVendorGroupItem policyHotelVendorGroupItem = new PolicyHotelVendorGroupItem();
            policyHotelVendorGroupItem = policyHotelVendorGroupItemRepository.GetPolicyHotelVendorGroupItem(id);

            //Check Exists
            if (policyHotelVendorGroupItem == null)
            {
                ViewData["ActionMethod"] = "GetDelete";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyHotelVendorGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //populate new PolicyHotelVendorGroupItem with known PolicyGroup Information
            policyHotelVendorGroupItemRepository.EditItemForDisplay(policyHotelVendorGroupItem);

            //Show 'Create' Form
            return View(policyHotelVendorGroupItem);
        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            //Get PolicyHotelVendorGroupItem
            PolicyHotelVendorGroupItem policyHotelVendorGroupItem = new PolicyHotelVendorGroupItem();
            policyHotelVendorGroupItem = policyHotelVendorGroupItemRepository.GetPolicyHotelVendorGroupItem(id);

            //Check Exists
            if (policyHotelVendorGroupItem == null)
            {
                ViewData["ActionMethod"] = "PostDelete";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyHotelVendorGroupItem.PolicyGroupId))
            {
                return View("Error");
            }

            //Delete Item
            try
            {
                policyHotelVendorGroupItem.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                policyHotelVendorGroupItemRepository.Delete(policyHotelVendorGroupItem);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/PolicyHotelVendorGroupItem.mvc/Delete/" + policyHotelVendorGroupItem.PolicyHotelVendorGroupItemId;
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            //Return
            return RedirectToAction("List", new { id = policyHotelVendorGroupItem.PolicyGroupId });
        }

		// GET: /EditSequence
		public ActionResult EditSequence(int id, int? page)
		{
			//Check Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(id);
			if (policyGroup == null)
			{
				ViewData["ActionMethod"] = "EditSequenceGet";
				return View("RecordDoesNotExistError");
			}

			//Check Access Rights
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToPolicyGroup(id))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}
			PolicyGroupSequenceRepository policyGroupSequenceRepository = new PolicyGroupSequenceRepository();
			var result = policyGroupSequenceRepository.GetPolicyHotelVendorGroupItemSequences(id, page ?? 1);

			ViewData["Page"] = page ?? 1;
			ViewData["PolicyGroupId"] = id;
			ViewData["PolicyGroupName"] = policyGroup.PolicyGroupName;
			return View(result);
		}

		// POST: /EditSequence
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult EditSequence(int page, int id, FormCollection collection)
		{

			//Check Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(id);
			if (policyGroup == null)
			{
				ViewData["ActionMethod"] = "EditSequencePost";
				return View("RecordDoesNotExistError");
			}

			//Check Access Rights
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToPolicyGroup(id))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			string[] sequences = collection["PolicySequence"].Split(new char[] { ',' });

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
				string[] policyPolicyHotelVendorItemIdPK = s.Split(new char[] { '_' });

				int PolicyHotelVendorItemIdId = Convert.ToInt32(policyPolicyHotelVendorItemIdPK[0]);
				int versionNumber = Convert.ToInt32(policyPolicyHotelVendorItemIdPK[1]);

				XmlElement xmlItem = doc.CreateElement("Item");
				root.AppendChild(xmlItem);

				XmlElement xmlSequence = doc.CreateElement("Sequence");
				xmlSequence.InnerText = sequence.ToString();
				xmlItem.AppendChild(xmlSequence);

				XmlElement xmlPolicyHotelVendorItemIdId = doc.CreateElement("PolicyHotelVendorGroupItemId");
				xmlPolicyHotelVendorItemIdId.InnerText = PolicyHotelVendorItemIdId.ToString();
				xmlItem.AppendChild(xmlPolicyHotelVendorItemIdId);

				XmlElement xmlVersionNumber = doc.CreateElement("VersionNumber");
				xmlVersionNumber.InnerText = versionNumber.ToString();
				xmlItem.AppendChild(xmlVersionNumber);

				sequence = sequence + 1;
			}

			try
			{
				PolicyGroupSequenceRepository policyGroupSequenceRepository = new PolicyGroupSequenceRepository();
				policyGroupSequenceRepository.UpdatePolicyHotelVendorGroupItemSequences(System.Xml.Linq.XElement.Parse(doc.OuterXml));
			}
			catch (SqlException ex)
			{
				//Versioning Error
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/PolicyHotelVendorGroupItem.mvc/EditSequence/" + id + "?page=" + page;
					return View("VersionError");
				}
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("List", new { id = id });
		}
    }
}
