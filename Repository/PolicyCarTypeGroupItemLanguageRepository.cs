using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Linq.Dynamic;

namespace CWTDesktopDatabase.Repository
{
    public class PolicyCarTypeGroupItemLanguageRepository
    {
        private PolicyCarTypeGroupItemLanguageDC db = new PolicyCarTypeGroupItemLanguageDC(Settings.getConnectionString());

        //Get a Page of Car Type Advice - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyCarTypeGroupItemCarTypeAdvice_v1Result> PagePolicyCarTypeGroupItemCarTypeAdvice(int policyAirVendorGroupItemId, int page, string sortField, int sortOrder)
        {
            //get a page of records
            var result = db.spDesktopDataAdmin_SelectPolicyCarTypeGroupItemCarTypeAdvice_v1(policyAirVendorGroupItemId, sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPolicyCarTypeGroupItemCarTypeAdvice_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get one Item
        public PolicyCarTypeGroupItemLanguage GetItem(int policyCarTypeGroupItemId, string languageCode)
        {
            return db.PolicyCarTypeGroupItemLanguages.SingleOrDefault(c => (c.PolicyCarTypeGroupItemId == policyCarTypeGroupItemId)
                    && (c.LanguageCode == languageCode));
        }

        //Add Data From Linked Tables for Display
        public void EditItemForDisplay(PolicyCarTypeGroupItemLanguage policyCarTypeGroupItemLanguage)
        {
            //Add LanguageName
            if (policyCarTypeGroupItemLanguage.LanguageCode != null)
            {
                LanguageRepository languageRepository = new LanguageRepository();
                Language language = new Language();
                language = languageRepository.GetLanguage(policyCarTypeGroupItemLanguage.LanguageCode);
                if (language != null)
                {
                    policyCarTypeGroupItemLanguage.LanguageName = language.LanguageName;
                }
            }

            //Add PolicyGroupName
            PolicyCarTypeGroupItemRepository policyCarTypeGroupItemRepository = new PolicyCarTypeGroupItemRepository();
            PolicyCarTypeGroupItem policyCarTypeGroupItem = new PolicyCarTypeGroupItem();
            policyCarTypeGroupItem = policyCarTypeGroupItemRepository.GetPolicyCarTypeGroupItem(policyCarTypeGroupItemLanguage.PolicyCarTypeGroupItemId);


            if (policyCarTypeGroupItem != null)
            {
                policyCarTypeGroupItemRepository.EditItemForDisplay(policyCarTypeGroupItem);
                policyCarTypeGroupItemLanguage.PolicyGroupName = policyCarTypeGroupItem.PolicyGroupName;
                policyCarTypeGroupItemLanguage.PolicyGroupId = policyCarTypeGroupItem.PolicyGroupId;
            }

        }

        //Languages not used by this policyCarTypeGroupItem
        public List<Language> GetUnUsedLanguages(int policyCarTypeGroupItemId)
        {

            var result = from n in db.spDesktopDataAdmin_SelectPolicyCarTypeGroupItemAvailableLanguages_v1(policyCarTypeGroupItemId)
                         select new Language
                         {
                             LanguageName = n.LanguageName,
                             LanguageCode = n.LanguageCode
                         };
            return result.ToList();
        }

        //Add to DB
        public void Add(PolicyCarTypeGroupItemLanguage policyCarTypeGroupItemLanguage)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertPolicyCarTypeGroupItemCarTypeAdvice_v1(
                policyCarTypeGroupItemLanguage.PolicyCarTypeGroupItemId,
                policyCarTypeGroupItemLanguage.LanguageCode,
                policyCarTypeGroupItemLanguage.CarTypeAdvice,
                adminUserGuid
                );

        }

        //Delete From DB
        public void Delete(PolicyCarTypeGroupItemLanguage policyCarTypeGroupItemLanguage)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeletePolicyCarTypeGroupItemCarTypeAdvice_v1(
               policyCarTypeGroupItemLanguage.PolicyCarTypeGroupItemId,
               policyCarTypeGroupItemLanguage.LanguageCode,
               adminUserGuid,
               policyCarTypeGroupItemLanguage.VersionNumber
               );
        }

        //Change the deleted status on an item
        public void Update(PolicyCarTypeGroupItemLanguage policyCarTypeGroupItemLanguage)
        {

            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            db.spDesktopDataAdmin_UpdatePolicyCarTypeGroupItemCarTypeAdvice_v1(
                policyCarTypeGroupItemLanguage.PolicyCarTypeGroupItemId,
                policyCarTypeGroupItemLanguage.LanguageCode,
                policyCarTypeGroupItemLanguage.CarTypeAdvice,
                adminUserGuid,
                policyCarTypeGroupItemLanguage.VersionNumber
                );

        }
    }
}
