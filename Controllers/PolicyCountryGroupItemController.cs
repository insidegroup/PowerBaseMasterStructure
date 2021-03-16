using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Repository;
using System.Data.SqlClient;
using System.Xml;
using CWTDesktopDatabase.ViewModels;
using System.IO;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Text;

namespace CWTDesktopDatabase.Controllers
{
    public class PolicyCountryGroupItemController : Controller
    {
        //main repositories
        PolicyCountryGroupItemRepository policyCountryGroupItemRepository = new PolicyCountryGroupItemRepository();
        PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();

        //GET: A list of Items for this PolicyGroup
        public ActionResult List(int id, string filter, int? page, string sortField, int? sortOrder)
        {
            //Check Parent Exists
            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(id);
            if (policyGroup == null)
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
            ViewData["PolicyGroupName"] = policyGroup.PolicyGroupName;


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

            var items = policyCountryGroupItemRepository.GetPolicyCountryGroupItems(id, filter ?? "", sortField, sortOrder ?? 0, page ?? 1);
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
            //Populate List of PolicyCountryStatuses
            PolicyCountryStatusRepository policyCountryStatusRepository = new PolicyCountryStatusRepository();
            SelectList policyCountryStatuses = new SelectList(policyCountryStatusRepository.GetAllPolicyCountryStatuses().ToList(), "PolicyCountryStatusId", "PolicyCountryStatusDescription");
            ViewData["PolicyCountryStatusList"] = policyCountryStatuses;

            //populateItem with known PolicyGroup Information           
            PolicyCountryGroupItem policyCountryGroupItem = new PolicyCountryGroupItem();
            policyCountryGroupItem.PolicyGroupId = id;
            policyCountryGroupItem.PolicyGroupName = policyGroup.PolicyGroupName;

            //Show 'Create' Form
            return View(policyCountryGroupItem);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PolicyCountryGroupItem policyCountryGroupItem)
        {
            PolicyGroup policyGroup = policyGroupRepository.GetGroup(policyCountryGroupItem.PolicyGroupId);
            if (policyGroup == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }
            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyCountryGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update Model from Form
            try
           {
               UpdateModel(policyCountryGroupItem);
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
                policyCountryGroupItemRepository.Add(policyCountryGroupItem);
            }
            catch
            {
                //Could not insert to database
                return View("Error");
            }

            return RedirectToAction("List", new { id = policyCountryGroupItem.PolicyGroupId });
        }

