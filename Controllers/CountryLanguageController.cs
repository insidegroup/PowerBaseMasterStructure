using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Repository;
using System.Data.SqlClient;

namespace CWTDesktopDatabase.Controllers
{
    public class CountryLanguageController : Controller
    {
        CountryRepository countryRepository = new CountryRepository();
        CountryLanguageRepository countryLanguageRepository = new CountryLanguageRepository();

        //GET:List
        public ActionResult List(string id, int? page, string sortField, int? sortOrder)
        {
            //Get Country
            Country country = new Country();
            country = countryRepository.GetCountry(id);

            //Check Exists
            if (country == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }

            //Set Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToReferenceInfo())
            {
                ViewData["Access"] = "WriteAccess";
            }

            //Parent data
            ViewData["CountryCode"] = id;
            ViewData["CountryName"] = country.CountryName;

            //SortField+SortOrder settings
            if (sortField != "CountryName")
            {
                sortField = "LanguageName";
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
                sortOrder = 0;
            }

            //Get data
            var cwtPaginatedList = countryLanguageRepository.PageCountryLanguages(id, page ?? 1, sortField, sortOrder ?? 0);
            return View(cwtPaginatedList);
        }

        // GET: /Create
        public ActionResult Create(string id)
        {
            //Get Country
            Country country = new Country();
            country = countryRepository.GetCountry(id);

            //Check Exists
            if (country == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToReferenceInfo())
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Parent data
            ViewData["CountryCode"] = id;
            ViewData["CountryName"] = country.CountryName;

            //New CountryLanguage
            CountryLanguage countryLanguage = new CountryLanguage();
            countryLanguage.CountryCode = id;
            countryLanguageRepository.EditItemForDisplay(countryLanguage);

            //Language SelectList
            SelectList languageList = new SelectList(countryLanguageRepository.GetUnUsedLanguages(id).ToList(), "LanguageCode", "LanguageName");
            ViewData["Languages"] = languageList;

            //Show Create Form
            return View(countryLanguage);


        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CountryLanguage countryLanguage)
        {
            Country country = new Country();
            country = countryRepository.GetCountry(countryLanguage.CountryCode);

            //Check Exists
            if (country == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToReferenceInfo())
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update  Model from Form
            try
            {
                UpdateModel(countryLanguage);
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


            countryLanguageRepository.Add(countryLanguage);

            return RedirectToAction("List", new { id = countryLanguage.CountryCode });
        }

        // GET: /Edit
        public ActionResult Edit(string id, string languageCode)
        {
            //Get Item 
            CountryLanguage countryLanguage = new CountryLanguage();
            countryLanguage = countryLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (countryLanguage == null)
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

            //Parent data
            ViewData["CountryCode"] = id;
            ViewData["CountryName"] = countryLanguage.Country.CountryName;

            //Language SelectList
            SelectList languageList = new SelectList(countryLanguageRepository.GetUnUsedLanguages(id).ToList(), "LanguageCode", "LanguageName");
            ViewData["Languages"] = languageList;

            countryLanguageRepository.EditItemForDisplay(countryLanguage);
            return View(countryLanguage);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, string languageCode, FormCollection formCollection)
        {
            //Get Item 
            CountryLanguage countryLanguage = new CountryLanguage();
            countryLanguage = countryLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (countryLanguage == null)
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
                UpdateModel(countryLanguage);
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



            //Update AirlineAdvice
            try
            {
                countryLanguageRepository.Update(countryLanguage);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/CountryLanguage.mvc/Edit?id=" + countryLanguage.CountryCode + "&languageCode=" + countryLanguage.LanguageCode;
                    return View("VersionError");
                }

                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List", new { id = countryLanguage.CountryCode });
        }

        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(string id, string languageCode)
        {
            //Get Item 
            CountryLanguage countryLanguage = new CountryLanguage();
            countryLanguage = countryLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (countryLanguage == null)
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

            //Parent data
            ViewData["CountryCode"] = id;
            ViewData["CountryName"] = countryLanguage.Country.CountryName;

            countryLanguageRepository.EditItemForDisplay(countryLanguage);
            return View(countryLanguage);

        }


        // GET: /Delete
        public ActionResult ViewItem(string id, string languageCode)
        {
            //Get Item 
            CountryLanguage countryLanguage = new CountryLanguage();
            countryLanguage = countryLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (countryLanguage == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }


            //Parent data
            ViewData["CountryCode"] = id;
            ViewData["CountryName"] = countryLanguage.Country.CountryName;


            countryLanguageRepository.EditItemForDisplay(countryLanguage);
            return View(countryLanguage);

        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, string languageCode, FormCollection collection)
        {
            //Get Item 
            CountryLanguage countryLanguage = new CountryLanguage();
            countryLanguage = countryLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (countryLanguage == null)
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

            //Delete Item
            try
            {
                countryLanguage.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                countryLanguageRepository.Delete(countryLanguage);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/CountryLanguage.mvc/Delete?id=" + countryLanguage.CountryCode + "&languageCode=" + countryLanguage.LanguageCode;
                    return View("VersionError");
                }
                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            //Return
            return RedirectToAction("List", new { id = countryLanguage.CountryCode });
        }
    }
}
