using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Repository;
using CWTDesktopDatabase.Helpers;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;

namespace CWTDesktopDatabase.Controllers
{
    public class ContactController : Controller
    {
        ClientDetailRepository clientDetailRepository = new ClientDetailRepository();
        ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
        ClientDetailClientSubUnitRepository clientDetailClientSubUnitRepository = new ClientDetailClientSubUnitRepository();
        ClientDetailContactRepository clientDetailContactRepository = new ClientDetailContactRepository();
		ClientSubUnitContactRepository clientSubUnitContactRepository = new ClientSubUnitContactRepository();


        // GET: /List
		public ActionResult List(string id, string filter, int? page, string sortField, int? sortOrder)
        {
			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(id);

            //Check Exists
			if (clientSubUnit == null)
            {
                ViewData["ActionMethod"] = "List";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
			if (rolesRepository.HasWriteAccessToClientSubUnit(id))
            {
                ViewData["Access"] = "WriteAccess";
            }

			//SortField
			if (sortField == string.Empty)
			{
				sortField = "ContactTypeName";
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

            //Populate View Model
            ContactsVM contactsVM = new ContactsVM();

			clientSubUnitRepository.EditGroupForDisplay(clientSubUnit);
			contactsVM.ClientSubUnit = clientSubUnit;

			var contacts = clientSubUnitContactRepository.PageClientSubUnitContacts(id, filter ?? "", sortField, sortOrder ?? 0, page ?? 1);
			if (contacts != null)
			{
				contactsVM.Contacts = contacts;
			}
            
            return View(contactsVM);
        }

        // GET: /Create
        public ActionResult Create(string id)
        {
			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(id);

			//Check Exists
			if (clientSubUnit == null)
			{
				ViewData["ActionMethod"] = "List";
				return View("RecordDoesNotExistError");
			}

			clientSubUnitRepository.EditGroupForDisplay(clientSubUnit);

            //Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToClientSubUnit(id))
            {
                ViewData["Access"] = "WriteAccess";
            }

			ContactVM contactVM = new ContactVM();
			contactVM.ClientSubUnit = clientSubUnit;

            Contact contact = new Contact();
            contactVM.Contact = contact;

			ContactTypeRepository contactTypeRepository = new ContactTypeRepository();
			contactVM.ContactTypes = new SelectList(contactTypeRepository.GetAllContactTypes().ToList(), "ContactTypeId", "ContactTypeName");

			CountryRepository countryRepository = new CountryRepository();
			contactVM.Countries = new SelectList(countryRepository.GetAllCountries().ToList(), "CountryCode", "CountryName");

			Dictionary<string, string> stateProvinces = new Dictionary<string, string>();
			contactVM.StateProvinces = new SelectList(stateProvinces);

			return View(contactVM);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
		public ActionResult Create(ContactVM contactVM)
        {
			string id = contactVM.ClientSubUnit.ClientSubUnitGuid;

			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(id);

			//Check Exists
			if (clientSubUnit == null)
			{
				ViewData["ActionMethod"] = "List";
				return View("RecordDoesNotExistError");
			}

			clientSubUnitRepository.EditGroupForDisplay(clientSubUnit);

			//Access Rights
			ViewData["Access"] = "";
			RolesRepository rolesRepository = new RolesRepository();
			if (rolesRepository.HasWriteAccessToClientSubUnit(id))
			{
				ViewData["Access"] = "WriteAccess";
			}

            //Update  Model from Form
            try
            {
                TryUpdateModel<Contact>(contactVM.Contact, "Contact");
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
				clientSubUnitContactRepository.Add(contactVM);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List", new { id = id });
        }

		// GET: /Edit
		public ActionResult Edit(int id) {
			
			Contact contact = new Contact();
			contact = clientSubUnitContactRepository.GetContact(id);

			//Check Exists
			if (contact == null)
			{
				ViewData["ActionMethod"] = "List";
				return View("RecordDoesNotExistError");
			}

			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(contact.ClientSubUnitGuid);

			//Check Exists
			if (clientSubUnit == null)
			{
				ViewData["ActionMethod"] = "List";
				return View("RecordDoesNotExistError");
			}

			clientSubUnitRepository.EditGroupForDisplay(clientSubUnit);

			//Access Rights
			ViewData["Access"] = "";
			RolesRepository rolesRepository = new RolesRepository();
			if (rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnit.ClientSubUnitGuid))
			{
				ViewData["Access"] = "WriteAccess";
			}

			ContactVM contactVM = new ContactVM();
			contactVM.ClientSubUnit = clientSubUnit;
			clientSubUnitContactRepository.EditForDisplay(contact);
			contactVM.Contact = contact;

			ContactTypeRepository contactTypeRepository = new ContactTypeRepository();
			contactVM.ContactTypes = new SelectList(contactTypeRepository.GetAllContactTypes().ToList(), "ContactTypeId", "ContactTypeName", contact.ContactTypeId);

			CountryRepository countryRepository = new CountryRepository();
			contactVM.Countries = new SelectList(countryRepository.GetAllCountries().ToList(), "CountryCode", "CountryName", contact.CountryCode);

			StateProvinceRepository stateProvinceRepository = new StateProvinceRepository();
			contactVM.StateProvinces = new SelectList(stateProvinceRepository.GetStateProvincesByCountryCode(contact.CountryCode).ToList(), "StateProvinceCode", "Name", contact.StateProvinceName);
			
			return View(contactVM);
		}

		// POST: /Edit
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(ContactVM contactVM)
		{
			string id = contactVM.Contact.ClientSubUnitGuid;

			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(id);

			//Check Exists
			if (clientSubUnit == null)
			{
				ViewData["ActionMethod"] = "List";
				return View("RecordDoesNotExistError");
			}

			clientSubUnitRepository.EditGroupForDisplay(clientSubUnit);
			contactVM.ClientSubUnit = clientSubUnit;

			//Access Rights
			ViewData["Access"] = "";
			RolesRepository rolesRepository = new RolesRepository();
			if (rolesRepository.HasWriteAccessToClientSubUnit(id))
			{
				ViewData["Access"] = "WriteAccess";
			}

			//Update  Model from Form
			try
			{
				TryUpdateModel<Contact>(contactVM.Contact, "Contact");
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
				clientSubUnitContactRepository.Edit(contactVM);
			}
			catch (SqlException ex)
			{
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("List", new { id = id });
		}

		// GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id)
		{
			Contact contact = new Contact();
			contact = clientSubUnitContactRepository.GetContact(id);

			//Check Exists
			if (contact == null)
			{
				ViewData["ActionMethod"] = "List";
				return View("RecordDoesNotExistError");
			}

			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(contact.ClientSubUnitGuid);

			//Check Exists
			if (clientSubUnit == null)
			{
				ViewData["ActionMethod"] = "List";
				return View("RecordDoesNotExistError");
			}

			clientSubUnitRepository.EditGroupForDisplay(clientSubUnit);

			//Access Rights
			ViewData["Access"] = "";
			RolesRepository rolesRepository = new RolesRepository();
			if (rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnit.ClientSubUnitGuid))
			{
				ViewData["Access"] = "WriteAccess";
			}

			ContactVM contactVM = new ContactVM();
			contactVM.ClientSubUnit = clientSubUnit;
			clientSubUnitContactRepository.EditForDisplay(contact);
			contactVM.Contact = contact;

			ContactTypeRepository contactTypeRepository = new ContactTypeRepository();
			contactVM.ContactTypes = new SelectList(contactTypeRepository.GetAllContactTypes().ToList(), "ContactTypeId", "ContactTypeName", contact.ContactTypeId);

			CountryRepository countryRepository = new CountryRepository();
			contactVM.Countries = new SelectList(countryRepository.GetAllCountries().ToList(), "CountryCode", "CountryName", contact.CountryCode);

			return View(contactVM);
		}

		// POST: /Delete
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, FormCollection collection)
		{
			Contact contact = new Contact();
			contact = clientSubUnitContactRepository.GetContact(id);

			//Check Exists
			if (contact == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			string clientSubUnitGuid = contact.ClientSubUnitGuid;
           
			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientSubUnitGuid);

			//Access Rights
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnitGuid))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Delete Item
			try
			{
				contact.VersionNumber = Int32.Parse(collection["Contact.VersionNumber"]);
				clientSubUnitContactRepository.Delete(contact);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/ClientSubUnitContact.mvc/Delete/" + id.ToString();
					return View("VersionError");
				}
				//Generic Error
				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("List", new { id = contact.ClientSubUnitGuid });
		}

