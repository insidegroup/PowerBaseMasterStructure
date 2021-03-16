using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Helpers;
using System.Linq.Dynamic;
using System.Text.RegularExpressions;

namespace CWTDesktopDatabase.Repository
{
    public class ProductGroupRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get a Page of PolicyGroups - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectProductGroups_v1Result> PageProductGroups(bool deleted, int productGroupDomainTypeId, int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectProductGroups_v1(deleted, productGroupDomainTypeId, filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectProductGroups_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get a Page of PolicyGroups (Orphaned) - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectProductGroupsOrphaned_v1Result> PageOrphanedPolicyGroups(int page, int productGroupDomainTypeId, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectProductGroupsOrphaned_v1(productGroupDomainTypeId, filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectProductGroupsOrphaned_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }


        //Change the deleted status on an item
        public void UpdateGroupDeletedStatus(ProductGroup group)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateProductGroupDeletedStatus_v1(
                group.ProductGroupId,
                group.DeletedFlag,
                adminUserGuid,
                group.VersionNumber
                );
        }

        //Add Data From Linked Tables for Display
        public void EditGroupForDisplay(ProductGroup group)
        {

            HierarchyRepository hierarchyRepository = new HierarchyRepository();

            fnDesktopDataAdmin_SelectProductGroupHierarchy_v1Result hierarchy = new fnDesktopDataAdmin_SelectProductGroupHierarchy_v1Result();
            hierarchy = GetGroupHierarchy(group.ProductGroupId);
            group.ProductGroupName = Regex.Replace(group.ProductGroupName, @"[^\w\-()*]", "-");

            if (hierarchy != null)
            {

                group.HierarchyType = hierarchy.HierarchyType;
                group.HierarchyCode = hierarchy.HierarchyCode.ToString();
                group.HierarchyItem = hierarchy.HierarchyName.Trim();
            }
            group.EnabledFlagNonNullable = (group.EnabledFlag == true)? true: false;
        }
        
        //Get Hierarchy Details
        public fnDesktopDataAdmin_SelectProductGroupHierarchy_v1Result GetGroupHierarchy(int id)
        {
            var result = db.fnDesktopDataAdmin_SelectProductGroupHierarchy_v1(id).FirstOrDefault();
            return result;
        }
        
 
    }
}

