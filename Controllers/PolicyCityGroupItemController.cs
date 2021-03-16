using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Repository;
using System.Data.SqlClient;
using System.Xml;
using System.IO;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Text;

namespace CWTDesktopDatabase.Controllers
{
    public class PolicyCityGroupItemController : Controller
    {
        //main repositories
        PolicyCityGroupItemRepository policyCityGroupItemRepository = new PolicyCityGroupItemRepository();
        PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();

        //GET: A list of PolicyCityGroupItems for this PolicyGroup
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

            PolicyCityGroupItemsVM policyCityGroupItemsVM = new PolicyCityGroupItemsVM();
            policyCityGroupItemsVM.PolicyCityGroupItems = policyCityGroupItemRepository.GetPolicyCityGroupItems(id,filter ?? "", sortField, sortOrder ?? 0, page ?? 1);
            policyCityGroupItemsVM.PolicyGroup = policyGroup;

            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToPolicyGroup(id))
            {
                policyCityGroupItemsVM.HasWriteAccess = true;
            }

            return View(policyCityGroupItemsVM);
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

            //Create ViewModel
            PolicyCityGroupItemVM policyCityGroupItemVM = new PolicyCityGroupItemVM();
            policyCityGroupItemVM.PolicyGroup = policyGroup;

            PolicyCityGroupItem policyCityGroupItem = new PolicyCityGroupItem();
            policyCityGroupItem.PolicyGroupId = policyGroup.PolicyGroupId;
            policyCityGroupItemVM.PolicyCityGroupItem = policyCityGroupItem;

            //Statuses
            PolicyCityStatusRepository policyCityStatusRepository = new PolicyCityStatusRepository();
            policyCityGroupItemVM.PolicyCityStatuses = new SelectList(policyCityStatusRepository.GetAllPolicyCityStatuses().ToList(), "PolicyCityStatusId", "PolicyCityStatusDescription");

