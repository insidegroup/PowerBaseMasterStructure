using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace CWTDesktopDatabase.Repository
{
	public class GDSEndWarningConfigurationLanguageRepository
	{
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//Get a Page of GDS Responses
		public CWTPaginatedList<spDesktopDataAdmin_SelectGDSEndWarningConfigurationLanguages_v1Result> PageGDSEndWarningConfigurationLanguages(int id, int page, string filter, string sortField, int sortOrder)
		{
			//get a page of records
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectGDSEndWarningConfigurationLanguages_v1(id, filter, sortField, sortOrder, page, adminUserGuid).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}

			//put into page object
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectGDSEndWarningConfigurationLanguages_v1Result>(result, page, totalRecords);

			//return to user
			return paginatedView;
		}

		//Get one GDSEndWarningConfigurationLanguage
		public GDSEndWarningConfigurationLanguage GetItem(int id, string languageCode)
		{
			return db.GDSEndWarningConfigurationLanguages.SingleOrDefault(c => c.GDSEndWarningConfigurationId == id && c.LanguageCode == languageCode);
		}

		//Get a list of available Languages
		public List<Language> GetAllAvailableLanguages(int gdsEndWarningConfigurationId)
		{
			var result = from n in db.spDesktopDataAdmin_SelectGDSEndWarningConfigurationLanguageAvailableLanguages_v1(gdsEndWarningConfigurationId)
						 orderby n.LanguageName
						 select
						 new Language
						 {
							 LanguageCode = n.LanguageCode,
							 LanguageName = n.LanguageName
						 };
			return result.ToList();

		}
		
		//Add GDSEndWarningConfigurationLanguage
		public void Add(GDSEndWarningConfigurationLanguage GDSEndWarningConfigurationLanguage)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_InsertGDSEndWarningConfigurationLanguage_v1(
				GDSEndWarningConfigurationLanguage.GDSEndWarningConfigurationId, 
				GDSEndWarningConfigurationLanguage.LanguageCode,
				GDSEndWarningConfigurationLanguage.AdviceMessage,
				adminUserGuid
			);
		}

		//Edit GDSEndWarningConfigurationLanguage
		public void Edit(GDSEndWarningConfigurationLanguage gdsEndWarningConfigurationLanguage, string oldLanguageCode)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdateGDSEndWarningConfigurationLanguage_v1(
				gdsEndWarningConfigurationLanguage.GDSEndWarningConfigurationId,
				gdsEndWarningConfigurationLanguage.LanguageCode,
				oldLanguageCode,
				gdsEndWarningConfigurationLanguage.AdviceMessage,
				adminUserGuid
			);
		}

		public void Delete(GDSEndWarningConfigurationLanguage gdsEndWarningConfigurationLanguage)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeleteGDSEndWarningConfigurationLanguage_v1(
				gdsEndWarningConfigurationLanguage.GDSEndWarningConfigurationId,
				gdsEndWarningConfigurationLanguage.LanguageCode,
				adminUserGuid,
				gdsEndWarningConfigurationLanguage.VersionNumber
			);
		}
	}
}