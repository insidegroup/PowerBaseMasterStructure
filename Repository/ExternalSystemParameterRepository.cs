using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using System.Linq.Dynamic;

namespace CWTDesktopDatabase.Repository
{
    public class ExternalSystemParameterRepository
    {
        private ExternalSystemParameterDC db = new ExternalSystemParameterDC(Settings.getConnectionString());

        //Get a Page of ExternalSystemParameters - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectExternalSystemParameters_v1Result> PageExternalSystemParameters(bool deleted, int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectExternalSystemParameters_v1(deleted, filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectExternalSystemParameters_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get a Page of ExternalSystemParameters (Orphaned) - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectExternalSystemParametersOrphaned_v1Result> PageOrphanedExternalSystemParameters(int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectExternalSystemParametersOrphaned_v1(filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectExternalSystemParametersOrphaned_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get one ExternalSystemParameter
        public ExternalSystemParameter GetGroup(int id)
        {
            return db.ExternalSystemParameters.SingleOrDefault(c => c.ExternalSystemParameterId == id);
        }

        //Add Data From Linked Tables for Display
        public void EditGroupForDisplay(ExternalSystemParameter group)
        {
            TripTypeRepository tripTypeRepository = new TripTypeRepository();
            TripType tripType = new TripType();
            tripType = tripTypeRepository.GetTripType(group.TripTypeId);
            if (tripType != null)
            {
                group.TripType = tripType.TripTypeDescription;
            }

            ExternalSystemParameterTypeRepository externalSystemParameterTypeRepository = new ExternalSystemParameterTypeRepository();
            ExternalSystemParameterType externalSystemParameterType = new ExternalSystemParameterType();
            externalSystemParameterType = externalSystemParameterTypeRepository.GetExternalSystemParameterType(group.ExternalSystemParameterTypeId);
            if (externalSystemParameterType != null)
            {
                group.ExternalSystemParameterType = externalSystemParameterType.ExternalSystemParameterTypeName;
            }

            fnDesktopDataAdmin_SelectExternalSystemParameterHierarchy_v1Result hierarchy = new fnDesktopDataAdmin_SelectExternalSystemParameterHierarchy_v1Result();
            hierarchy = GetGroupHierarchy(group.ExternalSystemParameterId);

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
        public fnDesktopDataAdmin_SelectExternalSystemParameterHierarchy_v1Result GetGroupHierarchy(int id)
        {
            var result = db.fnDesktopDataAdmin_SelectExternalSystemParameterHierarchy_v1(id).FirstOrDefault();
            return result;
        }

        //Edit Group
        public void Edit(ExternalSystemParameter group)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateExternalSystemParameter_v1(

                group.ExternalSystemParameterId,
                group.ExternalSystemParameterTypeId,
                group.ExternalSystemParameterValue,
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
        public void Add(ExternalSystemParameter group)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertExternalSystemParameter_v1(
                group.ExternalSystemParameterTypeId,
                group.ExternalSystemParameterValue,
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
        public void UpdateGroupDeletedStatus(ExternalSystemParameter group)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateExternalSystemParameterDeletedStatus_v1(
                    group.ExternalSystemParameterId,
                    group.DeletedFlag,
                    adminUserGuid,
                    group.VersionNumber
                    );

        }

        //REMOVED: List of All Items - Sortable
        /*public IQueryable<fnDesktopDataAdmin_SelectExternalSystemParameters_v1Result> GetGroupsByDeletedFlag(string filter, bool deleted, string sortField, int sortOrder)
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
                return db.fnDesktopDataAdmin_SelectExternalSystemParameters_v1(deleted, adminUserGuid).OrderBy(sortField);
            }
            else
            {
                return db.fnDesktopDataAdmin_SelectExternalSystemParameters_v1(deleted, adminUserGuid).OrderBy(sortField).Where(c => c.ExternalSystemParameterValue.Contains(filter));
            }
        }*/

    }
}

