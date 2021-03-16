    using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Repository;
using CWTDesktopDatabase.ViewModels;
using System.Data.SqlClient;
using System.Web.Util;

namespace CWTDesktopDatabase.Controllers
{
    [AjaxTimeOutCheck]
    public class SystemUserWizardController : Controller
    {
        //main repositories
        SystemUserRepository systemUserRepository = new SystemUserRepository();
        SystemUserWizardRepository systemUserWizardRepository = new SystemUserWizardRepository();
        WizardJSONResponse jsonResponse = new WizardJSONResponse();

        //WRAPPER PAGE FOR WIZARD- Returns View
        public ActionResult Index()
        {
            return View();
        }

        //WIZARD STEP 1: Show Search Options  - Returns PartialView
        public ActionResult SystemUserSelectionScreen()
        {
            return PartialView("SystemUserSelectionScreen", null);
        }

        //Search for Users (PART OF STEP 1)- Returns PartialView
        public ActionResult SystemUserSearch(string filterField, string filter)
        {
            filter = System.Web.HttpUtility.UrlDecode(filter);
  
            List<spDDAWizard_SelectSystemUsersFiltered_v1Result> searchResults = new List<spDDAWizard_SelectSystemUsersFiltered_v1Result>();
            searchResults = systemUserWizardRepository.GetAvailableSystemUsers(filter, filterField);
            return PartialView("SystemUserSearchResults", searchResults);
        }

        //WIZARD STEP 2A: Show a SystemUser for Editing - Returns PartialView
        [HttpPost]
        public ActionResult SystemUserDetailsScreen(string systemUserGuid)
        {
            SystemUser systemUser = new SystemUser();
            systemUser = systemUserRepository.GetUserBySystemUserGuid(systemUserGuid);

            //Check Exists
            if (systemUser == null)
            {
                return PartialView("Error", "System User Does Not Exist");
            }


            //add linked information including location
            systemUserRepository.EditForDisplay(systemUser);

            SystemUserWizardVM systemUserWizardViewModel = new SystemUserWizardVM();

            SystemUserGDSRepository systemUserGDSRepository = new SystemUserGDSRepository();
            List<fnDesktopDataAdmin_SelectSystemUserGDSs_v1Result> systemUserGDSs = new List<fnDesktopDataAdmin_SelectSystemUserGDSs_v1Result>();
            systemUserGDSs = systemUserGDSRepository.GetSystemUserGDSs(systemUser.SystemUserGuid).ToList();

            GDSRepository gdsRepository = new GDSRepository();
            List<GDS> gdss = new List<GDS>();
            gdss = gdsRepository.GetAllGDSs().ToList();

            SystemUserLocationRepository systemUserLocationRepository = new SystemUserLocationRepository();
            SystemUserLocation systemUserLocation = new SystemUserLocation();
            systemUserLocation = systemUserLocationRepository.GetSystemUserLocation(systemUser.SystemUserGuid);

			HierarchyRepository hierarchyRepository = new HierarchyRepository();

            //AccessRights           
            if (systemUserLocation != null)
            {
                RolesRepository rolesRepository = new RolesRepository();
                systemUserWizardViewModel.HasWriteAccessToLocation = rolesRepository.HasWriteAccessToLocation(systemUserLocation.LocationId);
            }
            else
            {
                systemUserWizardViewModel.HasWriteAccessToLocation = hierarchyRepository.AdminHasDomainWriteAccess("Location");
            }

			//Countries
			CountryRepository countryRepository = new CountryRepository();
			systemUserWizardViewModel.Countries = new SelectList(countryRepository.GetAllCountries().ToList(), "CountryCode", "CountryName");
			
			//ExternalSystemLogins
            ExternalSystemLoginRepository externalSystemLoginRepository = new ExternalSystemLoginRepository();
			List<ExternalSystemLoginSystemUserCountry> externalSystemLoginSystemUserCountries = new List<ExternalSystemLoginSystemUserCountry>();
			externalSystemLoginSystemUserCountries = externalSystemLoginRepository.GetBackOfficeIdentifiers(systemUser.SystemUserGuid);
			foreach (ExternalSystemLoginSystemUserCountry externalSystemLoginSystemUserCountry in externalSystemLoginSystemUserCountries)
			{
				externalSystemLoginSystemUserCountry.Countries = new SelectList(countryRepository.GetAllCountries().ToList(), "CountryCode", "CountryName", externalSystemLoginSystemUserCountry.CountryCode);
			}

			//Check Compliance Access
			ViewData["ComplianceAdministratorAccess"] = "";
			if (hierarchyRepository.AdminHasDomainWriteAccess("Compliance Administrator"))
			{
				ViewData["ComplianceAdministratorAccess"] = "WriteAccess";
			}
            
			//Check Restricted System User Access
			ViewData["RestrictedSystemUserAdministratorAccess"] = "";
			if (hierarchyRepository.AdminHasDomainWriteAccess("Restricted System User Administrator"))
			{
				ViewData["RestrictedSystemUserAdministratorAccess"] = "WriteAccess";
			}

            systemUserWizardViewModel.SystemUser = systemUser;
            systemUserWizardViewModel.SystemUserGDSs = systemUserGDSs;
            systemUserWizardViewModel.GDSs = gdss;
            systemUserWizardViewModel.SystemUserLocation = systemUserLocation;
			systemUserWizardViewModel.ExternalSystemLoginSystemUserCountries = externalSystemLoginSystemUserCountries;

			
            return PartialView("SystemUserDetailsScreen", systemUserWizardViewModel);
        }
		
