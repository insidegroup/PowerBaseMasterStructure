using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Repository
{
    public class StateProvinceRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());
        public IQueryable<StateProvince> GetAllStateProvinces()
        {
            return db.StateProvinces.OrderBy(t => t.Name);
        }

        //Issue when more than result with same code
        //NT AU  Northern Territory
        //NT CA  Northwest Territories
        public StateProvince GetStateProvince(string code)
        {
            return db.StateProvinces.SingleOrDefault(c => c.StateProvinceCode == code);
        }

        public StateProvince GetStateProvinceByCountry(string countryCode, string stateProvinceCode)
        {
            return db.StateProvinces.SingleOrDefault(c => c.StateProvinceCode == stateProvinceCode && c.CountryCode == countryCode);
        }

        public StateProvince GetStateProvinceByStateProvinceName(string countryCode, string stateProvinceName)
        {
            return db.StateProvinces.SingleOrDefault(c => c.Name == stateProvinceName && c.CountryCode == countryCode);
        }

        public List<StateProvince> GetStateProvincesByCountryCode(string countryCode)
		{
			return db.StateProvinces.Where(c => c.CountryCode == countryCode).ToList();
		}

        public List<StateProvinceJSON> GetStateProvinceByName(string searchText, string countryCode)
        {
            var result = from n in db.StateProvinces
                         where n.Name.Trim().Equals(searchText) &&
                            n.CountryCode.Trim().Equals(countryCode)
                         select
                             new StateProvinceJSON
                             {
                                 Name = n.Name.Trim(),
                                 StateProvinceCode = n.StateProvinceCode.ToString()
                             };
            return result.ToList();
        }

        public List<StateProvince> LookUpStateProvinces(string searchText, string countryCode, int maxResults)
        {
            var result = from n in db.StateProvinces
                         where n.Name.Contains(searchText)
                         && n.CountryCode == countryCode
                         orderby n.Name
                         select n;
            return result.Take(maxResults).ToList();
        }

    }
}
