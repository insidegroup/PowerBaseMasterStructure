using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;

namespace CWTDesktopDatabase.Repository
{
    public class LocationWizardRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //List of All Teams that an Admin can Edit
        public List<spDDAWizard_SelectLocationsFiltered_v1Result> GetLocations(string filter, string filterField)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            return db.spDDAWizard_SelectLocationsFiltered_v1(filter, filterField,adminUserGuid).ToList();
        }

        //Get all SystemUsers of a Location
        public List<spDDAWizard_SelectLocationSystemUsers_v1Result> GetLocationSystemUsers(int locationId)
        {
            return db.spDDAWizard_SelectLocationSystemUsers_v1(locationId).ToList();
        }

         //Get all Teams of a Location
        public List<spDDAWizard_SelectLocationTeams_v1Result> GetLocationTeams(int locationId)
        {
            return db.spDDAWizard_SelectLocationTeams_v1(locationId).ToList();
        }

        //Get Filtered List Of SystemUsers
        public List<spDDAWizard_SelectSystemUsersFiltered_v1Result> GetLocationAvailableSystemUsers(string filter, string filterField)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            return db.spDDAWizard_SelectSystemUsersFiltered_v1(filter, filterField, adminUserGuid).ToList();
        }

        //Get List of Items attached to a Location (to populate LocationLinkedItemsVM)
        public void AddLinkedItems(LocationLinkedItemsVM locationLinkedItemsScreenViewModel)
        {
            int locationId = locationLinkedItemsScreenViewModel.Location.LocationId;

            HierarchyDC hierarchyDC = new HierarchyDC(Settings.getConnectionString());
            locationLinkedItemsScreenViewModel.Addresses =
                (from n in hierarchyDC.LocationAddresses where n.LocationId == locationId select n.Address).ToList();

            locationLinkedItemsScreenViewModel.Contacts =
                (from n in hierarchyDC.LocationContacts where n.LocationId == locationId select n.Contact).ToList();
            
            ExternalSystemParameterDC externalSystemParameterDC = new ExternalSystemParameterDC(Settings.getConnectionString());
            locationLinkedItemsScreenViewModel.ExternalSystemParameters =
                (from n in externalSystemParameterDC.ExternalSystemParameterLocations where n.LocationId == locationId select n.ExternalSystemParameter).ToList();

            GDSAdditionalEntryDC gdsAdditionalEntryDC = new GDSAdditionalEntryDC(Settings.getConnectionString());
            locationLinkedItemsScreenViewModel.GDSAdditionalEntries =
                (from n in gdsAdditionalEntryDC.GDSAdditionalEntryLocations  where n.LocationId == locationId select n.GDSAdditionalEntry).ToList();

            
            locationLinkedItemsScreenViewModel.CreditCards =
                (from n in hierarchyDC.CreditCardLocations where n.LocationId == locationId select n.CreditCard).ToList();

            locationLinkedItemsScreenViewModel.QueueMinderGroups =
                (from n in hierarchyDC.QueueMinderGroupLocations where n.LocationId == locationId select n.QueueMinderGroup).ToList();


            locationLinkedItemsScreenViewModel.ValidPseudoCityOrOfficeIds =
                (from n in hierarchyDC.LocationDefaultPseudoCityOrOfficeIds where n.LocationId == locationId select n.ValidPseudoCityOrOfficeId).ToList();

            ServicingOptionGroupDC servicingOptionGroupDC = new ServicingOptionGroupDC(Settings.getConnectionString());
            locationLinkedItemsScreenViewModel.ServicingOptionGroups =
                (from n in servicingOptionGroupDC.ServicingOptionGroupLocations where n.LocationId == locationId select n.ServicingOptionGroup).ToList();

            TicketQueueGroupDC ticketQueueGroupDC = new TicketQueueGroupDC(Settings.getConnectionString());
            locationLinkedItemsScreenViewModel.TicketQueueGroups =
                (from n in ticketQueueGroupDC.TicketQueueGroupLocations where n.LocationId == locationId select n.TicketQueueGroup).ToList();

            TripTypeGroupDC tripTypeGroupDC = new TripTypeGroupDC(Settings.getConnectionString());
            locationLinkedItemsScreenViewModel.TripTypeGroups =
                (from n in tripTypeGroupDC.TripTypeGroupLocations where n.LocationId == locationId select n.TripTypeGroup).ToList();

            locationLinkedItemsScreenViewModel.PNROutputGroups =
                (from n in hierarchyDC.PNROutputGroupLocations where n.LocationId == locationId select n.PNROutputGroup).ToList();

            PublicHolidayGroupDC publicHolidayGroupDC = new PublicHolidayGroupDC(Settings.getConnectionString());
            locationLinkedItemsScreenViewModel.PublicHolidayGroups =
                (from n in publicHolidayGroupDC.PublicHolidayGroupLocations where n.LocationId == locationId select n.PublicHolidayGroup).ToList();

            locationLinkedItemsScreenViewModel.WorkFlowGroups =
                (from n in hierarchyDC.WorkFlowGroupLocations where n.LocationId == locationId select n.WorkFlowGroup).ToList();

            locationLinkedItemsScreenViewModel.Teams =
                (from n in hierarchyDC.TeamLocations where n.LocationId == locationId select n.Team).ToList();
            
            locationLinkedItemsScreenViewModel.LocalOperatingHoursGroups =
                (from n in hierarchyDC.LocalOperatingHoursGroupLocations where n.LocationId == locationId select n.LocalOperatingHoursGroup).ToList();

            PolicyGroupDC policyGroupDC = new PolicyGroupDC(Settings.getConnectionString());
            locationLinkedItemsScreenViewModel.PolicyGroups =
                (from n in policyGroupDC.PolicyGroupLocations where n.LocationId == locationId select n.PolicyGroup).ToList();
        }

        //Compare two Locations and return a list of messages about changes
        public WizardMessages BuildLocationChangeMessages(WizardMessages wizardMessages, LocationWizardVM originalLocationViewModel, LocationWizardVM updatedLocationViewModel)
        {

            /*Location*/
            LocationRepository locationRepository = new LocationRepository();
            Location originalLocation = new Location();
            Location updatedLocation = new Location();

            //If Editing a Location
            if (originalLocationViewModel != null)
            {
                originalLocation = originalLocationViewModel.Location;
                locationRepository.EditForDisplay(originalLocation);
            }
            updatedLocation = updatedLocationViewModel.Location;
            locationRepository.EditForDisplay(updatedLocation);

            if (originalLocation.LocationId == 0)
            {
                wizardMessages.AddMessage("A new Location \"" + updatedLocation.LocationName + "\" has been added.", true);
            }
            else
            {

                if (originalLocation.LocationName != updatedLocation.LocationName)
                {
                    wizardMessages.AddMessage("Location Name will be updated to \"" + updatedLocation.LocationName + "\".", true);
                }

                if (originalLocation.CountryCode != updatedLocation.CountryCode)
                {
                    wizardMessages.AddMessage("Location Country will be updated to \"" + updatedLocation.CountryRegionName + "\".", true);
                }
                if (originalLocation.CountryRegionId != updatedLocation.CountryRegionId)
                {
                    wizardMessages.AddMessage("Location Country Region will be updated to \"" + updatedLocation.CountryRegionName + "\".", true);
                }

               
            }

            /*Address*/
            AddressRepository addressRepository = new AddressRepository();
            Address originalAddress = new Address();
            Address updatedAddress = new Address();
            
            

            if (originalLocationViewModel != null)
            {
                originalAddress = originalLocationViewModel.Address;
                if (originalAddress != null)
                {
                    addressRepository.EditForDisplay(originalAddress);
                }
            }
            updatedAddress = updatedLocationViewModel.Address;
            addressRepository.EditForDisplay(updatedAddress);

            if (originalAddress == null)
             {
                //need this for Comparisons to work
                originalAddress = new Address();
            }

            if (originalAddress.FirstAddressLine != updatedAddress.FirstAddressLine || originalAddress.SecondAddressLine != updatedAddress.SecondAddressLine)
            {
                wizardMessages.AddMessage("Address Lines will be updated", true);
            }

            if (originalAddress.CityName != updatedAddress.CityName)
            {
                wizardMessages.AddMessage("City will be updated to \"" + updatedAddress.CityName + "\".", true);
            }
            if (originalAddress.StateProvinceCode != updatedAddress.StateProvinceCode)
            {
                wizardMessages.AddMessage("State/Province will be updated to \"" + updatedAddress.StateProvinceName + "\".", true);
            }
            if (originalAddress.CountyName != updatedAddress.CountyName)
            {
                wizardMessages.AddMessage("County will be updated to \"" + updatedAddress.CountyName + "\".", true);
            }
            if (originalAddress.LatitudeDecimal != updatedAddress.LatitudeDecimal)
            {
                wizardMessages.AddMessage("Latitude will be updated to \"" + updatedAddress.LatitudeDecimal + "\".", true);
            }
            if (originalAddress.LongitudeDecimal != updatedAddress.LongitudeDecimal)
            {
                wizardMessages.AddMessage("Longitude will be updated to \"" + updatedAddress.LongitudeDecimal + "\".", true);
            }
            if (originalAddress.MappingQualityCode != updatedAddress.MappingQualityCode)
            {
                wizardMessages.AddMessage("Mapping Quality Code will be updated to \"" + updatedAddress.MappingQualityCode + "\".", true);
            }
            if (originalAddress.PostalCode != updatedAddress.PostalCode)
            {
                wizardMessages.AddMessage("PostalCode will be updated to \"" + updatedAddress.PostalCode + "\".", true);
            }
            if (originalAddress.ReplicatedFromClientMaintenanceFlag != updatedAddress.ReplicatedFromClientMaintenanceFlag)
            {
                wizardMessages.AddMessage("Replicated from ClientMaintenance Flag will be updated to \"" + updatedAddress.ReplicatedFromClientMaintenanceFlag + "\".", true);
            }
            //Not Needed - Country is also Part of Location
            //if (originalAddress.CountryCode != updatedAddress.CountryCode)
            //{
            //    wizardMessages.AddMessage("Country will be updated to \"" + updatedAddress.CountryName + "\".", true);
            //}


            

            /*Systemusers*/
            SystemUserRepository systemUserRepository = new SystemUserRepository();
            SystemUserLocationRepository systemUserLocationRepository = new SystemUserLocationRepository();

            if (updatedLocationViewModel.SystemUsersAdded != null)
            {
                if (updatedLocationViewModel.SystemUsersAdded.Count > 0)
                {
                    foreach (SystemUser item in updatedLocationViewModel.SystemUsersAdded)
                    {
                        SystemUser systemUser = new SystemUser();
                        systemUser = systemUserRepository.GetUserBySystemUserGuid(item.SystemUserGuid);
                        if (systemUser != null)
                        {
                            wizardMessages.AddMessage("You will add User \"" + systemUser.LastName + "," + (systemUser.MiddleName != "" ? systemUser.MiddleName + " " : "") + systemUser.FirstName + "\".", true);

                            SystemUserLocation systemUserLocation = new SystemUserLocation();
                            systemUserLocation = systemUserLocationRepository.GetSystemUserLocation(item.SystemUserGuid);
                            if (systemUserLocation != null)
                            {        
                                systemUserLocationRepository.EditForDisplay(systemUserLocation);
                                wizardMessages.AddMessage("The user will be moved from Location: "+systemUserLocation.LocationName+".", true);
                            }
                        }
                    }
                }
            }
            if (updatedLocationViewModel.SystemUsersRemoved != null)
            {
                if (updatedLocationViewModel.SystemUsersRemoved.Count > 0)
                {
                    foreach (SystemUser item in updatedLocationViewModel.SystemUsersRemoved)
                    {
                        SystemUser systemUser = new SystemUser();
                        systemUser = systemUserRepository.GetUserBySystemUserGuid(item.SystemUserGuid);
                        if (systemUser != null)
                        {
                            wizardMessages.AddMessage("You will remove User \"" + systemUser.LastName + "," + (systemUser.MiddleName != "" ? systemUser.MiddleName + " " : "") + systemUser.FirstName + "\".", true);
                        }
                    }
                }
            }
           
           

          
            return wizardMessages;
        }

        //Update Location SystemUsers
        public WizardMessages UpdateLocationSystemUsers(LocationWizardVM locationChanges, WizardMessages wizardMessages)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            Location location = new Location();
            location = locationChanges.Location;
            bool changesExist = false;

            // Create the xml document container
            XmlDocument doc = new XmlDocument();// Create the XML Declaration, and append it to XML document
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(dec);
            XmlElement root = doc.CreateElement("LocationSystemUsers");
            doc.AppendChild(root);

            SystemUserRepository systemUserRepository = new SystemUserRepository();
            if (locationChanges.SystemUsersAdded != null)
            {
                if (locationChanges.SystemUsersAdded.Count > 0)
                {
                    changesExist = true;
                    //xml = xml + "<SystemUsersAdded>";
                    XmlElement xmlSystemUsersAdded = doc.CreateElement("SystemUsersAdded");

                    foreach (SystemUser item in locationChanges.SystemUsersAdded)
                    {
                        SystemUser systemUser = new SystemUser();
                        systemUser = systemUserRepository.GetUserBySystemUserGuid(item.SystemUserGuid);
                        string systemUserName = systemUser.FirstName + (item.MiddleName != "" ? item.MiddleName + " " : "") + systemUser.LastName;
                        //xml = xml + "<SystemUser>";
                        //xml = xml + "<SystemUserName>" + systemUserName + "</SystemUserName>";
                        //xml = xml + "<SystemUserGuid>" + item.SystemUserGuid + "</SystemUserGuid>";
                        //xml = xml + "</SystemUser>";

                        XmlElement xmlSystemUser = doc.CreateElement("SystemUser");
                        xmlSystemUsersAdded.AppendChild(xmlSystemUser);

                        XmlElement xmlSystemUserName = doc.CreateElement("SystemUserName");
                        xmlSystemUserName.InnerText = systemUserName;
                        xmlSystemUser.AppendChild(xmlSystemUserName);

                        XmlElement xmlSystemUserGuid = doc.CreateElement("SystemUserGuid");
                        xmlSystemUserGuid.InnerText = item.SystemUserGuid;
                        xmlSystemUser.AppendChild(xmlSystemUserGuid);
                    }
                    root.AppendChild(xmlSystemUsersAdded);
                    //xml = xml + "</SystemUsersAdded>";
                }
            }
            if (locationChanges.SystemUsersRemoved != null)
            {
                if (locationChanges.SystemUsersRemoved.Count > 0)
                {
                    changesExist = true;
                    //xml = xml + "<SystemUsersRemoved>";
                    XmlElement xmlSystemUsersRemoved = doc.CreateElement("SystemUsersRemoved");

                    foreach (SystemUser item in locationChanges.SystemUsersRemoved)
                    {
                        SystemUser systemUser = new SystemUser();
                        systemUser = systemUserRepository.GetUserBySystemUserGuid(item.SystemUserGuid);
                        string systemUserName = systemUser.FirstName + (item.MiddleName != "" ? item.MiddleName + " " : "") + systemUser.LastName;
                        //xml = xml + "<SystemUser>";
                        //xml = xml + "<SystemUserName>" + systemUserName + "</SystemUserName>";
                        //xml = xml + "<SystemUserGuid>" + item.SystemUserGuid + "</SystemUserGuid>";
                        //xml = xml + "</SystemUser>";

                        XmlElement xmlSystemUser = doc.CreateElement("SystemUser");
                        xmlSystemUsersRemoved.AppendChild(xmlSystemUser);

                        XmlElement xmlSystemUserName = doc.CreateElement("SystemUserName");
                        xmlSystemUserName.InnerText = systemUserName;
                        xmlSystemUser.AppendChild(xmlSystemUserName);

                        XmlElement xmlSystemUserGuid = doc.CreateElement("SystemUserGuid");
                        xmlSystemUserGuid.InnerText = item.SystemUserGuid;
                        xmlSystemUser.AppendChild(xmlSystemUserGuid);

                    }
                    root.AppendChild(xmlSystemUsersRemoved);
                    //xml = xml + "</SystemUsersRemoved>";
                }
            }
            //xml = xml + "</LocationSystemUsers>";
            if (changesExist)
            {
                var output = (from n in db.spDDAWizard_UpdateLocationSystemUsers_v1(
                    location.LocationId,
                    System.Xml.Linq.XElement.Parse(doc.OuterXml),
                    adminUserGuid)
                              select n).ToList();

                foreach (spDDAWizard_UpdateLocationSystemUsers_v1Result message in output)
                {
                    wizardMessages.AddMessage(message.MessageText.ToString(), (bool)message.Success);
                }
            }
            return wizardMessages;
        }

        //Add to DB
        public int AddLocationAddress(Location location, Address address)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            int? locationId = -1;
            HierarchyDC db2 = new HierarchyDC(Settings.getConnectionString());
            db2.spDesktopDataAdmin_InsertLocationAddress_v1(
                ref locationId, 
                location.LocationName,
                location.CountryRegionId,
                address.FirstAddressLine,
                address.SecondAddressLine,
                address.CityName,
                address.CountyName,
                address.StateProvinceCode,
                address.LatitudeDecimal,
                address.LongitudeDecimal,
                address.MappingQualityCode,
                address.PostalCode,
                address.ReplicatedFromClientMaintenanceFlag,
                address.CountryCode,
                adminUserGuid
            );
            return (int)locationId;
        }

        //Update Team
        public void UpdateLocationAddress(Location location, Address address)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];


            HierarchyDC db2 = new HierarchyDC(Settings.getConnectionString());
            db2.spDesktopDataAdmin_UpdateLocationAddress_v1(           
                location.LocationId,
                location.LocationName,
                location.CountryRegionId,
                location.VersionNumber,
                address.FirstAddressLine,
                address.SecondAddressLine,
                address.CityName,
                address.CountyName,
                address.StateProvinceCode,
                address.LatitudeDecimal,
                address.LongitudeDecimal,
                address.MappingQualityCode,
                address.PostalCode,
                address.ReplicatedFromClientMaintenanceFlag,
                address.CountryCode,
                adminUserGuid
            );
           
        }

        //Delete From DB
        public void DeleteLocationAddress(Location location)
        {
            HierarchyDC db2 = new HierarchyDC(Settings.getConnectionString());
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db2.spDesktopDataAdmin_DeleteLocation_v1(
                location.LocationId,
                adminUserGuid,  
                location.VersionNumber
            );
        }
    }
}