using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace CWTDesktopDatabase.Repository
{
	public class PolicyOtherGroupHeaderTableNameLanguageRepository
	{
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//Get a Page of PolicyOtherGroupHeaderTableNameLanguages
		public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyOtherGroupHeaderTableNameLanguages_v1Result> PagePolicyOtherGroupHeaderTableNameLanguages(int id, int page, string filter, string sortField, int sortOrder)
		{
			//get a page of records
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectPolicyOtherGroupHeaderTableNameLanguages_v1(id, filter, sortField, sortOrder, page, adminUserGuid).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}

			//put into page object
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPolicyOtherGroupHeaderTableNameLanguages_v1Result>(result, page, totalRecords);

			//return to user
			return paginatedView;
		}

		//Get one PolicyOtherGroupHeaderTableNameLanguage
		public PolicyOtherGroupHeaderTableNameLanguage GetPolicyOtherGroupHeaderTableNameLanguage(int policyOtherGroupHeaderTableNameLanguageId)
		{
			return db.PolicyOtherGroupHeaderTableNameLanguages.SingleOrDefault(c => c.PolicyOtherGroupHeaderTableNameLanguageId == policyOtherGroupHeaderTableNameLanguageId);
		}

		//Add PolicyOtherGroupHeaderTableNameLanguage
		public void Add(PolicyOtherGroupHeaderTableNameLanguageVM policyOtherGroupHeaderTableNameLanguageVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_InsertPolicyOtherGroupHeaderTableNameLanguage_v1(
				policyOtherGroupHeaderTableNameLanguageVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId,
				policyOtherGroupHeaderTableNameLanguageVM.PolicyOtherGroupHeaderTableNameLanguage.TableNameTranslation,
				policyOtherGroupHeaderTableNameLanguageVM.PolicyOtherGroupHeaderTableNameLanguage.LanguageCode,
				adminUserGuid
			);
		}

		//Edit PolicyOtherGroupHeaderTableNameLanguage
		public void Edit(PolicyOtherGroupHeaderTableNameLanguage policyOtherGroupHeaderTableNameLanguage)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdatePolicyOtherGroupHeaderTableNameLanguage_v1(
				policyOtherGroupHeaderTableNameLanguage.PolicyOtherGroupHeaderTableNameLanguageId,
				policyOtherGroupHeaderTableNameLanguage.TableNameTranslation,
				policyOtherGroupHeaderTableNameLanguage.LanguageCode,
				adminUserGuid,
				policyOtherGroupHeaderTableNameLanguage.VersionNumber
			);
		}

		public void Delete(PolicyOtherGroupHeaderTableNameLanguage policyOtherGroupHeaderTableNameLanguage)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeletePolicyOtherGroupHeaderTableNameLanguage_v1(
				policyOtherGroupHeaderTableNameLanguage.PolicyOtherGroupHeaderTableNameLanguageId,
				adminUserGuid,
				policyOtherGroupHeaderTableNameLanguage.VersionNumber
			);
		}

		public List<Language> GetAvailableLanguages(int id)
		{
			var result = from n in db.spDesktopDataAdmin_SelectPolicyOtherGroupHeaderTableNameLanguagesAvailableLanguages_v1(id)
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