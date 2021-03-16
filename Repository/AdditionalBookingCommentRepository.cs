using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Xml;
using CWTDesktopDatabase.ViewModels;

namespace CWTDesktopDatabase.Repository
{
	public class AdditionalBookingCommentRepository
	{
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//Get a Page of PageAdditionalBookingComment Items - for Page Listings
		public CWTPaginatedList<spDesktopDataAdmin_SelectAdditionalBookingComments_v1Result> PageAdditionalBookingComments(int page, int id, string sortField, int? sortOrder, string filter)
		{
			//get a page of records
			var result = db.spDesktopDataAdmin_SelectAdditionalBookingComments_v1(id, sortField, sortOrder, page, filter ?? "").ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}

			//put into page object
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectAdditionalBookingComments_v1Result>(result, page, totalRecords);

			//return to user
			return paginatedView;
		}

		//Get Available Booking Languages (Create)
		public List<Language> GetAvailableLanguages(int bookingChannelId)
		{
			//Get existing items
			var existingAdditionalBookingComments = db.AdditionalBookingComments.Where(x => x.BookingChannelId == bookingChannelId);

			//Get languages
			var existingLanguages = existingAdditionalBookingComments.Select(x => x.LanguageCode);

			//Return a list of available languages
			return db.Languages.Where(x => !existingLanguages.Contains(x.LanguageCode)).ToList();
		}

		//Get Available Booking Languages (Edit)
		public List<Language> GetAvailableLanguagesEdit(int additionalBookingCommentId)
		{
			AdditionalBookingComment additionalBookingComment = new AdditionalBookingComment();
			additionalBookingComment = GetAdditionalBookingComment(additionalBookingCommentId);

			if (additionalBookingComment == null)
			{
				return null;
			}

			//Get existing items
			var existingAdditionalBookingComments = db.AdditionalBookingComments.Where(x => x.BookingChannelId == additionalBookingComment.BookingChannelId);

			//Get languages
			var existingAdditionalBookingCommentLanguages = existingAdditionalBookingComments.Select(x => x.LanguageCode);

			//Available languages
			var availableLanguages = db.Languages.Where(x => !existingAdditionalBookingCommentLanguages.Contains(x.LanguageCode)).ToList();

			//Add current selected language
			Language language = new Language();
			LanguageRepository languageRepository = new LanguageRepository();
			if (language != null)
			{
				language = languageRepository.GetLanguage(additionalBookingComment.LanguageCode);
				availableLanguages.Add(language);
				availableLanguages = availableLanguages.OrderBy(x => x.LanguageName).ToList();
			}

			//Return a list of available languages
			return availableLanguages;
		}

        //Get a Single Item
        public AdditionalBookingComment GetAdditionalBookingComment(int additionalBookingCommentId)
        {
            return db.AdditionalBookingComments.SingleOrDefault(
                c => c.AdditionalBookingCommentId == additionalBookingCommentId
            );
        }

        //Get AdditionalBookingComments
        public List<AdditionalBookingComment> GetAdditionalBookingComments(int bookingChannelId)
        {
            return db.AdditionalBookingComments.Where(c => c.BookingChannelId == bookingChannelId).ToList();
        }

        //Add to DB
        public void Add(AdditionalBookingCommentVM additionalBookingCommentVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_InsertAdditionalBookingComment_v1(
				additionalBookingCommentVM.AdditionalBookingComment.BookingChannelId,
				additionalBookingCommentVM.AdditionalBookingComment.AdditionalBookingCommentDescription,
				additionalBookingCommentVM.AdditionalBookingComment.LanguageCode,
				adminUserGuid
			);
		}

		//Update in DB
		public void Update(AdditionalBookingCommentVM additionalBookingCommentVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdateAdditionalBookingComment_v1(
				additionalBookingCommentVM.AdditionalBookingComment.AdditionalBookingCommentId,
				additionalBookingCommentVM.AdditionalBookingComment.BookingChannelId,
				additionalBookingCommentVM.AdditionalBookingComment.AdditionalBookingCommentDescription,
				additionalBookingCommentVM.AdditionalBookingComment.LanguageCode,
				adminUserGuid,
				additionalBookingCommentVM.AdditionalBookingComment.VersionNumber
			);
		}

		//Delete From DB
		public void Delete(AdditionalBookingComment additionalBookingComment)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeleteAdditionalBookingComment_v1(
				additionalBookingComment.AdditionalBookingCommentId,
				adminUserGuid,
				additionalBookingComment.VersionNumber
			);
		}
	}
}