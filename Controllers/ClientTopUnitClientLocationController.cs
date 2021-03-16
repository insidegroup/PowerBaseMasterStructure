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
using System.Text;
using System.IO;

namespace CWTDesktopDatabase.Controllers
{
    public class ClientTopUnitClientLocationController : Controller
    {
		ClientTopUnitRepository clientTopUnitRepository = new ClientTopUnitRepository();
		ClientTopUnitClientLocationRepository clientTopUnitClientLocationRepository = new ClientTopUnitClientLocationRepository();
		HierarchyRepository hierarchyRepository = new HierarchyRepository();

		private string groupName = "Client Detail";
		private string groupNameClientLocation = "Client Location Administrator";

        // GET: /List
		public ActionResult List(int? page, string id, string filter, string sortField, int? sortOrder)
        {
            ClientTopUnit clientTopUnit = new ClientTopUnit();
			clientTopUnit = clientTopUnitRepository.GetClientTopUnit(id);

			//Check Exists
			if (clientTopUnit == null)
			{
				ViewData["ActionMethod"] = "ListSubMenu";
				return View("RecordDoesNotExistError");
			}

            //Set Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
			if (rolesRepository.HasWriteAccessToClientTopUnit(clientTopUnit.ClientTopUnitGuid) || hierarchyRepository.AdminHasDomainWriteAccess(groupNameClientLocation))
            {
                ViewData["Access"] = "WriteAccess";
            }

			//Set Create Rights
			ViewData["CreateAccess"] = "";
			if ((hierarchyRepository.AdminHasDomainWriteAccess(groupName) && rolesRepository.HasWriteAccessToClientTopUnit(clientTopUnit.ClientTopUnitGuid)) 
				|| hierarchyRepository.AdminHasDomainWriteAccess(groupNameClientLocation))
			{
				ViewData["CreateAccess"] = "WriteAccess";
			}

			//Set Import Access Rights
			ViewData["ImportAccess"] = "";
			if (rolesRepository.HasWriteAccessToClientTopUnitClientLocationImport(clientTopUnit.ClientTopUnitGuid))
			{
				ViewData["ImportAccess"] = "WriteAccess";
			}

            //SortField
			if (string.IsNullOrEmpty(sortField))
			{
				sortField = "AddressLocationName";
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
			ClientTopUnitClientLocationsVM clientTopUnitClientLocationsVM = new ClientTopUnitClientLocationsVM();
			clientTopUnitClientLocationsVM.ClientTopUnit = clientTopUnit;

			var getClientTopUnitClientLocations = clientTopUnitClientLocationRepository.GetClientTopUnitClientLocations(clientTopUnit.ClientTopUnitGuid, filter ?? "", sortField, sortOrder ?? 0, page ?? 1);
			if (getClientTopUnitClientLocations != null)
			{
				clientTopUnitClientLocationsVM.ClientLocations = getClientTopUnitClientLocations;
			}

			return View(clientTopUnitClientLocationsVM);
        }

		//// GET: /Create
		public ActionResult Create(string id) {
			
			ClientTopUnit clientTopUnit = new ClientTopUnit();
            clientTopUnit = clientTopUnitRepository.GetClientTopUnit(id);

			//Check Exists
			if (clientTopUnit == null)
			{
				ViewData["ActionMethod"] = "ListSubMenu";
				return View("RecordDoesNotExistError");
			}

            //Set Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
			if (rolesRepository.HasWriteAccessToClientTopUnit(clientTopUnit.ClientTopUnitGuid))
            {
                ViewData["Access"] = "WriteAccess";
            }

			//Set Create Rights
			ViewData["CreateAccess"] = "";
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName) || hierarchyRepository.AdminHasDomainWriteAccess(groupNameClientLocation))
			{
				ViewData["CreateAccess"] = "WriteAccess";
			}
			
			//Populate View Model
			ClientTopUnitClientLocationVM clientTopUnitClientLocationVM = new ClientTopUnitClientLocationVM();
			clientTopUnitClientLocationVM.ClientTopUnit = clientTopUnit;
			
