using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Repository;
using System.Data.SqlClient;
using CWTDesktopDatabase.ViewModels;

namespace CWTDesktopDatabase.Controllers
{
    public class ClientTelephonyController : Controller
    {
		ClientTelephonyRepository clientTelephonyRepository = new ClientTelephonyRepository();
		TelephonyTypeRepository telephonyTypeRepository = new TelephonyTypeRepository();
		TravelerBackOfficeTypeRepository travelerBackOfficeTypeRepository = new TravelerBackOfficeTypeRepository();
		CountryRepository countryRepository = new CountryRepository();

		HierarchyRepository hierarchyRepository = new HierarchyRepository();

		private string groupName = "Client Telephony Group Administrator";
		
		// GET: /List
        public ActionResult List(string filter, int? page, string sortField, int? sortOrder)
        {
			//Set Access Rights
			ViewData["Access"] = "";
			RolesRepository rolesRepository = new RolesRepository();
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Access"] = "WriteAccess";
			}
			
			//SortField
            if (string.IsNullOrEmpty(sortField))
            {
				sortField = "PhoneNumber";
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
            }

            //return items
            var cwtPaginatedList = clientTelephonyRepository.PageClientTelephonies(page ?? 1, filter ?? "", sortField, sortOrder ?? 0);
            return View(cwtPaginatedList);
        }

