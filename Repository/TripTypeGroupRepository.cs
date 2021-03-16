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
    public class TripTypeGroupRepository
    {
        private TripTypeGroupDC db = new TripTypeGroupDC(Settings.getConnectionString());

        //Get a Page of TripTypeGroups - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectTripTypeGroups_v1Result> PageTripTypeGroups(bool deleted, int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectTripTypeGroups_v1(deleted, filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectTripTypeGroups_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get a Page of TripTypeGroups (Orphaned) - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectTripTypeGroupsOrphaned_v1Result> PageOrphanedTripTypeGroups(int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectTripTypeGroupsOrphaned_v1(filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectTripTypeGroupsOrphaned_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }


        //Get one TripTypeGroup
        public TripTypeGroup GetGroup(int id)
        {
            return db.TripTypeGroups.SingleOrDefault(c => c.TripTypeGroupId == id);
        }

        //Add Data From Linked Tables for Display
        public void EditGroupForDisplay(TripTypeGroup group)
        {
            group.TripTypeGroupName = Regex.Replace(group.TripTypeGroupName, @"[^\w\-()*]", "-");

            //Hierarchy
            HierarchyRepository hierarchyRepository = new HierarchyRepository();
            ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();

            List<fnDesktopDataAdmin_SelectTripTypeGroupHierarchy_v1Result> hierarchy = GetGroupHierarchy(group.TripTypeGroupId);
            if (hierarchy.Count > 0)
            {
                if (hierarchy.Count == 1)
                {
                    group.HierarchyCode = hierarchy[0].HierarchyCode.ToString();
                    group.HierarchyItem = hierarchy[0].HierarchyName.Trim();
                    group.HierarchyType = hierarchy[0].HierarchyType;

                    if (hierarchy[0].HierarchyType == "ClientSubUnitTravelerType")
                    {
                        group.ClientSubUnitGuid = hierarchy[0].HierarchyCode.ToString();
                        group.ClientSubUnitName = hierarchy[0].HierarchyName.Trim();
                        group.ClientTopUnitName = clientSubUnitRepository.GetClientSubUnitClientTopUnitName(group.ClientSubUnitGuid);
                        group.TravelerTypeGuid = hierarchy[0].TravelerTypeGuid;
                        group.TravelerTypeName = hierarchy[0].TravelerTypeName.Trim();
                    }

                    if (hierarchy[0].HierarchyType == "ClientSubUnit")
                    {
                        group.ClientTopUnitName = clientSubUnitRepository.GetClientSubUnitClientTopUnitName(hierarchy[0].HierarchyCode);
                    }

                    if (hierarchy[0].HierarchyType == "ClientAccount")
                    {
                        group.SourceSystemCode = hierarchy[0].SourceSystemCode;
                    }
                }
                else
                {
                    List<MultipleHierarchyDefinition> multipleHierarchies = new List<MultipleHierarchyDefinition>();
                    foreach (fnDesktopDataAdmin_SelectTripTypeGroupHierarchy_v1Result item in hierarchy)
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
        public List<fnDesktopDataAdmin_SelectTripTypeGroupHierarchy_v1Result> GetGroupHierarchy(int id)
        {
            var result = db.fnDesktopDataAdmin_SelectTripTypeGroupHierarchy_v1(id);
            return result.ToList();
        }

        //Edit Group
        public void Edit(TripTypeGroup group)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateTripTypeGroup_v1(
                group.TripTypeGroupId,
                group.TripTypeGroupName,
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
        public void Add(TripTypeGroup group)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertTripTypeGroup_v1(
                group.TripTypeGroupName,
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

        //Change the deleted status on an item
        public void UpdateGroupDeletedStatus(TripTypeGroup group)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateTripTypeGroupDeletedStatus_v1(
                    group.TripTypeGroupId,
                    group.DeletedFlag,
                    adminUserGuid,
                    group.VersionNumber
                    );

        }

		//REMOVED: List of All Items - Sortable
		/*public IQueryable<fnDesktopDataAdmin_SelectTripTypeGroups_v1Result> GetGroupsByDeletedFlag(string filter, bool deleted, string sortField, int sortOrder)
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
                return db.fnDesktopDataAdmin_SelectTripTypeGroups_v1(deleted, adminUserGuid).OrderBy(sortField);
            }
            else
            {
                return db.fnDesktopDataAdmin_SelectTripTypeGroups_v1(deleted, adminUserGuid).OrderBy(sortField).Where(c => c.TripTypeGroupName.Contains(filter));
            }
        }*/

		//Change the deleted status on an item
		public void UpdateLinkedClientSubUnit(int tripTypeGroupId, string clientSubUnitGuid)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdateTripTypeGroupLinkedClientSubUnit_v1(
					tripTypeGroupId,
					clientSubUnitGuid,
					adminUserGuid
					);

		}

		//Get one ClientSubUnits Linked to a TripTypeGroup
		public List<ClientSubUnitCountryVM> GetLinkedClientSubUnits(int tripTypeGroupId, bool linked)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			var result = from n in db.spDesktopDataAdmin_SelectTripTypeGroupLinkedClientSubUnits_v1(tripTypeGroupId, adminUserGuid, linked)
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

