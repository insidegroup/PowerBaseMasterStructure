using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Linq.Dynamic;

namespace CWTDesktopDatabase.Repository
{
    public class PolicyCountryGroupItemLanguageRepository
    {
        private PolicyCountryGroupItemLanguageDC db = new PolicyCountryGroupItemLanguageDC(Settings.getConnectionString());

        //Get a Page of AirlineAdvice - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyCountryGroupItemCountryAdvice_v1Result> PagePolicyCountryGroupItemCountryAdvice(int policyAirVendorGroupItemId, int page, string sortField, int sortOrder)
        {
            //get a page of records
            var result = db.spDesktopDataAdmin_SelectPolicyCountryGroupItemCountryAdvice_v1(policyAirVendorGroupItemId, sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPolicyCountryGroupItemCountryAdvice_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }


        //Get one Item
        public PolicyCountryGroupItemLanguage GetItem(int policyCountryGroupItemId, string languageCode)
        {
            return db.PolicyCountryGroupItemLanguages.SingleOrDefault(c => (c.PolicyCountryGroupItemId == policyCountryGroupItemId)
                    && (c.LanguageCode == languageCode));
        }

        //Get Items
        public List<PolicyCountryGroupItemLanguage> GetItems(int policyCountryGroupItemId)
        {
            return db.PolicyCountryGroupItemLanguages.Where(c => c.PolicyCountryGroupItemId == policyCountryGroupItemId).ToList();
        }

        //Languages not used by this policyCountryGroupItem
        public List<Language> GetUnUsedLanguages(int policyCountryGroupItemId)
        {

            var result = from n in db.spDesktopDataAdmin_SelectPolicyCountryGroupItemAvailableLanguages_v1(policyCountryGroupItemId)
                         select new Language
                         {
                             LanguageName = n.LanguageName,
                             LanguageCode = n.LanguageCode
                         };
            return result.ToList();
        }

        //Add Data From Linked Tables for Display
        public void EditItemForDisplay(PolicyCountryGroupItemLanguage policyCountryGroupItemLanguage)
        {
            //Add LanguageName
            if (policyCountryGroupItemLanguage.LanguageCode != null)
            {
                LanguageRepository languageRepository = new LanguageRepository();
                Language language = new Language();
                language = languageRepository.GetLanguage(policyCountryGroupItemLanguage.LanguageCode);
                if (language != null)
                {
                    policyCountryGroupItemLanguage.LanguageName = language.LanguageName;
                }
            }

            //Add PolicyGroupName
            PolicyCountryGroupItemRepository policyCountryGroupItemRepository = new PolicyCountryGroupItemRepository();
            PolicyCountryGroupItem policyCountryGroupItem = new PolicyCountryGroupItem();
            policyCountryGroupItem = policyCountryGroupItemRepository.GetPolicyCountryGroupItem(policyCountryGroupItemLanguage.PolicyCountryGroupItemId);


            if (policyCountryGroupItem != null)
            {
                policyCountryGroupItemRepository.EditItemForDisplay(policyCountryGroupItem);
                policyCountryGroupItemLanguage.PolicyGroupName = policyCountryGroupItem.PolicyGroupName;
                policyCountryGroupItemLanguage.PolicyGroupId = policyCountryGroupItem.PolicyGroupId;
            }

        }

        //Add to DB
        public void Add(PolicyCountryGroupItemLanguage policyCountryGroupItemLanguage)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertPolicyCountryGroupItemCountryAdvice_v1(
                policyCountryGroupItemLanguage.PolicyCountryGroupItemId,
                policyCountryGroupItemLanguage.LanguageCode,
                policyCountryGroupItemLanguage.CountryAdvice,
                adminUserGuid
                );

        }

        //Delete From DB
        public void Delete(PolicyCountryGroupItemLanguage policyCountryGroupItemLanguage)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeletePolicyCountryGroupItemCountryAdvice_v1(
               policyCountryGroupItemLanguage.PolicyCountryGroupItemId,
               policyCountryGroupItemLanguage.LanguageCode,
               adminUserGuid,
               policyCountryGroupItemLanguage.VersionNumber
               );
        }

        //Change the deleted status on an item
        public void Update(PolicyCountryGroupItemLanguage policyCountryGroupItemLanguage)
        {

            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            db.spDesktopDataAdmin_UpdatePolicyCountryGroupItemCountryAdvice_v1(
                policyCountryGroupItemLanguage.PolicyCountryGroupItemId,
                policyCountryGroupItemLanguage.LanguageCode,
                policyCountryGroupItemLanguage.CountryAdvice,
                adminUserGuid,
                policyCountryGroupItemLanguage.VersionNumber
                );

        }
    }
}
