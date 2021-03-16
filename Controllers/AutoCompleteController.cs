using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Repository;
using CWTDesktopDatabase.Helpers;
using System.Linq.Dynamic;

namespace MvcApplication1.Controllers
{
    [AjaxTimeOutCheck]
    public class AutoCompleteController : Controller
    {
        /*
         * We will move All AutoCompletes to their own AutoCompleteController and AutoCompleteRepository
         * Many are used several times throughout the site resulting in Code Duplication
         * Stored Procedures Will be renamed to spDesktopDataAdmin_AutoCompleteXXX with Select top 15
         *      as many of the Autocompletes are currently using stored Procedurees which are used
         *      for validation too meaning they return all records even though we only display 15
         *  
         * AjaxTimeOutCheck - cause timeout page to be loaded in main page instead of div
         * 
         *   
         * NAMING CONVENTIONS
         * Locations >all items
         * Admin_Locations > only items a user has access to
         * CountryLocations > all items which are children of another item
         * Admin_CountryLocations > all items which are children of another item,  and only those that a user has access to
         * 
         * All Items should use the [HttpPost] Attribute and return Json()
         */
        AutoCompleteRepository autoCompleteRepository = new AutoCompleteRepository();

        // POST: AutoComplete Cities (from All Cities, no Access Rights checks)
        [HttpPost]
        public JsonResult Cities(string searchText)
        {
            return Json(autoCompleteRepository.AutoCompleteCities(searchText));
        }

		// POST: AutoComplete ClientTopUnitNames (no Access Rights checks)
		[HttpPost]
		public JsonResult ClientTopUnitName(string searchText)
		{
			return Json(autoCompleteRepository.AutoCompleteClientTopUnitName(searchText));
		}

		// POST: AutoComplete ClientTopUnitNames (no Access Rights checks)
		[HttpPost]
		public JsonResult ClientTopUnitClientAccounts(string searchText, string clientTopUnitGuid)
		{
			return Json(autoCompleteRepository.AutoCompleteClientTopUnitClientAccounts(searchText, clientTopUnitGuid));
		}

        // POST: AutoComplete Country (from All Countries, no Access Rights checks)
        [HttpPost]
        public JsonResult Countries(string searchText)
        {
            return Json(autoCompleteRepository.AutoCompleteCountries(searchText));
        }

          // POST: Get ValidTo date for a CreditCard
        [HttpPost]
        public JsonResult CreditCardValidTo(int creditCardId)
        {
            return Json(autoCompleteRepository.CreditCardValidTo(creditCardId));
        }

        // POST: AutoComplete Hierarchies
        [HttpPost]
        public JsonResult Hierarchies(string searchText, string hierarchyItem, string domainName, int resultCount = 15)
        {

            if (hierarchyItem == "ClientAccount")
            {
                return Json(autoCompleteRepository.AutoCompleteClientAccounts(searchText, domainName));
            }
            HierarchyRepository hierarchyRepository = new HierarchyRepository();
			return Json(hierarchyRepository.LookUpSystemUserHierarchies(searchText, hierarchyItem, resultCount, domainName, true));

        }

        // POST: AutoComplete Hierarchies
        [HttpPost]
		public JsonResult AvailableHierarchies(string searchText, string hierarchyItem, string domainName, int resultCount = 15)
        {
            //only used for Productgroups which has no ClientAccount Hierarchies
            //if (hierarchyItem == "ClientAccount")
            //{
            //    return Json(autoCompleteRepository.AutoCompleteClientAccounts(searchText, domainName));
            //}
            HierarchyRepository hierarchyRepository = new HierarchyRepository();
			return Json(hierarchyRepository.LookUpSystemUserHierarchies(searchText, hierarchyItem, resultCount, domainName, false));

        }

        // POST: AutoComplete LocationsAndCountries
        //searchText is on %LocationName%
        //returns LocationId, LocationName, CountryName (no Access Rights checks)
        [HttpPost]
        public JsonResult LocationsAndCountries(string searchText)
        {
            return Json(autoCompleteRepository.AutoCompleteLocationsAndCountries(searchText));
        }

        // POST: AutoComplete Countries
        //searchText is on %LocationName%
        //returns CountryCode, CountryName of COuntries based on "Location" Role
        [HttpPost]
        public JsonResult LocationCountries(string searchText)
        {
            return Json(autoCompleteRepository.AutoCompleteLocationCountries(searchText));
        }

