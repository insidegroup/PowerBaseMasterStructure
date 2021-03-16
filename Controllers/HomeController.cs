using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Security.Principal;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Repository;
using CWTDesktopDatabase.Helpers;
using System.Configuration;

namespace CWTDesktopDatabase.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        HomeRepository homeRepository = new HomeRepository();

        public ActionResult Index()
        {
            string connectionStringName = Settings.getConnectionStringName();          

            List<spDesktopDataAdmin_SelectSystemUserUserProfiles_v1Result> adminUserProfiles = new List<spDesktopDataAdmin_SelectSystemUserUserProfiles_v1Result>();
            adminUserProfiles = homeRepository.GetAdminUserProfiles();

            string currentUserProfileIdentifier = "";
            if (adminUserProfiles.Count > 0){
                currentUserProfileIdentifier = adminUserProfiles[0].CurrentUserProfileIdentifier;
            }
            GenericSelectListVM connectionSelectListVM = new GenericSelectListVM();
            connectionSelectListVM.SelectList = new SelectList(homeRepository.SelectListConnectionStrings().ToList(), "Value", "Name", connectionStringName); 
            connectionSelectListVM.Message = "You are currently using the " + connectionStringName + " connection";


            HomePageVM homePageVM = new HomePageVM();
            homePageVM.ConnectionSelectList = connectionSelectListVM;
            homePageVM.UserProfileList = adminUserProfiles;
            homePageVM.SystemUserGuid = User.Identity.Name.Split(new[] { '|' })[0];
            //homePageVM.UserProfileIdentifier = User.Identity.Name;
            homePageVM.UserProfileList = adminUserProfiles;
            homePageVM.UserNewSelectList = new SelectList(homeRepository.GetAdminUserProfiles().ToList(), "SystemUserGuid", "UserProfileIdentifier", homePageVM.SystemUserGuid);
            homePageVM.MappingQualityCodes = new SelectList(homeRepository.GetAdminUserProfiles().ToList(), "SystemUserGuid", "UserProfileIdentifier", homePageVM.SystemUserGuid);

            try
            {
                string adminUserGuid = User.Identity.Name.Split(new[] { '|' })[0];
                SystemUserRepository systemUserRepository = new SystemUserRepository();
                SystemUser systemUser = systemUserRepository.GetUserBySystemUserGuid(adminUserGuid);
                if (systemUser == null)
                {
                    ViewData["Message"] = "Your ID ( " + adminUserGuid + " ) does not exist in this database, therefore you have read-only access";
                }
                else
                {
                    string username = systemUser.FirstName + " " + systemUser.LastName;
                    if (currentUserProfileIdentifier == "" || currentUserProfileIdentifier == null)
                    {
                        ViewData["Message"] = "Welcome " + username + " ( ID: " + adminUserGuid + ")";
                    }
                    else
                    {
                        ViewData["Message"] = "Welcome " + username + " ( ID: " + adminUserGuid + ", Profile: " + currentUserProfileIdentifier + ")";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewData["Message"] = ex.Message;
            }

            return View(homePageVM);
        }

        [HttpPost]
        public ActionResult Index(string databaseBtn, string userProfileBtn, FormCollection collection)
        {

            //Update database string name to cookie
            if (databaseBtn != null) {
                //SessionRepository sessionRepository = new SessionRepository();
                //sessionRepository.UpdateSessionValue("ConnectionStringName", collection["DBList"]);
                AccountRepository accountRepository = new AccountRepository();

                string adminUserGuid = User.Identity.Name.Split(new[] { '|' })[0];
                accountRepository.persistUser(adminUserGuid, collection["DBList"]);

            }

            //Update userProfileIdentifier
            if (userProfileBtn != null) {
                string userProfileIdentifier = collection["UserProfileIdentifier"];

                string adminUserGuid = User.Identity.Name.Split(new[] { '|' })[0];
                string connectionString = User.Identity.Name.Split(new[] { '|' })[1];
                if (userProfileIdentifier != adminUserGuid)
                {
                    homeRepository.UpdateAdminUserProfile(userProfileIdentifier, connectionString);
                }
            
            }


            return RedirectToAction("Index","Home");
           
        }
    }
}
