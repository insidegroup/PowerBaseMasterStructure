using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using System.Linq.Dynamic;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.Repository
{
    public class PolicyCarVendorGroupItemLanguageRepository
    {
        private PolicyCarVendorGroupItemLanguageDC db = new PolicyCarVendorGroupItemLanguageDC(Settings.getConnectionString());

        //Get a Page of AirlineAdvice - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyCarVendorGroupItemCarAdvice_v1Result> PagePolicyCarVendorGroupItemCarAdvice(int policyAirVendorGroupItemId, int page, string sortField, int sortOrder)
        {
            //get a page of records
            var result = db.spDesktopDataAdmin_SelectPolicyCarVendorGroupItemCarAdvice_v1(policyAirVendorGroupItemId, sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPolicyCarVendorGroupItemCarAdvice_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get one Item
        public PolicyCarVendorGroupItemLanguage GetItem(int policyCarVendorGroupItemId, string languageCode)
        {
            return db.PolicyCarVendorGroupItemLanguages.SingleOrDefault(c => (c.PolicyCarVendorGroupItemId == policyCarVendorGroupItemId)
                    && (c.LanguageCode == languageCode));
        }

        //Add Data From Linked Tables for Display
        public void EditItemForDisplay(PolicyCarVendorGroupItemLanguage policyCarVendorGroupItemLanguage)
        {
            //Add LanguageName
            
            if (policyCarVendorGroupItemLanguage.LanguageCode != null)
            {
                LanguageRepository languageRepository = new LanguageRepository();
                Language language = new Language();
                language = languageRepository.GetLanguage(policyCarVendorGroupItemLanguage.LanguageCode);
                if (language != null)
                {
                    policyCarVendorGroupItemLanguage.LanguageName = language.LanguageName;
                }
            }

            //Add PolicyGroupName
            PolicyCarVendorGroupItemRepository policyCarVendorGroupItemRepository = new PolicyCarVendorGroupItemRepository();
            PolicyCarVendorGroupItem policyCarVendorGroupItem = new PolicyCarVendorGroupItem();
            policyCarVendorGroupItem = policyCarVendorGroupItemRepository.GetPolicyCarVendorGroupItem(policyCarVendorGroupItemLanguage.PolicyCarVendorGroupItemId);


            if (policyCarVendorGroupItem != null)
            {
                policyCarVendorGroupItemRepository.EditItemForDisplay(policyCarVendorGroupItem);
                policyCarVendorGroupItemLanguage.PolicyGroupName = policyCarVendorGroupItem.PolicyGroupName;
                policyCarVendorGroupItemLanguage.PolicyGroupId = policyCarVendorGroupItem.PolicyGroupId;
            }

        }

        //Languages not used by this policyCarVendorGroupItem
        public List<Language> GetUnUsedLanguages(int policyCarVendorGroupItemId)
        {

            var result = from n in db.spDesktopDataAdmin_SelectPolicyCarVendorGroupItemAvailableLanguages_v1(policyCarVendorGroupItemId)
                         select new Language
                         {
                             LanguageName = n.LanguageName,
                             LanguageCode = n.LanguageCode
                         };
            return result.ToList();
        }

        //Add to DB
        public void Add(PolicyCarVendorGroupItemLanguage policyCarVendorGroupItemLanguage)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertPolicyCarVendorGroupItemCarAdvice_v1(
                policyCarVendorGroupItemLanguage.PolicyCarVendorGroupItemId,
                policyCarVendorGroupItemLanguage.LanguageCode,
                policyCarVendorGroupItemLanguage.CarAdvice,
                adminUserGuid
                );

        }

        //Delete From DB
        public void Delete(PolicyCarVendorGroupItemLanguage policyCarVendorGroupItemLanguage)
        {

            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            db.spDesktopDataAdmin_DeletePolicyCarVendorGroupItemCarAdvice_v1(
               policyCarVendorGroupItemLanguage.PolicyCarVendorGroupItemId,
               policyCarVendorGroupItemLanguage.LanguageCode,
               adminUserGuid,
               policyCarVendorGroupItemLanguage.VersionNumber
               );
        }

        //Change the deleted status on an item
        public void Update(PolicyCarVendorGroupItemLanguage policyCarVendorGroupItemLanguage)
        {

            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            db.spDesktopDataAdmin_UpdatePolicyCarVendorGroupItemCarAdvice_v1(
                policyCarVendorGroupItemLanguage.PolicyCarVendorGroupItemId,
                policyCarVendorGroupItemLanguage.LanguageCode,
                policyCarVendorGroupItemLanguage.CarAdvice,
                adminUserGuid,
                policyCarVendorGroupItemLanguage.VersionNumber
                );

        }

    }
}
