using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using System.Linq.Dynamic;
using System.Text;

namespace CWTDesktopDatabase.Repository
{
    public class SystemUserRepository
    {
        //data
        private SystemUserDC db = new SystemUserDC(Settings.getConnectionString());
        private HierarchyDC hierarchyDC = new HierarchyDC(Settings.getConnectionString());

        //List of All System User Teams - Sortable
        public CWTPaginatedList<spDesktopDataAdmin_SelectTeamsBySystemUser_v1Result> GetUserTeams(string id, int page, string sortField, int sortOrder)
		{
			//get a page of records
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectTeamsBySystemUser_v1(id, sortField, sortOrder, page, adminUserGuid).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}

			//put into page object
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectTeamsBySystemUser_v1Result>(result, page, totalRecords);

			//return to user
			return paginatedView;
		}


        //SubUnits+associated data for a TopUnit
        public CWTPaginatedList<spDesktopDataAdmin_SelectSystemUsers_v1Result> GetSystemUsers(
			int page, 
			string sortField, 
			int sortOrder,
			string filterField_1 = "", 
			string filterValue_1 = "", 
			string filterField_2 = "", 
			string filterValue_2 = "", 
			string filterField_3 = "", 
			string filterValue_3 = ""
		)
        {

            //query db
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectSystemUsers_v1(
				adminUserGuid, 
				page, 
				sortField, 
				sortOrder,
				filterField_1,
				filterValue_1,
				filterField_2,
				filterValue_2,
				filterField_3,
				filterValue_3
			).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //add paging information and return1
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectSystemUsers_v1Result>(result, page, totalRecords);
            return paginatedView;


        }


        //List of All Systemuser ROles - Sortable
        public IQueryable<fnDesktopDataAdmin_SelectSystemUserRoles_v1Result> GetSystemUserRoles(string id, string sortField, int sortOrder)
        {
            if (sortOrder == 0)
            {
                sortField = sortField + " ascending";
            }
            else
            {
                sortField = sortField + " descending";
            }
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            return db.fnDesktopDataAdmin_SelectSystemUserRoles_v1(adminUserGuid, id).OrderBy("AdministratorRoleHierarchyLevelTypeName");
        }

        //Used for Authentication (/Account/LogOn) 
        public SystemUser GetUserByUsername(string username)
        {
            return (from u in db.SystemUsers where u.SystemUserLoginIdentifier == username select u).SingleOrDefault();
        }

		//Used for Authentication (/Account/LogOn)
		public SystemUser GetUserBySystemUserGuid(string guid)
		{
			return (from u in db.SystemUsers where u.SystemUserGuid == guid select u).SingleOrDefault();
		}

		//Used for DefineRoles
		public SystemUser GetUserByUserProfileIdentifier(string userProfileIdentifier)
		{
			return (from u in db.SystemUsers where u.UserProfileIdentifier == userProfileIdentifier select u).SingleOrDefault();
		}

        //Update user timestamp when logged in
        public void UpdateSystemUserLastLoginTimestamp(string guid)
        {
            db.spDesktopDataAdmin_UpdateSystemUserLastLoginTimestamp_v1(guid);
        }


        //USed for AUthentication??
        /*public List<SystemUserRole> GetSystemUserRoles(string username)
        {
            return (from u in db.spAdmin_SelectSystemUserRoles(username)orderby u.RoleName
                         select
                             new SystemUserRole
                             {
                                 RoleName = u.RoleName.Trim(),
                                 HierarchyLevelTypeDescription = u.HierarchyLevelTypeDescription.ToString()
                             }).ToList();
        }
        */

        //Used for Authentication??
		//Commented out v3.2.3
		//public static bool ValidateUser(string username)
		//{
		//	var systemUserRepository = new SystemUserRepository();

		//	SystemUser systemUser = systemUserRepository.GetUserByUsername(username);
		//	if (systemUser == null)
		//	{
		//		return false;
		//	}

		//	var authTicket = new FormsAuthenticationTicket(1, username, DateTime.Now,
		//												   DateTime.Now.AddMinutes(30), true, "");

		//	string cookieContents = FormsAuthentication.Encrypt(authTicket);
		//	var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, cookieContents)
		//	{
		//		Expires = authTicket.Expiration,
		//		Path = FormsAuthentication.FormsCookiePath,
		//		Secure = Helpers.Security.IsHttps()
		//	};

		//	if (HttpContext.Current != null)
		//	{
		//			HttpContext.Current.Response.Cookies.Add(cookie);
		//	}
		//	return true;
		//}

