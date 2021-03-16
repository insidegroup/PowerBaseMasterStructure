using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using System.Linq.Dynamic;
using CWTDesktopDatabase.Helpers;


namespace CWTDesktopDatabase.Repository
{
    public class ClientSubUnitCreateProfileAdviceRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Sortable List
		public CWTPaginatedList<spDesktopDataAdmin_SelectClientSubUnitCreateProfileAdviceLanguages_v1Result> GetProfileAdviceLanguages(string clientSubUnitGuid, int page, string sortField, int sortOrder)
		{
			//get a page of records
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectClientSubUnitCreateProfileAdviceLanguages_v1(clientSubUnitGuid, sortField, sortOrder, page, adminUserGuid).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}

			//put into page object
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectClientSubUnitCreateProfileAdviceLanguages_v1Result>(result, page, totalRecords);

			//return to user
			return paginatedView;
		}

        //Get one Item
        public ClientSubUnitCreateProfileAdvice GetItem(string clientSubUnitGuid, string languageCode)
        {
            return db.ClientSubUnitCreateProfileAdvices.SingleOrDefault(c => (c.ClientSubUnitGuid == clientSubUnitGuid)
                    && (c.LanguageCode == languageCode));
        }

        //Add Data From Linked Tables for Display
        public void EditItemForDisplay(ClientSubUnitCreateProfileAdvice clientSubUnitCreateProfileAdvice)
        {
            //Add LanguageName
            if (clientSubUnitCreateProfileAdvice.LanguageCode != null)
            {
                LanguageRepository languageRepository = new LanguageRepository();
                Language language = new Language();
                language = languageRepository.GetLanguage(clientSubUnitCreateProfileAdvice.LanguageCode);
                if (language != null)
                {
                    clientSubUnitCreateProfileAdvice.LanguageName = language.LanguageName;
                }
            }

            //Add ClientSubUnitName
            ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientSubUnitCreateProfileAdvice.ClientSubUnitGuid);
            clientSubUnitCreateProfileAdvice.ClientSubUnitDisplayName = clientSubUnit.ClientSubUnitDisplayName;

        }

        //Languages not used by this policyCarVendorGroupItem
        public List<Language> GetUnUsedLanguages(string clientSubUnitGuid)
        {

            var result = from n in db.spDesktopDataAdmin_SelectClientSubUnitCreateProfileAdviceAvailableLanguages_v1(clientSubUnitGuid)
                         select new Language
                         {
                             LanguageName = n.LanguageName,
                             LanguageCode = n.LanguageCode
                         };
            return result.ToList();
        }

        //Add to DB
        public void AddCreateProfileAdvice(ClientSubUnitCreateProfileAdvice clientSubUnitCreateProfileAdvice)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            db.spDesktopDataAdmin_InsertClientSubUnitCreateProfileAdvice_v1(
                clientSubUnitCreateProfileAdvice.ClientSubUnitGuid,
                clientSubUnitCreateProfileAdvice.LanguageCode,
                clientSubUnitCreateProfileAdvice.CreateProfileAdvice,
                adminUserGuid);

        }


        //Delete From DB
        public void DeleteCreateProfileAdvice(ClientSubUnitCreateProfileAdvice clientSubUnitCreateProfileAdvice)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeleteClientSubUnitCreateProfileAdvice_v1(
                clientSubUnitCreateProfileAdvice.ClientSubUnitGuid,
                clientSubUnitCreateProfileAdvice.LanguageCode,
                adminUserGuid,
                clientSubUnitCreateProfileAdvice.VersionNumber
                );
        }

        //Change the deleted status on an item
        public void UpdateCreateProfileAdvice(string clientSubUnitGuid, string languageCode, string createProfileAdvice, int versionNumber)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            db.spDesktopDataAdmin_UpdateClientSubUnitCreateProfileAdvice_v1(
                clientSubUnitGuid, 
                languageCode, 
                createProfileAdvice, 
                adminUserGuid, 
                versionNumber);

        }

    }
}
