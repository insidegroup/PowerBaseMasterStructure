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
    public class LocalOperatingHoursGroupRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get a Page of LocalOperatingHoursGroups - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectLocalOperatingHoursGroups_v1Result> PageLocalOperatingHoursGroups(int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectLocalOperatingHoursGroups_v1(filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectLocalOperatingHoursGroups_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get a Page of LocalOperatingHoursGroups (Orphaned) - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectLocalOperatingHoursGroupsOrphaned_v1Result> PageOrphanedLocalOperatingHoursGroups(int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectLocalOperatingHoursGroupsOrphaned_v1(filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectLocalOperatingHoursGroupsOrphaned_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get one LocalOperatingHoursGroup
        public LocalOperatingHoursGroup GetGroup(int id)
        {
            return db.LocalOperatingHoursGroups.SingleOrDefault(c => c.LocalOperatingHoursGroupId == id);
        }

        //Add Data From Linked Tables for Display
        public void EditGroupForDisplay(LocalOperatingHoursGroup group)
        {
            group.LocalOperatingHoursGroupName = Regex.Replace(group.LocalOperatingHoursGroupName, @"[^\w\-()*]", "-");

            fnDesktopDataAdmin_SelectLocalOperatingHoursGroupHierarchy_v1Result hierarchy = GetGroupHierarchy(group.LocalOperatingHoursGroupId);

            if (hierarchy != null){

                HierarchyRepository hierarchyRepository = new HierarchyRepository();
                HierarchyGroup hierarchyGroup = hierarchyRepository.GetHierarchyGroup(
                    hierarchy.HierarchyType ?? "",
					hierarchy.HierarchyCode ?? "",
                    hierarchy.HierarchyName ?? "",
                    hierarchy.TravelerTypeGuid ?? "",
                    hierarchy.TravelerTypeName ?? "",
                    hierarchy.SourceSystemCode ?? ""
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
        }

        //Get Hierarchy Details
        public fnDesktopDataAdmin_SelectLocalOperatingHoursGroupHierarchy_v1Result GetGroupHierarchy(int id)
        {
            var result = db.fnDesktopDataAdmin_SelectLocalOperatingHoursGroupHierarchy_v1(id).FirstOrDefault();
            return result;
        }

        //Edit Group
        public void Edit(LocalOperatingHoursGroup group)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateLocalOperatingHoursGroup_v1(

                group.LocalOperatingHoursGroupId,
                group.LocalOperatingHoursGroupName,
                group.HierarchyType,
                group.HierarchyCode,
                group.TravelerTypeGuid,
                group.ClientSubUnitGuid,
                group.SourceSystemCode,
                adminUserGuid,
                group.VersionNumber
            );
        }

        //Add Group
        public void Add(LocalOperatingHoursGroup group)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertLocalOperatingHoursGroup_v1(
                adminUserGuid,
                group.LocalOperatingHoursGroupName,
                group.HierarchyType,
                group.HierarchyCode,
                group.TravelerTypeGuid,
                group.ClientSubUnitGuid,
                group.SourceSystemCode,
                adminUserGuid
            );
        }

        //Delete Group
        public void Delete(LocalOperatingHoursGroup group)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeleteLocalOperatingHoursGroup_v1(
                group.LocalOperatingHoursGroupId,
                adminUserGuid,
                group.VersionNumber);
        }

        //REMOVED: List of All Items - Sortable
        /*
        public IQueryable<fnDesktopDataAdmin_SelectLocalOperatingHoursGroups_v1Result> GetGroups(string filter, string sortField, int sortOrder)
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
                return db.fnDesktopDataAdmin_SelectLocalOperatingHoursGroups_v1(adminUserGuid).OrderBy(sortField);
            }
            else
            {
                return db.fnDesktopDataAdmin_SelectLocalOperatingHoursGroups_v1(adminUserGuid).OrderBy(sortField).Where(c => c.LocalOperatingHoursGroupName.Contains(filter));
            } 
        }
        */
    }
}

