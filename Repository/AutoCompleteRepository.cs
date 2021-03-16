using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using System.Linq.Dynamic;

namespace CWTDesktopDatabase.Repository
{
    public class AutoCompleteRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        /*
         * AutoComplete on CountryName, returns CountryName+CountryCodes
         */
        public List<City> AutoCompleteCities(string searchText)
        {
            var result = from n in db.spDesktopDataAdmin_AutoCompleteCities_v1(searchText)
                         select
                         new City
                         {
                             CityCode = n.CityCode,
                             Name = n.Name
                         };
            return result.ToList();
        }

        /*
         * AutoComplete on ClientTopUnitName returns ClientTopUnitName+ClientTopUnituidG
         */
        public List<ClientTopUnit> AutoCompleteClientTopUnitName(string searchText)
        {
            var result = from n in db.spDesktopDataAdmin_AutoCompleteClientTopUnitName_v1(searchText)
                         select
                         new ClientTopUnit
                         {
                             ClientTopUnitName = n.ClientTopUnitName,
                             ClientTopUnitGuid = n.ClientTopUnitGuid
                         };
            return result.ToList();
        }
		
		/*
         * AutoComplete on ClientAccountNumber returns AccountName, AccountNumber + SystemSourceCode
         */
		public List<ClientAccountJSON> AutoCompleteClientTopUnitClientAccounts(string searchText, string clientTopUnitGuid)
        {
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = from n in db.spDesktopDataAdmin_AutoCompleteClientTopUnitClientAccounts(clientTopUnitGuid, searchText, adminUserGuid)
                         select
						 new ClientAccountJSON
						 {
							 HierarchyName = n.ClientAccountName.Trim(),
							 ClientAccountNumber = n.ClientAccountNumber.ToString(),
							 SourceSystemCode = n.SourceSystemCode.ToString()
						 }; ;
            return result.ToList();
        }

        /*
 * AutoComplete on CountryName, returns CountryName+CountryCodes
 */
        public List<Country> AutoCompleteCountries(string searchText)
        {
            var result = from n in db.spDesktopDataAdmin_AutoCompleteCountries_v1(searchText)
                         select
                         new Country
                         {
                             CountryCode = n.CountryCode,
                             CountryName = n.CountryName
                         };
            return result.ToList();
        }

        /*
         * Not really an AutoComplete, reurn the ValidTo date for a given CreditCardId
         */
        public DateTime CreditCardValidTo(int creditCardId)
        {
            var result = from n in db.CreditCards.Where(c => c.CreditCardId == creditCardId) select n;
            return result.First().CreditCardValidTo;
        }
        /*
         * AutoComplete on LocationName, returns Location+o information
         */
        public List<spDesktopDataAdmin_AutoCompleteLocationsAndCountries_v1Result> AutoCompleteLocationsAndCountries(string searchText)
        {
            
           
            var result = from n in db.spDesktopDataAdmin_AutoCompleteLocationsAndCountries_v1(searchText) select n;
            return result.ToList();
        }

