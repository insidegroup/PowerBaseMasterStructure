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
    public class PublicHolidayDateController : Controller
    {
        //main repository
        PublicHolidayGroupRepository publicHolidayGroupRepository = new PublicHolidayGroupRepository();
        PublicHolidayDateRepository publicHolidayDateRepository = new PublicHolidayDateRepository();

        //sortable List
        public ActionResult List(int id, int? page, string sortField, int? sortOrder)
        {
            PublicHolidayGroup publicHolidayGroup = new PublicHolidayGroup();
            publicHolidayGroup = publicHolidayGroupRepository.GetGroup(id);

            //Check Exists
            if (publicHolidayGroup == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }

            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToPublicHolidayGroup(id))
            {
                ViewData["Access"] = "WriteAccess";
            }


            if (sortField !="PublicHolidayDescription")
            {
                sortField = "PublicHolidayDate";
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
            }

            ViewData["PublicHolidayGroupId"] = publicHolidayGroup.PublicHolidayGroupId;
            ViewData["PublicHolidayGroupName"] = publicHolidayGroup.PublicHolidayGroupName;

            //return items
            var cwtPaginatedList = publicHolidayDateRepository.PagePublicHolidayDates(id, page ?? 1, "", sortField, sortOrder ?? 0);
            return View(cwtPaginatedList);

        }

        // GET: /Create
        public ActionResult Create(int id)
        {
            PublicHolidayGroup publicHolidayGroup = new PublicHolidayGroup();
            publicHolidayGroup = publicHolidayGroupRepository.GetGroup(id);

            //Check Exists
            if (publicHolidayGroup == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPublicHolidayGroup(id))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            PublicHolidayGroupHolidayDate publicHolidayDate = new PublicHolidayGroupHolidayDate();
            publicHolidayDate.PublicHolidayGroupName = publicHolidayGroup.PublicHolidayGroupName;
            publicHolidayDate.PublicHolidayGroupId = publicHolidayGroup.PublicHolidayGroupId;
            return View(publicHolidayDate);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PublicHolidayGroupHolidayDate publicHolidayGroupHolidayDate)
        {

            PublicHolidayGroup publicHolidayGroup = new PublicHolidayGroup();
            publicHolidayGroup = publicHolidayGroupRepository.GetGroup(publicHolidayGroupHolidayDate.PublicHolidayGroupId);

            //Check Exists
            if (publicHolidayGroup == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPublicHolidayGroup(publicHolidayGroupHolidayDate.PublicHolidayGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update  Model from Form
            try
            {
                UpdateModel(publicHolidayGroupHolidayDate);
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
                publicHolidayDateRepository.Add(publicHolidayGroupHolidayDate);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            return RedirectToAction("List", new { id = publicHolidayGroupHolidayDate.PublicHolidayGroupId });
        }

        // GET: /Edit
        public ActionResult Edit(int id)
        {
            //AccessRights
            PublicHolidayGroupHolidayDate publicHolidayGroupHolidayDate = new PublicHolidayGroupHolidayDate();
            publicHolidayGroupHolidayDate = publicHolidayDateRepository.GetItem(id);

            //Check Exists
            if (publicHolidayGroupHolidayDate == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }
            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPublicHolidayGroup(publicHolidayGroupHolidayDate.PublicHolidayGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            publicHolidayDateRepository.EditItemForDisplay(publicHolidayGroupHolidayDate);
            return View(publicHolidayGroupHolidayDate);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection collection)
        {
            //AccessRights
            PublicHolidayGroupHolidayDate publicHolidayGroupHolidayDate = new PublicHolidayGroupHolidayDate();
            publicHolidayGroupHolidayDate = publicHolidayDateRepository.GetItem(id);

            //Check Exists
            if (publicHolidayGroupHolidayDate == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }
            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPublicHolidayGroup(publicHolidayGroupHolidayDate.PublicHolidayGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            try
            {
                UpdateModel(publicHolidayGroupHolidayDate);
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

            return RedirectToAction("List", new { id = publicHolidayGroupHolidayDate.PublicHolidayGroupId });
        }

        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id)
        {
            //AccessRights
            PublicHolidayGroupHolidayDate publicHolidayGroupHolidayDate = new PublicHolidayGroupHolidayDate();
            publicHolidayGroupHolidayDate = publicHolidayDateRepository.GetItem(id);

            //Check Exists
            if (publicHolidayGroupHolidayDate == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }
            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPublicHolidayGroup(publicHolidayGroupHolidayDate.PublicHolidayGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            publicHolidayDateRepository.EditItemForDisplay(publicHolidayGroupHolidayDate);
            return View(publicHolidayGroupHolidayDate);
        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            //AccessRights
            PublicHolidayGroupHolidayDate publicHolidayGroupHolidayDate = new PublicHolidayGroupHolidayDate();
            publicHolidayGroupHolidayDate = publicHolidayDateRepository.GetItem(id);

            //Check Exists
            if (publicHolidayGroupHolidayDate == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }
            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPublicHolidayGroup(publicHolidayGroupHolidayDate.PublicHolidayGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Delete Item
            try
            {
                publicHolidayGroupHolidayDate.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                publicHolidayDateRepository.Delete(publicHolidayGroupHolidayDate);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/ReasonCodeItem.mvc/Delete/" + publicHolidayGroupHolidayDate.PublicHolidayDateId;
                    return View("VersionError");
                }
                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");            }
            //Return
            return RedirectToAction("List", new { id = publicHolidayGroupHolidayDate.PublicHolidayGroupId });
        }

        // GET: /View
        public ActionResult View(int id)
        {
            PublicHolidayGroupHolidayDate publicHolidayGroupHolidayDate = new PublicHolidayGroupHolidayDate();
            publicHolidayGroupHolidayDate = publicHolidayDateRepository.GetItem(id);
            //Check Exists
            if (publicHolidayGroupHolidayDate == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            publicHolidayDateRepository.EditItemForDisplay(publicHolidayGroupHolidayDate);
            return View(publicHolidayGroupHolidayDate);
        }
    }
}