		//WIZARD STEP 2B: Validate SystemUser+SystemUserLocation on Submit (no save)- Returns JSON of Step 3
        [HttpPost]
        public ActionResult ValidateSystemUser(SystemUserWizardVM systemUserWizardViewModel)
        {

            SystemUser systemUser = new SystemUser();
            systemUser = systemUserRepository.GetUserBySystemUserGuid(systemUserWizardViewModel.SystemUser.SystemUserGuid);
            systemUserWizardViewModel.SystemUser = systemUser;

            if (systemUserWizardViewModel.SystemUserLocation == null)
            {
                ModelState.Clear();
                ModelState.AddModelError("SystemUser.LocationName", "Location Required");

                //Validation Error
                string msg = "";
                foreach (ModelState modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        msg += error.ErrorMessage;
                    }
                }

                return Json(new WizardJSONResponse
                {
                    html = ControllerExtension.RenderPartialViewToString(this, "SystemUserDetailsScreen", systemUserWizardViewModel),
                    message = "ValidationError: " + msg,
                    success = false
                });
            }
           
            //Validate SystemUser data against Table
            if (!ModelState.IsValid)
            {           
               //not needed
            }

            //SystemUser + List of SystemUser's Teams 
            SystemUserTeamsVM systemUserTeamsViewModel = new SystemUserTeamsVM();

            //Add Systemuser
            systemUserTeamsViewModel.SystemUser = systemUser;

            //Add SystemUser's Teams
            SystemUserTeamRepository systemUserTeamRepository = new SystemUserTeamRepository();
            List<spDDAWizard_SelectSystemUserTeams_v1Result> systemUsers = new List<spDDAWizard_SelectSystemUserTeams_v1Result>();
            systemUserTeamsViewModel.Teams = systemUserWizardRepository.GetSystemUserTeams(systemUser.SystemUserGuid);

            //Return Page with list of SystemUser's Teams 
            return Json(new WizardJSONResponse
            {
                html = ControllerExtension.RenderPartialViewToString(this, "SystemUserTeamsScreen", systemUserTeamsViewModel),
                message = "Success",
                success = true
            });


        }

		// POST: Default Profile in Wizard
		// Returns the UserProfileIdentifier containing the DefaultProfileIdentifier flag checked for a given system user
		[HttpPost]
		public ActionResult DefaultProfileIdentifierExist(string systemUserGuid)
		{
			HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

			string result = db.fnDesktopDataAdmin_GetSystemUserDefaultProfile_v1(systemUserGuid).ToString();
			return Json(result);
		}

        //Search for Teams (PART OF STEP 3)- Returns PartialView
        public ActionResult TeamSearch(string filterField, string filter)
        {
            filter = System.Web.HttpUtility.UrlDecode(filter);

            var searchResults = systemUserWizardRepository.GetTeams(filter, filterField);
            return PartialView("TeamSearchResults", searchResults);
        }