        /*
       * AutoComplete on LocationName, returns Country Information
       */
        public List<Country> AutoCompleteLocationCountries(string searchText)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = from n in db.spDesktopDataAdmin_AutoCompleteLocationCountries_v1(searchText, adminUserGuid)
                         select 
                         new Country
                         {
                                CountryCode = n.CountryCode,
                               CountryName =n.CountryName
                         };
            return result.ToList();
        }

        /*
       * AutoComplete on LocationName, returns Country Information
       */
        public List<Country> AutoCompleteLocationCountriesByRole(string searchText, string roleName)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = from n in db.spDesktopDataAdmin_AutoCompleteCountriesByRoleLocation_v1(searchText, adminUserGuid, roleName)
                         select
                         new Country
                         {
                             CountryCode = n.CountryCode,
                             CountryName = n.CountryName
                         };
            return result.ToList();
        }

        /*
       * AutoComplete on LocationName, returns CountryRegion information
       */
        public List<CountryRegion> AutoCompleteLocationCountryRegion(string countryCode)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = from n in db.spDesktopDataAdmin_AutoCompleteLocationCountryCountryRegions_v1(countryCode, adminUserGuid)
                         select
                         new CountryRegion
                         {
                             CountryRegionId = n.CountryRegionId,
                             CountryRegionName = n.CountryRegionName
                         };
            return result.ToList();
        }
        /*
         * AutoComplete ClientAccounts for a Domain based on Curent Users Access Rights
         */
        public List<ClientAccountJSON> AutoCompleteClientAccounts(string searchText, string domainName)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = from n in db.spDesktopDataAdmin_AutoCompleteHierarchyClientAccounts(adminUserGuid, domainName, searchText)
                         select
                             new ClientAccountJSON
                             {
                                 HierarchyName = n.ClientAccountName.Trim(),
                                 ClientAccountNumber = n.ClientAccountNumber.ToString(),
                                 SourceSystemCode = n.SourceSystemCode.ToString()
                             };
            return result.ToList();
        }

        /*
        * AutoComplete Suppliers of a Product (for ClientDetail only)
        */
        public List<Supplier> AutoCompleteClientDetailProductSuppliers(string searchText, int clientDetailId, int productId)
        {
            var result = from n in db.spDesktopDataAdmin_AutoCompleteClientDetailProductSuppliers_v1(clientDetailId, productId, searchText)
                         select new Supplier
                         {
                             SupplierCode = n.SupplierCode.Trim(),
                             SupplierName = n.SupplierName.Trim(),
                         };
            return result.ToList();
        }

		/*
		* AutoComplete Meetings
		*/
		public List<Meeting> AutoCompleteAvailableMeetings(string hierarchyType, string hierarchyItem, string clientAccountNumber, string sourceSystemCode, string travelerTypeGuid, int resultCount = 5000)
        {
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            var result = from n in db.spDesktopDataAdmin_AutoCompleteMeetings_v1(hierarchyType, hierarchyItem, sourceSystemCode, clientAccountNumber, travelerTypeGuid, adminUserGuid)
                         select new Meeting
                         {
                             MeetingID = n.MeetingID,
                             MeetingName = (n.MeetingName != null) ? n.MeetingName.Trim() : (""),
							 MeetingReferenceNumber = (n.MeetingReferenceNumber != null) ? n.MeetingReferenceNumber.Trim() : (""),
                             MeetingDisplayName = ((n.MeetingName != null) ? n.MeetingName.Trim() : ("")) + " - " + ((n.MeetingReferenceNumber != null) ? n.MeetingReferenceNumber.Trim() : (""))
                         };
            return result.Take(resultCount).ToList();
        }

		/*
		* AutoComplete Partners
		*/
		public List<PartnerJSON> AutoCompletePartners(string searchText, string domainName)
        {
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			var result = from n in db.spDesktopDataAdmin_AutoCompletePartners_v1(searchText, domainName, adminUserGuid)
						 select new PartnerJSON
                         {
							 PartnerId = n.PartnerId,
							 PartnerName = n.PartnerName.Trim(),
							 CountryName = n.CountryName.Trim(),
                         };
            return result.ToList();
        }

        /*
        * AutoComplete Suppliers of a Product
        */
        public List<Supplier> AutoCompleteProductSuppliers(string searchText, int productId)
        {
            var result = from n in db.spDesktopDataAdmin_AutoCompleteProductSuppliers_v1(productId, searchText)
                         select new Supplier
                         {
                             SupplierCode = n.SupplierCode.Trim(),
                             SupplierName = n.SupplierName.Trim(),
                         };
            return result.ToList();
        }

        /*
        * AutoComplete on CountryName, returns CountryName+CountryCodes
        */
       /*public List<ValidPseudoCityOrOfficeId> AutoCompletePseudoCityOrOfficeId(string searchText, string gdsCode)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = from n in db.spDesktopDataAdmin_AutoCompletePseudoCityOrOfficeIds_v1(searchText, gdsCode)
                         select
                         new ValidPseudoCityOrOfficeId
                         {
                             PseudoCityOrOfficeId = n.PseudoCityOrOfficeId
                         };
            return result.ToList();
        }*/


	  /*
	   * AutoComplete SystemUsers
	   */
		public List<SystemUser> AutoCompleteSystemUsers(string searchText)
		{

			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = from n in db.spDesktopDataAdmin_AutoCompleteSystemUsers_v1(searchText, adminUserGuid)
						 select
						new SystemUser
						{
							SystemUserGuid = n.SystemUserGuid,
							FirstName = n.FirstName,
							MiddleName = n.MiddleName,
							LastName = n.LastName,
							IsActiveFlag = n.IsActiveFlag,
							SystemUserLoginIdentifier = n.SystemUserLoginIdentifier,
							UserProfileIdentifier= n.UserProfileIdentifier
						};
			return result.ToList();

		}

	 /*
	  * AutoComplete Locations for a SystemUser
	  */
		public List<HierarchyJSON> AutoCompleteSystemUserLocations(string searchText)
		{

			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = from n in db.spDesktopDataAdmin_AutoCompleteLocations_v1(searchText, adminUserGuid)
						 select
						new HierarchyJSON
						{
							HierarchyName = n.LocationName.Trim(),
							HierarchyCode = n.LocationId.ToString(),
							ParentName = n.CountryName
						};
			return result.ToList();

		}

        /*
       * AutoComplete ClientSubUnits for a SystemUser that can be added to a Team
       */
        public List<spDesktopDataAdmin_AutoCompleteTeamAvailableClientSubUnits_v1Result> TeamAvailableClientSubUnits(int id, string searchText)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = from n in db.spDesktopDataAdmin_AutoCompleteTeamAvailableClientSubUnits_v1(id, adminUserGuid, searchText) select n;
            return result.ToList();
        }

        /*
        * AutoComplete ClientSubUnits for a SystemUser that can be added to a Team
        */
        public List<spDesktopDataAdmin_AutoCompleteTeamOutOfOfficeItemBackupTeams_v1Result> TeamOutOfOfficeItemBackupTeams(int id, string searchText)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = from n in db.spDesktopDataAdmin_AutoCompleteTeamOutOfOfficeItemBackupTeams_v1(id, searchText, adminUserGuid) select n;
            return result.ToList();
        }

		/*
         * AutoComplete GetStateProvincesByCountryCodes
         */
		public List<StateProvinceJSON> GetStateProvincesByCountryCode(string countryCode)
		{
			var result = from n in db.StateProvinces.Where(c => c.CountryCode == countryCode)
						 select
							new StateProvinceJSON
							{
								StateProvinceCode = n.StateProvinceCode.Trim(),
								Name = n.Name.ToString()
							}; ;
			return result.ToList();
		}

        /*
		 * AutoComplete GetClientTelephonyHierarchyName
		 */
        public List<ClientTelephonyJSON> GetClientTelephonyHierarchyName(string searchText, string hierarchyType)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = from n in db.spDesktopDataAdmin_AutoCompleteClientTelephonyHierarchyName_v1(searchText, hierarchyType, adminUserGuid)
                         select
                        new ClientTelephonyJSON
                        {
                            ClientSubUnitGuid = n.ClientSubUnitGuid.Trim(),
                            ClientSubUnitName = n.ClientSubUnitName.Trim(),
                            ClientTopUnitGuid = n.ClientTopUnitGuid.Trim(),
                            ClientTopUnitName = n.ClientTopUnitName.Trim()
                        };
            return result.ToList();
        }

        /*
		 * AutoComplete ClientTopUnitMatrixDPCodes
		 */
        public List<HierarchyJSON> GetClientTopUnitMatrixDPCodes(string searchText, string hierarchyType, string clientTopUnitGuid)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = from n in db.spDesktopDataAdmin_AutoCompleteClientTopUnitMatrixDPCodes_v1(searchText, hierarchyType, clientTopUnitGuid)
                         select
                        new HierarchyJSON
                        {
                            HierarchyName = n.HierarchyName.ToString(),
                            HierarchyCode = n.HierarchyCode.ToString(),
                        };
            return result.ToList();
        }
    }
}