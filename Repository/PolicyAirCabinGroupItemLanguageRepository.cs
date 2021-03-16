using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using System.Linq.Dynamic;
using System.Web.Security;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.Repository
{
    public class PolicyAirCabinGroupItemLanguageRepository
    {
        private PolicyAirCabinGroupItemDC db = new PolicyAirCabinGroupItemDC(Settings.getConnectionString());

        //Get a Page of AirCabinAdvice - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyAirCabinGroupItemAirCabinAdvice_v1Result> PagePolicyAirCabinGroupItemAirCabinAdvice(int policyAirCabinGroupItemId, int page, string sortField, int sortOrder)
        {
            //get a page of records
            var result = db.spDesktopDataAdmin_SelectPolicyAirCabinGroupItemAirCabinAdvice_v1(policyAirCabinGroupItemId, sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPolicyAirCabinGroupItemAirCabinAdvice_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get one Item
        public PolicyAirCabinGroupItemLanguage GetItem(int policyAirCabinGroupItemId, string languageCode)
        {
            return db.PolicyAirCabinGroupItemLanguages.SingleOrDefault(c => (c.PolicyAirCabinGroupItemId == policyAirCabinGroupItemId)
                    && (c.LanguageCode == languageCode));
        }

        //Add Data From Linked Tables for Display
        public void EditItemForDisplay(PolicyAirCabinGroupItemLanguage policyAirCabinGroupItemLanguage)
        {
            //Add LanguageName
            if (policyAirCabinGroupItemLanguage.LanguageCode != null)
            {
                LanguageRepository languageRepository = new LanguageRepository();
                Language language = new Language();
                language = languageRepository.GetLanguage(policyAirCabinGroupItemLanguage.LanguageCode);
                if (language != null)
                {
                    policyAirCabinGroupItemLanguage.LanguageName = language.LanguageName;
                }
            }

            //Add PolicyGroup Information
            PolicyAirCabinGroupItemRepository policyAirCabinGroupItemRepository = new PolicyAirCabinGroupItemRepository();
            PolicyAirCabinGroupItem policyAirCabinGroupItem = new PolicyAirCabinGroupItem();
            policyAirCabinGroupItem = policyAirCabinGroupItemRepository.GetPolicyAirCabinGroupItem(policyAirCabinGroupItemLanguage.PolicyAirCabinGroupItemId);


            if (policyAirCabinGroupItem != null)
            {
                policyAirCabinGroupItemRepository.EditItemForDisplay(policyAirCabinGroupItem);
                policyAirCabinGroupItemLanguage.PolicyGroupName = policyAirCabinGroupItem.PolicyGroupName;
                policyAirCabinGroupItemLanguage.PolicyGroupId = policyAirCabinGroupItem.PolicyGroupId;
            }

        }

        //Languages not used by this policyAirCabinGroupItem
        public List<Language> GetUnUsedLanguages(int policyAirCabinGroupItemId)
        {

            var result = from n in db.spDesktopDataAdmin_SelectPolicyAirCabinGroupItemAvailableLanguages_v1(policyAirCabinGroupItemId)
                         select new Language
                         {
                             LanguageName = n.LanguageName,
                             LanguageCode = n.LanguageCode
                         };
            return result.ToList();
        }

        //Add to DB
        public void Add(PolicyAirCabinGroupItemLanguage policyAirCabinGroupItemLanguage)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertPolicyAirCabinGroupItemAirCabinAdvice_v1(
                policyAirCabinGroupItemLanguage.PolicyAirCabinGroupItemId,
                policyAirCabinGroupItemLanguage.LanguageCode,
                policyAirCabinGroupItemLanguage.AirCabinAdvice,
                adminUserGuid
                );

        }


        //Delete From DB
        public void Delete(PolicyAirCabinGroupItemLanguage policyAirCabinGroupItemLanguage)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeletePolicyAirCabinGroupItemAirCabinAdvice_v1(
               policyAirCabinGroupItemLanguage.PolicyAirCabinGroupItemId,
               policyAirCabinGroupItemLanguage.LanguageCode,
               adminUserGuid,
               policyAirCabinGroupItemLanguage.VersionNumber
               );
        }

        //Change the deleted status on an item
        public void Update(PolicyAirCabinGroupItemLanguage policyAirCabinGroupItemLanguage)
        {

            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            db.spDesktopDataAdmin_UpdatePolicyAirCabinGroupItemAirCabinAdvice_v1(
                policyAirCabinGroupItemLanguage.PolicyAirCabinGroupItemId,
                policyAirCabinGroupItemLanguage.LanguageCode,
                policyAirCabinGroupItemLanguage.AirCabinAdvice,
                adminUserGuid,
                policyAirCabinGroupItemLanguage.VersionNumber
                );

        }

    }
}
