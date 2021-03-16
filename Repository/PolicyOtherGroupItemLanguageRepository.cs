using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using System.Linq.Dynamic;
using CWTDesktopDatabase.ViewModels;

namespace CWTDesktopDatabase.Repository
{
	public class PolicyOtherGroupItemLanguageRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Sortable List
		public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyOtherGroupItemLanguages_v1Result> GetPolicyOtherGroupItemLanguages(int id, int policyGroupId, string filter, string sortField, int sortOrder, int page)
        {
            //query db
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectPolicyOtherGroupItemLanguages_v1(id, policyGroupId, filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //add paging information and return
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPolicyOtherGroupItemLanguages_v1Result>(result, page, totalRecords);
            return paginatedView;

        }

		//Get one Item
		public PolicyOtherGroupItemLanguage GetPolicyOtherGroupItemLanguage(int policyOtherGroupItemLanguageId)
		{
			return db.PolicyOtherGroupItemLanguages.SingleOrDefault(c => c.PolicyOtherGroupItemLanguageId == policyOtherGroupItemLanguageId);
		}

		//Add
		public void Add(PolicyOtherGroupItemLanguageVM policyOtherGroupItemLanguageVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_InsertPolicyOtherGroupItemLanguage_v1(
				policyOtherGroupItemLanguageVM.PolicyGroup.PolicyGroupId,
				policyOtherGroupItemLanguageVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId,
				policyOtherGroupItemLanguageVM.PolicyOtherGroupItemLanguage.Translation,
				policyOtherGroupItemLanguageVM.PolicyOtherGroupItemLanguage.LanguageCode,
				adminUserGuid
			);
		}

		////Edit
		public void Edit(PolicyOtherGroupItemLanguageVM policyOtherGroupItemLanguageVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdatePolicyOtherGroupItemLanguage_v1(
				policyOtherGroupItemLanguageVM.PolicyOtherGroupItemLanguage.PolicyOtherGroupItemLanguageId,
				policyOtherGroupItemLanguageVM.PolicyOtherGroupItemLanguage.Translation,
				policyOtherGroupItemLanguageVM.PolicyOtherGroupItemLanguage.LanguageCode,
				adminUserGuid,
				policyOtherGroupItemLanguageVM.PolicyOtherGroupItemLanguage.VersionNumber
			);

		}

		//Delete
		public void Delete(PolicyOtherGroupItemLanguage policyOtherGroupItemLanguage)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeletePolicyOtherGroupItemLanguage_v1(
				policyOtherGroupItemLanguage.PolicyOtherGroupItemLanguageId,
				adminUserGuid,
				policyOtherGroupItemLanguage.VersionNumber
				);
		}

		public List<Language> GetAvailableLanguages(int policyOtherGroupHeaderId, int policyGroupId)
		{
			var result = from n in db.spDesktopDataAdmin_SelectPolicyOtherGroupItemLanguageAvailableLanguages_v1(policyOtherGroupHeaderId, policyGroupId)
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