        //Add Data From Linked Tables for Display
        public void EditForDisplay(SystemUser systemUser)
        {
            //Add LanguageName
            if (systemUser.LanguageCode != null)
            {
                LanguageRepository languageRepository = new LanguageRepository();
                Language language = new Language();
                language = languageRepository.GetLanguage(systemUser.LanguageCode);
                if (language != null)
                {
                    systemUser.LanguageName = language.LanguageName;
                }
            }
            SystemUserLocationRepository systemUserLocationRepository = new SystemUserLocationRepository();
            SystemUserLocation systemUserLocation = new SystemUserLocation();
            systemUserLocation = systemUserLocationRepository.GetSystemUserLocation(systemUser.SystemUserGuid);
            if (systemUserLocation != null)
            {
                systemUser.LocationId = systemUserLocation.LocationId;

                HierarchyRepository hierarchyRepository = new HierarchyRepository();
                Location location = new Location();
                location = hierarchyRepository.GetLocation(systemUser.LocationId);
                if (location != null)
                {
                    systemUser.LocationName = location.LocationName;
                }
            }
            
        }


        //Add Data From Linked Tables for Display
        public void EditUserRoleForDisplay(AdministratorRoleHierarchyLevelTypeSystemUser administratorRoleHierarchyLevelTypeSystemUser)
        {
            SystemUserRepository systemUserRepository = new SystemUserRepository();
            SystemUser systemUser = new SystemUser();
            systemUser = systemUserRepository.GetUserBySystemUserGuid(administratorRoleHierarchyLevelTypeSystemUser.SystemUserGuid);
            if (systemUser != null)
            {
                administratorRoleHierarchyLevelTypeSystemUser.SystemUserName = (systemUser.LastName + ", " + systemUser.FirstName + " " + systemUser.MiddleName).Replace("  ", " ");
            }
            AdministratorRoleHierarchyLevelTypeRepository administratorRoleHierarchyLevelTypeRepository = new AdministratorRoleHierarchyLevelTypeRepository();
            AdministratorRoleHierarchyLevelType administratorRoleHierarchyLevelType = new AdministratorRoleHierarchyLevelType();
            administratorRoleHierarchyLevelType = administratorRoleHierarchyLevelTypeRepository.GetAdministratorRoleHierarchyLevelType(
                        administratorRoleHierarchyLevelTypeSystemUser.AdministratorRoleId,
                        administratorRoleHierarchyLevelTypeSystemUser.HierarchyLevelTypeId
                        );
            if (administratorRoleHierarchyLevelType != null)
            {
                administratorRoleHierarchyLevelTypeSystemUser.AdministratorRoleHierarchyLevelTypeName = administratorRoleHierarchyLevelType.AdministratorRoleHierarchyLevelTypeName;
            }
        }

        //Check if current user has Write Access to a SystemUser 
        //Updated to allow an LoggedIn user to alter other SystemUsers
        //the only 2 fields that can be edited are Location+Language
       // public bool HasWriteAccessToSystemUser(string id)
       // {
            //string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            //var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToSystemUser_v1(id, adminUserGuid);
            //return result;
            //return true;
       // }


        //Check if current user has Write Access to SystemUsers Roles
        //Based on MaintenanceAdminRole
        

        public AdministratorRoleHierarchyLevelTypeSystemUser GetUserRole(int aId, int hId, string userGuid)
        {
            HierarchyDC db1 = new HierarchyDC(Settings.getConnectionString());
            return db1.AdministratorRoleHierarchyLevelTypeSystemUsers.SingleOrDefault(
                        c => (c.SystemUserGuid == userGuid
                                && c.AdministratorRoleId== aId
                                && c.HierarchyLevelTypeId == hId
                                ));
        }

