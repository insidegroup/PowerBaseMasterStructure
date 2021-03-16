using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using System.Linq.Dynamic;
using System.Web.Security;
using CWTDesktopDatabase.Helpers;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using CWTDesktopDatabase.ViewModels;
using System.Text;
using Persits.PDF;
using System.IO;
using System.Configuration;

namespace CWTDesktopDatabase.Repository
{
	public class GDSOrderRepository
    {
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//Get a Page of Client Telephonies - for Page Listings
		public CWTPaginatedList<spDesktopDataAdmin_SelectGDSOrders_v1Result> PageGDSOrders(
			int page, 
			string filter, 
			string sortField,
			int sortOrder,
			int? gdsOrderId,
			int? gdsOrderStatusId,
			string ticketNumber,
			string pseudoCityOrOfficeId,
			int? gdsOrderTypeId,
			DateTime? gdsOrderDateTimeStart,
			DateTime? gdsOrderDateTimeEnd,
			string orderAnalyst,
			string internalSiteName,
			string gdsCode
			)
		{

			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			
			//get a page of records
			var result = db.spDesktopDataAdmin_SelectGDSOrders_v1(
				filter, 
				sortField, 
				sortOrder,
				page,
				gdsOrderId,
				gdsOrderStatusId,
				ticketNumber,
				pseudoCityOrOfficeId,
				gdsOrderTypeId,
				gdsOrderDateTimeStart,
				gdsOrderDateTimeEnd,
				orderAnalyst,
				internalSiteName,
				gdsCode,
				adminUserGuid
			).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}

			//put into page object
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectGDSOrders_v1Result>(result, page, totalRecords);

			//return to user
			return paginatedView;
		}

		//Get one GDSOrder
		public GDSOrder GetGDSOrder(int id)
		{
			return db.GDSOrders.SingleOrDefault(c => c.GDSOrderId == id);
		}

		//Get last GDSOrder for an Admin User
		public GDSOrder GetGDSOrderByUser(string userIdentifier)
		{
			return db.GDSOrders
				.Where(c => c.CreationUserIdentifier == userIdentifier || c.LastUpdateUserIdentifier == userIdentifier)
				.OrderByDescending(x => x.LastUpdateTimestamp)
				.ThenByDescending(x => x.CreationTimestamp)
				.FirstOrDefault();
		}

        //Add Data From Linked Tables for Display
        public void EditForDisplay(GDSOrder gdsOrder)
        {
			gdsOrder.ExpediteFlagNullable = (gdsOrder.ExpediteFlag == true);

			//PseudoCityOrOfficeId
			if(gdsOrder.PseudoCityOrOfficeMaintenance != null) {
				gdsOrder.PseudoCityOrOfficeId = gdsOrder.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeId;
			}

			//PseudoCityOrOfficeAddress
			if (gdsOrder.PseudoCityOrOfficeMaintenance != null && gdsOrder.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeAddress != null)
			{

                PseudoCityOrOfficeAddressRepository pseudoCityOrOfficeAddressRepository = new PseudoCityOrOfficeAddressRepository();
                pseudoCityOrOfficeAddressRepository.EditForDisplay(gdsOrder.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeAddress);

                gdsOrder.PseudoCityOrOfficeAddress = gdsOrder.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeAddress.FirstAddressLine;
			}

			//OrderAnalystCountry
			CountryRepository countryRepository = new CountryRepository();
			if (gdsOrder.OrderAnalystCountryCode != null)
			{
				Country country = countryRepository.GetCountry(gdsOrder.OrderAnalystCountryCode);
				if(country != null) {
					gdsOrder.OrderAnalystCountry = country;
				}
			}

			//RequesterCountry
			if (gdsOrder.RequesterCountryCode != null)
			{
				Country country = countryRepository.GetCountry(gdsOrder.RequesterCountryCode);
				if(country != null) {
					gdsOrder.RequesterCountry = country;
				}
			}

			//GDSOrderLineItems
			if (gdsOrder.GDSOrderLineItems != null && gdsOrder.GDSOrderLineItems.Count > 0)
			{

				foreach (GDSOrderLineItem item in gdsOrder.GDSOrderLineItems)
				{

					GDSOrderLineItemActionRepository gdsOrderLineItemActionRepository = new GDSOrderLineItemActionRepository();
					item.GDSOrderLineItemActions = new SelectList(
						gdsOrderLineItemActionRepository.GetAllGDSOrderLineItemActions().ToList(),
						"GDSOrderLineItemActionId",
						"GDSOrderLineItemActionName",
						item.GDSOrderLineItemActionId
					);

					GDSOrderDetailRepository gdsOrderDetailRepository = new GDSOrderDetailRepository();
					item.GDSOrderDetails = new SelectList(
						gdsOrderDetailRepository.GetGDSOrderDetailsByGDSCode(gdsOrder.GDSCode).ToList(),
						"GDSOrderDetailId",
						"GDSOrderDetailName",
						item.GDSOrderDetailId
					);
				}
			}

			//Email
			gdsOrder.EmailFromAddress = ConfigurationManager.AppSettings["GDSOrder_FromEmailAddress"].ToString();

		}

		//GDSOrderLineItems to XML
		public XElement GetGDSOrderLineItemsXML(GDSOrder gdsOrder)
		{
			XmlDocument doc = new XmlDocument();
			XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
			doc.AppendChild(dec);
			XmlElement root = doc.CreateElement("GDSOrderLineItems");
			doc.AppendChild(root);

			if (gdsOrder.GDSOrderLineItemsXML != null)
			{
				foreach (GDSOrderLineItem GDSOrderLineItem in gdsOrder.GDSOrderLineItemsXML)
				{
					XmlElement xmlGDSOrderLineItem = doc.CreateElement("GDSOrderLineItem");

					if (GDSOrderLineItem.GDSOrderLineItemActionId > 0 && GDSOrderLineItem.GDSOrderDetailId > 0)
					{
						//Quantity
						if (GDSOrderLineItem.Quantity > 0)
						{
							XmlElement xmlQuantity = doc.CreateElement("Quantity");
							xmlQuantity.InnerText = GDSOrderLineItem.Quantity.ToString();
							xmlGDSOrderLineItem.AppendChild(xmlQuantity);
						}

						//GDSOrderLineItemActionId
						if (GDSOrderLineItem.GDSOrderLineItemActionId > 0)
						{
							XmlElement xmlGDSOrderLineItemActionId = doc.CreateElement("GDSOrderLineItemActionId");
							xmlGDSOrderLineItemActionId.InnerText = GDSOrderLineItem.GDSOrderLineItemActionId.ToString();
							xmlGDSOrderLineItem.AppendChild(xmlGDSOrderLineItemActionId);
						}

						//GDSOrderDetailId
						if (GDSOrderLineItem.GDSOrderDetailId > 0)
						{
							XmlElement xmlGDSOrderDetailId = doc.CreateElement("GDSOrderDetailId");
							xmlGDSOrderDetailId.InnerText = GDSOrderLineItem.GDSOrderDetailId.ToString();
							xmlGDSOrderLineItem.AppendChild(xmlGDSOrderDetailId);
						}

						//Comment
						if (!string.IsNullOrEmpty(GDSOrderLineItem.Comment))
						{
							XmlElement xmlComment = doc.CreateElement("Comment");
							xmlComment.InnerText = GDSOrderLineItem.Comment.ToString();
							xmlGDSOrderLineItem.AppendChild(xmlComment);
						}

						root.AppendChild(xmlGDSOrderLineItem);
					}
				}
			}

			return System.Xml.Linq.XElement.Parse(doc.OuterXml);
		}

