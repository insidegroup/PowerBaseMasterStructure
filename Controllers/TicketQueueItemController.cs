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
    public class TicketQueueItemController : Controller
    {
        //main repositories
        TicketQueueGroupRepository ticketQueueGroupRepository = new TicketQueueGroupRepository();
        TicketQueueItemRepository ticketQueueItemRepository = new TicketQueueItemRepository();

        // GET: /List/
        public ActionResult List(int id, int? page, string sortField, int? sortOrder)
        {
            //Get WorkFlowGroup
            TicketQueueGroup ticketQueueGroup = new TicketQueueGroup();
            ticketQueueGroup = ticketQueueGroupRepository.GetGroup(id);

            //Check Exists
            if (ticketQueueGroup == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }

            //Set Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToTicketQueueGroup(id))
            {
                ViewData["Access"] = "WriteAccess";
            }

            if (sortField != "PseudoCityOrOfficeId")
            {
                sortField = "TicketQueueItemDescription";
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

            ViewData["TicketQueueGroupId"] = ticketQueueGroup.TicketQueueGroupId;
            ViewData["TicketQueueGroupName"] = ticketQueueGroup.TicketQueueGroupName;

			var paginatedView = ticketQueueItemRepository.GetTicketQueueGroupTicketQueueItems(id, page ?? 1, sortField, sortOrder ?? 0);

			return View(paginatedView);
        }

        // GET: /Create
        public ActionResult Create(int Id)
        {
            //Get WorkFlowGroup
            TicketQueueGroup ticketQueueGroup = new TicketQueueGroup();
            ticketQueueGroup = ticketQueueGroupRepository.GetGroup(Id);

            //Check Exists
            if (ticketQueueGroup == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToTicketQueueGroup(Id))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            TicketTypeRepository ticketTypeRepository = new TicketTypeRepository();
            SelectList ticketTypeList = new SelectList(ticketTypeRepository.GetAllTicketTypes().ToList(), "TicketTypeId", "TicketTypeDescription");
            ViewData["TicketTypes"] = ticketTypeList;

            GDSRepository gdsRepository = new GDSRepository();
            SelectList gdsList = new SelectList(gdsRepository.GetAllGDSs().ToList(), "GDSCode", "GDSName");
            ViewData["GDSs"] = gdsList;


            TicketQueueItem ticketQueueItem = new TicketQueueItem();

            ticketQueueItem.TicketQueueGroupId = ticketQueueGroup.TicketQueueGroupId;
            ticketQueueItem.TicketQueueGroupName = ticketQueueGroup.TicketQueueGroupName;
            return View(ticketQueueItem);
        }

        // POST: /Create/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TicketQueueItem ticketQueueItem)
        {
            //Get WorkFlowGroup
            TicketQueueGroup ticketQueueGroup = new TicketQueueGroup();
            ticketQueueGroup = ticketQueueGroupRepository.GetGroup(ticketQueueItem.TicketQueueGroupId);

            //Check Exists
            if (ticketQueueGroup == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToTicketQueueGroup(ticketQueueItem.TicketQueueGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update  Model from Form
            try
            {
                UpdateModel(ticketQueueItem);
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
                ticketQueueItemRepository.Add(ticketQueueItem);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            return RedirectToAction("List", new { id = ticketQueueItem.TicketQueueGroupId });
        }

        // GET: /Edit
        public ActionResult Edit(int id)
        {
            //AccessRights
            TicketQueueItem ticketQueueItem = new TicketQueueItem();
            ticketQueueItem = ticketQueueItemRepository.GetItem(id);

            //Check Exists
            if (ticketQueueItem == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToTicketQueueGroup(ticketQueueItem.TicketQueueGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            TicketTypeRepository ticketTypeRepository = new TicketTypeRepository();
            SelectList ticketTypeList = new SelectList(ticketTypeRepository.GetAllTicketTypes().ToList(), "TicketTypeId", "TicketTypeDescription");
            ViewData["TicketTypes"] = ticketTypeList;

            GDSRepository gdsRepository = new GDSRepository();
            SelectList gdsList = new SelectList(gdsRepository.GetAllGDSs().ToList(), "GDSCode", "GDSName");
            ViewData["GDSs"] = gdsList;

            //Parent Information
            ViewData["TicketQueueGroupId"] = ticketQueueItem.TicketQueueGroupId;
            ViewData["TicketQueueGroupName"] = ticketQueueGroupRepository.GetGroup(ticketQueueItem.TicketQueueGroupId).TicketQueueGroupName;

            //Add Linked Information
            ticketQueueItemRepository.EditItemForDisplay(ticketQueueItem);

            //Show To User
            return View(ticketQueueItem);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection collection)
        {
            //Get Item
            TicketQueueItem ticketQueueItem = new TicketQueueItem();
            ticketQueueItem = ticketQueueItemRepository.GetItem(id);

            //Check Exists
            if (ticketQueueItem == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToTicketQueueGroup(ticketQueueItem.TicketQueueGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            try
            {
                UpdateModel(ticketQueueItem);
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
                ticketQueueItemRepository.Edit(ticketQueueItem);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/TicketQueueItem.mvc/Edit?id=" + ticketQueueItem.TicketQueueItemId.ToString();
                    return View("VersionError");
                }

                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");            }
            return RedirectToAction("List", new { id = ticketQueueItem.TicketQueueGroupId });
        }

        // POST: /PolicyGroup/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
		public ActionResult Delete(int id, FormCollection collection)
        {
            //Get Item 
            TicketQueueItem ticketQueueItem = new TicketQueueItem();
            ticketQueueItem = ticketQueueItemRepository.GetItem(id);

            //Check Exists
            if (ticketQueueItem == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToTicketQueueGroup(ticketQueueItem.TicketQueueGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Delete Item
            try
            {
                ticketQueueItem.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                ticketQueueItemRepository.Delete(ticketQueueItem);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/TicketQueueItem.mvc/Delete?id=" + ticketQueueItem.TicketQueueItemId.ToString();
                    return View("VersionError");
                }
                //Generic Error
                ViewData["Message"] = ex.Message;
                return View("Error");
            }
            //Return
            return RedirectToAction("List", new { id = ticketQueueItem.TicketQueueGroupId });
        }

        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id)
        {
            //Get Item 
            TicketQueueItem ticketQueueItem = new TicketQueueItem();
            ticketQueueItem = ticketQueueItemRepository.GetItem(id);

            //Check Exists
            if (ticketQueueItem == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToTicketQueueGroup(ticketQueueItem.TicketQueueGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Parent Information
            ViewData["TicketQueueGroupId"] = ticketQueueItem.TicketQueueGroupId;
            ViewData["TicketQueueGroupName"] = ticketQueueGroupRepository.GetGroup(ticketQueueItem.TicketQueueGroupId).TicketQueueGroupName;

            //Add Linked Informationn
            ticketQueueItemRepository.EditItemForDisplay(ticketQueueItem);

            //Show To User
            return View(ticketQueueItem);
        }

        // GET: /View
        public ActionResult View(int id)
        {
            //Get Item 
            TicketQueueItem ticketQueueItem = new TicketQueueItem();
            ticketQueueItem = ticketQueueItemRepository.GetItem(id);

            //Check Exists
            if (ticketQueueItem == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            //Parent Information
            ViewData["TicketQueueGroupId"] = ticketQueueItem.TicketQueueGroupId;
            ViewData["TicketQueueGroupName"] = ticketQueueGroupRepository.GetGroup(ticketQueueItem.TicketQueueGroupId).TicketQueueGroupName;

            //Add Linked Information
            ticketQueueItemRepository.EditItemForDisplay(ticketQueueItem);

            //Show To User
            return View(ticketQueueItem);
        }

    }
}
