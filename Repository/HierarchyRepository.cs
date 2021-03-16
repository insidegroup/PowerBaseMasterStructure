using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Profile;
using CWTDesktopDatabase.Models;



namespace CWTDesktopDatabase.Repository
{
    public class HierarchyRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());
        private ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();

        #region Get an item by its Identifier
        public Location GetLocation(int locationId)
        {
            return db.Locations.SingleOrDefault(c => c.LocationId == locationId);
        }
        public Country GetCountry(string countryCode)
        {
            return db.Countries.SingleOrDefault(c => c.CountryCode == countryCode);
        }
        public GlobalSubRegion GetGlobalSubRegion(string globalSubRegionCode)
        {
            return db.GlobalSubRegions.SingleOrDefault(c => c.GlobalSubRegionCode == globalSubRegionCode);
        }
        public GlobalRegion GetGlobalRegion(string globalRegionCode)
        {
            return db.GlobalRegions.SingleOrDefault(c => c.GlobalRegionCode == globalRegionCode);
        }
        public Global GetGlobal(int globalId)
        {
            return db.Globals.SingleOrDefault(c => c.GlobalId == globalId);
        }
        #endregion

        //Lists for DropDowns
        public List<Global> GetAllGlobals()
        {
            List<Global> globals = new List<Global>();
            var result = from n in db.Globals orderby n.GlobalName select n;
            return result.ToList();
        }
        public List<GlobalRegion> GetAllGlobalRegions()
        {
            List<GlobalRegion> globals = new List<GlobalRegion>();
            var result = from n in db.GlobalRegions orderby n.GlobalRegionName select n;
            return result.ToList();
        }
        public List<GlobalSubRegion> GetAllGlobalSubRegions()
        {
            List<GlobalSubRegion> globals = new List<GlobalSubRegion>();
            var result = from n in db.GlobalSubRegions orderby n.GlobalSubRegionName select n;
            return result.ToList();
        }

        public List<HierarchyJSON> LookUpHierarchies(string searchText, string hierarchyItem, int maxResults)
        {
            var result = new List<HierarchyJSON>();
            switch (hierarchyItem)
            {

                case "Global":
                    result = LookUpGlobals(searchText, maxResults);
                    break;
                case "GlobalRegion":
                    result = LookUpGlobalRegions(searchText, maxResults);
                    break;
                case "GlobalSubRegion":
                    result = LookUpGlobalSubRegions(searchText, maxResults);
                    break;
                case "Country":
                    result = LookUpCountries(searchText, maxResults);
                    break;
                case "CountryRegion":
                    result = LookUpCountryRegions(searchText, maxResults);
                    break;
                case "Location":
                    result = LookUpLocations(searchText, maxResults);
                    break;
                case "ClientTopUnit":
                    result = LookUpClientTopUnits(searchText, maxResults);
                    break;
                case "ClientSubUnit":
                    result = LookUpClientSubUnits(searchText, maxResults);
                    break;
                case "ClientSubUnitTravelerType":
                    result = LookUpClientSubUnitTravelerTypes(searchText, maxResults);
                    break;
                default:
                    result = LookUpGlobals(searchText, maxResults);
                    break;
            }
            return result;
        }
        public List<HierarchyJSON> LookUpSystemUserClientSubUnitTravelerTypes(string searchText, string hierarchyItem, int maxResults, string roleName, string filterText)
        {
            var result = new List<HierarchyJSON>();
            switch (hierarchyItem)
            {

                case "ClientSubUnit":   // linked item from table ClientSubUnitTravelerType.ClientSubUnit
                    result = LookUpSystemUserClientSubUnitTravelerType_ClientSubUnits(searchText, maxResults, roleName, filterText);
                    break;
                case "TravelerType":   // linked item from table ClientSubUnitTravelerType.TravelerType
                    result = LookUpSystemUserClientSubUnitTravelerType_Travelers(searchText, maxResults, roleName, filterText);
                    break;
            }
            return result;
        }
        public List<HierarchyJSON> LookUpSystemUserHierarchies(string searchText, string hierarchyItem, int maxResults, string roleName, bool allowAlreadyUsedItems)
        {
            /*
             *ClientAccount and  ClientSubUnitTravelerTypes have their own functions
             */
            var result = new List<HierarchyJSON>();
            switch (hierarchyItem)
            {

                case "Global":
                    result = LookUpSystemUserGlobals(searchText, maxResults, roleName, allowAlreadyUsedItems);
                    break;
                case "GlobalRegion":
                    result = LookUpSystemUserGlobalRegions(searchText, maxResults, roleName, allowAlreadyUsedItems);
                    break;
                case "GlobalSubRegion":
                    result = LookUpSystemUserGlobalSubRegions(searchText, maxResults, roleName, allowAlreadyUsedItems);
                    break;
                case "Country":
                    result = LookUpSystemUserCountries(searchText, maxResults, roleName, allowAlreadyUsedItems);
                    break;
                case "CountryRegion":
                    result = LookUpSystemUserCountryRegions(searchText, maxResults, roleName, allowAlreadyUsedItems);
                    break;
                case "Location":
                    result = LookUpSystemUserLocations(searchText, maxResults, roleName, allowAlreadyUsedItems);
                    break;
                case "Team":
                    result = LookUpSystemUserTeams(searchText, maxResults, roleName);
                    break;
                case "ClientTopUnit":
                    result = LookUpSystemUserClientTopUnits(searchText, maxResults, roleName);
                    break;
				case "ClientSubUnit":
					result = LookUpSystemUserClientSubUnits(searchText, maxResults, roleName);
					break;
				case "ClientSubUnitGUID":
					result = LookUpSystemUserClientSubUnitGuids(searchText, maxResults, roleName);
					break;
				case "TravelerType":
                    result = LookUpSystemUserTravelerTypes(searchText, maxResults, roleName);
                    break;
                default:
                    result = LookUpSystemUserGlobals(searchText, maxResults, roleName, allowAlreadyUsedItems);
                    break;
            }
            return result;
        }


        #region LookUp item by searchtext

        public List<HierarchyJSON> LookUpGlobals(string searchText, int maxResults)
        {
            var result = from n in db.Globals
                         where n.GlobalName.Contains(searchText)
                         orderby n.GlobalName
                         select
                             new HierarchyJSON
                             {
                                 HierarchyName = n.GlobalName.Trim(),
                                 HierarchyCode = n.GlobalId.ToString()
                             };
            return result.Take(maxResults).ToList();

        }
        public List<HierarchyJSON> LookUpGlobalRegions(string searchText, int maxResults)
        {
            var result = from n in db.GlobalRegions
                         where n.GlobalRegionName.Contains(searchText)
                         orderby n.GlobalRegionName
                         select
                             new HierarchyJSON
                             {
                                 HierarchyName = n.GlobalRegionName.Trim(),
                                 HierarchyCode = n.GlobalRegionCode
                             };
            return result.Take(maxResults).ToList();
        }
        public List<HierarchyJSON> LookUpGlobalSubRegions(string searchText, int maxResults)
        {
            var result = from n in db.GlobalSubRegions
                         where n.GlobalSubRegionName.Contains(searchText)
                         orderby n.GlobalSubRegionName
                         select
                             new HierarchyJSON
                             {
                                 HierarchyName = n.GlobalSubRegionName.Trim(),
                                 HierarchyCode = n.GlobalSubRegionCode
                             };
            return result.Take(maxResults).ToList();
        }
        public List<HierarchyJSON> LookUpCountries(string searchText, int maxResults)
        {
            var result = from n in db.Countries
                         where n.CountryName.Contains(searchText)
                         orderby n.CountryName
                         select
                             new HierarchyJSON
                             {
                                 HierarchyName = n.CountryName.Trim(),
                                 HierarchyCode = n.CountryCode
                             };
            return result.Take(maxResults).ToList();
        }
        public List<HierarchyJSON> LookUpCountryRegions(string searchText, int maxResults)
        {
            var result = from n in db.CountryRegions
                         where n.CountryRegionName.Contains(searchText)
                         orderby n.CountryRegionName
                         select
                             new HierarchyJSON
                             {
                                 HierarchyName = n.CountryRegionName.Trim(),
                                 HierarchyCode = n.CountryRegionId.ToString()
                             };
            return result.Take(maxResults).ToList();
        }
        public List<HierarchyJSON> LookUpLocations(string searchText, int maxResults)
        {
            var result = from n in db.Locations
                         where n.LocationName.Contains(searchText)
                         orderby n.LocationName
                         select
                             new HierarchyJSON
                             {
                                 HierarchyName = n.LocationName.Trim(),
                                 HierarchyCode = n.LocationId.ToString(),
                                 ParentName = n.CountryRegion.CountryRegionName
                             };
            return result.Take(maxResults).ToList();
        }
        public List<HierarchyJSON> LookUpTeams(string searchText, int maxResults)
        {
            var result = from n in db.Teams
                         where n.TeamName.Contains(searchText)
                         orderby n.TeamName
                         select
                             new HierarchyJSON
                             {
                                 HierarchyName = n.TeamName.Trim(),
                                 HierarchyCode = n.TeamId.ToString()
                             };
            return result.Take(maxResults).ToList();
        }
        public List<HierarchyJSON> LookUpClientTopUnits(string searchText, int maxResults)
        {
            var result = from n in db.ClientTopUnits
                         where n.ClientTopUnitName.Contains(searchText)
                         orderby n.ClientTopUnitName
                         select
                             new HierarchyJSON
                             {
                                 HierarchyName = n.ClientTopUnitName.Trim(),
                                 HierarchyCode = n.ClientTopUnitGuid.ToString()
                             };
            return result.Take(maxResults).ToList();
        }
        public List<HierarchyJSON> LookUpClientSubUnits(string searchText, int maxResults)
        {
            var result = from n in db.ClientSubUnits
                         where n.ClientSubUnitName.Contains(searchText)
                         orderby n.ClientSubUnitName
                         select
                             new HierarchyJSON
                             {
                                 HierarchyName = n.ClientSubUnitName.Trim(),
                                 HierarchyCode = n.ClientSubUnitGuid.ToString()
                             };
            return result.Take(maxResults).ToList();
        }
        public List<HierarchyJSON> LookUpClientAccounts(string searchText, int maxResults)
        {
            var result = from n in db.ClientAccounts
                         where n.ClientAccountName.Contains(searchText)
                         orderby n.ClientAccountName
                         select
                             new HierarchyJSON
                             {
                                 HierarchyName = n.ClientAccountName.Trim(),
                                 HierarchyCode = n.ClientAccountNumber.ToString()
                             };
            return result.Take(maxResults).ToList();
        }
        public List<HierarchyJSON> LookUpClientSubUnitTravelerTypes(string searchText, int maxResults)
        {
            var result = from n in db.TravelerTypes
                         where n.TravelerTypeName.Contains(searchText)
                         orderby n.TravelerTypeName
                         select
                             new HierarchyJSON
                             {
                                 HierarchyName = n.TravelerTypeName.Trim(),
                                 HierarchyCode = n.TravelerTypeGuid
                             };
            return result.Take(maxResults).ToList();
        }
        public List<HierarchyJSON> LookUpTravelerTypes(string searchText, int maxResults)
        {
            var result = from n in db.TravelerTypes
                         where n.TravelerTypeName.Contains(searchText)
                         orderby n.TravelerTypeName
                         select
                             new HierarchyJSON
                             {
                                 HierarchyName = n.TravelerTypeName.Trim(),
                                 HierarchyCode = n.TravelerTypeGuid
                             };
            return result.Take(maxResults).ToList();
        }
        #endregion

        #region LookUp item by searchtext accessable by user

        public List<HierarchyJSON> LookUpSystemUserGlobals(string searchText, int maxResults, string roleName, bool allowAlreadyUsedItems)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            if (allowAlreadyUsedItems)
            {
                var result = from n in db.fnDesktopDataAdmin_SelectSystemUserHierarchyGlobals_v1(adminUserGuid, roleName)
                             where n.GlobalName.Contains(searchText)
                             orderby n.GlobalName
                             select
                                 new HierarchyJSON
                                 {
                                     HierarchyName = n.GlobalName.Trim(),
                                     HierarchyCode = n.GlobalId.ToString(),
                                     ParentName = "",
                                     GrandParentName = ""
                                 };
                return result.Take(maxResults).ToList();
            }
            else
            {
                var result = from n in db.fnDesktopDataAdmin_SelectSystemUserHierarchyAvailableGlobals_v1(adminUserGuid, roleName)
                             where n.GlobalName.Contains(searchText)
                             orderby n.GlobalName
                             select
                                 new HierarchyJSON
                                 {
                                     HierarchyName = n.GlobalName.Trim(),
                                     HierarchyCode = n.GlobalId.ToString(),
                                     ParentName = "",
                                     GrandParentName = ""
                                 };
                return result.Take(maxResults).ToList();
            }

        }
        public List<HierarchyJSON> LookUpSystemUserGlobalRegions(string searchText, int maxResults, string roleName, bool allowAlreadyUsedItems)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            if (allowAlreadyUsedItems)
            {

                var result = from n in db.fnDesktopDataAdmin_SelectSystemUserHierarchyGlobalRegions_v1(
                                    adminUserGuid,
                                    roleName)

                             where (n.GlobalRegionName.Contains(searchText) || n.GlobalRegionCode.Contains(searchText))
                             orderby n.GlobalRegionName
                             select
                                 new HierarchyJSON
                                 {
                                     HierarchyName = n.GlobalRegionName.Trim(),
                                     HierarchyCode = n.GlobalRegionCode.ToString(),
                                     ParentName = "",
                                     GrandParentName = ""
                                 };
                return result.Take(maxResults).ToList();
            }
            else
            {
                var result = from n in db.fnDesktopDataAdmin_SelectSystemUserHierarchyAvailableGlobalRegions_v1(
                                    adminUserGuid,
                                    roleName)

                             where (n.GlobalRegionName.Contains(searchText) || n.GlobalRegionCode.Contains(searchText))
                             orderby n.GlobalRegionName
                             select
                                 new HierarchyJSON
                                 {
                                     HierarchyName = n.GlobalRegionName.Trim(),
                                     HierarchyCode = n.GlobalRegionCode.ToString(),
                                     ParentName = "",
                                     GrandParentName = ""
                                 };
                return result.Take(maxResults).ToList();
            }
        }
        public List<HierarchyJSON> LookUpSystemUserGlobalSubRegions(string searchText, int maxResults, string roleName, bool allowAlreadyUsedItems)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            if (allowAlreadyUsedItems)
            {
                var result = from n in db.fnDesktopDataAdmin_SelectSystemUserHierarchyGlobalSubRegions_v1(
                                    adminUserGuid,
                                    roleName)

                             where (n.GlobalSubRegionName.Contains(searchText) || n.GlobalSubRegionCode.Contains(searchText))
                             orderby n.GlobalSubRegionName
                             select
                                 new HierarchyJSON
                                 {
                                     HierarchyName = n.GlobalSubRegionName.Trim(),
                                     HierarchyCode = n.GlobalSubRegionCode.ToString(),
                                     ParentName = n.GlobalRegionName.ToString().Trim(),
                                     GrandParentName = ""
                                 };
                return result.Take(maxResults).ToList();
            }
            else
            {
                var result = from n in db.fnDesktopDataAdmin_SelectSystemUserHierarchyAvailableGlobalSubRegions_v1(
                                  adminUserGuid,
                                  roleName)

                             where (n.GlobalSubRegionName.Contains(searchText) || n.GlobalSubRegionCode.Contains(searchText))
                             orderby n.GlobalSubRegionName
                             select
                                 new HierarchyJSON
                                 {
                                     HierarchyName = n.GlobalSubRegionName.Trim(),
                                     HierarchyCode = n.GlobalSubRegionCode.ToString(),
                                     ParentName = n.GlobalRegionName.ToString().Trim(),
                                     GrandParentName = ""
                                 };
                return result.Take(maxResults).ToList();
            }
        }
        public List<HierarchyJSON> LookUpSystemUserCountries(string searchText, int maxResults, string roleName, bool allowAlreadyUsedItems)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            if (allowAlreadyUsedItems)
            {
                var result = from n in db.fnDesktopDataAdmin_SelectSystemUserHierarchyCountries_v1(
                                    adminUserGuid,
                                    roleName)
                             where (n.CountryName.Contains(searchText) || n.CountryCode.Contains(searchText))
                             orderby n.CountryName
                             select
                                 new HierarchyJSON
                                 {
                                     HierarchyName = n.CountryName.Trim(),
                                     HierarchyCode = n.CountryCode.ToString(),
                                     ParentName = n.GlobalSubRegionName.ToString().Trim(),
                                     GrandParentName = n.GlobalRegionName.ToString().Trim()
                                 };
                return result.Take(maxResults).ToList();
            }
            else
            {
                var result = from n in db.fnDesktopDataAdmin_SelectSystemUserHierarchyAvailableCountries_v1(
                                    adminUserGuid,
                                    roleName)
                             where (n.CountryName.Contains(searchText) || n.CountryCode.Contains(searchText))
                             orderby n.CountryName
                             select
                                 new HierarchyJSON
                                 {
                                     HierarchyName = n.CountryName.Trim(),
                                     HierarchyCode = n.CountryCode.ToString(),
                                     ParentName = n.GlobalSubRegionName.ToString().Trim(),
                                     GrandParentName = n.GlobalRegionName.ToString().Trim()
                                 };
                return result.Take(maxResults).ToList();
            }
        }
        public List<HierarchyJSON> LookUpSystemUserCountryRegions(string searchText, int maxResults, string roleName, bool allowAlreadyUsedItems)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            if (allowAlreadyUsedItems)
            {
                var result = from n in db.fnDesktopDataAdmin_SelectSystemUserHierarchyCountryRegions_v1(
                                    adminUserGuid,
                                    roleName)

                             where n.CountryRegionName.Contains(searchText)
                             orderby n.CountryRegionName
                             select
                                 new HierarchyJSON
                                 {
                                     HierarchyName = n.CountryRegionName.Trim(),
                                     HierarchyCode = n.CountryRegionId.ToString(),
                                     ParentName = n.CountryName.ToString().Trim(),
                                     GrandParentName = n.GlobalSubRegionName.ToString().Trim()
                                 };
                return result.Take(maxResults).ToList();
            }
            else
            {
                var result = from n in db.fnDesktopDataAdmin_SelectSystemUserHierarchyAvailableCountryRegions_v1(
                                   adminUserGuid,
                                   roleName)

                             where n.CountryRegionName.Contains(searchText)
                             orderby n.CountryRegionName
                             select
                                 new HierarchyJSON
                                 {
                                     HierarchyName = n.CountryRegionName.Trim(),
                                     HierarchyCode = n.CountryRegionId.ToString(),
                                     ParentName = n.CountryName.ToString().Trim(),
                                     GrandParentName = n.GlobalSubRegionName.ToString().Trim()
                                 };
                return result.Take(maxResults).ToList();
            }
        }
        public List<HierarchyJSON> LookUpSystemUserLocations(string searchText, int maxResults, string roleName, bool allowAlreadyUsedItems)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            if (allowAlreadyUsedItems)
            {
                var result = from n in db.fnDesktopDataAdmin_SelectSystemUserHierarchyLocations_v1(
                                   adminUserGuid,
                                   roleName)

                             where n.LocationName.Contains(searchText)
                             orderby n.LocationName
                             select
                                 new HierarchyJSON
                                 {
                                     HierarchyName = n.LocationName.Trim(),
                                     HierarchyCode = n.LocationId.ToString(),
                                     ParentName = n.CountryRegionName.ToString().Trim(),
                                     GrandParentName = n.CountryName.ToString().Trim()
                                 };
                return result.Take(maxResults).ToList();
            }
            else
            {
                var result = from n in db.fnDesktopDataAdmin_SelectSystemUserHierarchyAvailableLocations_v1(
                                    adminUserGuid,
                                    roleName)

                             where n.LocationName.Contains(searchText)
                             orderby n.LocationName
                             select
                                 new HierarchyJSON
                                 {
                                     HierarchyName = n.LocationName.Trim(),
                                     HierarchyCode = n.LocationId.ToString(),
                                     ParentName = n.CountryRegionName.ToString().Trim(),
                                     GrandParentName = n.CountryName.ToString().Trim()
                                 };
                return result.Take(maxResults).ToList();
            }
        }

        public List<HierarchyJSON> LookUpSystemUserTeams(string searchText, int maxResults, string roleName)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = from n in db.fnDesktopDataAdmin_SelectSystemUserHierarchyTeams_v1(
                                adminUserGuid,
                                roleName)

                         where n.TeamName.Contains(searchText)
                         orderby n.TeamName
                         select
                             new HierarchyJSON
                             {
                                 HierarchyName = n.TeamName.Trim(),
                                 HierarchyCode = n.TeamId.ToString(),
                                 ParentName = "",
                                 GrandParentName = ""
                             };
            return result.Take(maxResults).ToList();
        }

		public List<HierarchyJSON> LookUpSystemUserClientSubUnits(string searchText, int maxResults, string roleName)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = from n in db.fnDesktopDataAdmin_SelectSystemUserHierarchyClientSubUnits_v1(
								adminUserGuid,
								roleName)

						 where n.ClientSubUnitName.Contains(searchText)
						 orderby n.ClientSubUnitName
						 select
							 new HierarchyJSON
							 {
								 HierarchyName = n.ClientSubUnitName.Trim(),
								 HierarchyCode = n.ClientSubUnitGuid.ToString(),
								 ParentName = n.ClientTopUnitName.ToString().Trim(),
								 GrandParentName = ""
							 };
			return result.ToList();
		}

		public List<HierarchyJSON> LookUpSystemUserClientSubUnitGuids(string searchText, int maxResults, string roleName)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = from n in db.fnDesktopDataAdmin_SelectSystemUserHierarchyClientSubUnits_v1(
								adminUserGuid,
								roleName)

						 where n.ClientSubUnitGuid.Contains(searchText)
						 orderby n.ClientSubUnitGuid, n.ClientSubUnitName
						 select
							 new HierarchyJSON
							 {
								 HierarchyName = n.ClientSubUnitName.Trim(),
								 HierarchyCode = n.ClientSubUnitGuid.ToString(),
								 ParentName = n.ClientTopUnitName.ToString().Trim(),
								 GrandParentName = ""
							 };
			return result.ToList();
		}

        public List<HierarchyJSON> LookUpSystemUserClientTopUnitClientSubUnits(string searchText, int maxResults, string clientTopUnitGuid, string roleName)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = from n in db.fnDesktopDataAdmin_SelectSystemUserHierarchyClientTopUnitClientSubUnits_v1(
                                adminUserGuid,
                                clientTopUnitGuid,
                                roleName)

                         where n.ClientSubUnitName.Contains(searchText)
                         orderby n.ClientSubUnitName
                         select
                             new HierarchyJSON
                             {
                                 HierarchyName = n.ClientSubUnitName.Trim(),
                                 HierarchyCode = n.ClientSubUnitGuid.ToString(),
                                 ParentName = n.ClientTopUnitName.ToString().Trim(),
                                 GrandParentName = ""
                             };
            return result.ToList();
        }

        public List<HierarchyJSON> LookUpSystemUserClientTopUnits(string searchText, int maxResults, string roleName)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = from n in db.fnDesktopDataAdmin_SelectSystemUserHierarchyClientTopUnits_v1(
                                adminUserGuid,
                                roleName)

                         where n.ClientTopUnitName.Contains(searchText)
                         orderby n.ClientTopUnitName
                         select
                             new HierarchyJSON
                             {
                                 HierarchyName = n.ClientTopUnitName.Trim(),
                                 HierarchyCode = n.ClientTopUnitGuid.ToString(),
                                 ParentName = "",
                                 GrandParentName = ""
                             };
            return result.Take(maxResults).ToList();
        }
        public List<HierarchyJSON> LookUpSystemUserTravelerTypes(string searchText, int maxResults, string roleName)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = from n in db.fnDesktopDataAdmin_SelectSystemUserHierarchyTravelerTypes_v1(
                                adminUserGuid,
                                roleName)

                         where n.TravelerTypeName.Contains(searchText)
                         orderby n.TravelerTypeName
                         select
                             new HierarchyJSON
                             {
                                 HierarchyName = n.TravelerTypeName.Trim(),
                                 HierarchyCode = n.TravelerTypeGuid.ToString(),
                                 ClientSubUnitGuid = n.ClientSubUnitGuid,
                                 ClientSubUnitName = n.ClientSubUnitName,
                                 ClientTopUnitGuid = n.ClientTopUnitGuid,
                                 ClientTopUnitName = n.ClientTopUnitName
                             };
            return result.Take(maxResults).ToList();
        }
        public List<HierarchyJSON> LookUpSystemUserClientTopUnitTravelerTypes(string searchText, int maxResults, string clientTopUnitGuid, string roleName)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = from n in db.fnDesktopDataAdmin_SelectSystemUserHierarchyTravelerTypes_v1(
                                adminUserGuid,
                                roleName)

                         where n.TravelerTypeName.Contains(searchText) && n.ClientTopUnitGuid == clientTopUnitGuid
                         orderby n.TravelerTypeName
                         select
                             new HierarchyJSON
                             {
                                 HierarchyName = n.TravelerTypeName.Trim(),
                                 HierarchyCode = n.TravelerTypeGuid.ToString(),
                                 ClientSubUnitGuid = n.ClientSubUnitGuid,
                                 ClientSubUnitName = n.ClientSubUnitName,
                                 ClientTopUnitGuid = n.ClientTopUnitGuid,
                                 ClientTopUnitName = n.ClientTopUnitName
                             };
            return result.Take(maxResults).ToList();
        }
        public List<HierarchyJSON> LookUpSystemUserClientSubUnitTravelerType_ClientSubUnits(string searchText, int maxResults, string roleName, string traveler)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = from n in db.fnDesktopDataAdmin_SelectSystemUserHierarchyClientSubUnitTravelerTypesClientSubUnits_v1(
                                adminUserGuid,
                                roleName,
                                traveler)

                         where n.ClientSubUnitName.Contains(searchText)
                         orderby n.ClientSubUnitName
                         select
                             new HierarchyJSON
                             {
                                 HierarchyName = n.ClientSubUnitName.Trim(),
                                 HierarchyCode = n.ClientSubUnitGuid.ToString(),
                                 ParentName = n.ClientTopUnitName.ToString().Trim(),
                                 GrandParentName = ""
                             };
            return result.Take(maxResults).ToList();
        }
        public List<HierarchyJSON> LookUpSystemUserClientSubUnitTravelerType_Travelers(string searchText, int maxResults, string roleName, string clientSubUnit)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = from n in db.fnDesktopDataAdmin_SelectSystemUserHierarchyClientSubUnitTravelerTypesTravelers_v1(
                                adminUserGuid,
                                roleName,
                                clientSubUnit
                                )

                         where n.TravelerTypeName.Contains(searchText)
                         orderby n.TravelerTypeName
                         select
                             new HierarchyJSON
                             {
                                 HierarchyName = n.TravelerTypeName.Trim(),
                                 HierarchyCode = n.TravelerTypeGuid.ToString(),
                                 ParentName = "",
                                 GrandParentName = ""
                             };
            return result.Take(maxResults).ToList();
        }



        #endregion

        #region Client Side validate Hierarchy/HierarchyItem combo (returns HierarchyJSON)
        /*
         * These items are used to return a single item as JSON
         * used to validate Hierarchy/HierarchyItem combo
         * Called from client side javascript
         */
        public List<HierarchyJSON> GetGlobalByName(string searchText)
        {

            var result = from n in db.Globals
                         where n.GlobalName.Trim().Equals(searchText)
                         orderby n.GlobalName
                         select
                             new HierarchyJSON
                             {
                                 HierarchyName = n.GlobalName.Trim(),
                                 HierarchyCode = n.GlobalId.ToString()
                             };
            return result.ToList();
        }
        public List<HierarchyJSON> GetGlobalRegionByName(string searchText)
        {
            var result = from n in db.GlobalRegions
                         where n.GlobalRegionName.Trim().Equals(searchText)
                         orderby n.GlobalRegionName
                         select
                             new HierarchyJSON
                             {
                                 HierarchyName = n.GlobalRegionName.Trim(),
                                 HierarchyCode = n.GlobalRegionCode
                             };
            return result.ToList();
        }
        public List<HierarchyJSON> GetGlobalSubRegionByName(string searchText)
        {
            var result = from n in db.GlobalSubRegions
                         where n.GlobalSubRegionName.Trim().Equals(searchText)
                         orderby n.GlobalSubRegionName
                         select
                             new HierarchyJSON
                             {
                                 HierarchyName = n.GlobalSubRegionName.Trim(),
                                 HierarchyCode = n.GlobalSubRegionCode
                             };
            return result.ToList();
        }
        public List<HierarchyJSON> GetCountryByName(string searchText)
        {
            var result = from n in db.Countries
                         where n.CountryName.Trim().Equals(searchText)
                         orderby n.CountryName
                         select
                             new HierarchyJSON
                             {
                                 HierarchyName = n.CountryName.Trim(),
                                 HierarchyCode = n.CountryCode
                             };
            return result.ToList();
        }
        public List<HierarchyJSON> GetCountryRegionByName(string searchText)
        {
            var result = from n in db.CountryRegions
                         where n.CountryRegionName.Trim().Equals(searchText)
                         orderby n.CountryRegionName
                         select
                             new HierarchyJSON
                             {
                                 HierarchyName = n.CountryRegionName.Trim(),
                                 HierarchyCode = n.CountryRegionId.ToString()
                             };
            return result.ToList();
        }
        public List<HierarchyJSON> GetLocationByName(string searchText)
        {
            var result = from n in db.Locations
                         where n.LocationName.Trim().Equals(searchText)
                         orderby n.LocationName
                         select
                             new HierarchyJSON
                             {
                                 HierarchyName = n.LocationName.Trim(),
                                 HierarchyCode = n.LocationId.ToString()
                             };
            return result.ToList();
        }

        public List<HierarchyJSON> GetClientTopUnitByName(string searchText)
        {
            var result = from n in db.ClientTopUnits
                         where n.ClientTopUnitName.Trim().Equals(searchText)
                         orderby n.ClientTopUnitName
                         select
                             new HierarchyJSON
                             {
                                 HierarchyName = n.ClientTopUnitName.Trim(),
                                 HierarchyCode = n.ClientTopUnitGuid.ToString()
                             };
            return result.ToList();
        }
        public List<HierarchyJSON> GetClientSubUnitByName(string searchText)
        {
            var result = from n in db.ClientSubUnits
                         where n.ClientSubUnitName.Trim().Equals(searchText)
                         orderby n.ClientSubUnitName
                         select
                             new HierarchyJSON
                             {
                                 HierarchyName = n.ClientSubUnitName.Trim(),
                                 HierarchyCode = n.ClientSubUnitGuid.ToString()
                             };
            return result.ToList();
        }
        public List<HierarchyJSON> GetClientAccountByName(string searchText)
        {
            var result = from n in db.ClientAccounts
                         where n.ClientAccountName.Trim().Equals(searchText)
                         orderby n.ClientAccountName
                         select
                             new HierarchyJSON
                             {
                                 HierarchyName = n.ClientAccountName.Trim(),
                                 HierarchyCode = n.ClientAccountNumber.ToString()
                             };
            return result.ToList();
        }
        public List<HierarchyJSON> GetTravelerTypeByName(string searchText)
        {
            var result = from n in db.TravelerTypes
                         where n.TravelerTypeName.Trim().Equals(searchText)
                         orderby n.TravelerTypeName
                         select
                             new HierarchyJSON
                             {
                                 HierarchyName = n.TravelerTypeName.Trim(),
                                 HierarchyCode = n.TravelerTypeGuid.ToString()
                             };
            return result.ToList();
        }
        public List<HierarchyJSON> GetClientTelephonyClientByMainNumber(string hierarchyType, string hierarchyItem, int clientTelephonyId)
        {
            var result = from n in db.spDesktopDataAdmin_SelectClientTelephonyClientByMainNumber_v1(hierarchyType, hierarchyItem, clientTelephonyId)
                         select
                             new HierarchyJSON
                             {
                                 HierarchyName = n.HierarchyName.Trim(),
                                 HierarchyCode = n.HierarchyItem.Trim()
                             };
            return result.ToList();
        }
        #endregion

        //Search for a particular PolicyRouting
        public List<PolicyRoutingJSON> LookUpPolicyRoutingFromTo(string searchText, int maxResults)
        {
            var result = from n in db.fnDesktopDataAdmin_SelectPolicyRoutingFromTo_v1()
                         where n.Name.StartsWith(searchText) || n.Code.StartsWith(searchText)
                         orderby n.Name
                         select
                             new PolicyRoutingJSON
                             {
                                 Code = n.Code,
                                 Name = n.Name.Trim(),
                                 Parent = n.Parent.Trim(),
                                 CodeType = n.CodeType
                             };
            return result.Take(maxResults).ToList();
        }

        //List PolicyRouting
        public IQueryable<fnDesktopDataAdmin_SelectPolicyRoutingFromTo_v1Result> ListPolicyRoutingFromTo()
        {
            var result = from n in db.fnDesktopDataAdmin_SelectPolicyRoutingFromTo_v1() orderby n.Name select n;
            return result;
        }

        //List PolicyRouting
        public bool IsValidPolicyRoutingFromTo(string fromTo)
        {
            var result = from n in db.fnDesktopDataAdmin_SelectPolicyRoutingFromTo_v1() where n.Code.Equals(fromTo) select n;

            if (result.Count() == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //can logged-in user add to a domain (to at least one hierarchy)
        // eg can Admin create a PolicyGroup with at least one of the hierarchy values
        public bool AdminHasDomainWriteAccess(string roleName)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            int hasAccess = db.spDesktopDataAdmin_GetAdminUserDomainWriteAccess_v1(
                adminUserGuid,
                roleName
                );

            if (hasAccess == 1) {
                return true;
            } else {
                return false;
            }
        }

        //can  logged-in user add to a domain (to a specific hierarchy type and item)
        // eg can Admin create a PolicyGroup with Country Hierarchy
        public bool AdminHasDomainHierarchyWriteAccess(string hierarchyType, string hierarchyCode, string sourceSystemCode, string roleName)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            return (bool)db.fnDesktopDataAdmin_GetSystemUserWriteAccessToHierarchy_v1(
                hierarchyType,
                hierarchyCode,
                sourceSystemCode,
                adminUserGuid,
                roleName
                );
        }

        public HierarchyGroup GetHierarchyGroup(string hierarchyType, string hierarchyCode, string hierarchyName, string travelerTypeGuid, string travelerTypeName, string sourceSystemCode)
        {
            HierarchyGroup hierachyItem = new HierarchyGroup
            {
                HierarchyType = hierarchyType,
                HierarchyCode = hierarchyCode,
                HierarchyItem = hierarchyName
            };

            if (hierarchyType == "ClientSubUnitTravelerType")
            {
                hierachyItem.ClientSubUnitGuid = hierarchyCode;
                hierachyItem.ClientSubUnitName = hierarchyName;
                hierachyItem.TravelerTypeGuid = travelerTypeGuid;
                hierachyItem.TravelerTypeName = travelerTypeName;
            }

            if (hierarchyType == "TravelerType")
            {
                hierachyItem.TravelerTypeGuid = hierarchyCode;
                hierachyItem.TravelerTypeName = hierarchyName;

                if (!string.IsNullOrEmpty(hierachyItem.TravelerTypeGuid))
                {
                    ClientSubUnitTravelerTypeRepository clientSubUnitTravelerTypeRepository = new ClientSubUnitTravelerTypeRepository();
                    ClientSubUnitTravelerType clientSubUnitTravelerType = clientSubUnitTravelerTypeRepository.GetClientSubUnitTravelerTypes(hierachyItem.TravelerTypeGuid).FirstOrDefault();
                    if (clientSubUnitTravelerType != null)
                    {
                        ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
                        ClientSubUnit clientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientSubUnitTravelerType.ClientSubUnitGuid);
                        if (clientSubUnit != null && clientSubUnit.ClientTopUnit != null)
                        {
                            hierachyItem.ClientTopUnitName = clientSubUnit.ClientTopUnit.ClientTopUnitName;
                        }
                    }
                }
            }

            if (hierarchyType == "ClientSubUnit" || hierarchyType == "ClientSubUnitTravelerType")
            {
                if (!string.IsNullOrEmpty(hierarchyCode))
                {
                    ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
                    ClientSubUnit clientSubUnit = clientSubUnitRepository.GetClientSubUnit(hierarchyCode);
                    if (clientSubUnit != null && clientSubUnit.ClientTopUnit != null)
                    {
                        hierachyItem.ClientTopUnitName = clientSubUnit.ClientTopUnit.ClientTopUnitName;
                    }
                }
            }

            if (hierarchyType == "ClientAccount")
            {
                hierachyItem.SourceSystemCode = sourceSystemCode;
            }

            return hierachyItem;
        }

        public List<ClientSubUnit> GetClientSubUnitHierarchies(List<MultipleHierarchyDefinition> multipleHierarchies)
        {
            List<ClientSubUnit> clientSubUnitsHierarchy = new List<ClientSubUnit>();
            foreach (MultipleHierarchyDefinition item in multipleHierarchies)
            {
                ClientSubUnit clientSubUnit = clientSubUnitRepository.GetClientSubUnit(item.HierarchyCode);
                if (clientSubUnit != null)
                {
                    clientSubUnitsHierarchy.Add(clientSubUnit);
                }
            }

            return clientSubUnitsHierarchy.OrderBy(x => x.ClientSubUnitName).ToList();
        }

        public Dictionary<string, List<MultipleHierarchy>> GetMultipleHierarchies(List<MultipleHierarchyDefinition> multipleHierarchies)
        {
            Dictionary<string, List<MultipleHierarchy>> multipleHierarchyList = new Dictionary<string, List<MultipleHierarchy>>();

            //ClientSubUnitTravelerType
            List<MultipleHierarchy> clientSubUnitTravelerTypes = new List<MultipleHierarchy>();
            foreach (MultipleHierarchyDefinition item in multipleHierarchies.Where(x => x.HierarchyType == "ClientSubUnitTravelerType"))
            {
                ClientSubUnitTravelerTypeRepository clientSubUnitTravelerTypeRepository = new ClientSubUnitTravelerTypeRepository();
                ClientSubUnitTravelerType clientSubUnitTravelerType = clientSubUnitTravelerTypeRepository.GetClientSubUnitTravelerType(item.HierarchyCode, item.TravelerTypeGuid);
                if (clientSubUnitTravelerType != null)
                {
                    clientSubUnitTravelerTypes.Add(new MultipleHierarchy()
                    {
                        Name = clientSubUnitTravelerType.TravelerType.TravelerTypeName,
                        ParentName = clientSubUnitTravelerType.ClientSubUnit.ClientSubUnitName,
                        GrandParentName = clientSubUnitTravelerType.ClientSubUnit.ClientTopUnit.ClientTopUnitName
                    });
                }
            }
            multipleHierarchyList.Add("ClientSubUnitTravelerType", clientSubUnitTravelerTypes.OrderBy(x => x.Name).ThenBy(x => x.ParentName).ThenBy(x => x.GrandParentName).ToList());

            //TravelerType
            List<MultipleHierarchy> travelerTypes = new List<MultipleHierarchy>();
            foreach (MultipleHierarchyDefinition item in multipleHierarchies.Where(x => x.HierarchyType == "TravelerType"))
            {
                TravelerTypeRepository travelerTypeRepository = new TravelerTypeRepository();
                TravelerType travelerType = travelerTypeRepository.GetTravelerType(item.HierarchyCode);
                if (travelerType != null)
                {
                    travelerTypes.Add(new MultipleHierarchy()
                    {
                        Name = travelerType.TravelerTypeName,
                        ParentName = travelerType.ClientTopUnit.ClientTopUnitName
                    });
                }
            }
            multipleHierarchyList.Add("TravelerType", travelerTypes.OrderBy(x => x.Name).ThenBy(x => x.ParentName).ThenBy(x => x.GrandParentName).ToList());

            //ClientAccount
            List<MultipleHierarchy> clientAccounts = new List<MultipleHierarchy>();
            foreach (MultipleHierarchyDefinition item in multipleHierarchies.Where(x => x.HierarchyType == "ClientAccount"))
            {
                ClientAccountRepository clientAccountRepository = new ClientAccountRepository();
                ClientAccount clientAccount = clientAccountRepository.GetClientAccount(item.HierarchyCode, item.SourceSystemCode);
                if (clientAccount != null)
                {
                    clientAccounts.Add(new MultipleHierarchy()
                    {
                        Name = clientAccount.ClientAccountName,
                        ParentName = clientAccount.ClientAccountNumber,
                        GrandParentName = clientAccount.SourceSystemCode
                    });
                }
            }
            multipleHierarchyList.Add("ClientAccount", clientAccounts.OrderBy(x => x.Name).ThenBy(x => x.ParentName).ThenBy(x => x.GrandParentName).ToList());

            //ClientSubUnit
            List<MultipleHierarchy> clientSubUnits = new List<MultipleHierarchy>();
            foreach (MultipleHierarchyDefinition item in multipleHierarchies.Where(x => x.HierarchyType == "ClientSubUnit"))
            {
                ClientSubUnit clientSubUnit = clientSubUnitRepository.GetClientSubUnit(item.HierarchyCode);
                if (clientSubUnit != null)
                {
                    clientSubUnits.Add(new MultipleHierarchy()
                    {
                        Name = clientSubUnit.ClientSubUnitName,
                        ParentName = clientSubUnit.ClientTopUnit.ClientTopUnitName
                    });
                }
            }
            multipleHierarchyList.Add("ClientSubUnit", clientSubUnits.OrderBy(x => x.Name).ThenBy(x => x.ParentName).ThenBy(x => x.GrandParentName).ToList());


            //ClientTopUnit
            List<MultipleHierarchy> clientTopUnits = new List<MultipleHierarchy>();
            foreach (MultipleHierarchyDefinition item in multipleHierarchies.Where(x => x.HierarchyType == "ClientTopUnit"))
            {
                ClientTopUnitRepository clientTopUnitRepository = new ClientTopUnitRepository();
                ClientTopUnit clientTopUnit = clientTopUnitRepository.GetClientTopUnit(item.HierarchyCode);
                if (clientTopUnit != null)
                {
                    clientTopUnits.Add(new MultipleHierarchy()
                    {
                        Name = clientTopUnit.ClientTopUnitName
                    });
                }
            }
            multipleHierarchyList.Add("ClientTopUnit", clientTopUnits.OrderBy(x => x.Name).ThenBy(x => x.ParentName).ThenBy(x => x.GrandParentName).ToList());

            //Team
            List<MultipleHierarchy> teams = new List<MultipleHierarchy>();
            foreach (MultipleHierarchyDefinition item in multipleHierarchies.Where(x => x.HierarchyType == "Team"))
            {
                TeamRepository teamRepository = new TeamRepository();
                Team team = teamRepository.GetTeam(Int32.Parse(item.HierarchyCode));
                if (team != null)
                {
                    teams.Add(new MultipleHierarchy()
                    {
                        Name = team.TeamName
                    });
                }
            }
            multipleHierarchyList.Add("Team", teams.OrderBy(x => x.Name).ThenBy(x => x.ParentName).ThenBy(x => x.GrandParentName).ToList());

            //Location
            List<MultipleHierarchy> locations = new List<MultipleHierarchy>();
            foreach (MultipleHierarchyDefinition item in multipleHierarchies.Where(x => x.HierarchyType == "Location"))
            {
                LocationRepository locationRepository = new LocationRepository();
                Location location = locationRepository.GetLocation(Int32.Parse(item.HierarchyCode));
                if (location != null)
                {
                    locations.Add(new MultipleHierarchy()
                    {
                        Name = location.LocationName
                    });
                }
            }
            multipleHierarchyList.Add("Location", locations.OrderBy(x => x.Name).ThenBy(x => x.ParentName).ThenBy(x => x.GrandParentName).ToList());

            //CountryRegion
            List<MultipleHierarchy> countryRegions = new List<MultipleHierarchy>();
            foreach (MultipleHierarchyDefinition item in multipleHierarchies.Where(x => x.HierarchyType == "CountryRegion"))
            {
                CountryRegionRepository countryRegionRepository = new CountryRegionRepository();
                CountryRegion countryRegion = countryRegionRepository.GetCountryRegion(Int32.Parse(item.HierarchyCode));
                if (countryRegion != null)
                {
                    countryRegions.Add(new MultipleHierarchy()
                    {
                        Name = countryRegion.CountryRegionName
                    });
                }
            }
            multipleHierarchyList.Add("CountryRegion", countryRegions.OrderBy(x => x.Name).ThenBy(x => x.ParentName).ThenBy(x => x.GrandParentName).ToList());

            //Country
            List<MultipleHierarchy> countries = new List<MultipleHierarchy>();
            foreach (MultipleHierarchyDefinition item in multipleHierarchies.Where(x => x.HierarchyType == "Country"))
            {
                CountryRepository countryRepository = new CountryRepository();
                Country country = countryRepository.GetCountry(item.HierarchyCode);
                if (country != null)
                {
                    countries.Add(new MultipleHierarchy()
                    {
                        Name = country.CountryName
                    });
                }
            }
            multipleHierarchyList.Add("Country", countries.OrderBy(x => x.Name).ThenBy(x => x.ParentName).ThenBy(x => x.GrandParentName).ToList());

            //GlobalSubRegion
            List<MultipleHierarchy> globalSubRegions = new List<MultipleHierarchy>();
            foreach (MultipleHierarchyDefinition item in multipleHierarchies.Where(x => x.HierarchyType == "GlobalSubRegion"))
            {
                HierarchyRepository globalSubRegionRepository = new HierarchyRepository();
                GlobalSubRegion globalSubRegion = globalSubRegionRepository.GetGlobalSubRegion(item.HierarchyCode);
                if (globalSubRegion != null)
                {
                    globalSubRegions.Add(new MultipleHierarchy()
                    {
                        Name = globalSubRegion.GlobalSubRegionName
                    });
                }
            }
            multipleHierarchyList.Add("GlobalSubRegion", globalSubRegions.OrderBy(x => x.Name).ThenBy(x => x.ParentName).ThenBy(x => x.GrandParentName).ToList());

            //GlobalRegion
            List<MultipleHierarchy> globalRegions = new List<MultipleHierarchy>();
            foreach (MultipleHierarchyDefinition item in multipleHierarchies.Where(x => x.HierarchyType == "GlobalRegion"))
            {
                HierarchyRepository globalRegionRepository = new HierarchyRepository();
                GlobalRegion globalRegion = globalRegionRepository.GetGlobalRegion(item.HierarchyCode);
                if (globalRegion != null)
                {
                    globalRegions.Add(new MultipleHierarchy()
                    {
                        Name = globalRegion.GlobalRegionName
                    });
                }
            }
            multipleHierarchyList.Add("GlobalRegion", globalRegions.OrderBy(x => x.Name).ThenBy(x => x.ParentName).ThenBy(x => x.GrandParentName).ToList());

            //Global
            List<MultipleHierarchy> globals = new List<MultipleHierarchy>();
            foreach (MultipleHierarchyDefinition item in multipleHierarchies.Where(x => x.HierarchyType == "Global"))
            {
                HierarchyRepository globalRepository = new HierarchyRepository();
                Global global = globalRepository.GetGlobal(Int32.Parse(item.HierarchyCode));
                if (global != null)
                {
                    globals.Add(new MultipleHierarchy()
                    {
                        Name = global.GlobalName
                    });
                }
            }
            multipleHierarchyList.Add("Global", globals.OrderBy(x => x.Name).ThenBy(x => x.ParentName).ThenBy(x => x.GrandParentName).ToList());

            return multipleHierarchyList;
        }
    }
}
