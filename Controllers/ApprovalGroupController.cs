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
    public class ApprovalGroupController : Controller
    {
        //main repository
        ApprovalGroupRepository approvalGroupRepository = new ApprovalGroupRepository();
        HierarchyRepository hierarchyRepository = new HierarchyRepository();
        ApprovalGroupApprovalTypeRepository approvalGroupApprovalTypeRepository = new ApprovalGroupApprovalTypeRepository();

        private string groupName = "Approval Group Administrator";

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
                sortField = "ApprovalGroupName";
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

            if (approvalGroupRepository == null)
            {
                ViewData["ActionMethod"] = "ListUnDeletedGet";
                return View("Error");
            }

            var cwtPaginatedList = approvalGroupRepository.PageApprovalGroups(false, page ?? 1, filter ?? "", sortField, sortOrder ?? 0);

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
            if (sortField != "HierarchyType" && sortField != "EnabledDate" && sortField != "LinkedItemCount")
            {
                sortField = "ApprovalGroupName";
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
            var cwtPaginatedList = approvalGroupRepository.PageApprovalGroups(true, page ?? 1, filter ?? "", sortField, sortOrder ?? 0);
            return View(cwtPaginatedList);
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
            sortField = "ApprovalGroupName";
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
            var cwtPaginatedList = approvalGroupRepository.PageOrphanedApprovalGroups(page ?? 1, filter ?? "", sortField, sortOrder ?? 0);
            return View(cwtPaginatedList);
        }

        // GET: /View
        public ActionResult View(int id)
        {
            ApprovalGroup group = new ApprovalGroup();
            group = approvalGroupRepository.GetGroup(id);

            //Check Exists
            if (group == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            approvalGroupRepository.EditGroupForDisplay(group);

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

            //ApprovalGroupApprovalTypeItems
            SelectList approvalGroupApprovalTypes = new SelectList(approvalGroupApprovalTypeRepository.GetAllApprovalGroupApprovalTypes().ToList(), "ApprovalGroupApprovalTypeId", "ApprovalGroupApprovalTypeDescription");
            ViewData["ApprovalGroupApprovalTypes"] = approvalGroupApprovalTypes;

            ApprovalGroup group = new ApprovalGroup();

            return View(group);
        }

        // POST: /Create/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ApprovalGroup group, FormCollection collection)
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

            //Create Approval Group Approval Type Items from Post values
            System.Data.Linq.EntitySet<ApprovalGroupApprovalTypeItem> approvalGroupApprovalTypeItems = new System.Data.Linq.EntitySet<ApprovalGroupApprovalTypeItem>();

            foreach (string key in collection)
            {
                if (key.StartsWith("ApprovalGroupApprovalTypeItem") && !string.IsNullOrEmpty(collection[key]))
                {
                    string[] values = collection[key].Split(',');

                    if (values[0] != null && values[1] != null && !string.IsNullOrEmpty(values[0]) && !string.IsNullOrEmpty(values[1]))
                    {
                        ApprovalGroupApprovalTypeItem approvalGroupApprovalTypeItem = new ApprovalGroupApprovalTypeItem()
                        {
                            ApprovalGroupApprovalTypeId = int.Parse(values[0]),
                            ApprovalGroupApprovalTypeItemValue = values[1]
                        };
                        approvalGroupApprovalTypeItems.Add(approvalGroupApprovalTypeItem);
                    }
                }
            }

            //Add Approval Group Approval Type Items
            group.ApprovalGroupApprovalTypeItems = approvalGroupApprovalTypeItems;

            //Database Update
            try
            {
                approvalGroupRepository.Add(group);
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
            ApprovalGroup group = new ApprovalGroup();
            group = approvalGroupRepository.GetGroup(id);

            //Check Exists
            if (group == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }
            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToApprovalGroup(id))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            TablesDomainHierarchyLevelRepository tablesDomainHierarchyLevelRepository = new TablesDomainHierarchyLevelRepository();
            SelectList hierarchyTypesList = new SelectList(tablesDomainHierarchyLevelRepository.GetDomainHierarchies(groupName).ToList(), "HierarchyLevelTableName", "HierarchyLevelTableName");
            ViewData["HierarchyTypes"] = hierarchyTypesList;

            //ApprovalGroupApprovalTypeItems
            SelectList approvalGroupApprovalTypes = new SelectList(approvalGroupApprovalTypeRepository.GetAllApprovalGroupApprovalTypes().ToList(), "ApprovalGroupApprovalTypeId", "ApprovalGroupApprovalTypeDescription");
            ViewData["ApprovalGroupApprovalTypes"] = approvalGroupApprovalTypes;

            approvalGroupRepository.EditGroupForDisplay(group);

            return View(group);
        }

        // POST: /Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection collection)
        {
            //Get Item From Database
            ApprovalGroup group = new ApprovalGroup();
            group = approvalGroupRepository.GetGroup(id);

            //Check Exists
            if (group == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToApprovalGroup(group.ApprovalGroupId))
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

                //Check Access Rights to Hierarchy
                HierarchyRepository hierarchyRepository = new HierarchyRepository();
                if (!hierarchyRepository.AdminHasDomainHierarchyWriteAccess(group.HierarchyType, hierarchyCode, group.SourceSystemCode, groupName))
                {
                    ViewData["Message"] = "You cannot add to this hierarchy item";
                    return View("Error");
                }
            }

            //Create Approval Group Approval Type Items from Post values
            System.Data.Linq.EntitySet<ApprovalGroupApprovalTypeItem> approvalGroupApprovalTypeItems = new System.Data.Linq.EntitySet<ApprovalGroupApprovalTypeItem>();

            foreach (string key in collection)
            {
                if (key.StartsWith("ApprovalGroupApprovalTypeItem") && !string.IsNullOrEmpty(collection[key]))
                {
                    string[] values = collection[key].Split(',');

                    if (values[0] != null && values[1] != null && !string.IsNullOrEmpty(values[0]) && !string.IsNullOrEmpty(values[1]))
                    {
                        ApprovalGroupApprovalTypeItem approvalGroupApprovalTypeItem = new ApprovalGroupApprovalTypeItem()
                        {
                            ApprovalGroupApprovalTypeId = int.Parse(values[0]),
                            ApprovalGroupApprovalTypeItemValue = values[1]
                        };
                        approvalGroupApprovalTypeItems.Add(approvalGroupApprovalTypeItem);
                    }
                }
            }

            //Remove Approval Group Approval Type Items if not set, otherwise add new ones in
            group.ApprovalGroupApprovalTypeItems = (approvalGroupApprovalTypeItems != null && approvalGroupApprovalTypeItems.Count > 0) ? approvalGroupApprovalTypeItems : null;

            //Database Update
            try
            {
                approvalGroupRepository.Edit(group);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/ApprovalGroup.mvc/Edit/" + group.ApprovalGroupId.ToString();
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
			ApprovalGroup group = new ApprovalGroup();
			group = approvalGroupRepository.GetGroup(id);

			//Check Exists
			if (group == null || group.DeletedFlag == true)
			{
				ViewData["ActionMethod"] = "DeleteGet";
				return View("RecordDoesNotExistError");
			}

			//Check AccessRights
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToApprovalGroup(id))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			approvalGroupRepository.EditGroupForDisplay(group);

			return View(group);
		}

		// POST: /Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, FormCollection collection)
		{
			//Get Item From Database
			ApprovalGroup group = new ApprovalGroup();
			group = approvalGroupRepository.GetGroup(id);

			//Check Exists
			if (group == null || group.DeletedFlag == true)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}
			
			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToApprovalGroup(id))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Delete Item
			try
			{
				group.VersionNumber = Int32.Parse(collection["VersionNumber"]);
				group.DeletedFlag = true;
				approvalGroupRepository.UpdateGroupDeletedStatus(group);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/ApprovalGroup.mvc/Delete/" + group.ApprovalGroupId;
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
			ApprovalGroup group = new ApprovalGroup();
			group = approvalGroupRepository.GetGroup(id);

			//Check Exists
			if (group == null || group.DeletedFlag == false)
			{
				ViewData["ActionMethod"] = "UnDeleteGet";
				return View("RecordDoesNotExistError");
			}
			//Check AccessRights
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToApprovalGroup(id))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			approvalGroupRepository.EditGroupForDisplay(group);

			return View(group);
		}

		// POST: /UnDelete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult UnDelete(int id, FormCollection collection)
		{
			//Get Item From Database
			ApprovalGroup group = new ApprovalGroup();
			group = approvalGroupRepository.GetGroup(id);

			//Check Exists
			if (group == null || group.DeletedFlag == false)
			{
				ViewData["ActionMethod"] = "UnDeletePost";
				return View("RecordDoesNotExistError");
			}
			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToApprovalGroup(id))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}            
			//Delete Item
			try
			{
				group.VersionNumber = Int32.Parse(collection["VersionNumber"]);
				group.DeletedFlag = false;
				approvalGroupRepository.UpdateGroupDeletedStatus(group);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/ApprovalGroup.mvc/UnDelete/" + group.ApprovalGroupId;
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
