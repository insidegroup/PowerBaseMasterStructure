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
	public class ClientProfileAdminItemsVM : CWTBaseViewModel
	{
        public int ClientProfileAdminGroupId { get; set; }
        public string ClientProfileAdminGroupClientProfileGroupName { get; set; }
		public string ClientProfileAdminGroupGDSName { get; set; }
		public string ClientProfileAdminGroupGDSCode { get; set; }
        public string ClientProfileAdminGroupHierarchyItem { get; set; }
        public string ClientProfileAdminGroupBackOfficeSystemDescription { get; set; }
        public int ClientProfilePanelId { get; set; } //this is the tab id rather that the PanelId from database

		public List<ClientProfileAdminItemVM> ClientProfileAdminItemsClientDetails { get; set; }
        public List<ClientProfileAdminItemVM> ClientProfileAdminItemsMidOffice { get; set; }
        public List<ClientProfileAdminItemVM> ClientProfileAdminItemsBackOffice { get; set; }
        public List<ClientProfileAdminItemVM> ClientProfileAdminItemsItinerary { get; set; }
        public List<ClientProfileAdminItemVM> ClientProfileAdminItems24Hours { get; set; }
        public List<ClientProfileAdminItemVM> ClientProfileAdminItemsAirRailPolicy { get; set; }
		public List<ClientProfileAdminItemVM> ClientProfileAdminItemsAmadeusTPM { get; set; }
		
		public bool HasDomainWriteAccess { get; set; }
 
        public ClientProfileAdminItemsVM()
        {
            HasDomainWriteAccess = false;
        }

		public ClientProfileAdminItemsVM(
                int clientProfileAdminGroupId,
                string clientProfileAdminGroupClientProfileGroupName,
				string clientProfileAdminGroupGDSName,
				string clientProfileAdminGroupGDSCode,
                string clientProfileAdminGroupHierarchyItem,
                string clientProfileAdminGroupBackOfficeSystemDescription,
                int clientProfilePanelId,
                List<ClientProfileAdminItemVM> clientProfileAdminItemsClientDetails,
                List<ClientProfileAdminItemVM> clientProfileAdminItemsMidOffice,
                List<ClientProfileAdminItemVM> clientProfileAdminItemsBackOffice,
                List<ClientProfileAdminItemVM> clientProfileAdminItemsItinerary,
                List<ClientProfileAdminItemVM> clientProfileAdminItems24Hours,
                List<ClientProfileAdminItemVM> clientProfileAdminItemsAirRailPolicy,
				List<ClientProfileAdminItemVM> clientProfileAdminItemsAmadeusTPM,
				bool hasDomainWriteAccess)
        {
            ClientProfileAdminGroupId = clientProfileAdminGroupId;
            ClientProfileAdminGroupClientProfileGroupName = clientProfileAdminGroupClientProfileGroupName;
            ClientProfileAdminGroupGDSName = clientProfileAdminGroupGDSName;
			ClientProfileAdminGroupGDSCode = clientProfileAdminGroupGDSCode;
            ClientProfileAdminGroupHierarchyItem = clientProfileAdminGroupHierarchyItem;
            ClientProfileAdminGroupBackOfficeSystemDescription = clientProfileAdminGroupBackOfficeSystemDescription;
            ClientProfilePanelId = clientProfilePanelId;
			ClientProfileAdminItemsClientDetails = clientProfileAdminItemsClientDetails;
			ClientProfileAdminItemsMidOffice = clientProfileAdminItemsMidOffice;
			ClientProfileAdminItemsBackOffice = clientProfileAdminItemsBackOffice;
			ClientProfileAdminItemsItinerary = clientProfileAdminItemsItinerary;
			ClientProfileAdminItems24Hours = clientProfileAdminItems24Hours;
			ClientProfileAdminItemsAirRailPolicy = clientProfileAdminItemsAirRailPolicy;
			ClientProfileAdminItemsAmadeusTPM = clientProfileAdminItemsAmadeusTPM;
			HasDomainWriteAccess = hasDomainWriteAccess;
        }
    }
}