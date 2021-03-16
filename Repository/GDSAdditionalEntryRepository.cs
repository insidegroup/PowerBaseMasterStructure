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
    public class GDSAdditionalEntryRepository
    {
        private GDSAdditionalEntryDC db = new GDSAdditionalEntryDC(Settings.getConnectionString());

        //Get a Page of GDSAdditionalEntries - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectGDSAdditionalEntries_v1Result> PageGDSAdditionalEntries(bool deleted, int page, string filter, string sortField, int sortOrder)
         {
             //get a page of records
             string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
             var result = db.spDesktopDataAdmin_SelectGDSAdditionalEntries_v1(deleted, filter, sortField, sortOrder, page, adminUserGuid).ToList();

             //total records for paging
             int totalRecords = 0;
             if (result.Count() > 0)
             {
                 totalRecords = (int)result.First().RecordCount;
             }

             //put into page object
             var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectGDSAdditionalEntries_v1Result>(result, page, totalRecords);

             //return to user
             return paginatedView;
         }

        //Get a Page of GDSAdditionalEntries (Orphaned) - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectGDSAdditionalEntriesOrphaned_v1Result> PageOrphanedGDSAdditionalEntries(int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectGDSAdditionalEntriesOrphaned_v1(filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectGDSAdditionalEntriesOrphaned_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get one GDSAdditionalEntry
        public GDSAdditionalEntry GetGroup(int id)
        {
            return db.GDSAdditionalEntries.SingleOrDefault(c => c.GDSAdditionalEntryId == id);
        }

        //Add Data From Linked Tables for Display
        public void EditGroupForDisplay(GDSAdditionalEntry group)
        {
            TripTypeRepository tripTypeRepository = new TripTypeRepository();
            TripType tripType = new TripType();
            tripType = tripTypeRepository.GetTripType(group.TripTypeId);
            if (tripType != null)
            {
                group.TripType = tripType.TripTypeDescription;
            }

            GDSRepository gDSRepository = new GDSRepository();
            GDS gds = new GDS();
            gds = gDSRepository.GetGDS(group.GDSCode);
            if (gds != null)
            {
                group.GDSName = gds.GDSName;
            }

            SubProductRepository subProductRepository = new SubProductRepository();
            SubProduct subProduct = new SubProduct();
            subProduct = subProductRepository.GetSubProduct(group.SubProductId);
            if (subProduct != null)
            {
                group.SubProductName = subProduct.SubProductName;

                Product product = new Product();
                product = subProductRepository.GetSubProductProduct(group.SubProductId);
                group.ProductId = product.ProductId;
                group.ProductName = product.ProductName;
            }


            HierarchyRepository hierarchyRepository = new HierarchyRepository();

            fnDesktopDataAdmin_SelectGDSAdditionalEntryHierarchy_v1Result hierarchy = new fnDesktopDataAdmin_SelectGDSAdditionalEntryHierarchy_v1Result();
            hierarchy = GetGroupHierarchy(group.GDSAdditionalEntryId);
            group.GDSAdditionalEntryValue = Regex.Replace(group.GDSAdditionalEntryValue, @"[^\w\-()*]", "-");

            if(hierarchy != null){
                group.HierarchyType = hierarchy.HierarchyType;
                group.HierarchyCode = hierarchy.HierarchyCode.ToString();
                group.HierarchyItem = hierarchy.HierarchyName.Trim();
                

                if (hierarchy.HierarchyType == "ClientSubUnitTravelerType")
                {
                    group.ClientSubUnitGuid = hierarchy.HierarchyCode.ToString();
                    group.ClientSubUnitName = hierarchy.HierarchyName.Trim();
                    group.TravelerTypeGuid = hierarchy.TravelerTypeGuid;
                    group.TravelerTypeName = hierarchy.TravelerTypeName.Trim();
                }
            }
        }

        //Get Hierarchy Details
        public fnDesktopDataAdmin_SelectGDSAdditionalEntryHierarchy_v1Result GetGroupHierarchy(int id)
        {
            var result = db.fnDesktopDataAdmin_SelectGDSAdditionalEntryHierarchy_v1(id).SingleOrDefault();
            return result;
        }

        //Edit Group
        public void Edit(GDSAdditionalEntry group)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateGDSAdditionalEntry_v1(

                group.GDSAdditionalEntryId,
                group.GDSAdditionalEntryValue,
                group.GDSCode,
                group.GDSAdditionalEntryEventId,
                group.SubProductId,
                group.EnabledFlag,
                group.EnabledDate,
                group.ExpiryDate,
                group.TripTypeId,
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
        public void Add(GDSAdditionalEntry group)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertGDSAdditionalEntry_v1(
                group.GDSAdditionalEntryValue,
                group.GDSCode,
                group.GDSAdditionalEntryEventId,
                group.SubProductId,
                group.EnabledFlag,
                group.EnabledDate,
                group.ExpiryDate,
                group.TripTypeId,
                group.HierarchyType,
                group.HierarchyCode,
                group.TravelerTypeGuid,
                group.ClientSubUnitGuid,
                group.SourceSystemCode,
                adminUserGuid
            );
        }

        //Change the deleted status on an item
        public void UpdateGroupDeletedStatus(GDSAdditionalEntry group)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateGDSAdditionalEntryDeletedStatus_v1(
                group.GDSAdditionalEntryId,
                group.DeletedFlag,
                adminUserGuid,
                group.VersionNumber
                );
        }

        //REMOVED:List of All Items - Sortable
        /*public IQueryable<fnDesktopDataAdmin_SelectGDSAdditionalEntries_v1Result> GetGroupsByDeletedFlag(string filter, bool deleted, string sortField, int sortOrder)
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
                return db.fnDesktopDataAdmin_SelectGDSAdditionalEntries_v1(deleted, adminUserGuid).OrderBy(sortField);
            }
            else
            {
                return db.fnDesktopDataAdmin_SelectGDSAdditionalEntries_v1(deleted, adminUserGuid).OrderBy(sortField).Where(c => c.GDSAdditionalEntryValue.Contains(filter));
            }
        }*/

    }
}

