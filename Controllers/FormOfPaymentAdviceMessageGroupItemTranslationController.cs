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
    public class FormOfPaymentAdviceMessageGroupItemTranslationController : Controller
    {
        FormOfPaymentAdviceMessageGroupItemRepository formOfPaymentAdviceMessageItemRepository = new FormOfPaymentAdviceMessageGroupItemRepository();
        FormOfPaymentAdviceMessageGroupItemTranslationRepository formOfPaymentAdviceMessageGroupItemTranslationRepository = new FormOfPaymentAdviceMessageGroupItemTranslationRepository();

        private string groupName = "Client Detail";

        //GET:List
        public ActionResult List(int id, int? page, string sortField, int? sortOrder)
        {
            //Get FormOfPaymentAdviceMessageGroupItem
            FormOfPaymentAdviceMessageGroupItem formOfPaymentAdviceMessageItem = new FormOfPaymentAdviceMessageGroupItem();
            formOfPaymentAdviceMessageItem = formOfPaymentAdviceMessageItemRepository.GetItem(id);

            //Check Exists
            if (formOfPaymentAdviceMessageItem == null)
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

            //SortField+SortOrder settings
            if (string.IsNullOrEmpty(sortField))
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

            //FormOfPaymentAdviceMessageGroupItem
            formOfPaymentAdviceMessageItemRepository.EditItemForDisplay(formOfPaymentAdviceMessageItem);

            //View data
            ViewData["FormOfPaymentAdviceMessageGroupItemId"] = id;
			ViewData["FormOfPaymentAdviceMessage"] = formOfPaymentAdviceMessageItem.FormOfPaymentAdviceMessage;
            ViewData["FormOfPaymentAdviceMessageGroupID"] = formOfPaymentAdviceMessageItem.FormOfPaymentAdviceMessageGroup.FormOfPaymentAdviceMessageGroupID;
            ViewData["FormOfPaymentAdviceMessageGroupName"] = formOfPaymentAdviceMessageItem.FormOfPaymentAdviceMessageGroup.FormOfPaymentAdviceMessageGroupName;

            //Get data
            var cwtPaginatedList = formOfPaymentAdviceMessageGroupItemTranslationRepository.PageFormOfPaymentAdviceMessageGroupItemTranslations(id, page ?? 1, sortField, sortOrder ?? 0);
            return View(cwtPaginatedList);
        }

        // GET: /Create
        public ActionResult Create(int id)
        {
            //Get FormOfPaymentAdviceMessageGroupItem
            FormOfPaymentAdviceMessageGroupItem formOfPaymentAdviceMessageItem = new FormOfPaymentAdviceMessageGroupItem();
            formOfPaymentAdviceMessageItem = formOfPaymentAdviceMessageItemRepository.GetItem(id);

            //Check Exists
            if (formOfPaymentAdviceMessageItem == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }

            formOfPaymentAdviceMessageItemRepository.EditItemForDisplay(formOfPaymentAdviceMessageItem);

            //AccessRights
            HierarchyRepository hierarchyRepository = new HierarchyRepository();
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //FormOfPaymentAdviceMessageGroupItem
            formOfPaymentAdviceMessageItemRepository.EditItemForDisplay(formOfPaymentAdviceMessageItem);

            //FormOfPaymentAdviceMessageGroupItemTranslation
            FormOfPaymentAdviceMessageGroupItemTranslation formOfPaymentAdviceMessageGroupItemTranslation = new FormOfPaymentAdviceMessageGroupItemTranslation();
            formOfPaymentAdviceMessageGroupItemTranslation.FormOfPaymentAdviceMessageGroupItemId = id;
            formOfPaymentAdviceMessageGroupItemTranslation.FormOfPaymentAdviceMessageGroupItem = formOfPaymentAdviceMessageItem;
            formOfPaymentAdviceMessageGroupItemTranslationRepository.EditItemForDisplay(formOfPaymentAdviceMessageGroupItemTranslation);

            //Language SelectList
            SelectList languageList = new SelectList(formOfPaymentAdviceMessageGroupItemTranslationRepository.GetUnUsedLanguages(id).ToList(), "LanguageCode", "LanguageName");
            ViewData["Languages"] = languageList;

            //View data
            ViewData["FormOfPaymentAdviceMessageGroupItemId"] = id;
            ViewData["FormOfPaymentAdviceMessage"] = formOfPaymentAdviceMessageItem.FormOfPaymentAdviceMessage;
            ViewData["FormOfPaymentAdviceMessageGroupID"] = formOfPaymentAdviceMessageItem.FormOfPaymentAdviceMessageGroup.FormOfPaymentAdviceMessageGroupID;
            ViewData["FormOfPaymentAdviceMessageGroupName"] = formOfPaymentAdviceMessageItem.FormOfPaymentAdviceMessageGroup.FormOfPaymentAdviceMessageGroupName;

            //Show Create Form
            return View(formOfPaymentAdviceMessageGroupItemTranslation);


        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormOfPaymentAdviceMessageGroupItemTranslation formOfPaymentAdviceMessageGroupItemTranslation)
        {
            //Get FormOfPaymentAdviceMessageGroupItem
            FormOfPaymentAdviceMessageGroupItem formOfPaymentAdviceMessageItem = new FormOfPaymentAdviceMessageGroupItem();
            formOfPaymentAdviceMessageItem = formOfPaymentAdviceMessageItemRepository.GetItem(formOfPaymentAdviceMessageGroupItemTranslation.FormOfPaymentAdviceMessageGroupItemId);

            //Check Exists
            if (formOfPaymentAdviceMessageItem == null)
            {
                ViewData["ActionMethod"] = "ListGet";
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
                UpdateModel(formOfPaymentAdviceMessageGroupItemTranslation);
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


            formOfPaymentAdviceMessageGroupItemTranslationRepository.Add(formOfPaymentAdviceMessageGroupItemTranslation);

            return RedirectToAction("List", new { id = formOfPaymentAdviceMessageGroupItemTranslation.FormOfPaymentAdviceMessageGroupItemId });
        }

        // GET: /Edit
        public ActionResult Edit(int id, string languageCode)
        {
            //Get Item 
            FormOfPaymentAdviceMessageGroupItemTranslation formOfPaymentAdviceMessageGroupItemTranslation = new FormOfPaymentAdviceMessageGroupItemTranslation();
            formOfPaymentAdviceMessageGroupItemTranslation = formOfPaymentAdviceMessageGroupItemTranslationRepository.GetItem(id, languageCode);

            //Check Exists
            if (formOfPaymentAdviceMessageGroupItemTranslation == null)
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

            //Get FormOfPaymentAdviceMessageGroupItem
            FormOfPaymentAdviceMessageGroupItem formOfPaymentAdviceMessageItem = new FormOfPaymentAdviceMessageGroupItem();
            formOfPaymentAdviceMessageItem = formOfPaymentAdviceMessageItemRepository.GetItem(formOfPaymentAdviceMessageGroupItemTranslation.FormOfPaymentAdviceMessageGroupItemId);

            //Check Exists
            if (formOfPaymentAdviceMessageItem == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }

            //FormOfPaymentAdviceMessageGroupItem
            formOfPaymentAdviceMessageItemRepository.EditItemForDisplay(formOfPaymentAdviceMessageItem);

            //FormOfPaymentAdviceMessageGroupItemTranslation
            formOfPaymentAdviceMessageGroupItemTranslation.FormOfPaymentAdviceMessageGroupItemId = id;
            formOfPaymentAdviceMessageGroupItemTranslation.FormOfPaymentAdviceMessageGroupItem = formOfPaymentAdviceMessageItem;
            formOfPaymentAdviceMessageGroupItemTranslationRepository.EditItemForDisplay(formOfPaymentAdviceMessageGroupItemTranslation);

            //Language SelectList
            List<Language> availableLanguages = formOfPaymentAdviceMessageGroupItemTranslationRepository.GetUnUsedLanguages(id).ToList();
            if (formOfPaymentAdviceMessageGroupItemTranslation.LanguageCode != null)
            {
                LanguageRepository languageRepository = new LanguageRepository();
                Language langage = languageRepository.GetLanguage(formOfPaymentAdviceMessageGroupItemTranslation.LanguageCode);
                if (langage != null)
                {
                    availableLanguages.Add(langage);
                }
            }

            SelectList languageList = new SelectList(availableLanguages, "LanguageCode", "LanguageName", formOfPaymentAdviceMessageGroupItemTranslation.LanguageCode);
            ViewData["Languages"] = languageList;

            //View data
            ViewData["FormOfPaymentAdviceMessageGroupItemId"] = id;
            ViewData["FormOfPaymentAdviceMessage"] = formOfPaymentAdviceMessageItem.FormOfPaymentAdviceMessage;
            ViewData["FormOfPaymentAdviceMessageGroupID"] = formOfPaymentAdviceMessageItem.FormOfPaymentAdviceMessageGroup.FormOfPaymentAdviceMessageGroupID;
            ViewData["FormOfPaymentAdviceMessageGroupName"] = formOfPaymentAdviceMessageItem.FormOfPaymentAdviceMessageGroup.FormOfPaymentAdviceMessageGroupName;

			return View(formOfPaymentAdviceMessageGroupItemTranslation);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, string languageCode, FormCollection formCollection)
        {
            //Get Item 
            FormOfPaymentAdviceMessageGroupItemTranslation formOfPaymentAdviceMessageGroupItemTranslation = new FormOfPaymentAdviceMessageGroupItemTranslation();
            formOfPaymentAdviceMessageGroupItemTranslation = formOfPaymentAdviceMessageGroupItemTranslationRepository.GetItem(id, languageCode);

            //Check Exists
            if (formOfPaymentAdviceMessageGroupItemTranslation == null)
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
                UpdateModel(formOfPaymentAdviceMessageGroupItemTranslation);

                if (!string.IsNullOrEmpty(formCollection["NewLanguageCode"]))
                {
                    formOfPaymentAdviceMessageGroupItemTranslation.LanguageCode = formCollection["NewLanguageCode"];
                }
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

            //Update
            try
            {
                formOfPaymentAdviceMessageGroupItemTranslationRepository.Update(formOfPaymentAdviceMessageGroupItemTranslation);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/FormOfPaymentAdviceMessageGroupItemTranslation.mvc/Edit?id=" + formOfPaymentAdviceMessageGroupItemTranslation.FormOfPaymentAdviceMessageGroupItemId + "&languageCode=" + formOfPaymentAdviceMessageGroupItemTranslation.LanguageCode;
                    return View("VersionError");
                }

                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List", new { id = formOfPaymentAdviceMessageGroupItemTranslation.FormOfPaymentAdviceMessageGroupItemId });
        }

        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id, string languageCode)
        {
            //Get Item 
            FormOfPaymentAdviceMessageGroupItemTranslation formOfPaymentAdviceMessageGroupItemTranslation = new FormOfPaymentAdviceMessageGroupItemTranslation();
            formOfPaymentAdviceMessageGroupItemTranslation = formOfPaymentAdviceMessageGroupItemTranslationRepository.GetItem(id, languageCode);

            //Check Exists
            if (formOfPaymentAdviceMessageGroupItemTranslation == null)
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

            //Get FormOfPaymentAdviceMessageGroupItem
            FormOfPaymentAdviceMessageGroupItem formOfPaymentAdviceMessageItem = new FormOfPaymentAdviceMessageGroupItem();
            formOfPaymentAdviceMessageItem = formOfPaymentAdviceMessageItemRepository.GetItem(formOfPaymentAdviceMessageGroupItemTranslation.FormOfPaymentAdviceMessageGroupItemId);

            //Check Exists
            if (formOfPaymentAdviceMessageItem == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }

            //FormOfPaymentAdviceMessageGroupItem
            formOfPaymentAdviceMessageItemRepository.EditItemForDisplay(formOfPaymentAdviceMessageItem);

            //FormOfPaymentAdviceMessageGroupItemTranslation
            formOfPaymentAdviceMessageGroupItemTranslation.FormOfPaymentAdviceMessageGroupItemId = id;
            formOfPaymentAdviceMessageGroupItemTranslation.FormOfPaymentAdviceMessageGroupItem = formOfPaymentAdviceMessageItem;
            formOfPaymentAdviceMessageGroupItemTranslationRepository.EditItemForDisplay(formOfPaymentAdviceMessageGroupItemTranslation);

            //Language SelectList
            SelectList languageList = new SelectList(formOfPaymentAdviceMessageGroupItemTranslationRepository.GetUnUsedLanguages(id).ToList(), "LanguageCode", "LanguageName");
            ViewData["Languages"] = languageList;

            //View data
            ViewData["FormOfPaymentAdviceMessageGroupItemId"] = id;
            ViewData["FormOfPaymentAdviceMessage"] = formOfPaymentAdviceMessageItem.FormOfPaymentAdviceMessage;
            ViewData["FormOfPaymentAdviceMessageGroupID"] = formOfPaymentAdviceMessageItem.FormOfPaymentAdviceMessageGroup.FormOfPaymentAdviceMessageGroupID;
            ViewData["FormOfPaymentAdviceMessageGroupName"] = formOfPaymentAdviceMessageItem.FormOfPaymentAdviceMessageGroup.FormOfPaymentAdviceMessageGroupName;

            return View(formOfPaymentAdviceMessageGroupItemTranslation);

        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, string languageCode, FormCollection collection)
        {
            //Get Item 
            FormOfPaymentAdviceMessageGroupItemTranslation formOfPaymentAdviceMessageGroupItemTranslation = new FormOfPaymentAdviceMessageGroupItemTranslation();
            formOfPaymentAdviceMessageGroupItemTranslation = formOfPaymentAdviceMessageGroupItemTranslationRepository.GetItem(id, languageCode);

            //Check Exists
            if (formOfPaymentAdviceMessageGroupItemTranslation == null)
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
                formOfPaymentAdviceMessageGroupItemTranslation.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                formOfPaymentAdviceMessageGroupItemTranslationRepository.Delete(formOfPaymentAdviceMessageGroupItemTranslation);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/FormOfPaymentAdviceMessageGroupItemTranslation.mvc/Delete?id=" + formOfPaymentAdviceMessageGroupItemTranslation.FormOfPaymentAdviceMessageGroupItemId + "&languageCode=" + formOfPaymentAdviceMessageGroupItemTranslation.LanguageCode;
                    return View("VersionError");
                }
                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            //Return
            return RedirectToAction("List", new { id = formOfPaymentAdviceMessageGroupItemTranslation.FormOfPaymentAdviceMessageGroupItemId });
        }
    }
}