		// GET: /View
		public ActionResult View(int id)
		{
			Contact contact = new Contact();
			contact = clientSubUnitContactRepository.GetContact(id);

			//Check Exists
			if (contact == null)
			{
				ViewData["ActionMethod"] = "List";
				return View("RecordDoesNotExistError");
			}

			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(contact.ClientSubUnitGuid);

			//Check Exists
			if (clientSubUnit == null)
			{
				ViewData["ActionMethod"] = "List";
				return View("RecordDoesNotExistError");
			}

			clientSubUnitRepository.EditGroupForDisplay(clientSubUnit);

			//Access Rights
			ViewData["Access"] = "";
			RolesRepository rolesRepository = new RolesRepository();
			if (rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnit.ClientSubUnitGuid))
			{
				ViewData["Access"] = "WriteAccess";
			}

			ContactVM contactVM = new ContactVM();
			contactVM.ClientSubUnit = clientSubUnit;
			clientSubUnitContactRepository.EditForDisplay(contact); 
			contactVM.Contact = contact;

			ContactTypeRepository contactTypeRepository = new ContactTypeRepository();
			contactVM.ContactTypes = new SelectList(contactTypeRepository.GetAllContactTypes().ToList(), "ContactTypeId", "ContactTypeName", contact.ContactTypeId);

