using System.Data.SqlClient;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Repository;
using CWTDesktopDatabase.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Controllers
{
	public class OptionalFieldGroupController : Controller
	{
		HierarchyRepository hierarchyRepository = new HierarchyRepository();
		OptionalFieldGroupRepository optionalFieldGroupRepository = new OptionalFieldGroupRepository();
		private string groupName = "Passive Segment Builder";

		// GET: /ListUnDeleted
		public ActionResult ListUnDeleted(string filter, int? page, string sortField, int? sortOrder)
		{
			//SortField
			if (sortField != "HierarchyType" && sortField != "HierarchyItem")
			{
				sortField = "OptionalFieldGroupName";
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
				sortOrder = 0;
			}

			OptionalFieldGroupsVM optionalFieldGroupsVM = new OptionalFieldGroupsVM();

			//Set Access Rights
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				optionalFieldGroupsVM.HasDomainWriteAccess = true;
			}

			if (optionalFieldGroupRepository != null)
			{
				var optionalFieldGroups = optionalFieldGroupRepository.PageOptionalFieldGroups(false, page ?? 1, filter ?? "", sortField, sortOrder ?? 0);
				
				if (optionalFieldGroups != null)
				{
					optionalFieldGroupsVM.OptionalFieldGroups = optionalFieldGroups;
				}
			}
			
			//Return items			
			return View(optionalFieldGroupsVM);
		}
		
		// GET: /ListDeleted
		public ActionResult ListDeleted(string filter, int? page, string sortField, int? sortOrder)
		{
			//SortField
			if (sortField != "HierarchyType" && sortField != "EnabledDate")
			{
				sortField = "OptionalFieldGroupName";
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
				sortOrder = 0;
			}

			OptionalFieldGroupsVM optionalFieldGroupsVM = new OptionalFieldGroupsVM();
			
			//Set Access Rights
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				optionalFieldGroupsVM.HasDomainWriteAccess = true;
			}

			//return items
			optionalFieldGroupsVM.OptionalFieldGroups = optionalFieldGroupRepository.PageOptionalFieldGroups(true, page ?? 1, filter ?? "", sortField, sortOrder ?? 0);
			
			return View(optionalFieldGroupsVM);
		}

		// GET: /ListOrphaned
		public ActionResult ListOrphaned(string filter, int? page, string sortField, int? sortOrder)
		{
			//Set Access Rights
			ViewData["Access"] = "";
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Access"] = "WriteAccess";
			}

			//SortField
			sortField = "OptionalFieldGroupName";
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

			OptionalFieldGroupsVM optionalFieldGroupsVM = new OptionalFieldGroupsVM();
			
			//Set Access Rights
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				optionalFieldGroupsVM.HasDomainWriteAccess = true;
			}

			//return items
			optionalFieldGroupsVM.OptionalFieldGroupsOrphaned = optionalFieldGroupRepository.PageOrphanedOptionalFieldGroups(page ?? 1, filter ?? "", sortField, sortOrder ?? 0);

			return View(optionalFieldGroupsVM);
		}
		

		// GET: /View
		public ActionResult View(int id)
		{
			OptionalFieldGroup optionalFieldGroup = new OptionalFieldGroup();
			optionalFieldGroup = optionalFieldGroupRepository.GetGroup(id);

			//Check Exists
			if (optionalFieldGroup == null)
			{
				ViewData["ActionMethod"] = "ViewGet";
				return View("RecordDoesNotExistError");
			}

			OptionalFieldGroupVM optionalFieldGroupVM = new OptionalFieldGroupVM();

			optionalFieldGroupRepository.EditGroupForDisplay(optionalFieldGroup);

			optionalFieldGroupVM.OptionalFieldGroup = optionalFieldGroup;

			return View(optionalFieldGroupVM);
		}

		// GET: /Create
		public ActionResult Create()
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			OptionalFieldGroupVM optionalFieldGroupVM = new OptionalFieldGroupVM();
			OptionalFieldGroup optionalFieldGroup = new OptionalFieldGroup();
			optionalFieldGroup.EnabledFlag = true;
			optionalFieldGroupVM.OptionalFieldGroup = optionalFieldGroup;

			TablesDomainHierarchyLevelRepository tablesDomainHierarchyLevelRepository = new TablesDomainHierarchyLevelRepository();
			SelectList hierarchyTypesList = new SelectList(tablesDomainHierarchyLevelRepository.GetDomainHierarchies(groupName).ToList(), "HierarchyLevelTableName", "HierarchyLevelTableName");
			optionalFieldGroupVM.HierarchyTypes = hierarchyTypesList;

			return View(optionalFieldGroupVM);
		}

		// POST: /Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(OptionalFieldGroupVM optionalFieldGroupVM)
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//We need to extract group from groupVM
			OptionalFieldGroup optionalFieldGroup = new OptionalFieldGroup();
			optionalFieldGroup = optionalFieldGroupVM.OptionalFieldGroup;
			if (optionalFieldGroup == null)
			{
				ViewData["Message"] = "ValidationError : missing item"; ;
				return View("Error");
			}

			//Check Access Rights to Domain Hierarchy
			if (!hierarchyRepository.AdminHasDomainHierarchyWriteAccess(
															optionalFieldGroup.HierarchyType, 
															optionalFieldGroup.HierarchyCode,
															optionalFieldGroup.SourceSystemCode, 
															groupName))
			{
				ViewData["Message"] = "You cannot add to this hierarchy item";
				return View("Error");
			}

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel(optionalFieldGroup);
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
				optionalFieldGroupRepository.Add(optionalFieldGroup);
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

			return RedirectToAction("ListUnDeleted");
		}

		
		// GET: /Edit
		public ActionResult Edit(int id)
		{
			//Get Item From Database
			OptionalFieldGroup optionalFieldGroup = new	OptionalFieldGroup();
			optionalFieldGroup = optionalFieldGroupRepository.GetGroup(id);

			//Check Exists
			if (optionalFieldGroup == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}
			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToOptionalFieldGroup(id))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			OptionalFieldGroupVM optionalFieldGroupVM = new OptionalFieldGroupVM();

			optionalFieldGroupRepository.EditGroupForDisplay(optionalFieldGroup);

			optionalFieldGroupVM.OptionalFieldGroup = optionalFieldGroup;
			
			TablesDomainHierarchyLevelRepository tablesDomainHierarchyLevelRepository = new TablesDomainHierarchyLevelRepository();
			SelectList hierarchyTypesList = new SelectList(
									tablesDomainHierarchyLevelRepository.GetDomainHierarchies(groupName).ToList(), 
									"HierarchyLevelTableName",
									"HierarchyLevelTableName",
									optionalFieldGroup.HierarchyType);
			optionalFieldGroupVM.HierarchyTypes = hierarchyTypesList;

			return View(optionalFieldGroupVM);
		}

		// POST: /Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(OptionalFieldGroupVM optionalFieldGroupVM)
		{
			//Get Item
			OptionalFieldGroup optionalFieldGroup = new OptionalFieldGroup();
			optionalFieldGroup = optionalFieldGroupRepository.GetGroup(optionalFieldGroupVM.OptionalFieldGroup.OptionalFieldGroupId);

			//Check Exists
			if (optionalFieldGroup == null)
			{
				ViewData["ActionMethod"] = "EditPost";
				return View("RecordDoesNotExistError");
			}
			//Check Access Rights
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToOptionalFieldGroup(optionalFieldGroup.OptionalFieldGroupId))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}
			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel<OptionalFieldGroup>(optionalFieldGroup, "OptionalFieldGroup");
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

			//Dont check for multiple as We are not editing Hierarchy, we have alrady checked access to the item itself
			if (optionalFieldGroup.HierarchyType != "Multiple")
			{
				string hierarchyCode = optionalFieldGroup.HierarchyCode;
				
				//Check Access Rights to PolicyGroup
				HierarchyRepository hierarchyRepository = new HierarchyRepository();
				if (!hierarchyRepository.AdminHasDomainHierarchyWriteAccess(optionalFieldGroup.HierarchyType, hierarchyCode, optionalFieldGroup.SourceSystemCode, groupName))
				{
					ViewData["Message"] = "You cannot add to this hierarchy item";
					return View("Error");
				}
			}

			//Database Update
			try
			{
				optionalFieldGroupRepository.Edit(optionalFieldGroup);
			}
			catch (SqlException ex)
			{
				//Versioning Error
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/OptionalFieldGroup.mvc/Edit/" + optionalFieldGroup.OptionalFieldGroupId;
					return View("VersionError");
				}
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			return RedirectToAction("ListUnDeleted");
		}

	
		// GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id)
		{
			//Get Item From Database
			OptionalFieldGroup optionalFieldGroup = new OptionalFieldGroup();
			optionalFieldGroup = optionalFieldGroupRepository.GetGroup(id);

			//Check Exists
			if (optionalFieldGroup == null || optionalFieldGroup.DeletedFlag == true)
			{
				ViewData["ActionMethod"] = "DeleteGet";
				return View("RecordDoesNotExistError");
			}
			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToOptionalFieldGroup(id))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			OptionalFieldGroupVM optionalFieldGroupVM = new OptionalFieldGroupVM();

			optionalFieldGroupRepository.EditGroupForDisplay(optionalFieldGroup);
			optionalFieldGroupVM.OptionalFieldGroup = optionalFieldGroup;
		
			return View(optionalFieldGroupVM);
		}

		// POST: /Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(OptionalFieldGroupVM optionalFieldGroupVM)
		{
			//Check Valid Item passed in Form       
			if (optionalFieldGroupVM.OptionalFieldGroup == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Get Item From Database
			OptionalFieldGroup optionalFieldGroup = new OptionalFieldGroup();
			optionalFieldGroup = optionalFieldGroupRepository.GetGroup(optionalFieldGroupVM.OptionalFieldGroup.OptionalFieldGroupId);

			//Check Exists in Databsase
			if (optionalFieldGroup == null || optionalFieldGroup.DeletedFlag == true)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}
			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToOptionalFieldGroup(optionalFieldGroup.OptionalFieldGroupId))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}
			//Delete Form Item
			try
			{
				optionalFieldGroupVM.OptionalFieldGroup.DeletedFlag = true;
				optionalFieldGroupRepository.UpdateGroupDeletedStatus(optionalFieldGroupVM.OptionalFieldGroup);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/OptionalFieldGroup.mvc/Delete/" + optionalFieldGroup.OptionalFieldGroupId;
					return View("VersionError");
				}

				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			return RedirectToAction("ListUnDeleted");
		}

		// GET: /UnDelete
		public ActionResult UnDelete(int id)
		{
			//Get Item From Database
			OptionalFieldGroup optionalFieldGroup = new OptionalFieldGroup();
			optionalFieldGroup = optionalFieldGroupRepository.GetGroup(id);

			//Check Exists
			if (optionalFieldGroup == null || optionalFieldGroup.DeletedFlag == false)
			{
				ViewData["ActionMethod"] = "DeleteGet";
				return View("RecordDoesNotExistError");
			}
			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToOptionalFieldGroup(id))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			OptionalFieldGroupVM optionalFieldGroupVM = new OptionalFieldGroupVM();

			optionalFieldGroupRepository.EditGroupForDisplay(optionalFieldGroup);
			optionalFieldGroupVM.OptionalFieldGroup = optionalFieldGroup;
		
			return View(optionalFieldGroupVM);
		}

		// POST: /UnDelete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult UnDelete(OptionalFieldGroupVM optionalFieldGroupVM)
		{
			//Check Valid Item passed in Form       
			if (optionalFieldGroupVM.OptionalFieldGroup == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Get Item From Database
			OptionalFieldGroup optionalFieldGroup = new OptionalFieldGroup();
			optionalFieldGroup = optionalFieldGroupRepository.GetGroup(optionalFieldGroupVM.OptionalFieldGroup.OptionalFieldGroupId);

			//Check Exists in Databsase
			if (optionalFieldGroup == null || optionalFieldGroup.DeletedFlag == false)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}
			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToOptionalFieldGroup(optionalFieldGroup.OptionalFieldGroupId))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}
			//Delete Form Item
			try
			{
				optionalFieldGroupVM.OptionalFieldGroup.DeletedFlag = false;
				optionalFieldGroupRepository.UpdateGroupDeletedStatus(optionalFieldGroupVM.OptionalFieldGroup);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/OptionalFieldGroup.mvc/UnDelete/" + optionalFieldGroup.OptionalFieldGroupId;
					return View("VersionError");
				}

				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			return RedirectToAction("ListUnDeleted");
		}
	}
}