			CountryRepository countryRepository = new CountryRepository();
			SelectList countriesList = new SelectList(countryRepository.GetAllCountries().ToList(), "CountryCode", "CountryName");
			clientTopUnitClientLocationVM.Countries = countriesList;

            //Generate ranking dropdown list data
            var list = new SelectList(new[]
                {
                    new { Value = "1", Text = "1" },
                    new { Value = "2", Text = "2" },
                    new { Value = "3", Text = "3" },
                    new { Value = "4", Text = "4" },
                    new { Value = "5", Text = "5" },
                    new { Value = "6", Text = "6" },
                    new { Value = "7", Text = "7" },
                    new { Value = "8", Text = "8" },
                    new { Value = "9", Text = "9" }
                },
            "Value", "Text", 0);
            ViewData["RankingList"] = list;

            return View(clientTopUnitClientLocationVM);
        }

		// POST: /Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(ClientTopUnitClientLocationVM clientTopUnitClientLocationVM)
		{
			string clientTopUnitGuid = clientTopUnitClientLocationVM.ClientTopUnit.ClientTopUnitGuid;

			ClientTopUnit clientTopUnit = new ClientTopUnit();
			clientTopUnit = clientTopUnitRepository.GetClientTopUnit(clientTopUnitGuid);

			//Check Exists
			if (clientTopUnit == null)
			{
				ViewData["ActionMethod"] = "ListSubMenu";
				return View("RecordDoesNotExistError");
			}

			//Access Rights
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToClientTopUnit(clientTopUnitGuid) && !hierarchyRepository.AdminHasDomainWriteAccess(groupNameClientLocation))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Update  Model from Form
			try
			{
				TryUpdateModel<ClientTopUnitClientLocation>(clientTopUnitClientLocationVM.ClientTopUnitClientLocation, "ClientTopUnitClientLocation");
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
				clientTopUnitClientLocationRepository.Add(clientTopUnitClientLocationVM);
			}
			catch (SqlException ex)
			{
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}