            //Return Form to Users
            return View(policyCityGroupItemVM);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PolicyCityGroupItemVM policyCityGroupItemVM)
        {

            //Get PolicyCityGroupItem Info
            PolicyCityGroupItem policyCityGroupItem = new PolicyCityGroupItem();
            policyCityGroupItem = policyCityGroupItemVM.PolicyCityGroupItem;

            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(policyCityGroupItem.PolicyGroupId);

            //Check Exists
            if (policyGroup == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyGroup.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update Model From Form + Validate against DB
            try
            {
                UpdateModel<PolicyCityGroupItem>(policyCityGroupItemVM.PolicyCityGroupItem, "PolicyCityGroupItem");
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

            //Save To DB
            try
            {
                policyCityGroupItemRepository.Add(policyCityGroupItem);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List", new { id = policyGroup.PolicyGroupId });

        }

        // GET: /Edit
        public ActionResult Edit(int id)
        {
            PolicyCityGroupItem policyCityGroupItem = new PolicyCityGroupItem();
            policyCityGroupItem = policyCityGroupItemRepository.GetPolicyCityGroupItem(id);

            //Check Exists
            if (policyCityGroupItem == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyCityGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Create ViewModel
            PolicyCityGroupItemVM policyCityGroupItemVM = new PolicyCityGroupItemVM();

            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(policyCityGroupItem.PolicyGroupId);
            policyCityGroupItemVM.PolicyGroup = policyGroup;

            policyCityGroupItemVM.PolicyCityGroupItem = policyCityGroupItem;

            //Statuses
            PolicyCityStatusRepository policyCityStatusRepository = new PolicyCityStatusRepository();
            policyCityGroupItemVM.PolicyCityStatuses = new SelectList(policyCityStatusRepository.GetAllPolicyCityStatuses().ToList(), "PolicyCityStatusId", "PolicyCityStatusDescription", policyCityGroupItem.PolicyCityStatusId);

            //Return Form to Users
            return View(policyCityGroupItemVM);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PolicyCityGroupItemVM policyCityGroupItemVM)
        {

            //Get PolicyCityGroupItem
            PolicyCityGroupItem policyCityGroupItem = new PolicyCityGroupItem();
            policyCityGroupItem = policyCityGroupItemRepository.GetPolicyCityGroupItem(policyCityGroupItemVM.PolicyCityGroupItem.PolicyCityGroupItemId);


            //Check Exists
            if (policyCityGroupItem == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyCityGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update Model From Form + Validate against DB
            try
            {
                UpdateModel<PolicyCityGroupItem>(policyCityGroupItemVM.PolicyCityGroupItem, "PolicyCityGroupItem");
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

            //Save To DB
            try
            {
                policyCityGroupItemRepository.Update(policyCityGroupItemVM.PolicyCityGroupItem);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List", new { id = policyCityGroupItemVM.PolicyCityGroupItem.PolicyGroupId });

        }

        // GET: /View
        public ActionResult View(int id)
        {
            //Get PolicyCarTypeGroupItem
            PolicyCityGroupItem policyCityGroupItem = new PolicyCityGroupItem();
            policyCityGroupItem = policyCityGroupItemRepository.GetPolicyCityGroupItem(id);

            //Check Exists
            if (policyCityGroupItem == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            //Create ViewModel
            PolicyCityGroupItemVM policyCityGroupItemVM = new PolicyCityGroupItemVM();

            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(policyCityGroupItem.PolicyGroupId);
            policyCityGroupItemVM.PolicyGroup = policyGroup;

            policyCityGroupItemVM.PolicyCityGroupItem = policyCityGroupItem;

            //Return Form to Users
            return View(policyCityGroupItemVM);
        }

        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id)
        {
            //Get PolicyCarTypeGroupItem
            PolicyCityGroupItem policyCityGroupItem = new PolicyCityGroupItem();
            policyCityGroupItem = policyCityGroupItemRepository.GetPolicyCityGroupItem(id);

            //Check Exists
            if (policyCityGroupItem == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyCityGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Create ViewModel
            PolicyCityGroupItemVM policyCityGroupItemVM = new PolicyCityGroupItemVM();

            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(policyCityGroupItem.PolicyGroupId);
            policyCityGroupItemVM.PolicyGroup = policyGroup;

            policyCityGroupItemVM.PolicyCityGroupItem = policyCityGroupItem;

            //Return Form to Users
            return View(policyCityGroupItemVM);
        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(PolicyCityGroupItemVM policyCityGroupItemVM)
        {
            //Get PolicyCityGroupItem
            PolicyCityGroupItem policyCityGroupItem = new PolicyCityGroupItem();
            policyCityGroupItem = policyCityGroupItemRepository.GetPolicyCityGroupItem(policyCityGroupItemVM.PolicyCityGroupItem.PolicyCityGroupItemId);

            //Check Exists
            if (policyCityGroupItem == null)
            {
                ViewData["ActionMethod"] = "PostDelete";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyCityGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
            //Delete Item
            try
            {
                policyCityGroupItemRepository.Delete(policyCityGroupItemVM.PolicyCityGroupItem);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/PolicyCityGroupItem.mvc/Delete/" + policyCityGroupItem.PolicyCityGroupItemId.ToString();
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            //Return
            return RedirectToAction("List", new { id = policyCityGroupItem.PolicyGroupId });
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
            var result = policyGroupSequenceRepository.GetPolicyCityGroupItemSequences(id, page ?? 1);

            ViewData["Page"] = page ?? 1;
            ViewData["PolicyGroupId"] = id;
            ViewData["PolicyGroupName"] = policyGroup.PolicyGroupName;

            return View(result);
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
            byte[] csvData = policyCityGroupItemRepository.Export(id);
            return File(csvData, "text/csv", "Policy City Group Items Export.csv");
        }

        // GET: /ExportErrors
        public ActionResult ExportErrors()
        {
            var preImportCheckResultVM = (PolicyCityImportStep1VM)TempData["ErrorMessages"];

            if (preImportCheckResultVM == null)
            {
                ViewData["ActionMethod"] = "ExportGet";
                return View("RecordDoesNotExistError");
            }

            var errors = preImportCheckResultVM.ImportStep2VM.ReturnMessages;

            //Get CSV Data
            var combinedErrors = errors.Aggregate((current, next) => current + "\r\n" + next);
            byte[] csvData = Encoding.ASCII.GetBytes(combinedErrors);
            return File(csvData, "text/plain", "PolicyCityGroupItemValidationSummary.txt");
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

			XmlDocument doc = new XmlDocument();
			XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
			XmlElement root = doc.DocumentElement;
			doc.InsertBefore(xmlDeclaration, root);
			XmlElement rootElement = doc.CreateElement(string.Empty, "SequenceXML", string.Empty);
			doc.AppendChild(rootElement);

			foreach (string s in sequences)
			{
				string[] policyCityGroupItemPK = s.Split(new char[] { '_' });

				int policyCityGroupItemId = Convert.ToInt32(policyCityGroupItemPK[0]);
				int versionNumber = Convert.ToInt32(policyCityGroupItemPK[1]);

				XmlElement itemElement = doc.CreateElement(string.Empty, "Item", string.Empty);

				XmlElement sequenceElement = doc.CreateElement(string.Empty, "Sequence", string.Empty);
				XmlCDataSection sequenceText = doc.CreateCDataSection(HttpUtility.HtmlEncode(sequence.ToString()));
				sequenceElement.AppendChild(sequenceText);
				itemElement.AppendChild(sequenceElement);

				XmlElement policyCityGroupItemIdElement = doc.CreateElement(string.Empty, "PolicyCityGroupItemId", string.Empty);
				XmlCDataSection policyCityGroupItemIdText = doc.CreateCDataSection(HttpUtility.HtmlEncode(policyCityGroupItemId.ToString()));
				policyCityGroupItemIdElement.AppendChild(policyCityGroupItemIdText);
				itemElement.AppendChild(policyCityGroupItemIdElement);

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
                policyGroupSequenceRepository.UpdatePolicyCityGroupItemSequences(doc.OuterXml);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/PolicyCityGroupItem.mvc/EditSequence/" + id + "?page=" + page;
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");

            }

            return RedirectToAction("List", new { id = id });
        }

        public ActionResult ImportStep1(int id)
        {

            PolicyCityImportStep1WithFileVM cdrLinkImportFileVM = new PolicyCityImportStep1WithFileVM();
            cdrLinkImportFileVM.PolicyGroupId = id;

            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(id);

            cdrLinkImportFileVM.PolicyGroup = policyGroup;

            return View(cdrLinkImportFileVM);
        }

        [HttpPost]
        public ActionResult ImportStep1(PolicyCityImportStep1WithFileVM csvfile)
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
                PolicyCityImportStep2VM preImportCheckResult = new PolicyCityImportStep2VM();
                List<string> returnMessages = new List<string>();

                preImportCheckResult = policyCityGroupItemRepository.PreImportCheck(csvfile.File, csvfile.PolicyGroupId);

                PolicyCityImportStep1VM preImportCheckResultVM = new PolicyCityImportStep1VM();
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
            PolicyCityImportStep1VM preImportCheckResultVM = new PolicyCityImportStep1VM();
            preImportCheckResultVM = (PolicyCityImportStep1VM)TempData["PreImportCheckResultVM"];

            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(preImportCheckResultVM.PolicyGroupId);
            preImportCheckResultVM.PolicyGroup = policyGroup;

            return View(preImportCheckResultVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ImportStep2(PolicyCityImportStep1VM preImportCheckResultVM)
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
            PolicyCityImportStep2VM preImportCheckResult = new PolicyCityImportStep2VM();
            preImportCheckResult = preImportCheckResultVM.ImportStep2VM;

            //Do the Import, return results
            PolicyCityImportStep3VM cdrPostImportResult = new PolicyCityImportStep3VM();
            cdrPostImportResult = policyCityGroupItemRepository.Import(
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
            PolicyCityImportStep3VM cdrPostImportResult = new PolicyCityImportStep3VM();
            cdrPostImportResult = (PolicyCityImportStep3VM)TempData["CdrPostImportResult"];

            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(cdrPostImportResult.PolicyGroupId);
            cdrPostImportResult.PolicyGroup = policyGroup;

            return View(cdrPostImportResult);
        }
    }


}
