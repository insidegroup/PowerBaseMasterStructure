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
	public class PolicyCarOtherGroupItemLanguageRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Sortable List
		public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyCarOtherGroupItemLanguages_v1Result> GetPolicyCarOtherGroupItemLanguages(int id, int policyGroupId, string filter, string sortField, int sortOrder, int page)
        {
            //query db
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectPolicyCarOtherGroupItemLanguages_v1(id, policyGroupId, filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //add paging information and return
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPolicyCarOtherGroupItemLanguages_v1Result>(result, page, totalRecords);
            return paginatedView;

        }

		//Get one Item
		public PolicyCarOtherGroupItemLanguage GetPolicyCarOtherGroupItemLanguage(int policyCarOtherGroupItemLanguageId)
		{
			return db.PolicyCarOtherGroupItemLanguages.SingleOrDefault(c => c.PolicyCarOtherGroupItemLanguageId == policyCarOtherGroupItemLanguageId);
		}

		//Add
		public void Add(PolicyCarOtherGroupItemLanguageVM policyCarOtherGroupItemLanguageVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_InsertPolicyCarOtherGroupItemLanguage_v1(
				policyCarOtherGroupItemLanguageVM.PolicyGroup.PolicyGroupId,
				policyCarOtherGroupItemLanguageVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId,
				policyCarOtherGroupItemLanguageVM.PolicyCarOtherGroupItemLanguage.Translation,
				policyCarOtherGroupItemLanguageVM.PolicyCarOtherGroupItemLanguage.LanguageCode,
				adminUserGuid
			);
		}

		////Edit
		public void Edit(PolicyCarOtherGroupItemLanguageVM policyCarOtherGroupItemLanguageVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdatePolicyCarOtherGroupItemLanguage_v1(
				policyCarOtherGroupItemLanguageVM.PolicyCarOtherGroupItemLanguage.PolicyCarOtherGroupItemLanguageId,
				policyCarOtherGroupItemLanguageVM.PolicyCarOtherGroupItemLanguage.Translation,
				policyCarOtherGroupItemLanguageVM.PolicyCarOtherGroupItemLanguage.LanguageCode,
				adminUserGuid,
				policyCarOtherGroupItemLanguageVM.PolicyCarOtherGroupItemLanguage.VersionNumber
			);

		}

		//Delete
		public void Delete(PolicyCarOtherGroupItemLanguage policyCarOtherGroupItemLanguage)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeletePolicyCarOtherGroupItemLanguage_v1(
				policyCarOtherGroupItemLanguage.PolicyCarOtherGroupItemLanguageId,
				adminUserGuid,
				policyCarOtherGroupItemLanguage.VersionNumber
				);
		}

		public List<Language> GetAvailableLanguages(int policyOtherGroupHeaderId, int policyGroupId)
		{
			var result = from n in db.spDesktopDataAdmin_SelectPolicyCarOtherGroupItemLanguageAvailableLanguages_v1(policyOtherGroupHeaderId, policyGroupId)
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
