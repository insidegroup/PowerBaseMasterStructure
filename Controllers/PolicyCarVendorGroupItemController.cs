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
    public class PolicyCarVendorGroupItemController : Controller
    {
        //main repositories
        PolicyCarVendorGroupItemRepository policyCarVendorGroupItemRepository = new PolicyCarVendorGroupItemRepository();
        PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();
        PolicyRoutingRepository policyRoutingRepository = new PolicyRoutingRepository();

        //GET: A list of PolicyGroupCarVendor Items for this PolicyGroup
        public ActionResult List(int id, int? page, string sortField, int? sortOrder)
        {
            PolicyGroup group = new PolicyGroup();
            group = policyGroupRepository.GetGroup(id);

            //Check Exists
            if (group == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }

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
            }

            var items = policyCarVendorGroupItemRepository.GetPolicyCarVendorGroupItems(id, sortField, sortOrder ?? 0, page ?? 1);
            return View(items);
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

            //Populate List of PolicyCarStatuses 
            PolicyCarStatusRepository policyCarStatusRepository = new PolicyCarStatusRepository();
            SelectList policyCarStatuses = new SelectList(policyCarStatusRepository.GetAllPolicyCarStatuses().ToList(), "PolicyCarStatusId", "PolicyCarStatusDescription");
            ViewData["PolicyCarStatusList"] = policyCarStatuses;

            //Populate List of Products 
            ProductRepository productRepository = new ProductRepository();
            SelectList products = new SelectList(productRepository.GetAllProducts().ToList(), "ProductId", "ProductName");
            ViewData["ProductList"] = products;

            //populate new PolicyCarVendorGroupItem with known PolicyGroup Information           
            PolicyCarVendorGroupItem policyCarVendorGroupItem = new PolicyCarVendorGroupItem();
            policyCarVendorGroupItem.PolicyGroupId = id;
            policyCarVendorGroupItem.PolicyGroupName = policyGroup.PolicyGroupName;

            //Show 'Create' Form
            return View(policyCarVendorGroupItem);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PolicyCarVendorGroupItem policyCarVendorGroupItem)
        {

            PolicyGroup policyGroup = policyGroupRepository.GetGroup(policyCarVendorGroupItem.PolicyGroupId);
            if (policyGroup == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }
            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyCarVendorGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update Model from Form
            try
            {
                UpdateModel(policyCarVendorGroupItem);
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
                policyCarVendorGroupItemRepository.Add(policyCarVendorGroupItem);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }
            return RedirectToAction("List", new { id = policyCarVendorGroupItem.PolicyGroupId });
        }


        // GET: /Edit
        public ActionResult Edit(int id)
        {
            //Get PolicyCarVendorGroupItem
            PolicyCarVendorGroupItem policyCarVendorGroupItem = new PolicyCarVendorGroupItem();
            policyCarVendorGroupItem = policyCarVendorGroupItemRepository.GetPolicyCarVendorGroupItem(id);

            //Check Exists
            if (policyCarVendorGroupItem == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyCarVendorGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Populate List of PolicyLocations
            PolicyLocationRepository policyLocationRepository = new PolicyLocationRepository();
            SelectList policyLocations = new SelectList(policyLocationRepository.GetAllPolicyLocations().ToList(), "PolicyLocationId", "PolicyLocationName");
            ViewData["PolicyLocationList"] = policyLocations;

            //Populate List of PolicyCarStatuses 
            PolicyCarStatusRepository policyCarStatusRepository = new PolicyCarStatusRepository();
            SelectList policyCarStatuses = new SelectList(policyCarStatusRepository.GetAllPolicyCarStatuses().ToList(), "PolicyCarStatusId", "PolicyCarStatusDescription");
            ViewData["PolicyCarStatusList"] = policyCarStatuses;

            //Populate List of Products 
            ProductRepository productRepository = new ProductRepository();
            SelectList products = new SelectList(productRepository.GetAllProducts().ToList(), "ProductId", "ProductName");
            ViewData["ProductList"] = products;

            //Populate new PolicyCarVendorGroupItem with known PolicyGroup Information
            PolicyGroup policyGroup = policyGroupRepository.GetGroup(policyCarVendorGroupItem.PolicyGroupId);
            policyCarVendorGroupItem.PolicyGroupName = policyGroup.PolicyGroupName;

            //return edit form
            policyCarVendorGroupItemRepository.EditItemForDisplay(policyCarVendorGroupItem);
            return View(policyCarVendorGroupItem);
        }

        // POST /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection collection)
        {
            PolicyCarVendorGroupItem policyCarVendorGroupItem = new PolicyCarVendorGroupItem();
            policyCarVendorGroupItem = policyCarVendorGroupItemRepository.GetPolicyCarVendorGroupItem(id);

            //Check Exists
            if (policyCarVendorGroupItem == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyCarVendorGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update PolicyCarVendorGroupItem Model From Form
            try
            {
                UpdateModel(policyCarVendorGroupItem);          
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
                policyCarVendorGroupItemRepository.Update(policyCarVendorGroupItem);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/PolicyCarVendorGroupItem.mvc/Edit/" + policyCarVendorGroupItem.PolicyCarVendorGroupItemId;
                    return View("VersionError");
                }

                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");            }

            //Return to Form
            return RedirectToAction("List", new { id = policyCarVendorGroupItem.PolicyGroupId });

        }

        // GET: /View
        public ActionResult View(int id)
        {
            //Get PolicyCarVendorGroupItem
            PolicyCarVendorGroupItem policyCarVendorGroupItem = new PolicyCarVendorGroupItem();
            policyCarVendorGroupItem = policyCarVendorGroupItemRepository.GetPolicyCarVendorGroupItem(id);

            //Check Exists
            if (policyCarVendorGroupItem == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            //populate new PolicyCarVendorGroupItem with known PolicyGroup Information
            policyCarVendorGroupItemRepository.EditItemForDisplay(policyCarVendorGroupItem);

            //Show 'View' Form
            return View(policyCarVendorGroupItem);
        }

        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id)
        {
            //Get PolicyCarVendorGroupItem
            PolicyCarVendorGroupItem policyCarVendorGroupItem = new PolicyCarVendorGroupItem();
            policyCarVendorGroupItem = policyCarVendorGroupItemRepository.GetPolicyCarVendorGroupItem(id);

            //Check Exists
            if (policyCarVendorGroupItem == null)
            {
                ViewData["ActionMethod"] = "GetDelete";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyCarVendorGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //populate new PolicyCarVendorGroupItem with known PolicyGroup Information
            policyCarVendorGroupItemRepository.EditItemForDisplay(policyCarVendorGroupItem);

            //Show 'Create' Form
            return View(policyCarVendorGroupItem);
        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            //Get PolicyCarVendorGroupItem
            PolicyCarVendorGroupItem policyCarVendorGroupItem = new PolicyCarVendorGroupItem();
            policyCarVendorGroupItem = policyCarVendorGroupItemRepository.GetPolicyCarVendorGroupItem(id);

            //Check Exists
            if (policyCarVendorGroupItem == null)
            {
                ViewData["ActionMethod"] = "PostDelete";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyCarVendorGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Delete Item
            try
            {
                policyCarVendorGroupItem.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                policyCarVendorGroupItemRepository.Delete(policyCarVendorGroupItem);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/PolicyCarVendorGroupItem.mvc/Delete/" + policyCarVendorGroupItem.PolicyCarVendorGroupItemId;
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            //Return
            return RedirectToAction("List", new { id = policyCarVendorGroupItem.PolicyGroupId });
        }

        // GET: /EditSequence
        public ActionResult EditSequence(int id, int? page)
        {
            //Check Exists
            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(id);
            if (policyGroup == null)
            {
                ViewData["ActionMethod"] = "GetEditSequence";
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
            var result = policyGroupSequenceRepository.GetPolicyCarVendorGroupItemSequences(id, page ?? 1);

            ViewData["Page"] = page ?? 1;
            ViewData["PolicyGroupID"] = id;
            ViewData["PolicyGroupName"] = policyGroupRepository.GetGroup(id).PolicyGroupName;

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
                ViewData["ActionMethod"] = "PostEditSequence";
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

			XmlDocument doc = new XmlDocument();
			XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
			XmlElement root = doc.DocumentElement;
			doc.InsertBefore(xmlDeclaration, root);
			XmlElement rootElement = doc.CreateElement(string.Empty, "SequenceXML", string.Empty);
			doc.AppendChild(rootElement);
			
			foreach (string s in sequences)
			{
				string[] policyCarVendorGroupItemIdPK = s.Split(new char[] { '_' });

				int policyCarVendorGroupItemId = Convert.ToInt32(policyCarVendorGroupItemIdPK[0]);
				int versionNumber = Convert.ToInt32(policyCarVendorGroupItemIdPK[1]); 
				
				XmlElement itemElement = doc.CreateElement(string.Empty, "Item", string.Empty);

				XmlElement sequenceElement = doc.CreateElement(string.Empty, "Sequence", string.Empty);
				XmlCDataSection sequenceText = doc.CreateCDataSection(HttpUtility.HtmlEncode(sequence.ToString()));
				sequenceElement.AppendChild(sequenceText);
				itemElement.AppendChild(sequenceElement);

				XmlElement policyCarVendorGroupItemIdElement = doc.CreateElement(string.Empty, "PolicyCarVendorGroupItemId", string.Empty);
				XmlCDataSection policyCarVendorGroupItemIdText = doc.CreateCDataSection(HttpUtility.HtmlEncode(policyCarVendorGroupItemId.ToString()));
				policyCarVendorGroupItemIdElement.AppendChild(policyCarVendorGroupItemIdText); 
				itemElement.AppendChild(policyCarVendorGroupItemIdElement);

				XmlElement versionNumberElement = doc.CreateElement(string.Empty, "VersionNumber", string.Empty);
				XmlCDataSection versionNumberText = doc.CreateCDataSection(HttpUtility.HtmlEncode(versionNumber.ToString()));
				versionNumberElement.AppendChild(versionNumberText); 
				itemElement.AppendChild(versionNumberElement);

				rootElement.AppendChild(itemElement);

				sequence = sequence + 1;
			}

            try
            {
                PolicyGroupSequenceRepository policyGroupSequenceRepository = new PolicyGroupSequenceRepository();
				policyGroupSequenceRepository.UpdatePolicyCarVendorGroupItemSequences(doc.OuterXml);
            }
            catch(SqlException ex)
            {
            
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/PolicyCarVendorGroupItem.mvc/EditSequence/" + id + "?page=" + page;
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");

            }

            //Return
            return RedirectToAction("List", new { id = id });
        }
    }
}
