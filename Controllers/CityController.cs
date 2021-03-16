using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Repository;
using CWTDesktopDatabase.ViewModels;

namespace CWTDesktopDatabase.Controllers
{
    public class CityController : Controller
    {
        CityRepository cityRepository = new CityRepository();
		CountryRepository countryRepository = new CountryRepository();
		StateProvinceRepository stateProvinceRepository = new StateProvinceRepository();
		HierarchyRepository hierarchyRepository = new HierarchyRepository();
        TimeZoneRuleRepository timeZoneRuleRepository = new TimeZoneRuleRepository();

        private string groupName = "System Data Administrator";

        //GET: List
		[HttpGet]
		public ActionResult List(string filter, int? page, string sortField, int? sortOrder)
        {

            //Set Access Rights
			ViewData["Access"] = "";
			RolesRepository rolesRepository = new RolesRepository();
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Access"] = "WriteAccess";
			}

            //SortField + SortOrder settings
            if (string.IsNullOrEmpty(sortField))
            {
                sortField = "Name";
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

            var items = cityRepository.PageCities(page ?? 1, filter ?? "", sortField, sortOrder ?? 0);
            return View(items);
        }

        // GET: View
		[HttpGet]
        public ActionResult View(string id)
        {
            City city = new City();
            city = cityRepository.GetCity(id);

            //Check Exists
            if (city == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            cityRepository.EditItemForDisplay(city);
            return View(city);
        }

		// GET: /Create
		[HttpGet]
		public ActionResult Create()
		{
			//Create Item 
			City city = new City();
			
			//AccessRights
			RolesRepository rolesRepository = new RolesRepository();
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//StateProvince SelectList
			SelectList stateProvinceList = new SelectList(stateProvinceRepository.GetStateProvincesByCountryCode(city.CountryCode).ToList(), "StateProvinceCode", "Name", city.StateProvinceCode);
			ViewData["StateProvinceList"] = stateProvinceList;

            //TimeZoneRules
            ViewData["TimeZoneRules"] = new SelectList(timeZoneRuleRepository.GetAllTimeZoneRules().ToList(), "TimeZoneRuleCode", "TimeZoneRuleCodeDesc");

            return View(city);
		}

		// POST: //Create/
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(City city)
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
				UpdateModel(city);
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
				cityRepository.Add(city);
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
		public ActionResult Edit(string id)
		{
			//Get Item 
			City city = new City();
			city = cityRepository.GetCity(id);

			//Check Exists
			if (city == null)
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

			//StateProvince SelectList
			SelectList stateProvinceList = new SelectList(stateProvinceRepository.GetStateProvincesByCountryCode(city.CountryCode).ToList(), "StateProvinceCode", "Name", city.StateProvinceCode);
			ViewData["StateProvinceList"] = stateProvinceList;

            //TimeZoneRules
            ViewData["TimeZoneRules"] = new SelectList(timeZoneRuleRepository.GetAllTimeZoneRules().ToList(), "TimeZoneRuleCode", "TimeZoneRuleCodeDesc", city.TimeZoneRuleCode);

            cityRepository.EditItemForDisplay(city);

			return View(city);
		}

		// POST: //Edit/
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(City city)
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
				UpdateModel(city);
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
				cityRepository.Update(city);
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
		public ActionResult Delete(string id)
		{
			CityVM cityVM = new CityVM();
			cityVM.AllowDelete = true;

			City city = new City();
			city = cityRepository.GetCity(id);

			//Check Exists
			if (city == null)
			{
				ViewData["ActionMethod"] = "ViewGet";
				return View("RecordDoesNotExistError");
			}

			cityVM.City = city;

			//Attached Items
			List<CityReference> supplierReferences = cityRepository.GetCityReferences(city.CityCode);
			if (supplierReferences.Count > 0)
			{
				cityVM.AllowDelete = false;
				cityVM.CityReferences = supplierReferences;
			}

			cityRepository.EditItemForDisplay(city);

			return View(cityVM);
		}

		// POST: /Delete
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(CityVM cityVM)
		{
			//Get Item 
			City city = new City();
			city = cityRepository.GetCity(cityVM.City.CityCode);

			//Check Exists
			if (cityVM.City == null)
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
				cityRepository.Delete(city);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/City.mvc/Delete/" + city.CityCode;
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
