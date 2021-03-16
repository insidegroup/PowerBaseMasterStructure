using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Linq.Dynamic;

namespace CWTDesktopDatabase.Repository
{
	public class PolicyAirParameterGroupItemLanguageRepository
	{
		private HierarchyDC main_db = new HierarchyDC(Settings.getConnectionString());
		private PolicyAirParameterGroupItemLanguageDC db = new PolicyAirParameterGroupItemLanguageDC(Settings.getConnectionString());

		//Get a Page of AirlineAdvice - for Page Listings
		public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyAirParameterGroupItemLanguages_v1Result> PagePolicyAirParameterGroupItemLanguages(int policyAirParameterGroupItemId, int policyGroupId, string filter, int page, string sortField, int sortOrder)
		{
			//get a page of records
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectPolicyAirParameterGroupItemLanguages_v1(policyAirParameterGroupItemId, policyGroupId, filter, sortField, sortOrder, page, adminUserGuid).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}

			//put into page object
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPolicyAirParameterGroupItemLanguages_v1Result>(result, page, totalRecords);

			//return to user
			return paginatedView;
		}


		//Get one Item
		public PolicyAirParameterGroupItemLanguage GetItem(int policyAirParameterGroupItemId, string languageCode)
		{
			return main_db.PolicyAirParameterGroupItemLanguages.SingleOrDefault(c => (c.PolicyAirParameterGroupItemId == policyAirParameterGroupItemId)
					&& (c.LanguageCode == languageCode));
		}

		//Languages not used by this policyAirParameterGroupItem
		public List<Language> GetAvailableLanguages(int policyAirParameterGroupItemId)
		{

			var result = from n in db.spDesktopDataAdmin_SelectPolicyAirParameterGroupItemAvailableLanguages_v1(policyAirParameterGroupItemId)
						 select new Language
						 {
							 LanguageName = n.LanguageName,
							 LanguageCode = n.LanguageCode
						 };
			return result.ToList();
		}

		//Add Data From Linked Tables for Display
		public void EditItemForDisplay(PolicyAirParameterGroupItemLanguage policyAirParameterGroupItemLanguage)
		{
			//Add LanguageName
			if (policyAirParameterGroupItemLanguage.LanguageCode != null)
			{
				LanguageRepository languageRepository = new LanguageRepository();
				Language language = new Language();
				language = languageRepository.GetLanguage(policyAirParameterGroupItemLanguage.LanguageCode);
				if (language != null)
				{
					policyAirParameterGroupItemLanguage.LanguageName = language.LanguageName;
				}
			}

			//Add PolicyGroupName
			PolicyAirParameterGroupItemRepository policyAirParameterGroupItemRepository = new PolicyAirParameterGroupItemRepository();
			PolicyAirParameterGroupItem policyAirParameterGroupItem = new PolicyAirParameterGroupItem();
			policyAirParameterGroupItem = policyAirParameterGroupItemRepository.GetPolicyAirParameterGroupItem(policyAirParameterGroupItemLanguage.PolicyAirParameterGroupItemId);


			if (policyAirParameterGroupItem != null)
			{
				PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();
				PolicyGroup policyGroup = new PolicyGroup();
				policyGroup = policyGroupRepository.GetGroup(policyAirParameterGroupItem.PolicyGroupId);

				//policyAirParameterGroupItemRepository.EditItemForDisplay(policyAirParameterGroupItem);
				policyAirParameterGroupItemLanguage.PolicyGroupName = policyGroup.PolicyGroupName;
				policyAirParameterGroupItemLanguage.PolicyGroupId = policyAirParameterGroupItem.PolicyGroupId;
			}

		}


		//Add to DB
		public void Add(PolicyAirParameterGroupItemLanguage policyAirParameterGroupItemLanguage)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_InsertPolicyAirParameterGroupItemLanguage_v1(
				policyAirParameterGroupItemLanguage.PolicyAirParameterGroupItemId,
				policyAirParameterGroupItemLanguage.LanguageCode,
				policyAirParameterGroupItemLanguage.Translation,
				adminUserGuid
				);

		}

		//Delete From DB
		public void Delete(PolicyAirParameterGroupItemLanguage policyAirParameterGroupItemLanguage)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeletePolicyAirParameterGroupItemLanguage_v1(
			   policyAirParameterGroupItemLanguage.PolicyAirParameterGroupItemId,
			   policyAirParameterGroupItemLanguage.LanguageCode,
			   adminUserGuid,
			   policyAirParameterGroupItemLanguage.VersionNumber
			   );
		}

		//Change the deleted status on an item
		public void Update(PolicyAirParameterGroupItemLanguage policyAirParameterGroupItemLanguage)
		{

			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			db.spDesktopDataAdmin_UpdatePolicyAirParameterGroupItemLanguage_v1(
				policyAirParameterGroupItemLanguage.PolicyAirParameterGroupItemId,
				policyAirParameterGroupItemLanguage.LanguageCode,
				policyAirParameterGroupItemLanguage.Translation,
				adminUserGuid,
				policyAirParameterGroupItemLanguage.VersionNumber
				);

		}
	}
}