			return RedirectToAction("List", new { id = clientTopUnitGuid });
		}

		// GET: /Edit
		public ActionResult Edit(string id, int addressId)
		{
			ClientTopUnit clientTopUnit = new ClientTopUnit();
			clientTopUnit = clientTopUnitRepository.GetClientTopUnit(id);

			//Check Exists
			if (clientTopUnit == null)
			{
				ViewData["ActionMethod"] = "ListSubMenu";
				return View("RecordDoesNotExistError");
			}

			//Set Access Rights
			ViewData["Access"] = "";
			RolesRepository rolesRepository = new RolesRepository();
			if (rolesRepository.HasWriteAccessToClientTopUnit(clientTopUnit.ClientTopUnitGuid))
			{
				ViewData["Access"] = "WriteAccess";
			}

			//Set Create Rights
			ViewData["CreateAccess"] = "";
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName) || hierarchyRepository.AdminHasDomainWriteAccess(groupNameClientLocation))
			{
				ViewData["CreateAccess"] = "WriteAccess";
			}

			//Populate View Model
			ClientTopUnitClientLocationVM clientTopUnitClientLocationVM = new ClientTopUnitClientLocationVM();
			clientTopUnitClientLocationVM.ClientTopUnit = clientTopUnit;

			//Add Address
			ClientTopUnitClientLocation clientTopUnitClientLocation = new ClientTopUnitClientLocation();
			ClientTopUnitClientLocationRepository clientTopUnitClientLocationRepository = new ClientTopUnitClientLocationRepository();
			clientTopUnitClientLocation = clientTopUnitClientLocationRepository.GetAddress(addressId);
			if (clientTopUnitClientLocation != null)
			{
				clientTopUnitClientLocationVM.ClientTopUnitClientLocation = clientTopUnitClientLocation;
			}
			
			CountryRepository countryRepository = new CountryRepository();
			SelectList countriesList = new SelectList(countryRepository.GetAllCountries().ToList(), "CountryCode", "CountryName", clientTopUnitClientLocation.CountryCode ?? "");
			clientTopUnitClientLocationVM.Countries = countriesList;

            //Generate ranking dropdown list data
            var list = new SelectList(new[]
                {
                    new { Value = "1", Text = "1" },
                    new { Value = "2", Text = "2" },
                    new { Value = "3", Text = "3" },
                    new { Value = "4", Text = "4" },
                    new { Value = "5", Text = "5" },
                    new { Value = "6", Text = "6" },
                    new { Value = "7", Text = "7" },
                    new { Value = "8", Text = "8" },
                    new { Value = "9", Text = "9" }
                },
            "Value", "Text", clientTopUnitClientLocationVM.ClientTopUnitClientLocation.Ranking != null ? clientTopUnitClientLocationVM.ClientTopUnitClientLocation.Ranking : 0);
            ViewData["RankingList"] = list;


            return View(clientTopUnitClientLocationVM);
		}

		// POST: /Edit
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(ClientTopUnitClientLocationVM clientTopUnitClientLocationVM)
		{
			string clientTopUnitGuid = clientTopUnitClientLocationVM.ClientTopUnit.ClientTopUnitGuid;

			ClientTopUnit clientTopUnit = new ClientTopUnit();
			clientTopUnit = clientTopUnitRepository.GetClientTopUnit(clientTopUnitGuid);

			//Check Exists
			if (clientTopUnit == null)
			{
				ViewData["ActionMethod"] = "ListSubMenu";
				return View("RecordDoesNotExistError");
			}

			//Access Rights
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToClientTopUnit(clientTopUnitGuid) && !hierarchyRepository.AdminHasDomainWriteAccess(groupNameClientLocation))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

            //Update  Model from Form
            try
			{
				TryUpdateModel<ClientTopUnitClientLocation>(clientTopUnitClientLocationVM.ClientTopUnitClientLocation, "ClientTopUnitClientLocation");
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
				clientTopUnitClientLocationRepository.Update(clientTopUnitClientLocationVM);
			}
			catch (SqlException ex)
			{
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("List", new { id = clientTopUnitGuid });
		}

		// GET: /Delete
		[HttpGet]
		public ActionResult Delete(string id, int addressId)
		{
			ClientTopUnit clientTopUnit = new ClientTopUnit();
			clientTopUnit = clientTopUnitRepository.GetClientTopUnit(id);

			//Check Exists
			if (clientTopUnit == null)
			{
				ViewData["ActionMethod"] = "ListSubMenu";
				return View("RecordDoesNotExistError");
			}

			//Set Access Rights
			ViewData["Access"] = "";
			RolesRepository rolesRepository = new RolesRepository();
			if (rolesRepository.HasWriteAccessToClientTopUnit(clientTopUnit.ClientTopUnitGuid))
			{
				ViewData["Access"] = "WriteAccess";
			}

			//Set Create Rights
			ViewData["CreateAccess"] = "";
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName) || hierarchyRepository.AdminHasDomainWriteAccess(groupNameClientLocation))
			{
				ViewData["CreateAccess"] = "WriteAccess";
			}

			//Populate View Model
			ClientTopUnitClientLocationVM clientTopUnitClientLocationVM = new ClientTopUnitClientLocationVM();
			clientTopUnitClientLocationVM.ClientTopUnit = clientTopUnit;
			
			//Add Address
			ClientTopUnitClientLocation clientTopUnitClientLocation = new ClientTopUnitClientLocation();
			ClientTopUnitClientLocationRepository clientTopUnitClientLocationRepository = new ClientTopUnitClientLocationRepository();
			clientTopUnitClientLocation = clientTopUnitClientLocationRepository.GetAddress(addressId);
			if (clientTopUnitClientLocation != null)
			{
				clientTopUnitClientLocationVM.ClientTopUnitClientLocation = clientTopUnitClientLocation;
			}
			
			return View(clientTopUnitClientLocationVM);
		}

		// POST: /Delete
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(ClientTopUnitClientLocationVM clientTopUnitClientLocationVM)
		{
			string clientTopUnitGuid = clientTopUnitClientLocationVM.ClientTopUnit.ClientTopUnitGuid;

			ClientTopUnit clientTopUnit = new ClientTopUnit();
			clientTopUnit = clientTopUnitRepository.GetClientTopUnit(clientTopUnitGuid);

			//Check Exists
			if (clientTopUnit == null)
			{
				ViewData["ActionMethod"] = "ListSubMenu";
				return View("RecordDoesNotExistError");
			}

			//Access Rights
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToClientTopUnit(clientTopUnitGuid) && !hierarchyRepository.AdminHasDomainWriteAccess(groupNameClientLocation))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			try
			{
				clientTopUnitClientLocationRepository.Delete(clientTopUnitClientLocationVM);
			}
			catch (SqlException ex)
			{
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("List", new { id = clientTopUnitGuid });
		}

		// GET: /View
		public ActionResult View(string id, int addressId)
		{
			ClientTopUnit clientTopUnit = new ClientTopUnit();
			clientTopUnit = clientTopUnitRepository.GetClientTopUnit(id);

			//Check Exists
			if (clientTopUnit == null)
			{
				ViewData["ActionMethod"] = "ListSubMenu";
				return View("RecordDoesNotExistError");
			}

			//Set Access Rights
			ViewData["Access"] = "";
			RolesRepository rolesRepository = new RolesRepository();
			if (rolesRepository.HasWriteAccessToClientTopUnit(clientTopUnit.ClientTopUnitGuid))
			{
				ViewData["Access"] = "WriteAccess";
			}

			//Set Create Rights
			ViewData["CreateAccess"] = "";
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["CreateAccess"] = "WriteAccess";
			}

			//Populate View Model
			ClientTopUnitClientLocationVM clientTopUnitClientLocationVM = new ClientTopUnitClientLocationVM();
			clientTopUnitClientLocationVM.ClientTopUnit = clientTopUnit;

			//Add Address
			ClientTopUnitClientLocation clientTopUnitClientLocation = new ClientTopUnitClientLocation();
			ClientTopUnitClientLocationRepository clientTopUnitClientLocationRepository = new ClientTopUnitClientLocationRepository();
			clientTopUnitClientLocation = clientTopUnitClientLocationRepository.GetAddress(addressId);
			if (clientTopUnitClientLocation != null)
			{
				clientTopUnitClientLocationVM.ClientTopUnitClientLocation = clientTopUnitClientLocation;
			}

			return View(clientTopUnitClientLocationVM);
		}

		// GET: /Export
		public ActionResult Export(string id)
		{
			ClientTopUnit clientTopUnit = new ClientTopUnit();
			clientTopUnit = clientTopUnitRepository.GetClientTopUnit(id);

			//Check Exists
			if (clientTopUnit == null)
			{
				ViewData["ActionMethod"] = "ListSubMenu";
				return View("RecordDoesNotExistError");
			}

			//Get CSV Data
			byte[] csvData = clientTopUnitClientLocationRepository.Export(id);

			//Remove special characters for filename
			string clientTopUnitName = Regex.Replace(clientTopUnit.ClientTopUnitName, "[^a-zA-Z0-9]+", "", RegexOptions.Compiled);

			return File(csvData, "text/csv", string.Format("{0}-ClientLocations Export.csv", clientTopUnitName));
		}

		// GET: /ExportErrors
		public ActionResult ExportErrors()
		{
			var preImportCheckResultVM = (ClientTopUnitImportStep1VM)TempData["ErrorMessages"];

			if (preImportCheckResultVM == null)
			{
				ViewData["ActionMethod"] = "ExportGet";
				return View("RecordDoesNotExistError");
			}

			var errors = preImportCheckResultVM.ImportStep2VM.ReturnMessages;

			//Get CSV Data
			var combinedErrors = errors.Aggregate((current, next) => current + "\r\n" + next);
			byte[] csvData = Encoding.ASCII.GetBytes(combinedErrors);
			return File(csvData, "text/plain", "ClientLocations.txt");
		}

		public ActionResult ImportStep1(string id)
		{
			ClientTopUnitImportStep1WithFileVM clientTopUnitImportStep1WithFileVM = new ClientTopUnitImportStep1WithFileVM();

			ClientTopUnit clientTopUnit = new ClientTopUnit();
			clientTopUnit = clientTopUnitRepository.GetClientTopUnit(id);

			//Check Exists
			if (clientTopUnit == null)
			{
				ViewData["ActionMethod"] = "ListSubMenu";
				return View("RecordDoesNotExistError");
			}

			clientTopUnitImportStep1WithFileVM.ClientTopUnit = clientTopUnit;
			clientTopUnitImportStep1WithFileVM.ClientTopUnitGuid = id;

			return View(clientTopUnitImportStep1WithFileVM);
		}

		[HttpPost]
		public ActionResult ImportStep1(ClientTopUnitImportStep1WithFileVM csvfile)
		{
			//used for return only
			ClientTopUnit clientTopUnit = new ClientTopUnit();
			clientTopUnit = clientTopUnitRepository.GetClientTopUnit(csvfile.ClientTopUnitGuid);
			csvfile.ClientTopUnit = clientTopUnit;

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
				ClientTopUnitImportStep2VM preImportCheckResult = new ClientTopUnitImportStep2VM();
				List<string> returnMessages = new List<string>();

				preImportCheckResult = clientTopUnitClientLocationRepository.PreImportCheck(csvfile.File, csvfile.ClientTopUnitGuid);

				ClientTopUnitImportStep1VM preImportCheckResultVM = new ClientTopUnitImportStep1VM();
				preImportCheckResultVM.ClientTopUnit = clientTopUnit;
				preImportCheckResultVM.ImportStep2VM = preImportCheckResult;
				preImportCheckResultVM.ClientTopUnitGuid = csvfile.ClientTopUnitGuid;

				TempData["PreImportCheckResultVM"] = preImportCheckResultVM;
				return RedirectToAction("ImportStep2");
			}

			return View();
		}

		public ActionResult ImportStep2()
		{
			ClientTopUnitImportStep1VM preImportCheckResultVM = new ClientTopUnitImportStep1VM();
            preImportCheckResultVM = (ClientTopUnitImportStep1VM)TempData["PreImportCheckResultVM"];
            if (preImportCheckResultVM != null)
            {
                ClientTopUnit clientTopUnit = new ClientTopUnit();
                clientTopUnit = clientTopUnitRepository.GetClientTopUnit(preImportCheckResultVM.ClientTopUnitGuid);
                preImportCheckResultVM.ClientTopUnit = clientTopUnit;
            }
            else
            {
                return View("Error");
            }

            return View(preImportCheckResultVM);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult ImportStep2(ClientTopUnitImportStep1VM preImportCheckResultVM)
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
			ClientTopUnitImportStep2VM preImportCheckResult = new ClientTopUnitImportStep2VM();
			preImportCheckResult = preImportCheckResultVM.ImportStep2VM;

			//Do the Import, return results
			ClientTopUnitImportStep3VM cdrPostImportResult = new ClientTopUnitImportStep3VM();
			cdrPostImportResult = clientTopUnitClientLocationRepository.Import(
				preImportCheckResult.FileBytes,
				preImportCheckResultVM.ClientTopUnit.ClientTopUnitGuid
			);

			cdrPostImportResult.ClientTopUnitGuid = preImportCheckResultVM.ClientTopUnit.ClientTopUnitGuid;
			TempData["CdrPostImportResult"] = cdrPostImportResult;

			//Pass Results to Next Page
			return RedirectToAction("ImportStep3");

		}

		public ActionResult ImportStep3()
		{
			//Display Results of Import
			ClientTopUnitImportStep3VM cdrPostImportResult = new ClientTopUnitImportStep3VM();
            cdrPostImportResult = (ClientTopUnitImportStep3VM)TempData["CdrPostImportResult"];
            if (cdrPostImportResult != null)
            {
                ClientTopUnit clientTopUnit = new ClientTopUnit();

                if (cdrPostImportResult.ClientTopUnitGuid != null)
                {
                    clientTopUnit = clientTopUnitRepository.GetClientTopUnit(cdrPostImportResult.ClientTopUnitGuid);
                    cdrPostImportResult.ClientTopUnit = clientTopUnit;
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
