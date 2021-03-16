using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Repository;
using System.Data.SqlClient;

namespace CWTDesktopDatabase.Controllers
{
    public class QueueMinderItemController : Controller
    {
        //main repository
        QueueMinderGroupRepository queueMinderGroupRepository = new QueueMinderGroupRepository();
        QueueMinderItemRepository queueMinderItemRepository = new QueueMinderItemRepository();

        public ActionResult List(int id, int? page, string filter, string sortField, int? sortOrder)
        {
            QueueMinderGroup queueMinderGroup = new QueueMinderGroup();
            queueMinderGroup = queueMinderGroupRepository.GetGroup(id);

            //Check Exists
            if (queueMinderGroup == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            //Set Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToQueueMinderGroup(id))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //SortField+SortOrder settings
            if (string.IsNullOrEmpty(sortField))
            {
                sortField = "QueueMinderItemDescription";
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
            QueueMinderItemsVM queueMinderItemsVM = new QueueMinderItemsVM();
            queueMinderItemsVM.QueueMinderGroup = queueMinderGroup;
            queueMinderItemsVM.QueueMinderItems = queueMinderItemRepository.PageQueueMinderItems(id, page ?? 1, filter ?? "", sortField, sortOrder ?? 0);

            //return items
            return View(queueMinderItemsVM);
        }

        public ActionResult Create(int id)
        {
            //Get QueueMinderGroup
            QueueMinderGroup queueMinderGroup = new QueueMinderGroup();
            queueMinderGroup = queueMinderGroupRepository.GetGroup(id);

            //Check Exists
            if (queueMinderGroup == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToQueueMinderGroup(id))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            QueueMinderItemVM queueMinderItemVM = new QueueMinderItemVM();

            QueueMinderItem queueMinderItem = new QueueMinderItem();
            queueMinderItem.QueueMinderGroup = queueMinderGroup;
            queueMinderItemVM.QueueMinderItem = queueMinderItem;

            GDSRepository gDSRepository = new GDSRepository();
            SelectList gDSs = new SelectList(gDSRepository.GetAllGDSs().ToList(), "GDSCode", "GDSName");
            queueMinderItemVM.GDSs = gDSs;

            ContextRepository contextRepository = new ContextRepository();
            SelectList contexts = new SelectList(contextRepository.GetAllContexts().ToList(), "ContextId", "ContextName");
            queueMinderItemVM.Contexts = contexts;

            //DONT SHOW 17,18,19 UNLESS ROLE ALLOWS
            QueueMinderTypeRepository queueMinderTypeRepository = new QueueMinderTypeRepository();
            SelectList queueMinderTypes = new SelectList(queueMinderTypeRepository.GetAllQueueMinderTypes().ToList(), "QueueMinderTypeId", "QueueMinderTypeDescription");
            queueMinderItemVM.QueueMinderTypes = queueMinderTypes;

            return View(queueMinderItemVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(QueueMinderItemVM queueMinderItemVM)
        {

             //Get QueueMinderGroup
            QueueMinderGroup queueMinderGroup = new QueueMinderGroup();
            queueMinderGroup = queueMinderGroupRepository.GetGroup(queueMinderItemVM.QueueMinderItem.QueueMinderGroupId);

            //Check Exists
            if (queueMinderGroup == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToQueueMinderGroup(queueMinderGroup.QueueMinderGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update  Model from Form
            try
            {
                UpdateModel <QueueMinderItem>(queueMinderItemVM.QueueMinderItem, "QueueMinderItem");
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
                queueMinderItemRepository.Add(queueMinderItemVM.QueueMinderItem);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            return RedirectToAction("List", new { id = queueMinderGroup.QueueMinderGroupId });
        }

        public ActionResult View(int id)
        {
            //Check Exists
            QueueMinderItem queueMinderItem = new QueueMinderItem();
            queueMinderItem = queueMinderItemRepository.GetItem(id);
            if (queueMinderItem == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            if (queueMinderItem.GDS == null)
            {
                GDS gDS = new GDS();
                queueMinderItem.GDS = gDS;
            }
            if (queueMinderItem.Context == null)
            {
                Context context = new Context();
                queueMinderItem.Context = context;
            }
            return View(queueMinderItem);
        }

        public ActionResult Edit(int id)
        {
            //Check Exists
            QueueMinderItem queueMinderItem = new QueueMinderItem();
            queueMinderItem = queueMinderItemRepository.GetItem(id);
            if (queueMinderItem == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToQueueMinderItem(queueMinderItem.QueueMinderItemId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            QueueMinderItemVM queueMinderItemVM = new QueueMinderItemVM();
            queueMinderItemVM.QueueMinderItem = queueMinderItem;


            GDSRepository gDSRepository = new GDSRepository();
            SelectList gDSs = new SelectList(gDSRepository.GetAllGDSs().ToList(), "GDSCode", "GDSName", queueMinderItem.GDSCode);
            queueMinderItemVM.GDSs = gDSs;

            ContextRepository contextRepository = new ContextRepository();
            SelectList contexts = new SelectList(contextRepository.GetAllContexts().ToList(), "ContextId", "ContextName", queueMinderItem.ContextId);
            queueMinderItemVM.Contexts = contexts;

            //DONT SHOW 17,18,19 UNLESS ROLE ALLOWSq
            QueueMinderTypeRepository queueMinderTypeRepository = new QueueMinderTypeRepository();
            SelectList queueMinderTypes = new SelectList(queueMinderTypeRepository.GetAllQueueMinderTypes().ToList(), "QueueMinderTypeId", "QueueMinderTypeDescription", queueMinderItem.QueueMinderTypeId);
            queueMinderItemVM.QueueMinderTypes = queueMinderTypes;

            return View(queueMinderItemVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(QueueMinderItemVM queueMinderItemVM)
        {
            //Get Item
            QueueMinderItem queueMinderItem = new QueueMinderItem();
            queueMinderItem = queueMinderItemRepository.GetItem(queueMinderItemVM.QueueMinderItem.QueueMinderItemId);
            
            //Check Exists
            if (queueMinderItem == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToQueueMinderItem(queueMinderItem.QueueMinderItemId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            try
            {
                UpdateModel<QueueMinderItem>(queueMinderItemVM.QueueMinderItem, "QueueMinderItem");
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
                queueMinderItemRepository.Edit(queueMinderItemVM.QueueMinderItem);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/QueueMinderItem.mvc/Edit/" + queueMinderItem.QueueMinderItemId;
                    return View("VersionError");
                }

                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            return RedirectToAction("List", new { id = queueMinderItem.QueueMinderGroupId });

        }

		[HttpGet]
		public ActionResult Delete(int id)
        {
            //Check Exists
            QueueMinderItem queueMinderItem = new QueueMinderItem();
            queueMinderItem = queueMinderItemRepository.GetItem(id);
            if (queueMinderItem == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToQueueMinderItem(queueMinderItem.QueueMinderItemId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            if (queueMinderItem.GDS == null)
            {
                GDS gDS = new GDS();
                queueMinderItem.GDS = gDS;
            }
            if (queueMinderItem.Context == null)
            {
                Context context = new Context();
                queueMinderItem.Context = context;
            }
            QueueMinderItemVM queueMinderItemVM = new QueueMinderItemVM();
            queueMinderItemVM.QueueMinderItem = queueMinderItem;
            return View(queueMinderItemVM);
        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(QueueMinderItemVM queueMinderItemVM)
        {
            //Get Item
            QueueMinderItem queueMinderItem = new QueueMinderItem();
            queueMinderItem = queueMinderItemRepository.GetItem(queueMinderItemVM.QueueMinderItem.QueueMinderItemId);
            //Check Exists
            if (queueMinderItem == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToQueueMinderItem(queueMinderItem.QueueMinderItemId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Delete Item
            try
            {
                queueMinderItemRepository.Delete(queueMinderItemVM.QueueMinderItem);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/QueueMinderItem.mvc/Delete/" + queueMinderItem.QueueMinderItemId.ToString();
                    return View("VersionError");
                }
                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            //Return
            return RedirectToAction("List", new { id = queueMinderItem.QueueMinderGroupId });
        }
    }
}
