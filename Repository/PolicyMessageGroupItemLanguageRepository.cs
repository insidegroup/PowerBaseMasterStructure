using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.Repository
{
    public class PolicyMessageGroupItemLanguageRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get a Page of PolicyMessage Translations - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyMessageGroupItemLanguage_v1Result> PagePolicyMessageGroupItemLanguages(int policyMessageGroupItemId, int page, string sortField, int sortOrder)
        {
            //get a page of records
            var result = db.spDesktopDataAdmin_SelectPolicyMessageGroupItemLanguage_v1(policyMessageGroupItemId, sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPolicyMessageGroupItemLanguage_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get one Item
        public PolicyMessageGroupItemLanguage GetItem(int policyMessageGroupItemId, string languageCode)
        {
            return db.PolicyMessageGroupItemLanguages.SingleOrDefault(c => (c.PolicyMessageGroupItemId == policyMessageGroupItemId)
                    && (c.LanguageCode == languageCode));
        }

        //Languages not used by this queueMinderItem
        public List<Language> GetUnUsedLanguages(int policyMessageGroupItemId)
        {
            var result = from n in db.spDesktopDataAdmin_SelectPolicyMessageGroupItemAvailableLanguages_v1(policyMessageGroupItemId)
                         select new Language
                         {
                             LanguageName = n.LanguageName,
                             LanguageCode = n.LanguageCode
                         };
            return result.ToList();
        }

        //Add to DB
        public void Add(PolicyMessageGroupItemLanguage policyMessageGroupItemLanguage)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertPolicyMessageGroupItemLanguage_v1(
                policyMessageGroupItemLanguage.PolicyMessageGroupItemId,
                policyMessageGroupItemLanguage.LanguageCode,
                policyMessageGroupItemLanguage.PolicyMessageGroupItemTranslation,
                policyMessageGroupItemLanguage.PolicyMessageGroupItemTranslationMarkdown,
                adminUserGuid
                );

        }

        //Delete From DB
        public void Delete(PolicyMessageGroupItemLanguage policyMessageGroupItemLanguage)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeletePolicyMessageGroupItemLanguage_v1(
               policyMessageGroupItemLanguage.PolicyMessageGroupItemId,
               policyMessageGroupItemLanguage.LanguageCode,
               adminUserGuid,
               policyMessageGroupItemLanguage.VersionNumber
               );
        }

        //Change the deleted status on an item
        public void Update(PolicyMessageGroupItemLanguage policyMessageGroupItemLanguage)
        {

            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            db.spDesktopDataAdmin_UpdatePolicyMessageGroupItemLanguage_v1(
                policyMessageGroupItemLanguage.PolicyMessageGroupItemId,
                policyMessageGroupItemLanguage.LanguageCode,
                policyMessageGroupItemLanguage.PolicyMessageGroupItemTranslation,
                policyMessageGroupItemLanguage.PolicyMessageGroupItemTranslationMarkdown,
                adminUserGuid,
                policyMessageGroupItemLanguage.VersionNumber
                );

        }
    }
}