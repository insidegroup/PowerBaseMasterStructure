using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Repository
{
    public class FormOfPaymentAdviceMessageGroupItemTranslationRepository
    {
        private FormOfPaymentAdviceMessageGroupDC db = new FormOfPaymentAdviceMessageGroupDC(Settings.getConnectionString());

        //Get a Page of Cities
		public CWTPaginatedList<spDesktopDataAdmin_SelectFormOfPaymentAdviceMessageGroupItemTranslations_v1Result> PageFormOfPaymentAdviceMessageGroupItemTranslations(int formOfPaymentAdviceMessageItemId, int page, string sortField, int sortOrder)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectFormOfPaymentAdviceMessageGroupItemTranslations_v1(formOfPaymentAdviceMessageItemId, sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectFormOfPaymentAdviceMessageGroupItemTranslations_v1Result>(result, page, Convert.ToInt32(totalRecords));
            return paginatedView;
        }

        //Get one Item
        public FormOfPaymentAdviceMessageGroupItemTranslation GetItem(int formOfPaymentAdviceMessageItemId, string languageCode)
        {
            return db.FormOfPaymentAdviceMessageGroupItemTranslations.SingleOrDefault(c => (c.FormOfPaymentAdviceMessageGroupItemId == formOfPaymentAdviceMessageItemId) && (c.LanguageCode == languageCode));
        }

        //Add Data From Linked Tables for Display
        public void EditItemForDisplay(FormOfPaymentAdviceMessageGroupItemTranslation formOfPaymentAdviceMessageGroupItemTranslation)
        {
            //Add LanguageName
            if (formOfPaymentAdviceMessageGroupItemTranslation.LanguageCode != null)
            {
                LanguageRepository languageRepository = new LanguageRepository();
                Language language = new Language();
                language = languageRepository.GetLanguage(formOfPaymentAdviceMessageGroupItemTranslation.LanguageCode);
                if (language != null)
                {
                    formOfPaymentAdviceMessageGroupItemTranslation.LanguageName = language.LanguageName;
                }
            }
        }

        //Languages not used by this policyAirVendorGroupItem
        public List<Language> GetUnUsedLanguages(int formOfPaymentAdviceMessageItemId)
        {
            var result = from n in db.spDesktopDataAdmin_SelectFormOfPaymentAdviceMessageGroupItemAvailableLanguages_v1(formOfPaymentAdviceMessageItemId)
                         select new Language
                         {
                             LanguageName = n.LanguageName,
                             LanguageCode = n.LanguageCode
                         };
            return result.ToList();
        }

        //Add to DB
        public void Add(FormOfPaymentAdviceMessageGroupItemTranslation formOfPaymentAdviceMessageGroupItemTranslation)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertFormOfPaymentAdviceMessageGroupItemTranslation_v1(
                formOfPaymentAdviceMessageGroupItemTranslation.FormOfPaymentAdviceMessageGroupItemId,
                formOfPaymentAdviceMessageGroupItemTranslation.LanguageCode,
                formOfPaymentAdviceMessageGroupItemTranslation.FormOfPaymentAdviceMessageTranslation,
                adminUserGuid
                );

        }

        //Change the deleted status on an item
        public void Update(FormOfPaymentAdviceMessageGroupItemTranslation formOfPaymentAdviceMessageGroupItemTranslation)
        {

            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            db.spDesktopDataAdmin_UpdateFormOfPaymentAdviceMessageGroupItemTranslation_v1(
                formOfPaymentAdviceMessageGroupItemTranslation.FormOfPaymentAdviceMessageGroupItemId,
                formOfPaymentAdviceMessageGroupItemTranslation.LanguageCode,
				formOfPaymentAdviceMessageGroupItemTranslation.FormOfPaymentAdviceMessageTranslation,
                adminUserGuid,
                formOfPaymentAdviceMessageGroupItemTranslation.VersionNumber
                );

        }

        //Delete From DB
        public void Delete(FormOfPaymentAdviceMessageGroupItemTranslation formOfPaymentAdviceMessageGroupItemTranslation)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeleteFormOfPaymentAdviceMessageGroupItemTranslation_v1(
               formOfPaymentAdviceMessageGroupItemTranslation.FormOfPaymentAdviceMessageGroupItemId,
               formOfPaymentAdviceMessageGroupItemTranslation.LanguageCode,
               adminUserGuid,
               formOfPaymentAdviceMessageGroupItemTranslation.VersionNumber
               );
        }

    }
}
