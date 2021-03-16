using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.ViewModels;

namespace CWTDesktopDatabase.Repository
{
    public class MeetingContactRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());
		private MeetingDC meetingDb = new MeetingDC(Settings.getConnectionString());

		//List of Contacts in a Meeting - Paged
		public CWTPaginatedList<spDesktopDataAdmin_SelectMeetingContacts_v1Result> PageMeetingContacts(int meetingID, string filter, string sortField, int sortOrder, int page)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = meetingDb.spDesktopDataAdmin_SelectMeetingContacts_v1(meetingID, filter, sortField, sortOrder, page).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectMeetingContacts_v1Result>(result, page, Convert.ToInt32(totalRecords));
			return paginatedView;

		}

		//Get MeetingContact
		public MeetingContact GetContact(int id)
		{
			return meetingDb.MeetingContacts.SingleOrDefault(c => c.MeetingContactID == id);
		}

		//Add Data From Linked Tables for Display
		public void EditForDisplay(MeetingContact meetingContact)
		{
			ContactType contactType = new ContactType();
			contactType = db.ContactTypes.SingleOrDefault(c => c.ContactTypeId == meetingContact.ContactTypeId);
			if (contactType != null)
			{
				meetingContact.ContactType = contactType;
			}

			Country country = new Country();
			country = db.Countries.SingleOrDefault(c => c.CountryCode == meetingContact.CountryCode);
			if (country != null)
			{
				meetingContact.Country = country;
			}
		}

		//Add Contact
		public void Add(MeetingContactVM meetingContactVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			meetingDb.spDesktopDataAdmin_InsertMeetingContact_v1(
				meetingContactVM.Meeting.MeetingID,
				meetingContactVM.MeetingContact.ContactTypeId,
				meetingContactVM.MeetingContact.MeetingContactLastName,
				meetingContactVM.MeetingContact.MeetingContactFirstName,
				meetingContactVM.MeetingContact.MeetingContactPhoneNumber,
				meetingContactVM.MeetingContact.MeetingContactEmailAddress,
				meetingContactVM.MeetingContact.CountryCode,
				meetingContactVM.MeetingContact.CopyItineraryFlag,
				adminUserGuid
			);

		}

		//Edit Contact
		public void Edit(MeetingContactVM meetingContactVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			meetingDb.spDesktopDataAdmin_UpdateMeetingContact_v1(
				meetingContactVM.Meeting.MeetingID,
				meetingContactVM.MeetingContact.MeetingContactID,
				meetingContactVM.MeetingContact.ContactTypeId,
				meetingContactVM.MeetingContact.MeetingContactLastName,
				meetingContactVM.MeetingContact.MeetingContactFirstName,
				meetingContactVM.MeetingContact.MeetingContactPhoneNumber,
				meetingContactVM.MeetingContact.MeetingContactEmailAddress,
				meetingContactVM.MeetingContact.CountryCode,
				meetingContactVM.MeetingContact.CopyItineraryFlag,
				adminUserGuid,
				meetingContactVM.MeetingContact.VersionNumber
			);
		}

		//Delete Contact
		public void Delete(MeetingContact meetingContact)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			meetingDb.spDesktopDataAdmin_DeleteMeetingContact_v1(
				meetingContact.MeetingContactID,
				adminUserGuid,
				meetingContact.VersionNumber
			);

		}
    }
}