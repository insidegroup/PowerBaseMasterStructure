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
    public class TravelPortLanguageController : Controller
    {
        TravelPortRepository travelPortRepository = new TravelPortRepository();
        TravelPortLanguageRepository travelPortLanguageRepository = new TravelPortLanguageRepository();

        private string groupName = "System Data Administrator";

        // GET: /List
        public ActionResult List(string id, int? page, string sortField, int? sortOrder)
        {
            //Check Exists
            TravelPort travelPort = new TravelPort();
            travelPort = travelPortRepository.GetTravelPort(id);
            if (travelPort == null)
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

            //SortField
            if (sortField != "TravelPortTypeDescription" && sortField != "LanguageName" && sortField != "TravelPortCode")
            {
                sortField = "TravelPortName";
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

            //Parent Information
            ViewData["TravelPortCode"] = travelPort.TravelPortCode;
            ViewData["TravelPortName"] = travelPort.TravelportName;

            //return items
            var cwtPaginatedList = travelPortLanguageRepository.PageTravelPortTranslations(id, page ?? 1, sortField, sortOrder ?? 0);
            return View(cwtPaginatedList);


        }

        // GET: /Create
        public ActionResult Create(string id)
        {
            //Get Parent
            TravelPort travelPort = new TravelPort();
            travelPort = travelPortRepository.GetTravelPort(id);

            //Check Exists
            if (travelPort == null)
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

            //New TravelPortLanguage
            TravelPortLanguage travelPortLanguage = new TravelPortLanguage();
            travelPortLanguage.TravelPortCode = id;
            travelPortLanguageRepository.EditForDisplay(travelPortLanguage);

            //Populate List of Languages
            LanguageRepository languageRepository = new LanguageRepository();
            SelectList languages = new SelectList(languageRepository.GetAllLanguages().ToList(), "LanguageCode", "LanguageName");
            ViewData["LanguageList"] = languages;

            //Populate List of Languages
            TravelPortTypeRepository travelPortTypeRepository = new TravelPortTypeRepository();
            SelectList travelPortTypes = new SelectList(travelPortTypeRepository.GetAllTravelPortTypes().ToList(), "TravelPortTypeId", "TravelPortTypeDescription");
            ViewData["TravelPortTypeList"] = travelPortTypes;

            //Show Create Form
            return View(travelPortLanguage);
        }

        // POST: //Create/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TravelPortLanguage travelPortLanguage)
        {

            //AccessRights
            HierarchyRepository hierarchyRepository = new HierarchyRepository();
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update  Model from Form
            try
            {
                UpdateModel(travelPortLanguage);
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
                travelPortLanguageRepository.Add(travelPortLanguage);
            }
            catch
            {
                //Could not insert to database
                return View("Error");
            }

            ViewData["NewSortOrder"] = 0;
            return RedirectToAction("List", new { id = travelPortLanguage.TravelPortCode });
        }

        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(string id, int travelPortTypeId, string languageCode)
        {
            //Get Item 
            TravelPortLanguage travelPortLanguage = new TravelPortLanguage();
            travelPortLanguage = travelPortLanguageRepository.GetTravelPortLanguage(id, travelPortTypeId, languageCode);

            //Check Exists
            if (travelPortLanguage == null)
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

            //Return View
            travelPortLanguageRepository.EditForDisplay(travelPortLanguage);
            return View(travelPortLanguage);

        }


        // GET: /Edit
        public ActionResult Edit(string id, int travelPortTypeId, string languageCode)
        {
            //Get Item 
            TravelPortLanguage travelPortLanguage = new TravelPortLanguage();
            travelPortLanguage = travelPortLanguageRepository.GetTravelPortLanguage(id, travelPortTypeId, languageCode);

            //Check Exists
            if (travelPortLanguage == null)
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

            //Return View
            travelPortLanguageRepository.EditForDisplay(travelPortLanguage);
            return View(travelPortLanguage);

        }

        // POST: /Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, int travelPortTypeId, string languageCode, FormCollection collection)
        {

            //Get Item 
            TravelPortLanguage travelPortLanguage = new TravelPortLanguage();
            travelPortLanguage = travelPortLanguageRepository.GetTravelPortLanguage(id, travelPortTypeId, languageCode);

            //Check Exists
            if (travelPortLanguage == null)
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
            //Update Item from Form
            try
            {
                UpdateModel(travelPortLanguage);
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
                travelPortLanguageRepository.Update(travelPortLanguage);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/TravelPortLanguage.mvc/Edit/?id=" + travelPortLanguage.TravelPortCode + "&languageCode=" + travelPortLanguage.LanguageCode + "&travelPortTypeId=" + travelPortLanguage.TravelPortTypeId;
                    return View("VersionError");
                }

                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");            }

            //Success
            return RedirectToAction("List", new { id = travelPortLanguage.TravelPortCode });

        }

        // POST: /Delete/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, int travelPortTypeId, string languageCode, FormCollection collection)
        {

            //Get Item 
            TravelPortLanguage travelPortLanguage = new TravelPortLanguage();
            travelPortLanguage = travelPortLanguageRepository.GetTravelPortLanguage(id, travelPortTypeId, languageCode);

            //Check Exists
            if (travelPortLanguage == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
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
                travelPortLanguage.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                travelPortLanguageRepository.Delete(travelPortLanguage);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/TravelPortLanguage.mvc/Delete/?id=" + travelPortLanguage.TravelPortCode + "&languageCode=" + travelPortLanguage.LanguageCode + "&travelPortTypeId=" + travelPortLanguage.TravelPortTypeId;
                    return View("VersionError");
                }
                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");            }

            //Success
            return RedirectToAction("List", new { id = travelPortLanguage.TravelPortCode });

        }

    }
}
