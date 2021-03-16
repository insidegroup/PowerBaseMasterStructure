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
    public class PublicHolidayGroupRepository
    {
        private PublicHolidayGroupDC db = new PublicHolidayGroupDC(Settings.getConnectionString());

        //Get a Page of PublicHolidayGroups - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectPublicHolidayGroups_v1Result> PagePublicHolidayGroups(int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectPublicHolidayGroups_v1(filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPublicHolidayGroups_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get a Page of PublicHolidayGroups (Orphaned) - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectPublicHolidayGroupsOrphaned_v1Result> PageOrphanedPublicHolidayGroups(int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectPublicHolidayGroupsOrphaned_v1(filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPublicHolidayGroupsOrphaned_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get one PublicHolidayGroup
        public PublicHolidayGroup GetGroup(int id)
        {
            return db.PublicHolidayGroups.SingleOrDefault(c => c.PublicHolidayGroupId == id);
        }

        //Add Data From Linked Tables for Display
        public void EditGroupForDisplay(PublicHolidayGroup group)
        {
            group.PublicHolidayGroupName = Regex.Replace(group.PublicHolidayGroupName, @"[^\w\-()*]", "-");

            fnDesktopDataAdmin_SelectPublicHolidayGroupHierarchy_v1Result hierarchy = GetGroupHierarchy(group.PublicHolidayGroupId);
            if (hierarchy != null)
            {
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
        public fnDesktopDataAdmin_SelectPublicHolidayGroupHierarchy_v1Result GetGroupHierarchy(int id)
        {
            var result = db.fnDesktopDataAdmin_SelectPublicHolidayGroupHierarchy_v1(id).FirstOrDefault();
            return result;
        }

        //Edit Group
        public void Edit(PublicHolidayGroup group)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdatePublicHolidayGroup_v1(

                group.PublicHolidayGroupId,
                group.PublicHolidayGroupName,
                group.EnabledFlag,
                group.EnabledDate,
                group.ExpiryDate,
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
        public void Add(PublicHolidayGroup group)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertPublicHolidayGroup_v1(
                group.PublicHolidayGroupName,
                group.EnabledFlag,
                group.EnabledDate,
                group.ExpiryDate,
                group.HierarchyType,
                group.HierarchyCode,
                group.TravelerTypeGuid,
                group.ClientSubUnitGuid,
                group.SourceSystemCode,
                adminUserGuid
            );
        }

        //Delete Group
        public void Delete(PublicHolidayGroup group)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeletePublicHolidayGroup_v1(
                group.PublicHolidayGroupId,
                adminUserGuid,
                group.VersionNumber);
        }

        //REMOVED: List of All Items - Sortable
        /*public IQueryable<fnDesktopDataAdmin_SelectPublicHolidayGroups_v1Result> GetGroups(string filter, string sortField, int sortOrder)
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
                return db.fnDesktopDataAdmin_SelectPublicHolidayGroups_v1(adminUserGuid).OrderBy(sortField);
            }
            else
            {
                return db.fnDesktopDataAdmin_SelectPublicHolidayGroups_v1(adminUserGuid).OrderBy(sortField).Where(c => c.PublicHolidayGroupName.Contains(filter));
            }
        }*/
    }
}

