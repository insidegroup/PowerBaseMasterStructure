using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Linq.Dynamic;

namespace CWTDesktopDatabase.Repository
{
    public class TravelPortRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get a Page of Country Regions - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectTravelPorts_v1Result> PageTravelPorts(int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            var result = db.spDesktopDataAdmin_SelectTravelPorts_v1(filter, sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectTravelPorts_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }


        //List of All Items - Sortable
        /*public IQueryable<fnDesktopDataAdmin_SelectTravelPorts_v1Result> GetTravelPorts(string sortField, int sortOrder)
        {
            if (sortOrder == 0)
            {
                sortField = sortField + " ascending";
            }
            else
            {
                sortField = sortField + " descending";
            }

            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
             return db.fnDesktopDataAdmin_SelectTravelPorts_v1(adminUserGuid).OrderBy(sortField);

        }*/

        public IQueryable<TravelPort> GetAllTravelPorts()
        {
            return db.TravelPorts.OrderBy(t => t.TravelportName);
        }

        public TravelPort GetTravelPort(string travelPortCode)
        {
            return db.TravelPorts.SingleOrDefault(c => c.TravelPortCode == travelPortCode);
        }

        //AutoCOmplete
        public List<TravelPortJSON> LookUpTravelPortNames(int typeId, string searchText, int maxResults)
        {
            var result = from n in db.fnDesktopDataAdmin_SelectTravelPortsByType_v1(typeId)
                         where n.TravelPortName.StartsWith(searchText)
                         orderby n.TravelPortName
                         select
                             new TravelPortJSON
                             {
                                 TravelPortName = n.TravelPortName.Trim(),
                                 TravelPortCode = n.TravelPortCode.Trim(),
                             };
            return result.Take(maxResults).ToList();
        }

        //Add Data From Linked Tables for Display
        public void EditForDisplay(TravelPort travelPort)
        {
            CountryRepository countryRepository = new CountryRepository();
            Country country = new Country();
            country = countryRepository.GetCountry(travelPort.CountryCode);
            if (country != null)
            {
                travelPort.CountryName = country.CountryName;
            }

            CityRepository cityRepository = new CityRepository();
            City city = new City();
            city = cityRepository.GetCity(travelPort.CityCode);
            if (city != null)
            {
                travelPort.CityName = city.Name;
            }

            TravelPortTypeRepository travelPortTypeRepository = new TravelPortTypeRepository();
            TravelPortType travelPortType = new TravelPortType();
            travelPortType = travelPortTypeRepository.GetTravelPortType(travelPort.TravelPortTypeId);
            if (travelPortType != null)
            {
                travelPort.TravelPortTypeDescription = travelPortType.TravelPortTypeDescription;
            }

			travelPort.LatitudeDecimal = Convert.ToDecimal(travelPort.Latitude);
			travelPort.LongitudeDecimal = Convert.ToDecimal(travelPort.Longitude);
        }

		//Get TravelPortItemsByCityCode
		public List<TravelPort> GetTravelPortItemsByCityCode(string cityCode)
		{
			return db.TravelPorts.Where(c => c.CityCode == cityCode).ToList();
		}

		//Add to DB
		public void Add(TravelPort travelPort)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_InsertTravelPort_v1(
				travelPort.TravelPortCode,
				travelPort.TravelportName,
				travelPort.TravelPortTypeId,
				travelPort.CityCode,
				travelPort.MetropolitanArea,
				travelPort.CountryCode,
				travelPort.StateProvinceCode,
				Convert.ToDouble(travelPort.LatitudeDecimal),
				Convert.ToDouble(travelPort.LongitudeDecimal),
				adminUserGuid
			);
		}

		//Update in DB
		public void Update(TravelPort travelPort)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdateTravelPort_v1(
				travelPort.TravelPortCode,
				travelPort.TravelportName,
				travelPort.TravelPortTypeId,
				travelPort.CityCode,
				travelPort.MetropolitanArea,
				travelPort.CountryCode,
				travelPort.StateProvinceCode,
				Convert.ToDouble(travelPort.LatitudeDecimal),
				Convert.ToDouble(travelPort.LongitudeDecimal),
				adminUserGuid,
				travelPort.VersionNumber				
			);
		}

		//Delete in DB
		public void Delete(TravelPort travelPort)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeleteTravelPort_v1(
				travelPort.TravelPortCode,
				adminUserGuid,
				travelPort.VersionNumber
			);
		}

		public List<TravelPortReference> GetTravelPortReferences(string travelPortCode)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			var result = from n in db.spDesktopDataAdmin_SelectTravelPortReferences_v1(travelPortCode, adminUserGuid)
						 select
							 new TravelPortReference
							 {
								 TableName = n.TableName.ToString()
							 };

			return result.ToList();
		}
    }
}
