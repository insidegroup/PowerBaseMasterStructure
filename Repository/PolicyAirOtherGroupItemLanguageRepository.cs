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
	public class PolicyAirOtherGroupItemLanguageRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Sortable List
		public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyAirOtherGroupItemLanguages_v1Result> GetPolicyAirOtherGroupItemLanguages(int id, int policyGroupId, string filter, string sortField, int sortOrder, int page)
        {
            //query db
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectPolicyAirOtherGroupItemLanguages_v1(id, policyGroupId, filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //add paging information and return
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPolicyAirOtherGroupItemLanguages_v1Result>(result, page, totalRecords);
            return paginatedView;

        }

		//Get one Item
		public PolicyAirOtherGroupItemLanguage GetPolicyAirOtherGroupItemLanguage(int policyAirOtherGroupItemLanguageId)
		{
			return db.PolicyAirOtherGroupItemLanguages.SingleOrDefault(c => c.PolicyAirOtherGroupItemLanguageId == policyAirOtherGroupItemLanguageId);
		}

		//Add
		public void Add(PolicyAirOtherGroupItemLanguageVM policyAirOtherGroupItemLanguageVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_InsertPolicyAirOtherGroupItemLanguage_v1(
				policyAirOtherGroupItemLanguageVM.PolicyGroup.PolicyGroupId,
				policyAirOtherGroupItemLanguageVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId,
				policyAirOtherGroupItemLanguageVM.PolicyAirOtherGroupItemLanguage.Translation,
				policyAirOtherGroupItemLanguageVM.PolicyAirOtherGroupItemLanguage.LanguageCode,
				adminUserGuid
			);
		}

		////Edit
		public void Edit(PolicyAirOtherGroupItemLanguageVM policyAirOtherGroupItemLanguageVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdatePolicyAirOtherGroupItemLanguage_v1(
				policyAirOtherGroupItemLanguageVM.PolicyAirOtherGroupItemLanguage.PolicyAirOtherGroupItemLanguageId,
				policyAirOtherGroupItemLanguageVM.PolicyAirOtherGroupItemLanguage.Translation,
				policyAirOtherGroupItemLanguageVM.PolicyAirOtherGroupItemLanguage.LanguageCode,
				adminUserGuid,
				policyAirOtherGroupItemLanguageVM.PolicyAirOtherGroupItemLanguage.VersionNumber
			);

		}

		//Delete
		public void Delete(PolicyAirOtherGroupItemLanguage policyAirOtherGroupItemLanguage)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeletePolicyAirOtherGroupItemLanguage_v1(
				policyAirOtherGroupItemLanguage.PolicyAirOtherGroupItemLanguageId,
				adminUserGuid,
				policyAirOtherGroupItemLanguage.VersionNumber
				);
		}

		public List<Language> GetAvailableLanguages(int policyOtherGroupHeaderId, int policyGroupId)
		{
			var result = from n in db.spDesktopDataAdmin_SelectPolicyAirOtherGroupItemLanguageAvailableLanguages_v1(policyOtherGroupHeaderId, policyGroupId)
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