		//GDSOrderThirdPartyVendors to XML
		public XElement GetGDSOrderThirdPartyVendorsXML(GDSOrder gdsOrder)
		{
			XmlDocument doc = new XmlDocument();
			XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
			doc.AppendChild(dec);
			XmlElement root = doc.CreateElement("GDSOrderThirdPartyVendors");
			doc.AppendChild(root);

			if (gdsOrder.GDSThirdPartyVendors != null)
			{
				foreach (GDSThirdPartyVendor gdsThirdPartyVendor in gdsOrder.GDSThirdPartyVendors)
				{
					XmlElement xmlGDSThirdPartyVendor = doc.CreateElement("GDSOrderThirdPartyVendor");

					XmlElement xmlGDSThirdPartyVendorId = doc.CreateElement("GDSOrderThirdPartyVendorId");
					xmlGDSThirdPartyVendorId.InnerText = gdsThirdPartyVendor.GDSThirdPartyVendorId.ToString();
					xmlGDSThirdPartyVendor.AppendChild(xmlGDSThirdPartyVendorId);

					root.AppendChild(xmlGDSThirdPartyVendor);
				}
			}

			return System.Xml.Linq.XElement.Parse(doc.OuterXml);
		}

		//PseudoCityOrOfficeMaintenanceGDSOrderThirdPartyVendors to XML
		public XElement GetPseudoCityOrOfficeMaintenanceGDSOrderThirdPartyVendorsXML(GDSOrderVM gdsOrderVM)
		{
			XmlDocument doc = new XmlDocument();
			XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
			doc.AppendChild(dec);
			XmlElement root = doc.CreateElement("PseudoCityOrOfficeMaintenanceGDSOrderThirdPartyVendors");
			doc.AppendChild(root);

			if (gdsOrderVM.PseudoCityOrOfficeMaintenanceGDSThirdPartyVendors != null)
			{
				foreach (GDSThirdPartyVendor gdsThirdPartyVendor in gdsOrderVM.PseudoCityOrOfficeMaintenanceGDSThirdPartyVendors)
				{
					XmlElement xmlGDSThirdPartyVendor = doc.CreateElement("PseudoCityOrOfficeMaintenanceGDSOrderThirdPartyVendor");

					XmlElement xmlGDSThirdPartyVendorId = doc.CreateElement("PseudoCityOrOfficeMaintenanceGDSOrderThirdPartyVendorId");
					xmlGDSThirdPartyVendorId.InnerText = gdsThirdPartyVendor.GDSThirdPartyVendorId.ToString();
					xmlGDSThirdPartyVendor.AppendChild(xmlGDSThirdPartyVendorId);

					root.AppendChild(xmlGDSThirdPartyVendor);
				}
			}

			return System.Xml.Linq.XElement.Parse(doc.OuterXml);
		}

