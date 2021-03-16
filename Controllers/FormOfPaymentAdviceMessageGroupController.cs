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
	public class FormOfPaymentAdviceMessageGroupController : Controller
	{
		//main repository
		FormOfPaymentAdviceMessageGroupRepository formOfPaymentAdviceMessageGroupRepository = new FormOfPaymentAdviceMessageGroupRepository();
		HierarchyRepository hierarchyRepository = new HierarchyRepository();

		private string groupName = "Client Detail";

		// GET: /ListUnDeleted
		public ActionResult ListUnDeleted(string filter, int? page, string sortField, int? sortOrder)
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
				sortField = "FormOfPaymentAdviceMessageGroupName";
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

			if (formOfPaymentAdviceMessageGroupRepository == null)
			{
				ViewData["ActionMethod"] = "ListUnDeletedGet";
				return View("Error");
			}

			var cwtPaginatedList = formOfPaymentAdviceMessageGroupRepository.PageFormOfPaymentAdviceMessageGroups(false, page ?? 1, filter ?? "", sortField, sortOrder ?? 0);

			if (cwtPaginatedList == null)
			{
				ViewData["ActionMethod"] = "ListUnDeletedGet";
				return View("Error");
			}
			
			//return items
			return View(cwtPaginatedList);  
		}

		// GET: /ListDeleted
		public ActionResult ListDeleted(string filter, int? page, string sortField, int? sortOrder)
		{
			//Set Access Rights
			ViewData["Access"] = "";
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Access"] = "WriteAccess";
			}

			//SortField
			if (sortField != "HierarchyType" && sortField != "EnabledDate" && sortField !="LinkedItemCount")
			{
				sortField = "FormOfPaymentAdviceMessageGroupName";
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


			//return items
			var cwtPaginatedList = formOfPaymentAdviceMessageGroupRepository.PageFormOfPaymentAdviceMessageGroups(true, page ?? 1, filter ?? "", sortField, sortOrder ?? 0);
			return View(cwtPaginatedList);
		}

		// GET: /View
		public ActionResult View(int id)
		{
			FormOfPaymentAdviceMessageGroup group = new FormOfPaymentAdviceMessageGroup();
			group = formOfPaymentAdviceMessageGroupRepository.GetGroup(id);

			//Check Exists
			if (group == null)
			{
				ViewData["ActionMethod"] = "ViewGet";
				return View("RecordDoesNotExistError");
			}

			formOfPaymentAdviceMessageGroupRepository.EditGroupForDisplay(group);

			return View(group);
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
		
			TablesDomainHierarchyLevelRepository tablesDomainHierarchyLevelRepository = new TablesDomainHierarchyLevelRepository();
			SelectList hierarchyTypesList = new SelectList(tablesDomainHierarchyLevelRepository.GetDomainHierarchies(groupName).ToList(), "HierarchyLevelTableName", "HierarchyLevelTableName");
			ViewData["HierarchyTypes"] = hierarchyTypesList;

			FormOfPaymentAdviceMessageGroup group = new FormOfPaymentAdviceMessageGroup();

			return View(group);
		}

		// POST: /Create/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(FormOfPaymentAdviceMessageGroup group)
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}            
			//Check Access Rights to Domain Hierarchy
			if (!hierarchyRepository.AdminHasDomainHierarchyWriteAccess(group.HierarchyType, group.HierarchyCode, group.SourceSystemCode, groupName))
			{
				ViewData["Message"] = "You cannot add to this hierarchy item";
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

			//ClientSubUnitTravelerType has extra field
			string hierarchyCode = group.HierarchyCode;
			if (group.HierarchyType == "ClientSubUnitTravelerType")
			{
				group.ClientSubUnitGuid = hierarchyCode;  //ClientSubUnitTravelerType has 2 primarykeys
			}

			//Database Update
			try
			{
				formOfPaymentAdviceMessageGroupRepository.Add(group);
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
			FormOfPaymentAdviceMessageGroup group = new FormOfPaymentAdviceMessageGroup();
			group = formOfPaymentAdviceMessageGroupRepository.GetGroup(id);

			//Check Exists
			if (group == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}
			//Check Access
			HierarchyRepository hierarchyRepository = new HierarchyRepository();
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			TablesDomainHierarchyLevelRepository tablesDomainHierarchyLevelRepository = new TablesDomainHierarchyLevelRepository();
			SelectList hierarchyTypesList = new SelectList(tablesDomainHierarchyLevelRepository.GetDomainHierarchies(groupName).ToList(), "HierarchyLevelTableName", "HierarchyLevelTableName");
			ViewData["HierarchyTypes"] = hierarchyTypesList;

			formOfPaymentAdviceMessageGroupRepository.EditGroupForDisplay(group);

			return View(group);
		}

		// POST: /Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, FormCollection collection)
		{
			//Get Item From Database
			FormOfPaymentAdviceMessageGroup group = new FormOfPaymentAdviceMessageGroup();
			group = formOfPaymentAdviceMessageGroupRepository.GetGroup(id);

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

			if (group.HierarchyType != "Multiple")
			{
				//ClientSubUnitTravelerType has extra field
				string hierarchyCode = group.HierarchyCode;
				if (group.HierarchyType == "ClientSubUnitTravelerType")
				{
					group.ClientSubUnitGuid = hierarchyCode;  //ClientSubUnitTravelerType has 2 primarykeys
				}
				
				//Check Access Rights to PolicyGroup
				if (!hierarchyRepository.AdminHasDomainHierarchyWriteAccess(group.HierarchyType, hierarchyCode, group.SourceSystemCode, groupName))
				{
					ViewData["Message"] = "You cannot add to this hierarchy item";
					return View("Error");
				}
			}

			//Database Update
			try
			{
				formOfPaymentAdviceMessageGroupRepository.Edit(group);
			}
			catch (SqlException ex)
			{
				//Versioning Error
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/FormOfPaymentAdviceMessageGroup.mvc/Edit/" + group.FormOfPaymentAdviceMessageGroupID.ToString();
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
			FormOfPaymentAdviceMessageGroup group = new FormOfPaymentAdviceMessageGroup();
			group = formOfPaymentAdviceMessageGroupRepository.GetGroup(id);

			//Check Exists
			if (group == null || group.DeletedFlag == true)
			{
				ViewData["ActionMethod"] = "DeleteGet";
				return View("RecordDoesNotExistError");
			}

			//Check AccessRights
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			formOfPaymentAdviceMessageGroupRepository.EditGroupForDisplay(group);

			return View(group);
		}

		// POST: /Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, FormCollection collection)
		{
			//Get Item From Database
			FormOfPaymentAdviceMessageGroup group = new FormOfPaymentAdviceMessageGroup();
			group = formOfPaymentAdviceMessageGroupRepository.GetGroup(id);

			//Check Exists
			if (group == null || group.DeletedFlag == true)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}
			
			//Check Access
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Delete Item
			try
			{
				group.VersionNumber = Int32.Parse(collection["VersionNumber"]);
				group.DeletedFlag = true;
				formOfPaymentAdviceMessageGroupRepository.UpdateGroupDeletedStatus(group);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/FormOfPaymentAdviceMessageGroup.mvc/Delete/" + group.FormOfPaymentAdviceMessageGroupID;
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
			FormOfPaymentAdviceMessageGroup group = new FormOfPaymentAdviceMessageGroup();
			group = formOfPaymentAdviceMessageGroupRepository.GetGroup(id);

			//Check Exists
			if (group == null || group.DeletedFlag == false)
			{
				ViewData["ActionMethod"] = "UnDeleteGet";
				return View("RecordDoesNotExistError");
			}
			//Check AccessRights
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			formOfPaymentAdviceMessageGroupRepository.EditGroupForDisplay(group);

			return View(group);
		}

		// POST: /UnDelete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult UnDelete(int id, FormCollection collection)
		{
			//Get Item From Database
			FormOfPaymentAdviceMessageGroup group = new FormOfPaymentAdviceMessageGroup();
			group = formOfPaymentAdviceMessageGroupRepository.GetGroup(id);

			//Check Exists
			if (group == null || group.DeletedFlag == false)
			{
				ViewData["ActionMethod"] = "UnDeletePost";
				return View("RecordDoesNotExistError");
			}
			//Check Access
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}            
			//Delete Item
			try
			{
				group.VersionNumber = Int32.Parse(collection["VersionNumber"]);
				group.DeletedFlag = false;
				formOfPaymentAdviceMessageGroupRepository.UpdateGroupDeletedStatus(group);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/FormOfPaymentAdviceMessageGroup.mvc/UnDelete/" + group.FormOfPaymentAdviceMessageGroupID;
					return View("VersionError");
				}
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			return RedirectToAction("ListDeleted");
		}

		[HttpPost]
		public JsonResult AutoCompleteClientSubUnitTravelerTypes(string searchText, string hierarchyItem, string filterText)
		{
			HierarchyRepository hierarchyRepository = new HierarchyRepository();

			int maxResults = 15;
			var result = hierarchyRepository.LookUpSystemUserClientSubUnitTravelerTypes(searchText, hierarchyItem, maxResults, groupName, filterText);
			return Json(result);
		}
    }
}
