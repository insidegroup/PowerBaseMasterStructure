using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using System.Linq.Dynamic;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.Repository
{
    public class PolicyHotelPropertyGroupItemLanguageRepository
    {
        private PolicyHotelPropertyGroupItemLanguageDC db = new PolicyHotelPropertyGroupItemLanguageDC(Settings.getConnectionString());


        //Get a Page of Hotel Advice - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyHotelPropertyGroupItemHotelAdvice_v1Result> PagePolicyHotelPropertyGroupItemHotelAdvice(int policyHotelPropertyGroupItemId, int page, string sortField, int sortOrder)
        {
            //get a page of records
            var result = db.spDesktopDataAdmin_SelectPolicyHotelPropertyGroupItemHotelAdvice_v1(policyHotelPropertyGroupItemId, sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPolicyHotelPropertyGroupItemHotelAdvice_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }


        //Get one Item
        public PolicyHotelPropertyGroupItemLanguage GetItem(int policyHotelPropertyGroupItemId, string languageCode)
        {
            return db.PolicyHotelPropertyGroupItemLanguages.SingleOrDefault(c => (c.PolicyHotelPropertyGroupItemId == policyHotelPropertyGroupItemId)
                    && (c.LanguageCode == languageCode));
        }

        //Add Data From Linked Tables for Display
        public void EditItemForDisplay(PolicyHotelPropertyGroupItemLanguage policyHotelPropertyGroupItemLanguage)
        {
            //Add LanguageName
            if (policyHotelPropertyGroupItemLanguage.LanguageCode != null)
            {
                LanguageRepository languageRepository = new LanguageRepository();
                Language language = new Language();
                language = languageRepository.GetLanguage(policyHotelPropertyGroupItemLanguage.LanguageCode);
                if (language != null)
                {
                    policyHotelPropertyGroupItemLanguage.LanguageName = language.LanguageName;
                }
            }

            //Add PolicyGroupName
            PolicyHotelPropertyGroupItemRepository policyHotelPropertyGroupItemRepository = new PolicyHotelPropertyGroupItemRepository();
            PolicyHotelPropertyGroupItem policyHotelPropertyGroupItem = new PolicyHotelPropertyGroupItem();
            policyHotelPropertyGroupItem = policyHotelPropertyGroupItemRepository.GetPolicyHotelPropertyGroupItem(policyHotelPropertyGroupItemLanguage.PolicyHotelPropertyGroupItemId);


            if (policyHotelPropertyGroupItem != null)
            {
                policyHotelPropertyGroupItemRepository.EditItemForDisplay(policyHotelPropertyGroupItem);
                policyHotelPropertyGroupItemLanguage.PolicyGroupName = policyHotelPropertyGroupItem.PolicyGroupName;
                policyHotelPropertyGroupItemLanguage.PolicyGroupId = policyHotelPropertyGroupItem.PolicyGroupId;
            }

        }

        //Languages not used by this policyHotelPropertyGroupItem
        public List<Language> GetUnUsedLanguages(int policyHotelPropertyGroupItemId)
        {

            var result = from n in db.spDesktopDataAdmin_SelectPolicyHotelPropertyGroupItemAvailableLanguages_v1(policyHotelPropertyGroupItemId)
                         select new Language
                         {
                             LanguageName = n.LanguageName,
                             LanguageCode = n.LanguageCode
                         };
            return result.ToList();
        }

        //Add to DB
        public void Add(PolicyHotelPropertyGroupItemLanguage policyHotelPropertyGroupItemLanguage)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertPolicyHotelPropertyGroupItemHotelAdvice_v1(
                policyHotelPropertyGroupItemLanguage.PolicyHotelPropertyGroupItemId,
                policyHotelPropertyGroupItemLanguage.LanguageCode,
                policyHotelPropertyGroupItemLanguage.HotelAdvice,
                adminUserGuid
                );

        }

        //Delete From DB
        public void Delete(PolicyHotelPropertyGroupItemLanguage policyHotelPropertyGroupItemLanguage)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeletePolicyHotelPropertyGroupItemHotelAdvice_v1(
               policyHotelPropertyGroupItemLanguage.PolicyHotelPropertyGroupItemId,
               policyHotelPropertyGroupItemLanguage.LanguageCode,
               adminUserGuid,
               policyHotelPropertyGroupItemLanguage.VersionNumber
               );
        }

        //Change the deleted status on an item
        public void Update(PolicyHotelPropertyGroupItemLanguage policyHotelPropertyGroupItemLanguage)
        {

            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            db.spDesktopDataAdmin_UpdatePolicyHotelPropertyGroupItemHotelAdvice_v1(
                policyHotelPropertyGroupItemLanguage.PolicyHotelPropertyGroupItemId,
                policyHotelPropertyGroupItemLanguage.LanguageCode,
                policyHotelPropertyGroupItemLanguage.HotelAdvice,
                adminUserGuid,
                policyHotelPropertyGroupItemLanguage.VersionNumber
                );

        }

    }
}
