using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Repository;
using System.Data.SqlClient;
using System.Collections;

namespace CWTDesktopDatabase.Controllers
{
    public class SystemUserController : Controller
    {
        //main repositories
        SystemUserRepository systemUserRepository = new SystemUserRepository();
        TeamRepository teamRepository = new TeamRepository();
        HierarchyRepository hierarchyRepository = new HierarchyRepository();
        LogRepository logRepository = new LogRepository();

        private string groupName = "Role Based Access Team";
        private string exportGroupName = "Role";

        // GET: /List
        public ActionResult List(
			int? page, 
			string sortField, 
			int? sortOrder, 
			string filterField_1 = "", 
			string filterValue_1 = "", 
			string filterField_2 = "", 
			string filterValue_2 = "", 
			string filterField_3 = "", 
			string filterValue_3 = "")
        {

            if (string.IsNullOrEmpty(sortField))
            {
                sortField = "LastName";
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
            if (page < 1)
            {
                page = 1;
            }
            var items = systemUserRepository.GetSystemUsers(
				page ?? 1, 
				sortField, 
				sortOrder ?? 0,
				filterField_1,
				filterValue_1,
				filterField_2,
				filterValue_2,
				filterField_3,
				filterValue_3
			);

			Dictionary<string, string> filters = new Dictionary<string, string>();
			filters.Add("Login", "Login");
			filters.Add("ProfileID", "Profile ID");
			filters.Add("FirstName", "First Name");
			filters.Add("LastName", "Last Name");
			filters.Add("HomePCCOfficeID", "Home PCC/Office ID");
			filters.Add("GDSSignOnID", "GDS Sign On ID");
			filters.Add("Location", "Location");

			ViewData["Filters_1"] = new SelectList(filters, "Key", "Value", filterField_1);
			ViewData["Filters_2"] = new SelectList(filters, "Key", "Value", filterField_2);
			ViewData["Filters_3"] = new SelectList(filters, "Key", "Value", filterField_3);

            ViewData["FilterField_1"] = filterField_1;
            ViewData["FilterValue_1"] = filterValue_1;
            ViewData["FilterField_2"] = filterField_2;
            ViewData["FilterValue_2"] = filterValue_2;
            ViewData["FilterField_3"] = filterField_3;
            ViewData["FilterValue_3"] = filterValue_3;

            return View(items);
        }

        // GET: /ListRoles
        //sortable List of Users Roles
        public ActionResult ListRoles(string id, int? page, string sortField, int? sortOrder)
        {

            sortField = "AdministratorRoleHierarchyLevelTypeName";

            //Get SystemUser
            SystemUser systemUser = new SystemUser();
            systemUser = systemUserRepository.GetUserBySystemUserGuid(id);

            //Check Exists
            if (systemUser == null)
            {
                return View("Error");
            }

            //Check AccessRights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToSystemUserRoles(systemUser.SystemUserGuid))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //Set Export Access Rights
            ViewData["ExportAccess"] = "";
            if (hierarchyRepository.AdminHasDomainWriteAccess(exportGroupName))
            {
                ViewData["ExportAccess"] = "WriteAccess";
            }

            if (sortField == null || sortField == "Name")
            {
                sortField = "Name";
            }

            ViewData["DeletedFlag"] = false;


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

            ViewData["SystemUserName"] = (systemUser.LastName + ", " + systemUser.LastName + " " + systemUser.MiddleName).Replace("  ", " ");
            ViewData["SystemUserGuid"] = systemUser.SystemUserGuid;

            const int pageSize = 15;
            var items = systemUserRepository.GetSystemUserRoles(id, sortField, sortOrder ?? 0);
            var paginatedView = new PaginatedList<fnDesktopDataAdmin_SelectSystemUserRoles_v1Result>(items, page ?? 0, pageSize);
            return View(paginatedView);
        }

        // GET: /ListTeams
        public ActionResult ListTeams(string id, int? page, string sortField, int? sortOrder)
        {

            //Get SystemUser
            SystemUser systemUser = new SystemUser();
            systemUser = systemUserRepository.GetUserBySystemUserGuid(id);

            //Check Exists
            if (systemUser == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            //Set Access Rights
            ViewData["Access"] = "";
            if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Access"] = "WriteAccess";
            }   

            if (sortField != "TeamName")
            {
                sortField = "TeamName";
            }
            ViewData["DeletedFlag"] = false;


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

            //Set Access Rights
            ViewData["Access"] = "";
            if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Access"] = "WriteAccess";
            }
            
            ViewData["SystemUserName"] = (systemUser.LastName + ", " + systemUser.LastName + " " + systemUser.MiddleName).Replace("  ", " ");
            ViewData["SystemUserGuid"] = systemUser.SystemUserGuid;

			var paginatedView = systemUserRepository.GetUserTeams(id, page ?? 1, sortField, sortOrder ?? 0);

			return View(paginatedView);
        }

