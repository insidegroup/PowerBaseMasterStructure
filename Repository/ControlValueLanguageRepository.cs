using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Linq.Dynamic;

namespace CWTDesktopDatabase.Repository
{
    public class ControlValueLanguageRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get a Page of ControlValueLanguages - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectControlValueLanguages_v1Result> PageControlValueLanguages(int controlValueId, int page, string sortField, int sortOrder)
        {
            //get a page of records
            var result = db.spDesktopDataAdmin_SelectControlValueLanguages_v1(controlValueId, sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectControlValueLanguages_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Sortable List
        /*public IQueryable<fnDesktopDataAdmin_SelectControlValueLanguages_v1Result> GetControlValueLanguages(int controlValueId, string sortField, int sortOrder)
        {
            if (sortOrder == 0)
            {
                sortField = sortField + " ascending";
            }
            else
            {
                sortField = sortField + " descending";
            }
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.fnDesktopDataAdmin_SelectControlValueLanguages_v1(controlValueId, adminUserGuid).OrderBy(sortField); ;
            return result;
        }
        */

        //Get one Item
        public ControlValueLanguage GetItem(int controlValueId, string languageCode)
        {
            return db.ControlValueLanguages.SingleOrDefault(c => (c.ControlValueId == controlValueId)
                    && (c.LanguageCode == languageCode));
        }

        //Add Data From Linked Tables for Display
        public void EditItemForDisplay(ControlValueLanguage controlValueLanguage)
        {
            //Add LanguageName
            if (controlValueLanguage.LanguageCode != null)
            {
                LanguageRepository languageRepository = new LanguageRepository();
                Language language = new Language();
                language = languageRepository.GetLanguage(controlValueLanguage.LanguageCode);
                if (language != null)
                {
                    controlValueLanguage.LanguageName = language.LanguageName;
                }
            }

            //Add PolicyGroupName
            ControlValueRepository controlValueRepository = new ControlValueRepository();
            ControlValue controlValue = new ControlValue();
            controlValue = controlValueRepository.GetControlValue(controlValueLanguage.ControlValueId);

            if (controlValue != null)
            {
                controlValueRepository.EditForDisplay(controlValue);
                //controlValueLanguage.ControlValue = controlValue.ControlValue1;
            }

        }

        //Languages not used by this item
        public List<Language> GetUnUsedLanguages(int controlValueId)
        {

            var result = from n in db.spDesktopDataAdmin_SelectControlValueAvailableLanguages_v1(controlValueId)
                         select new Language
                         {
                             LanguageName = n.LanguageName,
                             LanguageCode = n.LanguageCode
                         };
            return result.ToList();
        }

        //Add to DB
        public void Add(ControlValueLanguage controlValueLanguage)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertControlValueLanguage_v1(
                controlValueLanguage.ControlValueId,
                controlValueLanguage.LanguageCode,
                controlValueLanguage.ControlValueTranslation,
                adminUserGuid
                );
        }

        //Delete From DB
        public void Delete(ControlValueLanguage controlValueLanguage)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeleteControlValueLanguage_v1(
                controlValueLanguage.ControlValueId, 
                controlValueLanguage.LanguageCode,
                adminUserGuid,
                controlValueLanguage.VersionNumber
                );
        }

        //Change the deleted status on an item
        public void Update(ControlValueLanguage controlValueLanguage)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            db.spDesktopDataAdmin_UpdateControlValueLanguage_v1(
                controlValueLanguage.ControlValueId,
                controlValueLanguage.LanguageCode,
                controlValueLanguage.ControlValueTranslation,
                adminUserGuid,
                controlValueLanguage.VersionNumber
                );

        }

    }
}