        // GET: /View
        public ActionResult ViewItem(int id)
        {
            //Check Exists
            ClientTelephony clientTelephony = new ClientTelephony();
            clientTelephony = clientTelephonyRepository.GetClientTelephony(id);
            if (clientTelephony == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            clientTelephonyRepository.EditForDisplay(clientTelephony);
            return View(clientTelephony);
        }

		// GET: /Create
		[HttpGet]
		public ActionResult Create()
		{
			//Create Item 
			ClientTelephony clientTelephony = new ClientTelephony();

			//AccessRights
			RolesRepository rolesRepository = new RolesRepository();
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			ClientTelephonyVM clientTelephonyVM = new ClientTelephonyVM();

			clientTelephonyVM.ClientTelephony = clientTelephony;
			
			//Countries
			clientTelephonyVM.Countries = new SelectList(countryRepository.GetAllCountries().ToList(), "CountryCode", "CountryNameWithInternationalPrefixCode");
			
			//HierarchyTypes
			clientTelephonyVM.HierarchyTypes = new SelectList(clientTelephonyRepository.GetAllHierarchyTypes().ToList(), "Value", "Text");

            //TelephoneTypes
            clientTelephonyVM.TelephoneTypes = new SelectList(telephonyTypeRepository.GetAllTelephonyTypes().ToList(), "TelephonyTypeId", "TelephonyTypeDescription");

            //TravelerBackOfficeTypes
            clientTelephonyVM.TravelerBackOfficeTypes = new SelectList(travelerBackOfficeTypeRepository.GetAllTravelerBackOfficeTypes().ToList(), "TravelerBackOfficeTypeCode", "TravelerBackOfficeTypeDescription");

            //CallerEnteredDigitDefinitionTypes
            CallerEnteredDigitDefinitionTypeRepository callerEnteredDigitDefinitionTypeRepository = new CallerEnteredDigitDefinitionTypeRepository();
            clientTelephonyVM.CallerEnteredDigitDefinitionTypes = new SelectList(callerEnteredDigitDefinitionTypeRepository.GetAllCallerEnteredDigitDefinitionTypes().ToList(), "CallerEnteredDigitDefinitionTypeId", "CallerEnteredDigitDefinitionTypeDescription");

            return View(clientTelephonyVM);
		}

		// POST: //Create/
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(ClientTelephony clientTelephony)
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Update  Model from Form
			try
			{
				UpdateModel(clientTelephony);
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
				clientTelephonyRepository.Add(clientTelephony);
			}
			catch (SqlException ex)
			{
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("List");
		}

		// GET: /Edit
		public ActionResult Edit(int id)
		{
			//Get Item 
			ClientTelephony clientTelephony = new ClientTelephony();
			clientTelephony = clientTelephonyRepository.GetClientTelephony(id);

			//Check Exists
			if (clientTelephony == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			//AccessRights
			RolesRepository rolesRepository = new RolesRepository();
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			ClientTelephonyVM clientTelephonyVM = new ClientTelephonyVM();

			clientTelephonyVM.ClientTelephony = clientTelephony;

			//Countries
			clientTelephonyVM.Countries = new SelectList(countryRepository.GetAllCountries().ToList(), "CountryCode", "CountryNameWithInternationalPrefixCode", clientTelephony.CountryCode);

			//HierarchyTypes
			clientTelephonyVM.HierarchyTypes = new SelectList(clientTelephonyRepository.GetAllHierarchyTypes().ToList(), "Value", "Text", clientTelephony.HierarchyType);

			//TelephoneTypes
			clientTelephonyVM.TelephoneTypes = new SelectList(telephonyTypeRepository.GetAllTelephonyTypes().ToList(), "TelephonyTypeId", "TelephonyTypeDescription", clientTelephony.TelephonyTypeId);

			//TravelerBackOfficeTypes
			clientTelephonyVM.TravelerBackOfficeTypes = new SelectList(travelerBackOfficeTypeRepository.GetAllTravelerBackOfficeTypes().ToList(), "TravelerBackOfficeTypeCode", "TravelerBackOfficeTypeDescription", clientTelephony.TravelerBackOfficeTypeCode);

            //CallerEnteredDigitDefinitionTypes
            CallerEnteredDigitDefinitionTypeRepository callerEnteredDigitDefinitionTypeRepository = new CallerEnteredDigitDefinitionTypeRepository();
            clientTelephonyVM.CallerEnteredDigitDefinitionTypes = new SelectList(callerEnteredDigitDefinitionTypeRepository.GetAllCallerEnteredDigitDefinitionTypes().ToList(), "CallerEnteredDigitDefinitionTypeId", "CallerEnteredDigitDefinitionTypeDescription", clientTelephony.CallerEnteredDigitDefinitionTypeId);

            clientTelephonyRepository.EditForDisplay(clientTelephony);

			return View(clientTelephonyVM);
		}

		// POST: //Edit/
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(ClientTelephony clientTelephony)
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Update  Model from Form
			try
			{
				UpdateModel(clientTelephony);
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
				clientTelephonyRepository.Update(clientTelephony);
			}
			catch (SqlException ex)
			{
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("List");
		}

		// GET: Delete
		[HttpGet]
		public ActionResult Delete(int id)
		{
			ClientTelephonyVM clientTelephonyVM = new ClientTelephonyVM();

			ClientTelephony clientTelephony = new ClientTelephony();
			clientTelephony = clientTelephonyRepository.GetClientTelephony(id);

			//Check Exists
			if (clientTelephony == null)
			{
				ViewData["ActionMethod"] = "ViewGet";
				return View("RecordDoesNotExistError");
			}

			clientTelephonyVM.ClientTelephony = clientTelephony;

			clientTelephonyRepository.EditForDisplay(clientTelephony);

			return View(clientTelephonyVM);
		}

		// POST: /Delete
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(ClientTelephonyVM clientTelephonyVM)
		{
			//Get Item 
			ClientTelephony clientTelephony = new ClientTelephony();
			clientTelephony = clientTelephonyRepository.GetClientTelephony(clientTelephonyVM.ClientTelephony.ClientTelephonyId);

			//Check Exists
			if (clientTelephonyVM.ClientTelephony == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Delete Item
			try
			{
				clientTelephonyRepository.Delete(clientTelephony);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/ClientTelephony.mvc/Delete/" + clientTelephony.ClientTelephonyId;
					return View("VersionError");
				}

				//Generic Error
				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			//Return
			return RedirectToAction("List");
		}
    }
}
