using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Repository
{
    public class CityLanguageRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get a Page of Cities
        public CWTPaginatedList<spDesktopDataAdmin_SelectCityLanguages_v1Result> PageCityLanguages(string cityCode, int page, string sortField, int sortOrder)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectCityLanguages_v1(cityCode, sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectCityLanguages_v1Result>(result, page, Convert.ToInt32(totalRecords));
            return paginatedView;
        }

        //Get one Item
        public CityLanguage GetItem(string cityCode, string languageCode)
        {
            return db.CityLanguages.SingleOrDefault(c => (c.CityCode == cityCode)
                    && (c.LanguageCode == languageCode));
        }

        //Add Data From Linked Tables for Display
        public void EditItemForDisplay(CityLanguage cityLanguage)
        {
            //Add LanguageName
            if (cityLanguage.LanguageCode != null)
            {
                LanguageRepository languageRepository = new LanguageRepository();
                Language language = new Language();
                language = languageRepository.GetLanguage(cityLanguage.LanguageCode);
                if (language != null)
                {
                    cityLanguage.LanguageName = language.LanguageName;
                }
            }
        }

        //Languages not used by this policyAirVendorGroupItem
        public List<Language> GetUnUsedLanguages(string cityCode)
        {
            var result = from n in db.spDesktopDataAdmin_SelectCityAvailableLanguages_v1(cityCode)
                         select new Language
                         {
                             LanguageName = n.LanguageName,
                             LanguageCode = n.LanguageCode
                         };
            return result.ToList();
        }

        //Add to DB
        public void Add(CityLanguage cityLanguage)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertCityLanguage_v1(
                cityLanguage.CityCode,
                cityLanguage.LanguageCode,
                cityLanguage.CityName,
                adminUserGuid
                );

        }

        //Change the deleted status on an item
        public void Update(CityLanguage cityLanguage)
        {

            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            db.spDesktopDataAdmin_UpdateCityLanguage_v1(
                cityLanguage.CityCode,
                cityLanguage.LanguageCode,
                cityLanguage.CityName,
                adminUserGuid,
                cityLanguage.VersionNumber
                );

        }

        //Delete From DB
        public void Delete(CityLanguage cityLanguage)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeleteCityLanguage_v1(
               cityLanguage.CityCode,
               cityLanguage.LanguageCode,
               adminUserGuid,
               cityLanguage.VersionNumber
               );
        }

    }
}
