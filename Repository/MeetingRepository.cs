using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Helpers;
using System.Linq.Dynamic;
using System.Text.RegularExpressions;

namespace CWTDesktopDatabase.Repository
{
    public class MeetingRepository
    {
        private MeetingDC db = new MeetingDC(Settings.getConnectionString());

        //Get a Page of Meetings - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectMeetings_v1Result> PageMeetings(bool deleted, int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectMeetings_v1(deleted, filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectMeetings_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get one Meeting
        public Meeting GetGroup(int id)
        {
			Meeting meeting = new Meeting();
			meeting = db.Meetings.SingleOrDefault(c => c.MeetingID == id);
			if (meeting != null)
			{
				meeting.MeetingDisplayName = meeting.MeetingName + " - " + meeting.MeetingReferenceNumber;
			}
			return meeting;
        }

		//Get ClientSubUnits Linked to a Meeting
		public List<ClientSubUnitCountryVM> GetLinkedClientSubUnits(int Meetingid, bool linked)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			var result = from n in db.spDesktopDataAdmin_SelectMeetingLinkedClientSubUnits_v1(Meetingid, adminUserGuid, linked)
						 select new ClientSubUnitCountryVM
						 {
							 ClientSubUnitName = n.ClientSubUnitName.Trim(),
							 ClientSubUnitGuid = n.ClientSubUnitGuid,
							 CountryName = n.CountryName,
							 HasWriteAccess = (bool)n.HasWriteAccess,
                             IsClientExpiredFlag = n.IsClientExpiredFlag
                         };
			return result.ToList();
		}

