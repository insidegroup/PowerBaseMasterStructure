using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Linq.Dynamic;

namespace CWTDesktopDatabase.Repository
{
    public class WorkFlowGroupRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get a Page of WorkFlowGroups - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectWorkFlowGroups_v1Result> PageWorkFlowGroups(bool deleted, int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectWorkFlowGroups_v1(deleted, filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectWorkFlowGroups_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get a Page of WorkFlowGroups (Orphaned) - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectWorkFlowGroupsOrphaned_v1Result> PageOrphanedWorkFlowGroups(int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectWorkFlowGroupsOrphaned_v1(filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectWorkFlowGroupsOrphaned_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }


        //Get one WorkFlowGroup
        public WorkFlowGroup GetGroup(int id)
        {
            return db.WorkFlowGroups.SingleOrDefault(c => c.WorkFlowGroupId == id);
        }

        //Add Data From Linked Tables for Display
        /*public void EditGroupForDisplay(WorkFlowGroup group)
       {
          TripTypeRepository tripTypeRepository = new TripTypeRepository();
           TripType tripType = new TripType();
           tripType = tripTypeRepository.GetTripType(group.TripTypeId);
           if (tripType != null)
           {
               group.TripType = tripType.TripTypeDescription;
           }

           WorkFlowTypeRepository workFlowTypeRepository = new WorkFlowTypeRepository();
           WorkFlowType workFlowType = new WorkFlowType();
           workFlowType = workFlowTypeRepository.GetWorkFlowTypeType(group.WorkFlowTypeId);
           if (workFlowType != null)
           {
               group.WorkFlowType = workFlowType.WorkFlowTypeDescription;
           }


           HierarchyRepository hierarchyRepository = new HierarchyRepository();

           fnDesktopDataAdmin_SelectWorkflowGroupHierarchy_v1Result hierarchy = new fnDesktopDataAdmin_SelectWorkflowGroupHierarchy_v1Result();
           hierarchy = GetGroupHierarchy(group.WorkFlowGroupId);

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
       }*/

       //Get Hierarchy Details
       /*public fnDesktopDataAdmin_SelectWorkflowGroupHierarchy_v1Result GetGroupHierarchy(int id)
       {
           var result = db.fnDesktopDataAdmin_SelectWorkflowGroupHierarchy_v1(id).FirstOrDefault();
           return result;
       }

       //Edit Group
       public void Edit(WorkFlowGroup group)
       {
           string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

           db.spDesktopDataAdmin_UpdateWorkflowGroup_v1(

               group.WorkFlowGroupId,
               group.WorkFlowGroupName,
               group.WorkFlowTypeId,
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
       }*/

        //Add Group
        /*public void Add(WorkFlowGroup group)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertWorkflowGroup_v1(
                group.WorkFlowGroupName,
                group.WorkFlowTypeId,
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
        }*/

        //Change the deleted status on an item
        /*public void UpdateGroupDeletedStatus(WorkFlowGroup group)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateWorkflowGroupDeletedStatus_v1(
                    group.WorkFlowGroupId,
                    group.DeletedFlag,
                    adminUserGuid,
                    group.VersionNumber
                    );

        }*/

        //REMOVED: List of All Items - Sortable
        /*public IQueryable<fnDesktopDataAdmin_SelectWorkflowGroups_v1Result> GetGroupsByDeletedFlag(string filter, bool deleted, string sortField, int sortOrder)
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
                return db.fnDesktopDataAdmin_SelectWorkflowGroups_v1(deleted, adminUserGuid).OrderBy(sortField);
            }
            else
            {
                return db.fnDesktopDataAdmin_SelectWorkflowGroups_v1(deleted, adminUserGuid).OrderBy(sortField).Where(c => c.WorkFlowGroupName.Contains(filter));
            }
        }
        */
    }
}

