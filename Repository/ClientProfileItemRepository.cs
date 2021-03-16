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
	public class ClientProfileItemRepository
	{
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//Get Client Detail Items
        public List<ClientProfileItemVM> GetClientProfilePanelClientProfileDataElements(int clientProfileGroupId, int clientProfilePanelId)
        {
            ClientProfileMoveStatusRepository clientProfileMoveStatusRepository = new ClientProfileMoveStatusRepository();
            List<ClientProfileItemRow> clientProfileItems = new List<ClientProfileItemRow>();
            List<ClientProfileItemVM> clientProfileItemVMs = new List<ClientProfileItemVM>();

			clientProfileItems = (from n in db.spDesktopDataAdmin_SelectClientProfilePanelClientProfileItems_v1(clientProfileGroupId, clientProfilePanelId)
								  select new ClientProfileItemRow
								  {

									  ClientProfileItemId = n.ClientProfileItemId,
									  ClientProfileItemSequenceNumber = n.ClientProfileAdminItemSequenceNumber,
									  ClientProfileDataElementId = (int)n.ClientProfileDataElementId,
									  ClientProfileDataElementName = n.ClientProfileDataElementName,
									  ClientProfileGroupId = n.ClientProfileGroupId,
									  ClientProfileMoveStatusId = n.ClientProfileMoveStatusId,
									  ClientProfileAdminItemId = n.ClientProfileAdminItemId,

									  GDSCommandFormat = n.GDSCommandFormat,
									  MandatoryFlag = n.MandatoryFlag,
									  Remark = n.Remark,
									  SourceItem = n.SourceItem,
									  SourceName = n.SourceName,
									  ToolTip = n.ToolTip,
									  VersionNumber = n.VersionNumber,
									  
									  InheritedGDSCommandFormat = n.InheritedGDSCommandFormat,
									  InheritedMoveStatusFlag = n.InheritedMoveStatusFlag,
									  InheritedRemark = n.InheritedRemark,
									  InheritedFlag = n.InheritedFlag

								  }).ToList();

                    foreach(ClientProfileItemRow r in clientProfileItems){
                        ClientProfileItemVM clientProfileItemVM = new ClientProfileItemVM();
                        clientProfileItemVM.ClientProfileItem = r;

                        SelectList clientProfileMoveStatuses = new SelectList(clientProfileMoveStatusRepository.GetAllClientProfileMoveStatuses().ToList(), "ClientProfileMoveStatusId", "ClientProfileMoveStatusCode", clientProfileItemVM.ClientProfileItem.ClientProfileMoveStatusId);
                        clientProfileItemVM.ClientProfileMoveStatuses = clientProfileMoveStatuses;

                        clientProfileItemVMs.Add(clientProfileItemVM);

                    }
                    return clientProfileItemVMs;
        }

        public XmlElement ClientProfileItem(XmlDocument doc, ClientProfileItemVM item)
        {
            XmlElement xmlClientProfileItem = doc.CreateElement("ClientProfileItem");

			XmlElement xmlClientProfileItemId = doc.CreateElement("ClientProfileItemId");
			xmlClientProfileItemId.InnerText = item.ClientProfileItem.ClientProfileItemId.ToString();
			xmlClientProfileItem.AppendChild(xmlClientProfileItemId);

			XmlElement xmlClientProfileDataElementName = doc.CreateElement("ClientProfileDataElementName");
			if (item.ClientProfileItem.ClientProfileDataElementName != null)
			{
				xmlClientProfileDataElementName.InnerText = item.ClientProfileItem.ClientProfileDataElementName.ToString();
			}
			xmlClientProfileItem.AppendChild(xmlClientProfileDataElementName);

			XmlElement xmlClientProfileGroupId = doc.CreateElement("ClientProfileGroupId");
			xmlClientProfileGroupId.InnerText = item.ClientProfileItem.ClientProfileGroupId.ToString();
			xmlClientProfileItem.AppendChild(xmlClientProfileGroupId);

			XmlElement xmlClientProfileAdminItemId = doc.CreateElement("ClientProfileAdminItemId");
			xmlClientProfileAdminItemId.InnerText = item.ClientProfileItem.ClientProfileAdminItemId.ToString();
			xmlClientProfileItem.AppendChild(xmlClientProfileAdminItemId);

			XmlElement xmlClientProfileDataElementId = doc.CreateElement("ClientProfileDataElementId");
			xmlClientProfileDataElementId.InnerText = item.ClientProfileItem.ClientProfileDataElementId.ToString();
			xmlClientProfileItem.AppendChild(xmlClientProfileDataElementId);
			
			XmlElement xmlGDSCommandFormat = doc.CreateElement("GDSCommandFormat");
			if (item.ClientProfileItem.GDSCommandFormat != null)
            {
				xmlGDSCommandFormat.InnerText = item.ClientProfileItem.GDSCommandFormat.ToString();
            } 
            xmlClientProfileItem.AppendChild(xmlGDSCommandFormat);

            XmlElement xmlRemark = doc.CreateElement("Remark");
            if (item.ClientProfileItem.Remark != null)
            {
                xmlRemark.InnerText = item.ClientProfileItem.Remark.ToString();
            } 
            xmlClientProfileItem.AppendChild(xmlRemark);

            XmlElement xmlClientProfileMoveStatusId = doc.CreateElement("ClientProfileMoveStatusId");
            xmlClientProfileMoveStatusId.InnerText = item.ClientProfileItem.ClientProfileMoveStatusId.ToString();
            xmlClientProfileItem.AppendChild(xmlClientProfileMoveStatusId);

            XmlElement xmlVersionNumber = doc.CreateElement("VersionNumber");
            xmlVersionNumber.InnerText = item.ClientProfileItem.VersionNumber.ToString();
            xmlClientProfileItem.AppendChild(xmlVersionNumber);

            return xmlClientProfileItem;
        }
        
        public void UpdateClientProfileItems(ClientProfileItemsVM clientProfileItemsVM)
        {
            // Create the xml document container
            XmlDocument doc = new XmlDocument();// Create the XML Declaration, and append it to XML document
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(dec);
            XmlElement root = doc.CreateElement("ClientProfileItems");
            doc.AppendChild(root);
            
            ValidationRepository validationRepository = new ValidationRepository();

            #region ClientDetails
			if (clientProfileItemsVM.ClientProfileItemsClientDetails != null)
			{
				if (clientProfileItemsVM.ClientProfileItemsClientDetails.Count > 0)
				{
                    
					foreach (ClientProfileItemVM item in clientProfileItemsVM.ClientProfileItemsClientDetails)
					{
                        string validationError = validationRepository.IsValidClientProfileRowItem(item.ClientProfileItem);
                        if(validationError != "OK"){
                            throw new CustomValidationException(validationError);
                        }
						XmlElement xmlClientProfileItem = ClientProfileItem(doc, item);
						root.AppendChild(xmlClientProfileItem);
					}
				}
			}
            if (clientProfileItemsVM.ClientProfileItemsMidOffice != null)
            {
                if (clientProfileItemsVM.ClientProfileItemsMidOffice.Count > 0)
                {
                    foreach (ClientProfileItemVM item in clientProfileItemsVM.ClientProfileItemsMidOffice)
                    {
                        string validationError = validationRepository.IsValidClientProfileRowItem(item.ClientProfileItem);
                        if (validationError != "OK")
                        {
                            throw new CustomValidationException(validationError);
                        }
                        XmlElement xmlClientProfileItem = ClientProfileItem(doc, item);
                        root.AppendChild(xmlClientProfileItem);
                    }
                }
            }
            if (clientProfileItemsVM.ClientProfileItemsBackOffice != null)
            {
                if (clientProfileItemsVM.ClientProfileItemsBackOffice.Count > 0)
                {
                    foreach (ClientProfileItemVM item in clientProfileItemsVM.ClientProfileItemsBackOffice)
                    {
                        string validationError = validationRepository.IsValidClientProfileRowItem(item.ClientProfileItem);
                        if (validationError != "OK")
                        {
                            throw new CustomValidationException(validationError);
                        }
                        XmlElement xmlClientProfileItem = ClientProfileItem(doc, item);
                        root.AppendChild(xmlClientProfileItem);
                    }
                }
            }
            if (clientProfileItemsVM.ClientProfileItemsItinerary != null)
            {
                if (clientProfileItemsVM.ClientProfileItemsItinerary.Count > 0)
                {
                    foreach (ClientProfileItemVM item in clientProfileItemsVM.ClientProfileItemsItinerary)
                    {
                        string validationError = validationRepository.IsValidClientProfileRowItem(item.ClientProfileItem);
                        if (validationError != "OK")
                        {
                            throw new CustomValidationException(validationError);
                        }
                        XmlElement xmlClientProfileItem = ClientProfileItem(doc, item);
                        root.AppendChild(xmlClientProfileItem);
                    }
                }
            }
            if (clientProfileItemsVM.ClientProfileItems24Hours != null)
            {
                if (clientProfileItemsVM.ClientProfileItems24Hours.Count > 0)
                {
                    foreach (ClientProfileItemVM item in clientProfileItemsVM.ClientProfileItems24Hours)
                    {
                        string validationError = validationRepository.IsValidClientProfileRowItem(item.ClientProfileItem);
                        if (validationError != "OK")
                        {
                            throw new CustomValidationException(validationError);
                        }
                        XmlElement xmlClientProfileItem = ClientProfileItem(doc, item);
                        root.AppendChild(xmlClientProfileItem);
                    }
                }
            }
            if (clientProfileItemsVM.ClientProfileItemsAirRailPolicy != null)
            {
                if (clientProfileItemsVM.ClientProfileItemsAirRailPolicy.Count > 0)
                {
                    foreach (ClientProfileItemVM item in clientProfileItemsVM.ClientProfileItemsAirRailPolicy)
                    {
                        string validationError = validationRepository.IsValidClientProfileRowItem(item.ClientProfileItem);
                        if (validationError != "OK")
                        {
                            throw new CustomValidationException(validationError);
                        }
                        XmlElement xmlClientProfileItem = ClientProfileItem(doc, item);
                        root.AppendChild(xmlClientProfileItem);
                    }
                }
            }
			if (clientProfileItemsVM.ClientProfileItemsAmadeusTPM != null)
			{
				if (clientProfileItemsVM.ClientProfileItemsAmadeusTPM.Count > 0)
				{
					foreach (ClientProfileItemVM item in clientProfileItemsVM.ClientProfileItemsAmadeusTPM)
					{
						string validationError = validationRepository.IsValidClientProfileRowItem(item.ClientProfileItem);
						if (validationError != "OK")
						{
							throw new CustomValidationException(validationError);
						}
						XmlElement xmlClientProfileItem = ClientProfileItem(doc, item);
						root.AppendChild(xmlClientProfileItem);
					}
				}
			}
            #endregion

            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateClientProfileItems_v1(
                clientProfileItemsVM.ClientProfileGroupId,
                System.Xml.Linq.XElement.Parse(doc.OuterXml),
                adminUserGuid);
			
        }
	}
}