using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Repository
{
    public class PhraseTranslationRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get a Page of Cities
        public CWTPaginatedList<spDesktopDataAdmin_SelectPhraseTranslations_v1Result> PagePhraseTranslations(int phraseId, int page, string sortField, int sortOrder)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectPhraseTranslations_v1(phraseId, sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPhraseTranslations_v1Result>(result, page, Convert.ToInt32(totalRecords));
            return paginatedView;
        }

        //Get one Item
        public PhraseTranslation GetItem(int phraseId, string languageCode)
        {
            return db.PhraseTranslations.SingleOrDefault(c => (c.PhraseId == phraseId)
                    && (c.LanguageCode == languageCode));
        }

        //Add Data From Linked Tables for Display
        public void EditItemForDisplay(PhraseTranslation phraseTranslation)
        {
            //Add LanguageName
            if (phraseTranslation.LanguageCode != null)
            {
                LanguageRepository languageRepository = new LanguageRepository();
                Language language = new Language();
                language = languageRepository.GetLanguage(phraseTranslation.LanguageCode);
                if (language != null)
                {
                    phraseTranslation.LanguageName = language.LanguageName;
                }
            }
        }

        //Languages not used by this policyAirVendorGroupItem
        public List<Language> GetUnUsedLanguages(int phraseId)
        {

            var result = from n in db.spDesktopDataAdmin_SelectPhraseAvailableLanguages_v1(phraseId)
                         select new Language
                         {
                             LanguageName = n.LanguageName,
                             LanguageCode = n.LanguageCode
                         };
            return result.ToList();
        }

        //Add to DB
        public void Add(PhraseTranslation phraseTranslation)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertPhraseTranslation_v1(
                phraseTranslation.PhraseId,
                phraseTranslation.LanguageCode,
                phraseTranslation.PhraseTranslation1,
                adminUserGuid
                );

        }

        //Change the deleted status on an item
        public void Update(PhraseTranslation phraseTranslation)
        {

            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            db.spDesktopDataAdmin_UpdatePhraseTranslation_v1(
                phraseTranslation.PhraseId,
                phraseTranslation.LanguageCode,
                phraseTranslation.PhraseTranslation1,
                adminUserGuid,
                phraseTranslation.VersionNumber
                );

        }

        //Delete From DB
        public void Delete(PhraseTranslation phraseTranslation)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeletePhraseTranslation_v1(
               phraseTranslation.PhraseId,
               phraseTranslation.LanguageCode,
               adminUserGuid,
               phraseTranslation.VersionNumber
               );
        }

    }
}
