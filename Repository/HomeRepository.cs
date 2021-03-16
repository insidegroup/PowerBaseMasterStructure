using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using System.Configuration;
using System.Web.Security;

namespace CWTDesktopDatabase.Repository
{
    public class HomeRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());
		private DatabaseDC database_db = new DatabaseDC(Settings.getConnectionString());

		private SystemUserRepository systemUserRepository = new SystemUserRepository();

        /// <summary>
        /// Select a list of connection strings from the web.config
		/// Only users with the Military and Government user flag checked should be able to access the M&G flagged databases
		/// If a user does not have the Military and Government user flag checked, the database droplist should not display the M&G instances
        /// </summary>
        /// <returns></returns>
        public List<classConnectionStrings> SelectListConnectionStrings()
        {
            List<classConnectionStrings> items = new List<classConnectionStrings>();

			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			SystemUser systemUser = systemUserRepository.GetUserBySystemUserGuid(adminUserGuid);

			bool userHasMilitaryAndGovernmentAccess = (systemUser.MilitaryAndGovernmentUserFlag == true);

            if (System.Configuration.ConfigurationManager.ConnectionStrings.Count > 1)
            {
                foreach (ConnectionStringSettings cs in System.Configuration.ConfigurationManager.ConnectionStrings)
                {
                    if (cs.Name != "LocalSqlServer" && cs.Name != "CreditCardDatabase")
                    {
						if (!isMilitaryAndGovernmentDatabase(cs.Name))
						{
							items.Add(
								new classConnectionStrings
								{
									Name = cs.Name,
									Value = cs.Name,
								}
							 );
						}
						else if (userHasMilitaryAndGovernmentAccess && isMilitaryAndGovernmentDatabase(cs.Name))
						{
							items.Add(
								new classConnectionStrings
								{
									Name = cs.Name,
									Value = cs.Name,
								}
							 );
						}
                    }
                }
            }

            return items.ToList();
        }

		public bool isMilitaryAndGovernmentDatabase(string databaseName)
		{
			bool isMilitaryAndGovernmentDatabase = false;
			
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			try
			{
				DatabaseConnection databaseConnection = database_db.DatabaseConnections.Where(x => x.DatabaseName == databaseName).SingleOrDefault();

				if (databaseConnection != null)
				{
					if (databaseConnection.IsMilitaryGovernmentFlag == true)
					{
						isMilitaryAndGovernmentDatabase = true;
					}

				}
			}
			catch (Exception ex)
			{

			}

			return isMilitaryAndGovernmentDatabase;
		}

        public List<spDesktopDataAdmin_SelectSystemUserUserProfiles_v1Result> GetAdminUserProfiles()
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            return db.spDesktopDataAdmin_SelectSystemUserUserProfiles_v1(adminUserGuid).ToList();

        }


        public void UpdateAdminUserProfile(string userProfileIdentifier, string connectionString)
        {
            //try login user
            SystemUserRepository systemUserRepository = new SystemUserRepository();
            SystemUser systemUser = systemUserRepository.GetUserBySystemUserGuid(userProfileIdentifier);

            //store userdata in cookie for Forms Authentication
            AccountRepository accountRepository = new AccountRepository();
            accountRepository.persistUser(systemUser.SystemUserGuid, connectionString);

            //update login TimeStamp
            systemUserRepository.UpdateSystemUserLastLoginTimestamp(userProfileIdentifier);

        }
    }
}