using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Repository;
using System.Data.SqlClient;

namespace CWTDesktopDatabase.Controllers
{
    public class FollowUpQueueGroupController : Controller
    {
        //main repository
        QueueMinderGroupRepository queueMinderGroupRepository = new QueueMinderGroupRepository();
        HierarchyRepository hierarchyRepository = new HierarchyRepository();
        private string groupName = "Queue";

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
                sortField = "QueueMinderGroupName";
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

			if (queueMinderGroupRepository == null)
			{
				ViewData["ActionMethod"] = "ListUnDeletedGet";
				return View("Error");
			}

			var cwtPaginatedList = queueMinderGroupRepository.PageQueueMinderGroups(false, page ?? 1, filter ?? "", sortField, sortOrder ?? 0);

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
			if (string.IsNullOrEmpty(sortField))
            {
                sortField = "QueueMinderGroupName";
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
            var cwtPaginatedList = queueMinderGroupRepository.PageQueueMinderGroups(true, page ?? 1, filter ?? "", sortField, sortOrder ?? 0);
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
            sortField = "QueueMinderGroupName";
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
            var cwtPaginatedList = queueMinderGroupRepository.PageOrphanedQueueMinderGroups(  page ?? 1, filter ?? "", sortField, sortOrder ?? 0);
            return View(cwtPaginatedList);
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
            QueueMinderGroupVM queueMinderGroupVM = new QueueMinderGroupVM();
            QueueMinderGroup queueMinderGroup = new QueueMinderGroup();
            queueMinderGroupVM.QueueMinderGroup = queueMinderGroup;


            TripTypeRepository tripTypeRepository = new TripTypeRepository();
            SelectList tripTypesList = new SelectList(tripTypeRepository.GetAllTripTypes().ToList(), "TripTypeId", "TripTypeDescription");
            queueMinderGroupVM.TripTypes = tripTypesList;

            TablesDomainHierarchyLevelRepository tablesDomainHierarchyLevelRepository = new TablesDomainHierarchyLevelRepository();
            SelectList hierarchyTypesList = new SelectList(tablesDomainHierarchyLevelRepository.GetDomainHierarchies(groupName).ToList(), "HierarchyLevelTableName", "HierarchyLevelTableName");
            queueMinderGroupVM.HierarchyTypes = hierarchyTypesList;

			List<Meeting> meetings = new List<Meeting>();
			SelectList meetingsList = new SelectList(meetings.ToList(), "MeetingID", "MeetingName");
			ViewData["Meetings"] = meetingsList;

			return View(queueMinderGroupVM);
        }

