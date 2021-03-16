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
	public class PolicyPriceTrackingOtherGroupItemLanguageRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Sortable List
		public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyPriceTrackingOtherGroupItemLanguages_v1Result> GetPolicyPriceTrackingOtherGroupItemLanguages(int id, int policyGroupId, string filter, string sortField, int sortOrder, int page)
        {
            //query db
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectPolicyPriceTrackingOtherGroupItemLanguages_v1(id, policyGroupId, filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //add paging information and return
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPolicyPriceTrackingOtherGroupItemLanguages_v1Result>(result, page, totalRecords);
            return paginatedView;

        }

		//Get one Item
		public PolicyPriceTrackingOtherGroupItemLanguage GetPolicyPriceTrackingOtherGroupItemLanguage(int PolicyPriceTrackingOtherGroupItemLanguageId)
		{
			return db.PolicyPriceTrackingOtherGroupItemLanguages.SingleOrDefault(c => c.PolicyPriceTrackingOtherGroupItemLanguageId == PolicyPriceTrackingOtherGroupItemLanguageId);
		}

		//Add
		public void Add(PolicyPriceTrackingOtherGroupItemLanguageVM PolicyPriceTrackingOtherGroupItemLanguageVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_InsertPolicyPriceTrackingOtherGroupItemLanguage_v1(
				PolicyPriceTrackingOtherGroupItemLanguageVM.PolicyGroup.PolicyGroupId,
				PolicyPriceTrackingOtherGroupItemLanguageVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId,
				PolicyPriceTrackingOtherGroupItemLanguageVM.PolicyPriceTrackingOtherGroupItemLanguage.Translation,
				PolicyPriceTrackingOtherGroupItemLanguageVM.PolicyPriceTrackingOtherGroupItemLanguage.LanguageCode,
				adminUserGuid
			);
		}

		////Edit
		public void Edit(PolicyPriceTrackingOtherGroupItemLanguageVM PolicyPriceTrackingOtherGroupItemLanguageVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdatePolicyPriceTrackingOtherGroupItemLanguage_v1(
				PolicyPriceTrackingOtherGroupItemLanguageVM.PolicyPriceTrackingOtherGroupItemLanguage.PolicyPriceTrackingOtherGroupItemLanguageId,
				PolicyPriceTrackingOtherGroupItemLanguageVM.PolicyPriceTrackingOtherGroupItemLanguage.Translation,
				PolicyPriceTrackingOtherGroupItemLanguageVM.PolicyPriceTrackingOtherGroupItemLanguage.LanguageCode,
				adminUserGuid,
				PolicyPriceTrackingOtherGroupItemLanguageVM.PolicyPriceTrackingOtherGroupItemLanguage.VersionNumber
			);

		}

		//Delete
		public void Delete(PolicyPriceTrackingOtherGroupItemLanguage PolicyPriceTrackingOtherGroupItemLanguage)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeletePolicyPriceTrackingOtherGroupItemLanguage_v1(
				PolicyPriceTrackingOtherGroupItemLanguage.PolicyPriceTrackingOtherGroupItemLanguageId,
				adminUserGuid,
				PolicyPriceTrackingOtherGroupItemLanguage.VersionNumber
				);
		}

		public List<Language> GetAvailableLanguages(int policyOtherGroupHeaderId, int policyGroupId)
		{
			var result = from n in db.spDesktopDataAdmin_SelectPolicyPriceTrackingOtherGroupItemLanguageAvailableLanguages_v1(policyOtherGroupHeaderId, policyGroupId)
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