        //Roles that user is not a member of
        public List<AdministratorRoleHierarchyLevelType> GetUnUsedRoles(string id)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = from n in db.spDesktopDataAdmin_SelectSystemUserAvailableRoles_v1(id)
                         select new AdministratorRoleHierarchyLevelType
                         {
                             AdministratorRoleHierarchyLevelTypeName = n.AdministratorRoleHierarchyLevelTypeName,
                             AdministratorRoleId = n.AdministratorRoleId,
                             HierarchyLevelTypeId = n.HierarchyLevelTypeId
                         };
            return result.ToList();
        }


       

        //Add Role
        public void AddRole(AdministratorRoleHierarchyLevelType administratorRoleHierarchyLevelType)
        {
            LogRepository logRepository = new LogRepository();
            string computerName = logRepository.GetComputerName(); 
            
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertAdministratorRoleHierarchyLevelTypeSystemUser_v1(
                administratorRoleHierarchyLevelType.HierarchyLevelTypeId,
                administratorRoleHierarchyLevelType.AdministratorRoleId,
                administratorRoleHierarchyLevelType.SystemUserGuid,
                adminUserGuid,
                Settings.ApplicationName(),
                Settings.ApplicationVersion(),
                null,
                computerName,
                null,
                null,
                14,
                null,
                null,
                null
            );

        }

        //Delete Role
        public void DeleteRole(AdministratorRoleHierarchyLevelTypeSystemUser administratorRoleHierarchyLevelTypeSystemUser)
        {
            LogRepository logRepository = new LogRepository();
            string computerName = logRepository.GetComputerName();

            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeleteAdministratorRoleHierarchyLevelTypeSystemUser_v1(
                administratorRoleHierarchyLevelTypeSystemUser.HierarchyLevelTypeId,
                administratorRoleHierarchyLevelTypeSystemUser.AdministratorRoleId,
                administratorRoleHierarchyLevelTypeSystemUser.SystemUserGuid,
                adminUserGuid,
                administratorRoleHierarchyLevelTypeSystemUser.VersionNumber,
                Settings.ApplicationName(),
                Settings.ApplicationVersion(),
                null,
                computerName,
                null,
                null,
                15,
	            null,
	            null,
	            null
            );

        }

        //Add Team
        public void AddTeam(SystemUserTeam systemUserTeam)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertSystemUserTeam_v1(
                systemUserTeam.SystemUserGuid,
                systemUserTeam.TeamId,
                adminUserGuid
            );

        }

        //Delete Team from DB
        public void DeleteTeam(SystemUserTeam systemUserTeam)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeleteSystemUserTeam_v1(
                systemUserTeam.SystemUserGuid,
                systemUserTeam.TeamId
            );
        }

        //Edit SystemUser
        public void Edit(SystemUser systemUser)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdateSystemUser_v1(
                systemUser.SystemUserGuid,
                systemUser.LanguageCode ,
                systemUser.LocationId,
				systemUser.DefaultProfileIdentifier,
				systemUser.RestrictedFlag,
				systemUser.OnlineUserFlag,
                adminUserGuid,
                systemUser.VersionNumber
            );
        }

		//Copy UsersRoles from one user to another
		public void CopyUserRoles(string userId, string newUserId)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			db.spDesktopDataAdmin_CopyUserRoles_v1(userId, newUserId, adminUserGuid);
		}

		//Edit SystemUser DefaultProfileIdentifier
		public void EditDefaultProfileIdentifier(SystemUser systemUser)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdateSystemUserDefaultProfileIdentifier_v1(
				systemUser.SystemUserGuid,
				systemUser.DefaultProfileIdentifier,
				adminUserGuid,
				systemUser.VersionNumber
			);
		}

		//Edit SystemUser RestrictedFlag
		public void EditRestrictedFlag(SystemUser systemUser)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdateSystemUserRestrictedFlag_v1(
				systemUser.SystemUserGuid,
				systemUser.RestrictedFlag,
				adminUserGuid,
				systemUser.VersionNumber
			);
		}

		//Edit SystemUser CubaBookingAllowanceIndicator
		public void EditCubaBookingAllowanceIndicator(SystemUser systemUser)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			LogRepository logRepository = new LogRepository();
			string computerName = logRepository.GetComputerName();

			db.spDesktopDataAdmin_UpdateSystemUserCubaBookingAllowanceIndicator_v1(
				systemUser.SystemUserGuid,
				systemUser.CubaBookingAllowanceIndicator,
				adminUserGuid,
				systemUser.VersionNumber,
				Settings.ApplicationName(),
				Settings.ApplicationVersion(),
				computerName
			);
		}

		//Edit SystemUser MilitaryAndGovernmentUserFlag
		public void EditMilitaryAndGovernmentUserFlag(SystemUser systemUser)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			LogRepository logRepository = new LogRepository();
			string computerName = logRepository.GetComputerName();

			db.spDesktopDataAdmin_UpdateSystemUserMilitaryAndGovernmentUserFlag_v1(
				systemUser.SystemUserGuid,
				systemUser.MilitaryAndGovernmentUserFlag,
				adminUserGuid,
				systemUser.VersionNumber,
				Settings.ApplicationName(),
				Settings.ApplicationVersion(),
				computerName
			);
		}

		//Get PseudoCityOrOfficeIds by GDSCode based on SystemUser and logged in user
		public List<ValidPseudoCityOrOfficeIdJSON> GetSystemUserPseudoCityOrOfficeIdsByGDSCode(string systemUserGuid, string gdsCode)
		{
			HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			return (from n in db.fnDesktopDataAdmin_SelectSystemUserValidPseudoCityOrOfficeIds_v1(systemUserGuid, gdsCode, adminUserGuid).OrderBy(x => x.PseudoCityOrOfficeId)
					select
						new ValidPseudoCityOrOfficeIdJSON
						{
							PseudoCityOrOfficeId = n.PseudoCityOrOfficeId.ToString().Trim(),
							GDSCode = n.GDSCode
						}
			).ToList();
		}

        //Export Items to CSV
        public byte[] ExportSystemUserRoles(string id)
        {
            StringBuilder sb = new StringBuilder();

            //Add Headers
            List<string> headers = new List<string>
            {
                "Name", //SystemUser table - Name concatenated First Name Last Name
                "First Name", //SystemUser table
                "Last Name", //SystemUser table
                "SystemUserLoginIdentifier", //SystemUser table
                "UserProfileIdentifier", //SystemUser table
                "SystemUserGuid", //AdministratorRoleHierarchyLevelTypeSystemUser table
                "AdministratorRoleHierarchyLevelTypeName", //AdministratorRoleHierarchyLevelType table
                "AdministratorRoleId", //AdministratorRoleHierarchyLevelTypeSystemUser table
                "HierarchyLevelTypeId", //AdministratorRoleHierarchyLevelTypeSystemUser table
                "MilitaryAndGovernmentUserFlag", //SystemUser table", //should show Yes or No
                "RestrictedFlag", //SystemUser table", //should show Yes or No
                "CountryName", //Country table
                "CreationTimestamp", //AdministratorRoleHierarchyLevelTypeSystemUser table (MM/DD/YY HH:MM military time)
                "CreationUserIdentifier", //AdministratorRoleHierarchyLevelTypeSystemUser table
                "LastUpdateTimestamp", //AdministratorRoleHierarchyLevelTypeSystemUser table (MM/DD/YY HH:MM military time)
            };

            sb.AppendLine(String.Join(",", headers.Select(x => x.ToString()).ToArray()));

            SystemUser systemUser = new SystemUser();
            systemUser = GetUserBySystemUserGuid(id);

            string countryName = string.Empty;

            SystemUserLocationRepository systemUserLocationRepository = new SystemUserLocationRepository();
            SystemUserLocation systemUserLocation = systemUserLocationRepository.GetSystemUserLocation(id);
            if (systemUserLocation.LocationId > 0)
            {
                LocationRepository locationRepository = new LocationRepository();
                Location location = locationRepository.GetLocation(systemUserLocation.LocationId);
                if (location != null)
                {
                    locationRepository.EditForDisplay(location);
                    countryName = location.CountryName;
                }
            }

            //Add Items
            List<AdministratorRoleHierarchyLevelTypeSystemUser> systemUserRoles = hierarchyDC.AdministratorRoleHierarchyLevelTypeSystemUsers
                .Where(x => x.SystemUserGuid == id)
                .OrderBy(x => x.AdministratorRoleId)
                .ThenBy(x => x.HierarchyLevelTypeId)
                .ToList();

            foreach (AdministratorRoleHierarchyLevelTypeSystemUser item in systemUserRoles)
            {

                string date_format = "MM/dd/yy HH:mm";

                EditUserRoleForDisplay(item);

                sb.AppendFormat(
                    "{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14}",

                    string.Format("{0} {1}", systemUser.FirstName, systemUser.LastName),
                    !string.IsNullOrEmpty(systemUser.FirstName) ? systemUser.FirstName : " ",
                    !string.IsNullOrEmpty(systemUser.LastName) ? systemUser.LastName : " ",
                    !string.IsNullOrEmpty(systemUser.SystemUserLoginIdentifier) ? systemUser.SystemUserLoginIdentifier : " ",
                    !string.IsNullOrEmpty(systemUser.UserProfileIdentifier) ? systemUser.UserProfileIdentifier : " ",
                    !string.IsNullOrEmpty(item.SystemUserGuid) ? item.SystemUserGuid : " ",
                    !string.IsNullOrEmpty(item.AdministratorRoleHierarchyLevelTypeName) ? item.AdministratorRoleHierarchyLevelTypeName : " ",
                    item.AdministratorRoleId > 0 ? item.AdministratorRoleId.ToString() : " ",
                    item.HierarchyLevelTypeId > 0 ? item.HierarchyLevelTypeId.ToString() : " ",
                    systemUser.MilitaryAndGovernmentUserFlag != null && systemUser.MilitaryAndGovernmentUserFlag == true ? "True" : "False",
                    systemUser.RestrictedFlag != null  && systemUser.RestrictedFlag == true ? "True" : "False",
                    !string.IsNullOrEmpty(countryName) ? countryName: " ",
                    item.CreationTimestamp.HasValue ? item.CreationTimestamp.Value.ToString(date_format) : " ",
                    !string.IsNullOrEmpty(item.CreationUserIdentifier) ? item.CreationUserIdentifier : " ",
                    item.LastUpdateTimestamp.HasValue ? item.LastUpdateTimestamp.Value.ToString(date_format) : " "
                );

                sb.Append(Environment.NewLine);
            }

            return Encoding.ASCII.GetBytes(sb.ToString());
        }
    }
}
