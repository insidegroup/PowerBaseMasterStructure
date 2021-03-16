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
    public class WorkFlowItemController : Controller
    {
        //main repository
        WorkFlowGroupRepository workFlowGroupRepository = new WorkFlowGroupRepository();
        WorkFlowItemRepository workFlowItemRepository = new WorkFlowItemRepository();

        // GET: /List/
        public ActionResult List(int id, int? page, string filter, string sortField, int? sortOrder)
        {
            //Get WorkFlowGroup
            WorkFlowGroup workFlowGroup = new WorkFlowGroup();
            workFlowGroup = workFlowGroupRepository.GetGroup(id);

            //Check Exists
            if (workFlowGroup == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            //Set Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToWorkflowGroup(id))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //SortField+SortOrder settings
            if (sortField != "FormName")
            {
                sortField = "WorkFlowPanelDisplaySequence";
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

            ViewData["WorkFlowGroupId"] = workFlowGroup.WorkFlowGroupId;
            ViewData["WorkFlowGroupName"] = workFlowGroup.WorkFlowGroupName;

            //return items
            var cwtPaginatedList = workFlowItemRepository.PageWorkFlowItems(id, page ?? 1, filter ?? "", sortField, sortOrder ?? 0);
            return View(cwtPaginatedList);
        }

		/*
		  * CREATE, DELETE - Removed for version 1.07.1 (Jun 2012 - D McArdle)
		 * 
		 * 
		 * 
		// GET: /Create
		public ActionResult Create(int id)
		{
			//Get WorkFlowGroup
			WorkFlowGroup workFlowGroup = new WorkFlowGroup();
			workFlowGroup = workFlowGroupRepository.GetGroup(id);

			//Check Exists
			if (workFlowGroup == null)
			{
				ViewData["ActionMethod"] = "CreateGet";
				return View("RecordDoesNotExistError");
			}
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToWorkflowGroup(id))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}
			SelectList tripTypeList = new SelectList(workFlowItemRepository.GetUnUsedForms(id).ToList(), "FormId", "FormName");
			ViewData["Forms"] = tripTypeList;

			WorkFlowItem workFlowItem = new WorkFlowItem();
			workFlowItem.WorkFlowGroupName = workFlowGroup.WorkFlowGroupName;
			workFlowItem.WorkFlowGroupId = workFlowGroup.WorkFlowGroupId;
			return View(workFlowItem);
		}

		// POST: /Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(WorkFlowItem workFlowItem)
		{
			//Get WorkFlowGroup
			WorkFlowGroup workFlowGroup = new WorkFlowGroup();
			workFlowGroup = workFlowGroupRepository.GetGroup(workFlowItem.WorkFlowGroupId);

			//Check Exists
			if (workFlowGroup == null)
			{
				ViewData["ActionMethod"] = "CreateGet";
				return View("RecordDoesNotExistError");
			}
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToWorkflowGroup(workFlowItem.WorkFlowGroupId))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}


			//Update  Model from Form
			try
			{
				UpdateModel(workFlowItem);
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
				workFlowItemRepository.Add(workFlowItem);
			}
			catch (SqlException ex)
			{
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToRoute("Default", new { action="List", id = workFlowItem.WorkFlowGroupId });
		}

		// GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id, int workflowGroupId)
		{
			//Get Item 
			WorkFlowItem workFlowItem = new WorkFlowItem();
			workFlowItem = workFlowItemRepository.GetItem(workflowGroupId, id);

			//Check Exists
			if (workFlowItem == null)
			{
				ViewData["ActionMethod"] = "DeleteGet";
				return View("RecordDoesNotExistError");
			}

			//Check AccessRights
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToWorkflowGroup(workflowGroupId))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Return View
			workFlowItemRepository.EditItemForDisplay(workFlowItem);
			return View(workFlowItem);

		}

		// POST: /Delete
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, int workflowGroupId, FormCollection collection)
		{
			//Get Item 
			WorkFlowItem workFlowItem = new WorkFlowItem();
			workFlowItem = workFlowItemRepository.GetItem(workflowGroupId, id);

			//Check Exists
			if (workFlowItem == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Check AccessRights
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToWorkflowGroup(workflowGroupId))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Delete Item
			 try
			 {
				 workFlowItem.VersionNumber = Int32.Parse(collection["VersionNumber"]);
				 workFlowItemRepository.Delete(workFlowItem);
			 }
			 catch (SqlException ex)
			 {
				 //Versioning Error - go to standard versionError page
				 if (ex.Message == "SQLVersioningError")
				 {
					 ViewData["ReturnURL"] = "/WorkFlowItem.mvc/Delete?id=" + workFlowItem.FormId.ToString() + "&workflowGroupId=" + workFlowItem.WorkFlowGroupId.ToString();
					 return View("VersionError");
				 }
				 //Generic Error
				 return View("Error");
			 }

			//Return
			return RedirectToAction("List", new { id = workflowGroupId });
		}

		*/
	}
}
