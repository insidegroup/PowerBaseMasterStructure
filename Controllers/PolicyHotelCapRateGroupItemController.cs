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
using System.Text;

namespace CWTDesktopDatabase.Controllers
{
    public class PolicyHotelCapRateGroupItemController : Controller
    {
        //main repositories
        PolicyHotelCapRateGroupItemRepository policyHotelCapRateGroupItemRepository = new PolicyHotelCapRateGroupItemRepository();
        PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();

        //GET: A list of PolicyGroupCarType Items for this PolicyGroup
        public ActionResult List(int id, string filter, int? page, string sortField, int? sortOrder)
        {
            PolicyGroup group = new PolicyGroup();
            group = policyGroupRepository.GetGroup(id); ;

            //Check Exists
            if (group == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }

			policyGroupRepository.EditGroupForDisplay(group);

            //Set Access Rights
			RolesRepository rolesRepository = new RolesRepository();

            ViewData["Access"] = "";
            if (rolesRepository.HasWriteAccessToPolicyGroup(id))
            {
                ViewData["Access"] = "WriteAccess";
            }

			//Set Import Access Rights
			ViewData["ImportAccess"] = "";
			if ( rolesRepository.HasWriteAccessToPolicyHotelCapRateImport(id))
			{
				ViewData["ImportAccess"] = "WriteAccess";
			}

            //Parent Information
            ViewData["PolicyGroupID"] = id;
            ViewData["PolicyGroupName"] = group.PolicyGroupName;


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

			var items = policyHotelCapRateGroupItemRepository.GetPolicyHotelCapRateGroupItems(id, filter ?? "", sortField, sortOrder ?? 0, page ?? 1);
            return View(items);
        }

        // GET: /Create
        public ActionResult Create(int id)
        {
            PolicyGroup group = new PolicyGroup();
            group = policyGroupRepository.GetGroup(id); ;

            //Check Exists
            if (group == null)
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

            //Populate List of currencies 
            CurrencyRepository currencyRepository = new CurrencyRepository();
            SelectList currencies = new SelectList(currencyRepository.GetAllCurrencies().ToList(), "CurrencyCode", "Name");
            ViewData["CurrencyList"] = currencies;

            //populate new PolicyHotelCapRateGroupItem with known PolicyGroup Information    
            PolicyHotelCapRateGroupItem policyHotelCapRateGroupItem = new PolicyHotelCapRateGroupItem();
            policyHotelCapRateGroupItem.PolicyGroupId = id;
            policyHotelCapRateGroupItem.PolicyGroupName = group.PolicyGroupName;

            //Show 'Create' Form
            return View(policyHotelCapRateGroupItem);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PolicyHotelCapRateGroupItem policyHotelCapRateGroupItem)
        {

            PolicyGroup policyGroup = policyGroupRepository.GetGroup(policyHotelCapRateGroupItem.PolicyGroupId);
            if (policyGroup == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }
            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyHotelCapRateGroupItem.PolicyGroupId))
            {
                return View("Error");
            }
            //Update Model from Form
            try
            {
                UpdateModel(policyHotelCapRateGroupItem);
            }
            catch
            {
                return View("Error");
            }
            try
            {
                policyHotelCapRateGroupItemRepository.Add(policyHotelCapRateGroupItem);
            }
            catch
            {
                //Could not insert to database
                return View("Error");
            }


