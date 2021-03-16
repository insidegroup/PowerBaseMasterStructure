using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using System.Linq.Dynamic;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.Repository
{
    public class APISCountryRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get a Page of ContactTypes - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectAPISCountries_v1Result> PageAPISCountries(int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            var result = db.spDesktopDataAdmin_SelectAPISCountries_v1(filter, sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectAPISCountries_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //List of All Items - Sortable
        /*public IQueryable<fnDesktopDataAdmin_SelectAPISCountries_v1Result> GetAPISCountries(string filter, string sortField, int sortOrder)
        {
            var sortField2 = "OriginCountryName";
            if (sortField == "OriginCountryName")
            {
                sortField2 = "DestinationCountryName";

            }

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
                return db.fnDesktopDataAdmin_SelectAPISCountries_v1(adminUserGuid).OrderBy(sortField2).OrderBy(sortField);
            }
            else
            {
                return db.fnDesktopDataAdmin_SelectAPISCountries_v1(adminUserGuid).OrderBy(sortField2).OrderBy(sortField).Where(c =>
                        (c.OriginCountryName.Contains(filter) || c.DestinationCountryName.Contains(filter)));
            }
        }
        */
        
        //get one item
        public APISCountry GetAPISCountry(string occ, string dcc)
        {
            return db.APISCountries.SingleOrDefault(c => (c.DestinationCountryCode == dcc && c.OriginCountryCode == occ));
        }

        //Add Data From Linked Tables for Display
        public void EditForDisplay(APISCountry apisCountry)
        {
            CountryRepository countryRepository = new CountryRepository();
            Country country = new Country();
            country = countryRepository.GetCountry(apisCountry.OriginCountryCode);
            if (country != null)
            {
                apisCountry.OriginCountryName = country.CountryName;
            }
            country = countryRepository.GetCountry(apisCountry.DestinationCountryCode);
            if (country != null)
            {
                apisCountry.DestinationCountryName = country.CountryName;
            }

        }

        //Add to DB
        public void Add(APISCountry apisCountry)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            
            db.spDesktopDataAdmin_InsertAPISCountry_v1(
                apisCountry.OriginCountryCode,
                apisCountry.DestinationCountryCode,
                apisCountry.StartDate,
                adminUserGuid
            );
            

        }

        //Update in DB
        public void Update(APISCountry apisCountry, string originalOCC, string originalDCC)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            
            db.spDesktopDataAdmin_UpdateAPISCountry_v1(
                originalOCC,
                originalDCC,
                apisCountry.OriginCountryCode,
                apisCountry.DestinationCountryCode,
                apisCountry.StartDate,
                adminUserGuid,
                apisCountry.VersionNumber
            );
                          
        }

        //Delete From DB
        public void Delete(APISCountry apisCountry)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeleteAPISCountry_v1(
                apisCountry.OriginCountryCode,
                apisCountry.DestinationCountryCode,
                adminUserGuid,
                apisCountry.VersionNumber
            );  
        }

        //MOVED
        //Javascript Validation
        //trying to insert this item, must check if already exists
        //if available return true, if already exists return false
        /*public bool IsAvailable(string originCountryCode, string destinationCountryCode)
        {

            List<APISCountry> apisCountries = new List<APISCountry>();

            var result = from n in db.APISCountries
                         where n.OriginCountryCode.Trim().Equals(originCountryCode)
                         && n.DestinationCountryCode.Equals(destinationCountryCode)
                         select n;
            apisCountries = result.ToList();



            if (apisCountries.Count > 0)
            {
                return false;   //already in use
            }
            else
            {
                return true;
            }

        }
         * */

    }
}