        // POST: AutoComplete Countries
        //searchText is on %LocationName%
        //returns CountryCode, CountryName of COuntries based on Role Location
        [HttpPost]
        public JsonResult LocationCountriesByRole(string searchText, string roleName)
        {
            return Json(autoCompleteRepository.AutoCompleteLocationCountriesByRole(searchText, roleName));
        }

        // POST: AutoComplete CountryRegions
        //searchText is on %LocationName%
        //returns CountryRegionId, CountryRegion of COuntries based on "Location" Role
        [HttpPost]
        public JsonResult LocationCountryRegions(string countryCode)
        {
            return Json(autoCompleteRepository.AutoCompleteLocationCountryRegion(countryCode));
        }

		// POST: AutoComplete Availabe Meetings
		[HttpPost]
		public JsonResult AutoCompleteAvailableMeetings(string hierarchyType, string hierarchyItem, string clientAccountNumber, string sourceSystemCode, string travelerTypeGuid, int resultCount)
		{
			var result = autoCompleteRepository.AutoCompleteAvailableMeetings(hierarchyType, hierarchyItem, clientAccountNumber, sourceSystemCode, travelerTypeGuid, resultCount);
			return Json(result);
		}
		
		// POST: AutoComplete Suppliers of a Product
        [HttpPost]
        public JsonResult ClientDetailProductSuppliers(string searchText, int clientDetailId, int productId)
        {
            return Json(autoCompleteRepository.AutoCompleteClientDetailProductSuppliers(searchText, clientDetailId, productId));
        }

		// POST: AutoComplete Partners
		[HttpPost]
		public JsonResult Partners(string searchText, string domainName)
		{
			var result = autoCompleteRepository.AutoCompletePartners(searchText, domainName);
			return Json(result);
		}
		
		// POST: AutoComplete Products of a Supplier
        [HttpPost]
        public JsonResult ProductSuppliers(string searchText, int productId)
        {
            var result = autoCompleteRepository.AutoCompleteProductSuppliers(searchText, productId);
            return Json(result);
        }

        // POST: AutoComplete PseudoCityOrOfficeId (based on GDSCode)
        /*[HttpPost]
        public JsonResult PseudoCityOrOfficeId(string searchText, string gdsCode)
        {
            return Json(autoCompleteRepository.AutoCompletePseudoCityOrOfficeId(searchText, gdsCode));
        }*/

        // POST: AutoComplete SystemUser Locations
        [HttpPost]
        public JsonResult SystemUserLocations(string searchText)
        {
            return Json(autoCompleteRepository.AutoCompleteSystemUserLocations(searchText));
        }

		//AutoComplete SystemUsers
		[HttpPost]
		public JsonResult SystemUsers(string searchText)
		{
			var result = autoCompleteRepository.AutoCompleteSystemUsers(searchText);
			return Json(result);
		}

        //AutoComplete SystemUsers (who are not already in a team)
        [HttpPost]
        public JsonResult TeamAvailableClientSubUnits(int id, string searchText)
        {
            var result = autoCompleteRepository.TeamAvailableClientSubUnits(id, searchText);
            return Json(result);
        }

        //AutoComplete Teams
        [HttpPost]
        public JsonResult TeamOutOfOfficeItemBackupTeams(int id, string searchText)
        {
            var result = autoCompleteRepository.TeamOutOfOfficeItemBackupTeams(id, searchText);
            return Json(result);
        }
        
        //AutoComplete StateProvince
        [HttpPost]
		public JsonResult GetStateProvincesByCountryCode(string countryCode)
		{
			return Json(autoCompleteRepository.GetStateProvincesByCountryCode(countryCode));
		}

        //AutoComplete TelephonyHierarchyName
        [HttpPost]
        public JsonResult GetClientTelephonyHierarchyName(string searchText, string hierarchyType)
        {
            return Json(autoCompleteRepository.GetClientTelephonyHierarchyName(searchText, hierarchyType));
        }

        //AutoComplete ClientTopUnitMatrixDPCodes
        [HttpPost]
        public JsonResult ClientTopUnitMatrixDPCodes(string searchText, string hierarchyType, string clientTopUnitGuid)
        {
            return Json(autoCompleteRepository.GetClientTopUnitMatrixDPCodes(searchText, hierarchyType, clientTopUnitGuid));
        }

        
    }
}