        //WIZARD STEP 4A: Show A list of Changes made by User - Returns JSON
        [HttpPost]
        public ActionResult ConfirmChangesScreen(SystemUserWizardVM updatedSystemUser)
        {

            //Messages that will be displayed to User
            WizardMessages wizardMessages = new WizardMessages();

			//Model to Store Original Item for Comparison
            SystemUserWizardVM originalSystemUserWizardViewModel = new SystemUserWizardVM();

            //Location
            SystemUserLocationRepository systemUserLocationRepository = new SystemUserLocationRepository();
            SystemUserLocation originalSystemUserLocation = new SystemUserLocation();
            originalSystemUserLocation = systemUserLocationRepository.GetSystemUserLocation(updatedSystemUser.SystemUser.SystemUserGuid);
			if (originalSystemUserLocation != null)
			{
				originalSystemUserWizardViewModel.SystemUserLocation = originalSystemUserLocation;
			}

			//System User
			SystemUser originalSystemUser = new SystemUser();
            originalSystemUser = systemUserRepository.GetUserBySystemUserGuid(updatedSystemUser.SystemUser.SystemUserGuid);
			if (originalSystemUser != null)
			{
				originalSystemUserWizardViewModel.SystemUser = originalSystemUser;
			}

            //GDSs
            SystemUserGDSRepository systemUserGDSRepository = new SystemUserGDSRepository();
            List<fnDesktopDataAdmin_SelectSystemUserGDSs_v1Result> originalSystemuserGDSs = new List<fnDesktopDataAdmin_SelectSystemUserGDSs_v1Result>();       
            originalSystemuserGDSs = systemUserGDSRepository.GetSystemUserGDSs(updatedSystemUser.SystemUser.SystemUserGuid).ToList();
			if (originalSystemuserGDSs != null)
			{
				originalSystemUserWizardViewModel.SystemUserGDSs = originalSystemuserGDSs;
			}

			//ExternalSystemLoginSystemUserCountries
            ExternalSystemLoginRepository externalSystemLoginRepository = new ExternalSystemLoginRepository();
			List<ExternalSystemLoginSystemUserCountry> originalExternalSystemLoginSystemUserCountries = externalSystemLoginRepository.GetBackOfficeIdentifiers(updatedSystemUser.SystemUser.SystemUserGuid);
			if (originalExternalSystemLoginSystemUserCountries != null && originalExternalSystemLoginSystemUserCountries.Count > 0)
			{
				originalSystemUserWizardViewModel.ExternalSystemLoginSystemUserCountries = originalExternalSystemLoginSystemUserCountries;
			}

			systemUserWizardRepository.BuildSystemUserChangeMessages(wizardMessages, originalSystemUserWizardViewModel, updatedSystemUser);
           
            return Json(new WizardJSONResponse
            {
                html = ControllerExtension.RenderPartialViewToString(this, "ConfirmChangesScreen", wizardMessages),
                message = "Success",
                success = true
            });
        }

        //WIZARD STEP 4B: On COMPLETION - Commit Team Update/Creation to the Database - Returns JSON
        [HttpPost]
        public ActionResult CommitChanges(SystemUserWizardVM systemUserChanges)
        {

            SystemUserLocation systemUserLocation = new SystemUserLocation();
            systemUserLocation = systemUserChanges.SystemUserLocation;
            

            WizardMessages wizardMessages = new WizardMessages();

            try
            {
                TryUpdateModel(systemUserChanges.SystemUserLocation, "SystemUserLocation");
            }
            catch
            {
                //Validation Error
                string msg = "";
                foreach (ModelState modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        msg += error.ErrorMessage;
                    }
                }
                return Json(new WizardJSONResponse
                {
                    html = ControllerExtension.RenderPartialViewToString(this, "Error", msg),
                    message = msg,
                    success = false
                });
            }


