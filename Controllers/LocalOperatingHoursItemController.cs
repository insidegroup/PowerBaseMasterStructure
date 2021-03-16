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
    public class LocalOperatingHoursItemController : Controller
    {
        //main repositories
        LocalOperatingHoursGroupRepository localOperatingHoursGroupRepository = new LocalOperatingHoursGroupRepository();
        LocalOperatingHoursItemRepository localOperatingHoursRepository = new LocalOperatingHoursItemRepository();

        // GET: /lIST/
        public ActionResult List(int id, int? page, string sortField, int? sortOrder)
        {
            //Check Parent Exists
            LocalOperatingHoursGroup group = new LocalOperatingHoursGroup();
            group = localOperatingHoursGroupRepository.GetGroup(id);
            if (group == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }

            //Set Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToLocalOperatingHoursGroup(id))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //Sorting
            if (sortField != "OpeningDateTime" && sortField != "ClosingDateTime")
            {
                sortField = "WeekdayName";
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

            ViewData["DeletedFlag"] = false;

            LocalOperatingHoursGroup localOperatingHoursGroup = new LocalOperatingHoursGroup();
            localOperatingHoursGroup = localOperatingHoursGroupRepository.GetGroup(id);
            ViewData["LocalOperatingHoursGroupId"] = localOperatingHoursGroup.LocalOperatingHoursGroupId;
            ViewData["LocalOperatingHoursGroupName"] = localOperatingHoursGroup.LocalOperatingHoursGroupName;

            //return items
            var cwtPaginatedList = localOperatingHoursRepository.PageLocalOperatingHoursItems(id, page ?? 1, "", sortField, sortOrder ?? 0);
            return View(cwtPaginatedList);
        }
   
        // GET: /Create
        public ActionResult Create(int id)
        {
            LocalOperatingHoursGroup group = new LocalOperatingHoursGroup();
            group = localOperatingHoursGroupRepository.GetGroup(id);

            //Check Exists
            if (group == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToLocalOperatingHoursGroup(id))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            WeekdayRepository weekdayRepository = new WeekdayRepository();
            SelectList weekdayList = new SelectList(weekdayRepository.GetAllWeekdays().ToList(), "WeekdayId", "WeekdayName");
            ViewData["Weekdays"] = weekdayList;

           ViewData["hourList"] =
                new SelectList(new[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11" }
                .Select(x => new {value = x, text = x}), 
                "value", "text","");


            LocalOperatingHoursItem localOperatingHoursItem = new LocalOperatingHoursItem();
            localOperatingHoursItem.LocalOperatingHoursGroupName = group.LocalOperatingHoursGroupName;
            localOperatingHoursItem.LocalOperatingHoursGroupId = group.LocalOperatingHoursGroupId;
            return View(localOperatingHoursItem);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LocalOperatingHoursItem localOperatingHoursItem)
        {

            LocalOperatingHoursGroup group = new LocalOperatingHoursGroup();
            group = localOperatingHoursGroupRepository.GetGroup(localOperatingHoursItem.LocalOperatingHoursGroupId);

            //Check Exists
            if (group == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToLocalOperatingHoursGroup(localOperatingHoursItem.LocalOperatingHoursGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update Model from Form
            try
            {
                UpdateModel(localOperatingHoursItem);
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

            //Convert Times to DateTime for DB
			localOperatingHoursItem.OpeningDateTime = CWTStringHelpers.BuildDateTime(localOperatingHoursItem.OpeningTime);
			localOperatingHoursItem.ClosingDateTime = CWTStringHelpers.BuildDateTime(localOperatingHoursItem.ClosingTime);

            //Already Exists?
            LocalOperatingHoursItem localOperatingHoursItemOriginal = localOperatingHoursRepository.GetItem(
                localOperatingHoursItem.LocalOperatingHoursGroupId,
                localOperatingHoursItem.WeekDayId,
                localOperatingHoursItem.OpeningDateTime
            );

            if (localOperatingHoursItemOriginal != null)
            {
                try
                {
                    localOperatingHoursRepository.Delete(localOperatingHoursItemOriginal);
                }
                catch (SqlException ex)
                {
                    //Versioning Error - go to standard versionError page
                    if (ex.Message == "SQLVersioningError")
                    {
                        ViewData["ReturnURL"] = "/LocalOperatingHoursItem.mvc/Create/" + localOperatingHoursItem.LocalOperatingHoursGroupId;
                        return View("VersionError");
                    }
                    //Generic Error
                    ViewData["Message"] = "Unknown DBError";
                    return View("Error");
                }
            }

            try
            {
                localOperatingHoursRepository.Add(localOperatingHoursItem);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            //Return To List
            return RedirectToAction("List", new { id = localOperatingHoursItem.LocalOperatingHoursGroupId });
        }

        // GET: /Edit
        public ActionResult Edit(int id, int weekDayId, DateTime openingDateTime)
        {
            //AccessRights
            LocalOperatingHoursItem localOperatingHoursItem = new LocalOperatingHoursItem();
            localOperatingHoursItem = localOperatingHoursRepository.GetItem(id, weekDayId, openingDateTime);
            if (localOperatingHoursItem == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToLocalOperatingHoursGroup(localOperatingHoursItem.LocalOperatingHoursGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            WeekdayRepository weekdayRepository = new WeekdayRepository();
            SelectList weekdayList = new SelectList(weekdayRepository.GetAllWeekdays().ToList(), "WeekdayId", "WeekdayName");
            ViewData["Weekdays"] = weekdayList;

            localOperatingHoursRepository.EditItemForDisplay(localOperatingHoursItem);
            return View(localOperatingHoursItem);
        }

        // POST /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, DateTime OpeningDateTime, int WeekDayId, DateTime OpeningDateTimeOriginal, int WeekDayIdOriginal, FormCollection collection)
        {
            //Check Exists
            LocalOperatingHoursItem localOperatingHoursItemOriginal = localOperatingHoursRepository.GetItem(id, WeekDayIdOriginal, OpeningDateTimeOriginal);
            if (localOperatingHoursItemOriginal == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToLocalOperatingHoursGroup(id))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
            if (localOperatingHoursItemOriginal != null)
            {
                try
                {
                    localOperatingHoursRepository.Delete(localOperatingHoursItemOriginal);
                }
                catch (SqlException ex)
                {
                    //Versioning Error - go to standard versionError page
                    if (ex.Message == "SQLVersioningError")
                    {
                        ViewData["ReturnURL"] = "/LocalOperatingHoursItem.mvc/Edit/" + localOperatingHoursItemOriginal.LocalOperatingHoursGroupId;
                        return View("VersionError");
                    }
                    //Generic Error
                    ViewData["Message"] = "Unknown DBError";
                    return View("Error");
                }
            }

            //New Item
            LocalOperatingHoursItem localOperatingHoursItem = new LocalOperatingHoursItem();

            //Update Model from Form
            try
            {
                UpdateModel(localOperatingHoursItem);
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

            //Convert Times to DateTime for DB
            localOperatingHoursItem.OpeningDateTime = CWTStringHelpers.BuildDateTime(localOperatingHoursItem.OpeningTime);
			localOperatingHoursItem.ClosingDateTime = CWTStringHelpers.BuildDateTime(localOperatingHoursItem.ClosingTime);

            try
            {
                localOperatingHoursRepository.Add(localOperatingHoursItem);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/LocalOperatingHoursItem.mvc/Edit/" + localOperatingHoursItem.LocalOperatingHoursGroupId;
                    return View("VersionError");
                }
                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");            }
            //Return To List
            return RedirectToAction("List", new { id = localOperatingHoursItem.LocalOperatingHoursGroupId });
        
        }

        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id, int weekDayId, DateTime openingDateTime)
        {
            //Get Item
            LocalOperatingHoursItem localOperatingHoursItem = new LocalOperatingHoursItem();
            localOperatingHoursItem = localOperatingHoursRepository.GetItem(id, weekDayId, openingDateTime);
            
            //Check Exists
            if (localOperatingHoursItem == null)
            {
                ViewData["ActionMethod"] = "GetView";
                return View("RecordDoesNotExistError");
            }
            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToLocalOperatingHoursGroup(localOperatingHoursItem.LocalOperatingHoursGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            localOperatingHoursRepository.EditItemForDisplay(localOperatingHoursItem);
            return View(localOperatingHoursItem);
        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, int weekDayId, DateTime openingDateTime, FormCollection collection)
        {
            //Get Item
            LocalOperatingHoursItem localOperatingHoursItem = new LocalOperatingHoursItem();
            localOperatingHoursItem = localOperatingHoursRepository.GetItem(id, weekDayId, openingDateTime);

            //Check Exists
            if (localOperatingHoursItem == null)
            {
                ViewData["ActionMethod"] = "GetDelete";
                return View("RecordDoesNotExistError");
            }
            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToLocalOperatingHoursGroup(localOperatingHoursItem.LocalOperatingHoursGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
            //Parent
            LocalOperatingHoursGroup localOperatingHoursGroup = new LocalOperatingHoursGroup();
            localOperatingHoursGroup = localOperatingHoursGroupRepository.GetGroup(localOperatingHoursItem.LocalOperatingHoursGroupId);

            try
            {
                localOperatingHoursItem.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                localOperatingHoursRepository.Delete(localOperatingHoursItem);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/PolicyAirVendorGroupItem.mvc/Delete?id=" + localOperatingHoursItem .LocalOperatingHoursGroupId+ "&weekDayId=" + localOperatingHoursItem.WeekDayId + "&openingDateTime=" + localOperatingHoursItem.OpeningDateTime;
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            //Return
            return RedirectToAction("List", new { id = localOperatingHoursGroup.LocalOperatingHoursGroupId });
        }
    }
}