        // GET: /View
        public ActionResult ViewItem(string id)
        {
            //Check Exists
            SystemUser systemUser = new SystemUser();
            systemUser = systemUserRepository.GetUserBySystemUserGuid(id);
            if (systemUser == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            systemUserRepository.EditForDisplay(systemUser);
            return View(systemUser);
        }

        // GET: /CreateRole
        public ActionResult CreateRole(string id)
        {
            //Get SystemUser
            SystemUser systemUser = new SystemUser();
            systemUser = systemUserRepository.GetUserBySystemUserGuid(id);

            //Check Exists
            if (systemUser == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToSystemUserRoles(systemUser.SystemUserGuid))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            
            ViewData["SystemUserName"] = (systemUser.LastName + ", " + systemUser.FirstName + " " + systemUser.MiddleName).Replace("  ", " ");


            ViewData["Roles"] =
                    new SelectList((from s in systemUserRepository.GetUnUsedRoles(id).ToList() 
                                        select new { id = s.AdministratorRoleId + "_" +s.HierarchyLevelTypeId, 
                                                    Name = s.AdministratorRoleHierarchyLevelTypeName
                                                    }
                                    ), "id", "Name", null);

            //Show Create Form
            AdministratorRoleHierarchyLevelType administratorRoleHierarchyLevelType = new AdministratorRoleHierarchyLevelType();
            administratorRoleHierarchyLevelType.SystemUserGuid = systemUser.SystemUserGuid;
            return View(administratorRoleHierarchyLevelType);
        }

        // POST: /CreateRole
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRole(int hierarchyLevelTypeId, int administratorRoleId, string systemUserGuid, string btnSubmit)
        {
            AdministratorRoleHierarchyLevelType administratorRoleHierarchyLevelType = new AdministratorRoleHierarchyLevelType();
            administratorRoleHierarchyLevelType.AdministratorRoleId = administratorRoleId;
            administratorRoleHierarchyLevelType.HierarchyLevelTypeId  = hierarchyLevelTypeId;
            administratorRoleHierarchyLevelType.SystemUserGuid = systemUserGuid;

            //Get SystemUser
            SystemUser systemUser = new SystemUser();
            systemUser = systemUserRepository.GetUserBySystemUserGuid(systemUserGuid);

            //Check Exists
            if (systemUser == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }
            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToSystemUserRoles(administratorRoleHierarchyLevelType.SystemUserGuid))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update  Model from Form
            try
            {
                UpdateModel(administratorRoleHierarchyLevelType);
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
                systemUserRepository.AddRole(administratorRoleHierarchyLevelType);
            }
            catch (SqlException ex)
            {
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
        
            }


            ViewData["NewSortOrder"] = 0;
            return RedirectToAction("ListRoles", new { id = administratorRoleHierarchyLevelType.SystemUserGuid });

        }

        // GET: /DeleteRole
		[HttpGet]
		public ActionResult DeleteRole(int administratorRoleId, int hierarchyLevelTypeId, string id, string languageCode)
        {
            //Check Exists
            AdministratorRoleHierarchyLevelTypeSystemUser administratorRoleHierarchyLevelTypeSystemUser = new AdministratorRoleHierarchyLevelTypeSystemUser();
            administratorRoleHierarchyLevelTypeSystemUser = systemUserRepository.GetUserRole(administratorRoleId, hierarchyLevelTypeId, id);
            if (administratorRoleHierarchyLevelTypeSystemUser == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToSystemUserRoles(administratorRoleHierarchyLevelTypeSystemUser.SystemUserGuid))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Add Linked Tables
            systemUserRepository.EditUserRoleForDisplay(administratorRoleHierarchyLevelTypeSystemUser);

            //Get SystemUser
            SystemUser systemUser = new SystemUser();
            systemUser = systemUserRepository.GetUserBySystemUserGuid(id);
           
            ViewData["Name"] = (systemUser.LastName + ", " + systemUser.FirstName + " " + systemUser.MiddleName).Replace("  ", " ");
            return View(administratorRoleHierarchyLevelTypeSystemUser);
        }

