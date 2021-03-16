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
    public class GSTIdentificationNumberRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get a Page of Country Regions - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectGSTIdentificationNumbers_v1Result> PageGSTIdentificationNumbers(int page, string filter, string sortField, int sortOrder)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            
            //get a page of records
            var result = db.spDesktopDataAdmin_SelectGSTIdentificationNumbers_v1(filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectGSTIdentificationNumbers_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        public IQueryable<GSTIdentificationNumber> GetAllGSTIdentificationNumbers()
        {
            return db.GSTIdentificationNumbers.OrderBy(t => t.ClientEntityName);
        }

        public GSTIdentificationNumber GetGSTIdentificationNumber(int gstIdentificationNumberCode)
        {
            return db.GSTIdentificationNumbers.SingleOrDefault(c => c.GSTIdentificationNumberId == gstIdentificationNumberCode);
        }

        //Add Data From Linked Tables for Display
        public void EditForDisplay(GSTIdentificationNumber gstIdentificationNumber)
        {
            CountryRepository countryRepository = new CountryRepository();
            Country country = new Country();
            country = countryRepository.GetCountry(gstIdentificationNumber.CountryCode);
            if (country != null)
            {
                gstIdentificationNumber.CountryName = country.CountryName;
            }

            ClientTopUnitRepository clientTopUnitRepository = new ClientTopUnitRepository();
            ClientTopUnit clientTopUnit = clientTopUnitRepository.GetClientTopUnit(gstIdentificationNumber.ClientTopUnitGuid);
            if(clientTopUnit != null)
            {
                gstIdentificationNumber.ClientTopUnitName = clientTopUnit.ClientTopUnitName;
            }

            StateProvinceRepository stateProvinceRepository = new StateProvinceRepository();
            StateProvince stateProvince = stateProvinceRepository.GetStateProvinceByCountry(gstIdentificationNumber.CountryCode, gstIdentificationNumber.StateProvinceCode);
            if(stateProvince != null)
            {
                gstIdentificationNumber.StateProvinceName = stateProvince.Name;
            }
        }

		//Add to DB
		public void Add(GSTIdentificationNumber gstIdentificationNumber)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_InsertGSTIdentificationNumber_v1(
                gstIdentificationNumber.GSTIdentificationNumber1,
                gstIdentificationNumber.ClientTopUnitGuid,
                gstIdentificationNumber.ClientEntityName,
                gstIdentificationNumber.BusinessPhoneNumber,
                gstIdentificationNumber.BusinessEmailAddress,
                gstIdentificationNumber.FirstAddressLine,
                gstIdentificationNumber.SecondAddressLine,
                gstIdentificationNumber.CityName,
                gstIdentificationNumber.CountryCode,
				gstIdentificationNumber.StateProvinceCode,
                gstIdentificationNumber.PostalCode,
                adminUserGuid
			);
		}

		//Update in DB
		public void Update(GSTIdentificationNumber gstIdentificationNumber)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdateGSTIdentificationNumber_v1(
				gstIdentificationNumber.GSTIdentificationNumberId,
                gstIdentificationNumber.GSTIdentificationNumber1,
                gstIdentificationNumber.ClientTopUnitGuid,
                gstIdentificationNumber.ClientEntityName,
                gstIdentificationNumber.BusinessPhoneNumber,
                gstIdentificationNumber.BusinessEmailAddress,
                gstIdentificationNumber.FirstAddressLine,
                gstIdentificationNumber.SecondAddressLine,
                gstIdentificationNumber.CityName,
                gstIdentificationNumber.CountryCode,
                gstIdentificationNumber.StateProvinceCode,
                gstIdentificationNumber.PostalCode,
                adminUserGuid,
                gstIdentificationNumber.VersionNumber
			);
		}

		//Delete in DB
		public void Delete(GSTIdentificationNumber gstIdentificationNumber)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeleteGSTIdentificationNumber_v1(
				gstIdentificationNumber.GSTIdentificationNumberId,
				adminUserGuid,
				gstIdentificationNumber.VersionNumber
			);
		}
    }
}
