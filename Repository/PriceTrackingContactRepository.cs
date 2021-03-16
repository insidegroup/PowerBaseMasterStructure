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
	public class PriceTrackingContactRepository
    {
		private PriceTrackingDC db = new PriceTrackingDC(Settings.getConnectionString());

		//Get a Page of Client Telephonies - for Page Listings
		public CWTPaginatedList<spDesktopDataAdmin_SelectPriceTrackingContacts_v1Result> PagePriceTrackingContacts(int id, int page, string filter, string sortField, int sortOrder)
		{
			//get a page of records
			var result = db.spDesktopDataAdmin_SelectPriceTrackingContacts_v1(id, filter, sortField, sortOrder, page).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}

			//put into page object
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPriceTrackingContacts_v1Result>(result, page, totalRecords);

			//return to user
			return paginatedView;
		}

        //Get one PriceTrackingContact
        public PriceTrackingContact GetPriceTrackingContact(int id)
        {
            return db.PriceTrackingContacts.SingleOrDefault(c => c.PriceTrackingContactId == id);
        }

        //Get PriceTrackingContacts By PriceTrackingSetupGroupId
        public List<PriceTrackingContact> GetPriceTrackingContactByPriceTrackingSetupGroupId(int id)
        {
            return db.PriceTrackingContacts.Where(c => c.PriceTrackingSetupGroupId == id).ToList();
        }

        //Add Data From Linked Tables for Display
        public void EditForDisplay(PriceTrackingContact priceTrackingContact)
        {
            //ContactType
            if (priceTrackingContact.ContactTypeId > 0)
            {
                ContactTypeRepository contactTypeRepository = new ContactTypeRepository();
                ContactType contactType = contactTypeRepository.GetContactType(priceTrackingContact.ContactTypeId);
                if(contactType != null)
                {
                    priceTrackingContact.ContactType = contactType;
                }
            }
        }
        
        //Add to DB
        public void Add(PriceTrackingContact priceTrackingContact)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			
			db.spDesktopDataAdmin_InsertPriceTrackingContact_v1(
                priceTrackingContact.PriceTrackingSetupGroupId,
                priceTrackingContact.ContactTypeId,
                priceTrackingContact.LastName,
                priceTrackingContact.FirstName,
                priceTrackingContact.EmailAddress,
                priceTrackingContact.PriceTrackingContactUserTypeId,
                priceTrackingContact.PriceTrackingDashboardAccessFlag,
                priceTrackingContact.PriceTrackingEmailAlertTypeId,
		        adminUserGuid
			);
		}

		//Update in DB
		public void Update(PriceTrackingContact priceTrackingContact)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			
			db.spDesktopDataAdmin_UpdatePriceTrackingContact_v1(
                priceTrackingContact.PriceTrackingSetupGroupId,
                priceTrackingContact.PriceTrackingContactId,
                priceTrackingContact.ContactTypeId,
                priceTrackingContact.LastName,
                priceTrackingContact.FirstName,
                priceTrackingContact.EmailAddress,
                priceTrackingContact.PriceTrackingContactUserTypeId,
                priceTrackingContact.PriceTrackingDashboardAccessFlag,
                priceTrackingContact.PriceTrackingEmailAlertTypeId,
                adminUserGuid,
				priceTrackingContact.VersionNumber
			);
		}

		//Delete in DB
		public void Delete(PriceTrackingContact priceTrackingContact)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeletePriceTrackingContact_v1(
				priceTrackingContact.PriceTrackingContactId,
				adminUserGuid,
				priceTrackingContact.VersionNumber
			);
		}
    }
}
