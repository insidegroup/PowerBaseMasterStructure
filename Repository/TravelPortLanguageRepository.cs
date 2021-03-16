using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Linq.Dynamic;



namespace CWTDesktopDatabase.Repository
{
    public class TravelPortLanguageRepository
    {
        private TravelPortLanguageDC db = new TravelPortLanguageDC(Settings.getConnectionString());

        //Get a Page of ControlValueLanguages - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectTravelPortLanguages_v1Result> PageTravelPortTranslations(string travelPortCode, int page, string sortField, int sortOrder)
        {
            //get a page of records
            var result = db.spDesktopDataAdmin_SelectTravelPortLanguages_v1(travelPortCode, sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectTravelPortLanguages_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //List of All Items - Sortable
        /*public IQueryable<fnDesktopDataAdmin_SelectTravelPortLanguages_v1Result> GetTravelPortLanguages(string id, string sortField, int sortOrder)
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
            return db.fnDesktopDataAdmin_SelectTravelPortLanguages_v1(id, adminUserGuid).OrderBy(sortField);

        }*/

        public TravelPortLanguage GetTravelPortLanguage(string id, int travelPortTypeId, string languageCode)
        {
            return db.TravelPortLanguages.SingleOrDefault(c => (c.TravelPortCode == id) && (c.TravelPortTypeId == travelPortTypeId) && (c.LanguageCode == languageCode));
        }

        //Add Data From Linked Tables for Display
        public void EditForDisplay(TravelPortLanguage travelPortLanguage)
        {
            //Add TravelPortTypeDescription
            TravelPortTypeRepository travelPortTypeRepository = new TravelPortTypeRepository();
            TravelPortType travelPortType = new TravelPortType();
            travelPortType = travelPortTypeRepository.GetTravelPortType(travelPortLanguage.TravelPortTypeId);
            if (travelPortType != null)
            {
                travelPortLanguage.TravelPortTypeDescription = travelPortType.TravelPortTypeDescription;
            }

            //Add LanguageName
            if (travelPortLanguage.LanguageCode != null)
            {
                LanguageRepository languageRepository = new LanguageRepository();
                Language language = new Language();
                language = languageRepository.GetLanguage(travelPortLanguage.LanguageCode);
                if (language != null)
                {
                    travelPortLanguage.LanguageName = language.LanguageName;
                }
            }

            //Add TravelPortCodeTravelPortName (name of original)
            TravelPortRepository travelPortRepository = new TravelPortRepository();
            TravelPort travelPort = new TravelPort();
            travelPort = travelPortRepository.GetTravelPort(travelPortLanguage.TravelPortCode);
            if (travelPort != null)
            {
                travelPortLanguage.TravelPortCodeTravelPortName = travelPort.TravelportName;
            }
        }

        //Add to DB
        public void Add(TravelPortLanguage travelPortLanguage)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertTravelPortLanguage_v1(
                travelPortLanguage.TravelPortCode,
                travelPortLanguage.TravelPortTypeId,
                travelPortLanguage.LanguageCode,
                travelPortLanguage.TravelPortName,
                adminUserGuid
            );

        }

        //Change the deleted status on an item
        public void Update(TravelPortLanguage travelPortLanguage)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateTravelPortLanguage_v1(
                travelPortLanguage.TravelPortCode,
                travelPortLanguage.TravelPortTypeId,
                travelPortLanguage.LanguageCode,
                travelPortLanguage.TravelPortName,
                adminUserGuid,
                travelPortLanguage.VersionNumber
             );


        }

        //Delete From DB
        public void Delete(TravelPortLanguage travelPortLanguage)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeleteTravelPortLanguage_v1(
                travelPortLanguage.TravelPortCode,
                travelPortLanguage.TravelPortTypeId,
                travelPortLanguage.LanguageCode,
                adminUserGuid,
                travelPortLanguage.VersionNumber
            );
        }
    }
}
