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
	public class ReasonCodeProductTypeTravelerDescriptionRepository
	{
		ReasonCodeProductTypeTravelerDescriptionDC db = new ReasonCodeProductTypeTravelerDescriptionDC(Settings.getConnectionString());

		//Get a Page of ReasonCodeProductTypeTravelerDescriptions - for Page Listings
		public CWTPaginatedList<spDesktopDataAdmin_SelectReasonCodeProductTypeTravelerDescriptions_v1Result> PageReasonCodeProductTypeTravelerDescriptions(string reasonCode, int productId, int reasonCodeTypeId, int page, string sortField, int sortOrder)
		{
			//get a page of records
			var result = db.spDesktopDataAdmin_SelectReasonCodeProductTypeTravelerDescriptions_v1(reasonCode, productId, reasonCodeTypeId, sortField, sortOrder, page).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}

			//put into page object
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectReasonCodeProductTypeTravelerDescriptions_v1Result>(result, page, totalRecords);

			//return to user
			return paginatedView;
		}

		//Get a single ReasonCodeProductTypeTravelerDescription
		public ReasonCodeProductTypeTravelerDescription GetItem(string languageCode, string reasonCode, int productId, int reasonCodeTypeId)
		{
			return db.ReasonCodeProductTypeTravelerDescriptions.SingleOrDefault(c =>
				c.LanguageCode == languageCode &&
				c.ReasonCode == reasonCode &&
				c.ProductId == productId &&
				c.ReasonCodeTypeId == reasonCodeTypeId);
		}

		//Add Data From Linked Tables for Display
		public void EditItemForDisplay(ReasonCodeProductTypeTravelerDescription reasonCodeProductTypeTravelerDescription)
		{
			//Add LanguageName
			if (reasonCodeProductTypeTravelerDescription.LanguageCode != null)
			{
				LanguageRepository languageRepository = new LanguageRepository();
				Language language = new Language();
				language = languageRepository.GetLanguage(reasonCodeProductTypeTravelerDescription.LanguageCode);
				if (language != null)
				{
					reasonCodeProductTypeTravelerDescription.LanguageName = language.LanguageName;
				}
			}
		}

		//Languages not used by this item
		public List<Language> GetUnUsedLanguages(string reasonCode, int productId, int reasonCodeTypeId)
		{

			var result = from n in db.spDesktopDataAdmin_SelectReasonCodeProductTypeTravelerDescriptionAvailableLanguages_v1(reasonCode, productId, reasonCodeTypeId)
						 select new Language
						 {
							 LanguageName = n.LanguageName,
							 LanguageCode = n.LanguageCode
						 };
			return result.ToList();
		}

		//Add to DB
		public void Add(ReasonCodeProductTypeTravelerDescription reasonCodeProductTypeTravelerDescription)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_InsertReasonCodeProductTypeTravelerDescription_v1(
				reasonCodeProductTypeTravelerDescription.ReasonCode,
				reasonCodeProductTypeTravelerDescription.ProductId,
				reasonCodeProductTypeTravelerDescription.ReasonCodeTypeId,
				reasonCodeProductTypeTravelerDescription.LanguageCode,
				reasonCodeProductTypeTravelerDescription.ReasonCodeProductTypeTravelerDescription1,
				adminUserGuid
				);

		}

		//Delete From DB
		public void Delete(ReasonCodeProductTypeTravelerDescription reasonCodeProductTypeTravelerDescription)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeleteReasonCodeProductTypeTravelerDescription_v1(
			   reasonCodeProductTypeTravelerDescription.ReasonCode,
			   reasonCodeProductTypeTravelerDescription.ProductId,
			   reasonCodeProductTypeTravelerDescription.ReasonCodeTypeId,
			   reasonCodeProductTypeTravelerDescription.LanguageCode,
			   adminUserGuid,
			   reasonCodeProductTypeTravelerDescription.VersionNumber
			   );
		}

		//Change the deleted status on an item
		public void Update(ReasonCodeProductTypeTravelerDescription reasonCodeProductTypeTravelerDescription)
		{

			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			db.spDesktopDataAdmin_UpdateReasonCodeProductTypeTravelerDescription_v1(
				reasonCodeProductTypeTravelerDescription.ReasonCode,
			   reasonCodeProductTypeTravelerDescription.ProductId,
			   reasonCodeProductTypeTravelerDescription.ReasonCodeTypeId,
				reasonCodeProductTypeTravelerDescription.LanguageCode,
				reasonCodeProductTypeTravelerDescription.ReasonCodeProductTypeTravelerDescription1,
				adminUserGuid,
				reasonCodeProductTypeTravelerDescription.VersionNumber
				);

		}
	}
}