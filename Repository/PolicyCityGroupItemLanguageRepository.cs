using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Linq.Dynamic;

namespace CWTDesktopDatabase.Repository
{
    public class PolicyCityGroupItemLanguageRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get a Page of AirlineAdvice - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyCityGroupItemCityAdvice_v1Result> PagePolicyCityGroupItemCityAdvice(int policyAirVendorGroupItemId, int page, string sortField, int sortOrder)
        {
            //get a page of records
            var result = db.spDesktopDataAdmin_SelectPolicyCityGroupItemCityAdvice_v1(policyAirVendorGroupItemId, sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPolicyCityGroupItemCityAdvice_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        
        //Get one Item
        public PolicyCityGroupItemLanguage GetItem(int policyCityGroupItemId, string languageCode)
        {
            return db.PolicyCityGroupItemLanguages.SingleOrDefault(c => (c.PolicyCityGroupItemId == policyCityGroupItemId)
                    && (c.LanguageCode == languageCode));
        }

        //Languages not used by this policyCityGroupItem
        public List<Language> GetUnUsedLanguages(int policyCityGroupItemId)
        {

            var result = from n in db.spDesktopDataAdmin_SelectPolicyCityGroupItemAvailableLanguages_v1(policyCityGroupItemId)
                         select new Language
                         {
                             LanguageName = n.LanguageName,
                             LanguageCode = n.LanguageCode
                         };
            return result.ToList();
        }

        //Add Data From Linked Tables for Display
        public void EditItemForDisplay(PolicyCityGroupItemLanguage policyCityGroupItemLanguage)
        {
            //Add LanguageName
            if (policyCityGroupItemLanguage.LanguageCode != null)
            {
                LanguageRepository languageRepository = new LanguageRepository();
                Language language = new Language();
                language = languageRepository.GetLanguage(policyCityGroupItemLanguage.LanguageCode);
                if (language != null)
                {
                    policyCityGroupItemLanguage.LanguageName = language.LanguageName;
                }
            }

            //Add PolicyGroupName
            PolicyCityGroupItemRepository policyCityGroupItemRepository = new PolicyCityGroupItemRepository();
            PolicyCityGroupItem policyCityGroupItem = new PolicyCityGroupItem();
            policyCityGroupItem = policyCityGroupItemRepository.GetPolicyCityGroupItem(policyCityGroupItemLanguage.PolicyCityGroupItemId);


            if (policyCityGroupItem != null)
            {
                PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();
                PolicyGroup policyGroup = new PolicyGroup();
                policyGroup = policyGroupRepository.GetGroup(policyCityGroupItem.PolicyGroupId);

                //policyCityGroupItemRepository.EditItemForDisplay(policyCityGroupItem);
                policyCityGroupItemLanguage.PolicyGroupName = policyGroup.PolicyGroupName;
                policyCityGroupItemLanguage.PolicyGroupId = policyCityGroupItem.PolicyGroupId;
            }

        }


        //Add to DB
        public void Add(PolicyCityGroupItemLanguage policyCityGroupItemLanguage)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertPolicyCityGroupItemCityAdvice_v1(
                policyCityGroupItemLanguage.PolicyCityGroupItemId,
                policyCityGroupItemLanguage.LanguageCode,
                policyCityGroupItemLanguage.CityAdvice,
                adminUserGuid
                );

        }

        //Delete From DB
        public void Delete(PolicyCityGroupItemLanguage policyCityGroupItemLanguage)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeletePolicyCityGroupItemCityAdvice_v1(
               policyCityGroupItemLanguage.PolicyCityGroupItemId,
               policyCityGroupItemLanguage.LanguageCode,
               adminUserGuid,
               policyCityGroupItemLanguage.VersionNumber
               );
        }

        //Change the deleted status on an item
        public void Update(PolicyCityGroupItemLanguage policyCityGroupItemLanguage)
        {

            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            db.spDesktopDataAdmin_UpdatePolicyCityGroupItemCityAdvice_v1(
                policyCityGroupItemLanguage.PolicyCityGroupItemId,
                policyCityGroupItemLanguage.LanguageCode,
                policyCityGroupItemLanguage.CityAdvice,
                adminUserGuid,
                policyCityGroupItemLanguage.VersionNumber
                );

        }
    }
}
