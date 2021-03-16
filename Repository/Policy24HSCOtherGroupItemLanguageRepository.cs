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
	public class Policy24HSCOtherGroupItemLanguageRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Sortable List
		public CWTPaginatedList<spDesktopDataAdmin_SelectPolicy24HSCOtherGroupItemLanguages_v1Result> GetPolicy24HSCOtherGroupItemLanguages(int id, int policyGroupId, string filter, string sortField, int sortOrder, int page)
        {
            //query db
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectPolicy24HSCOtherGroupItemLanguages_v1(id, policyGroupId, filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //add paging information and return
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPolicy24HSCOtherGroupItemLanguages_v1Result>(result, page, totalRecords);
            return paginatedView;

        }

		//Get one Item
		public Policy24HSCOtherGroupItemLanguage GetPolicy24HSCOtherGroupItemLanguage(int policy24HSCOtherGroupItemLanguageId)
		{
			return db.Policy24HSCOtherGroupItemLanguages.SingleOrDefault(c => c.Policy24HSCOtherGroupItemLanguageId == policy24HSCOtherGroupItemLanguageId);
		}

		//Add
		public void Add(Policy24HSCOtherGroupItemLanguageVM policy24HSCOtherGroupItemLanguageVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_InsertPolicy24HSCOtherGroupItemLanguage_v1(
				policy24HSCOtherGroupItemLanguageVM.PolicyGroup.PolicyGroupId,
				policy24HSCOtherGroupItemLanguageVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId,
				policy24HSCOtherGroupItemLanguageVM.Policy24HSCOtherGroupItemLanguage.Translation,
				policy24HSCOtherGroupItemLanguageVM.Policy24HSCOtherGroupItemLanguage.LanguageCode,
				adminUserGuid
			);
		}

		////Edit
		public void Edit(Policy24HSCOtherGroupItemLanguageVM policy24HSCOtherGroupItemLanguageVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdatePolicy24HSCOtherGroupItemLanguage_v1(
				policy24HSCOtherGroupItemLanguageVM.Policy24HSCOtherGroupItemLanguage.Policy24HSCOtherGroupItemLanguageId,
				policy24HSCOtherGroupItemLanguageVM.Policy24HSCOtherGroupItemLanguage.Translation,
				policy24HSCOtherGroupItemLanguageVM.Policy24HSCOtherGroupItemLanguage.LanguageCode,
				adminUserGuid,
				policy24HSCOtherGroupItemLanguageVM.Policy24HSCOtherGroupItemLanguage.VersionNumber
			);

		}

		//Delete
		public void Delete(Policy24HSCOtherGroupItemLanguage policy24HSCOtherGroupItemLanguage)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeletePolicy24HSCOtherGroupItemLanguage_v1(
				policy24HSCOtherGroupItemLanguage.Policy24HSCOtherGroupItemLanguageId,
				adminUserGuid,
				policy24HSCOtherGroupItemLanguage.VersionNumber
				);
		}

		public List<Language> GetAvailableLanguages(int policyOtherGroupHeaderId, int policyGroupId)
		{
			var result = from n in db.spDesktopDataAdmin_SelectPolicy24HSCOtherGroupItemLanguageAvailableLanguages_v1(policyOtherGroupHeaderId, policyGroupId)
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
