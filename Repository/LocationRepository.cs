using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Linq.Dynamic;
using CWTDesktopDatabase.ViewModels;

namespace CWTDesktopDatabase.Repository
{
    public class LocationRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get a Page of Locations - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectLocations_v1Result> PageLocations(int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectLocations_v1(filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectLocations_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get a Single Location
        public Location GetLocation(int locationId)
        {
            return db.Locations.SingleOrDefault(c => c.LocationId == locationId);
        }

        //Get a Single Location by Name
        public Location GetLocationByName(string locationName)
        {
            return db.Locations.SingleOrDefault(c => c.LocationName == locationName);

        }
        //Get Location Address
        public Address GetLocationAddress(int locationId)
        {
            LocationAddress locationAddress = new LocationAddress();
            locationAddress = db.LocationAddresses.SingleOrDefault(c => c.LocationId == locationId);
            if (locationAddress != null){
                return db.Addresses.SingleOrDefault(c => c.AddressId == locationAddress.AddressId);
            }
             return null;

        }

        //Add Data From Linked Tables for Display
        public void EditForDisplay(Location location)
        {
            CountryRegionRepository countryRegionRepository = new CountryRegionRepository();
            CountryRegion countryRegion = new CountryRegion();
            countryRegion = countryRegionRepository.GetCountryRegion(location.CountryRegionId);

            CountryRepository countryRepository = new CountryRepository();
            Country country = new Country();
            country = countryRepository.GetCountry(countryRegion.CountryCode);
            location.CountryName = country.CountryName;
            location.CountryRegionName = countryRegion.CountryRegionName;
            location.CountryCode = country.CountryCode;
        }

        //Add to DB
		public void Add(AddressLocationVM addressLocationVM)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertLocation_v1(
				addressLocationVM.Location.LocationName,
				addressLocationVM.Location.CountryRegionId,
				addressLocationVM.Address.FirstAddressLine,
				addressLocationVM.Address.SecondAddressLine,
				addressLocationVM.Address.CityName,
				addressLocationVM.Address.StateProvinceCode,
				addressLocationVM.Address.PostalCode,
				addressLocationVM.Address.CountryCode,
                adminUserGuid
            );

        }

        //Update in DB
		public void Update(AddressLocationVM addressLocationVM)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateLocation_v1(
				addressLocationVM.Location.LocationId,
				addressLocationVM.Location.LocationName,
				addressLocationVM.Location.CountryRegionId,
				addressLocationVM.Address.FirstAddressLine,
				addressLocationVM.Address.SecondAddressLine,
				addressLocationVM.Address.CityName,
				addressLocationVM.Address.StateProvinceCode,
				addressLocationVM.Address.PostalCode,
				addressLocationVM.Address.CountryCode,
                adminUserGuid,
				addressLocationVM.Location.VersionNumber
            );

        }

        //Delete From DB
        public void Delete(Location Location)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeleteLocation_v1(
                Location.LocationId,
                adminUserGuid,
                Location.VersionNumber
            );
        }

        //trying to insert this item, must check if already exists
        //if available return true, if already exists return false
        public bool IsAvailableLocation(string locationName, int countryRegionId, int? locationId)
        {

            List<Location> locations = new List<Location>();

            if (locationId.HasValue && locationId != 0)
            {
                //if location has value,  we are editing and therefore can include the current value
                var result = from n in db.Locations
                             where n.CountryRegionId.Equals(countryRegionId)
                             && n.LocationName.Equals(locationName)
                             && !n.LocationId.Equals(locationId)
                             select n;
                locations = result.ToList();

            }
            else
            {
                var result = from n in db.Locations
                             where n.CountryRegionId.Equals(countryRegionId)
                             && n.LocationName.Equals(locationName)
                             select n;
                locations = result.ToList();
            }

            if (locations.Count > 0)
            {
                return false;   //already in use
            }
            else
            {
                return true;
            }

        }

        //REMOVED: List of All Items - Sortable
        /*public IQueryable<fnDesktopDataAdmin_SelectLocations_v1Result> GetLocations(string filter, string sortField, int sortOrder)
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
            if (filter == "")
            {
                return db.fnDesktopDataAdmin_SelectLocations_v1(adminUserGuid).OrderBy(sortField);
            }
            else
            {
                return db.fnDesktopDataAdmin_SelectLocations_v1(adminUserGuid).OrderBy(sortField).Where(c => 
                    (c.LocationName.Contains(filter)||c.CountryRegionName.Contains(filter)||c.CountryName.Contains(filter))
                    );
            }
        }*/

    }
}