        // POST: /DeleteRole
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRole(int hierarchyLevelTypeId, int administratorRoleId, string id, FormCollection collection)
        {
            AdministratorRoleHierarchyLevelTypeSystemUser administratorRoleHierarchyLevelTypeSystemUser = new AdministratorRoleHierarchyLevelTypeSystemUser();
            administratorRoleHierarchyLevelTypeSystemUser = systemUserRepository.GetUserRole(administratorRoleId, hierarchyLevelTypeId, id);

            //Get SystemUser
            SystemUser systemUser = new SystemUser();
            systemUser = systemUserRepository.GetUserBySystemUserGuid(administratorRoleHierarchyLevelTypeSystemUser.SystemUserGuid);

            //Check Exists
            if (systemUser == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToSystemUserRoles(administratorRoleHierarchyLevelTypeSystemUser.SystemUserGuid))
            {
              ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
            try{
                administratorRoleHierarchyLevelTypeSystemUser.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                systemUserRepository.DeleteRole(administratorRoleHierarchyLevelTypeSystemUser);
             }
            catch (SqlException ex)
            {
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
        
            }



            //Return
            ViewData["NewSortOrder"] = 0;
            return RedirectToAction("ListRoles", new { id = administratorRoleHierarchyLevelTypeSystemUser.SystemUserGuid });
        }

     

        // GET: /DeleteTeam
		[HttpGet]
		public ActionResult DeleteTeam(int teamId, string id)
        {
            //Get SystemUser
            SystemUser systemUser = new SystemUser();
            systemUser = systemUserRepository.GetUserBySystemUserGuid(id);

            //Check Exists
            if (systemUser == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToTeam(teamId))
            {
                ViewData["Message"] = "You do not have access to this item";
               return View("Error");
            }

            //Show Create Form
            SystemUserTeam systemUserTeam = new SystemUserTeam();
            systemUserTeam.SystemUserGuid = id;
            systemUserTeam.TeamId = teamId;

            SystemUserTeamRepository systemUserTeamRepository = new SystemUserTeamRepository();
            systemUserTeamRepository.EditForDisplay(systemUserTeam);
            return View(systemUserTeam);
        }

        // POST: /DeleteTeam
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteTeam(int teamId, string id, FormCollection collection)
        {
            SystemUserTeamRepository systemUserTeamRepository = new SystemUserTeamRepository();
            SystemUserTeam systemUserTeam = new SystemUserTeam();
            systemUserTeam.TeamId = teamId;
            systemUserTeam.SystemUserGuid = id;

            //Get SystemUser
            SystemUser systemUser = new SystemUser();
            systemUser = systemUserRepository.GetUserBySystemUserGuid(id);

            //Check Exists
            if (systemUser == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }
            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToTeam(teamId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            systemUserRepository.DeleteTeam(systemUserTeam);

            //Return
            ViewData["NewSortOrder"] = 0;
            return RedirectToAction("ListTeams", new { id = systemUserTeam.SystemUserGuid });
        }

        // POST: /Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
		[HandleError(View = "Error", ExceptionType = typeof(HttpAntiForgeryException))]
        public ActionResult Edit(string id, int locationId, FormCollection collection)
        {

            SystemUser systemUser = new SystemUser();
            systemUser = systemUserRepository.GetUserBySystemUserGuid(id);

            //Check Exists
            if (systemUser == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToLocation(locationId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

			
			SystemUser originalSystemUser = new SystemUser();
			originalSystemUser = systemUserRepository.GetUserBySystemUserGuid(id);

			//CubaBookingAllowanceIndicator with Logging
			bool cubaBookingAllowanceIndicator = false;
			if (originalSystemUser.CubaBookingAllowanceIndicator == true)
			{
				cubaBookingAllowanceIndicator = true;
			}

			//MilitaryAndGovernmentUserFlag with Logging
			bool militaryAndGovernmentUserFlag = false;
			if (originalSystemUser.MilitaryAndGovernmentUserFlag == true)
			{
				militaryAndGovernmentUserFlag = true;
			}

            //Update Item from Form
            try
            {
                UpdateModel(systemUser);
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
                systemUserRepository.Edit(systemUser);

				//Edit SystemUser CubaBookingAllowanceIndicator
				if (cubaBookingAllowanceIndicator != systemUser.CubaBookingAllowanceIndicator)
				{
					systemUser.VersionNumber = systemUser.VersionNumber + 1;
					systemUserRepository.EditCubaBookingAllowanceIndicator(systemUser);
				}

				//Edit SystemUser MilitaryAndGovernmentUserFlag
				if (militaryAndGovernmentUserFlag != systemUser.MilitaryAndGovernmentUserFlag)
				{
					systemUser.VersionNumber = systemUser.VersionNumber + 1;
					systemUserRepository.EditMilitaryAndGovernmentUserFlag(systemUser);
				}
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/SystemUser.mvc/Edit?id=" + systemUser.SystemUserGuid;
                    return View("VersionError");
                }

                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
         
            }

            //Success
            return RedirectToAction("List");

        }

        // GET: /Edit
        public ActionResult Edit(string id)
        {
            SystemUser systemUser = new SystemUser();
            systemUser = systemUserRepository.GetUserBySystemUserGuid(id);
            
            //Check Exists
            if (systemUser == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }
            //SystemUserLocation
            SystemUserLocationRepository systemUserLocationRepository = new SystemUserLocationRepository();
            SystemUserLocation systemUserLocation = new SystemUserLocation();
            systemUserLocation = systemUserLocationRepository.GetSystemUserLocation(id);

			RolesRepository rolesRepository = new RolesRepository();

            //AccessRights           
            if (systemUserLocation != null)
            {
                if (!rolesRepository.HasWriteAccessToLocation(systemUserLocation.LocationId))
                {
                    ViewData["Message"] = "You do not have access to this item";
                    return View("Error");
                }
            }
            else
            {

                if (!hierarchyRepository.AdminHasDomainWriteAccess("Location"))
                {
                    ViewData["Message"] = "You do not have access to this item";
                    return View("Error");
                }
            }

			//Check Compliance Administrator Access
			ViewData["ComplianceAdministratorAccess"] = "";
			if (hierarchyRepository.AdminHasDomainWriteAccess("Compliance Administrator"))
			{
				ViewData["ComplianceAdministratorAccess"] = "WriteAccess";
			}

			//Check GDS Government Administrator Access
			ViewData["GDSGovernmentAdministratorAccess"] = "";
			if (rolesRepository.HasWriteAccessToSystemUserMilitaryAndGovernmentUserFlag(systemUser.SystemUserGuid))
			{
				ViewData["GDSGovernmentAdministratorAccess"] = "WriteAccess";
			}

			//Check Restricted System User Access
			ViewData["RestrictedSystemUserAdministratorAccess"] = "";
			if (rolesRepository.HasWriteAccessToSystemUserRestrictedFlag(systemUser.SystemUserGuid))
			{
				ViewData["RestrictedSystemUserAdministratorAccess"] = "WriteAccess";
			}

            //Check Online System User Access
            ViewData["OnlineUserAccess"] = "";
			if (rolesRepository.HasWriteAccessToSystemUserOnlineUserFlag())
			{
				ViewData["OnlineUserAccess"] = "WriteAccess";
			}

            systemUserRepository.EditForDisplay(systemUser);
            return View(systemUser);
        }

		// POST: AutoComplete Hierarchies
		[HttpPost]
		public JsonResult AutoCompleteHierarchies(string searchText)
		{
			HierarchyRepository hierarchyRepository = new HierarchyRepository();

			int maxResults = 15;
			var result = hierarchyRepository.LookUpSystemUserHierarchies(searchText, "Location", maxResults, groupName, true);
			return Json(result);
		}

		#region Define Roles

		// GET: /DefineRolesStep1
		public ActionResult DefineRolesStep1(string id)
		{
			//Get User
			SystemUser systemUser = new SystemUser();
			systemUser = systemUserRepository.GetUserBySystemUserGuid(id);

			//Check Exists
			if (systemUser == null)
			{
				ViewData["ActionMethod"] = "ViewGet";
				return View("RecordDoesNotExistError");
			}
			
			//Check AccessRights
			ViewData["Access"] = "";
			RolesRepository rolesRepository = new RolesRepository();
			if (rolesRepository.HasWriteAccessToSystemUserRoles(systemUser.SystemUserGuid))
			{
				ViewData["Access"] = "WriteAccess";
			}
			
			return View(systemUser);
		}

		// POST: /DefineRolesStep1
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult DefineRolesStep1(string id, string newUserProfileIdentifier)
		{
			//Get SystemUsers
			SystemUser systemUser = new SystemUser();
			systemUser = systemUserRepository.GetUserBySystemUserGuid(id);

			SystemUser newSystemUser = new SystemUser();
			newSystemUser = systemUserRepository.GetUserByUserProfileIdentifier(newUserProfileIdentifier);

			//Check Exists
			if (systemUser == null || newSystemUser == null)
			{
				ViewData["ActionMethod"] = "DefineRolesStep1";
				return View("RecordDoesNotExistError");
			}

			//Check AccessRights
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToSystemUserRoles(systemUser.SystemUserGuid) || 
				!rolesRepository.HasWriteAccessToSystemUserRoles(newSystemUser.SystemUserGuid))
			{
				return View("Error");
			}

			return RedirectToAction("DefineRolesStep2", new { id = id, newUserProfileIdentifier = newUserProfileIdentifier });

		}

		//GET - DefineRolesStep2
		public ActionResult DefineRolesStep2(string id, string newUserProfileIdentifier)
		{
			//Get SystemUsers
			SystemUser systemUser = new SystemUser();
			systemUser = systemUserRepository.GetUserBySystemUserGuid(id);

			SystemUser newSystemUser = new SystemUser();
			newSystemUser = systemUserRepository.GetUserByUserProfileIdentifier(newUserProfileIdentifier);

			//Check Exists
			if (systemUser == null || newSystemUser == null)
			{
				ViewData["ActionMethod"] = "DefineRolesStep2";
				return View("RecordDoesNotExistError");
			}

			//Check AccessRights
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToSystemUserRoles(systemUser.SystemUserGuid) || 
				!rolesRepository.HasWriteAccessToSystemUserRoles(newSystemUser.SystemUserGuid))
			{
				return View("Error");
			}

			ViewData["UserProfileIdentifier"] = systemUser.UserProfileIdentifier;
			ViewData["NewUserProfileIdentifier"] = newSystemUser.UserProfileIdentifier;
			ViewData["Access"] = "WriteAccess";

			return View();
		}

		// POST: /DefineRolesStep2
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult DefineRolesStep2(string id, string newUserProfileIdentifier, FormCollection collection)
		{
			//Get SystemUsers
			SystemUser systemUser = new SystemUser();
			systemUser = systemUserRepository.GetUserBySystemUserGuid(id);

			SystemUser newSystemUser = new SystemUser();
			newSystemUser = systemUserRepository.GetUserByUserProfileIdentifier(newUserProfileIdentifier);

			//Check Exists
			if (systemUser == null || newSystemUser == null)
			{
				ViewData["ActionMethod"] = "DefineRolesStep1";
				return View("RecordDoesNotExistError");
			}

			//Check AccessRights
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToSystemUserRoles(systemUser.SystemUserGuid) ||
				!rolesRepository.HasWriteAccessToSystemUserRoles(newSystemUser.SystemUserGuid))
			{
				return View("Error");
			}

			try
			{
				systemUserRepository.CopyUserRoles(id, newSystemUser.SystemUserGuid);
			}
			catch
			{
				return View("Error");
			}

			return RedirectToAction("DefineRolesCompleted", new { id = id, newUserProfileIdentifier = newUserProfileIdentifier });
		}

