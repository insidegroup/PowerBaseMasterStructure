using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using System.Text.RegularExpressions;

namespace CWTDesktopDatabase.Repository
{
    public class PolicyRoutingRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        public PolicyRouting GetPolicyRouting(int policyRoutingId)
        {
            PolicyRouting policyRouting = db.PolicyRoutings.SingleOrDefault(c => c.PolicyRoutingId == policyRoutingId);

            if (policyRouting.FromCode == null && policyRouting.FromGlobalFlag == false)
            {
                policyRouting.FromCode = (policyRouting.FromGlobalRegionCode
                                        ?? policyRouting.FromGlobalSubRegionCode
                                        ?? policyRouting.FromCountryCode
                                        ?? policyRouting.FromCityCode
                                        ?? policyRouting.FromTravelPortCode
                                        ?? policyRouting.FromTraverlPortTypeId.ToString() ?? ""
                                        );
            }
            if (policyRouting.ToCode == null && policyRouting.ToGlobalFlag == false)
            {
                policyRouting.ToCode = (policyRouting.ToGlobalRegionCode
                                        ?? policyRouting.ToGlobalSubRegionCode
                                        ?? policyRouting.ToCountryCode
                                        ?? policyRouting.ToCityCode
                                        ?? policyRouting.ToTravelPortCode
                                        ?? (policyRouting.ToTravelPortTypeId != null ? policyRouting.ToTravelPortTypeId.ToString() : "")
                                        );
            }

            if (policyRouting.ToGlobalRegionCode != null)
            {
                policyRouting.ToCodeType = "GlobalRegion";
            }
            else if (policyRouting.ToGlobalSubRegionCode != null)
            {
                policyRouting.ToCodeType = "GlobalSubRegion";
            }
            else if (policyRouting.ToCountryCode != null)
            {
                policyRouting.ToCodeType = "Country";
            }
            else if (policyRouting.ToCityCode != null)
            {
                policyRouting.ToCodeType = "City";
            }
            else if (policyRouting.ToTravelPortCode != null)
            {
                policyRouting.ToCodeType = "TravelPort";
            }
            else if (policyRouting.ToTravelPortTypeId != null)
            {
                policyRouting.ToCodeType = "TravelPortType";

                TravelPortTypeRepository travelPortTypeRepository = new TravelPortTypeRepository();
                TravelPortType travelPortType = new TravelPortType();
                travelPortType = travelPortTypeRepository.GetTravelPortType(Convert.ToInt32(policyRouting.ToCode));
                policyRouting.ToCode = travelPortType.TravelPortTypeDescription;
            }
            else
            {
                policyRouting.ToCodeType = "Global";
            }

            if (policyRouting.FromGlobalRegionCode != null)
            {
                policyRouting.FromCodeType = "GlobalRegion";
            }
            else if (policyRouting.FromGlobalSubRegionCode != null)
            {
                policyRouting.FromCodeType = "GlobalSubRegion";
            }
            else if (policyRouting.FromCountryCode != null)
            {
                policyRouting.FromCodeType = "Country";
            }
            else if (policyRouting.FromCityCode != null)
            {
                policyRouting.FromCodeType = "City";
            }
            else if (policyRouting.FromTravelPortCode != null)
            {
                policyRouting.FromCodeType = "TravelPort";
            }
            else if (policyRouting.FromTraverlPortTypeId != null)
            {
                policyRouting.FromCodeType = "TravelPortType";
                TravelPortTypeRepository travelPortTypeRepository = new TravelPortTypeRepository();
                TravelPortType travelPortType = new TravelPortType();
                travelPortType = travelPortTypeRepository.GetTravelPortType(Convert.ToInt32(policyRouting.FromCode));
                policyRouting.FromCode = travelPortType.TravelPortTypeDescription;
            
            }
            else
            {
                policyRouting.FromCodeType = "Global";
            }

            policyRouting.Name = Regex.Replace(policyRouting.Name, @"[^\w\s\-()*]", "-");
            return policyRouting;
        }

        //puts codes into correct properties
        public string BuildRoutingName(string routingName)
        {
            return routingName + "_" + GetPolicyRoutingIdentifier(routingName);
        }