		//Add to DB
		public void Add(GDSOrderVM gdsOrderVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			XElement gdsOrderThirdPartyVendors = GetGDSOrderThirdPartyVendorsXML(gdsOrderVM.GDSOrder);
			XElement gdsOrderLineItems = GetGDSOrderLineItemsXML(gdsOrderVM.GDSOrder); 
			XElement pseudoCityOrOfficeMaintenanceGDSThirdPartyVendors = GetPseudoCityOrOfficeMaintenanceGDSOrderThirdPartyVendorsXML(gdsOrderVM);

			db.spDesktopDataAdmin_InsertGDSOrder_v1(
				
				//GDS Order
				gdsOrderVM.GDSOrder.GDSCode,
				gdsOrderVM.GDSOrder.GDSOrderDateTime,
				gdsOrderVM.GDSOrder.GDSOrderTypeId,
				gdsOrderVM.GDSOrder.GDSOrderStatusId,
				gdsOrderVM.GDSOrder.PseudoCityOrOfficeMaintenanceId,
				gdsOrderVM.GDSOrder.OrderAnalystName,
				gdsOrderVM.GDSOrder.OrderAnalystEmail,
				gdsOrderVM.GDSOrder.OrderAnalystPhone,
				gdsOrderVM.GDSOrder.OrderAnalystCountryCode,
				gdsOrderVM.GDSOrder.TicketNumber,
				gdsOrderVM.GDSOrder.ExpediteFlagNullable,
				gdsOrderVM.GDSOrder.CWTCostCenterNumber,
				gdsOrderVM.GDSOrder.RequesterFirstName,
				gdsOrderVM.GDSOrder.RequesterLastName,
				gdsOrderVM.GDSOrder.RequesterEmail,
				gdsOrderVM.GDSOrder.RequesterPhone,
				gdsOrderVM.GDSOrder.RequesterCountryCode,
				gdsOrderVM.GDSOrder.RequesterUID,
				gdsOrderVM.GDSOrder.ExternalRemarks,
				gdsOrderVM.GDSOrder.DeactivationDateTime,
				gdsOrderThirdPartyVendors,
				
				//GDS Order Details
				gdsOrderLineItems,

				//PseudoCityOrOfficeMaintenance
				gdsOrderVM.GDSOrder.PseudoCityOrOfficeId,
				gdsOrderVM.GDSOrder.PseudoCityOrOfficeMaintenance_GDSCode,
				gdsOrderVM.GDSOrder.PseudoCityOrOfficeMaintenance_IATAId,
				gdsOrderVM.GDSOrder.PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeAddressId,
				gdsOrderVM.GDSOrder.PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeDefinedRegionId,
				gdsOrderVM.GDSOrder.PseudoCityOrOfficeMaintenance_LocationContactName,
				gdsOrderVM.GDSOrder.PseudoCityOrOfficeMaintenance_LocationPhone,
				gdsOrderVM.GDSOrder.PseudoCityOrOfficeMaintenance_ActiveFlagNonNullable,
				gdsOrderVM.GDSOrder.PseudoCityOrOfficeMaintenance_InternalSiteName,
				gdsOrderVM.GDSOrder.PseudoCityOrOfficeMaintenance_ExternalNameId,
				gdsOrderVM.GDSOrder.PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeTypeId,
				gdsOrderVM.GDSOrder.PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeLocationTypeId,
				gdsOrderVM.GDSOrder.PseudoCityOrOfficeMaintenance_FareRedistributionId,
				gdsOrderVM.GDSOrder.PseudoCityOrOfficeMaintenance_SharedPseudoCityOrOfficeFlagNonNullable,
				gdsOrderVM.GDSOrder.PseudoCityOrOfficeMaintenance_CWTOwnedPseudoCityOrOfficeFlagNonNullable,
				gdsOrderVM.GDSOrder.PseudoCityOrOfficeMaintenance_ClientDedicatedPseudoCityOrOfficeFlagNonNullable,
				gdsOrderVM.GDSOrder.PseudoCityOrOfficeMaintenance_ClientGDSAccessFlagNonNullable,
				gdsOrderVM.GDSOrder.PseudoCityOrOfficeMaintenance_DevelopmentOrInternalPseudoCityOrOfficeFlagNonNullable,
				gdsOrderVM.GDSOrder.PseudoCityOrOfficeMaintenance_CubaPseudoCityOrOfficeFlagNonNullable,
				gdsOrderVM.GDSOrder.PseudoCityOrOfficeMaintenance_GovernmentPseudoCityOrOfficeFlagNonNullable,
				gdsOrderVM.GDSOrder.PseudoCityOrOfficeMaintenance_GDSThirdPartyVendorFlagNonNullable,
				pseudoCityOrOfficeMaintenanceGDSThirdPartyVendors,
				
				adminUserGuid
			);
		}

		//Update in DB
		public void Update(GDSOrderVM gdsOrderVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			XElement gdsOrderThirdPartyVendors = GetGDSOrderThirdPartyVendorsXML(gdsOrderVM.GDSOrder);
			XElement gdsOrderLineItems = GetGDSOrderLineItemsXML(gdsOrderVM.GDSOrder);
			XElement pseudoCityOrOfficeMaintenanceGDSThirdPartyVendors = GetPseudoCityOrOfficeMaintenanceGDSOrderThirdPartyVendorsXML(gdsOrderVM);
			
			db.spDesktopDataAdmin_UpdateGDSOrder_v1(
				gdsOrderVM.GDSOrder.GDSOrderId,
				gdsOrderVM.GDSOrder.GDSCode,
				gdsOrderVM.GDSOrder.GDSOrderDateTime,
				gdsOrderVM.GDSOrder.GDSOrderTypeId,
				gdsOrderVM.GDSOrder.GDSOrderStatusId,
				gdsOrderVM.GDSOrder.PseudoCityOrOfficeMaintenanceId,
				gdsOrderVM.GDSOrder.OrderAnalystName,
				gdsOrderVM.GDSOrder.OrderAnalystEmail,
				gdsOrderVM.GDSOrder.OrderAnalystPhone,
				gdsOrderVM.GDSOrder.OrderAnalystCountryCode,
				gdsOrderVM.GDSOrder.TicketNumber,
				gdsOrderVM.GDSOrder.ExpediteFlagNullable,
				gdsOrderVM.GDSOrder.CWTCostCenterNumber,
				gdsOrderVM.GDSOrder.RequesterFirstName,
				gdsOrderVM.GDSOrder.RequesterLastName,
				gdsOrderVM.GDSOrder.RequesterEmail,
				gdsOrderVM.GDSOrder.RequesterPhone,
				gdsOrderVM.GDSOrder.RequesterCountryCode,
				gdsOrderVM.GDSOrder.RequesterUID,
				gdsOrderVM.GDSOrder.ExternalRemarks,
				gdsOrderVM.GDSOrder.DeactivationDateTime,
				gdsOrderThirdPartyVendors,

				//GDS Order Details
				gdsOrderLineItems,

				//PseudoCityOrOfficeMaintenance
				gdsOrderVM.GDSOrder.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeMaintenanceId,
				gdsOrderVM.GDSOrder.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeId,
				gdsOrderVM.GDSOrder.PseudoCityOrOfficeMaintenance.GDSCode,
				gdsOrderVM.GDSOrder.PseudoCityOrOfficeMaintenance.IATAId,
				gdsOrderVM.GDSOrder.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeAddressId,
				gdsOrderVM.GDSOrder.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeDefinedRegionId,
				gdsOrderVM.GDSOrder.PseudoCityOrOfficeMaintenance.LocationContactName,
				gdsOrderVM.GDSOrder.PseudoCityOrOfficeMaintenance.LocationPhone,
				gdsOrderVM.GDSOrder.PseudoCityOrOfficeMaintenance.ActiveFlagNonNullable,
				gdsOrderVM.GDSOrder.PseudoCityOrOfficeMaintenance.InternalSiteName,
				gdsOrderVM.GDSOrder.PseudoCityOrOfficeMaintenance.ExternalNameId,
				gdsOrderVM.GDSOrder.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeTypeId,
				gdsOrderVM.GDSOrder.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeLocationTypeId,
				gdsOrderVM.GDSOrder.PseudoCityOrOfficeMaintenance.FareRedistributionId,
				gdsOrderVM.GDSOrder.PseudoCityOrOfficeMaintenance.SharedPseudoCityOrOfficeFlagNonNullable,
				gdsOrderVM.GDSOrder.PseudoCityOrOfficeMaintenance.CWTOwnedPseudoCityOrOfficeFlagNonNullable,
				gdsOrderVM.GDSOrder.PseudoCityOrOfficeMaintenance.ClientDedicatedPseudoCityOrOfficeFlagNonNullable,
				gdsOrderVM.GDSOrder.PseudoCityOrOfficeMaintenance.ClientGDSAccessFlagNonNullable,
				gdsOrderVM.GDSOrder.PseudoCityOrOfficeMaintenance.DevelopmentOrInternalPseudoCityOrOfficeFlagNonNullable,
				gdsOrderVM.GDSOrder.PseudoCityOrOfficeMaintenance.CubaPseudoCityOrOfficeFlagNonNullable,
				gdsOrderVM.GDSOrder.PseudoCityOrOfficeMaintenance.GovernmentPseudoCityOrOfficeFlagNonNullable,
				gdsOrderVM.GDSOrder.PseudoCityOrOfficeMaintenance.GDSThirdPartyVendorFlagNonNullable,
				pseudoCityOrOfficeMaintenanceGDSThirdPartyVendors,

				adminUserGuid,
				gdsOrderVM.GDSOrder.VersionNumber
			);
		}

