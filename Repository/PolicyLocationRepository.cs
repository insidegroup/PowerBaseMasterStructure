using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Linq.Dynamic;

namespace CWTDesktopDatabase.Repository
{
    public class PolicyLocationRepository
    {
        private PolicyLocationDC db = new PolicyLocationDC(Settings.getConnectionString());

        //Get a Page of PolicyGroups - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyLocations_v1Result> PagePolicyLocations(int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectPolicyLocations_v1(filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPolicyLocations_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //List of All Items - Sortable
        /*
         * public IQueryable<fnDesktopDataAdmin_SelectPolicyLocations_v1Result> ListPolicyLocations(string filter, string sortField, int sortOrder)
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
                return db.fnDesktopDataAdmin_SelectPolicyLocations_v1(adminUserGuid).OrderBy(sortField);
            }
            else
            {
                return db.fnDesktopDataAdmin_SelectPolicyLocations_v1(adminUserGuid).OrderBy(sortField).Where
                        (c => c.PolicyLocationName.Contains(filter));
            }
        }
        */

        //List All Items
        public IQueryable<PolicyLocation> GetAllPolicyLocations()
        {
            return db.PolicyLocations.OrderBy("PolicyLocationName");
        }

        //Get single item
        public PolicyLocation GetPolicyLocation(int policyLocationId)
        {
			HierarchyRepository hierarchyRepository = new HierarchyRepository();
			CountryRepository countryRepository = new CountryRepository();
			CityRepository cityRepository = new CityRepository();

			PolicyLocation policyLocation = new PolicyLocation();

			policyLocation = db.PolicyLocations.SingleOrDefault(c => c.PolicyLocationId == policyLocationId);

			if(policyLocation != null) {

				if (policyLocation.GlobalFlag)
				{
					policyLocation.LocationType = "Global";
				} 
				else if (policyLocation.CityCode != null)
				{
					policyLocation.LocationType = "City";

					City city = cityRepository.GetCity(policyLocation.CityCode);
					Country country = countryRepository.GetCountry(city.CountryCode);
					policyLocation.ParentName = country.CountryName;
				}
				else if (policyLocation.CountryCode != null)
				{
					policyLocation.LocationType = "Country";

					Country country = countryRepository.GetCountry(policyLocation.CountryCode);
					GlobalSubRegion globalSubRegion = hierarchyRepository.GetGlobalSubRegion(country.GlobalSubRegionCode);
					policyLocation.ParentName = globalSubRegion.GlobalSubRegionName;

				}
				else if (policyLocation.GlobalSubRegionCode != null)
				{
					policyLocation.ParentName = "GlobalSubRegion";

					GlobalSubRegion globalSubRegion = hierarchyRepository.GetGlobalSubRegion(policyLocation.GlobalSubRegionCode);
					GlobalRegion globalRegion = hierarchyRepository.GetGlobalRegion(globalSubRegion.GlobalRegionCode);
					policyLocation.ParentName = globalRegion.GlobalRegionName;
					
				}
				else if (policyLocation.GlobalRegionCode != null)
				{
					policyLocation.LocationType = "GlobalRegion";

					GlobalRegion globalRegion = hierarchyRepository.GetGlobalRegion(policyLocation.GlobalRegionCode);
					policyLocation.ParentName = globalRegion.Global.GlobalName;
				}
			}

			return policyLocation;
        }

        //AutoCOmplete
        public List<PolicyLocationLocationJSON> LookUpPolicyLocationLocations(string searchText, int maxResults)
        {
            var result = from n in db.fnDesktopDataAdmin_SelectPolicyLocationLocations_v1()
                         where n.Name.StartsWith(searchText)
                         orderby n.Name
                         select
                             new PolicyLocationLocationJSON
                             {
                                 Code = n.Code,
                                 Name = n.Name.Trim(),
                                 Parent = n.Parent.Trim(),
                                 CodeType = n.CodeType
                             };
            return result.Take(maxResults).ToList();
        }

        public fnDesktopDataAdmin_SelectPolicyLocationLocations_v1Result GetPolicyLocationLocationByName(string locationName)
        {
            var result = db.fnDesktopDataAdmin_SelectPolicyLocationLocations_v1().FirstOrDefault(c => c.Name == locationName);
            return result;
        }

		public List<PolicyLocation> GetPolicyLocationByLocationCode(string locationCode, string locationType, int policyLocationID = 0)
        {
			List<PolicyLocation> result = new List<PolicyLocation>();
			
			if (policyLocationID != 0)
			{
				result = db.PolicyLocations
					.Where(x =>
						(x.GlobalRegionCode == locationCode && locationType == "GlobalRegion") ||
						(x.GlobalSubRegionCode == locationCode && locationType == "GlobalSubRegion") ||
						(x.CountryCode == locationCode && locationType == "Country") ||
						(x.CityCode == locationCode && locationType == "City")
					)
					.Where(x => x.PolicyLocationId != policyLocationID)
					.ToList();
			}
			else
			{
				result = db.PolicyLocations.Where(x =>
						(x.GlobalRegionCode == locationCode && locationType == "GlobalRegion") ||
						(x.GlobalSubRegionCode == locationCode && locationType == "GlobalSubRegion") ||
						(x.CountryCode == locationCode && locationType == "Country") ||
						(x.CityCode == locationCode && locationType == "City")
				).ToList();
			}

            return result;
        }

		//Get PolicyLocationItemsByCityCode
		public List<PolicyLocation> GetPolicyLocationItemsByCityCode(string cityCode)
		{
			return db.PolicyLocations.Where(c => c.CityCode == cityCode).ToList();
		}