			//Editing A SystemUser Location
			try
			{
				SystemUserLocationRepository systemUserLocationRepository = new SystemUserLocationRepository();
				SystemUserLocation originalSystemUserLocation = new SystemUserLocation();
				originalSystemUserLocation = systemUserLocationRepository.GetSystemUserLocation(systemUserChanges.SystemUser.SystemUserGuid);
				if (originalSystemUserLocation == null)
				{
					systemUserLocationRepository.Add(systemUserLocation);
					wizardMessages.AddMessage("User's Location successfully updated", true);
				}
				else
				{
					if (originalSystemUserLocation.LocationId != systemUserChanges.SystemUserLocation.LocationId)
					{
						systemUserLocationRepository.Update(systemUserLocation);
						wizardMessages.AddMessage("User's Location successfully updated", true);
					}
				}
			}
			catch (SqlException ex)
			{
				//If there is error we will continue, but store error to return to user

				//Versioning Error
				if (ex.Message == "SQLVersioningError")
				{
					wizardMessages.AddMessage("User's Location was not updated. Another user has already changed this Location.", false);
				}
				else //Other Error
				{
					LogRepository logRepository = new LogRepository();
					logRepository.LogError(ex.Message);

					wizardMessages.AddMessage("User's Location was not updated, please check Event Log for details", false);
					wizardMessages.AddMessage("There was a problem with your request, please see the log file or contact an administrator for details", false);
				}
			}


			//Editing Default Profile Identifier
			try
			{
				SystemUserRepository systemUserRepository = new SystemUserRepository();
				SystemUser originalSystemUser = new SystemUser();
				originalSystemUser = systemUserRepository.GetUserBySystemUserGuid(systemUserChanges.SystemUser.SystemUserGuid);
				
				if (originalSystemUser.DefaultProfileIdentifier == null)
				{
					originalSystemUser.DefaultProfileIdentifier = false;
				}
				if (originalSystemUser.DefaultProfileIdentifier != systemUserChanges.SystemUser.DefaultProfileIdentifier)
				{
					wizardMessages.AddMessage("User's Default Profile successfully updated", true);
					systemUserRepository.EditDefaultProfileIdentifier(systemUserChanges.SystemUser);
				}
			}
			catch (SqlException ex)
			{
				//If there is error we will continue, but store error to return to user

				//Versioning Error
				if (ex.Message == "SQLVersioningError")
				{
					wizardMessages.AddMessage("User's Default Profile was not updated. Another user has already changed this.", false);
				}
				else //Other Error
				{
					LogRepository logRepository = new LogRepository();
					logRepository.LogError(ex.Message);

					wizardMessages.AddMessage("User's Default Profile was not updated, please check Event Log for details", false);
					wizardMessages.AddMessage("There was a problem with your request, please see the log file or contact an administrator for details", false);
				}
			}

			//Editing Restricted Flag
			try
			{
				SystemUserRepository systemUserRepository = new SystemUserRepository();
				SystemUser originalSystemUser = new SystemUser();
				originalSystemUser = systemUserRepository.GetUserBySystemUserGuid(systemUserChanges.SystemUser.SystemUserGuid);

				if (originalSystemUser.RestrictedFlag == null)
				{
					originalSystemUser.RestrictedFlag = false;
				}
				if (originalSystemUser.RestrictedFlag != systemUserChanges.SystemUser.RestrictedFlag)
				{
					wizardMessages.AddMessage("User's Restricted Flag successfully updated", true);
					systemUserRepository.EditRestrictedFlag(systemUserChanges.SystemUser);
				}
			}
			catch (SqlException ex)
			{
				//If there is error we will continue, but store error to return to user

				//Versioning Error
				if (ex.Message == "SQLVersioningError")
				{
					wizardMessages.AddMessage("User's Restricted Flag was not updated. Another user has already changed this.", false);
				}
				else //Other Error
				{
					LogRepository logRepository = new LogRepository();
					logRepository.LogError(ex.Message);

					wizardMessages.AddMessage("User's Restricted Flag was not updated, please check Event Log for details", false);
					wizardMessages.AddMessage("There was a problem with your request, please see the log file or contact an administrator for details", false);
				}
			}

