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
	public class TeamOutOfOfficeItemController : Controller
	{
		//main repository
		TeamOutOfOfficeItemRepository teamOutOfOfficeItemRepository = new TeamOutOfOfficeItemRepository();
		HierarchyRepository hierarchyRepository = new HierarchyRepository();

        private string groupName = "Client Detail";

		// GET: /ListUnDeleted
		public ActionResult List(int id, string filter, int? page, string sortField, int? sortOrder)
		{
			//Set Access Rights
			ViewData["Access"] = "";
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Access"] = "WriteAccess";
			}

			//SortField
			if (string.IsNullOrEmpty(sortField))
			{
				sortField = "PrimaryTeamName";
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

			if (teamOutOfOfficeItemRepository == null)
			{
				ViewData["ActionMethod"] = "ListGet";
				return View("Error");
			}

			var cwtPaginatedList = teamOutOfOfficeItemRepository.PageTeamOutOfOfficeItems(id, page ?? 1, filter ?? "", sortField, sortOrder ?? 0);

			if (cwtPaginatedList == null)
			{
				ViewData["ActionMethod"] = "ListGet";
				return View("Error");
			}

            //TeamOutOfOfficeGroup
            TeamOutOfOfficeGroupRepository teamOutOfOfficeGroupRepository = new TeamOutOfOfficeGroupRepository();
            TeamOutOfOfficeGroup group = new TeamOutOfOfficeGroup();
            group = teamOutOfOfficeGroupRepository.GetGroup(id);

            //Check Exists
            if (group == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            ViewData["TeamOutOfOfficeGroupName"] = group.TeamOutOfOfficeGroupName;

            //return items
            return View(cwtPaginatedList);  
		}

		// GET: /Edit
		public ActionResult Edit(int id)
		{
			//Get Item From Database
			TeamOutOfOfficeItem group = new TeamOutOfOfficeItem();
			group = teamOutOfOfficeItemRepository.GetItem(id);

			//Check Exists
			if (group == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			//Check Access
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

            teamOutOfOfficeItemRepository.EditItemForDisplay(group);

			return View(group);
		}

		// POST: /Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, FormCollection collection)
		{
			//Get Item From Database
			TeamOutOfOfficeItem group = new TeamOutOfOfficeItem();
			group = teamOutOfOfficeItemRepository.GetItem(id);

			//Check Exists
			if (group == null)
			{
				ViewData["ActionMethod"] = "EditPost";
				return View("RecordDoesNotExistError");
			}
            //Check Access
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
				return View("Error");
			}

		   //Update Model From Form + Validate against DB
			try
			{
				UpdateModel(group);
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
				teamOutOfOfficeItemRepository.Edit(group);
			}
			catch (SqlException ex)
			{
				//Versioning Error
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/TeamOutOfOfficeItem.mvc/Edit/" + group.TeamOutOfOfficeItemId.ToString();
					return View("VersionError");
				}
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("List", new { id = group.TeamOutOfOfficeGroupId } );
		}
	}
}
