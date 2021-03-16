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
    public class CityLanguageController : Controller
    {
        CityRepository cityRepository = new CityRepository();
        CityLanguageRepository cityLanguageRepository = new CityLanguageRepository();

        private string groupName = "System Data Administrator";

        //GET:List
        public ActionResult List(string id, int? page, string sortField, int? sortOrder)
        {
            //Get City
            City city = new City();
            city = cityRepository.GetCity(id);

            //Check Exists
            if (city == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }

            //Set Access Rights
            HierarchyRepository hierarchyRepository = new HierarchyRepository();
            ViewData["Access"] = "";
            if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //Parent data
            ViewData["CityCode"] = id;
            ViewData["CityName"] = city.Name;

            //SortField+SortOrder settings
            if (sortField != "CityName")
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
            var cwtPaginatedList = cityLanguageRepository.PageCityLanguages(id, page ?? 1, sortField, sortOrder ?? 0);
            return View(cwtPaginatedList);
        }

        // GET: /Create
        public ActionResult Create(string id)
        {
            //Get City
            City city = new City();
            city = cityRepository.GetCity(id);

            //Check Exists
            if (city == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            //AccessRights
            HierarchyRepository hierarchyRepository = new HierarchyRepository();
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Parent data
            ViewData["CityCode"] = id;
            ViewData["CityName"] = city.Name;

            //New CityLanguage
            CityLanguage cityLanguage = new CityLanguage();
            cityLanguage.CityCode = id;
            cityLanguageRepository.EditItemForDisplay(cityLanguage);

            //Language SelectList
            SelectList languageList = new SelectList(cityLanguageRepository.GetUnUsedLanguages(id).ToList(), "LanguageCode", "LanguageName");
            ViewData["Languages"] = languageList;

            //Show Create Form
            return View(cityLanguage);


        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CityLanguage cityLanguage)
        {
            City city = new City();
            city = cityRepository.GetCity(cityLanguage.CityCode);

            //Check Exists
            if (city == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            HierarchyRepository hierarchyRepository = new HierarchyRepository();
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update  Model from Form
            try
            {
                UpdateModel(cityLanguage);
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


            cityLanguageRepository.Add(cityLanguage);

            return RedirectToAction("List", new { id = cityLanguage.CityCode });
        }

        // GET: /Edit
        public ActionResult Edit(string id, string languageCode)
        {
            //Get Item 
            CityLanguage cityLanguage = new CityLanguage();
            cityLanguage = cityLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (cityLanguage == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //AccessRights
            HierarchyRepository hierarchyRepository = new HierarchyRepository();
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Parent data
            ViewData["CityCode"] = id;
            ViewData["CityName"] = cityLanguage.City.Name;

            //Language SelectList
            SelectList languageList = new SelectList(cityLanguageRepository.GetUnUsedLanguages(id).ToList(), "LanguageCode", "LanguageName");
            ViewData["Languages"] = languageList;

            cityLanguageRepository.EditItemForDisplay(cityLanguage);
            return View(cityLanguage);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, string languageCode, FormCollection formCollection)
        {
            //Get Item 
            CityLanguage cityLanguage = new CityLanguage();
            cityLanguage = cityLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (cityLanguage == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //AccessRights
            HierarchyRepository hierarchyRepository = new HierarchyRepository();
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update Item from Form
            try
            {
                UpdateModel(cityLanguage);
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
                cityLanguageRepository.Update(cityLanguage);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/CityLanguage.mvc/Edit?id=" + cityLanguage.CityCode + "&languageCode=" + cityLanguage.LanguageCode;
                    return View("VersionError");
                }

                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List", new { id = cityLanguage.CityCode });
        }

        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(string id, string languageCode)
        {
            //Get Item 
            CityLanguage cityLanguage = new CityLanguage();
            cityLanguage = cityLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (cityLanguage == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            //AccessRights
            HierarchyRepository hierarchyRepository = new HierarchyRepository();
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Parent data
            ViewData["CityCode"] = id;
            ViewData["CityName"] = cityLanguage.City.Name;
			ViewData["CountryName"] = cityLanguage.City.Country.CountryName;

            //Language SelectList
            SelectList languageList = new SelectList(cityLanguageRepository.GetUnUsedLanguages(id).ToList(), "LanguageCode", "LanguageName");
            ViewData["Languages"] = languageList;

            cityLanguageRepository.EditItemForDisplay(cityLanguage);
            return View(cityLanguage);

        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, string languageCode, FormCollection collection)
        {
            //Get Item 
            CityLanguage cityLanguage = new CityLanguage();
            cityLanguage = cityLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (cityLanguage == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //AccessRights
            HierarchyRepository hierarchyRepository = new HierarchyRepository();
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Delete Item
            try
            {
                cityLanguage.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                cityLanguageRepository.Delete(cityLanguage);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/CityLanguage.mvc/Delete?id=" + cityLanguage.CityCode + "&languageCode=" + cityLanguage.LanguageCode;
                    return View("VersionError");
                }
                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            //Return
            return RedirectToAction("List", new { id = cityLanguage.CityCode });
        }

        // GET: /View
        public ActionResult ViewItem(string id, string languageCode)
        {
            //Get Item 
            CityLanguage cityLanguage = new CityLanguage();
            cityLanguage = cityLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (cityLanguage == null)
            {
                ViewData["ActionMethod"] = "ViewItemGet";
                return View("RecordDoesNotExistError");
            }

            //Parent data
            ViewData["CityCode"] = id;
            ViewData["CityName"] = cityLanguage.City.Name;
			ViewData["CountryName"] = cityLanguage.City.Country.CountryName;

            //Language SelectList
            SelectList languageList = new SelectList(cityLanguageRepository.GetUnUsedLanguages(id).ToList(), "LanguageCode", "LanguageName");
            ViewData["Languages"] = languageList;

            cityLanguageRepository.EditItemForDisplay(cityLanguage);
            return View(cityLanguage);

        }
    }
}