		//GET - DefineRolesCompleted
		public ActionResult DefineRolesCompleted(string id, string newUserProfileIdentifier)
		{
			//Get SystemUsers
			SystemUser systemUser = new SystemUser();
			systemUser = systemUserRepository.GetUserBySystemUserGuid(id);

			SystemUser newSystemUser = new SystemUser();
			newSystemUser = systemUserRepository.GetUserByUserProfileIdentifier(newUserProfileIdentifier);

			//Check Exists
			if (systemUser == null || newSystemUser == null)
			{
				ViewData["ActionMethod"] = "DefineRolesCompleted";
				return View("RecordDoesNotExistError");
			}

			//Check AccessRights
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToSystemUserRoles(systemUser.SystemUserGuid) ||
				!rolesRepository.HasWriteAccessToSystemUserRoles(newSystemUser.SystemUserGuid))
			{
				return View("Error");
			}

			ViewData["ConfirmationMessage"] = "You have successfully copied all roles to " + systemUser.UserProfileIdentifier + " from " + newSystemUser.UserProfileIdentifier;
			ViewData["Access"] = "WriteAccess";

			return View();
		}

        #endregion

        // GET: /Export
        public ActionResult ExportSystemUserRoles(string id)
        {
            //Check Export Access Rights
            if (!hierarchyRepository.AdminHasDomainWriteAccess(exportGroupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Get CSV Data
            byte[] csvData = systemUserRepository.ExportSystemUserRoles(id);
            return File(csvData, "text/csv", "System User Export.csv");
        }
    }
}