			//Editing Cuba Booking Allowed Identifier
			try
			{
				SystemUserRepository systemUserRepository = new SystemUserRepository();
				SystemUser originalSystemUser = new SystemUser();
				originalSystemUser = systemUserRepository.GetUserBySystemUserGuid(systemUserChanges.SystemUser.SystemUserGuid);
				
				if (originalSystemUser.CubaBookingAllowanceIndicator == null)
				{
					originalSystemUser.CubaBookingAllowanceIndicator = false;
				}

				if (originalSystemUser.CubaBookingAllowanceIndicator != systemUserChanges.SystemUser.CubaBookingAllowanceIndicator)
				{
					wizardMessages.AddMessage("User's Cuba Booking Allowed successfully updated", true);
					systemUserRepository.EditCubaBookingAllowanceIndicator(systemUserChanges.SystemUser);
				}
			}
			catch (SqlException ex)
			{
				//If there is error we will continue, but store error to return to user

				//Versioning Error
				if (ex.Message == "SQLVersioningError")
				{
					wizardMessages.AddMessage("User's Cuba Booking Allowed was not updated. Another user has already changed this.", false);
				}
				else //Other Error
				{
					LogRepository logRepository = new LogRepository();
					logRepository.LogError(ex.Message);

					wizardMessages.AddMessage("User's Cuba Booking Allowed was not updated, please check Event Log for details", false);
					wizardMessages.AddMessage("There was a problem with your request, please see the log file or contact an administrator for details", false);
				}
			}


			//ExternalSystemLoginSystemUserCountries
			ExternalSystemLoginRepository externalSystemLoginRepository = new ExternalSystemLoginRepository();
			List<ExternalSystemLoginSystemUserCountry> originalExternalSystemLoginSystemUserCountries = externalSystemLoginRepository.GetBackOfficeIdentifiers(systemUserChanges.SystemUser.SystemUserGuid);
			if (originalExternalSystemLoginSystemUserCountries != null && originalExternalSystemLoginSystemUserCountries.Count == 0)
			{
				originalExternalSystemLoginSystemUserCountries = null;
			}

			if (originalExternalSystemLoginSystemUserCountries != systemUserChanges.ExternalSystemLoginSystemUserCountries)
			{
				string systemUserGuid = systemUserChanges.SystemUser.SystemUserGuid;
				try
				{
					externalSystemLoginRepository.AddBackOfficeIdentifiers(systemUserGuid, systemUserChanges.ExternalSystemLoginSystemUserCountries);
					wizardMessages.AddMessage("User's Back Office Identifiers successfully updated", true);
				}
				catch (SqlException ex)
				{
					//Versioning Error
					if (ex.Message == "SQLVersioningError")
					{
						wizardMessages.AddMessage("User's BackOffice Identifier was not updated. Another user has already changed this item.", false);
					}
					else //Other Error
					{
						LogRepository logRepository = new LogRepository();
						logRepository.LogError(ex.Message);

						wizardMessages.AddMessage("User's BackOffice Identifier was not updated, please check Event Log for details", false);
						wizardMessages.AddMessage("There was a problem with your request, please see the log file or contact an administrator for details", false);
					}
				}
			}

            //we continue to add Teams
            try
            {
                wizardMessages = systemUserWizardRepository.UpdateSystemUserTeams(systemUserChanges, wizardMessages);

            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                wizardMessages.AddMessage("User's Teams were not changed, please check Event Log for details", false);
                wizardMessages.AddMessage("There was a problem with your request, please see the log file or contact an administrator for details", false);
            }

            //we continue to add GDSs
            if (systemUserChanges.GDSChanged)
            {
                try
                {
                    wizardMessages = systemUserWizardRepository.UpdateSystemUserGDSs(systemUserChanges, wizardMessages);

                }
                catch (SqlException ex)
                {
                    LogRepository logRepository = new LogRepository();
                    logRepository.LogError(ex.Message);

                    wizardMessages.AddMessage("User's GDS settings were not changed, please check Event Log for details", false);
                    wizardMessages.AddMessage("There was a problem with your request, please see the log file or contact an administrator for details", false);
                }
            }
            return Json(new
            {
                html = ControllerExtension.RenderPartialViewToString(this, "FinishedScreen", wizardMessages),
                message = "Success",
                success = true
            });
        }
         
    }
}
