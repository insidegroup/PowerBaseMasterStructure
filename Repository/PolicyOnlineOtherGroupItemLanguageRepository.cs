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
	public class PolicyOnlineOtherGroupItemLanguageRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Sortable List
		public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyOnlineOtherGroupItemLanguages_v1Result> GetPolicyOnlineOtherGroupItemLanguages(int id, int policyGroupId, string filter, string sortField, int sortOrder, int page)
        {
            //query db
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectPolicyOnlineOtherGroupItemLanguages_v1(id, policyGroupId, filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //add paging information and return
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPolicyOnlineOtherGroupItemLanguages_v1Result>(result, page, totalRecords);
            return paginatedView;

        }

		//Get one Item
		public PolicyOnlineOtherGroupItemLanguage GetPolicyOnlineOtherGroupItemLanguage(int PolicyOnlineOtherGroupItemLanguageId)
		{
			return db.PolicyOnlineOtherGroupItemLanguages.SingleOrDefault(c => c.PolicyOnlineOtherGroupItemLanguageId == PolicyOnlineOtherGroupItemLanguageId);
		}

		//Add
		public void Add(PolicyOnlineOtherGroupItemLanguageVM PolicyOnlineOtherGroupItemLanguageVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_InsertPolicyOnlineOtherGroupItemLanguage_v1(
				PolicyOnlineOtherGroupItemLanguageVM.PolicyGroup.PolicyGroupId,
				PolicyOnlineOtherGroupItemLanguageVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId,
				PolicyOnlineOtherGroupItemLanguageVM.PolicyOnlineOtherGroupItemLanguage.Translation,
				PolicyOnlineOtherGroupItemLanguageVM.PolicyOnlineOtherGroupItemLanguage.LanguageCode,
				adminUserGuid
			);
		}

		////Edit
		public void Edit(PolicyOnlineOtherGroupItemLanguageVM PolicyOnlineOtherGroupItemLanguageVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdatePolicyOnlineOtherGroupItemLanguage_v1(
				PolicyOnlineOtherGroupItemLanguageVM.PolicyOnlineOtherGroupItemLanguage.PolicyOnlineOtherGroupItemLanguageId,
				PolicyOnlineOtherGroupItemLanguageVM.PolicyOnlineOtherGroupItemLanguage.Translation,
				PolicyOnlineOtherGroupItemLanguageVM.PolicyOnlineOtherGroupItemLanguage.LanguageCode,
				adminUserGuid,
				PolicyOnlineOtherGroupItemLanguageVM.PolicyOnlineOtherGroupItemLanguage.VersionNumber
			);

		}

		//Delete
		public void Delete(PolicyOnlineOtherGroupItemLanguage PolicyOnlineOtherGroupItemLanguage)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeletePolicyOnlineOtherGroupItemLanguage_v1(
				PolicyOnlineOtherGroupItemLanguage.PolicyOnlineOtherGroupItemLanguageId,
				adminUserGuid,
				PolicyOnlineOtherGroupItemLanguage.VersionNumber
				);
		}

		public List<Language> GetAvailableLanguages(int policyOtherGroupHeaderId, int policyGroupId)
		{
			var result = from n in db.spDesktopDataAdmin_SelectPolicyOnlineOtherGroupItemLanguageAvailableLanguages_v1(policyOtherGroupHeaderId, policyGroupId)
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
