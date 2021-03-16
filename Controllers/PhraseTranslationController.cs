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
    public class PhraseTranslationController : Controller
    {
        PhraseRepository phraseRepository = new PhraseRepository();
        PhraseTranslationRepository phraseLanguageRepository = new PhraseTranslationRepository();

        //GET:List
        public ActionResult List(int? page, string sortField, int? sortOrder, int id = 0)
        {
            //Get Phrase
            Phrase phrase = new Phrase();
            phrase = phraseRepository.GetPhrase(id);

            //Check Exists
            if (phrase == null)
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
            ViewData["PhraseId"] = id;
            ViewData["PhraseName"] = phrase.PhraseName;

            //SortField+SortOrder settings
            if (sortField != "PhraseName")
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
            var cwtPaginatedList = phraseLanguageRepository.PagePhraseTranslations(id, page ?? 1, sortField, sortOrder ?? 0);
            return View(cwtPaginatedList);
        }

        // GET: /Create
        public ActionResult Create(int id)
        {
            //Get Phrase
            Phrase phrase = new Phrase();
            phrase = phraseRepository.GetPhrase(id);

            //Check Exists
            if (phrase == null)
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
            ViewData["PhraseId"] = id;
            ViewData["PhraseName"] = phrase.PhraseName;

            //New PhraseTranslation
            PhraseTranslation phraseLanguage = new PhraseTranslation();
            phraseLanguage.PhraseId = id;
            phraseLanguageRepository.EditItemForDisplay(phraseLanguage);

            //Language SelectList
            SelectList languageList = new SelectList(phraseLanguageRepository.GetUnUsedLanguages(id).ToList(), "LanguageCode", "LanguageName");
            ViewData["Languages"] = languageList;

            //Show Create Form
            return View(phraseLanguage);


        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PhraseTranslation phraseLanguage)
        {
            Phrase phrase = new Phrase();
            phrase = phraseRepository.GetPhrase(phraseLanguage.PhraseId);

            //Check Exists
            if (phrase == null)
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
                UpdateModel(phraseLanguage);
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


            phraseLanguageRepository.Add(phraseLanguage);

            return RedirectToAction("List", new { id = phraseLanguage.PhraseId });
        }

        // GET: /Edit
        public ActionResult Edit(int id, string languageCode)
        {
            //Get Item 
            PhraseTranslation phraseLanguage = new PhraseTranslation();
            phraseLanguage = phraseLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (phraseLanguage == null)
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
            ViewData["PhraseId"] = id;
            ViewData["PhraseName"] = phraseLanguage.Phrase.PhraseName;

            //Language SelectList
            SelectList languageList = new SelectList(phraseLanguageRepository.GetUnUsedLanguages(id).ToList(), "LanguageCode", "LanguageName");
            ViewData["Languages"] = languageList;

            phraseLanguageRepository.EditItemForDisplay(phraseLanguage);
            return View(phraseLanguage);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, string languageCode, FormCollection formCollection)
        {
            //Get Item 
            PhraseTranslation phraseLanguage = new PhraseTranslation();
            phraseLanguage = phraseLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (phraseLanguage == null)
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
                UpdateModel(phraseLanguage);
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
                phraseLanguageRepository.Update(phraseLanguage);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/PhraseTranslation.mvc/Edit?id=" + phraseLanguage.PhraseId.ToString() + "&languageCode=" + phraseLanguage.LanguageCode;
                    return View("VersionError");
                }

                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List", new { id = phraseLanguage.PhraseId });
        }

        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id, string languageCode)
        {
            //Get Item 
            PhraseTranslation phraseLanguage = new PhraseTranslation();
            phraseLanguage = phraseLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (phraseLanguage == null)
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
            ViewData["PhraseId"] = id;
            ViewData["PhraseName"] = phraseLanguage.Phrase.PhraseName;

            phraseLanguageRepository.EditItemForDisplay(phraseLanguage);
            return View(phraseLanguage);

        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, string languageCode, FormCollection collection)
        {
            //Get Item 
            PhraseTranslation phraseLanguage = new PhraseTranslation();
            phraseLanguage = phraseLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (phraseLanguage == null)
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
                phraseLanguage.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                phraseLanguageRepository.Delete(phraseLanguage);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/PhraseTranslation.mvc/Delete?id=" + phraseLanguage.PhraseId.ToString() + "&languageCode=" + phraseLanguage.LanguageCode;
                    return View("VersionError");
                }
                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            //Return
            return RedirectToAction("List", new { id = phraseLanguage.PhraseId });
        }

        // GET: /Delete
        public ActionResult ViewItem(int id, string languageCode)
        {
            //Get Item 
            PhraseTranslation phraseLanguage = new PhraseTranslation();
            phraseLanguage = phraseLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (phraseLanguage == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }


            //Parent data
            ViewData["PhraseId"] = id;
            ViewData["PhraseName"] = phraseLanguage.Phrase.PhraseName;


            phraseLanguageRepository.EditItemForDisplay(phraseLanguage);
            return View(phraseLanguage);

        }
    }
}