        // GET: /Edit
        public ActionResult Edit(int id)
        {
            //Get PolicyCountryGroupItem
            PolicyCountryGroupItem policyCountryGroupItem = new PolicyCountryGroupItem();
            policyCountryGroupItem = policyCountryGroupItemRepository.GetPolicyCountryGroupItem(id);

            //Check Exists
            if (policyCountryGroupItem == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyCountryGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Populate List of PolicyCountryStatuses
            PolicyCountryStatusRepository policyCountryStatusRepository = new PolicyCountryStatusRepository();
            SelectList policyCountryStatuses = new SelectList(policyCountryStatusRepository.GetAllPolicyCountryStatuses().ToList(), "PolicyCountryStatusId", "PolicyCountryStatusDescription");
            ViewData["PolicyCountryStatusList"] = policyCountryStatuses;


            //Show 'Edit' Form
            policyCountryGroupItemRepository.EditItemForDisplay(policyCountryGroupItem);
            return View(policyCountryGroupItem);
        }

        // POST /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection collection)
        {
            PolicyCountryGroupItem policyCountryGroupItem = new PolicyCountryGroupItem();
            policyCountryGroupItem = policyCountryGroupItemRepository.GetPolicyCountryGroupItem(id);

            //Check Exists
            if (policyCountryGroupItem == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyCountryGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update PolicyCountryGroupItem Model From Form
            try
            {
                UpdateModel(policyCountryGroupItem);
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
                policyCountryGroupItemRepository.Update(policyCountryGroupItem);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/PolicyCountryGroupItem.mvc/Edit/" + policyCountryGroupItem.PolicyCountryGroupItemId;
                    return View("VersionError");
                }

                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");            }


            //Return to Form
            return RedirectToAction("List", new { id = policyCountryGroupItem.PolicyGroupId });

        }

        // GET: /View
        public ActionResult View(int id)
        {
            //Get PolicyCountryGroupItem
            PolicyCountryGroupItem policyCountryGroupItem = new PolicyCountryGroupItem();
            policyCountryGroupItem = policyCountryGroupItemRepository.GetPolicyCountryGroupItem(id);

            //Check Exists
            if (policyCountryGroupItem == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            //populate new PolicyCountryGroupItem with known PolicyGroup Information
            policyCountryGroupItemRepository.EditItemForDisplay(policyCountryGroupItem);

            //Show 'View' Form
            return View(policyCountryGroupItem);
        }


        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id)
        {
            //Get PolicyCountryGroupItem
            PolicyCountryGroupItem policyCountryGroupItem = new PolicyCountryGroupItem();
            policyCountryGroupItem = policyCountryGroupItemRepository.GetPolicyCountryGroupItem(id);

            //Check Exists
            if (policyCountryGroupItem == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyCountryGroupItem.PolicyGroupId))
            {
                return View("Error");
            }

            //populate new PolicyHotelVendorGroupItem with known PolicyGroup Information
            policyCountryGroupItemRepository.EditItemForDisplay(policyCountryGroupItem);

            //Show 'Delete' Form
            return View(policyCountryGroupItem);
        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            //Get PolicyCountryGroupItem
            PolicyCountryGroupItem policyCountryGroupItem = new PolicyCountryGroupItem();
            policyCountryGroupItem = policyCountryGroupItemRepository.GetPolicyCountryGroupItem(id);

            //Check Exists
            if (policyCountryGroupItem == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyCountryGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Delete Item
            try
            {
                policyCountryGroupItem.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                policyCountryGroupItemRepository.Delete(policyCountryGroupItem);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/PolicyCountryGroupItem.mvc/Delete/" + policyCountryGroupItem.PolicyCountryGroupItemId;
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            //Return
            return RedirectToAction("List", new { id = policyCountryGroupItem.PolicyGroupId });
        }

        // GET: /EditSequence
        public ActionResult EditSequence(int id, int? page)
        {
            //Check Exists
            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(id);
            if (policyGroup == null)
            {
                return View("Error");
            }

            //Check Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(id))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
            PolicyGroupSequenceRepository policyGroupSequenceRepository = new PolicyGroupSequenceRepository();
            var result = policyGroupSequenceRepository.GetPolicyCountryGroupItemSequences(id, page ?? 1);

            ViewData["Page"] = page ?? 1;
            ViewData["PolicyGroupId"] = id;
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
                return View("Error");
            }

            //Check Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(id))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
            string[] sequences = collection["PolicySequence"].Split(new char[] { ',' });

            int sequence = ((page - 1) * 50) - 2;
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
				string[] policyCountryGroupItemPK = s.Split(new char[] { '_' });

				int policyCountryGroupItemId = Convert.ToInt32(policyCountryGroupItemPK[0]);
				int versionNumber = Convert.ToInt32(policyCountryGroupItemPK[1]);

				XmlElement itemElement = doc.CreateElement(string.Empty, "Item", string.Empty);

				XmlElement sequenceElement = doc.CreateElement(string.Empty, "Sequence", string.Empty);
				XmlCDataSection sequenceText = doc.CreateCDataSection(HttpUtility.HtmlEncode(sequence.ToString()));
				sequenceElement.AppendChild(sequenceText);
				itemElement.AppendChild(sequenceElement);

				XmlElement policyCountryGroupItemIdElement = doc.CreateElement(string.Empty, "PolicyCountryGroupItemId", string.Empty);
				XmlCDataSection policyCountryGroupItemIdText = doc.CreateCDataSection(HttpUtility.HtmlEncode(policyCountryGroupItemId.ToString()));
				policyCountryGroupItemIdElement.AppendChild(policyCountryGroupItemIdText);
				itemElement.AppendChild(policyCountryGroupItemIdElement);

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
                policyGroupSequenceRepository.UpdatePolicyCountryGroupItemSequences(doc.OuterXml);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/PolicyCountryGroupItem.mvc/EditSequence/" + id + "?page=" + page;
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
			byte[] csvData = policyCountryGroupItemRepository.Export(id);
			return File(csvData, "text/csv", "Policy Country Group Items Export.csv");
		}

        // GET: /ExportErrors
        public ActionResult ExportErrors()
        {
            var preImportCheckResultVM = (PolicyCountryImportStep1VM)TempData["ErrorMessages"];

            if (preImportCheckResultVM == null)
            {
                ViewData["ActionMethod"] = "ExportGet";
                return View("RecordDoesNotExistError");
            }

            var errors = preImportCheckResultVM.ImportStep2VM.ReturnMessages;

            //Get CSV Data
            var combinedErrors = errors.Aggregate((current, next) => current + "\r\n" + next);
            byte[] csvData = Encoding.ASCII.GetBytes(combinedErrors);
            return File(csvData, "text/plain", "PolicyCountryGroupItemValidationSummary.txt");
        }
        public ActionResult ImportStep1(int id)
        {

            PolicyCountryImportStep1WithFileVM cdrLinkImportFileVM = new PolicyCountryImportStep1WithFileVM();
            cdrLinkImportFileVM.PolicyGroupId = id;

            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(id);

            cdrLinkImportFileVM.PolicyGroup = policyGroup;

            return View(cdrLinkImportFileVM);
        }
        
        [HttpPost]
        public ActionResult ImportStep1(PolicyCountryImportStep1WithFileVM csvfile)
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
                PolicyCountryImportStep2VM preImportCheckResult = new PolicyCountryImportStep2VM();
                List<string> returnMessages = new List<string>();

                preImportCheckResult = policyCountryGroupItemRepository.PreImportCheck(csvfile.File, csvfile.PolicyGroupId);

                PolicyCountryImportStep1VM preImportCheckResultVM = new PolicyCountryImportStep1VM();
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
            PolicyCountryImportStep1VM preImportCheckResultVM = new PolicyCountryImportStep1VM();
            preImportCheckResultVM = (PolicyCountryImportStep1VM)TempData["PreImportCheckResultVM"];

            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(preImportCheckResultVM.PolicyGroupId);
            preImportCheckResultVM.PolicyGroup = policyGroup;

            return View(preImportCheckResultVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ImportStep2(PolicyCountryImportStep1VM preImportCheckResultVM)
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
            PolicyCountryImportStep2VM preImportCheckResult = new PolicyCountryImportStep2VM();
            preImportCheckResult = preImportCheckResultVM.ImportStep2VM;

            //Do the Import, return results
            PolicyCountryImportStep3VM cdrPostImportResult = new PolicyCountryImportStep3VM();
            cdrPostImportResult = policyCountryGroupItemRepository.Import(
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
            PolicyCountryImportStep3VM cdrPostImportResult = new PolicyCountryImportStep3VM();
            cdrPostImportResult = (PolicyCountryImportStep3VM)TempData["CdrPostImportResult"];

            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(cdrPostImportResult.PolicyGroupId);
            cdrPostImportResult.PolicyGroup = policyGroup;

            return View(cdrPostImportResult);
        }

    }
}
