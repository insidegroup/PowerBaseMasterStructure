using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Xml.Serialization;

namespace CWTDesktopDatabase.ViewModels
{
	public class ClientProfileItemsVM : CWTBaseViewModel
	{
        public int ClientProfileGroupId { get; set; }
        public string ClientProfileGroupClientProfileGroupName { get; set; }
        public string ClientProfileGroupGDSName { get; set; }
        public string ClientProfileGroupHierarchyItem { get; set; }
        public string ClientProfileGroupBackOfficeSystemDescription { get; set; }
        public int ClientProfilePanelId { get; set; } //this is the tab id rather that the PanelId from database

		public ClientProfileGroup ClientProfileGroup { get; set; }
		
		//Removed US1976
		//public List<ClientProfileItemVM> ClientProfileItemsGeneral { get; set; }
        //public List<ClientProfileItemVM> ClientProfileItemsLandPolicy { get; set; }
		public List<ClientProfileItemVM> ClientProfileItemsClientDetails { get; set; }
		public List<ClientProfileItemVM> ClientProfileItemsMidOffice { get; set; }
        public List<ClientProfileItemVM> ClientProfileItemsBackOffice { get; set; }
        public List<ClientProfileItemVM> ClientProfileItemsItinerary { get; set; }
        public List<ClientProfileItemVM> ClientProfileItems24Hours { get; set; }
        public List<ClientProfileItemVM> ClientProfileItemsAirRailPolicy { get; set; }
		public List<ClientProfileItemVM> ClientProfileItemsAmadeusTPM { get; set; }

		public bool HasDomainWriteAccess { get; set; }
 
        public ClientProfileItemsVM()
        {
            HasDomainWriteAccess = false;
        }

		public ClientProfileItemsVM(
                int clientProfileGroupId,
                string clientProfileGroupClientProfileGroupName,
                string clientProfileGroupGDSName,
                string clientProfileGroupHierarchyItem,
                string clientProfileGroupBackOfficeSystemDescription,
                int clientProfilePanelId,
				ClientProfileGroup clientProfileGroup,
                List<ClientProfileItemVM> clientProfileItemsClientDetails,
                List<ClientProfileItemVM> clientProfileItemsMidOffice,
                List<ClientProfileItemVM> clientProfileItemsBackOffice,
                List<ClientProfileItemVM> clientProfileItemsItinerary,
                List<ClientProfileItemVM> clientProfileItems24Hours,
                List<ClientProfileItemVM> clientProfileItemsAirRailPolicy,
				List<ClientProfileItemVM> ClientProfileItemsAmadeusTPM,
				bool hasDomainWriteAccess)
        {
            ClientProfileGroupId = clientProfileGroupId;
            ClientProfileGroupClientProfileGroupName = clientProfileGroupClientProfileGroupName;
            ClientProfileGroupGDSName = clientProfileGroupGDSName;
            ClientProfileGroupHierarchyItem = clientProfileGroupHierarchyItem;
            ClientProfileGroupBackOfficeSystemDescription = clientProfileGroupBackOfficeSystemDescription;
            ClientProfilePanelId = clientProfilePanelId;
			ClientProfileGroup = clientProfileGroup;
			ClientProfileItemsClientDetails = clientProfileItemsClientDetails;
			ClientProfileItemsMidOffice = clientProfileItemsMidOffice;
			ClientProfileItemsBackOffice = clientProfileItemsBackOffice;
			ClientProfileItemsItinerary = clientProfileItemsItinerary;
			ClientProfileItems24Hours = clientProfileItems24Hours;
			ClientProfileItemsAirRailPolicy = clientProfileItemsAirRailPolicy;
            HasDomainWriteAccess = hasDomainWriteAccess;
        }
    }
}