        // POST: /Create/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(QueueMinderGroupVM queueMinderGroupVM)
        {
            //Check Access Rights to Domain
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //We need to extract group from groupVM
            QueueMinderGroup queueMinderGroup = new QueueMinderGroup();
            queueMinderGroup = queueMinderGroupVM.QueueMinderGroup;
            if (queueMinderGroup == null)
            {
                ViewData["Message"] = "ValidationError : missing item"; ;
                return View("Error");
            }

            //Check Access Rights to Domain Hierarchy          
            if (!hierarchyRepository.AdminHasDomainHierarchyWriteAccess(queueMinderGroup.HierarchyType, queueMinderGroup.HierarchyCode, queueMinderGroup.SourceSystemCode, groupName))
            {
                ViewData["Message"] = "You cannot add to this hierarchy item";
                return View("Error");
            }

            //Update Model From Form + Validate against DB
            try
            {
                UpdateModel<QueueMinderGroup>(queueMinderGroup, "QueueMinderGroup");
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
            string hierarchyCode = queueMinderGroup.HierarchyCode;
            if (queueMinderGroup.HierarchyType == "ClientSubUnitTravelerType")
            {
                queueMinderGroup.ClientSubUnitGuid = hierarchyCode;  //ClientSubUnitTravelerType has 2 primarykeys
            }

            //Database Update
            try
            {
                queueMinderGroupRepository.Add(queueMinderGroup);
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

        // GET: /View
        public ActionResult Edit(int id)
        {
            QueueMinderGroup queueMinderGroup = new QueueMinderGroup();
            queueMinderGroup = queueMinderGroupRepository.GetGroup(id);

            //Check Exists
            if (queueMinderGroup == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToQueueMinderGroup(id))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            if (queueMinderGroup.TripTypeId == null)
            {
                TripType tripType = new TripType();
                queueMinderGroup.TripType = tripType;
            }
            
            QueueMinderGroupVM queueMinderGroupVM = new QueueMinderGroupVM();

            queueMinderGroupRepository.EditGroupForDisplay(queueMinderGroup);
            queueMinderGroupVM.QueueMinderGroup = queueMinderGroup;

            TripTypeRepository tripTypeRepository = new TripTypeRepository();
            SelectList tripTypesList = new SelectList(tripTypeRepository.GetAllTripTypes().ToList(), "TripTypeId", "TripTypeDescription");
            queueMinderGroupVM.TripTypes = tripTypesList;

            TablesDomainHierarchyLevelRepository tablesDomainHierarchyLevelRepository = new TablesDomainHierarchyLevelRepository();
            SelectList hierarchyTypesList = new SelectList(tablesDomainHierarchyLevelRepository.GetDomainHierarchies(groupName).ToList(), "HierarchyLevelTableName", "HierarchyLevelTableName");
            queueMinderGroupVM.HierarchyTypes = hierarchyTypesList;

			//Meetings
			MeetingRepository meetingRepository = new MeetingRepository();
			List<Meeting> meetings = meetingRepository.GetAvailableMeetings(queueMinderGroup.HierarchyType, queueMinderGroup.HierarchyCode, null, queueMinderGroup.SourceSystemCode, queueMinderGroup.TravelerTypeGuid);
			SelectList meetingsList = new SelectList(meetings.ToList(), "MeetingID", "MeetingDisplayName", queueMinderGroup.MeetingID != null ? queueMinderGroup.MeetingID.ToString() : "");
			ViewData["Meetings"] = meetingsList;
			
			return View(queueMinderGroupVM);
        }

        // POST: /Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, QueueMinderGroupVM queueMinderGroupVM)
        {
            //Get Item
            QueueMinderGroup queueMinderGroup = new QueueMinderGroup();
            queueMinderGroup = queueMinderGroupRepository.GetGroup(queueMinderGroupVM.QueueMinderGroup.QueueMinderGroupId);

            //Check Exists
            if (queueMinderGroup == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToQueueMinderGroup(queueMinderGroup.QueueMinderGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
            try
            {
                UpdateModel<QueueMinderGroup>(queueMinderGroup, "QueueMinderGroup");
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
			if (queueMinderGroup.HierarchyType != "Multiple")
			{
            //ClientSubUnitTravelerType has extra field
            string hierarchyCode = queueMinderGroup.HierarchyCode;
            if (queueMinderGroup.HierarchyType == "ClientSubUnitTravelerType")
            {
                queueMinderGroup.ClientSubUnitGuid = hierarchyCode;  //ClientSubUnitTravelerType has 2 primarykeys
            }
            //Check Access Rights to PolicyGroup
            HierarchyRepository hierarchyRepository = new HierarchyRepository();
            if (!hierarchyRepository.AdminHasDomainHierarchyWriteAccess(queueMinderGroup.HierarchyType, hierarchyCode, queueMinderGroup.SourceSystemCode, groupName))
            {
                ViewData["Message"] = "You cannot add to this hierarchy item";
                return View("Error");
            }
			}

            //Database Update
            try
            {
                queueMinderGroupRepository.Edit(queueMinderGroup);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/QueueMinderGroup.mvc/Edit/" + queueMinderGroup.QueueMinderGroupId.ToString();
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }
            return RedirectToAction("ListUnDeleted");
        }

        // GET: /View
        public ActionResult View(int id)
        {
            QueueMinderGroup group = new QueueMinderGroup();
            group = queueMinderGroupRepository.GetGroup(id);

            //Check Exists
            if (group == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }
            if (group.TripTypeId == null)
            {
                TripType tripType = new TripType();
                group.TripType = tripType;
            }

            queueMinderGroupRepository.EditGroupForDisplay(group);
            return View(group);
        }

        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id)
        {
            //Get Item From Database
            QueueMinderGroup group = new QueueMinderGroup();
            group = queueMinderGroupRepository.GetGroup(id);

            //Check Exists
            if (group == null || group.DeletedFlag == true)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            //Check Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToQueueMinderGroup(id))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
            //Add Linked Items and Display
            queueMinderGroupRepository.EditGroupForDisplay(group);
            return View(group);
        }


        // POST: /Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            //Get Item From Database
            QueueMinderGroup group = new QueueMinderGroup();
            group = queueMinderGroupRepository.GetGroup(id);

            //Check Exists
            if (group == null || group.DeletedFlag == true)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }
            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToQueueMinderGroup(id))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
            //Delete Item
            try
            {
                group.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                group.DeletedFlag = true;
                queueMinderGroupRepository.UpdateGroupDeletedStatus(group);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/QueueMinderGroup.mvc/Delete/" + id.ToString();
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
            QueueMinderGroup group = new QueueMinderGroup();
            group = queueMinderGroupRepository.GetGroup(id);

            //Check Exists
            if (group == null || group.DeletedFlag == false)
            {
                ViewData["ActionMethod"] = "UnDeleteGet";
                return View("RecordDoesNotExistError");
            }

            //Check Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToQueueMinderGroup(id))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
            //Add Linked Items and Display
            queueMinderGroupRepository.EditGroupForDisplay(group);
            return View(group);
        }

        // POST: /UnDelete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UnDelete(int id, FormCollection collection)
        {
            //Get Item From Database
            QueueMinderGroup group = new QueueMinderGroup();
            group = queueMinderGroupRepository.GetGroup(id);

            //Check Exists
            if (group == null || group.DeletedFlag == false)
            {
                ViewData["ActionMethod"] = "UnDeletePost";
                return View("RecordDoesNotExistError");
            }
            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToQueueMinderGroup(id))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
            //Delete Item
            try
            {
                group.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                group.DeletedFlag = false;
                queueMinderGroupRepository.UpdateGroupDeletedStatus(group);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/QueueMinderGroup.mvc/Delete/" + id.ToString();
                    return View("VersionError");
                }

                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }
            return RedirectToAction("ListUnDeleted");
        }

        [HttpPost]
        public JsonResult AutoCompleteClientSubUnitTravelerTypes(string searchText, string hierarchyItem, string filterText)
        {
            HierarchyRepository hierarchyRepository = new HierarchyRepository();

            int maxResults = 15;
            var result = hierarchyRepository.LookUpSystemUserClientSubUnitTravelerTypes(searchText, hierarchyItem, maxResults, groupName, filterText);
            return Json(result);
        }

		// GET: Linked ClientSubUnits
		public ActionResult LinkedClientSubUnits(int id)
		{
			//Get Group From Database
			QueueMinderGroup group = new QueueMinderGroup();
			group = queueMinderGroupRepository.GetGroup(id);

			//Check Exists
			if (group == null)
			{
				ViewData["ActionMethod"] = "ClientSubUnitGet";
				return View("RecordDoesNotExistError");
			}

			QueueMinderGroupClientSubUnitCountriesVM queueMinderGroupClientSubUnits = new QueueMinderGroupClientSubUnitCountriesVM();
			queueMinderGroupClientSubUnits.QueueMinderGroupId = id;
			queueMinderGroupClientSubUnits.QueueMinderGroupName = group.QueueMinderGroupName;

			List<ClientSubUnitCountryVM> clientSubUnitsAvailable = new List<ClientSubUnitCountryVM>();
			clientSubUnitsAvailable = queueMinderGroupRepository.GetLinkedClientSubUnits(id, false);
			queueMinderGroupClientSubUnits.ClientSubUnitsAvailable = clientSubUnitsAvailable;

			List<ClientSubUnitCountryVM> clientSubUnitsUnAvailable = new List<ClientSubUnitCountryVM>();
			clientSubUnitsUnAvailable = queueMinderGroupRepository.GetLinkedClientSubUnits(id, true);
			queueMinderGroupClientSubUnits.ClientSubUnitsUnAvailable = clientSubUnitsUnAvailable;


			return View(queueMinderGroupClientSubUnits);
		}

		// POST: Linked ClientSubUnits
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult LinkedClientSubUnits(int queueMinderGroupId, string ClientSubUnitGuid)
		{
			//Get Item From Database
			QueueMinderGroup group = new QueueMinderGroup();
			group = queueMinderGroupRepository.GetGroup(queueMinderGroupId);

			//Check Exists
			if (group == null)
			{
				ViewData["ActionMethod"] = "ClientSubUnitPost";
				return View("RecordDoesNotExistError");
			}
			//Check Access Rights
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToQueueMinderGroup(queueMinderGroupId))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Database Update
			try
			{
				queueMinderGroupRepository.UpdateLinkedClientSubUnit(queueMinderGroupId, ClientSubUnitGuid);
			}
			catch (SqlException ex)
			{
				//Versioning Error
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/QueueMinderGroup.mvc/ClientSubUnit/" + queueMinderGroupId.ToString();
					return View("VersionError");
				}
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}


			return RedirectToAction("LinkedClientSubUnits", new { id = queueMinderGroupId });
		}
    }
}
