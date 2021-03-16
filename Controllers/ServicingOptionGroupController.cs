using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Repository;
using System.Data.SqlClient;

namespace CWTDesktopDatabase.Controllers
{
    public class ServicingOptionGroupController : Controller
    {
        //main repositories
        ServicingOptionGroupRepository servicingOptionGroupRepository = new ServicingOptionGroupRepository();
        HierarchyRepository hierarchyRepository = new HierarchyRepository();
        private string groupName = "Servicing Option";
       
        public ActionResult ListUnDeleted(string filter, int? page, string sortField, int? sortOrder)
        {
            //Set Access Rights
            ViewData["Access"] = "";
            if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //SortField
            if (sortField != "HierarchyType" && sortField != "LinkedItemCount")
            {
                sortField = "ServicingOptionGroupName";
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

			if (servicingOptionGroupRepository == null)
			{
				ViewData["ActionMethod"] = "ListUnDeletedGet";
				return View("Error");
			}

			var cwtPaginatedList = servicingOptionGroupRepository.PageServicingOptionGroups(false, page ?? 1, filter ?? "", sortField, sortOrder ?? 0);

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
                sortField = "ServicingOptionGroupName";
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
            var cwtPaginatedList = servicingOptionGroupRepository.PageServicingOptionGroups(true, page ?? 1, filter ?? "", sortField, sortOrder ?? 0);
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
            sortField = "ServicingOptionGroupName";
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
            var cwtPaginatedList = servicingOptionGroupRepository.PageOrphanedServicingOptionGroups(  page ?? 1, filter ?? "", sortField, sortOrder ?? 0);
            return View(cwtPaginatedList);
        }

        // GET: /View
        public ActionResult View(int id)
        {
            ServicingOptionGroup group = new ServicingOptionGroup();
            group = servicingOptionGroupRepository.GetGroup(id);

            //Check Exists
            if (group == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            servicingOptionGroupRepository.EditGroupForDisplay(group);
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

			List<Meeting> meetings = new List<Meeting>();
			SelectList meetingsList = new SelectList(meetings.ToList(), "MeetingID", "MeetingName");
			ViewData["Meetings"] = meetingsList;

            ServicingOptionGroup group = new ServicingOptionGroup();
            return View(group);
        }

        // POST: /Create/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ServicingOptionGroup group)
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
                servicingOptionGroupRepository.Add(group);
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
            ServicingOptionGroup group = new ServicingOptionGroup();
            group = servicingOptionGroupRepository.GetGroup(id);

            //Check Exists
            if (group == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToServicingOptionGroup(id))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            TripTypeRepository tripTypeRepository = new TripTypeRepository();
            SelectList tripTypesList = new SelectList(tripTypeRepository.GetAllTripTypes().ToList(), "TripTypeId", "TripTypeDescription", group.TripTypeId);
            ViewData["TripTypes"] = tripTypesList;

            TablesDomainHierarchyLevelRepository tablesDomainHierarchyLevelRepository = new TablesDomainHierarchyLevelRepository();
            SelectList hierarchyTypesList = new SelectList(tablesDomainHierarchyLevelRepository.GetDomainHierarchies(groupName).ToList(), "HierarchyLevelTableName", "HierarchyLevelTableName");
            ViewData["HierarchyTypes"] = hierarchyTypesList;

            servicingOptionGroupRepository.EditGroupForDisplay(group);

			//Meetings
			MeetingRepository meetingRepository = new MeetingRepository();
			List<Meeting> meetings = meetingRepository.GetAvailableMeetings(group.HierarchyType, group.HierarchyCode, null, group.SourceSystemCode, group.TravelerTypeGuid);
			SelectList meetingsList = new SelectList(meetings.ToList(), "MeetingID", "MeetingDisplayName", group.MeetingID != null ? group.MeetingID.ToString() : "");
			ViewData["Meetings"] = meetingsList;

            return View(group);
        }


        // POST: /Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection collection)
        {
            //Get Item From Database
            ServicingOptionGroup group = new ServicingOptionGroup();
            group = servicingOptionGroupRepository.GetGroup(id);

            //Check Exists
            if (group == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }
            //Check Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToServicingOptionGroup(id))
            {
		        ViewData["Message"] = "You do not have access to this item";
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

            //Dont check for multiple as We are not editing Hierarchy, we have alrady checked access to the item itself
			if (group.HierarchyType != "Multiple")
			{
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
			}

            //Database Update
            try
            {
                servicingOptionGroupRepository.Edit(group);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/WorkFlowGroup.mvc/Edit/" + group.ServicingOptionGroupId.ToString();
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
            ServicingOptionGroup group = new ServicingOptionGroup();
            group = servicingOptionGroupRepository.GetGroup(id);

            //Check Exists
            if (group == null || group.DeletedFlag == true)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }
            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToServicingOptionGroup(id))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
            servicingOptionGroupRepository.EditGroupForDisplay(group);
            return View(group);
        }

        // POST: /Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            
            //Get Item From Database
            ServicingOptionGroup group = new ServicingOptionGroup();
            group = servicingOptionGroupRepository.GetGroup(id);

            //Check Exists
            if (group == null || group.DeletedFlag == true)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }
            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToServicingOptionGroup(id))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Delete Item
            try
            {
                group.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                group.DeletedFlag = true;
                servicingOptionGroupRepository.UpdateGroupDeletedStatus(group);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/ServicingOPtionGroup.mvc/Delete/" + group.ServicingOptionGroupId;
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
            ServicingOptionGroup group = new ServicingOptionGroup();
            group = servicingOptionGroupRepository.GetGroup(id);

            //Check Exists
            if (group == null || group.DeletedFlag == false)
            {
                ViewData["ActionMethod"] = "UnDeleteGet";
                return View("RecordDoesNotExistError");
            }
            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToServicingOptionGroup(id))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
            servicingOptionGroupRepository.EditGroupForDisplay(group);
            return View(group);
        }

        // POST: /UnDelete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UnDelete(int id, FormCollection collection)
        {
            //Get Item From Database
            ServicingOptionGroup group = new ServicingOptionGroup();
            group = servicingOptionGroupRepository.GetGroup(id);

            //Check Exists
            if (group == null || group.DeletedFlag == false)
            {
                ViewData["ActionMethod"] = "UnDeletePost";
                return View("RecordDoesNotExistError");
            }
            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToServicingOptionGroup(id))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Delete Item
            try
            {
                group.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                group.DeletedFlag = false;
                servicingOptionGroupRepository.UpdateGroupDeletedStatus(group);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/ServicingOptionGroup.mvc/UnDelete/" + group.ServicingOptionGroupId;
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

            int maxResults = 500;
            var result = hierarchyRepository.LookUpSystemUserClientSubUnitTravelerTypes(searchText, hierarchyItem, maxResults, groupName, filterText);
            return Json(result);
        }

        // GET: Linked ClientSubUnits
        public ActionResult LinkedClientSubUnits(int id)
        {
            //Get Group From Database
            ServicingOptionGroup group = new ServicingOptionGroup();
            group = servicingOptionGroupRepository.GetGroup(id);

            //Check Exists
            if (group == null)
            {
                ViewData["ActionMethod"] = "ClientSubUnitGet";
                return View("RecordDoesNotExistError");
            }

            ServicingOptionGroupClientSubUnitCountriesVM servicingOptionGroupClientSubUnits = new ServicingOptionGroupClientSubUnitCountriesVM();
            servicingOptionGroupClientSubUnits.ServicingOptionGroupId = id;
            servicingOptionGroupClientSubUnits.ServicingOptionGroupName = group.ServicingOptionGroupName;

            List<ClientSubUnitCountryVM> clientSubUnitsAvailable = new List<ClientSubUnitCountryVM>();
            clientSubUnitsAvailable = servicingOptionGroupRepository.GetLinkedClientSubUnits(id, false);
            servicingOptionGroupClientSubUnits.ClientSubUnitsAvailable = clientSubUnitsAvailable;

            List<ClientSubUnitCountryVM> clientSubUnitsUnAvailable = new List<ClientSubUnitCountryVM>();
            clientSubUnitsUnAvailable = servicingOptionGroupRepository.GetLinkedClientSubUnits(id, true);
            servicingOptionGroupClientSubUnits.ClientSubUnitsUnAvailable = clientSubUnitsUnAvailable;

            //Check User Access
            servicingOptionGroupClientSubUnits.HasWriteAccess = false;
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToServicingOptionGroup(id))
            {
                servicingOptionGroupClientSubUnits.HasWriteAccess = true;
            }

            return View(servicingOptionGroupClientSubUnits);
        }

        // POST: Linked ClientSubUnits
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkedClientSubUnits(int ServicingOptionGroupId, string ClientSubUnitGuid)
        {
            //Get Item From Database
            ServicingOptionGroup group = new ServicingOptionGroup();
            group = servicingOptionGroupRepository.GetGroup(ServicingOptionGroupId);

            //Check Exists
            if (group == null)
            {
                ViewData["ActionMethod"] = "ClientSubUnitPost";
                return View("RecordDoesNotExistError");
            }
            //Check Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToServicingOptionGroup(ServicingOptionGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Database Update
            try
            {
                servicingOptionGroupRepository.UpdateLinkedClientSubUnit(ServicingOptionGroupId, ClientSubUnitGuid);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/ServicingOptionGroup.mvc/ClientSubUnit/" + ServicingOptionGroupId.ToString();
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            return RedirectToAction("LinkedClientSubUnits", new { id = ServicingOptionGroupId });
        }



    }
}
