using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using System.Linq.Dynamic;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.Repository
{
    public class PolicyHotelPropertyGroupItemRepository
    {
        private PolicyHotelPropertyGroupItemDC db = new PolicyHotelPropertyGroupItemDC(Settings.getConnectionString());

        //Sortable List
		public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyHotelPropertyGroupItems_v1Result> GetPolicyHotelPropertyGroupItemsByPolicyGroup(int policyGroupID, int page, string sortField, int sortOrder)
		{
			//get a page of records
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectPolicyHotelPropertyGroupItems_v1(policyGroupID, sortField, sortOrder, page, adminUserGuid).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}

			//put into page object
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPolicyHotelPropertyGroupItems_v1Result>(result, page, totalRecords);

			//return to user
			return paginatedView;
		}

        //Get one Item
        public PolicyHotelPropertyGroupItem GetPolicyHotelPropertyGroupItem(int policyHotelPropertyGroupItemId)
        {
            return db.PolicyHotelPropertyGroupItems.SingleOrDefault(c => c.PolicyHotelPropertyGroupItemId == policyHotelPropertyGroupItemId);
        }
        //Add
        public void Add(PolicyHotelPropertyGroupItem policyHotelPropertyGroupItem)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertPolicyHotelPropertyGroupItem_v1(
                 policyHotelPropertyGroupItem.PolicyGroupId,
                policyHotelPropertyGroupItem.PolicyHotelStatusId,
                policyHotelPropertyGroupItem.HarpHotelId,
                policyHotelPropertyGroupItem.EnabledFlag,
                policyHotelPropertyGroupItem.EnabledDate,
                policyHotelPropertyGroupItem.ExpiryDate,
                policyHotelPropertyGroupItem.TravelDateValidFrom,
                policyHotelPropertyGroupItem.TravelDateValidTo,
                adminUserGuid
            );

        }

        //Edit
        public void Update(PolicyHotelPropertyGroupItem policyHotelPropertyGroupItem)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdatePolicyHotelPropertyGroupItem_v1(
                policyHotelPropertyGroupItem.PolicyHotelPropertyGroupItemId,
                policyHotelPropertyGroupItem.PolicyGroupId,
                policyHotelPropertyGroupItem.PolicyHotelStatusId,
                policyHotelPropertyGroupItem.HarpHotelId,
                policyHotelPropertyGroupItem.EnabledFlag,
                policyHotelPropertyGroupItem.EnabledDate,
                policyHotelPropertyGroupItem.ExpiryDate,
                policyHotelPropertyGroupItem.TravelDateValidFrom,
                policyHotelPropertyGroupItem.TravelDateValidTo,
                adminUserGuid,
                policyHotelPropertyGroupItem.VersionNumber
            );

        }

        //Delete
        public void Delete(PolicyHotelPropertyGroupItem policyHotelPropertyGroupItem)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeletePolicyHotelPropertyGroupItem_v1(
                policyHotelPropertyGroupItem.PolicyHotelPropertyGroupItemId,
                adminUserGuid,
                policyHotelPropertyGroupItem.VersionNumber
                );
        }

        //Add Data From Linked Tables for Display
        public void EditItemForDisplay(PolicyHotelPropertyGroupItem policyHotelPropertyGroupItem)
        {
            //PolicyGroupName    
            PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();
            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(policyHotelPropertyGroupItem.PolicyGroupId);
            policyHotelPropertyGroupItem.PolicyGroupName = policyGroup.PolicyGroupName;

            //PolicyHotelStatus
            if (policyHotelPropertyGroupItem.PolicyHotelStatusId != null)
            {
                int policyHotelStatusId = (int)policyHotelPropertyGroupItem.PolicyHotelStatusId;
                PolicyHotelStatusRepository policyHotelStatusRepository = new PolicyHotelStatusRepository();
                PolicyHotelStatus policyHotelStatus = new PolicyHotelStatus();
                policyHotelStatus = policyHotelStatusRepository.GetPolicyHotelStatus(policyHotelStatusId);
                policyHotelPropertyGroupItem.PolicyHotelStatus = policyHotelStatus.PolicyHotelStatusDescription;
            }

            //HarpHotel
            HarpHotelRepository harpHotelRepository = new HarpHotelRepository();
            HarpHotel harpHotel = new HarpHotel();
            harpHotel = harpHotelRepository.GetHarpHotel(policyHotelPropertyGroupItem.HarpHotelId);
            if (harpHotel != null)
            {
                policyHotelPropertyGroupItem.HarpHotelName = harpHotel.HarpHotelName;
            }

        }

    }
}