			CountryRepository countryRepository = new CountryRepository();
			contactVM.Countries = new SelectList(countryRepository.GetAllCountries().ToList(), "CountryCode", "CountryName", contact.CountryCode);

			return View(contactVM);
		}

		// GET: /Export
		public ActionResult Export(string id)
		{
			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(id);

			//Check Exists
			if (clientSubUnit == null)
			{
				ViewData["ActionMethod"] = "List";
				return View("RecordDoesNotExistError");
			}

			//Get CSV Data
			byte[] csvData = clientSubUnitContactRepository.Export(id);

			//Remove special characters for filename
			string clientSubUnitName = Regex.Replace(clientSubUnit.ClientSubUnitName, "[^a-zA-Z0-9]+", "", RegexOptions.Compiled);

			return File(csvData, "text/csv", string.Format("{0}-ContactsExport.csv", clientSubUnitName));
		}

		// GET: /ExportErrors
		public ActionResult ExportErrors()
		{
			var preImportCheckResultVM = (ClientSubUnitContactImportStep1VM)TempData["ErrorMessages"];

			if (preImportCheckResultVM == null)
			{
				ViewData["ActionMethod"] = "ExportGet";
				return View("RecordDoesNotExistError");
			}

			var errors = preImportCheckResultVM.ImportStep2VM.ReturnMessages;
			
			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(preImportCheckResultVM.ClientSubUnit.ClientSubUnitGuid);

			//Check Exists
			if (clientSubUnit == null)
			{
				ViewData["ActionMethod"] = "List";
				return View("RecordDoesNotExistError");
			}

			//Remove special characters for filename
			string clientSubUnitName = Regex.Replace(clientSubUnit.ClientSubUnitName, "[^a-zA-Z0-9]+", "", RegexOptions.Compiled);

			//Get CSV Data
			var combinedErrors = errors.Aggregate((current, next) => current + "\r\n" + next);
			byte[] csvData = Encoding.ASCII.GetBytes(combinedErrors);
			return File(csvData, "text/plain", string.Format("{0}Contacts.txt", clientSubUnitName));
		}

		public ActionResult ImportStep1(string id)
		{
			ClientSubUnitContactImportStep1WithFileVM ClientSubUnitContactImportStep1WithFileVM = new ClientSubUnitContactImportStep1WithFileVM();

			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(id);

			//Check Exists
			if (clientSubUnit == null)
			{
				ViewData["ActionMethod"] = "ListSubMenu";
				return View("RecordDoesNotExistError");
			}

			ClientSubUnitContactImportStep1WithFileVM.ClientSubUnit = clientSubUnit;
			ClientSubUnitContactImportStep1WithFileVM.ClientSubUnitGuid = id;

			return View(ClientSubUnitContactImportStep1WithFileVM);
		}

