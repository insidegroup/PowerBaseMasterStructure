using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Linq.Dynamic;

namespace CWTDesktopDatabase.Repository
{
    public class PolicyAirVendorGroupItemLanguageRepository
    {
        private PolicyAirVendorGroupItemLanguageDC db = new PolicyAirVendorGroupItemLanguageDC(Settings.getConnectionString());

        //Get a Page of AirlineAdvice - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyAirVendorGroupItemAirVendorAdvice_v1Result> PagePolicyAirVendorGroupItemAirlineAdvice(int policyAirVendorGroupItemId, int page, string sortField, int sortOrder)
        {
            //get a page of records
            var result = db.spDesktopDataAdmin_SelectPolicyAirVendorGroupItemAirVendorAdvice_v1(policyAirVendorGroupItemId, sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPolicyAirVendorGroupItemAirVendorAdvice_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get one Item
        public PolicyAirVendorGroupItemLanguage GetItem(int policyAirVendorGroupItemId, string languageCode)
        {
            return db.PolicyAirVendorGroupItemLanguages.SingleOrDefault(c => (c.PolicyAirVendorGroupItemId == policyAirVendorGroupItemId)
                    && (c.LanguageCode == languageCode));
        }

        //Get Items
        public List<PolicyAirVendorGroupItemLanguage> GetItems(int policyAirVendorGroupItemId)
        {
            return db.PolicyAirVendorGroupItemLanguages.Where(c => c.PolicyAirVendorGroupItemId == policyAirVendorGroupItemId).ToList();
        }

        //Add Data From Linked Tables for Display
        public void EditItemForDisplay(PolicyAirVendorGroupItemLanguage policyAirVendorGroupItemLanguage)
        {
            //Add LanguageName
            if (policyAirVendorGroupItemLanguage.LanguageCode != null)
            {
                LanguageRepository languageRepository = new LanguageRepository();
                Language language = new Language();
                language = languageRepository.GetLanguage(policyAirVendorGroupItemLanguage.LanguageCode);
                if (language != null)
                {
                    policyAirVendorGroupItemLanguage.LanguageName = language.LanguageName;
                }
            }

            //Add PolicyGroup Information
            PolicyAirVendorGroupItemRepository policyAirVendorGroupItemRepository = new PolicyAirVendorGroupItemRepository();
            PolicyAirVendorGroupItem policyAirVendorGroupItem = new PolicyAirVendorGroupItem();
            policyAirVendorGroupItem = policyAirVendorGroupItemRepository.GetPolicyAirVendorGroupItem(policyAirVendorGroupItemLanguage.PolicyAirVendorGroupItemId);
            
            
            if (policyAirVendorGroupItem != null)
            {
                policyAirVendorGroupItemRepository.EditItemForDisplay(policyAirVendorGroupItem);
                policyAirVendorGroupItemLanguage.PolicyGroupName = policyAirVendorGroupItem.PolicyGroupName;
                policyAirVendorGroupItemLanguage.PolicyGroupId = policyAirVendorGroupItem.PolicyGroupId;
            }

        }

        //Languages not used by this policyAirVendorGroupItem
        public List<Language> GetUnUsedLanguages(int policyAirVendorGroupItemId)
        {

            var result = from n in db.spDesktopDataAdmin_SelectPolicyAirVendorGroupItemAvailableLanguages_v1(policyAirVendorGroupItemId)
                         select new Language{
                             LanguageName = n.LanguageName,
                             LanguageCode = n.LanguageCode
                         };
            return result.ToList();
        }

        //Add to DB
        public void Add(PolicyAirVendorGroupItemLanguage policyAirVendorGroupItemLanguage)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertPolicyAirVendorGroupItemAirlineAdvice_v1(
                policyAirVendorGroupItemLanguage.PolicyAirVendorGroupItemId,
                policyAirVendorGroupItemLanguage.LanguageCode,
                policyAirVendorGroupItemLanguage.AirlineAdvice,
                adminUserGuid
                );

        }
       
        //Delete From DB
        public void Delete(PolicyAirVendorGroupItemLanguage policyAirVendorGroupItemLanguage)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeletePolicyAirVendorGroupItemAirlineAdvice_v1(
               policyAirVendorGroupItemLanguage.PolicyAirVendorGroupItemId,
               policyAirVendorGroupItemLanguage.LanguageCode,
               adminUserGuid,
               policyAirVendorGroupItemLanguage.VersionNumber
               );
        }

        //Change the deleted status on an item
        public void Update(PolicyAirVendorGroupItemLanguage policyAirVendorGroupItemLanguage)
        {

            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            db.spDesktopDataAdmin_UpdatePolicyAirVendorGroupItemAirlineAdvice_v1(
                policyAirVendorGroupItemLanguage.PolicyAirVendorGroupItemId,
                policyAirVendorGroupItemLanguage.LanguageCode,
                policyAirVendorGroupItemLanguage.AirlineAdvice,
                adminUserGuid,
                policyAirVendorGroupItemLanguage.VersionNumber
                );

        }

    }
}
