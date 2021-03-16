using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using System.Linq.Dynamic;

namespace CWTDesktopDatabase.Repository
{
    public class PolicyAirParameterGroupItemRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Sortable List
        public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyAirParameterGroupItems_v1Result> GetPolicyAirParameterGroupItems(int policyGroupID, int policyAirParameterTypeId, string filter, string sortField, int sortOrder, int page)
        {
            //query db
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectPolicyAirParameterGroupItems_v1(policyGroupID, policyAirParameterTypeId, adminUserGuid, filter, sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //add paging information and return
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPolicyAirParameterGroupItems_v1Result>(result,page,totalRecords);
            return paginatedView;

        }

        //Add
        public void Add(PolicyAirParameterGroupItem policyAirParameterGroupItem, PolicyRouting policyRouting)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            int? policyAirParameterGroupItemId = new Int32();

            db.spDesktopDataAdmin_InsertPolicyAirParameterGroupItem_v1(
                ref policyAirParameterGroupItemId,
				policyAirParameterGroupItem.PolicyAirParameterTypeId,
				policyAirParameterGroupItem.PolicyAirParameterValue,
				policyAirParameterGroupItem.EnabledFlag,
                policyAirParameterGroupItem.EnabledDate,
                policyAirParameterGroupItem.ExpiryDate,
                policyAirParameterGroupItem.TravelDateValidFrom,
                policyAirParameterGroupItem.TravelDateValidTo,
                policyAirParameterGroupItem.PolicyGroupId,
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

            policyAirParameterGroupItem.PolicyAirParameterGroupItemId = (int)policyAirParameterGroupItemId;
        }

        //Edit
        public void Update(PolicyAirParameterGroupItem policyAirParameterGroupItem, PolicyRouting policyRouting)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdatePolicyAirParameterGroupItem_v1(
                policyAirParameterGroupItem.PolicyAirParameterGroupItemId,
				policyAirParameterGroupItem.PolicyAirParameterTypeId,
				policyAirParameterGroupItem.PolicyAirParameterValue,
				policyAirParameterGroupItem.EnabledFlag,
                policyAirParameterGroupItem.EnabledDate,
                policyAirParameterGroupItem.ExpiryDate,
                policyAirParameterGroupItem.TravelDateValidFrom,
                policyAirParameterGroupItem.TravelDateValidTo,
                policyAirParameterGroupItem.PolicyGroupId,
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
                policyAirParameterGroupItem.VersionNumber
            );

        }

        //Delete
        public void Delete(PolicyAirParameterGroupItem policyAirParameterGroupItem)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeletePolicyAirParameterGroupItem_v1(
                policyAirParameterGroupItem.PolicyAirParameterGroupItemId,
                adminUserGuid,
                policyAirParameterGroupItem.VersionNumber
            );
        }

        //Get one Item
        public PolicyAirParameterGroupItem GetPolicyAirParameterGroupItem(int policyAirParameterGroupItemId)
        {
            return db.PolicyAirParameterGroupItems.SingleOrDefault(c => c.PolicyAirParameterGroupItemId == policyAirParameterGroupItemId);
        }

        //Add Data From Linked Tables for Display
        public void EditItemForDisplay(PolicyAirParameterGroupItem policyAirParameterGroupItem)
        {
            //populate new PolicyAirParameterGroupItem with PolicyGroupName    
            if (policyAirParameterGroupItem.PolicyGroupId != 0)
            {
                PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();
                PolicyGroup policyGroup = new PolicyGroup();
                policyGroup = policyGroupRepository.GetGroup(policyAirParameterGroupItem.PolicyGroupId);
                policyAirParameterGroupItem.PolicyGroupName = policyGroup.PolicyGroupName;
            }
        }
    }
}
