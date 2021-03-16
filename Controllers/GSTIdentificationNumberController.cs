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
    public class GSTIdentificationNumberController : Controller
    {
		GSTIdentificationNumberRepository gstIdentificationNumberRepository = new GSTIdentificationNumberRepository();

		HierarchyRepository hierarchyRepository = new HierarchyRepository();
		StateProvinceRepository stateProvinceRepository = new StateProvinceRepository();
        CountryRepository countryRepository = new CountryRepository();

		private string groupName = "GST Identification Number Administrator";
		
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
                sortField = "ClientTopUnitName";
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
            var cwtPaginatedList = gstIdentificationNumberRepository.PageGSTIdentificationNumbers(page ?? 1, filter ?? "", sortField, sortOrder ?? 0);
            return View(cwtPaginatedList);
        }

        // GET: /View
        public ActionResult ViewItem(int id)
        {
            //Check Exists
            GSTIdentificationNumber gstIdentificationNumber = new GSTIdentificationNumber();
            gstIdentificationNumber = gstIdentificationNumberRepository.GetGSTIdentificationNumber(id);
            if (gstIdentificationNumber == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            gstIdentificationNumberRepository.EditForDisplay(gstIdentificationNumber);
            return View(gstIdentificationNumber);
        }

		// GET: /Create
		[HttpGet]
		public ActionResult Create()
		{
			//Create Item 
			GSTIdentificationNumber gstIdentificationNumber = new GSTIdentificationNumber();

			//AccessRights
			RolesRepository rolesRepository = new RolesRepository();
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

            //CountryList SelectList
            SelectList countryList = new SelectList(countryRepository.GetCountriesbyRole(groupName).ToList(), "CountryCode", "CountryName");
            ViewData["CountryList"] = countryList;

            //StateProvince SelectList
            SelectList stateProvinceList = new SelectList(stateProvinceRepository.GetStateProvincesByCountryCode(gstIdentificationNumber.CountryCode).ToList(), "StateProvinceCode", "Name");
            ViewData["StateProvinceList"] = stateProvinceList;

            return View(gstIdentificationNumber);
		}

		// POST: //Create/
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(GSTIdentificationNumber gstIdentificationNumber)
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
				UpdateModel(gstIdentificationNumber);
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
				gstIdentificationNumberRepository.Add(gstIdentificationNumber);
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
			GSTIdentificationNumber gstIdentificationNumber = new GSTIdentificationNumber();
			gstIdentificationNumber = gstIdentificationNumberRepository.GetGSTIdentificationNumber(id);

			//Check Exists
			if (gstIdentificationNumber == null)
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

			gstIdentificationNumberRepository.EditForDisplay(gstIdentificationNumber);

            //CountryList SelectList
            SelectList countryList = new SelectList(countryRepository.GetCountriesbyRole(groupName).ToList(), "CountryCode", "CountryName", gstIdentificationNumber.CountryCode);
            ViewData["CountryList"] = countryList;

            //StateProvince SelectList
            SelectList stateProvinceList = new SelectList(stateProvinceRepository.GetStateProvincesByCountryCode(gstIdentificationNumber.CountryCode).ToList(), "StateProvinceCode", "Name", gstIdentificationNumber.StateProvinceCode);
			ViewData["StateProvinceList"] = stateProvinceList;

			return View(gstIdentificationNumber);
		}

		// POST: //Edit/
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(GSTIdentificationNumber gstIdentificationNumber)
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
				UpdateModel(gstIdentificationNumber);
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
				gstIdentificationNumberRepository.Update(gstIdentificationNumber);
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
			GSTIdentificationNumberVM gstIdentificationNumberVM = new GSTIdentificationNumberVM();

			GSTIdentificationNumber gstIdentificationNumber = new GSTIdentificationNumber();
			gstIdentificationNumber = gstIdentificationNumberRepository.GetGSTIdentificationNumber(id);

			//Check Exists
			if (gstIdentificationNumber == null)
			{
				ViewData["ActionMethod"] = "ViewGet";
				return View("RecordDoesNotExistError");
			}

            //Check Access Rights to Domain
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            gstIdentificationNumberRepository.EditForDisplay(gstIdentificationNumber);

            gstIdentificationNumberVM.GSTIdentificationNumber = gstIdentificationNumber;

            return View(gstIdentificationNumberVM);
		}

		// POST: /Delete
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(GSTIdentificationNumberVM gstIdentificationNumberVM)
		{
			//Get Item 
			GSTIdentificationNumber gstIdentificationNumber = new GSTIdentificationNumber();
			gstIdentificationNumber = gstIdentificationNumberRepository.GetGSTIdentificationNumber(gstIdentificationNumberVM.GSTIdentificationNumber.GSTIdentificationNumberId);

			//Check Exists
			if (gstIdentificationNumberVM.GSTIdentificationNumber == null)
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
				gstIdentificationNumberRepository.Delete(gstIdentificationNumber);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/GSTIdentificationNumber.mvc/Delete/" + gstIdentificationNumber.GSTIdentificationNumberId.ToString();
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
