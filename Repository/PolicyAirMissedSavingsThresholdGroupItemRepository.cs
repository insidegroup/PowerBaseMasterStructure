using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Helpers;
using System.Linq.Dynamic;

namespace CWTDesktopDatabase.Repository
{
    public class PolicyAirMissedSavingsThresholdGroupItemRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Sortable List
        public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyAirMissedSavingsThresholdGroupItems_v1Result> GetPolicyAirMissedSavingsThresholdGroupItems(int policyGroupID, string sortField, int sortOrder, int page)
        {
            //query db
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectPolicyAirMissedSavingsThresholdGroupItems_v1(policyGroupID, adminUserGuid, sortField, Convert.ToBoolean(sortOrder), page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //add paging information and return1
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPolicyAirMissedSavingsThresholdGroupItems_v1Result>(result, page, totalRecords);
            return paginatedView;

        }

        //Get one Item
        public PolicyAirMissedSavingsThresholdGroupItem GetPolicyAirMissedSavingsThresholdGroupItem(int policyAirMissedSavingsThresholdGroupItemId)
        {
            return db.PolicyAirMissedSavingsThresholdGroupItems.SingleOrDefault(c => c.PolicyAirMissedSavingsThresholdGroupItemId == policyAirMissedSavingsThresholdGroupItemId);

        }

        //Add
        public void Add(PolicyAirMissedSavingsThresholdGroupItem policyAirMissedItem)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertPolicyAirMissedSavingsThresholdGroupItem_v1(
                policyAirMissedItem.PolicyGroupId,
                policyAirMissedItem.MissedThresholdAmount,
                policyAirMissedItem.CurrencyCode,
                policyAirMissedItem.RoutingCode,
                policyAirMissedItem.EnabledFlag,
                policyAirMissedItem.EnabledDate,
                policyAirMissedItem.ExpiryDate,
                policyAirMissedItem.TravelDateValidFrom,
                policyAirMissedItem.TravelDateValidTo,
                adminUserGuid
            );

        }

        //Update
        public void Update(PolicyAirMissedSavingsThresholdGroupItem policyAirMissedItem)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdatePolicyAirMissedSavingsThresholdGroupItem_v1(
                policyAirMissedItem.PolicyAirMissedSavingsThresholdGroupItemId,
                policyAirMissedItem.MissedThresholdAmount,
                policyAirMissedItem.CurrencyCode,
                policyAirMissedItem.RoutingCode,
                policyAirMissedItem.EnabledFlag,
                policyAirMissedItem.EnabledDate,
                policyAirMissedItem.ExpiryDate,
                policyAirMissedItem.TravelDateValidFrom,
                policyAirMissedItem.TravelDateValidTo,
                adminUserGuid,
                policyAirMissedItem.VersionNumber
            );

        }
        
        //Delete
        public void Delete(PolicyAirMissedSavingsThresholdGroupItem policyAirMissedItem)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeletePolicyAirMissedSavingsThresholdGroupItem_v1(
                policyAirMissedItem.PolicyAirMissedSavingsThresholdGroupItemId,
                adminUserGuid,
                policyAirMissedItem.VersionNumber
                );
        }
    }
}
