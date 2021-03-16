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
using System.Text;
using System.IO;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace CWTDesktopDatabase.Controllers
{
    public class PolicyAirVendorGroupItemController : Controller
    {
        //main repositories
        PolicyAirVendorGroupItemRepository policyAirVendorGroupItemRepository = new PolicyAirVendorGroupItemRepository();
        PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();
        PolicyRoutingRepository policyRoutingRepository = new PolicyRoutingRepository();

        //Preferred AirStatus, Ranking is null unless Preferred
        private int PreferredAirStatusId = 1;

        //GET: A list of PolicyGroupAirVendor Items for this PolicyGroup
        public ActionResult List(int id, string filter, int? page, string sortField, int? sortOrder)
        {
            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(id); ;

            //Check Exists
            if (policyGroup == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }

            //SortField + SortOrder settings
            if (string.IsNullOrEmpty(sortField))
            {
                sortField = "AirVendorRanking";
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
            PolicyAirVendorGroupItemsVM policyAirVendorGroupItemsVM = new PolicyAirVendorGroupItemsVM();
            policyAirVendorGroupItemsVM.PolicyAirVendorGroupItems = policyAirVendorGroupItemRepository.GetPolicyAirVendorGroupItems(id, filter ?? "", sortField, sortOrder ?? 0, page ?? 1);
            policyAirVendorGroupItemsVM.PolicyGroup = policyGroup;

            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToPolicyGroup(id))
            {
                policyAirVendorGroupItemsVM.HasWriteAccess = true;
            }

            return View(policyAirVendorGroupItemsVM);
        }

        // GET: /Create
        public ActionResult Create(int id)
        {

            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(id); ;

            //Check Exists
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


            PolicyAirVendorGroupItem policyAirVendorGroupItem = new PolicyAirVendorGroupItem();

            //Populate List of PolicyAirStatuses
            PolicyAirStatusRepository policyAirStatusRepository = new PolicyAirStatusRepository();
            SelectList policyAirStatuses = new SelectList(policyAirStatusRepository.GetAllPolicyAirStatuses().ToList(), "PolicyAirStatusId", "PolicyAirStatusDescription");
            ViewData["PolicyAirStatusList"] = policyAirStatuses;

            //Populate List of Products
            ProductRepository productRepository = new ProductRepository();
            SelectList products = new SelectList(productRepository.GetAllProducts().ToList(), "ProductId", "ProductName");
            ViewData["ProductList"] = products;

            //Populate List of AirVendorRankings
            SelectList airVendorRankings = new SelectList(policyAirVendorGroupItemRepository.AirVendorRankings().ToList(), "Value", "Text", policyAirVendorGroupItem.AirVendorRanking);
            ViewData["AirVendorRankings"] = airVendorRankings;

            //populate new PolicyAirVendorGroupItem with known PolicyGroup Information           
            policyAirVendorGroupItem.PolicyGroupId = id;
            policyAirVendorGroupItem.PolicyGroupName = policyGroup.PolicyGroupName;

            PolicyRouting policyRouting = new PolicyRouting();
            policyRouting.FromGlobalFlag = false;
            policyRouting.ToGlobalFlag = false;

            PolicyAirVendorGroupItemVM policyAirVendorGroupItemViewModel = new PolicyAirVendorGroupItemVM(policyGroup, policyAirVendorGroupItem, policyRouting);
            
            //Show 'Create' Form
            return View(policyAirVendorGroupItemViewModel);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PolicyAirVendorGroupItemVM policyAirVendorGroupItemViewModel,string btnSubmit)
        {
            //Get PolicyRouting Info
            PolicyRouting policyRouting = new PolicyRouting();
            policyRouting = policyAirVendorGroupItemViewModel.PolicyRouting;
            
            //Get PolicyAirVendorGroupItem Info
            PolicyAirVendorGroupItem policyAirVendorGroupItem = new PolicyAirVendorGroupItem();
            policyAirVendorGroupItem = policyAirVendorGroupItemViewModel.PolicyAirVendorGroupItem;

            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(policyAirVendorGroupItem.PolicyGroupId);

            //Check Exists
            if (policyGroup == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyAirVendorGroupItem.PolicyGroupId))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Edit Routing
            policyRoutingRepository.EditPolicyRouting(policyRouting);

            //Preferred AirStatus, Ranking is null unless Preferred
            if (policyAirVendorGroupItem.PolicyAirStatusId != PreferredAirStatusId)
            {
                policyAirVendorGroupItem.AirVendorRanking = null;
            }

            //Save To DB
            try
            {
                policyAirVendorGroupItemRepository.Add(policyAirVendorGroupItem, policyRouting);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            //Send to new form based on submit button pressed
            int policyAirVendorGroupItemID = policyAirVendorGroupItem.PolicyAirVendorGroupItemId;
            int PolicyGroupID = policyAirVendorGroupItem.PolicyGroupId;
            switch (btnSubmit)
            {
                case "Save":
                    return RedirectToAction("List", new { id = PolicyGroupID });
                default:
                    return RedirectToAction("CreatePolicyRouting", new { id = PolicyGroupID, policyAirVendorGroupItemId = policyAirVendorGroupItemID });
            }

        }

        // GET: /CreatePolicyRouting
        public ActionResult CreatePolicyRouting(int id, int policyAirVendorGroupItemId)
        {

            //Get PolicyAirVendorGroupItem
            PolicyAirVendorGroupItem policyAirVendorGroupItem = new PolicyAirVendorGroupItem();
            policyAirVendorGroupItem = policyAirVendorGroupItemRepository.GetPolicyAirVendorGroupItem(policyAirVendorGroupItemId);

            //Check Exists
            if (policyAirVendorGroupItem == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyAirVendorGroupItem.PolicyGroupId))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //populate new PolicyAirVendorGroupItem with known PolicyGroup Information
            policyAirVendorGroupItemRepository.EditItemForDisplay(policyAirVendorGroupItem);

            //Policy Group
            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(policyAirVendorGroupItem.PolicyGroupId);

            PolicyRouting policyRouting = new PolicyRouting();
            PolicyAirVendorGroupItemVM policyAirVendorGroupItemViewModel = new PolicyAirVendorGroupItemVM(policyGroup, policyAirVendorGroupItem, policyRouting);

            //Show 'Create' Form
            return View(policyAirVendorGroupItemViewModel);
        }

        // POST: /CreatePolicyRouting
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePolicyRouting(int policyAirVendorGroupItemId, PolicyRouting policyRouting, string btnSubmit)
        {
            //Get PolicyAirVendorGroupItem (Original)
            PolicyAirVendorGroupItem policyAirVendorGroupItemOriginal = new PolicyAirVendorGroupItem();
            policyAirVendorGroupItemOriginal = policyAirVendorGroupItemRepository.GetPolicyAirVendorGroupItem(policyAirVendorGroupItemId);

            //Check Exists
            if (policyAirVendorGroupItemOriginal == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyAirVendorGroupItemOriginal.PolicyGroupId))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }



            //Update from+to fields from form to correct properties
            policyRoutingRepository.EditPolicyRouting(policyRouting);

            //Copy policyAirVendorGroupItem from original
            PolicyAirVendorGroupItem policyAirVendorGroupItem = new PolicyAirVendorGroupItem();
            policyAirVendorGroupItem.PolicyAirStatusId = policyAirVendorGroupItemOriginal.PolicyAirStatusId;
            policyAirVendorGroupItem.EnabledDate = policyAirVendorGroupItemOriginal.EnabledDate;
            policyAirVendorGroupItem.ExpiryDate = policyAirVendorGroupItemOriginal.ExpiryDate;
            policyAirVendorGroupItem.EnabledFlag = policyAirVendorGroupItemOriginal.EnabledFlag;
            policyAirVendorGroupItem.PolicyGroupId = policyAirVendorGroupItemOriginal.PolicyGroupId;
            policyAirVendorGroupItem.ProductId = policyAirVendorGroupItemOriginal.ProductId;
            policyAirVendorGroupItem.SupplierCode = policyAirVendorGroupItemOriginal.SupplierCode;
            policyAirVendorGroupItem.TravelDateValidFrom = policyAirVendorGroupItemOriginal.TravelDateValidFrom;
            policyAirVendorGroupItem.TravelDateValidTo = policyAirVendorGroupItemOriginal.TravelDateValidTo;
            policyAirVendorGroupItem.AirVendorRanking = policyAirVendorGroupItemOriginal.AirVendorRanking;

            //Save policyAirVendorGroupItem to DB
            try
            {
                policyAirVendorGroupItemRepository.Add(policyAirVendorGroupItem, policyRouting);
            }
            catch (SqlException ex)
            {

                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            //Send to new form based on submit button pressed
            int policyAirVendorGroupItemID = policyAirVendorGroupItem.PolicyAirVendorGroupItemId;
            int PolicyGroupID = policyAirVendorGroupItem.PolicyGroupId;
            switch (btnSubmit)
            {
                case "Save":
                    return RedirectToAction("List", new { id = PolicyGroupID });
                default:
                    return RedirectToAction("CreatePolicyRouting", new { id = PolicyGroupID, policyAirVendorGroupItemId = policyAirVendorGroupItemID });
            }

        }

        // GET: /Edit
        public ActionResult Edit(int id)
        {
            //Get PolicyAirVendorGroupItem
            PolicyAirVendorGroupItem policyAirVendorGroupItem = new PolicyAirVendorGroupItem();
            policyAirVendorGroupItem = policyAirVendorGroupItemRepository.GetPolicyAirVendorGroupItem(id);

            //Check Exists
            if (policyAirVendorGroupItem == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyAirVendorGroupItem.PolicyGroupId))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
             
            //Populate List of PolicyAirStatuses
            PolicyAirStatusRepository policyAirStatusRepository = new PolicyAirStatusRepository();
            SelectList policyAirStatuses = new SelectList(policyAirStatusRepository.GetAllPolicyAirStatuses().ToList(), "PolicyAirStatusId", "PolicyAirStatusDescription");
            ViewData["PolicyAirStatusList"] = policyAirStatuses;

            //Populate List of Products
            ProductRepository productRepository = new ProductRepository();
            SelectList products = new SelectList(productRepository.GetAllProducts().ToList(), "ProductId", "ProductName", policyAirVendorGroupItem.ProductId);
            ViewData["ProductList"] = products;

            //Populate List of Products
            SelectList airVendorRankings = new SelectList(policyAirVendorGroupItemRepository.AirVendorRankings().ToList(), "Value", "Text", policyAirVendorGroupItem.AirVendorRanking);
            ViewData["AirVendorRankings"] = airVendorRankings;

            //populate new PolicyAirVendorGroupItem with known PolicyGroup Information
            policyAirVendorGroupItemRepository.EditItemForDisplay(policyAirVendorGroupItem);

            //Policy Group
            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(policyAirVendorGroupItem.PolicyGroupId);

            //Add the PolicyRouting information
            PolicyRouting policyRouting = policyRoutingRepository.GetPolicyRouting(policyAirVendorGroupItem.PolicyRoutingId);
            PolicyAirVendorGroupItemVM policyAirVendorGroupItemViewModel = new PolicyAirVendorGroupItemVM(policyGroup, policyAirVendorGroupItem, policyRouting);

            //Show 'Edit' Form
            return View(policyAirVendorGroupItemViewModel);
        }

        // POST /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, PolicyAirVendorGroupItemVM policyAirVendorGroupItemViewModel, FormCollection collection)
        {
            PolicyAirVendorGroupItem policyAirVendorGroupItem = new PolicyAirVendorGroupItem();
            policyAirVendorGroupItem = policyAirVendorGroupItemRepository.GetPolicyAirVendorGroupItem(id);

            PolicyRouting policyRouting = new PolicyRouting();
            policyRouting = policyAirVendorGroupItemViewModel.PolicyRouting;

            //Check Exists
            if (policyAirVendorGroupItem == null || policyRouting == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyAirVendorGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Preferred AirStatus, Ranking is null unless Preferred
            if (policyAirVendorGroupItem.AirVendorRanking != PreferredAirStatusId)
            {
                policyAirVendorGroupItem.AirVendorRanking = null;
            }

            //Update Item from Form
            try
            {
                UpdateModel(policyAirVendorGroupItem, "PolicyAirVendorGroupItem");
                UpdateModel(policyRouting, "PolicyRouting");
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

            //Edit Routing
            policyRoutingRepository.EditPolicyRouting(policyRouting);

            //Database Update
            try
            {
                policyAirVendorGroupItemRepository.Update(policyAirVendorGroupItem, policyRouting);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/PolicyAirVendorGroupItem.mvc/Edit/" + policyAirVendorGroupItem.PolicyAirVendorGroupItemId;
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }



            //Return to Form
            return RedirectToAction("List", new { id = policyAirVendorGroupItem.PolicyGroupId });
        }

        // GET: /View
        public ActionResult View(int id)
        {
            //Get PolicyAirVendorGroupItem
            PolicyAirVendorGroupItem policyAirVendorGroupItem = new PolicyAirVendorGroupItem();
            policyAirVendorGroupItem = policyAirVendorGroupItemRepository.GetPolicyAirVendorGroupItem(id);

            //Check Exists
            if (policyAirVendorGroupItem == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }


            //populate new PolicyAirVendorGroupItem with known PolicyGroup Information
            policyAirVendorGroupItemRepository.EditItemForDisplay(policyAirVendorGroupItem);

            //Policy Group
            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(policyAirVendorGroupItem.PolicyGroupId);

            //add the PolicyRouting information
            PolicyRouting policyRouting = policyRoutingRepository.GetPolicyRouting(policyAirVendorGroupItem.PolicyRoutingId);
            policyRoutingRepository.EditForDisplay(policyRouting);
            PolicyAirVendorGroupItemVM policyAirVendorGroupItemViewModel = new PolicyAirVendorGroupItemVM(policyGroup, policyAirVendorGroupItem, policyRouting);

            //Show 'Create' Form
            return View(policyAirVendorGroupItemViewModel);
        }

        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id)
        {
            //Get PolicyAirVendorGroupItem
            PolicyAirVendorGroupItem policyAirVendorGroupItem = new PolicyAirVendorGroupItem();
            policyAirVendorGroupItem = policyAirVendorGroupItemRepository.GetPolicyAirVendorGroupItem(id);

            //Check Exists
            if (policyAirVendorGroupItem == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyAirVendorGroupItem.PolicyGroupId))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
          
            //populate new PolicyAirVendorGroupItem with known PolicyGroup Information
            policyAirVendorGroupItemRepository.EditItemForDisplay(policyAirVendorGroupItem);

            //Policy Group
            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(policyAirVendorGroupItem.PolicyGroupId);

            //add the PolicyRouting information
            PolicyRouting policyRouting = policyRoutingRepository.GetPolicyRouting(policyAirVendorGroupItem.PolicyRoutingId);
            policyRoutingRepository.EditForDisplay(policyRouting);
            PolicyAirVendorGroupItemVM policyAirVendorGroupItemViewModel = new PolicyAirVendorGroupItemVM(policyGroup, policyAirVendorGroupItem, policyRouting);           

            //Show 'Create' Form
            return View(policyAirVendorGroupItemViewModel);
        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            //Get PolicyAirVendorGroupItem
            PolicyAirVendorGroupItem policyAirVendorGroupItem = new PolicyAirVendorGroupItem();
            policyAirVendorGroupItem = policyAirVendorGroupItemRepository.GetPolicyAirVendorGroupItem(id);

            //Check Exists
            if (policyAirVendorGroupItem == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyAirVendorGroupItem.PolicyGroupId))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Delete
            //Delete Item
            try
            {
                policyAirVendorGroupItem.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                policyAirVendorGroupItemRepository.Delete(policyAirVendorGroupItem);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/PolicyAirVendorGroupItem.mvc/Delete/" + policyAirVendorGroupItem.PolicyAirVendorGroupItemId;
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }
            //Return
            return RedirectToAction("List", new { id = policyAirVendorGroupItem.PolicyGroupId});
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
            var result = policyGroupSequenceRepository.GetPolicyAirVendorGroupItemSequences(id, page ?? 1);

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
                string[] policyAirVendorGroupItemPK = s.Split(new char[] { '_' });

                int policyAirVendorGroupItemId = Convert.ToInt32(policyAirVendorGroupItemPK[0]);
                int versionNumber = Convert.ToInt32(policyAirVendorGroupItemPK[1]);

                XmlElement xmlItem = doc.CreateElement("Item");
                root.AppendChild(xmlItem);

                XmlElement xmlSequence = doc.CreateElement("Sequence");
                xmlSequence.InnerText = sequence.ToString();
                xmlItem.AppendChild(xmlSequence);

                XmlElement xmlPolicyAirVendorGroupItemId = doc.CreateElement("PolicyAirVendorGroupItemId");
                xmlPolicyAirVendorGroupItemId.InnerText = policyAirVendorGroupItemId.ToString();
                xmlItem.AppendChild(xmlPolicyAirVendorGroupItemId);

                XmlElement xmlVersionNumber = doc.CreateElement("VersionNumber");
                xmlVersionNumber.InnerText = versionNumber.ToString();
                xmlItem.AppendChild(xmlVersionNumber);

                sequence = sequence + 1;
            }

            try
            {
                PolicyGroupSequenceRepository policyGroupSequenceRepository = new PolicyGroupSequenceRepository();
                policyGroupSequenceRepository.UpdatePolicyAirVendorGroupItemSequences(System.Xml.Linq.XElement.Parse(doc.OuterXml));
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/PolicyAirVendorGroupItem.mvc/EditSequence/" + id + "?page=" + page;
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List", new { id = id });
        }

		// GET: /Export
		public ActionResult Export(int id)
		{
			//Check Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(id);
			if (policyGroup == null)
			{
				ViewData["ActionMethod"] = "ExportGet";
				return View("RecordDoesNotExistError");
			}

			//Get CSV Data
			byte[] csvData = policyAirVendorGroupItemRepository.Export(id);
			return File(csvData, "text/csv", "Policy Air Vendor Group Items Export.csv");
		}

        // GET: /ExportErrors
        public ActionResult ExportErrors()
        {
            var preImportCheckResultVM = (PolicyAirVendorImportStep1VM)TempData["ErrorMessages"];

            if (preImportCheckResultVM == null)
            {
                ViewData["ActionMethod"] = "ExportGet";
                return View("RecordDoesNotExistError");
            }

            var errors = preImportCheckResultVM.ImportStep2VM.ReturnMessages;

            //Get CSV Data
            var combinedErrors = errors.Aggregate((current, next) => current + "\r\n" + next);
            byte[] csvData = Encoding.ASCII.GetBytes(combinedErrors);
            return File(csvData, "text/plain", "PolicyAirVendorGroupItemValidationSummary.txt");
        }

        public ActionResult ImportStep1(int id)
        {

            PolicyAirVendorImportStep1WithFileVM cdrLinkImportFileVM = new PolicyAirVendorImportStep1WithFileVM();
            cdrLinkImportFileVM.PolicyGroupId = id;

            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(id);

            cdrLinkImportFileVM.PolicyGroup = policyGroup;

            return View(cdrLinkImportFileVM);
        }

        [HttpPost]
        public ActionResult ImportStep1(PolicyAirVendorImportStep1WithFileVM csvfile)
        {
            //used for return only
            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(csvfile.PolicyGroupId);
            csvfile.PolicyGroup = policyGroup;


            if (!ModelState.IsValid)
            {

                return View(csvfile);
            }
            string fileExtension = Path.GetExtension(csvfile.File.FileName);
            if (fileExtension != ".csv") // && fileExtension != ".xls" && fileExtension != ".xlsx")
            {
                ModelState.AddModelError("file", "This is not a valid entry");
				return View(csvfile);
            }

            if (csvfile.File.ContentLength > 0)
            {
                PolicyAirVendorImportStep2VM preImportCheckResult = new PolicyAirVendorImportStep2VM();
                List<string> returnMessages = new List<string>();

                preImportCheckResult = policyAirVendorGroupItemRepository.PreImportCheck(csvfile.File, csvfile.PolicyGroupId);

                PolicyAirVendorImportStep1VM preImportCheckResultVM = new PolicyAirVendorImportStep1VM();
                preImportCheckResultVM.PolicyGroup = policyGroup;
                preImportCheckResultVM.ImportStep2VM = preImportCheckResult;
                preImportCheckResultVM.PolicyGroupId = csvfile.PolicyGroupId;

                TempData["PreImportCheckResultVM"] = preImportCheckResultVM;
                return RedirectToAction("ImportStep2");
            }

            return View();
        }

        public ActionResult ImportStep2()
        {
            PolicyAirVendorImportStep1VM preImportCheckResultVM = new PolicyAirVendorImportStep1VM();
            preImportCheckResultVM = (PolicyAirVendorImportStep1VM)TempData["PreImportCheckResultVM"];

            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(preImportCheckResultVM.PolicyGroupId);
            preImportCheckResultVM.PolicyGroup = policyGroup;

            return View(preImportCheckResultVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ImportStep2(PolicyAirVendorImportStep1VM preImportCheckResultVM)
        {
            if (preImportCheckResultVM.ImportStep2VM.IsValidData == false)
            {
                //Check JSON for valid messages
                if (preImportCheckResultVM.ImportStep2VM.ReturnMessages[0] != null)
                {
                    List<string> returnMessages = new List<string>();

                    var settings = new JsonSerializerSettings
                    {
                        StringEscapeHandling = StringEscapeHandling.EscapeHtml,
                    };

                    List<string> returnMessagesJSON = JsonConvert.DeserializeObject<List<string>>(preImportCheckResultVM.ImportStep2VM.ReturnMessages[0], settings);

                    foreach (string message in returnMessagesJSON)
                    {
                        string validMessage = Regex.Replace(message, @"[^À-ÿ\w\s&:._()\-]", "");

                        if (!string.IsNullOrEmpty(validMessage))
                        {
                            returnMessages.Add(validMessage);
                        }
                    }

                    preImportCheckResultVM.ImportStep2VM.ReturnMessages = returnMessages;
                }
                
                TempData["ErrorMessages"] = preImportCheckResultVM;
                return RedirectToAction("ExportErrors");
            }

            //PreImport Check Results (check has passed)
            PolicyAirVendorImportStep2VM preImportCheckResult = new PolicyAirVendorImportStep2VM();
            preImportCheckResult = preImportCheckResultVM.ImportStep2VM;

            //Do the Import, return results
            PolicyAirVendorImportStep3VM cdrPostImportResult = new PolicyAirVendorImportStep3VM();
            cdrPostImportResult = policyAirVendorGroupItemRepository.Import(
                preImportCheckResult.FileBytes,
                preImportCheckResultVM.PolicyGroupId
            );

            cdrPostImportResult.PolicyGroupId = preImportCheckResultVM.PolicyGroupId;
            TempData["CdrPostImportResult"] = cdrPostImportResult;

            //Pass Results to Next Page
            return RedirectToAction("ImportStep3");

        }

        public ActionResult ImportStep3()
        {
            //Display Results of Import
            PolicyAirVendorImportStep3VM cdrPostImportResult = new PolicyAirVendorImportStep3VM();
            cdrPostImportResult = (PolicyAirVendorImportStep3VM)TempData["CdrPostImportResult"];

            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(cdrPostImportResult.PolicyGroupId);
            cdrPostImportResult.PolicyGroup = policyGroup;

            return View(cdrPostImportResult);
        }
    }
}
