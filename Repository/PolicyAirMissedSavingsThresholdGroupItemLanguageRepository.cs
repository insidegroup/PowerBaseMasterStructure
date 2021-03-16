using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Linq.Dynamic;

namespace CWTDesktopDatabase.Repository
{
    public class PolicyAirMissedSavingsThresholdGroupItemLanguageRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get a Page of AirlineAdvice - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyAirMissedSavingsThresholdGroupItemMissedSavingsAdvice_v1Result> PagePolicyAirMissedSavingsThresholdGroupItemMissedSavingsAdvice(int policyAirMissedSavingsThresholdGroupItemId, int page, string sortField, int sortOrder)
        {
            //get a page of records
            var result = db.spDesktopDataAdmin_SelectPolicyAirMissedSavingsThresholdGroupItemMissedSavingsAdvice_v1(policyAirMissedSavingsThresholdGroupItemId, sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPolicyAirMissedSavingsThresholdGroupItemMissedSavingsAdvice_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        
        //Get one Item
        public PolicyAirMissedSavingsThresholdGroupItemLanguage GetItem(int policyAirMissedSavingsThresholdGroupItemId, string languageCode)
        {
            return db.PolicyAirMissedSavingsThresholdGroupItemLanguages.SingleOrDefault(c => (c.PolicyAirMissedSavingsThresholdGroupItemId == policyAirMissedSavingsThresholdGroupItemId)
                    && (c.LanguageCode == languageCode));
        }

        //Languages not used by this policyAirMissedSavingsThresholdGroupItem
        public List<Language> GetUnUsedLanguages(int policyAirMissedSavingsThresholdGroupItemId)
        {

            var result = from n in db.spDesktopDataAdmin_SelectPolicyAirMissedSavingsThresholdGroupItemAvailableLanguages_v1(policyAirMissedSavingsThresholdGroupItemId)
                         select new Language
                         {
                             LanguageName = n.LanguageName,
                             LanguageCode = n.LanguageCode
                         };
            return result.ToList();
        }

        //Add Data From Linked Tables for Display
        public void EditItemForDisplay(PolicyAirMissedSavingsThresholdGroupItemLanguage policyAirMissedSavingsThresholdGroupItemLanguage)
        {
            //Add LanguageName
            if (policyAirMissedSavingsThresholdGroupItemLanguage.LanguageCode != null)
            {
                LanguageRepository languageRepository = new LanguageRepository();
                Language language = new Language();
                language = languageRepository.GetLanguage(policyAirMissedSavingsThresholdGroupItemLanguage.LanguageCode);
                if (language != null)
                {
                    policyAirMissedSavingsThresholdGroupItemLanguage.LanguageName = language.LanguageName;
                }
            }

            //Add PolicyGroupName
            PolicyAirMissedSavingsThresholdGroupItemRepository policyAirMissedSavingsThresholdGroupItemRepository = new PolicyAirMissedSavingsThresholdGroupItemRepository();
            PolicyAirMissedSavingsThresholdGroupItem policyAirMissedSavingsThresholdGroupItem = new PolicyAirMissedSavingsThresholdGroupItem();
            policyAirMissedSavingsThresholdGroupItem = policyAirMissedSavingsThresholdGroupItemRepository.GetPolicyAirMissedSavingsThresholdGroupItem(policyAirMissedSavingsThresholdGroupItemLanguage.PolicyAirMissedSavingsThresholdGroupItemId);


            if (policyAirMissedSavingsThresholdGroupItem != null)
            {
                PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();
                PolicyGroup policyGroup = new PolicyGroup();
                policyGroup = policyGroupRepository.GetGroup(policyAirMissedSavingsThresholdGroupItem.PolicyGroupId);

                //policyAirMissedSavingsThresholdGroupItemRepository.EditItemForDisplay(policyAirMissedSavingsThresholdGroupItem);
                policyAirMissedSavingsThresholdGroupItemLanguage.PolicyGroupName = policyGroup.PolicyGroupName;
                policyAirMissedSavingsThresholdGroupItemLanguage.PolicyGroupId = policyAirMissedSavingsThresholdGroupItem.PolicyGroupId;
            }

        }


        //Add to DB
        public void Add(PolicyAirMissedSavingsThresholdGroupItemLanguage policyAirMissedSavingsThresholdGroupItemLanguage)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertPolicyAirMissedSavingsThresholdGroupItemMissedSavingsAdvice_v1(
                policyAirMissedSavingsThresholdGroupItemLanguage.PolicyAirMissedSavingsThresholdGroupItemId,
                policyAirMissedSavingsThresholdGroupItemLanguage.LanguageCode,
                policyAirMissedSavingsThresholdGroupItemLanguage.MissedSavingsAdvice,
                adminUserGuid
                );

        }

        //Delete From DB
        public void Delete(PolicyAirMissedSavingsThresholdGroupItemLanguage policyAirMissedSavingsThresholdGroupItemLanguage)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeletePolicyAirMissedSavingsThresholdGroupItemMissedSavingsAdvice_v1(
               policyAirMissedSavingsThresholdGroupItemLanguage.PolicyAirMissedSavingsThresholdGroupItemId,
               policyAirMissedSavingsThresholdGroupItemLanguage.LanguageCode,
               adminUserGuid,
               policyAirMissedSavingsThresholdGroupItemLanguage.VersionNumber
               );
        }

        //Change the deleted status on an item
        public void Update(PolicyAirMissedSavingsThresholdGroupItemLanguage policyAirMissedSavingsThresholdGroupItemLanguage)
        {

            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            db.spDesktopDataAdmin_UpdatePolicyAirMissedSavingsThresholdGroupItemMissedSavingsAdvice_v1(
                policyAirMissedSavingsThresholdGroupItemLanguage.PolicyAirMissedSavingsThresholdGroupItemId,
                policyAirMissedSavingsThresholdGroupItemLanguage.LanguageCode,
                policyAirMissedSavingsThresholdGroupItemLanguage.MissedSavingsAdvice,
                adminUserGuid,
                policyAirMissedSavingsThresholdGroupItemLanguage.VersionNumber
                );

        }
    }
}
