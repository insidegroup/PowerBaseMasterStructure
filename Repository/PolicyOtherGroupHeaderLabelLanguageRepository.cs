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
	public class PolicyOtherGroupHeaderLabelLanguageRepository
	{
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//Get a Page of PolicyOtherGroupHeaderLabelLanguages
		public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyOtherGroupHeaderLabelLanguages_v1Result> PagePolicyOtherGroupHeaderLabelLanguages(int id, int page, string filter, string sortField, int sortOrder)
		{
			//get a page of records
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectPolicyOtherGroupHeaderLabelLanguages_v1(id, filter, sortField, sortOrder, page, adminUserGuid).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}

			//put into page object
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPolicyOtherGroupHeaderLabelLanguages_v1Result>(result, page, totalRecords);

			//return to user
			return paginatedView;
		}

		//Get one PolicyOtherGroupHeaderLabelLanguage
		public PolicyOtherGroupHeaderLabelLanguage GetPolicyOtherGroupHeaderLabelLanguage(int policyOtherGroupHeaderLabelLanguageId)
		{
			return db.PolicyOtherGroupHeaderLabelLanguages.SingleOrDefault(c => c.PolicyOtherGroupHeaderLabelLanguageId == policyOtherGroupHeaderLabelLanguageId);
		}

		//Add PolicyOtherGroupHeaderLabelLanguage
		public void Add(PolicyOtherGroupHeaderLabelLanguageVM policyOtherGroupHeaderLabelLanguageVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_InsertPolicyOtherGroupHeaderLabelLanguage_v1(
				policyOtherGroupHeaderLabelLanguageVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId,
				policyOtherGroupHeaderLabelLanguageVM.PolicyOtherGroupHeaderLabelLanguage.LabelTranslation,
				policyOtherGroupHeaderLabelLanguageVM.PolicyOtherGroupHeaderLabelLanguage.LanguageCode,
				adminUserGuid
			);
		}

		//Edit PolicyOtherGroupHeaderLabelLanguage
		public void Edit(PolicyOtherGroupHeaderLabelLanguage policyOtherGroupHeaderLabelLanguage)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdatePolicyOtherGroupHeaderLabelLanguage_v1(
				policyOtherGroupHeaderLabelLanguage.PolicyOtherGroupHeaderLabelLanguageId,
				policyOtherGroupHeaderLabelLanguage.LabelTranslation,
				policyOtherGroupHeaderLabelLanguage.LanguageCode,
				adminUserGuid,
				policyOtherGroupHeaderLabelLanguage.VersionNumber
			);
		}

		public void Delete(PolicyOtherGroupHeaderLabelLanguage policyOtherGroupHeaderLabelLanguage)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeletePolicyOtherGroupHeaderLabelLanguage_v1(
				policyOtherGroupHeaderLabelLanguage.PolicyOtherGroupHeaderLabelLanguageId,
				adminUserGuid,
				policyOtherGroupHeaderLabelLanguage.VersionNumber
			);
		}

		public List<Language> GetAvailableLanguages(int id)
		{
			var result = from n in db.spDesktopDataAdmin_SelectPolicyOtherGroupHeaderLabelLanguagesAvailableLanguages_v1(id)
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