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
	public class PolicyHotelOtherGroupItemLanguageRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Sortable List
		public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyHotelOtherGroupItemLanguages_v1Result> GetPolicyHotelOtherGroupItemLanguages(int id, int policyGroupId, string filter, string sortField, int sortOrder, int page)
        {
            //query db
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectPolicyHotelOtherGroupItemLanguages_v1(id, policyGroupId, filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //add paging information and return
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPolicyHotelOtherGroupItemLanguages_v1Result>(result, page, totalRecords);
            return paginatedView;

        }

		//Get one Item
		public PolicyHotelOtherGroupItemLanguage GetPolicyHotelOtherGroupItemLanguage(int policyHotelOtherGroupItemLanguageId)
		{
			return db.PolicyHotelOtherGroupItemLanguages.SingleOrDefault(c => c.PolicyHotelOtherGroupItemLanguageId == policyHotelOtherGroupItemLanguageId);
		}

		//Add
		public void Add(PolicyHotelOtherGroupItemLanguageVM policyHotelOtherGroupItemLanguageVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_InsertPolicyHotelOtherGroupItemLanguage_v1(
				policyHotelOtherGroupItemLanguageVM.PolicyGroup.PolicyGroupId,
				policyHotelOtherGroupItemLanguageVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId,
				policyHotelOtherGroupItemLanguageVM.PolicyHotelOtherGroupItemLanguage.Translation,
				policyHotelOtherGroupItemLanguageVM.PolicyHotelOtherGroupItemLanguage.LanguageCode,
				adminUserGuid
			);
		}

		////Edit
		public void Edit(PolicyHotelOtherGroupItemLanguageVM policyHotelOtherGroupItemLanguageVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdatePolicyHotelOtherGroupItemLanguage_v1(
				policyHotelOtherGroupItemLanguageVM.PolicyHotelOtherGroupItemLanguage.PolicyHotelOtherGroupItemLanguageId,
				policyHotelOtherGroupItemLanguageVM.PolicyHotelOtherGroupItemLanguage.Translation,
				policyHotelOtherGroupItemLanguageVM.PolicyHotelOtherGroupItemLanguage.LanguageCode,
				adminUserGuid,
				policyHotelOtherGroupItemLanguageVM.PolicyHotelOtherGroupItemLanguage.VersionNumber
			);

		}

		//Delete
		public void Delete(PolicyHotelOtherGroupItemLanguage policyHotelOtherGroupItemLanguage)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeletePolicyHotelOtherGroupItemLanguage_v1(
				policyHotelOtherGroupItemLanguage.PolicyHotelOtherGroupItemLanguageId,
				adminUserGuid,
				policyHotelOtherGroupItemLanguage.VersionNumber
				);
		}

		public List<Language> GetAvailableLanguages(int policyOtherGroupHeaderId, int policyGroupId)
		{
			var result = from n in db.spDesktopDataAdmin_SelectPolicyHotelOtherGroupItemLanguageAvailableLanguages_v1(policyOtherGroupHeaderId, policyGroupId)
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
