using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;

namespace CWTDesktopDatabase.Repository
{
	public class OptionalFieldLanguageRespository
	{
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//Get a Page of OptionalFieldLanguages
		public CWTPaginatedList<spDesktopDataAdmin_SelectOptionalFieldLanguages_v1Result> PageOptionalFieldLanguages(int optionalFieldId, string filter, int page, string sortField, int sortOrder)
		{
			//get a page of records
			var result = db.spDesktopDataAdmin_SelectOptionalFieldLanguages_v1(optionalFieldId, filter, sortField, sortOrder, page).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}

			//put into page object
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectOptionalFieldLanguages_v1Result>(result, page, totalRecords);

			//return to user
			return paginatedView;
		}
		
		//Get one Item
		public OptionalFieldLanguage GetItem(int optionalFieldId, string languageCode)
		{
			return db.OptionalFieldLanguages.SingleOrDefault(
				c => (
					c.OptionalField.OptionalFieldId == optionalFieldId && 
					c.LanguageCode == languageCode)
				);
		}
		
		//Add to DB
		public void Add(OptionalFieldLanguageVM optionalFieldLanguageVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_InsertOptionalFieldLanguage_v1(
				optionalFieldLanguageVM.OptionalFieldLanguage.OptionalFieldId,
				optionalFieldLanguageVM.OptionalFieldLanguage.LanguageCode,
				optionalFieldLanguageVM.OptionalFieldLanguage.OptionalFieldLabel,
				optionalFieldLanguageVM.OptionalFieldLanguage.OptionalFieldTooltip,
				optionalFieldLanguageVM.OptionalFieldLanguage.OptionalFieldDefaultText,
				optionalFieldLanguageVM.OptionalFieldLanguage.OptionalFieldValidationRegularExpression,
				optionalFieldLanguageVM.OptionalFieldLanguage.OptionalFieldValidationFailureMessage,
				optionalFieldLanguageVM.OptionalFieldLanguage.OptionalFieldSize,
				adminUserGuid
			);
		}

		//Update in DB
		public void Update(OptionalFieldLanguageVM optionalFieldLanguageVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] {'|'})[0];

			db.spDesktopDataAdmin_UpdateOptionalFieldLanguage_v1(
				optionalFieldLanguageVM.OptionalFieldLanguage.OptionalFieldId,
				optionalFieldLanguageVM.OptionalFieldLanguage.LanguageCode,
				optionalFieldLanguageVM.OptionalFieldLanguage.OptionalFieldLabel,
				optionalFieldLanguageVM.OptionalFieldLanguage.OptionalFieldTooltip,
				optionalFieldLanguageVM.OptionalFieldLanguage.OptionalFieldDefaultText,
				optionalFieldLanguageVM.OptionalFieldLanguage.OptionalFieldValidationRegularExpression,
				optionalFieldLanguageVM.OptionalFieldLanguage.OptionalFieldValidationFailureMessage,
				optionalFieldLanguageVM.OptionalFieldLanguage.OptionalFieldSize,
				adminUserGuid,
				optionalFieldLanguageVM.OptionalFieldLanguage.VersionNumber
			);
		}

		//Delete From DB
		public void Delete(OptionalFieldLanguage optionalFieldLanguage)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeleteOptionalFieldLanguage_v1(
				optionalFieldLanguage.OptionalFieldId,
				optionalFieldLanguage.LanguageCode,
				adminUserGuid,
				optionalFieldLanguage.VersionNumber
			);
		}

		//Get a list of available Languages
		public List<Language> GetAllAvailableLanguages(int optionalFieldId)
		{
			var result = from n in db.spDesktopDataAdmin_SelectOptionalFieldLanguageAvailableLanguages_v1(optionalFieldId)
						 orderby n.LanguageName
						 select
						 new Language
						 {
							 LanguageCode = n.LanguageCode,
							 LanguageName = n.LanguageName
						 };
			return result.ToList();

		}
	}
}