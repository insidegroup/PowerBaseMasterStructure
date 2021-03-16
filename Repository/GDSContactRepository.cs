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

namespace CWTDesktopDatabase.Repository
{
	public class GDSContactRepository
    {
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//Get a Page of Client Telephonies - for Page Listings
		public CWTPaginatedList<spDesktopDataAdmin_SelectGDSContacts_v1Result> PageGDSContacts(int page, string filter, string sortField, int sortOrder)
		{
			//get a page of records
			var result = db.spDesktopDataAdmin_SelectGDSContacts_v1(filter, sortField, sortOrder, page).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}

			//put into page object
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectGDSContacts_v1Result>(result, page, totalRecords);

			//return to user
			return paginatedView;
		}
		
		//Get one GDSContact
        public GDSContact GetGDSContact(int id)
        {
            return db.GDSContacts.SingleOrDefault(c => c.GDSContactId == id);
        }

        //Add Data From Linked Tables for Display
        public void EditForDisplay(GDSContact gdsContact)
        {
			//GDSRequestTypeIds
			GDSContactGDSRequestTypeRepository gdsContactGDSRequestTypeRepository = new GDSContactGDSRequestTypeRepository();
			gdsContact.GDSRequestTypeIds = gdsContactGDSRequestTypeRepository.GetGDSContactGDSRequestTypes(gdsContact.GDSContactId).Select(x => x.GDSRequestTypeId);

			//GDSRequestTypes
			List<GDSRequestType> gdsContactRequestTypes = new List<GDSRequestType>(); 
			foreach (int gdsContactRequestTypeId in gdsContact.GDSRequestTypeIds)
			{
				GDSRequestTypeRepository gdsRequestTypeRepository = new GDSRequestTypeRepository();
				GDSRequestType gdsRequestType = gdsRequestTypeRepository.GetGDSRequestType(gdsContactRequestTypeId);
				if (gdsRequestType != null)
				{
					gdsContactRequestTypes.Add(gdsRequestType);
				}
			}

			gdsContact.GDSRequestTypes = gdsContactRequestTypes;
		}

		//GDSRequestTypes to XML
		public XElement GetGDSRequestTypesXML(GDSContact gdsContact)
		{	
			XmlDocument doc = new XmlDocument();
			XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
			doc.AppendChild(dec);
			XmlElement root = doc.CreateElement("GDSRequestTypes");
			doc.AppendChild(root);

			if (gdsContact.GDSRequestTypes != null)
			{
				foreach (GDSRequestType gdsRequestType in gdsContact.GDSRequestTypes)
				{
					XmlElement xmlGDSRequestType = doc.CreateElement("GDSRequestType");

					XmlElement xmlGDSRequestTypeId = doc.CreateElement("GDSRequestTypeId");
					xmlGDSRequestTypeId.InnerText = gdsRequestType.GDSRequestTypeId.ToString();
					xmlGDSRequestType.AppendChild(xmlGDSRequestTypeId);

					root.AppendChild(xmlGDSRequestType);
				}
			}

			return System.Xml.Linq.XElement.Parse(doc.OuterXml);
		}

		//Add to DB
		public void Add(GDSContact gdsContact)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			XElement gdsRequestTypes = GetGDSRequestTypesXML(gdsContact);
			
			db.spDesktopDataAdmin_InsertGDSContact_v1(
				gdsContact.GDSCode,
				gdsContact.CountryCode,
				gdsContact.GlobalRegionCode,
				gdsContact.PseudoCityOrOfficeBusinessId,
				gdsContact.PseudoCityOrOfficeDefinedRegionId,
				gdsContact.LastName,
				gdsContact.FirstName,
				gdsContact.EmailAddress,
				gdsContact.Phone,
				gdsRequestTypes,
				adminUserGuid
			);
		}

		//Update in DB
		public void Update(GDSContact gdsContact)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			XElement gdsRequestTypes = GetGDSRequestTypesXML(gdsContact); 
			
			db.spDesktopDataAdmin_UpdateGDSContact_v1(
				gdsContact.GDSContactId,
				gdsContact.GDSCode,
				gdsContact.CountryCode,
				gdsContact.GlobalRegionCode,
				gdsContact.PseudoCityOrOfficeBusinessId,
				gdsContact.PseudoCityOrOfficeDefinedRegionId,
				gdsContact.LastName,
				gdsContact.FirstName,
				gdsContact.EmailAddress,
				gdsContact.Phone,
				gdsRequestTypes,
				adminUserGuid,
				gdsContact.VersionNumber
			);
		}

		//Delete in DB
		public void Delete(GDSContact gdsContact)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeleteGDSContact_v1(
				gdsContact.GDSContactId,
				adminUserGuid,
				gdsContact.VersionNumber
			);
		}
    }
}
