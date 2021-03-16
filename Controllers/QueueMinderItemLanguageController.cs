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
    public class QueueMinderItemLanguageController : Controller
    {
        //main repositories
        QueueMinderItemLanguageRepository queueMinderItemLanguageRepository = new QueueMinderItemLanguageRepository();
        QueueMinderItemRepository queueMinderItemRepository = new QueueMinderItemRepository();
        QueueMinderGroupRepository queueMinderGroupRepository = new QueueMinderGroupRepository();

        public ActionResult List(int id, int? page, string sortField, int? sortOrder)
        {
            //Get QueueMinderItem
            QueueMinderItem queueMinderItem = new QueueMinderItem();
            queueMinderItem = queueMinderItemRepository.GetItem(id);

            //Check Exists
            if (queueMinderItem == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }


            //SortField+SortOrder settings
            if (sortField != "ItineraryDescription" && sortField != "NotepadDescription")
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
            QueueMinderItemLanguagesVM queueMinderItemLanguagesVM = new QueueMinderItemLanguagesVM();
            queueMinderItemLanguagesVM.QueueMinderItemLanguages = queueMinderItemLanguageRepository.PageQueueMinderItemLanguages(id, page ?? 1, sortField, sortOrder ?? 0);
            queueMinderItemLanguagesVM.QueueMinderItem = queueMinderItem;

            //Set Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToQueueMinderItem(id))
            {
                queueMinderItemLanguagesVM.HasWriteAccess = true;
            }

            return View(queueMinderItemLanguagesVM);
        }

        public ActionResult Create(int id)
        {
            //Get Item From Database
            QueueMinderItem queueMinderItem = new QueueMinderItem();
            queueMinderItem = queueMinderItemRepository.GetItem(id);

            //Check Exists
            if (queueMinderItem == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToQueueMinderGroup(queueMinderItem.QueueMinderGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            QueueMinderItemLanguageVM queueMinderItemLanguageVM = new QueueMinderItemLanguageVM();

            QueueMinderItemLanguage queueMinderItemLanguage = new QueueMinderItemLanguage();
            queueMinderItemLanguage.QueueMinderItem = queueMinderItem;
            queueMinderItemLanguageVM.QueueMinderItemLanguage = queueMinderItemLanguage;

            SelectList queueMinderItemLanguages = new SelectList(queueMinderItemLanguageRepository.GetUnUsedLanguages(queueMinderItem.QueueMinderItemId).ToList(), "LanguageCode", "LanguageName");
            queueMinderItemLanguageVM.QueueMinderItemLanguages = queueMinderItemLanguages;

            return View(queueMinderItemLanguageVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(QueueMinderItemLanguageVM queueMinderItemLanguageVM)
        {

            //Get QueueMinderItem
            QueueMinderItem queueMinderItem = new QueueMinderItem();
            queueMinderItem = queueMinderItemRepository.GetItem(queueMinderItemLanguageVM.QueueMinderItemLanguage.QueueMinderItemId);

            //Check Exists
            if (queueMinderItem == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToQueueMinderGroup(queueMinderItem.QueueMinderGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update  Model from Form
            try
            {
                UpdateModel<QueueMinderItemLanguage>(queueMinderItemLanguageVM.QueueMinderItemLanguage, "QueueMinderItemLanguage");
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

            try
            {
                queueMinderItemLanguageRepository.Add(queueMinderItemLanguageVM.QueueMinderItemLanguage);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            return RedirectToAction("List", new { id = queueMinderItem.QueueMinderItemId });
        }

        // GET: /Edit
        public ActionResult Edit(int id, string languageCode)
        {
            //Get Item 
            QueueMinderItemLanguage queueMinderItemLanguage = new QueueMinderItemLanguage();
            queueMinderItemLanguage = queueMinderItemLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (queueMinderItemLanguage == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            QueueMinderItem queueMinderItem = new QueueMinderItem();
            queueMinderItem = queueMinderItemRepository.GetItem(id);
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToQueueMinderGroup(queueMinderItem.QueueMinderGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            QueueMinderItemLanguageVM queueMinderItemLanguageVM = new QueueMinderItemLanguageVM();
            queueMinderItemLanguageVM.QueueMinderItemLanguage = queueMinderItemLanguage;

            return View(queueMinderItemLanguageVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(QueueMinderItemLanguageVM queueMinderItemLanguageVM)
        {

            //Get QueueMinderItem
            QueueMinderItem queueMinderItem = new QueueMinderItem();
            queueMinderItem = queueMinderItemRepository.GetItem(queueMinderItemLanguageVM.QueueMinderItemLanguage.QueueMinderItemId);

            //Check Exists
            if (queueMinderItem == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToQueueMinderGroup(queueMinderItem.QueueMinderGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update  Model from Form
            try
            {
                UpdateModel<QueueMinderItemLanguage>(queueMinderItemLanguageVM.QueueMinderItemLanguage, "QueueMinderItemLanguage");
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

            try
            {
                queueMinderItemLanguageRepository.Update(queueMinderItemLanguageVM.QueueMinderItemLanguage);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            return RedirectToAction("List", new { id = queueMinderItem.QueueMinderItemId });
        }

        // GET: /Edit
        public ActionResult View(int id, string languageCode)
        {
            //Get Item 
            QueueMinderItemLanguage queueMinderItemLanguage = new QueueMinderItemLanguage();
            queueMinderItemLanguage = queueMinderItemLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (queueMinderItemLanguage == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            QueueMinderItemLanguageVM queueMinderItemLanguageVM = new QueueMinderItemLanguageVM();
            queueMinderItemLanguageVM.QueueMinderItemLanguage = queueMinderItemLanguage;

            return View(queueMinderItemLanguageVM);
        }

		[HttpGet]
		public ActionResult Delete(int id, string languageCode)
        {
            //Get Item 
            QueueMinderItemLanguage queueMinderItemLanguage = new QueueMinderItemLanguage();
            queueMinderItemLanguage = queueMinderItemLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (queueMinderItemLanguage == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            QueueMinderItemLanguageVM queueMinderItemLanguageVM = new QueueMinderItemLanguageVM();
            queueMinderItemLanguageVM.QueueMinderItemLanguage = queueMinderItemLanguage;

            return View(queueMinderItemLanguageVM);
        }


        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(QueueMinderItemLanguageVM queueMinderItemLanguageVM)
        {
            //Check QueueMinderItemLanguage Valid        
            if (queueMinderItemLanguageVM.QueueMinderItemLanguage == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            //Check QueueMinderItemLanguage Exists
            QueueMinderItemLanguage queueMinderItemLanguage = new QueueMinderItemLanguage();
            queueMinderItemLanguage = queueMinderItemLanguageRepository.GetItem(queueMinderItemLanguageVM.QueueMinderItemLanguage.QueueMinderItemId, queueMinderItemLanguageVM.QueueMinderItemLanguage.LanguageCode);
            if (queueMinderItemLanguage == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }
            //CHeck Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToQueueMinderGroup(queueMinderItemLanguage.QueueMinderItem.QueueMinderGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }


            //Delete Item
            try
            {
                queueMinderItemLanguageRepository.Delete(queueMinderItemLanguageVM.QueueMinderItemLanguage);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/QueueMinderItemLanguage.mvc/Delete/" + queueMinderItemLanguage.QueueMinderItemId.ToString() + "/" + queueMinderItemLanguage.LanguageCode;
                    return View("VersionError");
                }
                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            //Return
            return RedirectToAction("List", new { id = queueMinderItemLanguage.QueueMinderItemId });
        }
    }
}
