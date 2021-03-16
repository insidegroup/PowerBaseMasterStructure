using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace CWTDesktopDatabase.Repository
{
	public class ClientProfileAdminItemRepository
	{
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//Get a Page of Client Profile Groups
		public CWTPaginatedList<spDesktopDataAdmin_SelectClientProfileAdminItems_v1Result> PageClientProfileAdminItems(int clientProfileAdminGroupId, int page, string filter, string sortField, int sortOrder)
		{
			//get a page of records
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectClientProfileAdminItems_v1(clientProfileAdminGroupId, sortField, sortOrder, page).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}

			//put into page object
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectClientProfileAdminItems_v1Result>(result, page, totalRecords);

			//return to user
			return paginatedView;
		}

		//Get Client Detail Items
		public List<ClientProfileAdminItem> GetClientProfileAdminItemsUNUSED(int clientProfileAdminGroupId, int clientProfilePanelId)
		{
			return db.ClientProfileAdminItems.Where(
					c => c.ClientProfileAdminGroupId == clientProfileAdminGroupId &&
					c.ClientProfileDataElement.ClientProfilePanelId == clientProfilePanelId).ToList();
		}

        //Get Client Detail Items
        public List<ClientProfileAdminItemVM> GetClientProfilePanelClientProfileDataElementsX(int clientProfileAdminGroupId, int clientProfilePanelId)
        {

			var d = (from n in db.spDesktopDataAdmin_SelectClientProfilePanelClientProfileAdminItems_v1(clientProfileAdminGroupId, clientProfilePanelId)
                                       select new ClientProfileAdminItemRow
                                       {
                                           ClientProfileAdminItemId = n.ClientProfileAdminItemId,
                                           ClientProfileDataElementId = (int)n.ClientProfileDataElementId,
                                           ClientProfileDataElementName = n.ClientProfileDataElementName.ToString(),
                                           ClientProfileAdminGroupId = n.ClientProfileAdminGroupId,
                                           MandatoryFlag = n.MandatoryFlag,
                                           DefaultGDSCommandFormat = n.DefaultGDSCommandFormat,
                                           DefaultRemark = n.DefaultRemark,
                                           ToolTip = "n.ToolTip",
                                           ClientProfileMoveStatusId = n.ClientProfileMoveStatusId,
                                           ClientProfileAdminItemSequenceNumber = n.ClientProfileAdminItemSequenceNumber,
                                           VersionNumber = n.VersionNumber
                                       }).ToList();

            var x = new List<ClientProfileAdminItemVM>();
            return x;
        }

        //Get Client Detail Items
        public List<ClientProfileAdminItemVM> GetClientProfilePanelClientProfileDataElements(int clientProfileAdminGroupId, int clientProfilePanelId)
        {
            ClientProfileMoveStatusRepository clientProfileMoveStatusRepository = new ClientProfileMoveStatusRepository();
            List<ClientProfileAdminItemRow> clientProfileAdminItems = new List<ClientProfileAdminItemRow>();
            List<ClientProfileAdminItemVM> clientProfileAdminItemVMs = new List<ClientProfileAdminItemVM>();

			clientProfileAdminItems = (from n in db.spDesktopDataAdmin_SelectClientProfilePanelClientProfileAdminItems_v1(clientProfileAdminGroupId, clientProfilePanelId)
                                       select new ClientProfileAdminItemRow
                                       {
                                           ClientProfileAdminItemId = n.ClientProfileAdminItemId,
                                           ClientProfileDataElementId = (int)n.ClientProfileDataElementId,
                                           ClientProfileDataElementName = n.ClientProfileDataElementName,
                                           ClientProfileAdminGroupId = n.ClientProfileAdminGroupId,
                                           MandatoryFlag = n.MandatoryFlag,
                                           DefaultGDSCommandFormat = n.DefaultGDSCommandFormat,
                                           DefaultRemark = n.DefaultRemark,
                                           ToolTip = n.ToolTip,
                                           ClientProfileMoveStatusId = n.ClientProfileMoveStatusId,
                                           ClientProfileAdminItemSequenceNumber = n.ClientProfileAdminItemSequenceNumber,
                                           VersionNumber = n.VersionNumber,
										   Source = n.Source,
										   InheritedFlag = n.InheritedFlag
                                       }).ToList();

                    foreach(ClientProfileAdminItemRow r in clientProfileAdminItems){
                        ClientProfileAdminItemVM clientProfileAdminItemVM = new ClientProfileAdminItemVM();
                        clientProfileAdminItemVM.ClientProfileAdminItem = r;

                        SelectList clientProfileMoveStatuses = new SelectList(clientProfileMoveStatusRepository.GetAllClientProfileMoveStatuses().ToList(), "ClientProfileMoveStatusId", "ClientProfileMoveStatusCode", clientProfileAdminItemVM.ClientProfileAdminItem.ClientProfileMoveStatusId);
                        clientProfileAdminItemVM.ClientProfileMoveStatuses = clientProfileMoveStatuses;


                        clientProfileAdminItemVMs.Add(clientProfileAdminItemVM);

                    }
                    return clientProfileAdminItemVMs;
        }

        public XmlElement ClientProfileAdminItem(XmlDocument doc, ClientProfileAdminItemVM item)
        {
            XmlElement xmlClientProfileAdminItem = doc.CreateElement("ClientProfileAdminItem");

            XmlElement xmlClientProfileAdminItemId = doc.CreateElement("ClientProfileAdminItemId");
            xmlClientProfileAdminItemId.InnerText = item.ClientProfileAdminItem.ClientProfileAdminItemId.ToString();
            xmlClientProfileAdminItem.AppendChild(xmlClientProfileAdminItemId);

            XmlElement xmlClientProfileDataElementId = doc.CreateElement("ClientProfileDataElementId");
            xmlClientProfileDataElementId.InnerText = item.ClientProfileAdminItem.ClientProfileDataElementId.ToString();
            xmlClientProfileAdminItem.AppendChild(xmlClientProfileDataElementId);

           ///XmlElement xmlClientProfileAdminGroupId = doc.CreateElement("ClientProfileAdminGroupId");
            ///xmlClientProfileAdminGroupId.InnerText = item.ClientProfileAdminGroupId.ToString();
           /// xmlClientProfileAdminItem.AppendChild(xmlClientProfileAdminGroupId);

            XmlElement xmlClientProfileDataElementName = doc.CreateElement("ClientProfileDataElementName");
            if (item.ClientProfileAdminItem.ClientProfileDataElementName != null)
            {
                xmlClientProfileDataElementName.InnerText = item.ClientProfileAdminItem.ClientProfileDataElementName.ToString();
            }
            xmlClientProfileAdminItem.AppendChild(xmlClientProfileDataElementName);

            XmlElement xmlMandatoryFlag = doc.CreateElement("MandatoryFlag");
            xmlMandatoryFlag.InnerText = item.ClientProfileAdminItem.MandatoryFlag == true ? "1" : "0";
            xmlClientProfileAdminItem.AppendChild(xmlMandatoryFlag);

            XmlElement xmlDefaultGDSCommandFormat = doc.CreateElement("DefaultGDSCommandFormat");
            if (item.ClientProfileAdminItem.DefaultGDSCommandFormat != null)
            {
                xmlDefaultGDSCommandFormat.InnerText = item.ClientProfileAdminItem.DefaultGDSCommandFormat.ToString();
            } 
            xmlClientProfileAdminItem.AppendChild(xmlDefaultGDSCommandFormat);

            XmlElement xmlDefaultRemark = doc.CreateElement("DefaultRemark");
            if (item.ClientProfileAdminItem.DefaultRemark != null)
            {
                xmlDefaultRemark.InnerText = item.ClientProfileAdminItem.DefaultRemark.ToString();
            } 
            xmlClientProfileAdminItem.AppendChild(xmlDefaultRemark);

            XmlElement xmlToolTip = doc.CreateElement("ToolTip");
            if (item.ClientProfileAdminItem.ToolTip != null)
            {
                xmlToolTip.InnerText = item.ClientProfileAdminItem.ToolTip.ToString();
            } 
            xmlClientProfileAdminItem.AppendChild(xmlToolTip);

            XmlElement xmlClientProfileMoveStatusId = doc.CreateElement("ClientProfileMoveStatusId");
            xmlClientProfileMoveStatusId.InnerText = item.ClientProfileAdminItem.ClientProfileMoveStatusId.ToString();
            xmlClientProfileAdminItem.AppendChild(xmlClientProfileMoveStatusId);

            XmlElement xmlVersionNumber = doc.CreateElement("VersionNumber");
            xmlVersionNumber.InnerText = item.ClientProfileAdminItem.VersionNumber.ToString();
            xmlClientProfileAdminItem.AppendChild(xmlVersionNumber);

            return xmlClientProfileAdminItem;
        }
        
        public void UpdateClientProfileAdminItems(ClientProfileAdminItemsVM clientProfileAdminItemsVM)
        {
            // Create the xml document container
            XmlDocument doc = new XmlDocument();// Create the XML Declaration, and append it to XML document
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(dec);
            XmlElement root = doc.CreateElement("ClientProfileAdminItems");
            doc.AppendChild(root);
            
            #region ClientDetails
            if (clientProfileAdminItemsVM.ClientProfileAdminItemsClientDetails != null)
            {
                if (clientProfileAdminItemsVM.ClientProfileAdminItemsClientDetails.Count > 0)
                {
                    foreach (ClientProfileAdminItemVM item in clientProfileAdminItemsVM.ClientProfileAdminItemsClientDetails)
                    {
                        XmlElement xmlClientProfileAdminItem = ClientProfileAdminItem(doc, item);
                        root.AppendChild(xmlClientProfileAdminItem);
                    }
                }
            }
            if (clientProfileAdminItemsVM.ClientProfileAdminItemsMidOffice != null)
            {
                if (clientProfileAdminItemsVM.ClientProfileAdminItemsMidOffice.Count > 0)
                {
                    foreach (ClientProfileAdminItemVM item in clientProfileAdminItemsVM.ClientProfileAdminItemsMidOffice)
                    {
                        XmlElement xmlClientProfileAdminItem = ClientProfileAdminItem(doc, item);
                        root.AppendChild(xmlClientProfileAdminItem);
                    }
                }
            }
            if (clientProfileAdminItemsVM.ClientProfileAdminItemsBackOffice != null)
            {
                if (clientProfileAdminItemsVM.ClientProfileAdminItemsBackOffice.Count > 0)
                {
                    foreach (ClientProfileAdminItemVM item in clientProfileAdminItemsVM.ClientProfileAdminItemsBackOffice)
                    {
                        XmlElement xmlClientProfileAdminItem = ClientProfileAdminItem(doc, item);
                        root.AppendChild(xmlClientProfileAdminItem);
                    }
                }
            }
            if (clientProfileAdminItemsVM.ClientProfileAdminItemsItinerary != null)
            {
                if (clientProfileAdminItemsVM.ClientProfileAdminItemsItinerary.Count > 0)
                {
                    foreach (ClientProfileAdminItemVM item in clientProfileAdminItemsVM.ClientProfileAdminItemsItinerary)
                    {
                        XmlElement xmlClientProfileAdminItem = ClientProfileAdminItem(doc, item);
                        root.AppendChild(xmlClientProfileAdminItem);
                    }
                }
            }
            if (clientProfileAdminItemsVM.ClientProfileAdminItems24Hours != null)
            {
                if (clientProfileAdminItemsVM.ClientProfileAdminItems24Hours.Count > 0)
                {
                    foreach (ClientProfileAdminItemVM item in clientProfileAdminItemsVM.ClientProfileAdminItems24Hours)
                    {
                        XmlElement xmlClientProfileAdminItem = ClientProfileAdminItem(doc, item);
                        root.AppendChild(xmlClientProfileAdminItem);
                    }
                }
            }
            if (clientProfileAdminItemsVM.ClientProfileAdminItemsAirRailPolicy != null)
            {
                if (clientProfileAdminItemsVM.ClientProfileAdminItemsAirRailPolicy.Count > 0)
                {
                    foreach (ClientProfileAdminItemVM item in clientProfileAdminItemsVM.ClientProfileAdminItemsAirRailPolicy)
                    {
                        XmlElement xmlClientProfileAdminItem = ClientProfileAdminItem(doc, item);
                        root.AppendChild(xmlClientProfileAdminItem);
                    }
                }
            }
			if (clientProfileAdminItemsVM.ClientProfileAdminItemsAmadeusTPM != null)
			{
				if (clientProfileAdminItemsVM.ClientProfileAdminItemsAmadeusTPM.Count > 0)
				{
					foreach (ClientProfileAdminItemVM item in clientProfileAdminItemsVM.ClientProfileAdminItemsAmadeusTPM)
					{
						XmlElement xmlClientProfileAdminItem = ClientProfileAdminItem(doc, item);
						root.AppendChild(xmlClientProfileAdminItem);
					}
				}
			}
            #endregion
            
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateClientProfileAdminItems_v1(
                clientProfileAdminItemsVM.ClientProfileAdminGroupId,
                System.Xml.Linq.XElement.Parse(doc.OuterXml),
                adminUserGuid);
        }
	}
}