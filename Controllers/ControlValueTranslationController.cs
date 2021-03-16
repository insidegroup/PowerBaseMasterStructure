using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Repository;
using System.Data.SqlClient;

namespace CWTDesktopDatabase.Controllers
{
    public class ControlValueTranslationController : Controller
    {
        //main repositories
        ControlValueRepository controlValueRepository = new ControlValueRepository();
        ControlValueLanguageRepository controlValueLanguageRepository = new ControlValueLanguageRepository();

        // GET: /Create
        public ActionResult Create(int id)
        {
            //Get ControlValue
            ControlValue controlValue = new ControlValue();
            controlValue = controlValueRepository.GetControlValue(id);

            //Check Exists
            if (controlValue == null)
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
            ViewData["ControlValueId"] = id;
            ViewData["ControlValue"] = controlValue.ControlValue1;

            //New ControlValueLanguage
            ControlValueLanguage controlValueLanguage = new ControlValueLanguage();
            controlValueLanguage.ControlValueId = id;
            controlValueLanguageRepository.EditItemForDisplay(controlValueLanguage);

            //Language SelectList
            SelectList languageList = new SelectList(controlValueLanguageRepository.GetUnUsedLanguages(id).ToList(), "LanguageCode", "LanguageName");
            ViewData["Languages"] = languageList;

            //Show Create Form
            return View(controlValueLanguage);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ControlValueLanguage controlValueLanguage)
        {
            ControlValue controlValue = new ControlValue();
            controlValue = controlValueRepository.GetControlValue(controlValueLanguage.ControlValueId);

            //Check Exists
            if (controlValue == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

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
                UpdateModel(controlValueLanguage);
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

            controlValueLanguageRepository.Add(controlValueLanguage);

            return RedirectToAction("List", new { id = controlValue.ControlValueId });
        }

        //GET:List
        public ActionResult List(int id, int? page, string sortField, int? sortOrder)
        {
            //Get PolicyAirVendorGroupItem
            ControlValue controlValue = new ControlValue();
            controlValue = controlValueRepository.GetControlValue(id);

            //Check Exists
            if (controlValue == null)
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

            //SortField+SortOrder settings
            if (sortField !="ControlValueTranslation")
            {
                sortField = "LanguageName";
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

            ViewData["ControlValueId"] = controlValue.ControlValueId;

            //return items
            var cwtPaginatedList = controlValueLanguageRepository.PageControlValueLanguages(id, page ?? 1, sortField, sortOrder ?? 0);
            return View(cwtPaginatedList);
        }

        //GET: View
        public ActionResult View(int id, string languageCode)
        {
            ControlValueLanguage controlValueLanguage = new ControlValueLanguage();
            controlValueLanguage = controlValueLanguageRepository.GetItem(id, languageCode);
            if (controlValueLanguage == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }
            controlValueLanguageRepository.EditItemForDisplay(controlValueLanguage);
            return View(controlValueLanguage);
        }

        // GET: /Edit
        public ActionResult Edit(int id, string languageCode)
        {
            //Get Item 
            ControlValueLanguage controlValueLanguage = new ControlValueLanguage();
            controlValueLanguage = controlValueLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (controlValueLanguage == null)
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
            ControlValue controlValue = new ControlValue();
            controlValue = controlValueRepository.GetControlValue(id);
            ViewData["ControlValueId"] = id;
            ViewData["ControlValue"] = controlValue.ControlValue1;

            //Language SelectList
            SelectList languageList = new SelectList(controlValueLanguageRepository.GetUnUsedLanguages(id).ToList(), "LanguageCode", "LanguageName");
            ViewData["Languages"] = languageList;

            controlValueLanguageRepository.EditItemForDisplay(controlValueLanguage);
            return View(controlValueLanguage);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, string languageCode, string controlValueTranslation)
        {
             //Get Item 
            ControlValueLanguage controlValueLanguage = new ControlValueLanguage();
            controlValueLanguage = controlValueLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (controlValueLanguage == null)
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
                UpdateModel(controlValueLanguage);
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
                controlValueLanguageRepository.Update(controlValueLanguage);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/ControlValueTranslation.mvc/Edit/" + controlValueLanguage.ControlValueId + "/" + controlValueLanguage.LanguageCode;
                    return View("VersionError");
                }

                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");            }

            //Update AirlineAdvice
            try
            {
                UpdateModel(controlValueLanguage);
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


            return RedirectToAction("List", new { id = controlValueLanguage.ControlValueId });
        }

        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id, string languageCode)
        {
            ControlValueLanguage controlValueLanguage = new ControlValueLanguage();
            controlValueLanguage = controlValueLanguageRepository.GetItem(id, languageCode);
            if (controlValueLanguage == null)
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

            controlValueLanguageRepository.EditItemForDisplay(controlValueLanguage);
            return View(controlValueLanguage);
        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, string languageCode, FormCollection collection)
        {
            ControlValueLanguage controlValueLanguage = new ControlValueLanguage();
            controlValueLanguage = controlValueLanguageRepository.GetItem(id, languageCode);
            if (controlValueLanguage == null)
            {
                //Doesn't Exist Error
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
                 controlValueLanguage.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                 controlValueLanguageRepository.Delete(controlValueLanguage);
             }
             catch (SqlException ex)
             {
                 //Versioning Error - go to standard versionError page
                 if (ex.Message == "SQLVersioningError")
                 {
                     ViewData["ReturnURL"] = "/ControlValueTranslation.mvc/Delete/" + controlValueLanguage.ControlValueId.ToString() + "/" + controlValueLanguage.LanguageCode;
                     return View("VersionError");
                 }
                 //Generic Error
                 return View("Error");
             }

            //Return
            return RedirectToAction("List", new { id = controlValueLanguage.ControlValueId });
        }
    }
}
