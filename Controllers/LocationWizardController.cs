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
    public class LocationWizardController : Controller
    {
        //main repositories
        LocationRepository locationRepository = new LocationRepository();
        LocationWizardRepository locationWizardRepository = new LocationWizardRepository();
        HierarchyRepository hierarchyRepository = new HierarchyRepository();
        RolesRepository rolesRepository = new RolesRepository();

        private string groupName = "Location";

        //WRAPPER PAGE FOR WIZARD- Returns View
        public ActionResult Index()
        {
            return View();
        }

        //WIZARD STEP 1: Show a list of Locations  - Returns PartialView
		[HttpPost]
		public ActionResult LocationSelectionScreen(string filter, string filterField)
        {
            //Check Access Rights
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                return PartialView("Error", "You Do Not Have Access to Locations. Please Contact an Administrator");
            }

            filter = System.Web.HttpUtility.UrlDecode(filter);
            var locations = locationWizardRepository.GetLocations(filter ?? "", filterField ?? "");

            return PartialView("LocationSelectionScreen", locations);
        }

        //WIZARD STEP 2A: Show a Team for Adding/Editing/Deleting - Returns PartialView
        [HttpPost]
        public ActionResult LocationDetailsScreen(int locationId = 0)
        {
            if (!hierarchyRepository.AdminHasDomainWriteAccess("Location"))
            {
                return PartialView("Error", "You do not have access to this item");
            }

            Location location = new Location();
            Address address = new Address();
           
            CountryRepository countryRepository = new CountryRepository();
            if (locationId > 0)
            {

                location = locationRepository.GetLocation(locationId);

                //Check Exists
                if (location == null)
                {
                    return PartialView("Error", "Location Does Not Exist");
                }
                
                //get Address
                address = locationRepository.GetLocationAddress(locationId);
                AddressRepository addressRepository = new AddressRepository();
                addressRepository.EditForDisplay(address);

                //add linked information
                locationRepository.EditForDisplay(location);
            }

            LocationWizardVM locationWizardViewModel = new LocationWizardVM();
            locationWizardViewModel.Location = location;
            locationWizardViewModel.Address = address;
            locationWizardViewModel.SystemUserCount = locationWizardRepository.GetLocationSystemUsers(locationId).Count;

            MappingQualityRepository mappingQualityRepository = new MappingQualityRepository();
            if (locationWizardViewModel.Address != null)
            {
                locationWizardViewModel.MappingQualityCodes = new SelectList(mappingQualityRepository.GetAllMappingQualities().ToList(), "MappingQualityCode", "MappingQualityCode", locationWizardViewModel.Address.MappingQualityCode);
            }
            else
            {
                locationWizardViewModel.MappingQualityCodes = new SelectList(mappingQualityRepository.GetAllMappingQualities().ToList(), "MappingQualityCode", "MappingQualityCode");
            }

            //StateProvince SelectList
            StateProvinceRepository stateProvinceRepository = new StateProvinceRepository();
            locationWizardViewModel.StateProvinces = new SelectList(stateProvinceRepository.GetStateProvincesByCountryCode(location.CountryCode).ToList(), "StateProvinceCode", "Name", address.StateProvinceCode);

            return PartialView("LocationDetailsScreen", locationWizardViewModel);
        }

        //Show Popup When a User tries To Delete (PART OF STEP 2)- Returns PartialView
        public ActionResult ShowConfirmDelete(int locationId = 0)
        {
            //Access to Locations
            if (!hierarchyRepository.AdminHasDomainWriteAccess("Location"))
            {
                return PartialView("Error", "You do not have access to this item");
            }
           
            Location location = new Location();
            location = locationRepository.GetLocation(locationId);

            //Check Exists
            if (location == null)
            {
                return PartialView("Error", "Location Does Not Exist");
            }
            //Access to this Location
            if (!rolesRepository.HasWriteAccessToLocation(locationId))
            {
                return PartialView("Error", "You do not have access to this item");
            }
            LocationSystemUsersVM locationUsersScreenViewModel = new LocationSystemUsersVM();
            locationUsersScreenViewModel.SystemUsers = locationWizardRepository.GetLocationSystemUsers(locationId);
            locationUsersScreenViewModel.Location = location;

            List<spDDAWizard_SelectLocationTeams_v1Result> teams = new List<spDDAWizard_SelectLocationTeams_v1Result>();
            teams = locationWizardRepository.GetLocationTeams(locationId);
            locationUsersScreenViewModel.LocationTeamCount = teams.Count();

            return PartialView("ConfirmDeletePopup", locationUsersScreenViewModel);
        }

        //Show 2nd Popup When a User tries To Delete (PART OF STEP 2)- Returns PartialView
        public ActionResult ShowConfirmDelete2(int locationId)
        {
            //Access to Locations
            if (!hierarchyRepository.AdminHasDomainWriteAccess("Location"))
            {
                return PartialView("Error", "You do not have access to this item");
            }
           
            Location location = new Location();
            location = locationRepository.GetLocation(locationId);

            //Check Exists
            if (location == null)
            {
                return PartialView("Error", "Location Does Not Exist");
            }
            //Access to this Location
            if (!rolesRepository.HasWriteAccessToLocation(locationId))
            {
                return PartialView("Error", "You do not have access to this item");
            }
            LocationTeamsVM locationTeamsScreenViewModel = new LocationTeamsVM();
            locationTeamsScreenViewModel.Teams = locationWizardRepository.GetLocationTeams(locationId);
            locationTeamsScreenViewModel.Location = location;

            LocationLinkedItemsVM locationLinkedItems = new LocationLinkedItemsVM();
            locationLinkedItems.Location = location;
            locationWizardRepository.AddLinkedItems(locationLinkedItems);
            locationTeamsScreenViewModel.LinkedItemsCount =
                    (locationLinkedItems.Contacts.Count +

                    locationLinkedItems.CreditCards.Count +
                    locationLinkedItems.ExternalSystemParameters.Count +
                    locationLinkedItems.GDSAdditionalEntries.Count +
                    locationLinkedItems.LocalOperatingHoursGroups.Count +

                    locationLinkedItems.PolicyGroups.Count +
                    locationLinkedItems.PublicHolidayGroups.Count +
                    locationLinkedItems.QueueMinderGroups.Count +
                    locationLinkedItems.ServicingOptionGroups.Count +
                    locationLinkedItems.TicketQueueGroups.Count +
                    locationLinkedItems.TripTypeGroups.Count);
                    //removing these as we only want the items that prevent us deleting
                    //locationLinkedItems.PNROutputGroups.Count +
                    //locationLinkedItems.Addresses.Count + 
                    //locationLinkedItems.ValidPseudoCityOrOfficeIds.Count +
                    //locationLinkedItems.WorkFlowGroups.Count);

            return PartialView("ConfirmDeletePopup2", locationTeamsScreenViewModel);
        }

        //Show 3rd Popup When a User tries To Delete (PART OF STEP 2)- Returns PartialView
        public ActionResult ShowConfirmDelete3(int locationId)
        {
            
            Location location = new Location();
            location = locationRepository.GetLocation(locationId);

            //Check Exists
            if (location == null)
            {
                return PartialView("Error", "Location Does Not Exist");
            }
            //Access Rights to this Location
            if (!rolesRepository.HasWriteAccessToLocation(location.LocationId))
            {
                return PartialView("Error", "You do not have access to this item");
            }
            LocationLinkedItemsVM locationLinkedItemsScreenViewModel = new LocationLinkedItemsVM();
            locationLinkedItemsScreenViewModel.Location = location;
            locationWizardRepository.AddLinkedItems(locationLinkedItemsScreenViewModel);


            return PartialView("ConfirmDeletePopup3", locationLinkedItemsScreenViewModel);
        }

        //Delete Team (PART OF STEP 2)- Returns JSON
        [HttpPost]
        public ActionResult DeleteLocation(Location location)
        {
            //Access Rights to Locations
            if (!hierarchyRepository.AdminHasDomainWriteAccess("Location"))
            {
                return PartialView("Error", "You do not have access to this item");
            }
           
            //Check Exists
            if (location == null)
            {
                return Json(new WizardJSONResponse
                {
                    html = ControllerExtension.RenderPartialViewToString(this, "Error", "Location does not Exist"),
                    message = "NoRecordError",
                    success = false
                });
            }
            //Access Rights to this Location
            if (!rolesRepository.HasWriteAccessToLocation(location.LocationId))
            {
                return PartialView("Error", "You do not have access to this item");
            }

            //Delete Item
            try
            {
                locationRepository.Delete(location);
            }
            catch (SqlException ex)
            {
                if (ex.Message == "SQLVersioningError")
                {
                    return Json(new WizardJSONResponse
                    {
                        html = ControllerExtension.RenderPartialViewToString(this, "VersionError", null),
                        message = "VersionError",
                        success = false
                    });
                }
                if (ex.Message == "SQLDeleteError")
                {
                    return Json(new WizardJSONResponse
                    {
                        html = ControllerExtension.RenderPartialViewToString(this, "DeleteError", null),
                        message = "DeleteError",
                        success = false
                    });
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                return Json(new WizardJSONResponse
                {
                    html = ControllerExtension.RenderPartialViewToString(this, "Error", ex.Message),
                    message = "DBError",
                    success = false
                });
            }

            WizardMessages wizardMessages = new WizardMessages();
            wizardMessages.AddMessage("Location has been successfully deleted.", true);

            //Item Deleted - Return response
            return Json(new WizardJSONResponse
            {
                html = ControllerExtension.RenderPartialViewToString(this, "FinishedScreen", wizardMessages),
                message = "Success",
                success = true
            });
        }

        //WIZARD STEP 2B: Validate Location on Submit (no save)- Returns STEP 3 as JSON
        [HttpPost]
        public ActionResult ValidateLocation(LocationWizardVM locationWizardViewModel)
        {					
			int locationId = 0;
            if(locationWizardViewModel.Location != null){
                locationId = locationWizardViewModel.Location.LocationId;
            }

            //Validate Team data against Table
            if (!ModelState.IsValid)
            {
                string n = "";
                foreach (ModelState modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        n += error.ErrorMessage;
                    }
                }

                //Need to repopulate List
                MappingQualityRepository mappingQualityRepository = new MappingQualityRepository();
                locationWizardViewModel.MappingQualityCodes = new SelectList(mappingQualityRepository.GetAllMappingQualities().ToList(), "MappingQualityCode", "MappingQualityCode", locationWizardViewModel.Address.MappingQualityCode); ;
                locationWizardViewModel.SystemUserCount = locationWizardRepository.GetLocationSystemUsers(locationId).Count;

                //StateProvince SelectList
                StateProvinceRepository stateProvinceRepository = new StateProvinceRepository();
                locationWizardViewModel.StateProvinces = new SelectList(stateProvinceRepository.GetStateProvincesByCountryCode(locationWizardViewModel.Location.CountryCode).ToList(), "StateProvinceCode", "Name", locationWizardViewModel.Address.StateProvinceCode);

                //Validation Error - retrun to Details Screen
                return Json(new WizardJSONResponse
                {
                    html = ControllerExtension.RenderPartialViewToString(this, "LocationDetailsScreen", locationWizardViewModel),
                    message = "ValidationError (" + n + ")",
                    success = false
                });
            }

            //Location
            Location location = new Location();
            location = locationWizardViewModel.Location;
            LocationSystemUsersVM locationUsersScreen = new LocationSystemUsersVM();
            locationUsersScreen.Location = locationWizardViewModel.Location;

            // Systemusers
            List<spDDAWizard_SelectLocationSystemUsers_v1Result> systemUsers = new List<spDDAWizard_SelectLocationSystemUsers_v1Result>();
            systemUsers = locationWizardRepository.GetLocationSystemUsers(location.LocationId);
            locationUsersScreen.SystemUsers = systemUsers;

            //Show Location's systemUsers
            return Json(new WizardJSONResponse
            {
                html = ControllerExtension.RenderPartialViewToString(this, "LocationUsersScreen", locationUsersScreen),
                message = "Success",
                success = true
            });

        }

        //Search for Users (PART OF STEP 3)- Returns PartialView
        public ActionResult SystemUserSearch(string filterField, string filter)
        {
            filter = System.Web.HttpUtility.UrlDecode(filter);
            List<spDDAWizard_SelectSystemUsersFiltered_v1Result> searchResults = new List<spDDAWizard_SelectSystemUsersFiltered_v1Result>();
            searchResults = locationWizardRepository.GetLocationAvailableSystemUsers(filter, filterField);
            return PartialView("SystemUserSearchResults", searchResults);
        }

        //WIZARD STEP 4A: Show A list of Changes made by User - Returns JSON
        [HttpPost]
        public ActionResult ConfirmChangesScreen(LocationWizardVM updatedLocationViewModel)
        {

            //Make sure we have minimum data
            if (updatedLocationViewModel.Location == null)
            {
                return Json(new WizardJSONResponse
                {
                    html = ControllerExtension.RenderPartialViewToString(this, "Error", "No Location Information"),
                    message = "Error",
                    success = false
                });
            }

            //get LocationId
            int locationId = updatedLocationViewModel.Location.LocationId;

            //Object to store messages for Display
            WizardMessages wizardMessages = new WizardMessages();

            //Editing a Location
            if (locationId > 0)
            {

                //Get Original Information from Databse
                Location originalLocation = new Location();
                originalLocation = locationRepository.GetLocation(locationId);

                AddressRepository addressRepository = new AddressRepository();
                Address originalAddress = new Address();
                int addressId = updatedLocationViewModel.Address.AddressId;
                originalAddress = addressRepository.GetAddress(addressId);

                LocationWizardVM originalLocationViewModel = new LocationWizardVM();
                originalLocationViewModel.Location = originalLocation;
                originalLocationViewModel.Address = originalAddress;

                //Compare Original Information to Submitted Information to Create a List of Messages about changes
                locationWizardRepository.BuildLocationChangeMessages(wizardMessages, originalLocationViewModel, updatedLocationViewModel);
            }
            else
            {
                //Adding an Item - Create a List of Messages about changes
                locationWizardRepository.BuildLocationChangeMessages(wizardMessages, null, updatedLocationViewModel);
            }

            //Return List of Changes to user for Final Confirmation
            return Json(new WizardJSONResponse
            {
                html = ControllerExtension.RenderPartialViewToString(this, "ConfirmChangesScreen", wizardMessages),
                message = "Success",
                success = true
            });
        }

        //WIZARD STEP 4B: On COMPLETION - Commit Location Update/Creation to the Database - Returns JSON
        [HttpPost]
        public ActionResult CommitChanges(LocationWizardVM locationChanges)
        {			
			Location location = new Location();
            location = locationChanges.Location;
            string msg = "";

            Address address = new Address();
            address = locationChanges.Address;

            WizardMessages wizardMessages = new WizardMessages();

            try
            {
                UpdateModel(location);
            }
            catch
            {
                //Validation Error            
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
            //Editing A Location
            if (location.LocationId > 0)
            {
                //Access Rights to this Location
                if (!rolesRepository.HasWriteAccessToLocation(location.LocationId))
                {
                    msg =  "You do not have access to this item";
                    return Json(new WizardJSONResponse
                    {
                        html = ControllerExtension.RenderPartialViewToString(this, "Error", msg),
                        message = msg,
                        success = false
                    });
                }

               
                try
                {
                    locationWizardRepository.UpdateLocationAddress(location, address);
                    wizardMessages.AddMessage("Location Details successfully updated", true);
                }
                catch (SqlException ex)
                {
                    //If there is error we will continue, but store error to return to user

                    //Versioning Error
                    if (ex.Message == "SQLVersioningError")
                    {
                        wizardMessages.AddMessage("Location was not updated. Another user has already changed this Location.", false);
                    }
                    else //Other Error
                    {
                        LogRepository logRepository = new LogRepository();
                        logRepository.LogError(ex.Message);

                        wizardMessages.AddMessage("Location Details were not updated, please check Event Log for details", false);
                        wizardMessages.AddMessage("There was a problem with your request, please see the log file or contact an administrator for details", false);
                    }
                }
            }
            else //Adding A LOcation
            {
                //Access Rights to Locations
                if (!hierarchyRepository.AdminHasDomainWriteAccess("Location"))
                {
                    msg = "You do not have access to this item";
                    return Json(new WizardJSONResponse
                    {
                        html = ControllerExtension.RenderPartialViewToString(this, "Error", msg),
                        message = msg,
                        success = false
                    });
                }
                try
                {
                    int locationId = locationWizardRepository.AddLocationAddress(location, address);
                    location = locationRepository.GetLocation(locationId);
                    locationChanges.Location = location;
                    wizardMessages.AddMessage("Location added successfully", true);
                }
                catch (SqlException ex)
                {
                    LogRepository logRepository = new LogRepository();
                    logRepository.LogError(ex.Message);

                    wizardMessages.AddMessage("Location was not added, please check Event Log for details", false);
                    wizardMessages.AddMessage("There was a problem with your request, please see the log file or contact an administrator for details", false);

                    //If we cannot add a Team , we cannot continue, so return error to User
                    return Json(new
                    {
                        html = ControllerExtension.RenderPartialViewToString(this, "FinishedScreen", wizardMessages),
                        message = "DBError",
                        success = false
                    });
                }
            }
            //If we have added a Location successfully, or edited a Location (successfully or unsuccessfully), 
            //we continue to make SystemUsers changes
            if (locationChanges.SystemUsersAdded != null || locationChanges.SystemUsersRemoved != null)
            {
                try
                {
                    wizardMessages = locationWizardRepository.UpdateLocationSystemUsers(locationChanges, wizardMessages);

                }
                catch (SqlException ex)
                {
                    LogRepository logRepository = new LogRepository();
                    logRepository.LogError(ex.Message);

                    wizardMessages.AddMessage("Team SystemUser were not changed, please check Event Log for details", false);
                    wizardMessages.AddMessage("There was a problem with your request, please see the log file or contact an administrator for details", false);
                }
            }
            //return to user
            return Json(new
            {
                html = ControllerExtension.RenderPartialViewToString(this, "FinishedScreen", wizardMessages),
                message = "Success",
                success = true
            });
        }
    }
}
