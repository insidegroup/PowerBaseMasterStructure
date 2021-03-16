using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Repository
{
    public class CityRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get a Page of Cities
        public CWTPaginatedList<spDesktopDataAdmin_SelectCities_v1Result> PageCities(int page, string filter, string sortField, int sortOrder)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectCities_v1(filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectCities_v1Result>(result, page, Convert.ToInt32(totalRecords));
            return paginatedView;
        }

        public IQueryable<City> GetAllCities()
        {
            return db.Cities.OrderBy(c => c.Name);
        }

        public City GetCity(string cityCode)
        {
            return db.Cities.SingleOrDefault(c => c.CityCode == cityCode);
        }

        public City EditItemForDisplay(City city)
        {
            HierarchyRepository hierarchyRepository = new HierarchyRepository();
            
			Country country = new Country();
            country = hierarchyRepository.GetCountry(city.CountryCode);
			if (country != null)
			{
				city.CountryName = country.CountryName;
			}

			city.LatitudeDecimal = Convert.ToDecimal(city.Latitude);
			city.LongitudeDecimal = Convert.ToDecimal(city.Longitude);

            TimeZoneRuleRepository timeZoneRuleRepository = new TimeZoneRuleRepository();
            TimeZoneRule timeZoneRule = timeZoneRuleRepository.GetTimeZoneRule(city.TimeZoneRuleCode);
            if (timeZoneRule != null)
            {
                city.TimeZoneRule = timeZoneRule;
            }

            return city;
        }

		public List<CityReference> GetCityReferences(string cityCode)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = from n in db.spDesktopDataAdmin_SelectCityReferences_v1(cityCode, adminUserGuid)
						 select
							 new CityReference
							 {
								 TableName = n.TableName.ToString()
							 };

			return result.ToList();
		}

		//Add to DB
		public void Add(City city)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_InsertCity_v1(
				city.CityCode,
				city.Name,
				city.CountryCode,
				city.StateProvinceCode,
				Convert.ToDouble(city.LatitudeDecimal),
				Convert.ToDouble(city.LongitudeDecimal),
                city.TimeZoneRuleCode,
				adminUserGuid
			);
		}

		//Update in DB
		public void Update(City city)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdateCity_v1(
				city.CityCode,
				city.Name,
				city.CountryCode,
				city.StateProvinceCode,
				Convert.ToDouble(city.LatitudeDecimal),
				Convert.ToDouble(city.LongitudeDecimal),
                city.TimeZoneRuleCode,
                city.VersionNumber,
				adminUserGuid
			);
		}

		//Delete in DB
		public void Delete(City city)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeleteCity_v1(
				city.CityCode,
				adminUserGuid,
				city.VersionNumber
			);
		}
    }
}