		[HttpPost]
		public ActionResult ImportStep1(ClientSubUnitContactImportStep1WithFileVM csvfile)
		{
			//used for return only
			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csvfile.ClientSubUnitGuid);
			csvfile.ClientSubUnit = clientSubUnit;

			if (!ModelState.IsValid)
			{

				return View(csvfile);
			}
			string fileExtension = Path.GetExtension(csvfile.File.FileName);
			if (fileExtension != ".csv")
			{
				ModelState.AddModelError("file", "This is not a valid entry");
				return View(csvfile);
			}

			if (csvfile.File.ContentLength > 0)
			{
				ClientSubUnitContactImportStep2VM preImportCheckResult = new ClientSubUnitContactImportStep2VM();
				List<string> returnMessages = new List<string>();

				preImportCheckResult = clientSubUnitContactRepository.PreImportCheck(csvfile.File, csvfile.ClientSubUnitGuid);

				ClientSubUnitContactImportStep1VM preImportCheckResultVM = new ClientSubUnitContactImportStep1VM();
				preImportCheckResultVM.ClientSubUnit = clientSubUnit;
				preImportCheckResultVM.ImportStep2VM = preImportCheckResult;
				preImportCheckResultVM.ClientSubUnitGuid = csvfile.ClientSubUnitGuid;

				TempData["PreImportCheckResultVM"] = preImportCheckResultVM;
				return RedirectToAction("ImportStep2");
			}

			return View();
		}

		public ActionResult ImportStep2()
		{
			ClientSubUnitContactImportStep1VM preImportCheckResultVM = new ClientSubUnitContactImportStep1VM();
			preImportCheckResultVM = (ClientSubUnitContactImportStep1VM)TempData["PreImportCheckResultVM"];
			if (preImportCheckResultVM != null)
			{
				ClientSubUnit clientSubUnit = new ClientSubUnit();
				clientSubUnit = clientSubUnitRepository.GetClientSubUnit(preImportCheckResultVM.ClientSubUnitGuid);
				preImportCheckResultVM.ClientSubUnit = clientSubUnit;
			}
			else
			{
				return View("Error");
			}

			return View(preImportCheckResultVM);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult ImportStep2(ClientSubUnitContactImportStep1VM preImportCheckResultVM)
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
			ClientSubUnitContactImportStep2VM preImportCheckResult = new ClientSubUnitContactImportStep2VM();
			preImportCheckResult = preImportCheckResultVM.ImportStep2VM;

			//Do the Import, return results
			ClientSubUnitContactImportStep3VM cdrPostImportResult = new ClientSubUnitContactImportStep3VM();
			cdrPostImportResult = clientSubUnitContactRepository.Import(
				preImportCheckResult.FileBytes,
				preImportCheckResultVM.ClientSubUnit.ClientSubUnitGuid
			);

			cdrPostImportResult.ClientSubUnitGuid = preImportCheckResultVM.ClientSubUnit.ClientSubUnitGuid;
			TempData["CdrPostImportResult"] = cdrPostImportResult;

			//Pass Results to Next Page
			return RedirectToAction("ImportStep3");

		}

		public ActionResult ImportStep3()
		{
			//Display Results of Import
			ClientSubUnitContactImportStep3VM cdrPostImportResult = new ClientSubUnitContactImportStep3VM();
			cdrPostImportResult = (ClientSubUnitContactImportStep3VM)TempData["CdrPostImportResult"];
			if (cdrPostImportResult != null)
			{
				ClientSubUnit clientSubUnit = new ClientSubUnit();

				if (cdrPostImportResult.ClientSubUnitGuid != null)
				{
					clientSubUnit = clientSubUnitRepository.GetClientSubUnit(cdrPostImportResult.ClientSubUnitGuid);
					cdrPostImportResult.ClientSubUnit = clientSubUnit;
				}
			}
			else
			{
				return View("Error");
			}

			return View(cdrPostImportResult);
		}
    }
}