        //on form submit - check user input against existing items
        //if editing, id is passed to ignore current item
        public bool IsAvailablePolicyRoutingName(string policyRoutingName, int? policyRoutingId)
        {
            int count = 0;

            if (policyRoutingId.HasValue)
            {
                var result = from n in db.PolicyRoutings
                             where n.Name.Trim().Equals(policyRoutingName) && n.PolicyRoutingId != policyRoutingId
                             select n.Name;
                count = result.Count();
            }
            else
            {
                var result = from n in db.PolicyRoutings
                             where n.Name.Trim().Equals(policyRoutingName)
                             select n.Name;
                count = result.Count();
            }
            if (count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //return a 5 digit number, this number is included as part of the groupname to make it unique
        public string GetPolicyRoutingIdentifier(string policyRoutingName)
        {
            int? Id = db.fnDesktopDataAdmin_GetPolicyRoutingNameCounter_v1(policyRoutingName);
            int nonNullId = (Id.HasValue) ? Id.Value : 0;
            string result = nonNullId.ToString();
            return result.PadLeft(5, '0');
        }

		//Get PolicyRoutingItemsByCityCode
		public List<PolicyRouting> GetPolicyRoutingItemsByCityCode(string cityCode)
		{
			return db.PolicyRoutings.Where(c => c.FromCityCode == cityCode || c.ToCityCode == cityCode).ToList();
		}

        //puts codes into correct properties
        public void EditPolicyRouting(PolicyRouting policyRouting)
        {

            string fromCodeType = policyRouting.FromCodeType;
            string fromCode = policyRouting.FromCode;
            string toCodeType = policyRouting.ToCodeType;
            string toCode = policyRouting.ToCode;

            #region polpulate from/to
            if (fromCodeType == "GlobalRegion")
            {
                policyRouting.FromTravelPortCode = null;
                policyRouting.FromTraverlPortTypeId = null;
                policyRouting.FromCityCode = null;
                policyRouting.FromCountryCode = null;
                policyRouting.FromGlobalSubRegionCode = null;
                policyRouting.FromGlobalRegionCode = fromCode;
                policyRouting.FromGlobalFlag = false;
            }
            else if (fromCodeType == "GlobalSubRegion")
            {
                policyRouting.FromTravelPortCode = null;
                policyRouting.FromTraverlPortTypeId = null;
                policyRouting.FromCityCode = null;
                policyRouting.FromCountryCode = null;
                policyRouting.FromGlobalSubRegionCode = fromCode;
                policyRouting.FromGlobalRegionCode = null;
                policyRouting.FromGlobalFlag = false;
            }
            else if (fromCodeType == "Country")
            {
                policyRouting.FromTravelPortCode = null;
                policyRouting.FromTraverlPortTypeId = null;
                policyRouting.FromCityCode = null;
                policyRouting.FromCountryCode = fromCode;
                policyRouting.FromGlobalSubRegionCode = null;
                policyRouting.FromGlobalRegionCode = null;
                policyRouting.FromGlobalFlag = false;
            }
            else if (fromCodeType == "City")
            {
                policyRouting.FromTravelPortCode = null;
                policyRouting.FromTraverlPortTypeId = null;
                policyRouting.FromCityCode = fromCode;
                policyRouting.FromCountryCode = null;
                policyRouting.FromGlobalSubRegionCode = null;
                policyRouting.FromGlobalRegionCode = null;
                policyRouting.FromGlobalFlag = false;
            }
            else if (fromCodeType == "TravelPort")
            {
                policyRouting.FromTravelPortCode = fromCode;
                policyRouting.FromTraverlPortTypeId = null;
                policyRouting.FromCityCode = null;
                policyRouting.FromCountryCode = null;
                policyRouting.FromGlobalSubRegionCode = null;
                policyRouting.FromGlobalRegionCode = null;
                policyRouting.FromGlobalFlag = false;
            }
            else if (fromCodeType == "TravelPortType")
            {
                TravelPortTypeRepository travelPortTypeRepository = new TravelPortTypeRepository();
                TravelPortType travelPortType = new TravelPortType();
                travelPortType = travelPortTypeRepository.GetTravelPortTypeByDescription(policyRouting.FromCode);

                policyRouting.FromTravelPortCode = null;
                policyRouting.FromTraverlPortTypeId = travelPortType.TravelPortTypeId;
                policyRouting.FromCityCode = null;
                policyRouting.FromCountryCode = null;
                policyRouting.FromGlobalSubRegionCode = null;
                policyRouting.FromGlobalRegionCode = null;
                policyRouting.FromGlobalFlag = false;
            }
            else if (fromCodeType == "Global")
            {
                policyRouting.FromTravelPortCode = null;
                policyRouting.FromTraverlPortTypeId = null;
                policyRouting.FromCityCode = null;
                policyRouting.FromCountryCode = null;
                policyRouting.FromGlobalSubRegionCode = null;
                policyRouting.FromGlobalRegionCode = null;
                policyRouting.FromGlobalFlag = true;
            }
            if (toCodeType == "GlobalRegion")
            {
                policyRouting.ToTravelPortCode = null;
                policyRouting.ToTravelPortTypeId = null;
                policyRouting.ToCityCode = null;
                policyRouting.ToCountryCode = null;
                policyRouting.ToGlobalSubRegionCode = null;
                policyRouting.ToGlobalRegionCode = toCode;
                policyRouting.ToGlobalFlag = false;
            }
            else if (toCodeType == "GlobalSubRegion")
            {
                policyRouting.ToTravelPortCode = null;
                policyRouting.ToTravelPortTypeId = null;
                policyRouting.ToCityCode = null;
                policyRouting.ToCountryCode = null;
                policyRouting.ToGlobalSubRegionCode = toCode;
                policyRouting.ToGlobalRegionCode = null;
                policyRouting.ToGlobalFlag = false;
            }
            else if (toCodeType == "Country")
            {
                policyRouting.ToTravelPortCode = null;
                policyRouting.ToTravelPortTypeId = null;
                policyRouting.ToCityCode = null;
                policyRouting.ToCountryCode = toCode;
                policyRouting.ToGlobalSubRegionCode = null;
                policyRouting.ToGlobalRegionCode = null;
                policyRouting.ToGlobalFlag = false;
            }
            else if (toCodeType == "City")
            {
                policyRouting.ToTravelPortCode = null;
                policyRouting.ToTravelPortTypeId = null;
                policyRouting.ToCityCode = toCode;
                policyRouting.ToCountryCode = null;
                policyRouting.ToGlobalSubRegionCode = null;
                policyRouting.ToGlobalRegionCode = null;
                policyRouting.ToGlobalFlag = false;
            }
            else if (toCodeType == "TravelPort")
            {
                policyRouting.ToTravelPortCode = toCode;
                policyRouting.ToTravelPortTypeId = null;
                policyRouting.ToCityCode = null;
                policyRouting.ToCountryCode = null;
                policyRouting.ToGlobalSubRegionCode = null;
                policyRouting.ToGlobalRegionCode = null;
                policyRouting.ToGlobalFlag = false;
            }
            else if (toCodeType == "TravelPortType")
            {
                TravelPortTypeRepository travelPortTypeRepository = new TravelPortTypeRepository();
                TravelPortType travelPortType = new TravelPortType();
                travelPortType = travelPortTypeRepository.GetTravelPortTypeByDescription(policyRouting.ToCode);

                policyRouting.ToTravelPortCode = null;
                policyRouting.ToTravelPortTypeId = travelPortType.TravelPortTypeId.ToString();
                policyRouting.ToCityCode = null;
                policyRouting.ToCountryCode = null;
                policyRouting.ToGlobalSubRegionCode = null;
                policyRouting.ToGlobalRegionCode = null;
                policyRouting.ToGlobalFlag = false;
            }
            else if (toCodeType == "Global")
            {
                policyRouting.ToTravelPortCode = null;
                policyRouting.ToTravelPortTypeId = null;
                policyRouting.ToCityCode = null;
                policyRouting.ToCountryCode = null;
                policyRouting.ToGlobalSubRegionCode = null;
                policyRouting.ToGlobalRegionCode = null;
                policyRouting.ToGlobalFlag = true;
            }
            #endregion

        }


        //adds data from linked tables
        public void EditForDisplay(PolicyRouting policyRouting)
        {

            if (policyRouting.FromCityCode != null)
            {
                CityRepository cityRepository = new CityRepository();
                City city = new City();
                city = cityRepository.GetCity(policyRouting.FromCityCode);
                policyRouting.FromName = city.Name;
            }
            if (policyRouting.FromCountryCode != null)
            {
                CountryRepository countryRepository = new CountryRepository();
                Country country = new Country();
                country = countryRepository.GetCountry(policyRouting.FromCountryCode);
                policyRouting.FromName = country.CountryName;
            }
            if (policyRouting.FromGlobalSubRegionCode != null)
            {
                HierarchyRepository hierarchyRepository = new HierarchyRepository();
                GlobalSubRegion globalSubRegion = new GlobalSubRegion();
                globalSubRegion = hierarchyRepository.GetGlobalSubRegion(policyRouting.FromGlobalSubRegionCode);
                policyRouting.FromName = globalSubRegion.GlobalSubRegionName;
            }
			if (policyRouting.FromGlobalRegionCode != null)
			{
				HierarchyRepository hierarchyRepository = new HierarchyRepository();
				GlobalRegion globalRegion = new GlobalRegion();
				globalRegion = hierarchyRepository.GetGlobalRegion(policyRouting.FromGlobalRegionCode);
				policyRouting.FromName = globalRegion.GlobalRegionName;
			}
			if (policyRouting.FromGlobalFlag)
			{
				policyRouting.FromName = "Global";
			}


            if (policyRouting.ToCityCode != null)
            {
                CityRepository cityRepository = new CityRepository();
                City city = new City();
                city = cityRepository.GetCity(policyRouting.ToCityCode);
                policyRouting.ToName = city.Name;
            }
            if (policyRouting.ToCountryCode != null)
            {
                CountryRepository countryRepository = new CountryRepository();
                Country country = new Country();
                country = countryRepository.GetCountry(policyRouting.ToCountryCode);
                policyRouting.ToName = country.CountryName;
            }
            if (policyRouting.ToGlobalSubRegionCode != null)
            {
                HierarchyRepository hierarchyRepository = new HierarchyRepository();
                GlobalSubRegion globalSubRegion = new GlobalSubRegion();
                globalSubRegion = hierarchyRepository.GetGlobalSubRegion(policyRouting.ToGlobalSubRegionCode);
                policyRouting.ToName = globalSubRegion.GlobalSubRegionName;
            }
            if (policyRouting.ToGlobalRegionCode != null)
            {
                HierarchyRepository hierarchyRepository = new HierarchyRepository();
                GlobalRegion globalRegion = new GlobalRegion();
                globalRegion = hierarchyRepository.GetGlobalRegion(policyRouting.ToGlobalRegionCode);
                policyRouting.ToName = globalRegion.GlobalRegionName;
            }
			if (policyRouting.ToGlobalFlag)
			{
				policyRouting.ToName = "Global";
			}

        }

        public void Add(PolicyRouting policyRouting)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            int? policyRoutingId = new Int32();

            db.spDesktopDataAdmin_InsertPolicyRouting_v1(
                ref policyRoutingId,
                policyRouting.Name,
                policyRouting.FromGlobalFlag,
                policyRouting.FromGlobalRegionCode,
                policyRouting.FromGlobalSubRegionCode,
                policyRouting.FromCountryCode,
                policyRouting.FromCityCode,
                policyRouting.FromTravelPortCode,
                policyRouting.FromTraverlPortTypeId,
                policyRouting.ToGlobalFlag,
                policyRouting.ToGlobalRegionCode,
                policyRouting.ToGlobalSubRegionCode,
                policyRouting.ToCountryCode,
                policyRouting.ToCityCode,
                policyRouting.ToTravelPortCode,
                policyRouting.ToTravelPortTypeId,
                policyRouting.RoutingViceVersaFlag,
                adminUserGuid
            );

            policyRouting.PolicyRoutingId  = (int)policyRoutingId;
        }

    }
}
