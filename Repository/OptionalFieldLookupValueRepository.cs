using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.Repository
{
	public class OptionalFieldLookupValueRepository
	{
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//Get a Page of Optional Field Lookup Values
		public CWTPaginatedList<spDesktopDataAdmin_SelectOptionalFieldLookupValues_v1Result> PageOptionalFieldLookupValues(int optionalFieldId, string filter, int page, string sortField, int sortOrder)
		{
			//get a page of records
			var result = db.spDesktopDataAdmin_SelectOptionalFieldLookupValues_v1(optionalFieldId, filter, sortField, sortOrder, page).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}

			//put into page object
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectOptionalFieldLookupValues_v1Result>(result, page, totalRecords);

			//return to user
			return paginatedView;
		}
		
		//Get one Item
		public OptionalFieldLookupValue GetItem(int? OptionalFieldLookupValueId)
		{
			return db.OptionalFieldLookupValues.SingleOrDefault(
				c => (
					c.OptionalFieldLookupValueId == OptionalFieldLookupValueId)
				);
		}
		
		//Add to DB
		public void Add(OptionalFieldLookupValueVM OptionalFieldLookupValueVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_InsertOptionalFieldLookupValue_v1(
				OptionalFieldLookupValueVM.OptionalFieldLookupValue.OptionalFieldId,
				OptionalFieldLookupValueVM.OptionalFieldLookupValueLanguage.LanguageCode,
				OptionalFieldLookupValueVM.OptionalFieldLookupValueLanguage.OptionalFieldLookupValueLabel,
				adminUserGuid
			);
		}

		//Update in DB
		public void Update(OptionalFieldLookupValueVM OptionalFieldLookupValueVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] {'|'})[0];

			db.spDesktopDataAdmin_UpdateOptionalFieldLookupValue_v1(
				OptionalFieldLookupValueVM.OptionalFieldLookupValueLanguage.OptionalFieldLookupValueId,
				OptionalFieldLookupValueVM.OptionalFieldLookupValueLanguage.LanguageCode,
				OptionalFieldLookupValueVM.OptionalFieldLookupValueLanguage.OptionalFieldLookupValueLabel,
				adminUserGuid,
				OptionalFieldLookupValueVM.OptionalFieldLookupValueLanguage.VersionNumber
			);
		}

		//Delete From DB
		public void Delete(OptionalFieldLookupValue OptionalFieldLookupValue)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeleteOptionalFieldLookupValue_v1(
				OptionalFieldLookupValue.OptionalFieldLookupValueId,
				adminUserGuid,
				OptionalFieldLookupValue.VersionNumber
			);
		}
	}
}