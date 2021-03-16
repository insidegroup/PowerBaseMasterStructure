using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Linq.Dynamic;
using System.Text.RegularExpressions;

namespace CWTDesktopDatabase.Repository
{
    public class CommissionableRouteGroupRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get a Page of Commissionable Route Groups - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectCommissionableRouteGroups_v1Result> PageCommissionableRouteGroups(bool deleted, int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectCommissionableRouteGroups_v1(deleted, filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectCommissionableRouteGroups_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get a Page of Commissionable Route Groups (Orphaned) - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectCommissionableRouteGroupsOrphaned_v1Result> PageOrphanedCommissionableRouteGroups(int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectCommissionableRouteGroupsOrphaned_v1(filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectCommissionableRouteGroupsOrphaned_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get one Commissionable Route Group
        public CommissionableRouteGroup GetGroup(int id)
        {
            return db.CommissionableRouteGroups.SingleOrDefault(c => c.CommissionableRouteGroupId == id);
        }

        //Change the deleted status on an item
        public void UpdateLinkedClientSubUnit(int CommissionableRouteGroupId, string clientSubUnitGuid)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			//db.spDesktopDataAdmin_UpdateCommissionableRouteGroupLinkedClientSubUnit_v1(
			//		CommissionableRouteGroupId,
			//		clientSubUnitGuid,
			//		adminUserGuid
			//		);

        }

        //Add Data From Linked Tables for Display
        public void EditGroupForDisplay(CommissionableRouteGroup group)
        {

            TripTypeRepository tripTypeRepository = new TripTypeRepository();
            TripType tripType = new TripType();
            tripType = tripTypeRepository.GetTripType(group.TripTypeId);
            if (tripType != null)
            {
                group.TripType = tripType;
            }

            group.CommissionableRouteGroupName = Regex.Replace(group.CommissionableRouteGroupName, @"[^\w\-()*]", "-");

            List<fnDesktopDataAdmin_SelectCommissionableRouteGroupHierarchy_v1Result> hierarchy = new List<fnDesktopDataAdmin_SelectCommissionableRouteGroupHierarchy_v1Result>();
            hierarchy = GetGroupHierarchy(group.CommissionableRouteGroupId);

            if (hierarchy.Count > 0)
            {
                HierarchyRepository hierarchyRepository = new HierarchyRepository();
                HierarchyGroup hierarchyGroup = hierarchyRepository.GetHierarchyGroup(
                    hierarchy[0].HierarchyType ?? "",
					hierarchy[0].HierarchyCode ?? "",
                    hierarchy[0].HierarchyName ?? "",
                    hierarchy[0].TravelerTypeGuid ?? "",
                    hierarchy[0].TravelerTypeName ?? "",
                    hierarchy[0].SourceSystemCode ?? ""
                );

                if (hierarchyGroup != null)
                {
                    group.HierarchyType = hierarchyGroup.HierarchyType;
                    group.HierarchyCode = hierarchyGroup.HierarchyCode;
                    group.HierarchyItem = hierarchyGroup.HierarchyItem;
                    group.ClientSubUnitGuid = hierarchyGroup.ClientSubUnitGuid;
                    group.ClientSubUnitName = hierarchyGroup.ClientSubUnitName;
                    group.TravelerTypeGuid = hierarchyGroup.TravelerTypeGuid;
                    group.TravelerTypeName = hierarchyGroup.TravelerTypeName;
                    group.ClientTopUnitName = hierarchyGroup.ClientTopUnitName;
                    group.SourceSystemCode = hierarchyGroup.SourceSystemCode;
                }
            }

			if (hierarchy.Count > 1)
			{
				group.IsMultipleHierarchy = true;
				group.HierarchyType = "Multiple";
				group.HierarchyItem = "Multiple";
				group.HierarchyCode = "Multiple";
			}
			else
			{
				group.IsMultipleHierarchy = false;
			}
        }

        //Get Hierarchy Details
        public List<fnDesktopDataAdmin_SelectCommissionableRouteGroupHierarchy_v1Result> GetGroupHierarchy(int id)
        {
            var result = db.fnDesktopDataAdmin_SelectCommissionableRouteGroupHierarchy_v1(id);
            return result.ToList();
        }

        //Change the deleted status on an item
        public void UpdateGroupDeletedStatus(CommissionableRouteGroup group)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateCommissionableRouteGroupDeletedStatus_v1(
                    group.CommissionableRouteGroupId,
                    group.DeletedFlag,
                    adminUserGuid,
                    group.VersionNumber
                    );

        }

        //Edit Group
        public void Edit(CommissionableRouteGroup group)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateCommissionableRouteGroup_v1(
                adminUserGuid,
                group.CommissionableRouteGroupId,
                group.CommissionableRouteGroupName,
                group.EnabledFlag,
                group.EnabledDate,
                group.ExpiryDate,
                group.InheritFromParentFlag,
                group.HierarchyType,
                group.HierarchyCode,
                group.TravelerTypeGuid,
                group.ClientSubUnitGuid,
                group.SourceSystemCode,
                group.IsMultipleHierarchy,
                adminUserGuid,
                group.VersionNumber
            );
        }

        //Add Group
        public void Add(CommissionableRouteGroup group)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertCommissionableRouteGroup_v1(
                adminUserGuid,
                group.CommissionableRouteGroupName,
                group.EnabledFlag,
                group.EnabledDate,
                group.ExpiryDate,
				group.InheritFromParentFlag,
                group.HierarchyType,
                group.HierarchyCode,
                group.TravelerTypeGuid,
                group.ClientSubUnitGuid,
                group.SourceSystemCode,
                adminUserGuid
            );
        }
    }
}
