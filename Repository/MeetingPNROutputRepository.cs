using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.ViewModels;

namespace CWTDesktopDatabase.Repository
{
	public class MeetingPNROutputRepository
    {
		private MeetingDC db = new MeetingDC(Settings.getConnectionString());

		//List of Meetings for a ClientSubUnit - Paged
		public CWTPaginatedList<spDesktopDataAdmin_SelectMeetingPNROutputItems_v1Result> PageMeetingPNROutputItems(string filter, int id, int page, string sortField, int sortOrder)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectMeetingPNROutputItems_v1(id, filter, sortField, sortOrder, page).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectMeetingPNROutputItems_v1Result>(result, page, Convert.ToInt32(totalRecords));
			return paginatedView;
		}

		//Get one MeetingPNROutput
		public MeetingPNROutput GetMeetingPNROutput(int meetingPNROutputId)
		{
			MeetingPNROutput meetingPNROutput = new MeetingPNROutput();

			meetingPNROutput = db.MeetingPNROutputs.SingleOrDefault(c => c.MeetingPNROutputId == meetingPNROutputId);

			if(meetingPNROutput != null)
			{
				PNROutputRemarkType PNROutputRemarkType = new PNROutputRemarkType();
				PNROutputRemarkTypeRepository PNROutputRemarkTypeRepository = new PNROutputRemarkTypeRepository();
				PNROutputRemarkType = PNROutputRemarkTypeRepository.GetPNROutputRemarkType(meetingPNROutput.PNROutputRemarkTypeCode);
				if (PNROutputRemarkType != null)
				{
					meetingPNROutput.PNROutputRemarkType = PNROutputRemarkType;
				}

			}

			return meetingPNROutput;
		}

		//Add Data From Linked Tables for Display
		public void EditGroupForDisplay(MeetingPNROutput group)
		{
			if (group.GDSCode != "")
			{
				GDSRepository GDSRepository = new GDSRepository();
				GDS GDS = GDSRepository.GetGDS(group.GDSCode);
				if (GDS != null)
				{
					group.GDS = GDS;
				}
			}

			if (group.CountryCode != "")
			{
				CountryRepository countryRepository = new CountryRepository();
				Country country = countryRepository.GetCountry(group.CountryCode);
				if (country != null)
				{
					group.Country = country;
				}
			}

			if (group.DefaultLanguageCode != "")
			{
				LanguageRepository languageRepository = new LanguageRepository();
				Language language = languageRepository.GetLanguage(group.DefaultLanguageCode);
				if (language != null)
				{
					group.Language = language;
				}
			}
		}

		//Add Item
		public void Add(MeetingPNROutput meetingPNROutput)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_InsertMeetingPNROutput_v1(
				meetingPNROutput.MeetingID,
				meetingPNROutput.PNROutputRemarkTypeCode,
				meetingPNROutput.DefaultLanguageCode,
				meetingPNROutput.GDSCode,
				meetingPNROutput.GDSRemarkQualifier,
				meetingPNROutput.DefaultRemark,
				meetingPNROutput.CountryCode,
				adminUserGuid
			);
		}

		//Update Item
		public void Update(MeetingPNROutput meetingPNROutput)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdateMeetingPNROutput_v1(
				meetingPNROutput.MeetingPNROutputId,
				meetingPNROutput.MeetingID,
				meetingPNROutput.PNROutputRemarkTypeCode,
				meetingPNROutput.DefaultLanguageCode,
				meetingPNROutput.GDSCode,
				meetingPNROutput.GDSRemarkQualifier,
				meetingPNROutput.DefaultRemark,
				meetingPNROutput.CountryCode,
				meetingPNROutput.VersionNumber,
				adminUserGuid
			);
		}

		//Delete Item
		public void Delete(MeetingPNROutput meetingPNROutput)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeleteMeetingPNROutput_v1(
				meetingPNROutput.MeetingPNROutputId,
				meetingPNROutput.VersionNumber,
				adminUserGuid
			);
		}

    }
}