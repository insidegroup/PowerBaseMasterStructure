using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.Repository
{
    public class CountryRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get a Page of Countries
        public CWTPaginatedList<spDesktopDataAdmin_SelectCountries_v1Result> PageCountries(int page, string filter, string sortField, int sortOrder)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectCountries_v1(filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectCountries_v1Result>(result, page, Convert.ToInt32(totalRecords));
            return paginatedView;
        }

        public IQueryable<Country> GetAllCountries()
        {
            return db.Countries.OrderBy(c => c.CountryName);
        }

        public Country GetCountry(string countryCode)
        {
            return db.Countries.SingleOrDefault(c => c.CountryCode == countryCode);
        }

        public List<CountryRegion> GetCountryCountryRegions(string countryCode)
        {
            var result = from n in db.CountryRegions
                         where n.CountryCode.Equals(countryCode)
                         orderby n.CountryRegionName
                         select n;
            return result.ToList();
        }

        public List<HierarchyJSON> LookUpCountryCountryRegions(string countryCode)
        {
            var result = from n in db.CountryRegions
                         where n.CountryCode.Equals(countryCode)
                         orderby n.CountryRegionName
                         select
                             new HierarchyJSON
                             {
                                 HierarchyName = n.CountryRegionName.Trim(),
                                 HierarchyCode = n.CountryRegionId.ToString()
                             };
            return result.Take(15).ToList();
        }

        public List<CountryRegion> GetCountryRegions(string countryCode)
        {
            return db.CountryRegions.Where(c => c.CountryCode == countryCode).OrderBy(c => c.CountryRegionName).Take(15).ToList();
        }

		public List<HierarchyJSON> GetCountryGlobalRegions(string countryCode)
		{
			var result = from Countries in db.Countries
						 join GlobalSubRegions in db.GlobalSubRegions on Countries.GlobalSubRegionCode equals GlobalSubRegions.GlobalSubRegionCode
						 join GlobalRegions in db.GlobalRegions on GlobalSubRegions.GlobalRegionCode equals GlobalRegions.GlobalRegionCode
						 where Countries.CountryCode.Equals(countryCode)
						 orderby GlobalRegions.GlobalRegionName
						 select
							 new HierarchyJSON
							 {
								 HierarchyName = GlobalRegions.GlobalRegionName.Trim(),
								 HierarchyCode = GlobalRegions.GlobalRegionCode.ToString()
							 };
			return result.ToList();
		}

        public List<Country> GetCountriesbyRole(string roleName)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            List<Country> countries = new List<Country>();

            var result = from n in db.fnDesktopDataAdmin_SelectSystemUserHierarchyCountries_v1(
                                adminUserGuid,
                                roleName)
                            orderby n.CountryName
                            select
                                new HierarchyJSON
                                {
                                    HierarchyName = n.CountryName.Trim(),
                                    HierarchyCode = n.CountryCode.ToString()
                                };

            foreach(HierarchyJSON country in result)
            {
                countries.Add(
                    new Country()
                    {
                        CountryCode = country.HierarchyCode,
                        CountryName = country.HierarchyName
                    }
                );
            }
            return countries.ToList();
        }
    }
}