        //Edit Location fields
        public void EditPolicyLocationLocation(PolicyLocation policyLocation)
        {

            string locationType = policyLocation.LocationType;

            #region
            if (locationType == "GlobalRegion")
            {
                policyLocation.CityCode = null;
                policyLocation.CountryCode = null;
                policyLocation.GlobalSubRegionCode = null;
                policyLocation.GlobalRegionCode = policyLocation.LocationCode;
                policyLocation.GlobalFlag = false;
            }
            else if (locationType == "GlobalSubRegion")
            {
                policyLocation.CityCode = null;
                policyLocation.CountryCode = null;
                policyLocation.GlobalSubRegionCode = policyLocation.LocationCode;
                policyLocation.GlobalRegionCode = null;
                policyLocation.GlobalFlag = false;
            }
            else if (locationType == "Country")
            {
                policyLocation.CityCode = null;
                policyLocation.CountryCode = policyLocation.LocationCode;
                policyLocation.GlobalSubRegionCode = null;
                policyLocation.GlobalRegionCode = null;
                policyLocation.GlobalFlag = false;
            }
            else if (locationType == "City")
            {
                policyLocation.CityCode = policyLocation.LocationCode;
                policyLocation.CountryCode = null;
                policyLocation.GlobalSubRegionCode = null;
                policyLocation.GlobalRegionCode = null;
                policyLocation.GlobalFlag = false;
            }
            else if (locationType == "Global")
            {
                policyLocation.CityCode = null;
                policyLocation.CountryCode = null;
                policyLocation.GlobalSubRegionCode = null;
                policyLocation.GlobalRegionCode = null;
                policyLocation.GlobalFlag = true;
            }
            
            #endregion

        }


        //Add Data From Linked Tables for Display
        public void EditForDisplay(PolicyLocation policyLocation)
        {
            if (policyLocation.TravelPortCode != null)
            {
                TravelPortRepository travelPortRepository = new TravelPortRepository();
                TravelPort travelPort = new TravelPort();
                travelPort = travelPortRepository.GetTravelPort(policyLocation.TravelPortCode);
                if (travelPort != null)
                {
                    policyLocation.TravelPortName = travelPort.TravelportName;
					policyLocation.LocationCode = travelPort.TravelPortCode;
					policyLocation.LocationName = travelPort.TravelportName;
                }
            }
            if (policyLocation.TravelPortTypeId != null)
            {
                int travelPortTypeId = (int)policyLocation.TravelPortTypeId;
                TravelPortTypeRepository travelPortTypeRepository = new TravelPortTypeRepository();
                TravelPortType travelPortType = new TravelPortType();
                travelPortType = travelPortTypeRepository.GetTravelPortType(travelPortTypeId);
                if (travelPortType != null)
                {
                    policyLocation.TravelPortType = travelPortType.TravelPortTypeDescription;
				}
            }

            HierarchyRepository hierarchyRepository = new HierarchyRepository();

            string globalRegionCode = policyLocation.GlobalRegionCode;
            if (globalRegionCode != null)
            {
                GlobalRegion globalRegion = new GlobalRegion();
                globalRegion = hierarchyRepository.GetGlobalRegion(globalRegionCode);
                policyLocation.LocationCode = globalRegion.GlobalRegionCode;
                policyLocation.LocationName = globalRegion.GlobalRegionName;
            }

            string globalSubRegionCode = policyLocation.GlobalSubRegionCode;
            if (globalSubRegionCode != null)
            {
                GlobalSubRegion globalSubRegion = new GlobalSubRegion();
                globalSubRegion = hierarchyRepository.GetGlobalSubRegion(globalSubRegionCode);
                policyLocation.LocationCode = globalSubRegion.GlobalSubRegionCode;
                policyLocation.LocationName = globalSubRegion.GlobalSubRegionName;
            }

            string countryCode = policyLocation.CountryCode;
            if (countryCode != null)
            {
                Country country = new Country();
                country = hierarchyRepository.GetCountry(countryCode);
                policyLocation.LocationCode = country.CountryCode;
                policyLocation.LocationName = country.CountryName;
            }
            string cityCode = policyLocation.CityCode;
            if (cityCode != null)
            {
                CityRepository cityRepository = new CityRepository();
                City city = new City();
                city = cityRepository.GetCity(cityCode);
                policyLocation.LocationCode = city.CityCode;
                policyLocation.LocationName = city.Name;
            }

			if (policyLocation.GlobalFlag)
			{
				policyLocation.LocationName = "Global";
				policyLocation.LocationCode = "Global";
			}

        }

        //Edit policyLocation
        public void Update(PolicyLocation policyLocation)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdatePolicyLocation_v1(

                policyLocation.PolicyLocationId,
                policyLocation.PolicyLocationName,
                policyLocation.GlobalFlag,
                policyLocation.GlobalRegionCode,
                policyLocation.GlobalSubRegionCode,
                policyLocation.CountryCode,
                policyLocation.CityCode,
                policyLocation.TravelPortCode,
                policyLocation.TravelPortTypeId,
                adminUserGuid,
                policyLocation.VersionNumber
            );
        }

        //Add policyLocation
        public void Add(PolicyLocation policyLocation)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertPolicyLocation_v1(
                policyLocation.PolicyLocationName,
                policyLocation.GlobalFlag,
                policyLocation.GlobalRegionCode,
                policyLocation.GlobalSubRegionCode,
                policyLocation.CountryCode,
                policyLocation.CityCode,
                policyLocation.TravelPortCode,
                policyLocation.TravelPortTypeId,
                adminUserGuid
            );
        }

        //Delete From DB
        public void Delete(PolicyLocation policyLocation)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeletePolicyLocation_v1(
                policyLocation.PolicyLocationId,
                adminUserGuid,
                policyLocation.VersionNumber
            );
        }
    }
}
