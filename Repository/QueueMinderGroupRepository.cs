using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Linq.Dynamic;
using System.Text.RegularExpressions;
using CWTDesktopDatabase.ViewModels;

namespace CWTDesktopDatabase.Repository
{
    public class QueueMinderGroupRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get a Page of QueueMinderGroups - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectQueueMinderGroups_v1Result> PageQueueMinderGroups(bool deleted, int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectQueueMinderGroups_v1(deleted, filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectQueueMinderGroups_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get a Page of QueueMinderGroups (Orphaned) - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectQueueMinderGroupsOrphaned_v1Result> PageOrphanedQueueMinderGroups(int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectQueueMinderGroupsOrphaned_v1(filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectQueueMinderGroupsOrphaned_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get one QueueMinderGroup
        public QueueMinderGroup GetGroup(int id)
        {
            return db.QueueMinderGroups.SingleOrDefault(c => c.QueueMinderGroupId == id);
        }

        //Add Data From Linked Tables for Display
        public void EditGroupForDisplay(QueueMinderGroup group)
        {
            //QueueMinderGroupName
            group.QueueMinderGroupName = Regex.Replace(group.QueueMinderGroupName, @"[^\w\-()*]", "-");

            //Hierarchy
            HierarchyRepository hierarchyRepository = new HierarchyRepository();

            List<fnDesktopDataAdmin_SelectQueueMinderGroupHierarchy_v1Result> hierarchy = GetGroupHierarchy(group.QueueMinderGroupId);
            if (hierarchy.Count > 0)
            {
                if (hierarchy.Count == 1)
                {
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
                else
                {
                    List<MultipleHierarchyDefinition> multipleHierarchies = new List<MultipleHierarchyDefinition>();
                    foreach (fnDesktopDataAdmin_SelectQueueMinderGroupHierarchy_v1Result item in hierarchy)
                    {
                        multipleHierarchies.Add(new MultipleHierarchyDefinition()
                        {
                            HierarchyType = item.HierarchyType,
                            HierarchyItem = item.HierarchyName,
                            HierarchyCode = item.HierarchyCode,
                            TravelerTypeGuid = item.TravelerTypeGuid,
                            SourceSystemCode = item.SourceSystemCode
                        });
                    }
                    group.ClientSubUnitsHierarchy = hierarchyRepository.GetClientSubUnitHierarchies(multipleHierarchies);

                    ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
                    group.ClientTopUnitName = clientSubUnitRepository.GetClientSubUnitClientTopUnitName(group.ClientSubUnitsHierarchy.First().ClientSubUnitGuid);
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

            //Meetings
            if (group.MeetingID != null)
			{
				int meetingID = Int32.Parse(group.MeetingID.ToString());
				MeetingRepository meetingRepository = new MeetingRepository();
				Meeting meeting = meetingRepository.GetGroup(meetingID);
				if (meeting != null)
				{
					group.Meeting = meeting;
				}
			}
        }

        //Get Hierarchy Details
        public List<fnDesktopDataAdmin_SelectQueueMinderGroupHierarchy_v1Result> GetGroupHierarchy(int id)
        {
            var result = db.fnDesktopDataAdmin_SelectQueueMinderGroupHierarchy_v1(id);
            return result.ToList();
        }

        //Add Group
        public void Add(QueueMinderGroup group)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertQueueMinderGroup_v1(
                group.QueueMinderGroupName,
                group.EnabledFlag,
                group.EnabledDate,
                group.ExpiryDate,
                group.InheritFromParentFlag,
                group.TripTypeId,
                group.HierarchyType,
                group.HierarchyCode,
                group.TravelerTypeGuid,
                group.ClientSubUnitGuid,
                group.SourceSystemCode,
				group.MeetingID,
                adminUserGuid

            );
        }

        //Edit Group
        public void Edit(QueueMinderGroup group)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateQueueMinderGroup_v1(
                group.QueueMinderGroupId,
                group.QueueMinderGroupName,
                group.EnabledFlag,
                group.EnabledDate,
                group.ExpiryDate,
                group.InheritFromParentFlag,
                group.TripTypeId,
                group.HierarchyType,
                group.HierarchyCode,
                group.TravelerTypeGuid,
                group.ClientSubUnitGuid,
                group.SourceSystemCode,
				group.MeetingID,
                group.IsMultipleHierarchy,
                adminUserGuid,
                group.VersionNumber
            );
        }

        //Change the deleted status on an item
        public void UpdateGroupDeletedStatus(QueueMinderGroup group)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateQueueMinderGroupDeletedStatus_v1(
                group.QueueMinderGroupId,
                group.DeletedFlag,
                adminUserGuid,
                group.VersionNumber
                );
        }

		//Change the deleted status on an item
		public void UpdateLinkedClientSubUnit(int queueMinderGroupId, string clientSubUnitGuid)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdateQueueMinderGroupLinkedClientSubUnit_v1(
					queueMinderGroupId,
					clientSubUnitGuid,
					adminUserGuid
					);

		}

		//Get one ClientSubUnits Linked to a QueueMinderGroup
		public List<ClientSubUnitCountryVM> GetLinkedClientSubUnits(int queueMinderGroupId, bool linked)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			var result = from n in db.spDesktopDataAdmin_SelectQueueMinderGroupLinkedClientSubUnits_v1(queueMinderGroupId, adminUserGuid, linked)
						 select new ClientSubUnitCountryVM
						 {
							 ClientSubUnitName = n.ClientSubUnitName.Trim(),
							 ClientSubUnitGuid = n.ClientSubUnitGuid,
							 CountryName = n.CountryName,
							 HasWriteAccess = (bool)n.HasWriteAccess,
                             IsClientExpiredFlag = n.IsClientExpiredFlag
						 };
			return result.ToList();
		}
	}
}

