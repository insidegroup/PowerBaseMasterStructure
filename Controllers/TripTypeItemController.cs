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
    public class TripTypeItemController : Controller
    {
        //main repository
        TripTypeGroupRepository tripTypeGroupRepository = new TripTypeGroupRepository();
        TripTypeItemRepository tripTypeItemRepository = new TripTypeItemRepository();

        // GET: /List
        public ActionResult List(int id, int? page, string filter, string sortField, int? sortOrder)
        {
            TripTypeGroup tripTypeGroup = new TripTypeGroup();
            tripTypeGroup = tripTypeGroupRepository.GetGroup(id);

            //Check Exists
            if (tripTypeGroup == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            //Set Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToTripTypeGroup(id))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //SortField+SortOrder settings
            if (sortField != "TripTypeDescription")
            {
                sortField = "TripTypeDescription";
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
            ViewData["TripTypeGroupId"] = tripTypeGroup.TripTypeGroupId;
            ViewData["TripTypeGroupName"] = tripTypeGroup.TripTypeGroupName;

            //return items
            var cwtPaginatedList = tripTypeItemRepository.PageTripTypeItems(id, page ?? 1, filter ?? "", sortField, sortOrder ?? 0);
            return View(cwtPaginatedList);
        }

        // GET: /Create
        public ActionResult Create(int id)
        {
            //Get TripTypeGroup
            TripTypeGroup tripTypeGroup = new TripTypeGroup();
            tripTypeGroup = tripTypeGroupRepository.GetGroup(id);

            //Check Exists
            if (tripTypeGroup == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToTripTypeGroup(id))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            SelectList tripTypeList = new SelectList(tripTypeItemRepository.GetUnUsedTripTypes(id).ToList(), "TripTypeId", "TripTypeDescription");
            ViewData["TripTypes"] = tripTypeList;

            TripTypeItem tripTypeItem = new TripTypeItem();
            tripTypeItem.TripTypeGroupId = tripTypeGroup.TripTypeGroupId;
            tripTypeItem.TripTypeGroupName = tripTypeGroup.TripTypeGroupName;
            return View(tripTypeItem);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TripTypeItem tripTypeItem)
        {

            //Get TripTypeGroup
            TripTypeGroup tripTypeGroup = new TripTypeGroup();
            tripTypeGroup = tripTypeGroupRepository.GetGroup(tripTypeItem.TripTypeGroupId);

            //Check Exists
            if (tripTypeGroup == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToTripTypeGroup(tripTypeItem.TripTypeGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update  Model from Form
            try
            {
                UpdateModel(tripTypeItem);
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
                tripTypeItemRepository.Add(tripTypeItem);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            return RedirectToAction("List", new { id = tripTypeItem.TripTypeGroupId });
        }


        // GET: /Edit
        public ActionResult Edit(int id, int tripTypeGroupid)
        {
            //Get Item
            TripTypeItem tripTypeItem = new TripTypeItem();
            tripTypeItem = tripTypeItemRepository.GetItem(tripTypeGroupid, id);

            //Check Exists
            if (tripTypeItem == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToTripTypeGroup(tripTypeGroupid))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            tripTypeItemRepository.EditItemForDisplay(tripTypeItem);
            return View(tripTypeItem);
        }

        // POST: /PolicyGroup/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, int tripTypeGroupid, FormCollection collection)
        {
            //Get Item
            TripTypeItem tripTypeItem = new TripTypeItem();
            tripTypeItem = tripTypeItemRepository.GetItem(tripTypeGroupid, id);

            //Check Exists
            if (tripTypeItem == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToTripTypeGroup(tripTypeGroupid))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            try
            {
                UpdateModel(tripTypeItem);
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
                tripTypeItemRepository.Edit(tripTypeItem);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/TripTypeItem.mvc/Edit?id=" + tripTypeItem.TripTypeId + "&tripTypeGroupId="  +tripTypeItem.TripTypeGroupId;
                    return View("VersionError");
                }

                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");            
            }


            return RedirectToAction("List", new { id = tripTypeItem.TripTypeGroupId});

        }

        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id, int tripTypeGroupId)
        {
            //Get Item 
            TripTypeItem tripTypeItem = new TripTypeItem();
            tripTypeItem = tripTypeItemRepository.GetItem(tripTypeGroupId, id);

            //Check Exists
            if (tripTypeItem == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToTripTypeGroup(tripTypeGroupId))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Add Linked Information
            tripTypeItemRepository.EditItemForDisplay(tripTypeItem);

            //Return View
            return View(tripTypeItem);

        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, int tripTypeGroupId, FormCollection collection)
        {
            //Get Item 
            TripTypeItem tripTypeItem = new TripTypeItem();
            tripTypeItem = tripTypeItemRepository.GetItem(tripTypeGroupId, id);

            //Check Exists
            if (tripTypeItem == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToTripTypeGroup(tripTypeGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Delete Item
            try
            {
                tripTypeItem.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                tripTypeItemRepository.Delete(tripTypeItem);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/TripTypeItem.mvc/Delete?id=" + tripTypeItem.TripTypeId.ToString() + "&tripTypeGroupId=" + tripTypeItem.TripTypeGroupId.ToString();
                    return View("VersionError");
                }
                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");            
            }

            //Return
            return RedirectToAction("List", new { id = tripTypeGroupId });
        }
    }
}
