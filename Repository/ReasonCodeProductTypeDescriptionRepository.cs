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
   

    public class ReasonCodeProductTypeDescriptionRepository
    {
        ReasonCodeProductTypeDescriptionDC db = new ReasonCodeProductTypeDescriptionDC(Settings.getConnectionString());


        //Get a Page of ReasonCodeProductTypeDescriptions - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectReasonCodeProductTypeDescriptions_v1Result> PageReasonCodeProductTypeDescriptions(string reasonCode, int productId, int reasonCodeTypeId, int page, string sortField, int sortOrder)
        {
            //get a page of records
            var result = db.spDesktopDataAdmin_SelectReasonCodeProductTypeDescriptions_v1(reasonCode, productId, reasonCodeTypeId, sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectReasonCodeProductTypeDescriptions_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get a single ReasonCodeProductTypeDescription
        public ReasonCodeProductTypeDescription GetItem(string languageCode, string reasonCode, int productId, int reasonCodeTypeId)
        {
            return db.ReasonCodeProductTypeDescriptions.SingleOrDefault(c =>
                c.LanguageCode == languageCode &&
                c.ReasonCode == reasonCode &&
                c.ProductId == productId &&
                c.ReasonCodeTypeId == reasonCodeTypeId);
        }

        //Add Data From Linked Tables for Display
        public void EditItemForDisplay(ReasonCodeProductTypeDescription reasonCodeProductTypeDescription)
        {
            //Add LanguageName
            if (reasonCodeProductTypeDescription.LanguageCode != null)
            {
                LanguageRepository languageRepository = new LanguageRepository();
                Language language = new Language();
                language = languageRepository.GetLanguage(reasonCodeProductTypeDescription.LanguageCode);
                if (language != null)
                {
                    reasonCodeProductTypeDescription.LanguageName = language.LanguageName;
                }
            }



        }

        //Languages not used by this policyAirVendorGroupItem
        public List<Language> GetUnUsedLanguages(string reasonCode, int productId, int reasonCodeTypeId)
        {

            var result = from n in db.spDesktopDataAdmin_SelectReasonCodeProductTypeDescriptionAvailableLanguages_v1(reasonCode,productId,reasonCodeTypeId)
                         select new Language
                         {
                             LanguageName = n.LanguageName,
                             LanguageCode = n.LanguageCode
                         };
            return result.ToList();
        }

        //Add to DB
        public void Add(ReasonCodeProductTypeDescription reasonCodeProductTypeDescription)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertReasonCodeProductTypeDescription_v1(
                reasonCodeProductTypeDescription.ReasonCode,
                reasonCodeProductTypeDescription.ProductId,
                reasonCodeProductTypeDescription.ReasonCodeTypeId,
                reasonCodeProductTypeDescription.LanguageCode,
                reasonCodeProductTypeDescription.ReasonCodeProductTypeDescription1,
                adminUserGuid
                );

        }

        //Delete From DB
        public void Delete(ReasonCodeProductTypeDescription reasonCodeProductTypeDescription)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeleteReasonCodeProductTypeDescription_v1(
               reasonCodeProductTypeDescription.ReasonCode,
               reasonCodeProductTypeDescription.ProductId,
               reasonCodeProductTypeDescription.ReasonCodeTypeId,
               reasonCodeProductTypeDescription.LanguageCode,
               adminUserGuid,
               reasonCodeProductTypeDescription.VersionNumber
               );
        }

        //Change the deleted status on an item
        public void Update(ReasonCodeProductTypeDescription reasonCodeProductTypeDescription)
        {

            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            db.spDesktopDataAdmin_UpdateReasonCodeProductTypeDescription_v1(
                reasonCodeProductTypeDescription.ReasonCode,
               reasonCodeProductTypeDescription.ProductId,
               reasonCodeProductTypeDescription.ReasonCodeTypeId,
                reasonCodeProductTypeDescription.LanguageCode,
                reasonCodeProductTypeDescription.ReasonCodeProductTypeDescription1,
                adminUserGuid,
                reasonCodeProductTypeDescription.VersionNumber
                );

        }
    }
}