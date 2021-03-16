using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Repository;
using System.Data.SqlClient;
using CWTDesktopDatabase.ViewModels;

namespace CWTDesktopDatabase.Controllers
{
    public class ApprovalItemController : Controller
    {
        //main repository
		ApprovalItemRepository approvalItemRepository = new ApprovalItemRepository();
		ApprovalGroupRepository approvalGroupRepository = new ApprovalGroupRepository();
		
		HierarchyRepository hierarchyRepository = new HierarchyRepository();

		private string groupName = "Approval Group Administrator";

        // GET: /List
        public ActionResult List(int id, string filter, int? page, string sortField, int? sortOrder)
        {
             RolesRepository rolesRepository = new RolesRepository();
			
			//Set Access Rights
            ViewData["Access"] = "";
            if (hierarchyRepository.AdminHasDomainWriteAccess(groupName) && rolesRepository.HasWriteAccessToApprovalGroup(id))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //SortField
            if (string.IsNullOrEmpty(sortField))
            {
                sortField = "ProductName";
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

			if (approvalItemRepository == null)
			{
				ViewData["ActionMethod"] = "ListGet";
				return View("Error");
			}

			ApprovalGroup approvalGroup = new ApprovalGroup();
			approvalGroup = approvalGroupRepository.GetGroup(id);
			if (approvalGroup != null)
			{
				ViewData["ApprovalGroupId"] = approvalGroup.ApprovalGroupId; 
				ViewData["ApprovalGroupName"] = approvalGroup.ApprovalGroupName;
			}

			var cwtPaginatedList = approvalItemRepository.PageApprovalItems(id, page ?? 1, filter ?? "", sortField, sortOrder ?? 0);

			if (cwtPaginatedList == null)
			{
				ViewData["ActionMethod"] = "ListGet";
				return View("Error");
			}
			
			//return items
            return View(cwtPaginatedList);  
        }

		// GET: /View
		public ActionResult View(int id)
		{
			ApprovalItem approvalItem = new ApprovalItem();
			approvalItem = approvalItemRepository.ApprovalItem(id);

			//Check Exists
			if (approvalItem == null)
			{
				ViewData["ActionMethod"] = "ViewGet";
				return View("RecordDoesNotExistError");
			}

			ApprovalItemVM approvalItemVM = new ApprovalItemVM();
			approvalItemVM.ApprovalItem = approvalItem;

			ApprovalGroup approvalGroup = new ApprovalGroup();
			approvalGroup = approvalGroupRepository.GetGroup(approvalItem.ApprovalGroupId);
			if (approvalGroup != null)
			{
				approvalItem.ApprovalGroupId = approvalGroup.ApprovalGroupId;
				approvalItemVM.ApprovalItem = approvalItem;
				ViewData["ApprovalGroupId"] = approvalGroup.ApprovalGroupId;
				ViewData["ApprovalGroupName"] = approvalGroup.ApprovalGroupName;
			}

			return View(approvalItemVM);
		}

		// GET: /Create
		public ActionResult Create(int id)
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToApprovalGroup(id))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			} 
			
			ApprovalItemVM approvalItemVM = new ApprovalItemVM();

			ApprovalGroup approvalGroup = new ApprovalGroup();
			approvalGroup = approvalGroupRepository.GetGroup(id);
			if (approvalGroup != null)
			{
				ApprovalItem approvalItem = new ApprovalItem();
				approvalItem.ApprovalGroupId = approvalGroup.ApprovalGroupId;
				approvalItemVM.ApprovalItem = approvalItem;
				ViewData["ApprovalGroupId"] = approvalGroup.ApprovalGroupId;
				ViewData["ApprovalGroupName"] = approvalGroup.ApprovalGroupName;
			}

			return View(approvalItemVM);
		}

		// POST: /Create/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(ApprovalItemVM approvalItemVM)
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel(approvalItemVM);
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
				approvalItemRepository.Add(approvalItemVM.ApprovalItem);
			}
			catch (SqlException ex)
			{
				//Non-Unique Name
				if (ex.Message == "NonUniqueName")
				{
					return View("NonUniqueNameError");
				}
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			ViewData["NewSortOrder"] = 0;
			return RedirectToAction("List", new { id = approvalItemVM.ApprovalItem.ApprovalGroupId});
		}

		// GET: /Edit
		public ActionResult Edit(int id)
		{
			//Get Item From Database
			ApprovalItem approvalItem = new ApprovalItem();
			approvalItem = approvalItemRepository.ApprovalItem(id);

			//Check Exists
			if (approvalItem == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToApprovalGroup(approvalItem.ApprovalGroupId))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			ApprovalItemVM approvalItemVM = new ApprovalItemVM();
			approvalItemRepository.EditForDisplay(approvalItem);

			ApprovalGroup approvalGroup = new ApprovalGroup();
			approvalGroup = approvalGroupRepository.GetGroup(approvalItem.ApprovalGroupId);
			if (approvalGroup != null)
			{
				approvalItem.ApprovalGroupId = approvalGroup.ApprovalGroupId;
				approvalItemVM.ApprovalItem = approvalItem;
				ViewData["ApprovalGroupId"] = approvalGroup.ApprovalGroupId;
				ViewData["ApprovalGroupName"] = approvalGroup.ApprovalGroupName;
			}

			return View(approvalItemVM);
		}

		// POST: /Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(ApprovalItemVM approvalItemVM)
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel(approvalItemVM);
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
				approvalItemRepository.Update(approvalItemVM.ApprovalItem);
			}
			catch (SqlException ex)
			{
				//Non-Unique Name
				if (ex.Message == "NonUniqueName")
				{
					return View("NonUniqueNameError");
				}
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			ViewData["NewSortOrder"] = 0;
			return RedirectToAction("List", new { id = approvalItemVM.ApprovalItem.ApprovalGroupId });
		}

		// GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id)
		{
			ApprovalItem approvalItem = new ApprovalItem();
			approvalItem = approvalItemRepository.ApprovalItem(id);

			//Check Exists
			if (approvalItem == null)
			{
				ViewData["ActionMethod"] = "DeleteGet";
				return View("RecordDoesNotExistError");
			}

			ApprovalItemVM approvalItemVM = new ApprovalItemVM();
			approvalItemVM.ApprovalItem = approvalItem;

			ApprovalGroup approvalGroup = new ApprovalGroup();
			approvalGroup = approvalGroupRepository.GetGroup(approvalItem.ApprovalGroupId);
			if (approvalGroup != null)
			{
				approvalItem.ApprovalGroupId = approvalGroup.ApprovalGroupId;
				approvalItemVM.ApprovalItem = approvalItem;
				ViewData["ApprovalGroupId"] = approvalGroup.ApprovalGroupId;
				ViewData["ApprovalGroupName"] = approvalGroup.ApprovalGroupName;
			}

			return View(approvalItemVM);
		}

		// POST: /Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(ApprovalItemVM approvalItemVM)
		{
			//Get Item From Database
			ApprovalItem approvalItem = new ApprovalItem();
			approvalItem = approvalItemRepository.ApprovalItem(approvalItemVM.ApprovalItem.ApprovalItemId);

			//Check Exists
			if (approvalItem == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToApprovalGroup(approvalItemVM.ApprovalItem.ApprovalGroupId))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Delete Item
			try
			{
				approvalItemRepository.Delete(approvalItem);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/ApprovalItem.mvc/Delete/" + approvalItem.ApprovalItemId;
					return View("VersionError");
				}
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			return RedirectToAction("List", new { id = approvalItemVM.ApprovalItem.ApprovalGroupId });
		}
    }
}
