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
    public class CountryRegionRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get a Page of Country Regions - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectCountryRegions_v1Result> PageCountryRegions(int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            var result = db.spDesktopDataAdmin_SelectCountryRegions_v1(filter, sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectCountryRegions_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get a Page of Locations within a Country Regions - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectCountryRegionLocations_v1Result> PageCountryRegionLocations(int page, string filter, string sortField, int sortOrder, int countryRegionId)
        {
            //get a page of records
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectCountryRegionLocations_v1(countryRegionId, filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectCountryRegionLocations_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //List of All Items - Sortable
        /*public IQueryable<fnDesktopDataAdmin_SelectCountryRegions_v1Result> GetCountryRegions(string filter, string sortField, int sortOrder)
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
                return db.fnDesktopDataAdmin_SelectCountryRegions_v1(adminUserGuid).OrderBy(sortField);
            }
            else
            {
                return db.fnDesktopDataAdmin_SelectCountryRegions_v1(adminUserGuid)
                        .OrderBy(sortField)
                        .Where(c => c.CountryName.Contains(filter) || c.CountryRegionName.Contains(filter));
            }
        }*/

        //List of All Locations within a Country Region - Sortable
        /*public IQueryable<fnDesktopDataAdmin_SelectCountryRegionLocations_v1Result> GetCountryRegionLocations(string filter, int id, string sortField, int sortOrder)
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
                return db.fnDesktopDataAdmin_SelectCountryRegionLocations_v1(adminUserGuid, id).OrderBy(sortField);
            }
            else
            {
                return db.fnDesktopDataAdmin_SelectCountryRegionLocations_v1(adminUserGuid, id).OrderBy(sortField).Where(c => c.LocationName.Contains(filter));
            }
        }*/

        public CountryRegion GetCountryRegion(int id)
        {
            return db.CountryRegions.SingleOrDefault(c => c.CountryRegionId == id);
        }

        //Add Data From Linked Tables for Display
        public void EditForDisplay(CountryRegion countryRegion)
        {
           
        }


        //trying to insert this item, must check if already exists
        //if available return true, if already exists return false
        public bool IsAvailableCountryRegion (string countryRegionName, string countryCode, int? countryRegionId)
        {
            
            List<CountryRegion> countryRegions = new List<CountryRegion>();

            if (countryRegionId.HasValue)
            {
                //if countryRegionId has value,  we are editing and therefore can include the current value
                var result = from n in db.CountryRegions
                             where n.CountryCode.Trim().Equals(countryCode)
                             && n.CountryRegionName.Equals(countryRegionName)
                             && !n.CountryRegionId.Equals(countryRegionId) select n;
                countryRegions = result.ToList();             

            }
            else
            {
                var result = from n in db.CountryRegions
                             where n.CountryCode.Trim().Equals(countryCode)
                             && n.CountryRegionName.Equals(countryRegionName)
                                select n;
                countryRegions = result.ToList();
            }

            if (countryRegions.Count > 0)
            {
                return false;   //already in use
            }
            else
            {
                return true;
            }

        }

        //Add to DB
        public void Add(CountryRegion countryRegion)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertCountryRegion_v1(
                countryRegion.CountryRegionName,
                countryRegion.CountryCode,
                adminUserGuid
            );

        }

        //Update in DB
        public void Update(CountryRegion countryRegion)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateCountryRegion_v1(
                countryRegion.CountryRegionId,
                countryRegion.CountryRegionName,
                countryRegion.CountryCode,
                adminUserGuid,
                countryRegion.VersionNumber
            );

        }

        //Delete From DB
        public void Delete(CountryRegion countryRegion)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeleteCountryRegion_v1(
                countryRegion.CountryRegionId,
                adminUserGuid,
                countryRegion.VersionNumber
            );
        }

    }
}
