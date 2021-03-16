using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.ViewModels;

namespace CWTDesktopDatabase.Repository
{
	public class MeetingPNROutputLanguageRepository
    {
		private MeetingDC db = new MeetingDC(Settings.getConnectionString());

		//List of MeetingPNROutputLanguages - Paged
		public CWTPaginatedList<spDesktopDataAdmin_SelectMeetingPNROutputLanguages_v1Result> PageMeetingPNROutputLanguages(int id, string filter, int page, string sortField, int sortOrder)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectMeetingPNROutputLanguages_v1(id, filter, sortField, sortOrder, page).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectMeetingPNROutputLanguages_v1Result>(result, page, Convert.ToInt32(totalRecords));
			return paginatedView;

		}

		//Get one Item from MeetingPNROutput
		public MeetingPNROutputLanguage GetMeetingPNROutputLanguage(int meetingPNROutputId, string languageCode)
		{
			MeetingPNROutputLanguage meetingPNROutputLanguage = new MeetingPNROutputLanguage();

			meetingPNROutputLanguage = db.MeetingPNROutputLanguages.SingleOrDefault(c => c.MeetingPNROutputId == meetingPNROutputId && c.LanguageCode == languageCode);

			if (meetingPNROutputLanguage != null)
			{
				Language language = new Language();
				LanguageRepository languageRepository = new LanguageRepository();
				language = languageRepository.GetLanguage(meetingPNROutputLanguage.LanguageCode);
				if (language != null)
				{
					meetingPNROutputLanguage.Language = language;
				}
			}

			return meetingPNROutputLanguage;
		}

		//Select MeetingPNROutputLanguage Available Languages
		public List<Language> GetAvailableLanguages(int clientDefinedReferenceItemPNROutputId)
		{
			var result = from n in db.spDesktopDataAdmin_SelectMeetingPNROutputLanguageAvailableLanguages_v1(clientDefinedReferenceItemPNROutputId)
						 select new Language
						 {
							 LanguageName = n.LanguageName,
							 LanguageCode = n.LanguageCode
						 };
			return result.ToList();
		}

		//Add Item
		public void Add(MeetingPNROutputLanguage clientDefinedReferenceItemPNROutputLanguage)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_InsertMeetingPNROutputLanguage_v1(
				clientDefinedReferenceItemPNROutputLanguage.MeetingPNROutputId,
				clientDefinedReferenceItemPNROutputLanguage.LanguageCode,
				clientDefinedReferenceItemPNROutputLanguage.RemarkTranslation,
				adminUserGuid
			);
		}

		//Update Item
		public void Update(MeetingPNROutputLanguage clientDefinedReferenceItemPNROutputLanguage)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdateMeetingPNROutputLanguage_v1(
				clientDefinedReferenceItemPNROutputLanguage.MeetingPNROutputId,
				clientDefinedReferenceItemPNROutputLanguage.CurrentLanguageCode,
				clientDefinedReferenceItemPNROutputLanguage.LanguageCode,
				clientDefinedReferenceItemPNROutputLanguage.RemarkTranslation,
				clientDefinedReferenceItemPNROutputLanguage.VersionNumber,
				adminUserGuid
			);
		}

		//Delete Item
		public void Delete(MeetingPNROutputLanguage clientDefinedReferenceItemPNROutputLanguage)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeleteMeetingPNROutputLanguage_v1(
				clientDefinedReferenceItemPNROutputLanguage.MeetingPNROutputId,
				clientDefinedReferenceItemPNROutputLanguage.LanguageCode,
				clientDefinedReferenceItemPNROutputLanguage.VersionNumber,
				adminUserGuid
			);
		}

    }
}