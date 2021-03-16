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
    public class ClientFeeLanguageRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get a Page of ClientFeelanguages - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientFeeLanguageDescription_v1Result> PageClientFeeLanguageDescription(int clientFeeId, int page, string sortField, int sortOrder)
        {
            //get a page of records
            var result = db.spDesktopDataAdmin_SelectClientFeeLanguageDescription_v1(clientFeeId, sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectClientFeeLanguageDescription_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get one Item
        public ClientFeeLanguage GetItem(int clientFeeId, string languageCode)
        {
            return db.ClientFeeLanguages.SingleOrDefault(c => (c.ClientFeeId == clientFeeId)
                    && (c.LanguageCode == languageCode));
        }

        //Add Data From Linked Tables for Display
        /*
         * public void EditItemForDisplay(ClientFeeLanguage clientFeeLanguage)
        {
            //Add LanguageName
            if (clientFeeLanguage.LanguageCode != null)
            {
                LanguageRepository languageRepository = new LanguageRepository();
                Language language = new Language();
                language = languageRepository.GetLanguage(policyAirCabinGroupItemLanguage.LanguageCode);
                if (language != null)
                {
                    clientFeeLanguage.LanguageName = language.LanguageName;
                }
            }

            //Add PolicyGroup Information
            PolicyAirCabinGroupItemRepository policyAirCabinGroupItemRepository = new PolicyAirCabinGroupItemRepository();
            PolicyAirCabinGroupItem policyAirCabinGroupItem = new PolicyAirCabinGroupItem();
            policyAirCabinGroupItem = policyAirCabinGroupItemRepository.GetPolicyAirCabinGroupItem(policyAirCabinGroupItemLanguage.PolicyAirCabinGroupItemId);


            if (policyAirCabinGroupItem != null)
            {
                policyAirCabinGroupItemRepository.EditItemForDisplay(policyAirCabinGroupItem);
                policyAirCabinGroupItemLanguage.PolicyGroupName = policyAirCabinGroupItem.PolicyGroupName;
                policyAirCabinGroupItemLanguage.PolicyGroupId = policyAirCabinGroupItem.PolicyGroupId;
            }

        }
        */
        //Languages not used by this ClientFee
        public List<Language> GetUnUsedLanguages(int clientFeeId)
        {

            var result = from n in db.spDesktopDataAdmin_SelectClientFeeAvailableLanguages_v1(clientFeeId)
                         select new Language
                         {
                             LanguageName = n.LanguageName,
                             LanguageCode = n.LanguageCode
                         };
            return result.ToList();
        }

        //Add to DB
        public void Add(ClientFeeLanguage clientFeeLanguage)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertClientFeeLanguageDescription_v1(
                clientFeeLanguage.ClientFeeId,
                clientFeeLanguage.LanguageCode,
                clientFeeLanguage.ClientFeeLanguageDescription,
                adminUserGuid
                );

        }

        //Delete From DB
        public void Delete(ClientFeeLanguage clientFeeLanguage)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeleteClientFeeLanguageDescription_v1(
               clientFeeLanguage.ClientFeeId,
               clientFeeLanguage.LanguageCode,
               adminUserGuid,
               clientFeeLanguage.VersionNumber
               );
        }

        //Change the deleted status on an item
        public void Update(ClientFeeLanguage clientFeeLanguage)
        {

            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            db.spDesktopDataAdmin_UpdateClientFeeLanguageDescription_v1(
                clientFeeLanguage.ClientFeeId,
                clientFeeLanguage.LanguageCode,
                clientFeeLanguage.ClientFeeLanguageDescription,
                adminUserGuid,
                clientFeeLanguage.VersionNumber
                );

        }

    }
}
