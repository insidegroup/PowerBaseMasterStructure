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
    public class ServicingOptionGroupRepository
    {
        private ServicingOptionGroupDC db = new ServicingOptionGroupDC(Settings.getConnectionString());

        //Get a Page of ServicingOptionGroups - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectServicingOptionGroups_v1Result> PageServicingOptionGroups(bool deleted, int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectServicingOptionGroups_v1(deleted, filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectServicingOptionGroups_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

       //Get a Page of ServicingOptionGroups (Orphaned) - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectServicingOptionGroupsOrphaned_v1Result> PageOrphanedServicingOptionGroups(int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectServicingOptionGroupsOrphaned_v1(filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectServicingOptionGroupsOrphaned_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get one ServicingOptionGroup
        public ServicingOptionGroup GetGroup(int id)
        {
            return db.ServicingOptionGroups.SingleOrDefault(c => c.ServicingOptionGroupId == id);
        }

        //Get one ClientSubUnits Linked to a ServicingOptionGroup
        public List<ClientSubUnitCountryVM> GetLinkedClientSubUnits(int servicingOptionGroupid, bool linked)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            var result = from n in db.spDesktopDataAdmin_SelectServicingOptionGroupLinkedClientSubUnits_v1(servicingOptionGroupid, adminUserGuid, linked)
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

        //Change the deleted status on an item
        public void UpdateLinkedClientSubUnit(int servicingOptionGroupId, string clientSubUnitGuid)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateServicingOptionGroupLinkedClientSubUnit_v1(
                    servicingOptionGroupId,
                    clientSubUnitGuid,
                    adminUserGuid
                    );

        }

        //Add Data From Linked Tables for Display
        public void EditGroupForDisplay(ServicingOptionGroup group)
        {

            TripTypeRepository tripTypeRepository = new TripTypeRepository();
            TripType tripType = new TripType();
            tripType = tripTypeRepository.GetTripType(group.TripTypeId);
            if (tripType != null)
            {
                group.TripType = tripType.TripTypeDescription;
            }

            group.ServicingOptionGroupName = Regex.Replace(group.ServicingOptionGroupName, @"[^\w\-()*]", "-");

            //Hierarchy
            HierarchyRepository hierarchyRepository = new HierarchyRepository();

            List<fnDesktopDataAdmin_SelectServicingOptionGroupHierarchy_v1Result> hierarchy = GetGroupHierarchy(group.ServicingOptionGroupId);
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
                    foreach (fnDesktopDataAdmin_SelectServicingOptionGroupHierarchy_v1Result item in hierarchy)
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
        public List<fnDesktopDataAdmin_SelectServicingOptionGroupHierarchy_v1Result> GetGroupHierarchy(int id)
        {
            var result = db.fnDesktopDataAdmin_SelectServicingOptionGroupHierarchy_v1(id);
            return result.ToList();
        }

        //Change the deleted status on an item
        public void UpdateGroupDeletedStatus(ServicingOptionGroup group)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateServicingOptionGroupDeletedStatus_v1(
                    group.ServicingOptionGroupId ,
                    group.DeletedFlag,
                    adminUserGuid,
                    group.VersionNumber
                    );

        }

        //Edit Group
        public void Edit(ServicingOptionGroup group)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            db.spDesktopDataAdmin_UpdateServicingOptionGroup_v1(

                adminUserGuid,
                group.ServicingOptionGroupId,
                group.ServicingOptionGroupName,
                group.EnabledFlag,
                group.EnabledDate,
                group.ExpiryDate,
                group.TripTypeId,
                group.HierarchyType,
                group.HierarchyCode,
                group.TravelerTypeGuid,
                group.ClientSubUnitGuid,
                group.SourceSystemCode,
                group.IsMultipleHierarchy,
				group.MeetingID,
                adminUserGuid,
                group.VersionNumber
            );
        }

        //Add Group
        public void Add(ServicingOptionGroup group)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertServicingOptionGroup_v1(
                group.ServicingOptionGroupName,
                group.EnabledFlag,
                group.EnabledDate,
                group.ExpiryDate,
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

 
    }
}

