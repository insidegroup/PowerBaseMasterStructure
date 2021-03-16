using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Repository;
using System.Data.SqlClient;

namespace CWTDesktopDatabase.Controllers
{
    public class APISCountryController : Controller
    {
        //main repository
        APISCountryRepository apisCountryRepository = new APISCountryRepository();

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
            if (sortField != "DestinationCountryName" && sortField != "StartDate")
            {
                sortField = "OriginCountryName";
                ViewData["CurrentSortField"] = "OriginCountryName";
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
                sortOrder = 0;
            }

            //return items
            var cwtPaginatedList = apisCountryRepository.PageAPISCountries(page ?? 1, filter ?? "", sortField, sortOrder ?? 0);
            return View(cwtPaginatedList);
        }

        // GET: /Create
        public ActionResult Create()
        {
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

            APISCountry apisCountry = new APISCountry();
            return View(apisCountry);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(APISCountry apisCountry)
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
                UpdateModel(apisCountry);
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
                apisCountryRepository.Add(apisCountry);
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
        public ActionResult Edit(string occ, string  dcc)
        {
            //Check Exists
            APISCountry apisCountry = new APISCountry();
            apisCountry = apisCountryRepository.GetAPISCountry(occ, dcc);
            if (apisCountry == null)
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

            apisCountryRepository.EditForDisplay(apisCountry);
            return View(apisCountry);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string occ, string dcc, FormCollection collection)
        {

            //Get Item From Database
            APISCountry apisCountry = apisCountryRepository.GetAPISCountry(collection["OriginalOCC"], collection["OriginalDCC"]);

            //Check Exists
            if (apisCountry == null)
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
                UpdateModel(apisCountry);
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
                apisCountryRepository.Update(apisCountry, collection["OriginalOCC"], collection["OriginalDCC"]);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/APISCountry.mvc/Edit?occ=" + collection["OriginalOCC"] + "&dcc=" + collection["OriginalDCC"];
                    return View("VersionError");
                }

                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");            
            }

            //Success
            return RedirectToAction("List");

        }

        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(string occ, string dcc)
        {
            //Check Exists
            APISCountry apisCountry = new APISCountry();
            apisCountry = apisCountryRepository.GetAPISCountry(occ, dcc);
            if (apisCountry == null)
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

            apisCountryRepository.EditForDisplay(apisCountry);
            return View(apisCountry);
        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string originCountryCode, string destinationCountryCode, FormCollection collection)
        {
            //Get Item From Database
            APISCountry apisCountry = apisCountryRepository.GetAPISCountry(originCountryCode, destinationCountryCode);

            //Check Exists
            if (apisCountry == null)
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
                apisCountry.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                apisCountryRepository.Delete(apisCountry);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/APISCountry.mvc/Delete?occ=" + originCountryCode + "&dcc=" + destinationCountryCode;
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List");
        }
    }
}
