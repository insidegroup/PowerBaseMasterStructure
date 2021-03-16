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
    public class ReasonCodeTravelerDescriptionRepository
    {
		private ReasonCodeTravelerDescriptionDC db = new ReasonCodeTravelerDescriptionDC(Settings.getConnectionString());

        //Sortable List
		public CWTPaginatedList<spDesktopDataAdmin_SelectReasonCodeTravelerDescriptions_v1Result> GetTravelerDescriptions(int reasonCodeItemId, int page, string sortField, int sortOrder)
		{
			//get a page of records
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectReasonCodeTravelerDescriptions_v1(reasonCodeItemId, sortField, sortOrder, page, adminUserGuid).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}

			//put into page object
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectReasonCodeTravelerDescriptions_v1Result>(result, page, totalRecords);

			//return to user
			return paginatedView;
		}
        
        //Languages not used by this policyAirVendorGroupItem
        public List<Language> GetUnUsedLanguages(int reasonCodeItemId)
        {

            var result = from n in db.spDesktopDataAdmin_SelectReasonCodeTravelerDescriptionAvailableLanguages_v1(reasonCodeItemId)
                         select new Language
                         {
                             LanguageName = n.LanguageName,
                             LanguageCode = n.LanguageCode
                         };
            return result.ToList();
        }

        //Get one Item
        public ReasonCodeTravelerDescription GetItem(int reasonCodeItemId, string languageCode)
        {
            return db.ReasonCodeTravelerDescriptions.SingleOrDefault(c => (c.ReasonCodeItemId == reasonCodeItemId)
                    && (c.LanguageCode == languageCode));
        }

        //Add Data From Linked Tables for Display
        public void EditItemForDisplay(ReasonCodeTravelerDescription reasonCodeTravelerDescription)
        {
            //Add LanguageName
            if (reasonCodeTravelerDescription.LanguageCode != null)
            {
                LanguageRepository languageRepository = new LanguageRepository();
                Language language = new Language();
                language = languageRepository.GetLanguage(reasonCodeTravelerDescription.LanguageCode);
                if (language != null)
                {
                    reasonCodeTravelerDescription.LanguageName = language.LanguageName;
                }
            }

            //Add PolicyGroupName
            ReasonCodeItemRepository reasonCodeItemRepository = new ReasonCodeItemRepository();
            ReasonCodeItem reasonCodeItem = new ReasonCodeItem();
            reasonCodeItem = reasonCodeItemRepository.GetItem(reasonCodeTravelerDescription.ReasonCodeItemId);
			if (reasonCodeItem != null)
			{
				reasonCodeItemRepository.EditItemForDisplay(reasonCodeItem);
				reasonCodeTravelerDescription.ReasonCodeItemDisplayOrder = reasonCodeItem.DisplayOrder;

				//ReasonCodeProductTypeTravelerDescription
				ReasonCodeProductTypeTravelerDescriptionRepository reasonCodeProductTypeTravelerDescriptionRepository = new ReasonCodeProductTypeTravelerDescriptionRepository();
				ReasonCodeProductTypeTravelerDescription reasonCodeProductTypeTravelerDescription = new ReasonCodeProductTypeTravelerDescription();
				reasonCodeProductTypeTravelerDescription = reasonCodeProductTypeTravelerDescriptionRepository.GetItem(
					"en-GB",
					reasonCodeItem.ReasonCode,
					reasonCodeItem.ProductId,
					reasonCodeItem.ReasonCodeTypeId
				);

				if (reasonCodeProductTypeTravelerDescription != null)
				{
					reasonCodeTravelerDescription.ReasonCodeProductTypeTravelerDescription = reasonCodeProductTypeTravelerDescription;
				}
				else
				{
					reasonCodeTravelerDescription.ReasonCodeProductTypeTravelerDescription = new ReasonCodeProductTypeTravelerDescription();
				}
			}

        }
        //Add to DB
        public void Add(ReasonCodeTravelerDescription reasonCodeTravelerDescription)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertReasonCodeTravelerDescription_v1(
                reasonCodeTravelerDescription.ReasonCodeItemId,
                reasonCodeTravelerDescription.LanguageCode,
                reasonCodeTravelerDescription.ReasonCodeTravelerDescription1,
                adminUserGuid
                );

        }

        //Delete From DB
        public void Delete(ReasonCodeTravelerDescription reasonCodeTravelerDescription)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeleteReasonCodeTravelerDescription_v1(
               reasonCodeTravelerDescription.ReasonCodeItemId,
               reasonCodeTravelerDescription.LanguageCode,
               adminUserGuid,
               reasonCodeTravelerDescription.VersionNumber
               );
        }

        //Change the deleted status on an item
        public void Update(ReasonCodeTravelerDescription reasonCodeTravelerDescription)
        {

            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            db.spDesktopDataAdmin_UpdateReasonCodeTravelerDescription_v1(
                reasonCodeTravelerDescription.ReasonCodeItemId,
                reasonCodeTravelerDescription.LanguageCode,
                reasonCodeTravelerDescription.ReasonCodeTravelerDescription1,
                adminUserGuid,
                reasonCodeTravelerDescription.VersionNumber
                );

        }
    }
}