		//Get GetGDSOrder GDSThirdPartyVendors
		public List<GDSThirdPartyVendor> GetGDSOrderThirdPartyVendors(int gdsOrderId)
		{
			GDSThirdPartyVendorRepository gdsThirdPartyVendorRepository = new GDSThirdPartyVendorRepository();
			
			List<GDSThirdPartyVendor> gdsThirdPartyVendors = new List<GDSThirdPartyVendor>();
				
			List<GDSOrderThirdPartyVendor> gdsOrderThirdPartyVendors = db.GDSOrderThirdPartyVendors.Where(x => x.GDSOrderId == gdsOrderId).ToList();
			foreach (GDSOrderThirdPartyVendor gdsOrderThirdPartyVendor in gdsOrderThirdPartyVendors)
			{
				GDSThirdPartyVendor gdsThirdPartyVendor = gdsThirdPartyVendorRepository.GetGDSThirdPartyVendor(gdsOrderThirdPartyVendor.GDSThirdPartyVendorId.Value);
				if(gdsThirdPartyVendor != null) {
					gdsThirdPartyVendors.Add(gdsThirdPartyVendor);
				}
			}

			return gdsThirdPartyVendors;
		}

		// Get PseudoCityOrOfficeMaintenance
		public List<PseudoCityOrOfficeMaintenanceJSON> GetPseudoCityOrOfficeMaintenance(string searchText, string gdsCode)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			List<PseudoCityOrOfficeMaintenanceJSON> pseudoCityOrOfficeMaintenanceJSON =
				(from n in db.fnDesktopDataAdmin_SelectPseudoCityOrOfficeMaintenance_v1(searchText, gdsCode, adminUserGuid)
						select
						new PseudoCityOrOfficeMaintenanceJSON
						{
							PseudoCityOrOfficeId = n.PseudoCityOrOfficeId,
							PseudoCityOrOfficeMaintenanceId = Int32.Parse(n.PseudoCityOrOfficeMaintenanceId.ToString()),
                            GDSThirdPartyVendorFlag = (n.GDSThirdPartyVendorFlag == true)
                        }
				).ToList();

			foreach (PseudoCityOrOfficeMaintenanceJSON item in pseudoCityOrOfficeMaintenanceJSON)
			{
				PseudoCityOrOfficeMaintenanceRepository pseudoCityOrOfficeMaintenanceRepository = new PseudoCityOrOfficeMaintenanceRepository();
				PseudoCityOrOfficeMaintenance pseudoCityOrOfficeMaintenance = pseudoCityOrOfficeMaintenanceRepository.GetPseudoCityOrOfficeMaintenance(item.PseudoCityOrOfficeMaintenanceId);
				if(pseudoCityOrOfficeMaintenance != null) {
					PseudoCityOrOfficeAddressRepository pseudoCityOrOfficeAddressRepository = new PseudoCityOrOfficeAddressRepository();
					PseudoCityOrOfficeAddress pseudoCityOrOfficeAddress = pseudoCityOrOfficeAddressRepository.GetPseudoCityOrOfficeAddress(pseudoCityOrOfficeMaintenance.PseudoCityOrOfficeAddressId);
					if (pseudoCityOrOfficeAddress != null)
					{
						item.PseudoCityOrOfficeAddressId = pseudoCityOrOfficeAddress.PseudoCityOrOfficeAddressId;
						item.FirstAddressLine = pseudoCityOrOfficeAddress.FirstAddressLine;
					}
				}
			}
			
