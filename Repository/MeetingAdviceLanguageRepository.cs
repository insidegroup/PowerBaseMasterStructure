using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;

namespace CWTDesktopDatabase.Repository
{
	public class MeetingAdviceLanguageRepository
	{
		private MeetingDC db = new MeetingDC(Settings.getConnectionString());

		//Get a Page of MeetingAdviceLanguages
		public CWTPaginatedList<spDesktopDataAdmin_SelectMeetingAdviceLanguages_v1Result> PageMeetingAdviceLanguages(int meetingId, int meetingAdviceLanguageTypeId, int page, string sortField, int sortOrder)
		{
			//get a page of records
			var result = db.spDesktopDataAdmin_SelectMeetingAdviceLanguages_v1(meetingId, meetingAdviceLanguageTypeId, sortField, sortOrder, page).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}

			//put into page object
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectMeetingAdviceLanguages_v1Result>(result, page, totalRecords);

			//return to user
			return paginatedView;
		}
		
		//Get one Item
		public MeetingAdviceLanguage GetMeetingAdviceLanguage(int meetingID, int meetingAdviceTypeId, string languageCode)
		{
			return db.MeetingAdviceLanguages.SingleOrDefault(
				c => (
					c.MeetingID == meetingID &&
					c.MeetingAdviceTypeId == meetingAdviceTypeId &&
					c.LanguageCode == languageCode)
				);
		}

		//Get MeetingAdviceTypeName
		public string GetMeetingAdviceTypeName(int meetingAdviceTypeId)
		{
			string meetingAdviceTypeName = string.Empty;

			MeetingAdviceType meetingAdviceType = db.MeetingAdviceTypes.SingleOrDefault(c => c.MeetingAdviceTypeId == meetingAdviceTypeId);
			if (meetingAdviceType != null)
			{
				meetingAdviceTypeName = meetingAdviceType.MeetingAdviceTypeName;
			}

			return meetingAdviceTypeName;
		}

		//Get MeetingAdviceTypeLabelName
		public string GetMeetingAdviceTypeLabelName(int meetingAdviceTypeId)
		{
			string meetingAdviceTypeName = GetMeetingAdviceTypeName(meetingAdviceTypeId);

			if (meetingAdviceTypeName != null)
			{
				meetingAdviceTypeName = (meetingAdviceTypeName != "Advice") ? meetingAdviceTypeName : "Meeting Advice";
			}

			return meetingAdviceTypeName;
		}

		public void EditItemForDisplay(MeetingAdviceLanguage meetingAdviceLanguage)
		{
			//Get Language
			if(meetingAdviceLanguage.LanguageCode != null) {
				LanguageRepository languageRepository = new LanguageRepository();
				Language language = new Language();
				language = languageRepository.GetLanguage(meetingAdviceLanguage.LanguageCode);
				if (language != null)
				{
					meetingAdviceLanguage.Language = language;
				}
			}
		}

		//Add to DB
		public void Add(MeetingAdviceLanguageVM meetingAdviceLanguageVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_InsertMeetingAdviceLanguage_v1(
				meetingAdviceLanguageVM.MeetingAdviceLanguage.MeetingID,
				meetingAdviceLanguageVM.MeetingAdviceLanguage.MeetingAdviceTypeId,
				meetingAdviceLanguageVM.MeetingAdviceLanguage.LanguageCode,
				meetingAdviceLanguageVM.MeetingAdviceLanguage.MeetingAdvice,
				adminUserGuid
			);
		}

		//Update in DB
		public void Update(MeetingAdviceLanguage meetingAdviceLanguage)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] {'|'})[0];

			db.spDesktopDataAdmin_UpdateMeetingAdviceLanguage_v1(
				meetingAdviceLanguage.MeetingID,
				meetingAdviceLanguage.MeetingAdviceTypeId,
				meetingAdviceLanguage.LanguageCode,
				meetingAdviceLanguage.MeetingAdvice,
				adminUserGuid,
				meetingAdviceLanguage.VersionNumber
			);
		}

		//Delete From DB
		public void Delete(MeetingAdviceLanguage meetingAdviceLanguage)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeleteMeetingAdviceLanguage_v1(
				meetingAdviceLanguage.MeetingID,
				meetingAdviceLanguage.MeetingAdviceTypeId,
				meetingAdviceLanguage.LanguageCode,
				adminUserGuid,
				meetingAdviceLanguage.VersionNumber
			);
		}

		//Get a list of available Languages
		public List<Language> GetAllAvailableLanguages(int meetingId, int meetingAdviceTypeId)
		{
			var result = from n in db.spDesktopDataAdmin_SelectMeetingAdviceLanguageAvailableLanguages_v1(meetingId, meetingAdviceTypeId)
						 orderby n.LanguageName
						 select
						 new Language
						 {
							 LanguageCode = n.LanguageCode,
							 LanguageName = n.LanguageName
						 };
			return result.ToList();

		}
	}
}