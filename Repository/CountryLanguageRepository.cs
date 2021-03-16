using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Repository
{
    public class CountryLanguageRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get a Page of Cities
        public CWTPaginatedList<spDesktopDataAdmin_SelectCountryLanguages_v1Result> PageCountryLanguages(string countryCode, int page, string sortField, int sortOrder)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectCountryLanguages_v1(countryCode, sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectCountryLanguages_v1Result>(result, page, Convert.ToInt32(totalRecords));
            return paginatedView;
        }

        //Get one Item
        public CountryLanguage GetItem(string countryCode, string languageCode)
        {
            return db.CountryLanguages.SingleOrDefault(c => (c.CountryCode == countryCode)
                    && (c.LanguageCode == languageCode));
        }

        //Add Data From Linked Tables for Display
        public void EditItemForDisplay(CountryLanguage countryLanguage)
        {
            //Add LanguageName
            if (countryLanguage.LanguageCode != null)
            {
                LanguageRepository languageRepository = new LanguageRepository();
                Language language = new Language();
                language = languageRepository.GetLanguage(countryLanguage.LanguageCode);
                if (language != null)
                {
                    countryLanguage.LanguageName = language.LanguageName;
                }
            }
        }

        //Languages not used by this policyAirVendorGroupItem
        public List<Language> GetUnUsedLanguages(string countryCode)
        {

            var result = from n in db.spDesktopDataAdmin_SelectCountryAvailableLanguages_v1(countryCode)
                         select new Language
                         {
                             LanguageName = n.LanguageName,
                             LanguageCode = n.LanguageCode
                         };
            return result.ToList();
        }

        //Add to DB
        public void Add(CountryLanguage countryLanguage)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertCountryLanguage_v1(
                countryLanguage.CountryCode,
                countryLanguage.LanguageCode,
                countryLanguage.CountryName,
                adminUserGuid
                );

        }

        //Change the deleted status on an item
        public void Update(CountryLanguage countryLanguage)
        {

            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            db.spDesktopDataAdmin_UpdateCountryLanguage_v1(
                countryLanguage.CountryCode,
                countryLanguage.LanguageCode,
                countryLanguage.CountryName,
                adminUserGuid,
                countryLanguage.VersionNumber
                );

        }

        //Delete From DB
        public void Delete(CountryLanguage countryLanguage)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeleteCountryLanguage_v1(
               countryLanguage.CountryCode,
               countryLanguage.LanguageCode,
               adminUserGuid,
               countryLanguage.VersionNumber
               );
        }

    }
}
