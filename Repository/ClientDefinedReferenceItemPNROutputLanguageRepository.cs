using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.ViewModels;

namespace CWTDesktopDatabase.Repository
{
	public class ClientDefinedReferenceItemPNROutputLanguageRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//List of ClientDefinedReferenceItemPNROutputLanguages - Paged
		public CWTPaginatedList<spDesktopDataAdmin_SelectClientDefinedReferenceItemPNROutputLanguages_v1Result> PageClientDefinedReferenceItemPNROutputLanguages(int id, string filter, int page, string sortField, int sortOrder)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectClientDefinedReferenceItemPNROutputLanguages_v1(id, filter, sortField, sortOrder, page).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectClientDefinedReferenceItemPNROutputLanguages_v1Result>(result, page, Convert.ToInt32(totalRecords));
			return paginatedView;

		}

		//Get one Item from ClientDefinedReferenceItemPNROutput
		public ClientDefinedReferenceItemPNROutputLanguage GetClientDefinedReferenceItemPNROutputLanguage(int clientDefinedReferenceItemPNROutputId, string languageCode)
		{
			return db.ClientDefinedReferenceItemPNROutputLanguages.SingleOrDefault(c => c.ClientDefinedReferenceItemPNROutputId == clientDefinedReferenceItemPNROutputId && c.LanguageCode == languageCode);
		}

		//Select ClientDefinedReferenceItemPNROutputLanguage Available Languages
		public List<Language> GetAvailableLanguages(int clientDefinedReferenceItemPNROutputId)
		{
			var result = from n in db.spDesktopDataAdmin_SelectClientDefinedReferenceItemPNROutputLanguageAvailableLanguages_v1(clientDefinedReferenceItemPNROutputId)
						 select new Language
						 {
							 LanguageName = n.LanguageName,
							 LanguageCode = n.LanguageCode
						 };
			return result.ToList();
		}

		//Add Item
		public void Add(ClientDefinedReferenceItemPNROutputLanguage clientDefinedReferenceItemPNROutputLanguage)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_InsertClientDefinedReferenceItemPNROutputLanguage_v1(
				clientDefinedReferenceItemPNROutputLanguage.ClientDefinedReferenceItemPNROutputId,
				clientDefinedReferenceItemPNROutputLanguage.LanguageCode,
				clientDefinedReferenceItemPNROutputLanguage.RemarkTranslation,
				adminUserGuid
			);
		}

		//Update Item
		public void Update(ClientDefinedReferenceItemPNROutputLanguage clientDefinedReferenceItemPNROutputLanguage)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdateClientDefinedReferenceItemPNROutputLanguage_v1(
				clientDefinedReferenceItemPNROutputLanguage.ClientDefinedReferenceItemPNROutputId,
				clientDefinedReferenceItemPNROutputLanguage.CurrentLanguageCode,
				clientDefinedReferenceItemPNROutputLanguage.LanguageCode,
				clientDefinedReferenceItemPNROutputLanguage.RemarkTranslation,
				clientDefinedReferenceItemPNROutputLanguage.VersionNumber,
				adminUserGuid
			);
		}

		//Delete Item
		public void Delete(ClientDefinedReferenceItemPNROutputLanguage clientDefinedReferenceItemPNROutputLanguage)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeleteClientDefinedReferenceItemPNROutputLanguage_v1(
				clientDefinedReferenceItemPNROutputLanguage.ClientDefinedReferenceItemPNROutputId,
				clientDefinedReferenceItemPNROutputLanguage.LanguageCode,
				clientDefinedReferenceItemPNROutputLanguage.VersionNumber,
				adminUserGuid
			);
		}

    }
}