		//Change the status on an item
        public void UpdateLinkedClientSubUnit(int MeetingId, string clientSubUnitGuid)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateMeetingLinkedClientSubUnit_v1(
                    MeetingId,
                    clientSubUnitGuid,
                    adminUserGuid
                    );

        }

		//Get CreditCards Linked to a Meeting
		public List<MeetingLinkedCreditCardVM> GetLinkedCreditCards(int meetingId, bool linked)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			var result = from n in db.spDesktopDataAdmin_SelectMeetingLinkedCreditCards_v1(meetingId, adminUserGuid, linked)
						 select new MeetingLinkedCreditCardVM
						 {
							 CreditCardId = Int32.Parse(n.CreditCardId.ToString()),
							 CreditCardHolderName = n.CreditCardHolderName != null ? n.CreditCardHolderName : "",
							 MaskedCreditCardNumber = n.MaskedCreditCardNumber != null ? n.MaskedCreditCardNumber.ToString() : "",
							 ProductName = n.ProductName != null ? n.ProductName : "",
							 SupplierName = n.SupplierName != null ? n.SupplierName : "",
							 ClientTopUnitGuid = n.ClientTopUnitGuid != null ? n.ClientTopUnitGuid : "",
							 ClientSubUnitGuid = n.ClientSubUnitGuid != null ? n.ClientSubUnitGuid : "",
							 SourceSystemCode = n.SourceSystemCode != null ? n.SourceSystemCode : "",
							 ClientAccountNumber = n.ClientAccountNumber != null ? n.ClientAccountNumber : "",
							 TravelerTypeGuid = n.TravelerTypeGuid != null ? n.TravelerTypeGuid : "",
							 HierarchyItem = n.HierarchyItem != null ? n.HierarchyItem : "",
							 HierarchyType = n.HierarchyType != null ? n.HierarchyType : "",
							 HasWriteAccess = n.HasWriteAccess.HasValue ? n.HasWriteAccess.Value : false
						 };
			return result.ToList();
		}

        //Change the status on an item
		public void UpdateLinkedCreditCard(
			int meetingId,
			string hierarchyType,
			string clientTopUnitGuid,
			string clientSubUnitGuid,
			string sourceSystemCode,
			string clientAccountNumber,
			string travelerTypeGuid,
			int creditCardId
		)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdateMeetingLinkedCreditCard_v1(
					meetingId,
					hierarchyType,
					clientTopUnitGuid,
					clientSubUnitGuid,
					sourceSystemCode,
					clientAccountNumber,
					travelerTypeGuid,
					creditCardId,
					adminUserGuid
			);
		}

        //Add Data From Linked Tables for Display
        public void EditGroupForDisplay(Meeting group)
		{
			if (group.ClientTopUnitGuid != "")
			{
				ClientTopUnitRepository clientTopUnitRepository = new ClientTopUnitRepository();
				ClientTopUnit clientTopUnit = clientTopUnitRepository.GetClientTopUnit(group.ClientTopUnitGuid);
				if (clientTopUnit != null)
				{
					group.ClientTopUnit = clientTopUnit;
						
					group.HierarchyType = "ClientTopUnit";
					group.HierarchyCode = clientTopUnit.ClientTopUnitGuid;
					group.HierarchyItem = clientTopUnit.ClientTopUnitName;
				}
			}

			if (group.CityCode != "")
			{
				CityRepository cityRepository = new CityRepository();
				City city = cityRepository.GetCity(group.CityCode);
				if (city != null)
				{
					group.City = city;
				}
			}

			//Set MeetingArriveByDateTime from date
			if (group.MeetingArriveByDateTime != null)
			{
				group.MeetingArriveByTime = group.MeetingArriveByDateTime.Value.ToString("HH:mm tt");
			}

			//Set MeetingArriveByDateTime from date
			if (group.MeetingLeaveAfterDateTime != null)
			{
				group.MeetingLeaveAfterTime = group.MeetingLeaveAfterDateTime.Value.ToString("HH:mm tt");
			}
        }

        //Change the deleted status on an item
        public void UpdateGroupDeletedStatus(Meeting group)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateMeetingDeletedStatus_v1(
                    group.MeetingID,
                    group.DeletedFlag,
                    adminUserGuid,
                    group.VersionNumber
			);
        }

        //Edit Group
        public void Edit(Meeting group)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            db.spDesktopDataAdmin_UpdateMeeting_v1(
                group.MeetingID,
                group.MeetingName,
				group.MeetingReferenceNumber,
				group.MeetingLocation,
				group.MeetingStartDate,
				group.MeetingEndDate,
				group.MeetingArriveByDateTime,
				group.MeetingLeaveAfterDateTime,
				group.CityCode,
				group.HierarchyCode,
				group.EnabledFlag,
				group.EnabledDate,
				group.ExpiryDate,
				group.DeletedFlag,
                adminUserGuid,
                group.VersionNumber
            );
        }

        //Add Group
        public void Add(Meeting group)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertMeeting_v1(
				group.MeetingName,
				group.MeetingReferenceNumber,
				group.MeetingLocation,
				group.MeetingStartDate,
				group.MeetingEndDate,
				group.MeetingArriveByDateTime,
				group.MeetingLeaveAfterDateTime,
				group.CityCode,
				group.HierarchyCode,
				group.EnabledFlag,
				group.EnabledDate,
				group.ExpiryDate,
				group.DeletedFlag,
                adminUserGuid
            );
        } 

		//Add Group
		public List<Meeting> GetAvailableMeetings(string hierarchyType, string hierarchyItem, string clientAccountNumber, string sourceSystemCode, string travelerTypeGuid)
		{
			AutoCompleteRepository autoCompleteRepository = new AutoCompleteRepository();
			List<Meeting> meetings = new List<Meeting>();

			if (hierarchyType == "ClientSubUnitTravelerType")
			{
				meetings = autoCompleteRepository.AutoCompleteAvailableMeetings(hierarchyType, hierarchyItem, null, null, travelerTypeGuid);
			}
			else if (hierarchyType == "TravelerType")
			{
				string clientSubUnitName = string.Empty;
				TravelerTypeRepository travelerTypeRepository = new TravelerTypeRepository();
				TravelerType travelerType = travelerTypeRepository.GetTravelerType(hierarchyItem);
				if (travelerType != null)
				{
					travelerTypeRepository.EditForDisplay(travelerType);
					if (travelerType.ClientSubUnitTravelerTypes != null)
					{
						foreach (ClientSubUnitTravelerType clientSubUnitTravelerType in travelerType.ClientSubUnitTravelerTypes)
						{
							if (clientSubUnitTravelerType.ClientSubUnit != null)
							{
								clientSubUnitName = clientSubUnitTravelerType.ClientSubUnit.ClientSubUnitName;
								List<Meeting> tt_meetings = autoCompleteRepository.AutoCompleteAvailableMeetings(hierarchyType, clientSubUnitName, null, null, hierarchyItem);
								foreach (Meeting meeting in tt_meetings)
								{
									if (!meetings.Any(x => x.MeetingID == meeting.MeetingID))
									{
										meetings.Add(meeting);
									}
								}
							}
						}
					}
				}
			}
			else if (hierarchyType == "ClientAccount")
			{
				meetings = autoCompleteRepository.AutoCompleteAvailableMeetings(hierarchyType, null, hierarchyItem, sourceSystemCode, null);
			}
			else
			{
				meetings = autoCompleteRepository.AutoCompleteAvailableMeetings(hierarchyType, hierarchyItem, null, null, null);
			}

			return meetings;
		}

		//Get MeetingItemsByCityCode
		public List<Meeting> GetMeetingItemsByCityCode(string cityCode)
		{
			return db.Meetings.Where(c => c.CityCode == cityCode).ToList();
		}
    }
}

