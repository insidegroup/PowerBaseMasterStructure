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
    public class TicketQueueGroupRepository
    {
        private TicketQueueGroupDC db = new TicketQueueGroupDC(Settings.getConnectionString());

        //Get a Page of TicketQueueGroups - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectTicketQueueGroups_v1Result> PageTicketQueueGroups(bool deleted, int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectTicketQueueGroups_v1(deleted, filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectTicketQueueGroups_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get a Page of TicketQueueGroups (Orphaned) - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectTicketQueueGroupsOrphaned_v1Result> PageOrphanedTicketQueueGroups(int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectTicketQueueGroupsOrphaned_v1(filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectTicketQueueGroupsOrphaned_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }


        //Get one TicketQueueGroup
        public TicketQueueGroup GetGroup(int id)
        {
            return db.TicketQueueGroups.SingleOrDefault(c => c.TicketQueueGroupId == id);
        }

        //Add Data From Linked Tables for Display
        public void EditGroupForDisplay(TicketQueueGroup group)
        {
            TripTypeRepository tripTypeRepository = new TripTypeRepository();
            TripType tripType = new TripType();
            tripType = tripTypeRepository.GetTripType(group.TripTypeId);
            if (tripType != null)
            {
                group.TripType = tripType.TripTypeDescription;
            }

            group.TicketQueueGroupName = Regex.Replace(group.TicketQueueGroupName, @"[^\w\-()*]", "-");

            //Hierarchy
            HierarchyRepository hierarchyRepository = new HierarchyRepository();

            List<fnDesktopDataAdmin_SelectTicketQueueGroupHierarchy_v1Result> hierarchy = GetGroupHierarchy(group.TicketQueueGroupId);
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
                    foreach (fnDesktopDataAdmin_SelectTicketQueueGroupHierarchy_v1Result item in hierarchy)
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
        }

        //Get Hierarchy Details
        public List<fnDesktopDataAdmin_SelectTicketQueueGroupHierarchy_v1Result> GetGroupHierarchy(int id)
        {
            var result = db.fnDesktopDataAdmin_SelectTicketQueueGroupHierarchy_v1(id);
            return result.ToList();
        }

        //Edit Group
        public void Edit(TicketQueueGroup group)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateTicketQueueGroup_v1(
                group.TicketQueueGroupId,
                group.TicketQueueGroupName,
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
                group.IsMultipleHierarchy,
                adminUserGuid,
                group.VersionNumber
            );
        }

        //Add Group
        public void Add(TicketQueueGroup group)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertTicketQueueGroup_v1(
                group.TicketQueueGroupName,
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
                adminUserGuid

            );
        }

        //Change the deleted status on an item
        public void UpdateGroupDeletedStatus(TicketQueueGroup group)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateTicketQueueGroupDeletedStatus_v1(
                group.TicketQueueGroupId,
                group.DeletedFlag,
                adminUserGuid,
                group.VersionNumber
                );
        }

		//REMOVED: List of All Items - Sortable
		/*public IQueryable<fnDesktopDataAdmin_SelectTicketQueueGroups_v1Result> GetGroupsByDeletedFlag(string filter, bool deleted, string sortField, int sortOrder)
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
                return db.fnDesktopDataAdmin_SelectTicketQueueGroups_v1(deleted,adminUserGuid).OrderBy(sortField);
            }
            else
            {
                return db.fnDesktopDataAdmin_SelectTicketQueueGroups_v1(deleted, adminUserGuid).OrderBy(sortField).Where(c => c.TicketQueueGroupName.Contains(filter));
            }
        }*/

		//Change the deleted status on an item
		public void UpdateLinkedClientSubUnit(int ticketQueueGroupId, string clientSubUnitGuid)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdateTicketQueueGroupLinkedClientSubUnit_v1(
					ticketQueueGroupId,
					clientSubUnitGuid,
					adminUserGuid
					);

		}

		//Get one ClientSubUnits Linked to a TicketQueueGroup
		public List<ClientSubUnitCountryVM> GetLinkedClientSubUnits(int ticketQueueGroupId, bool linked)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			var result = from n in db.spDesktopDataAdmin_SelectTicketQueueGroupLinkedClientSubUnits_v1(ticketQueueGroupId, adminUserGuid, linked)
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