            return RedirectToAction("List", new { id = policyHotelCapRateGroupItem.PolicyGroupId });
        }


        // GET: /Edit
        public ActionResult Edit(int id)
        {
            //Get PolicyHotelCapRateGroupItem
            PolicyHotelCapRateGroupItem policyHotelCapRateGroupItem = new PolicyHotelCapRateGroupItem();
            policyHotelCapRateGroupItem = policyHotelCapRateGroupItemRepository.GetPolicyHotelCapRateGroupItem(id);

            //Check Exists
            if (policyHotelCapRateGroupItem == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyHotelCapRateGroupItem.PolicyGroupId))
            {
                return View("Error");
            }

            //Populate List of PolicyLocations
            PolicyLocationRepository policyLocationRepository = new PolicyLocationRepository();
            SelectList policyLocations = new SelectList(policyLocationRepository.GetAllPolicyLocations().ToList(), "PolicyLocationId", "PolicyLocationName");
            ViewData["PolicyLocationList"] = policyLocations;

            //Populate List of currencies 
            CurrencyRepository currencyRepository = new CurrencyRepository();
            SelectList currencies = new SelectList(currencyRepository.GetAllCurrencies().ToList(), "CurrencyCode", "Name");
            ViewData["CurrencyList"] = currencies;

            //Populate new PolicyHotelCapRateGroupItem with known PolicyGroup Information
            PolicyGroup policyGroup = policyGroupRepository.GetGroup(policyHotelCapRateGroupItem.PolicyGroupId);
            policyHotelCapRateGroupItem.PolicyGroupName = policyGroup.PolicyGroupName;

            //Show 'Edit' Form
            policyHotelCapRateGroupItemRepository.EditItemForDisplay(policyHotelCapRateGroupItem);
            return View(policyHotelCapRateGroupItem);
        }

        // POST /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection collection)
        {
            PolicyHotelCapRateGroupItem policyHotelCapRateGroupItem = new PolicyHotelCapRateGroupItem();
            policyHotelCapRateGroupItem = policyHotelCapRateGroupItemRepository.GetPolicyHotelCapRateGroupItem(id);

            //Check Exists
            if (policyHotelCapRateGroupItem == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyHotelCapRateGroupItem.PolicyGroupId))
            {
                return View("Error");
            }

            //Update PolicyHotelCapRateGroupItem Model From Form
            try
            {
                UpdateModel(policyHotelCapRateGroupItem);
            }
            catch
            {
                return View("Error");
            }

            //Database Update
            try
            {
                policyHotelCapRateGroupItemRepository.Update(policyHotelCapRateGroupItem);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/PolicyHotelCapRateGroupItem.mvc/Edit/" + policyHotelCapRateGroupItem.PolicyHotelCapRateItemId.ToString();
                    return View("VersionError");
                }

                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");            }

            //Return to Form
            return RedirectToAction("List", new { id = policyHotelCapRateGroupItem.PolicyGroupId });

        }

        // GET: /View
        public ActionResult View(int id)
        {
            //Get PolicyHotelCapRateGroupItem
            PolicyHotelCapRateGroupItem policyHotelCapRateGroupItem = new PolicyHotelCapRateGroupItem();
            policyHotelCapRateGroupItem = policyHotelCapRateGroupItemRepository.GetPolicyHotelCapRateGroupItem(id);

            //Check Exists
            if (policyHotelCapRateGroupItem == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            //populate new PolicyHotelCapRateGroupItem with known PolicyGroup Information
            policyHotelCapRateGroupItemRepository.EditItemForDisplay(policyHotelCapRateGroupItem);

            //Show 'View' Form
            return View(policyHotelCapRateGroupItem);
        }

        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id)
        {
            //Get PolicyHotelCapRateGroupItem
            PolicyHotelCapRateGroupItem policyHotelCapRateGroupItem = new PolicyHotelCapRateGroupItem();
            policyHotelCapRateGroupItem = policyHotelCapRateGroupItemRepository.GetPolicyHotelCapRateGroupItem(id);

            //Check Exists
            if (policyHotelCapRateGroupItem == null)
            {
                ViewData["ActionMethod"] = "GetDelete";
                return View("RecordDoesNotExistError");
            }


            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyHotelCapRateGroupItem.PolicyGroupId))
            {
                return View("Error");
            }

            //populate new PolicyHotelCapRateGroupItem with known PolicyGroup Information
            policyHotelCapRateGroupItemRepository.EditItemForDisplay(policyHotelCapRateGroupItem);

            //Show 'Create' Form
            return View(policyHotelCapRateGroupItem);
        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            //Get PolicyHotelCapRateGroupItem
            PolicyHotelCapRateGroupItem policyHotelCapRateGroupItem = new PolicyHotelCapRateGroupItem();
            policyHotelCapRateGroupItem = policyHotelCapRateGroupItemRepository.GetPolicyHotelCapRateGroupItem(id);

            //Check Exists
            if (policyHotelCapRateGroupItem == null)
            {
                ViewData["ActionMethod"] = "PostDelete";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyHotelCapRateGroupItem.PolicyGroupId))
            {
                return View("Error");
            }
            //Delete Item
            try
            {
                policyHotelCapRateGroupItem.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                policyHotelCapRateGroupItemRepository.Delete(policyHotelCapRateGroupItem);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/PolicyHotelCapRateGroupItem.mvc/Delete/" + policyHotelCapRateGroupItem.PolicyHotelCapRateItemId.ToString();
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            //Return
            return RedirectToAction("List", new { id = policyHotelCapRateGroupItem.PolicyGroupId });
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
			var result = policyGroupSequenceRepository.GetPolicyHotelCapRateGroupItemSequences(id, page ?? 1);

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
				string[] policyPolicyHotelCapRateItemPK = s.Split(new char[] { '_' });

				int policyHotelCapRateItemId = Convert.ToInt32(policyPolicyHotelCapRateItemPK[0]);
				int versionNumber = Convert.ToInt32(policyPolicyHotelCapRateItemPK[1]);

				XmlElement xmlItem = doc.CreateElement("Item");
				root.AppendChild(xmlItem);

				XmlElement xmlSequence = doc.CreateElement("Sequence");
				xmlSequence.InnerText = sequence.ToString();
				xmlItem.AppendChild(xmlSequence);

				XmlElement xmlPolicyHotelCapRateItemId = doc.CreateElement("PolicyHotelCapRateItemId");
				xmlPolicyHotelCapRateItemId.InnerText = policyHotelCapRateItemId.ToString();
				xmlItem.AppendChild(xmlPolicyHotelCapRateItemId);

				XmlElement xmlVersionNumber = doc.CreateElement("VersionNumber");
				xmlVersionNumber.InnerText = versionNumber.ToString();
				xmlItem.AppendChild(xmlVersionNumber);

				sequence = sequence + 1;
			}

			try
			{
				PolicyGroupSequenceRepository policyGroupSequenceRepository = new PolicyGroupSequenceRepository();
				policyGroupSequenceRepository.UpdatePolicyHotelCapRateGroupItemSequences(System.Xml.Linq.XElement.Parse(doc.OuterXml));
			}
			catch (SqlException ex)
			{
				//Versioning Error
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/PolicyHotelCapRateGroupItem.mvc/EditSequence/" + id + "?page=" + page;
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
			byte[] csvData = policyHotelCapRateGroupItemRepository.Export(id);
			return File(csvData, "text/csv", "Policy Hotel Cap Rate Items Export.csv");
		}

        // GET: /ExportErrors
        public ActionResult ExportErrors()
        {            
            var preImportCheckResultVM = (PolicyHotelCapRateImportStep1VM)TempData["ErrorMessages"];

            if (preImportCheckResultVM == null)
            {
                ViewData["ActionMethod"] = "ExportGet";
                return View("RecordDoesNotExistError");
            }

            //var errors = policyHotelCapRateGroupItemRepository.PreImportCheck(preImportCheckResultVM.ImportStep2VM.FileBytes, preImportCheckResultVM.PolicyGroupId).ReturnMessages;
            var errors = preImportCheckResultVM.ImportStep2VM.ReturnMessages;

            //Get CSV Data
            var combinedErrors = errors.Aggregate((current, next) => current + "\r\n" + next);
            byte[] csvData = Encoding.ASCII.GetBytes(combinedErrors);
            return File(csvData, "text/plain", "PolicyHotelCapRateGroupItem.txt");
        }

        public ActionResult ImportStep1(int id)
        {

            PolicyHotelCapRateImportStep1WithFileVM cdrLinkImportFileVM = new PolicyHotelCapRateImportStep1WithFileVM();
            cdrLinkImportFileVM.PolicyGroupId = id;

            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(id);

            cdrLinkImportFileVM.PolicyGroup = policyGroup;

            return View(cdrLinkImportFileVM);
        }

        [HttpPost]
        public ActionResult ImportStep1(PolicyHotelCapRateImportStep1WithFileVM csvfile)
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
                PolicyHotelCapRateImportStep2VM preImportCheckResult = new PolicyHotelCapRateImportStep2VM();
                List<string> returnMessages = new List<string>();

                preImportCheckResult = policyHotelCapRateGroupItemRepository.PreImportCheck(csvfile.File, csvfile.PolicyGroupId);

                PolicyHotelCapRateImportStep1VM preImportCheckResultVM = new PolicyHotelCapRateImportStep1VM();
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
            PolicyHotelCapRateImportStep1VM preImportCheckResultVM = new PolicyHotelCapRateImportStep1VM();
            preImportCheckResultVM = (PolicyHotelCapRateImportStep1VM)TempData["PreImportCheckResultVM"];

            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(preImportCheckResultVM.PolicyGroupId);
            preImportCheckResultVM.PolicyGroup = policyGroup;

            return View(preImportCheckResultVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ImportStep2(PolicyHotelCapRateImportStep1VM preImportCheckResultVM)
        {
            if (preImportCheckResultVM.ImportStep2VM.IsValidData == false)
            {
				//Check JSON for valid messages
				if (preImportCheckResultVM.ImportStep2VM.ReturnMessages[0] != null)
				{
					List<string> returnMessages = new List<string>();
					List<string> returnMessagesJSON = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(preImportCheckResultVM.ImportStep2VM.ReturnMessages[0]);
					
					foreach (string message in returnMessagesJSON)
					{
						if (message.StartsWith("Row"))
						{
							returnMessages.Add(message);
						}
					}

					preImportCheckResultVM.ImportStep2VM.ReturnMessages = returnMessages;
				}

                TempData["ErrorMessages"] = preImportCheckResultVM;
                return RedirectToAction("ExportErrors");
            }

            //PreImport Check Results (check has passed)
            PolicyHotelCapRateImportStep2VM preImportCheckResult = new PolicyHotelCapRateImportStep2VM();
            preImportCheckResult = preImportCheckResultVM.ImportStep2VM;

            //Do the Import, return results
            PolicyHotelCapRateImportStep3VM cdrPostImportResult = new PolicyHotelCapRateImportStep3VM();
            cdrPostImportResult = policyHotelCapRateGroupItemRepository.Import(
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
            PolicyHotelCapRateImportStep3VM cdrPostImportResult = new PolicyHotelCapRateImportStep3VM();
            cdrPostImportResult = (PolicyHotelCapRateImportStep3VM)TempData["CdrPostImportResult"];

            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(cdrPostImportResult.PolicyGroupId);
            cdrPostImportResult.PolicyGroup = policyGroup;

            return View(cdrPostImportResult);
        }
    }
}
