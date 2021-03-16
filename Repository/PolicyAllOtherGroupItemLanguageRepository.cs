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
	public class PolicyAllOtherGroupItemLanguageRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Sortable List
		public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyAllOtherGroupItemLanguages_v1Result> GetPolicyAllOtherGroupItemLanguages(int id, int policyGroupId, string filter, string sortField, int sortOrder, int page)
        {
            //query db
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectPolicyAllOtherGroupItemLanguages_v1(id, policyGroupId, filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //add paging information and return
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPolicyAllOtherGroupItemLanguages_v1Result>(result, page, totalRecords);
            return paginatedView;

        }

		//Get one Item
		public PolicyAllOtherGroupItemLanguage GetPolicyAllOtherGroupItemLanguage(int policyAllOtherGroupItemLanguageId)
		{
			return db.PolicyAllOtherGroupItemLanguages.SingleOrDefault(c => c.PolicyAllOtherGroupItemLanguageId == policyAllOtherGroupItemLanguageId);
		}

		//Add
		public void Add(PolicyAllOtherGroupItemLanguageVM policyAllOtherGroupItemLanguageVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_InsertPolicyAllOtherGroupItemLanguage_v1(
				policyAllOtherGroupItemLanguageVM.PolicyGroup.PolicyGroupId,
				policyAllOtherGroupItemLanguageVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId,
				policyAllOtherGroupItemLanguageVM.PolicyAllOtherGroupItemLanguage.Translation,
				policyAllOtherGroupItemLanguageVM.PolicyAllOtherGroupItemLanguage.LanguageCode,
				adminUserGuid
			);
		}

		////Edit
		public void Edit(PolicyAllOtherGroupItemLanguageVM policyAllOtherGroupItemLanguageVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdatePolicyAllOtherGroupItemLanguage_v1(
				policyAllOtherGroupItemLanguageVM.PolicyAllOtherGroupItemLanguage.PolicyAllOtherGroupItemLanguageId,
				policyAllOtherGroupItemLanguageVM.PolicyAllOtherGroupItemLanguage.Translation,
				policyAllOtherGroupItemLanguageVM.PolicyAllOtherGroupItemLanguage.LanguageCode,
				adminUserGuid,
				policyAllOtherGroupItemLanguageVM.PolicyAllOtherGroupItemLanguage.VersionNumber
			);

		}

		//Delete
		public void Delete(PolicyAllOtherGroupItemLanguage policyAllOtherGroupItemLanguage)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeletePolicyAllOtherGroupItemLanguage_v1(
				policyAllOtherGroupItemLanguage.PolicyAllOtherGroupItemLanguageId,
				adminUserGuid,
				policyAllOtherGroupItemLanguage.VersionNumber
				);
		}

		public List<Language> GetAvailableLanguages(int policyOtherGroupHeaderId, int policyGroupId)
		{
			var result = from n in db.spDesktopDataAdmin_SelectPolicyAllOtherGroupItemLanguageAvailableLanguages_v1(policyOtherGroupHeaderId, policyGroupId)
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
