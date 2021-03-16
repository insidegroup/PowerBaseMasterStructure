using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.ViewModels;
using System.Linq.Dynamic;

namespace CWTDesktopDatabase.Repository
{
    public class PolicyAirCabinGroupItemRepository
    {
        private PolicyAirCabinGroupItemDC db = new PolicyAirCabinGroupItemDC(Settings.getConnectionString());

        //Sortable List
        public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyAirCabinGroupItems_v1Result> GetPolicyAirCabinGroupItems(int policyGroupID, string sortField, int sortOrder, int page)
        {
            //query db
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectPolicyAirCabinGroupItems_v1(policyGroupID, adminUserGuid, sortField, Convert.ToBoolean(sortOrder), page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //add paging information and return1
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPolicyAirCabinGroupItems_v1Result>(result, page, totalRecords);
            return paginatedView;

        }

        //Add to DB
        public void Add(PolicyAirCabinGroupItem policyAirCabinGroupItem, PolicyRouting policyRouting)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            int? policyAirCabinGroupItemId = new Int32();

            db.spDesktopDataAdmin_InsertPolicyAirCabinGroupItem_v1(
                ref policyAirCabinGroupItemId,
                policyAirCabinGroupItem.PolicyGroupId,
                policyAirCabinGroupItem.AirlineCabinCode,
                policyAirCabinGroupItem.FlightDurationAllowedMin,
                policyAirCabinGroupItem.FlightDurationAllowedMax,
                policyAirCabinGroupItem.FlightMileageAllowedMin,
                policyAirCabinGroupItem.FlightMileageAllowedMax,
                policyAirCabinGroupItem.PolicyProhibitedFlag,
                policyAirCabinGroupItem.EnabledFlag,
                policyAirCabinGroupItem.EnabledDate,
                policyAirCabinGroupItem.ExpiryDate,
                policyAirCabinGroupItem.TravelDateValidFrom,
                policyAirCabinGroupItem.TravelDateValidTo,
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

            policyAirCabinGroupItem.PolicyAirCabinGroupItemId = (int)policyAirCabinGroupItemId;

        }

        //Update in DB
        public void Update(PolicyAirCabinGroupItem policyAirCabinGroupItem, PolicyRouting policyRouting)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdatePolicyAirCabinGroupItem_v1(
                policyAirCabinGroupItem.PolicyAirCabinGroupItemId,
                policyAirCabinGroupItem.PolicyGroupId,
                policyAirCabinGroupItem.AirlineCabinCode,
                 policyAirCabinGroupItem.FlightDurationAllowedMin,
                policyAirCabinGroupItem.FlightDurationAllowedMax,
                policyAirCabinGroupItem.FlightMileageAllowedMin,
                policyAirCabinGroupItem.FlightMileageAllowedMax,
                policyAirCabinGroupItem.PolicyProhibitedFlag,
                policyAirCabinGroupItem.EnabledFlag,
                policyAirCabinGroupItem.EnabledDate,
                policyAirCabinGroupItem.ExpiryDate,
                policyAirCabinGroupItem.TravelDateValidFrom,
                policyAirCabinGroupItem.TravelDateValidTo,
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
                adminUserGuid,
                policyAirCabinGroupItem.VersionNumber
            );

        }

        //Delete From DB
        public void Delete(PolicyAirCabinGroupItem policyAirCabinGroupItem)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeletePolicyAirCabinGroupItem_v1(
                policyAirCabinGroupItem.PolicyAirCabinGroupItemId,
                adminUserGuid,
                policyAirCabinGroupItem.VersionNumber
            );
        }

        //Get one PolicyAirCabinGroupItem
        public PolicyAirCabinGroupItem GetPolicyAirCabinGroupItem(int policyAirCabinGroupItemId)
        {
            return db.PolicyAirCabinGroupItems.SingleOrDefault(c => c.PolicyAirCabinGroupItemId == policyAirCabinGroupItemId);
        }

        //Add Data From Linked Tables for Display
        public void EditItemForDisplay(PolicyAirCabinGroupItem policyAirCabinGroupItem)
        {
            //populate new PolicyAirCabinGroupItem with PolicyGroupName    
            PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();
            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(policyAirCabinGroupItem.PolicyGroupId);
            if (policyGroup != null)
            {
                policyAirCabinGroupItem.PolicyGroupName = policyGroup.PolicyGroupName;
            }

            //AirlineCabin
            AirlineCabinRepository airlineCabinRepository = new AirlineCabinRepository();
            AirlineCabin airlineCabin = new AirlineCabin();
            airlineCabin = airlineCabinRepository.GetAirlineCabin(policyAirCabinGroupItem.AirlineCabinCode);
            if (airlineCabin != null)
            {
                policyAirCabinGroupItem.AirlineCabinDefaultDescription = airlineCabin.AirlineCabinDefaultDescription;
            }

        }

    }
}
