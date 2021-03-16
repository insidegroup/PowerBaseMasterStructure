using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Linq.Dynamic;

namespace CWTDesktopDatabase.Repository
{
    public class PolicyHotelVendorGroupItemLanguageRepository
    {
        private PolicyHotelVendorGroupItemLanguageDC db = new PolicyHotelVendorGroupItemLanguageDC(Settings.getConnectionString());

        //Get a Page of Hotel Advice - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyHotelVendorGroupItemHotelAdvice_v1Result> PagePolicyHotelVendorGroupItemHotelAdvice(int policyHotelPropertyGroupItemId, int page, string sortField, int sortOrder)
        {
            //get a page of records
            var result = db.spDesktopDataAdmin_SelectPolicyHotelVendorGroupItemHotelAdvice_v1(policyHotelPropertyGroupItemId, sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPolicyHotelVendorGroupItemHotelAdvice_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get one Item
        public PolicyHotelVendorGroupItemLanguage GetItem(int policyHotelVendorGroupItemId, string languageCode)
        {
            return db.PolicyHotelVendorGroupItemLanguages.SingleOrDefault(c => (c.PolicyHotelVendorGroupItemId == policyHotelVendorGroupItemId)
                    && (c.LanguageCode == languageCode));
        }

        //Add Data From Linked Tables for Display
        public void EditItemForDisplay(PolicyHotelVendorGroupItemLanguage policyHotelVendorGroupItemLanguage)
        {
            //Add LanguageName
            if (policyHotelVendorGroupItemLanguage.LanguageCode != null)
            {
                LanguageRepository languageRepository = new LanguageRepository();
                Language language = new Language();
                language = languageRepository.GetLanguage(policyHotelVendorGroupItemLanguage.LanguageCode);
                if (language != null)
                {
                    policyHotelVendorGroupItemLanguage.LanguageName = language.LanguageName;
                }
            }

            //Add PolicyGroupName
            PolicyHotelVendorGroupItemRepository policyHotelVendorGroupItemRepository = new PolicyHotelVendorGroupItemRepository();
            PolicyHotelVendorGroupItem policyHotelVendorGroupItem = new PolicyHotelVendorGroupItem();
            policyHotelVendorGroupItem = policyHotelVendorGroupItemRepository.GetPolicyHotelVendorGroupItem(policyHotelVendorGroupItemLanguage.PolicyHotelVendorGroupItemId);


            if (policyHotelVendorGroupItem != null)
            {
                policyHotelVendorGroupItemRepository.EditItemForDisplay(policyHotelVendorGroupItem);
                policyHotelVendorGroupItemLanguage.PolicyGroupName = policyHotelVendorGroupItem.PolicyGroupName;
                policyHotelVendorGroupItemLanguage.PolicyGroupId = policyHotelVendorGroupItem.PolicyGroupId;
            }

        }

        //Languages not used by this policyHotelVendorGroupItem
        public List<Language> GetUnUsedLanguages(int policyHotelVendorGroupItemId)
        {

            var result = from n in db.spDesktopDataAdmin_SelectPolicyHotelVendorGroupItemAvailableLanguages_v1(policyHotelVendorGroupItemId)
                         select new Language
                         {
                             LanguageName = n.LanguageName,
                             LanguageCode = n.LanguageCode
                         };
            return result.ToList();
        }

        //Add to DB
        public void Add(PolicyHotelVendorGroupItemLanguage policyHotelVendorGroupItemLanguage)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertPolicyHotelVendorGroupItemHotelAdvice_v1(
                policyHotelVendorGroupItemLanguage.PolicyHotelVendorGroupItemId,
                policyHotelVendorGroupItemLanguage.LanguageCode,
                policyHotelVendorGroupItemLanguage.HotelAdvice,
                adminUserGuid
                );

        }

        //Delete From DB
        public void Delete(PolicyHotelVendorGroupItemLanguage policyHotelVendorGroupItemLanguage)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeletePolicyHotelVendorGroupItemHotelAdvice_v1(
               policyHotelVendorGroupItemLanguage.PolicyHotelVendorGroupItemId,
               policyHotelVendorGroupItemLanguage.LanguageCode,
               adminUserGuid,
               policyHotelVendorGroupItemLanguage.VersionNumber
               );
        }

        //Change the deleted status on an item
        public void Update(PolicyHotelVendorGroupItemLanguage policyHotelVendorGroupItemLanguage)
        {

            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            db.spDesktopDataAdmin_UpdatePolicyHotelVendorGroupItemHotelAdvice_v1(
                policyHotelVendorGroupItemLanguage.PolicyHotelVendorGroupItemId,
                policyHotelVendorGroupItemLanguage.LanguageCode,
                policyHotelVendorGroupItemLanguage.HotelAdvice,
                adminUserGuid,
                policyHotelVendorGroupItemLanguage.VersionNumber
                );

        }

    }
}