			return pseudoCityOrOfficeMaintenanceJSON;
		}

		//Get GDSOrderAnalyst
		public GDSOrderAnalyst GetGDSOrderAnalyst()
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			GDSOrderAnalyst gdsOrderAnalyst = new GDSOrderAnalyst();

			SystemUserRepository systemUserRepository = new SystemUserRepository();
			SystemUser systemUser = systemUserRepository.GetUserBySystemUserGuid(adminUserGuid);
			if (systemUser != null)
			{
				gdsOrderAnalyst.FirstName = systemUser.FirstName;
				gdsOrderAnalyst.LastName = systemUser.LastName;

				//Get Last Order for this user (Barbara High - B:630)
				string gdsOrderCreationUserIdentifier = string.Format("{0} {1} - {2}", systemUser.FirstName, systemUser.LastName, adminUserGuid);
				GDSOrder gdsOrder = GetGDSOrderByUser(gdsOrderCreationUserIdentifier);
				if (gdsOrder != null)
				{
					gdsOrderAnalyst.Email = gdsOrder.OrderAnalystEmail;
					gdsOrderAnalyst.Phone = gdsOrder.OrderAnalystPhone;
					gdsOrderAnalyst.CountryCode = gdsOrder.OrderAnalystCountryCode;
				}

			}

			return gdsOrderAnalyst;
		}

		//Get GDSOrderAnalysts
		public Dictionary<string, string> GetAllGDSOrderAnalysts()
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			
			Dictionary<string, string> gdsOrderAnalysts = new Dictionary<string, string>();

			List<string> analysts = db.GDSOrders.Select(x => x.OrderAnalystName).Distinct().ToList();
			foreach (string analyst in analysts)
			{
				gdsOrderAnalysts.Add(analyst, analyst);
			}

			return gdsOrderAnalysts;
		}

		//Export Items to CSV
		public byte[] Export(
			int? gdsOrderId,
			int? gdsOrderStatusId,
			string ticketNumber,
			string pseudoCityOrOfficeId,
			int? gdsOrderTypeId,
			DateTime? gdsOrderDateTimeStart,
			DateTime? gdsOrderDateTimeEnd,
			string orderAnalyst,
			string internalSiteName,
			string gdsCode
		)
		{
			StringBuilder sb = new StringBuilder();

			//Add Headers
			List<string> headers = new List<string>();
			headers.Add("Order Number");
			headers.Add("Order Date");
			headers.Add("Order Type");
			headers.Add("Order Status");
			headers.Add("Order Analyst");
			headers.Add("Order Analyst Email");
			headers.Add("Order Analyst Phone");
			headers.Add("Order Analyst Country");
			headers.Add("Ticket Number");
			headers.Add("Expedite?");
			headers.Add("CWT Cost Center Number");
			headers.Add("Requester First Name");
			headers.Add("Requester Last Name");
			headers.Add("Requester Email");
			headers.Add("Requester Phone");
			headers.Add("Requester Country");
			headers.Add("Requester UID");
			headers.Add("External Remarks");
			headers.Add("PCC/OID");
			headers.Add("PCC/OID Address");
			headers.Add("Quantity - Add/Delete - Additional Order Detail - Comments");
			headers.Add("Deactivation Date");
			headers.Add("IATA");
			headers.Add("GDS");
			headers.Add("Location Contact Name");
			headers.Add("Location Phone");
			headers.Add("Country");
			headers.Add("Global Region");
			headers.Add("Pseudo City/Office ID Defined Region");
			headers.Add("Active?");
			headers.Add("Internal Site Name");
			headers.Add("External Name");
			headers.Add("Pseudo City/Office ID Type");
			headers.Add("Pseudo City/Office ID Location Type");
			headers.Add("Fare Redistribution");
			headers.Add("Third Party Vendors");

			sb.AppendLine(String.Join(",", headers.Select(x => x.ToString()).ToArray()));

			//Add Items
			List<GDSOrderExport> gdsOrders = GetGDSOrderExports(
				gdsOrderId ?? null,
				gdsOrderStatusId ?? null,
				ticketNumber ?? "",
				pseudoCityOrOfficeId ?? "",
				gdsOrderTypeId ?? null,
				gdsOrderDateTimeStart ?? null,
				gdsOrderDateTimeEnd ?? null,
				orderAnalyst ?? "",
				internalSiteName ?? "",
				gdsCode ?? ""
			);

			foreach (GDSOrderExport item in gdsOrders)
			{

				string date_format = "MM/dd/yy HH:mm";

				EditForDisplay(item);

				//PseudoCityOrOfficeMaintenance
				PseudoCityOrOfficeMaintenanceRepository pseudoCityOrOfficeMaintenanceRepository = new PseudoCityOrOfficeMaintenanceRepository();
				PseudoCityOrOfficeMaintenance pseudoCityOrOfficeMaintenance = pseudoCityOrOfficeMaintenanceRepository.GetPseudoCityOrOfficeMaintenance(item.PseudoCityOrOfficeMaintenanceId);
				if (pseudoCityOrOfficeMaintenance != null)
				{
					item.PseudoCityOrOfficeMaintenance = pseudoCityOrOfficeMaintenance;
					pseudoCityOrOfficeMaintenanceRepository.EditForDisplay(item.PseudoCityOrOfficeMaintenance);
				}

				//GDSOrderLineItems
				StringBuilder gdsOrderLineItemsList = new StringBuilder();
				string fieldSeparator = ", ";

				GDSOrderLineItemRepository gdsOrderLineItemRepository = new GDSOrderLineItemRepository();
				List<GDSOrderLineItem> gdsOrderLineItems = gdsOrderLineItemRepository.GetAllGDSOrderLineItems(item.GDSOrderId);
				if (gdsOrderLineItems != null && gdsOrderLineItems.Count > 0)
				{
					foreach (GDSOrderLineItem gdsOrderLineItem in gdsOrderLineItems)
					{
						gdsOrderLineItemsList.AppendFormat(
							"{0}{1}{2}{3}; ", 
							gdsOrderLineItem.Quantity,
							!string.IsNullOrEmpty(gdsOrderLineItem.GDSOrderLineItemAction.GDSOrderLineItemActionName) ? fieldSeparator + gdsOrderLineItem.GDSOrderLineItemAction.GDSOrderLineItemActionName.Trim() : "",
							!string.IsNullOrEmpty(gdsOrderLineItem.GDSOrderDetail.GDSOrderDetailName) ? fieldSeparator + gdsOrderLineItem.GDSOrderDetail.GDSOrderDetailName.Trim() : "",
							!string.IsNullOrEmpty(gdsOrderLineItem.Comment) ? fieldSeparator + gdsOrderLineItem.Comment.Trim() : ""
						);
					}
				}

				//GDSOrderType
				GDSOrderTypeRepository gdsOrderTypeRepository = new GDSOrderTypeRepository();
				if(item.GDSOrderTypeId != null) {
					item.GDSOrderType = gdsOrderTypeRepository.GetGDSOrderType(Int32.Parse(item.GDSOrderTypeId.ToString()));
				}

				//GDSOrderStatus
				GDSOrderStatusRepository gdsOrderStatusRepository = new GDSOrderStatusRepository();
				if (item.GDSOrderStatusId > 0)
				{
					item.GDSOrderStatus = gdsOrderStatusRepository.GetGDSOrderStatus(item.GDSOrderStatusId);
				}

				//GDSThirdPartyVendors
				GDSThirdPartyVendorRepository gdsThirdPartyVendorRepository = new GDSThirdPartyVendorRepository();
				List<GDSThirdPartyVendor> selectedPseudoCityOrOfficeMaintenanceGDSThirdPartyVendors = gdsThirdPartyVendorRepository.GetGDSThirdPartyVendorsByPseudoCityOrOfficeMaintenanceId(
					item.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeMaintenanceId
				);

				sb.AppendFormat(
					"{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30},{31},{32},{33},{34},{35}",
					
					item.GDSOrderId > 0 ? FormatCSVField(item.GDSOrderId.ToString()) : " ",
					item.GDSOrderDateTime != null ? FormatCSVField(item.GDSOrderDateTime.ToString(date_format)) : " ",
					item.GDSOrderType != null && !string.IsNullOrEmpty(item.GDSOrderType.GDSOrderTypeName) ? FormatCSVField(item.GDSOrderType.GDSOrderTypeName) : " ",
					item.GDSOrderStatus != null && !string.IsNullOrEmpty(item.GDSOrderStatus.GDSOrderStatusName) ? FormatCSVField(item.GDSOrderStatus.GDSOrderStatusName) : " ",
					
					//OrderAnalyst
					!string.IsNullOrEmpty(item.OrderAnalystName) ? FormatCSVField(item.OrderAnalystName) : " ",
					!string.IsNullOrEmpty(item.OrderAnalystEmail) ? FormatCSVField(item.OrderAnalystEmail) : " ",
					!string.IsNullOrEmpty(item.OrderAnalystPhone) ? FormatCSVField(item.OrderAnalystPhone) : " ",
					item.OrderAnalystCountry != null && !string.IsNullOrEmpty(item.OrderAnalystCountry.CountryName) ? FormatCSVField(item.OrderAnalystCountry.CountryName) : " ",
					!string.IsNullOrEmpty(item.TicketNumber) ? FormatCSVField(item.TicketNumber) : " ",
					
					item.ExpediteFlag == true ? "True" : "False",
					!string.IsNullOrEmpty(item.CWTCostCenterNumber) ? FormatCSVField(item.CWTCostCenterNumber) : " ",

					//Requester
					!string.IsNullOrEmpty(item.RequesterFirstName) ? FormatCSVField(item.RequesterFirstName) : " ",
					!string.IsNullOrEmpty(item.RequesterLastName) ? FormatCSVField(item.RequesterLastName) : " ",
					!string.IsNullOrEmpty(item.RequesterEmail) ? FormatCSVField(item.RequesterEmail) : " ",
					!string.IsNullOrEmpty(item.RequesterPhone) ? FormatCSVField(item.RequesterPhone) : " ",
					item.RequesterCountry != null && !string.IsNullOrEmpty(item.RequesterCountry.CountryName) ? FormatCSVField(item.RequesterCountry.CountryName) : " ",
					!string.IsNullOrEmpty(item.RequesterUID) ? FormatCSVField(item.RequesterUID) : " ",

					!string.IsNullOrEmpty(item.ExternalRemarks) ? FormatCSVField(item.ExternalRemarks) : " ",

					//PseudoCityOrOfficeMaintenance
					item.PseudoCityOrOfficeMaintenance != null && !string.IsNullOrEmpty(item.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeId) ? FormatCSVField(item.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeId) : " ",
					item.PseudoCityOrOfficeMaintenance != null && item.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeAddress != null && !string.IsNullOrEmpty(item.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeAddress.FirstAddressLine) ? FormatCSVField(item.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeAddress.FirstAddressLine) : " ",

					//Order Details
					!string.IsNullOrEmpty(gdsOrderLineItemsList.ToString()) ? string.Format("\"{0}\"", gdsOrderLineItemsList) : " ",

					//PseudoCityOrOfficeMaintenance
					item.DeactivationDateTime.HasValue ? FormatCSVField(item.DeactivationDateTime.Value.ToString(date_format)) : " ",
					item.PseudoCityOrOfficeMaintenance != null && item.PseudoCityOrOfficeMaintenance.IATA != null && !string.IsNullOrEmpty(item.PseudoCityOrOfficeMaintenance.IATA.IATANumber) ? FormatCSVField(item.PseudoCityOrOfficeMaintenance.IATA.IATANumber) : " ",
					item.PseudoCityOrOfficeMaintenance != null && item.PseudoCityOrOfficeMaintenance.GDS != null && !string.IsNullOrEmpty(item.PseudoCityOrOfficeMaintenance.GDS.GDSName) ? FormatCSVField(item.PseudoCityOrOfficeMaintenance.GDS.GDSName) : " ",
					item.PseudoCityOrOfficeMaintenance != null && !string.IsNullOrEmpty(item.PseudoCityOrOfficeMaintenance.LocationContactName) ? FormatCSVField(item.PseudoCityOrOfficeMaintenance.LocationContactName) : " ",
					item.PseudoCityOrOfficeMaintenance != null && !string.IsNullOrEmpty(item.PseudoCityOrOfficeMaintenance.LocationPhone) ? FormatCSVField(item.PseudoCityOrOfficeMaintenance.LocationPhone) : " ",
					item.PseudoCityOrOfficeMaintenance != null && !string.IsNullOrEmpty(item.PseudoCityOrOfficeMaintenance.CountryName) ? FormatCSVField(item.PseudoCityOrOfficeMaintenance.CountryName) : " ",
					item.PseudoCityOrOfficeMaintenance != null && !string.IsNullOrEmpty(item.PseudoCityOrOfficeMaintenance.GlobalRegionName) ? FormatCSVField(item.PseudoCityOrOfficeMaintenance.GlobalRegionName) : " ",
					item.PseudoCityOrOfficeMaintenance != null && item.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeDefinedRegion != null && !string.IsNullOrEmpty(item.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeDefinedRegion.PseudoCityOrOfficeDefinedRegionName) ? FormatCSVField(item.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeDefinedRegion.PseudoCityOrOfficeDefinedRegionName) : " ",
					item.PseudoCityOrOfficeMaintenance != null && item.PseudoCityOrOfficeMaintenance.ActiveFlag == true ? "True" : "False",
					item.PseudoCityOrOfficeMaintenance != null && !string.IsNullOrEmpty(item.PseudoCityOrOfficeMaintenance.InternalSiteName) ? FormatCSVField(item.PseudoCityOrOfficeMaintenance.InternalSiteName) : " ",
					item.PseudoCityOrOfficeMaintenance != null && item.PseudoCityOrOfficeMaintenance.ExternalName != null && !string.IsNullOrEmpty(item.PseudoCityOrOfficeMaintenance.ExternalName.ExternalName1) ? FormatCSVField(item.PseudoCityOrOfficeMaintenance.ExternalName.ExternalName1) : " ",
					item.PseudoCityOrOfficeMaintenance != null && item.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeType != null && !string.IsNullOrEmpty(item.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeType.PseudoCityOrOfficeTypeName) ? FormatCSVField(item.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeType.PseudoCityOrOfficeTypeName) : " ",
					item.PseudoCityOrOfficeMaintenance != null && item.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeLocationType != null && !string.IsNullOrEmpty(item.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeLocationType.PseudoCityOrOfficeLocationTypeName) ? FormatCSVField(item.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeLocationType.PseudoCityOrOfficeLocationTypeName) : " ",
					item.PseudoCityOrOfficeMaintenance != null && item.PseudoCityOrOfficeMaintenance.FareRedistribution != null && !string.IsNullOrEmpty(item.PseudoCityOrOfficeMaintenance.FareRedistribution.FareRedistributionName) ? FormatCSVField(item.PseudoCityOrOfficeMaintenance.FareRedistribution.FareRedistributionName) : " ",
					
					//ThirdPartyVendors
					selectedPseudoCityOrOfficeMaintenanceGDSThirdPartyVendors != null ? string.Format("\"{0}\"", String.Join(",", selectedPseudoCityOrOfficeMaintenanceGDSThirdPartyVendors.Select(x => x.GDSThirdPartyVendorName))) : " "
				);

				sb.Append(Environment.NewLine);
			}

			return Encoding.ASCII.GetBytes(sb.ToString());
		}

		private string FormatCSVField(string field)
		{
			return string.Format("\"{0}\"", field);
		}

		public List<GDSOrderExport> GetGDSOrderExports(
			int? gdsOrderId,
			int? gdsOrderStatusId,
			string ticketNumber,
			string pseudoCityOrOfficeId,
			int? gdsOrderTypeId,
			DateTime? gdsOrderDateTimeStart,
			DateTime? gdsOrderDateTimeEnd,
			string orderAnalyst,
			string internalSiteName,
			string gdsCode)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			var result = from n in db.spDesktopDataAdmin_SelectGDSOrderExport_v1(
							gdsOrderId,
							gdsOrderStatusId,
							ticketNumber,
							pseudoCityOrOfficeId,
							gdsOrderTypeId,
							gdsOrderDateTimeStart,
							gdsOrderDateTimeEnd,
							orderAnalyst,
							internalSiteName,
							gdsCode
						)
						 select
							 new GDSOrderExport
							 {
								 GDSOrderId = n.GDSOrderId,
								 GDSOrderDateTime = n.GDSOrderDateTime,
								 GDSOrderTypeId = n.GDSOrderTypeId,
								 GDSOrderStatusId = Int32.Parse(n.GDSOrderStatusId.ToString()),
								 OrderAnalystName = n.OrderAnalystName ?? "",
								 OrderAnalystEmail = n.OrderAnalystEmail ?? "",
								 OrderAnalystPhone = n.OrderAnalystPhone ?? "",
								 OrderAnalystCountryCode = n.OrderAnalystCountryCode ?? "",
								 TicketNumber = n.TicketNumber ?? "",
								 ExpediteFlag = n.ExpediteFlag ?? false,
								 CWTCostCenterNumber = n.CWTCostCenterNumber ?? "",
								 RequesterFirstName = n.RequesterFirstName ?? "",
								 RequesterLastName = n.RequesterLastName ?? "",
								 RequesterPhone = n.RequesterPhone ?? "",
								 RequesterEmail = n.RequesterEmail ?? "",
								 RequesterCountryCode = n.RequesterCountryCode ?? "",
								 RequesterUID = n.RequesterUID ?? "",
								 ExternalRemarks = n.ExternalRemarks ?? "",
								 DeactivationDateTime = n.DeactivationDateTime,
								 PseudoCityOrOfficeMaintenanceId = Int32.Parse(n.PseudoCityOrOfficeMaintenanceId.ToString()),
								 PseudoCityOrOfficeId = n.PseudoCityOrOfficeId ?? "",
								 GDSCode = n.GDSCode ?? ""
							 };

			return result.ToList();
		}

		public string GeneratePDF(GDSOrder gdsOrder)
		{
			string download_url = "";

			try
			{
				//Order Directory
				string orderDirectory = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["GDSOrder_Directory"]);

				// Create instance of the PDF manager.
				PdfManager objPDF = new PdfManager();

				// Create new document.
				using (PdfDocument objDoc = objPDF.CreateDocument())
				{
					// Specify title and creator for the document.
					string pageTitle = GetGDSOrderTitle(gdsOrder);
					objDoc.Title = pageTitle;
					objDoc.Creator = "Carlson Wagonlit Travel";

					//Import From Document
					string htmlTemplate = File.ReadAllText(orderDirectory + "\\Template.html");

					htmlTemplate = htmlTemplate.Replace("##TITLE##", pageTitle);
					htmlTemplate = htmlTemplate.Replace("##LOGO##", HttpContext.Current.Server.MapPath("\\Images\\PDF\\Logo.png"));

					//GDSOrder Fields
					htmlTemplate = htmlTemplate.Replace("##GDSNAME##", gdsOrder.GDS.GDSName);
					htmlTemplate = htmlTemplate.Replace("##ORDERNUMBER##", gdsOrder.GDSOrderId.ToString());
					htmlTemplate = htmlTemplate.Replace("##ORDERDATE##", gdsOrder.GDSOrderDateTime.ToString("MMM dd, yyyy"));
					htmlTemplate = htmlTemplate.Replace("##ORDERTYPE##", gdsOrder.GDSOrderType != null && !string.IsNullOrEmpty(gdsOrder.GDSOrderType.GDSOrderTypeName) ? gdsOrder.GDSOrderType.GDSOrderTypeName : "");
					htmlTemplate = htmlTemplate.Replace("##EXPEDITE##", gdsOrder.ExpediteFlag == true ? "True" : "False");
					htmlTemplate = htmlTemplate.Replace("##PCCOID##", gdsOrder.GDSOrderType != null && !string.IsNullOrEmpty(gdsOrder.GDSOrderType.GDSOrderTypeName) ? gdsOrder.PseudoCityOrOfficeId : "");
					htmlTemplate = htmlTemplate.Replace("##DEINSTALLDATE##", gdsOrder.DeactivationDateTime.HasValue ? gdsOrder.DeactivationDateTime.Value.ToString("MMM dd, yyyy") : "");
					htmlTemplate = htmlTemplate.Replace("##ORDERANALYSTNAME##", gdsOrder.OrderAnalystName);
					htmlTemplate = htmlTemplate.Replace("##ORDERANALYSPHONE##", gdsOrder.OrderAnalystPhone);
					htmlTemplate = htmlTemplate.Replace("##ORDERFROMEMAIL##", gdsOrder.EmailFromAddress);

					//PseudoCityOrOfficeMaintenance Fields
					htmlTemplate = htmlTemplate.Replace("##IATA##", gdsOrder.PseudoCityOrOfficeMaintenance.IATA.IATANumber);
					htmlTemplate = htmlTemplate.Replace("##AGENCY##", gdsOrder.PseudoCityOrOfficeMaintenance.ExternalName.ExternalName1);
					htmlTemplate = htmlTemplate.Replace("##LOCATIONCONTACTNAME##", gdsOrder.PseudoCityOrOfficeMaintenance.LocationContactName);
					htmlTemplate = htmlTemplate.Replace("##LOCATIONPHONE##", gdsOrder.PseudoCityOrOfficeMaintenance.LocationPhone);
					htmlTemplate = htmlTemplate.Replace("##ADDRESSLINE1##", gdsOrder.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeAddress.FirstAddressLine);
					htmlTemplate = htmlTemplate.Replace("##ADDRESSLINE2##", gdsOrder.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeAddress.SecondAddressLine);
					htmlTemplate = htmlTemplate.Replace("##CITY##", gdsOrder.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeAddress.CityName);
					htmlTemplate = htmlTemplate.Replace("##STATEPROVINCE##", gdsOrder.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeAddress.StateProvinceName);
					htmlTemplate = htmlTemplate.Replace("##POSTALCODE##", gdsOrder.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeAddress.PostalCode);
					htmlTemplate = htmlTemplate.Replace("##COUNTRY##", gdsOrder.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeAddress.Country.CountryName);
					htmlTemplate = htmlTemplate.Replace("##REMARKS##", gdsOrder.ExternalRemarks);

					//Additional Order Details
					if (gdsOrder.GDSOrderLineItems != null && gdsOrder.GDSOrderLineItems.Count > 0)
					{
						StringBuilder additionalOrderDetails = new StringBuilder();
						foreach (GDSOrderLineItem gdsOrderLineItem in gdsOrder.GDSOrderLineItems)
						{
							additionalOrderDetails.AppendFormat(@"<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td></tr>",
								gdsOrderLineItem.Quantity > 0 ? gdsOrderLineItem.Quantity.ToString() : "&nbsp;",
								gdsOrderLineItem.GDSOrderLineItemAction.GDSOrderLineItemActionName ?? "&nbsp;",
								gdsOrderLineItem.GDSOrderDetail.GDSOrderDetailName ?? "&nbsp;",
								gdsOrderLineItem.Comment ?? "&nbsp;"
							);
						}
						htmlTemplate = htmlTemplate.Replace("##ADDITIONALORDERDETAILSOPEN##", string.Empty);
						htmlTemplate = htmlTemplate.Replace("##ADDITIONALORDERDETAILS##", additionalOrderDetails.ToString());
						htmlTemplate = htmlTemplate.Replace("##ADDITIONALORDERDETAILSCLOSE##", string.Empty);
					}
					else
					{
						htmlTemplate = htmlTemplate.Replace("##ADDITIONALORDERDETAILSOPEN##", "<!--");
						htmlTemplate = htmlTemplate.Replace("##ADDITIONALORDERDETAILS##", string.Empty);
						htmlTemplate = htmlTemplate.Replace("##ADDITIONALORDERDETAILSCLOSE##", "-->");
					}

					objDoc.ImportFromUrl(htmlTemplate.ToString(), "scale=1; hyperlinks=true; drawbackground=true;");

					//Check for directory and create if not exists
					if (!Directory.Exists(orderDirectory))
					{
						Directory.CreateDirectory(orderDirectory);
					}

					//PDF file will be named by the GDSOrderID (Example:  Order Apollo123.pdf)
					string filename = string.Format("{0}/Order {1}{2}.pdf",
						ConfigurationManager.AppSettings["GDSOrder_Directory"],
						gdsOrder.GDS.GDSName,
						gdsOrder.GDSOrderId.ToString()
					);

					// Save File (Overwrite existing PDF File if exists)
					download_url = objDoc.Save(HttpContext.Current.Server.MapPath(filename), true);

					//Close and Dispose
					if (objDoc != null)
					{
						objDoc.Close();
						objDoc.Dispose();
					}
				}
			}
			catch (Exception ex)
			{
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);
			}

			return download_url;
		}

		public bool SendGDSOrderEmail(GDSOrder gdsOrder)
		{
			bool success = false;

			try
			{
				string download_url = GeneratePDF(gdsOrder);

				if (!string.IsNullOrEmpty(download_url))
				{
					//GDS Order Reference
					string gdsOrderReference = string.Format("{0}{1}", gdsOrder.GDS.GDSName, gdsOrder.GDSOrderId.ToString());

					//Subject
					string subject = GetGDSOrderTitle(gdsOrder);

					//Email Body
					string emailBody = "This is an automated message. Please do not reply to this message.";

					// Attachments
					List<string> attachments = new List<string>();
					attachments.Add(HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["GDSOrder_Directory"]) + "\\" + download_url);

                    //Send Email
					if (CWTEmailHelper.SendEmail(gdsOrder.EmailToAddresses, gdsOrder.EmailFromAddress, subject, emailBody, attachments))
					{
						if (LogGDSOrderEmail(gdsOrder))
						{
							success = true;
						}
					}
				}
			}
			catch (Exception ex)
			{
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				success = false;
			}

			return success;
		}

		public bool LogGDSOrderEmail(GDSOrder gdsOrder)
		{
			bool success = false;

			try
			{
				string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

				db.spDesktopDataAdmin_InsertGDSOrderEmailLog_v1(
					gdsOrder.GDSOrderId,
					gdsOrder.EmailFromAddress,
					string.Join("; ", gdsOrder.EmailToAddresses),
					adminUserGuid
				);

				success = true;
			}
			catch (Exception ex)
			{
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				success = false;
			}

			return success;
		}

		public string GetGDSOrderTitle(GDSOrder gdsOrder)
		{

			return string.Format("*{0} {1}* CWT Order: {2}, PCC/OID: {3}, Order Type: {1}",
					gdsOrder.GDS != null && !string.IsNullOrEmpty(gdsOrder.GDS.GDSName) ? gdsOrder.GDS.GDSName : "",
					gdsOrder.GDSOrderType != null && !string.IsNullOrEmpty(gdsOrder.GDSOrderType.GDSOrderTypeName) ? gdsOrder.GDSOrderType.GDSOrderTypeName : "",
					gdsOrder.GDSOrderId.ToString(),
					gdsOrder.PseudoCityOrOfficeId != null ? gdsOrder.PseudoCityOrOfficeId : ""
				);
		}
    }
}
