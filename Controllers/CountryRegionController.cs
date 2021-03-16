using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Repository;
using System.Data.SqlClient;
using System.Linq.Dynamic;

namespace CWTDesktopDatabase.Controllers
{
    public class CountryRegionController : Controller
    {
        //main repository
        CountryRegionRepository countryRegionRepository = new CountryRegionRepository();

        // GET: /List/
        public ActionResult List(string filter, int? page, string sortField, int? sortOrder)
        {
            //Set Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToReferenceInfo())
            {
                ViewData["Access"] = "WriteAccess";
            }

            ViewData["CurrentSortField"] = sortField;
            if (sortField !="CountryName")
            {
                sortField = "CountryRegionName";
                ViewData["CurrentSortField"] = "Default";
            }


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
            var cwtPaginatedList = countryRegionRepository.PageCountryRegions(page ?? 1, filter ?? "", sortField, sortOrder ?? 0);
            return View(cwtPaginatedList);
        }

        // GET: /List/
        public ActionResult ListLocations(string filter, int id, int? page, string sortField, int? sortOrder)
        {
            //Set Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToReferenceInfo())
            {
                ViewData["Access"] = "WriteAccess";
            }

            //only one field shown - should remove from ActionResult parameters too
            sortField = "LocationName"; 
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

            CountryRegion countryRegion = new CountryRegion();
            countryRegion = countryRegionRepository.GetCountryRegion(id);
            ViewData["CountryRegionName"] = countryRegion.CountryRegionName;
            ViewData["CountryRegionId"] = countryRegion.CountryRegionId;

            //return items
            var cwtPaginatedList = countryRegionRepository.PageCountryRegionLocations(page ?? 1, filter ?? "", sortField, sortOrder ?? 0, id);
            return View(cwtPaginatedList);
        }

        // GET: /View
        public ActionResult View(int id)
        {
            //Check Exists
            CountryRegion countryRegion = new CountryRegion();
            countryRegion = countryRegionRepository.GetCountryRegion(id);
            if (countryRegion == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            return View(countryRegion);
        }

        // GET: /Create
        public ActionResult Create()
        {
            CountryRepository countryRepository = new CountryRepository();
            SelectList countriesList = new SelectList(countryRepository.GetAllCountries().ToList(), "CountryCode", "CountryName");
            ViewData["Countries"] = countriesList;

            CountryRegion countryRegion = new CountryRegion();
            return View(countryRegion);
        }

        // POST: /Create/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CountryRegion countryRegion)
        {
            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToReferenceInfo())
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update  Model from Form
            try
            {
                UpdateModel(countryRegion);
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
                countryRegionRepository.Add(countryRegion);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");     
            }

            ViewData["NewSortOrder"] = 0;
            return RedirectToAction("List");
        }

        // GET: /Edit
        public ActionResult Edit(int id)
        {
            //Check Exists
            CountryRegion countryRegion = new CountryRegion();
            countryRegion = countryRegionRepository.GetCountryRegion(id);
            if (countryRegion == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }
            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToReferenceInfo())
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
            CountryRepository countryRepository = new CountryRepository();
            SelectList countriesList = new SelectList(countryRepository.GetAllCountries().ToList(), "CountryCode", "CountryName");
            ViewData["Countries"] = countriesList;

            return View(countryRegion);
        }


        // POST: /Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection collection)
        {
            
            //Get Item From Database
            CountryRegion countryRegion = countryRegionRepository.GetCountryRegion(id);

            //Check Exists
            if (countryRegion == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }
            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToReferenceInfo())
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
            //Update Item from Form
            try
            {
                UpdateModel(countryRegion);
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
                countryRegionRepository.Update(countryRegion);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/CountryRegion.mvc/Edit/" + countryRegion.CountryRegionId.ToString();
                    return View("VersionError");
                }

                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");            }

            //Success
            return RedirectToAction("List");

        }

        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id)
        {
            //Check Exists
            CountryRegion countryRegion = new CountryRegion();
            countryRegion = countryRegionRepository.GetCountryRegion(id);
            if (countryRegion == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }
            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToReferenceInfo())
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
            return View(countryRegion);
        }

        // POST: /Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            //Check Exists
            CountryRegion countryRegion = new CountryRegion();
            countryRegion = countryRegionRepository.GetCountryRegion(id);
            if (countryRegion == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }
            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToReferenceInfo())
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
            //Delete Item
            try
            {
                countryRegion.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                countryRegionRepository.Delete(countryRegion);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/CountryRegion.mvc/Delete/" + countryRegion.CountryRegionId.ToString();
                    return View("VersionError");
                }

                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List");
        }


        //Validation
        [HttpPost]
        public JsonResult IsAvailableCountryRegion(string countryRegionName, string countryCode, int? countryRegionId)
        {

            var result = countryRegionRepository.IsAvailableCountryRegion(countryRegionName, countryCode, countryRegionId);
            return Json(result);
        }

    }
}
