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
    public class ClientFeeGroupRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get a Page of ClientFeeGroups - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientFeeGroups_v1Result> PageClientFeeGroups(int feeTypeId, bool deleted, int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectClientFeeGroups_v1(deleted, feeTypeId,  filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectClientFeeGroups_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get a Page of PublicHolidayGroups (Orphaned) - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientFeeGroupsOrphaned_v1Result> PageOrphanedClientFeeGroups(int feeTypeId, int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectClientFeeGroupsOrphaned_v1(feeTypeId, filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectClientFeeGroupsOrphaned_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }


        //Get Hierarchies linked to a ClientFeeGroup at one level or all levels
        public List<spDesktopDataAdmin_SelectClientFeeGroupLinkedHierarchies_v1Result> ClientFeeGroupLinkedHierarchies(int clientFeeGroupId, string filterHierarchyType)
        {
            return db.spDesktopDataAdmin_SelectClientFeeGroupLinkedHierarchies_v1(clientFeeGroupId, filterHierarchyType).ToList();

        }

        //Get A Count of all Hierarchies linked to a ClientFeeGroup
        public int CountClientFeeGroupLinkedHierarchies(int clientFeeGroupId)
        {
            return db.spDesktopDataAdmin_SelectClientFeeGroupLinkedHierarchies_v1(clientFeeGroupId, "Multiple").Count();
        }

        //Get Hierarchies linked to of ClientFeeGroups
        public List<spDesktopDataAdmin_SelectClientFeeGroupAvailableHierarchies_v1Result> ClientFeeGroupAvailableHierarchies(int clientFeeGroupId, string hierarchyType, string filter)
        {
            //get a page of records
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            return db.spDesktopDataAdmin_SelectClientFeeGroupAvailableHierarchies_v1(clientFeeGroupId, hierarchyType, filter, adminUserGuid).ToList();

        }

        //Get one ClientFeeGroup
        public ClientFeeGroup GetGroup(int id)
        {
            return db.ClientFeeGroups.SingleOrDefault(c => c.ClientFeeGroupId == id);
        }

        public string FeeTypeDisplayName(int feeTypeId)
        {

            if (feeTypeId == 1)
            {
                return "Supplemental Fee Group";
            }
            else if (feeTypeId == 2)
            {
                return "Transaction Fee Group";
            }
            else if (feeTypeId == 3)
            {
                return "Mid Office Transaction Fee Group";
            }
            else
            {
                return "Mid Office Merchant Fee Group";
            }
        }

        public string FeeTypeDisplayNameShort(int feeTypeId)
        {

            if (feeTypeId == 1)
            {
                return "Supplemental Fee Group";
            }
            else if (feeTypeId == 2)
            {
                return "Transaction Fee Group";
            }
            else if (feeTypeId == 3)
            {
                return "MO Transaction Fee Group";
            }
            else
            {
                return "MO Merchant Fee Group";
            }
        }

        //Add Data From Linked Tables for Display
        public void EditGroupForDisplay(ClientFeeGroup group)
        {
            TripTypeRepository tripTypeRepository = new TripTypeRepository();
            TripType tripType = new TripType();
            tripType = tripTypeRepository.GetTripType(group.TripTypeId);
            if (tripType != null)
            {
                group.TripType = tripType.TripTypeDescription;
            }

            group.ClientFeeGroupName = Regex.Replace(group.ClientFeeGroupName, @"[^\w\-()*]", "-");

            HierarchyRepository hierarchyRepository = new HierarchyRepository();

            List<fnDesktopDataAdmin_SelectClientFeeGroupHierarchy_v1Result> hierarchy = GetGroupHierarchy(group.ClientFeeGroupId);
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
                    foreach (fnDesktopDataAdmin_SelectClientFeeGroupHierarchy_v1Result item in hierarchy)
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

					

                    group.MultipleHierarchies = hierarchyRepository.GetMultipleHierarchies(multipleHierarchies);
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
        public List<fnDesktopDataAdmin_SelectClientFeeGroupHierarchy_v1Result> GetGroupHierarchy(int id)
        {
            var result = db.fnDesktopDataAdmin_SelectClientFeeGroupHierarchy_v1(id);
            return result.ToList();
        }

        //Edit Group
        public void Edit(ClientFeeGroup group)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateClientFeeGroup_v1(

                group.ClientFeeGroupId,
                group.ClientFeeGroupName,
                group.DisplayName,
                group.EnabledFlag,
                group.EnabledDate,
                group.ExpiryDate,
                group.TripTypeId,
                group.Mandatory,
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
        public void Add(ClientFeeGroup group)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertClientFeeGroup_v1(
                group.ClientFeeGroupName,
                group.FeeTypeId,
                group.DisplayName,
                group.EnabledFlag,
                group.EnabledDate,
                group.ExpiryDate,
                group.TripTypeId,
                group.Mandatory,
                group.HierarchyType,
                group.HierarchyCode,
                group.TravelerTypeGuid,
                group.ClientSubUnitGuid,
                group.SourceSystemCode,
                adminUserGuid

            );
        }

        //Change the deleted status on an item
        public void UpdateGroupDeletedStatus(ClientFeeGroup group)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateClientFeeGroupDeletedStatus_v1(
                    group.ClientFeeGroupId,
                    group.DeletedFlag,
                    adminUserGuid,
                    group.VersionNumber
                    );

        }

        //Change the deleted status on an item
        public void UpdateLinkedHierarchy(GroupHierarchyVM groupHierarchyVM)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateClientFeeGroupLinkedHierarchy_v1(
                    groupHierarchyVM.GroupId,
                    groupHierarchyVM.HierarchyType,
                    groupHierarchyVM.HierarchyCode,
                    groupHierarchyVM.TravelerTypeGuid,
                    groupHierarchyVM.ClientSubUnitGuid,
                    groupHierarchyVM.SourceSystemCode,
                    adminUserGuid
                    );

        }

        public string getHierarchyType(string filterHierarchySearchProperty)
        {
            switch (filterHierarchySearchProperty)
            {
                case "ClientTopUnitName":
                    return "ClientTopUnit";
                case "ClientTopUnitGuid":
                    return "ClientTopUnit";
                case "ClientSubUnitName":
                    return "ClientSubUnit";
                case "ClientSubUnitGuid":
                    return "ClientSubUnit";
                case "ClientAccountName":
                    return "ClientAccount";
                case "ClientAccountNumber":
                    return "ClientAccount";
                case "CountryName":
                    return "Country";
                case "CountryCode":
                    return "Country";
                case "LocationName":
                    return "Location";
                default:
                    return "Multiple";
            }
        }
    }
}


