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
    public class ReasonCodeAlternativeDescriptionRepository
    {
        private ReasonCodeAlternativeDescriptionDC db = new ReasonCodeAlternativeDescriptionDC(Settings.getConnectionString());

        //Sortable List
		public CWTPaginatedList<spDesktopDataAdmin_SelectReasonCodeAlternativeDescriptions_v1Result> GetAlternativeDescriptions(int reasonCodeItemId, int page, string sortField, int sortOrder)
		{
			//get a page of records
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectReasonCodeAlternativeDescriptions_v1(reasonCodeItemId, sortField, sortOrder, page, adminUserGuid).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}

			//put into page object
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectReasonCodeAlternativeDescriptions_v1Result>(result, page, totalRecords);

			//return to user
			return paginatedView;
		}
        
        //Languages not used by this policyAirVendorGroupItem
        public List<Language> GetUnUsedLanguages(int reasonCodeItemId)
        {

            var result = from n in db.spDesktopDataAdmin_SelectReasonCodeAlternativeDescriptionAvailableLanguages_v1(reasonCodeItemId)
                         select new Language
                         {
                             LanguageName = n.LanguageName,
                             LanguageCode = n.LanguageCode
                         };
            return result.ToList();
        }

        //Get one Item
        public ReasonCodeAlternativeDescription GetItem(int reasonCodeItemId, string languageCode)
        {
            return db.ReasonCodeAlternativeDescriptions.SingleOrDefault(c => (c.ReasonCodeItemId == reasonCodeItemId)
                    && (c.LanguageCode == languageCode));
        }

        //Add Data From Linked Tables for Display
        public void EditItemForDisplay(ReasonCodeAlternativeDescription reasonCodeAlternativeDescription)
        {
            //Add LanguageName
            if (reasonCodeAlternativeDescription.LanguageCode != null)
            {
                LanguageRepository languageRepository = new LanguageRepository();
                Language language = new Language();
                language = languageRepository.GetLanguage(reasonCodeAlternativeDescription.LanguageCode);
                if (language != null)
                {
                    reasonCodeAlternativeDescription.LanguageName = language.LanguageName;
                }
            }

            //Add PolicyGroupName
            ReasonCodeItemRepository reasonCodeItemRepository = new ReasonCodeItemRepository();
            ReasonCodeItem reasonCodeItem = new ReasonCodeItem();
            reasonCodeItem = reasonCodeItemRepository.GetItem(reasonCodeAlternativeDescription.ReasonCodeItemId);
            if (reasonCodeItem != null)
            {
                reasonCodeItemRepository.EditItemForDisplay(reasonCodeItem);
                reasonCodeAlternativeDescription.ReasonCodeItemDisplayOrder = reasonCodeItem.DisplayOrder;
            }

        }
        //Add to DB
        public void Add(ReasonCodeAlternativeDescription reasonCodeAlternativeDescription)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertReasonCodeAlternativeDescription_v1(
                reasonCodeAlternativeDescription.ReasonCodeItemId,
                reasonCodeAlternativeDescription.LanguageCode,
                reasonCodeAlternativeDescription.ReasonCodeAlternativeDescription1,
                adminUserGuid
                );

        }

        //Delete From DB
        public void Delete(ReasonCodeAlternativeDescription reasonCodeAlternativeDescription)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeleteReasonCodeAlternativeDescription_v1(
               reasonCodeAlternativeDescription.ReasonCodeItemId,
               reasonCodeAlternativeDescription.LanguageCode,
               adminUserGuid,
               reasonCodeAlternativeDescription.VersionNumber
               );
        }

        //Change the deleted status on an item
        public void Update(ReasonCodeAlternativeDescription reasonCodeAlternativeDescription)
        {

            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            db.spDesktopDataAdmin_UpdateReasonCodeAlternativeDescription_v1(
                reasonCodeAlternativeDescription.ReasonCodeItemId,
                reasonCodeAlternativeDescription.LanguageCode,
                reasonCodeAlternativeDescription.ReasonCodeAlternativeDescription1,
                adminUserGuid,
                reasonCodeAlternativeDescription.VersionNumber
                );

        }
    }
}
