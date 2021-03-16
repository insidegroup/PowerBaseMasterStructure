using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Linq.Dynamic;

namespace CWTDesktopDatabase.Repository
{
    public class PolicyHotelCapRateGroupItemLanguageRepository
    {
        private PolicyHotelCapRateGroupItemLanguageDC db = new PolicyHotelCapRateGroupItemLanguageDC(Settings.getConnectionString());

        //Get a Page of HotelCapRateAdvice - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyHotelCapRateGroupItemHotelCapRateAdvice_v1Result> PagePolicyHotelCapRateGroupItemHotelCapRateAdvice(int policyHotelCapRateGroupItemId, int page, string sortField, int sortOrder)
        {
            //get a page of records
            var result = db.spDesktopDataAdmin_SelectPolicyHotelCapRateGroupItemHotelCapRateAdvice_v1(policyHotelCapRateGroupItemId, sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPolicyHotelCapRateGroupItemHotelCapRateAdvice_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get one Item
        public PolicyHotelCapRateGroupItemLanguage GetItem(int policyHotelCapRateItemId, string languageCode)
        {
            return db.PolicyHotelCapRateGroupItemLanguages.SingleOrDefault(c => (c.PolicyHotelCapRateItemId == policyHotelCapRateItemId)
                    && (c.LanguageCode == languageCode));
        }

        //Get Items
        public List<PolicyHotelCapRateGroupItemLanguage> GetItems(int policyHotelCapRateItemId)
        {
            return db.PolicyHotelCapRateGroupItemLanguages.Where(c => c.PolicyHotelCapRateItemId == policyHotelCapRateItemId).ToList();
        }

        //Add Data From Linked Tables for Display
        public void EditItemForDisplay(PolicyHotelCapRateGroupItemLanguage policyHotelCapRateGroupItemLanguage)
        {
            //Add LanguageName
            if (policyHotelCapRateGroupItemLanguage.LanguageCode != null)
            {
                LanguageRepository languageRepository = new LanguageRepository();
                Language language = new Language();
                language = languageRepository.GetLanguage(policyHotelCapRateGroupItemLanguage.LanguageCode);
                if (language != null)
                {
                    policyHotelCapRateGroupItemLanguage.LanguageName = language.LanguageName;
                }
            }

            //Add PolicyGroupName
            PolicyHotelCapRateGroupItemRepository policyHotelCapRateGroupItemRepository = new PolicyHotelCapRateGroupItemRepository();
            PolicyHotelCapRateGroupItem policyHotelCapRateGroupItem = new PolicyHotelCapRateGroupItem();
            policyHotelCapRateGroupItem = policyHotelCapRateGroupItemRepository.GetPolicyHotelCapRateGroupItem(policyHotelCapRateGroupItemLanguage.PolicyHotelCapRateItemId);


            if (policyHotelCapRateGroupItem != null)
            {
                policyHotelCapRateGroupItemRepository.EditItemForDisplay(policyHotelCapRateGroupItem);
                policyHotelCapRateGroupItemLanguage.PolicyGroupName = policyHotelCapRateGroupItem.PolicyGroupName;
                policyHotelCapRateGroupItemLanguage.PolicyGroupId = policyHotelCapRateGroupItem.PolicyGroupId;
            }

        }


        //Languages not used by this item
        public List<Language> GetUnUsedLanguages(int policyHotelCapRateGroupItemId)
        {

            var result = from n in db.spDesktopDataAdmin_SelectPolicyHotelCapRateGroupItemAvailableLanguages_v1(policyHotelCapRateGroupItemId)
                         select new Language
                         {
                             LanguageName = n.LanguageName,
                             LanguageCode = n.LanguageCode
                         };
            return result.ToList();
        }


        //Add to DB
        public void Add(PolicyHotelCapRateGroupItemLanguage policyHotelCapRateGroupItemLanguage)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertPolicyHotelCapRateGroupItemHotelCapRateAdvice_v1(
                policyHotelCapRateGroupItemLanguage.PolicyHotelCapRateItemId,
                policyHotelCapRateGroupItemLanguage.LanguageCode,
                policyHotelCapRateGroupItemLanguage.HotelCapRateAdvice,
                adminUserGuid
                );

        }

        //Delete From DB
        public void Delete(PolicyHotelCapRateGroupItemLanguage policyHotelCapRateGroupItemLanguage)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeletePolicyHotelCapRateGroupItemHotelCapRateAdvice_v1(
               policyHotelCapRateGroupItemLanguage.PolicyHotelCapRateItemId,
               policyHotelCapRateGroupItemLanguage.LanguageCode,
               adminUserGuid,
               policyHotelCapRateGroupItemLanguage.VersionNumber
               );
        }

        //Change the deleted status on an item
        public void Update(PolicyHotelCapRateGroupItemLanguage policyHotelCapRateGroupItemLanguage)
        {

            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            db.spDesktopDataAdmin_UpdatePolicyHotelCapRateGroupItemHotelCapRateAdvice_v1(
                policyHotelCapRateGroupItemLanguage.PolicyHotelCapRateItemId,
                policyHotelCapRateGroupItemLanguage.LanguageCode,
                policyHotelCapRateGroupItemLanguage.HotelCapRateAdvice,
                adminUserGuid,
                policyHotelCapRateGroupItemLanguage.VersionNumber
                );

        }

        //Save DB Changes
        public void Save()
        {
            db.SubmitChanges();
        }
    }
}
