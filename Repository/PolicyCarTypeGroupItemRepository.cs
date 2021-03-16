using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Linq.Dynamic;

namespace CWTDesktopDatabase.Repository
{
    public class PolicyCarTypeGroupItemRepository
    {
        private PolicyCarTypeGroupItemDC db = new PolicyCarTypeGroupItemDC(Settings.getConnectionString());

        //Sortable List
        public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyCarTypeGroupItems_v1Result> GetPolicyCarTypeGroupItems(int policyGroupID, string sortField, int sortOrder, int page)
        {
            //query db
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectPolicyCarTypeGroupItems_v1(policyGroupID, adminUserGuid, sortField, Convert.ToBoolean(sortOrder), page).ToList();
            
            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //add paging information and return1
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPolicyCarTypeGroupItems_v1Result>(result, page, totalRecords);
            return paginatedView;

        }

        //Get one Item
        public PolicyCarTypeGroupItem GetPolicyCarTypeGroupItem(int policyCarTypeGroupItemId)
        {
            return db.PolicyCarTypeGroupItems.SingleOrDefault(c => c.PolicyCarTypeGroupItemId == policyCarTypeGroupItemId);
        }

        //Add Data From Linked Tables for Display
        public void EditItemForDisplay(PolicyCarTypeGroupItem policyCarTypeGroupItem)
        {
            //PolicyGroupName    
            PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();
            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(policyCarTypeGroupItem.PolicyGroupId);
            policyCarTypeGroupItem.PolicyGroupName = policyGroup.PolicyGroupName;

            //CarStatus
            PolicyCarStatusRepository policyCarStatusRepository = new PolicyCarStatusRepository();
            PolicyCarStatus policyCarStatus = new PolicyCarStatus();
            policyCarStatus = policyCarStatusRepository.GetPolicyCarStatus(policyCarTypeGroupItem.PolicyCarStatusId);
            policyCarTypeGroupItem.PolicyCarStatusDescription = policyCarStatus.PolicyCarStatusDescription;

            //CarType Category
            CarTypeCategoryRepository carTypeCategoryRepository = new CarTypeCategoryRepository();
            CarTypeCategory carTypeCategory = new CarTypeCategory();
            carTypeCategory = carTypeCategoryRepository.GetCarTypeCategory(policyCarTypeGroupItem.CarTypeCategoryId);
            if (carTypeCategory != null)
            {
                policyCarTypeGroupItem.CarTypeCategoryName = carTypeCategory.CarTypeCategoryName;
            }

            //PolicyLocation
            PolicyLocationRepository policyLocationRepository = new PolicyLocationRepository();
            PolicyLocation policyLocation = new PolicyLocation();
            policyLocation = policyLocationRepository.GetPolicyLocation(policyCarTypeGroupItem.PolicyLocationId);
            if (policyLocation != null)
            {
                policyCarTypeGroupItem.PolicyLocation = policyLocation.PolicyLocationName;
            }

        }


        //Add
        public void Add(PolicyCarTypeGroupItem policyCarTypeGroupItem)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertPolicyCarTypeGroupItem_v1(
                policyCarTypeGroupItem.PolicyGroupId,
                policyCarTypeGroupItem.PolicyLocationId,
                policyCarTypeGroupItem.PolicyCarStatusId,
                policyCarTypeGroupItem.EnabledFlag,
                policyCarTypeGroupItem.EnabledDate,
                policyCarTypeGroupItem.ExpiryDate,
                policyCarTypeGroupItem.CarTypeCategoryId,
                policyCarTypeGroupItem.TravelDateValidFrom,
                policyCarTypeGroupItem.TravelDateValidTo,
                adminUserGuid
            );

        }

        //Add
        public void Update(PolicyCarTypeGroupItem policyCarTypeGroupItem)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdatePolicyCarTypeGroupItem_v1(
                policyCarTypeGroupItem.PolicyCarTypeGroupItemId,
                policyCarTypeGroupItem.PolicyGroupId,
                policyCarTypeGroupItem.PolicyLocationId,
                policyCarTypeGroupItem.PolicyCarStatusId,
                policyCarTypeGroupItem.CarTypeCategoryId,
                policyCarTypeGroupItem.EnabledFlag,
                policyCarTypeGroupItem.EnabledDate,
                policyCarTypeGroupItem.ExpiryDate,
                policyCarTypeGroupItem.TravelDateValidFrom,
                policyCarTypeGroupItem.TravelDateValidTo,
                adminUserGuid,
                policyCarTypeGroupItem.VersionNumber
            );

        }

        //Delete
        public void Delete(PolicyCarTypeGroupItem policyCarTypeGroupItem)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeletePolicyCarTypeGroupItem_v1(
                policyCarTypeGroupItem.PolicyCarTypeGroupItemId,
                adminUserGuid,
                policyCarTypeGroupItem.VersionNumber
                );
        }

    }
}
