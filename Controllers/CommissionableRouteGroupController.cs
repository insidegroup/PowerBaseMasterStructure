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
    public class CommissionableRouteGroupController : Controller
    {
        //main repository
        CommissionableRouteGroupRepository CommissionableRouteGroupRepository = new CommissionableRouteGroupRepository();
        HierarchyRepository hierarchyRepository = new HierarchyRepository();
		private string groupName = "Commissionable Route";

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
				sortField = "CommissionableRouteGroupName";
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

			if (CommissionableRouteGroupRepository == null)
			{
				ViewData["ActionMethod"] = "ListUnDeletedGet";
				return View("Error");
			}

			var cwtPaginatedList = CommissionableRouteGroupRepository.PageCommissionableRouteGroups(false, page ?? 1, filter ?? "", sortField, sortOrder ?? 0);

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
                sortField = "CommissionableRouteGroupName";
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
            var cwtPaginatedList = CommissionableRouteGroupRepository.PageCommissionableRouteGroups(true, page ?? 1, filter ?? "", sortField, sortOrder ?? 0);
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
            sortField = "CommissionableRouteGroupName";
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
            var cwtPaginatedList = CommissionableRouteGroupRepository.PageOrphanedCommissionableRouteGroups(  page ?? 1, filter ?? "", sortField, sortOrder ?? 0);
            return View(cwtPaginatedList);
        }

        // GET: /View
        public ActionResult View(int id)
        {
            CommissionableRouteGroup group = new CommissionableRouteGroup();
            group = CommissionableRouteGroupRepository.GetGroup(id);

            //Check Exists
            if (group == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            CommissionableRouteGroupRepository.EditGroupForDisplay(group);

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
            TripTypeRepository tripTypeRepository = new TripTypeRepository();
            SelectList tripTypesList = new SelectList(tripTypeRepository.GetAllTripTypes().ToList(), "TripTypeId", "TripTypeDescription");
            ViewData["TripTypes"] = tripTypesList;

            TablesDomainHierarchyLevelRepository tablesDomainHierarchyLevelRepository = new TablesDomainHierarchyLevelRepository();
            SelectList hierarchyTypesList = new SelectList(tablesDomainHierarchyLevelRepository.GetDomainHierarchies(groupName).ToList(), "HierarchyLevelTableName", "HierarchyLevelTableName");
            ViewData["HierarchyTypes"] = hierarchyTypesList;

            CommissionableRouteGroup group = new CommissionableRouteGroup();

			return View(group);
        }

        // POST: /Create/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CommissionableRouteGroup group)
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
                CommissionableRouteGroupRepository.Add(group);
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
            CommissionableRouteGroup group = new CommissionableRouteGroup();
            group = CommissionableRouteGroupRepository.GetGroup(id);

            //Check Exists
            if (group == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }
            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToCommissionableRouteGroup(id))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
            TripTypeRepository tripTypeRepository = new TripTypeRepository();
            SelectList tripTypesList = new SelectList(tripTypeRepository.GetAllTripTypes().ToList(), "TripTypeId", "TripTypeDescription");
            ViewData["TripTypes"] = tripTypesList;

            TablesDomainHierarchyLevelRepository tablesDomainHierarchyLevelRepository = new TablesDomainHierarchyLevelRepository();
            SelectList hierarchyTypesList = new SelectList(tablesDomainHierarchyLevelRepository.GetDomainHierarchies(groupName).ToList(), "HierarchyLevelTableName", "HierarchyLevelTableName");
            ViewData["HierarchyTypes"] = hierarchyTypesList;

            CommissionableRouteGroupRepository.EditGroupForDisplay(group);
            return View(group);
        }

        // POST: /Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection collection)
        {
            //Get Item From Database
            CommissionableRouteGroup group = new CommissionableRouteGroup();
            group = CommissionableRouteGroupRepository.GetGroup(id);

            //Check Exists
            if (group == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }
            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToCommissionableRouteGroup(group.CommissionableRouteGroupId))
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

            //ClientSubUnitTravelerType has extra field
            string hierarchyCode = group.HierarchyCode;
            if (group.HierarchyType == "ClientSubUnitTravelerType")
            {
                group.ClientSubUnitGuid = hierarchyCode;  //ClientSubUnitTravelerType has 2 primarykeys
            }
            //Check Access Rights to PolicyGroup
            HierarchyRepository hierarchyRepository = new HierarchyRepository();
            if (!hierarchyRepository.AdminHasDomainHierarchyWriteAccess(group.HierarchyType, hierarchyCode, group.SourceSystemCode, groupName))
            {
                ViewData["Message"] = "You cannot add to this hierarchy item";
                return View("Error");
            }

            //Database Update
            try
            {
                CommissionableRouteGroupRepository.Edit(group);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/CommissionableRouteGroup.mvc/Edit/" + group.CommissionableRouteGroupId.ToString();
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
            CommissionableRouteGroup group = new CommissionableRouteGroup();
            group = CommissionableRouteGroupRepository.GetGroup(id);

            //Check Exists
            if (group == null || group.DeletedFlag == true)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }
            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToCommissionableRouteGroup(id))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
            CommissionableRouteGroupRepository.EditGroupForDisplay(group);
            return View(group);
        }

        // POST: /Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            //Get Item From Database
            CommissionableRouteGroup group = new CommissionableRouteGroup();
            group = CommissionableRouteGroupRepository.GetGroup(id);

            //Check Exists
            if (group == null || group.DeletedFlag == true)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }
            
			//Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToCommissionableRouteGroup(id))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }            
            
			//Delete Item
            try
            {
                group.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                group.DeletedFlag = true;
                CommissionableRouteGroupRepository.UpdateGroupDeletedStatus(group);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/CommissionableRouteGroup.mvc/Delete/" + group.CommissionableRouteGroupId;
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
            CommissionableRouteGroup group = new CommissionableRouteGroup();
            group = CommissionableRouteGroupRepository.GetGroup(id);

            //Check Exists
            if (group == null || group.DeletedFlag == false)
            {
                ViewData["ActionMethod"] = "UnDeleteGet";
                return View("RecordDoesNotExistError");
            }
            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToCommissionableRouteGroup(id))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
            CommissionableRouteGroupRepository.EditGroupForDisplay(group);
            return View(group);
        }

        // POST: /UnDelete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UnDelete(int id, FormCollection collection)
        {
            //Get Item From Database
            CommissionableRouteGroup group = new CommissionableRouteGroup();
            group = CommissionableRouteGroupRepository.GetGroup(id);

            //Check Exists
            if (group == null || group.DeletedFlag == false)
            {
                ViewData["ActionMethod"] = "UnDeletePost";
                return View("RecordDoesNotExistError");
            }
            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToCommissionableRouteGroup(id))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }            
            //Delete Item
            try
            {
                group.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                group.DeletedFlag = false;
                CommissionableRouteGroupRepository.UpdateGroupDeletedStatus(group);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/CommissionableRouteGroup.mvc/UnDelete/" + group.CommissionableRouteGroupId;
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

		//// GET: Linked ClientSubUnits
		//public ActionResult LinkedClientSubUnits(int id)
		//{
		//	//Get Group From Database
		//	CommissionableRouteGroup group = new CommissionableRouteGroup();
		//	group = CommissionableRouteGroupRepository.GetGroup(id);

		//	//Check Exists
		//	if (group == null)
		//	{
		//		ViewData["ActionMethod"] = "ClientSubUnitGet";
		//		return View("RecordDoesNotExistError");
		//	}
		//	CommissionableRouteGroupClientSubUnitCountriesVM CommissionableRouteGroupClientSubUnits = new CommissionableRouteGroupClientSubUnitCountriesVM();
		//	CommissionableRouteGroupClientSubUnits.CommissionableRouteGroupId = id;
		//	CommissionableRouteGroupClientSubUnits.CommissionableRouteGroupName = group.CommissionableRouteGroupName;

		//	List<CommissionableRouteClientSubUnitCountryVM> clientSubUnitsAvailable = new List<CommissionableRouteClientSubUnitCountryVM>();
		//	clientSubUnitsAvailable = CommissionableRouteGroupRepository.GetLinkedClientSubUnits(id, false);
		//	CommissionableRouteGroupClientSubUnits.ClientSubUnitsAvailable = clientSubUnitsAvailable;

		//	List<CommissionableRouteClientSubUnitCountryVM> clientSubUnitsUnAvailable = new List<CommissionableRouteClientSubUnitCountryVM>();
		//	clientSubUnitsUnAvailable = CommissionableRouteGroupRepository.GetLinkedClientSubUnits(id, true);
		//	CommissionableRouteGroupClientSubUnits.ClientSubUnitsUnAvailable = clientSubUnitsUnAvailable;

		//	return View(CommissionableRouteGroupClientSubUnits);
		//}

		//// POST: Linked ClientSubUnits
		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public ActionResult LinkedClientSubUnits(int CommissionableRouteGroupId, string ClientSubUnitGuid)
		//{
		//	//Get Item From Database
		//	CommissionableRouteGroup group = new CommissionableRouteGroup();
		//	group = CommissionableRouteGroupRepository.GetGroup(CommissionableRouteGroupId);

		//	//Check Exists
		//	if (group == null)
		//	{
		//		ViewData["ActionMethod"] = "ClientSubUnitPost";
		//		return View("RecordDoesNotExistError");
		//	}
		//	//Check Access Rights
		//	RolesRepository rolesRepository = new RolesRepository();
		//	if (!rolesRepository.HasWriteAccessToCommissionableRouteGroup(CommissionableRouteGroupId))
		//	{
		//		ViewData["Message"] = "You do not have access to this item";
		//		return View("Error");
		//	}

		//	//Database Update
		//	try
		//	{
		//		CommissionableRouteGroupRepository.UpdateLinkedClientSubUnit(CommissionableRouteGroupId, ClientSubUnitGuid);
		//	}
		//	catch (SqlException ex)
		//	{
		//		//Versioning Error
		//		if (ex.Message == "SQLVersioningError")
		//		{
		//			ViewData["ReturnURL"] = "/CommissionableRouteGroup.mvc/ClientSubUnit/" + CommissionableRouteGroupId.ToString();
		//			return View("VersionError");
		//		}
		//		LogRepository logRepository = new LogRepository();
		//		logRepository.LogError(ex.Message);

		//		ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
		//		return View("Error");
		//	}


		//	return RedirectToAction("LinkedClientSubUnits", new { id = CommissionableRouteGroupId });
		//}

    }
}
