using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace CWTDesktopDatabase.Repository
{
	public class PolicyOtherGroupHeaderColumnNameLanguageRepository
	{
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//Get a Page of PolicyOtherGroupHeaderColumnNameLanguages
		public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyOtherGroupHeaderColumnNameLanguages_v1Result> PagePolicyOtherGroupHeaderColumnNameLanguages(int id, int page, string filter, string sortField, int sortOrder)
		{
			//get a page of records
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectPolicyOtherGroupHeaderColumnNameLanguages_v1(id, filter, sortField, sortOrder, page, adminUserGuid).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}

			//put into page object
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPolicyOtherGroupHeaderColumnNameLanguages_v1Result>(result, page, totalRecords);

			//return to user
			return paginatedView;
		}

		//Get one PolicyOtherGroupHeaderColumnNameLanguage
		public PolicyOtherGroupHeaderColumnNameLanguage GetPolicyOtherGroupHeaderColumnNameLanguage(int policyOtherGroupHeaderColumnNameLanguageId)
		{
			return db.PolicyOtherGroupHeaderColumnNameLanguages.SingleOrDefault(c => c.PolicyOtherGroupHeaderColumnNameLanguageId == policyOtherGroupHeaderColumnNameLanguageId);
		}

		//Add PolicyOtherGroupHeaderColumnNameLanguage
		public void Add(PolicyOtherGroupHeaderColumnNameLanguage policyOtherGroupHeaderColumnNameLanguage)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_InsertPolicyOtherGroupHeaderColumnNameLanguage_v1(
				policyOtherGroupHeaderColumnNameLanguage.PolicyOtherGroupHeaderColumnNameId,
				policyOtherGroupHeaderColumnNameLanguage.ColumnNameTranslation,
				policyOtherGroupHeaderColumnNameLanguage.LanguageCode,
				adminUserGuid
			);
		}

		//Edit PolicyOtherGroupHeaderColumnNameLanguage
		public void Edit(PolicyOtherGroupHeaderColumnNameLanguage policyOtherGroupHeaderColumnNameLanguage)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdatePolicyOtherGroupHeaderColumnNameLanguage_v1(
				policyOtherGroupHeaderColumnNameLanguage.PolicyOtherGroupHeaderColumnNameLanguageId,
				policyOtherGroupHeaderColumnNameLanguage.ColumnNameTranslation,
				policyOtherGroupHeaderColumnNameLanguage.LanguageCode,
				adminUserGuid,
				policyOtherGroupHeaderColumnNameLanguage.VersionNumber
			);
		}

		public void Delete(PolicyOtherGroupHeaderColumnNameLanguage policyOtherGroupHeaderColumnNameLanguage)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeletePolicyOtherGroupHeaderColumnNameLanguage_v1(
				policyOtherGroupHeaderColumnNameLanguage.PolicyOtherGroupHeaderColumnNameLanguageId,
				adminUserGuid,
				policyOtherGroupHeaderColumnNameLanguage.VersionNumber
			);
		}

		public List<Language> GetAvailableLanguages(int id)
		{
			var result = from n in db.spDesktopDataAdmin_SelectPolicyOtherGroupHeaderColumnNameLanguagesAvailableLanguages_v1(id)
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