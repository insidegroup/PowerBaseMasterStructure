using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Linq.Dynamic;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using CWTDesktopDatabase.ViewModels;

namespace CWTDesktopDatabase.Repository
{
    public class PNROutputGroupRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get a Page of PNROutputGroups - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectPNROutputGroups_v1Result> PagePNROutputGroups(bool deleted, int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectPNROutputGroups_v1(deleted, filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPNROutputGroups_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get a Page of PNROutputGroups (Orphaned) - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectPNROutputGroupsOrphaned_v1Result> PageOrphanedPNROutputGroups(int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectPNROutputGroupsOrphaned_v1(filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPNROutputGroupsOrphaned_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }


        //Get one PNROutputGroup
        public PNROutputGroup GetGroup(int id)
        {
            return db.PNROutputGroups.SingleOrDefault(c => c.PNROutputGroupId == id);
        }

        //Add Data From Linked Tables for Display
        public void EditGroupForDisplay(PNROutputGroup group)
        {
           // TripTypeRepository tripTypeRepository = new TripTypeRepository();
            //TripType tripType = new TripType();
            //tripType = tripTypeRepository.GetTripType(group.TripTypeId);
            //if (tripType != null)
           // {
           //     group.TripType = tripType.TripTypeDescription;
           // }

            GDSRepository gDSRepository = new GDSRepository();
            GDS gds = new GDS();
            gds = gDSRepository.GetGDS(group.GDSCode);
            if (gds != null)
            {
                group.GDSName = gds.GDSName;
            }

            PNROutputTypeRepository pNROutputTypeRepository = new PNROutputTypeRepository();
            PNROutputType pNROutputType = new PNROutputType();
            pNROutputType = pNROutputTypeRepository.GetPNROutputType(group.PNROutputTypeID);
            if (pNROutputType != null)
            {
                group.PNROutputTypeName = pNROutputType.PNROutputTypeName;
            }


            group.PNROutputGroupName = Regex.Replace(group.PNROutputGroupName, @"[^\w\-()*]", "-");

            HierarchyRepository hierarchyRepository = new HierarchyRepository();

            List<fnDesktopDataAdmin_SelectPNROutputGroupHierarchy_v1Result> hierarchy = GetGroupHierarchy(group.PNROutputGroupId);
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
                    foreach (fnDesktopDataAdmin_SelectPNROutputGroupHierarchy_v1Result item in hierarchy)
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

        //Change the deleted status on an item
        public void UpdateLinkedHierarchy(PNROutputGroupHierarchyVM groupHierarchyVM)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdatePNROutputGroupLinkedHierarchy_v1(
                    groupHierarchyVM.GroupId,
                    groupHierarchyVM.HierarchyType,
                    groupHierarchyVM.HierarchyCode,
                    groupHierarchyVM.TravelerTypeGuid,
                    groupHierarchyVM.ClientSubUnitGuid,
                    groupHierarchyVM.SourceSystemCode,
                    adminUserGuid
                );
        }

        //Get Hierarchy Details
        public List<fnDesktopDataAdmin_SelectPNROutputGroupHierarchy_v1Result> GetGroupHierarchy(int id)
        {
            var result = db.fnDesktopDataAdmin_SelectPNROutputGroupHierarchy_v1(id);
            return result.ToList();
        }

        //Get Hierarchies linked to a PNROutputGroup at one level or all levels
        public List<spDesktopDataAdmin_SelectPNROutputGroupLinkedHierarchies_v1Result> PNROutputGroupLinkedHierarchies(int pnrOutputGroupId, string filterHierarchyType)
        {
            return db.spDesktopDataAdmin_SelectPNROutputGroupLinkedHierarchies_v1(pnrOutputGroupId, filterHierarchyType).ToList();
        }

        //Get A Count of all Hierarchies linked to a PNROutputGroup
        public int CountPNROutputGroupLinkedHierarchies(int clientDefinedRuleGroupId)
        {
            return db.spDesktopDataAdmin_SelectPNROutputGroupLinkedHierarchies_v1(clientDefinedRuleGroupId, "Multiple").Count();
        }

        //Get Hierarchies linked to of PNROutputGroups
        public List<spDesktopDataAdmin_SelectPNROutputGroupAvailableHierarchies_v1Result> PNROutputGroupAvailableHierarchies(int clientDefinedRuleGroupId, string hierarchyType, string filter, string clientSubUnit = "", string travelerType = "")
        {
            //get a page of records
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            return db.spDesktopDataAdmin_SelectPNROutputGroupAvailableHierarchies_v1(clientDefinedRuleGroupId, hierarchyType, filter, clientSubUnit, travelerType, adminUserGuid).ToList();
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

        public string getAvailableHierarchyTypeDisplayName(string filterHierarchySearchProperty)
        {
            switch (filterHierarchySearchProperty)
            {
                case "ClientTopUnitName":
                    return "Client TopUnits";
                case "ClientTopUnit":
                    return "Client TopUnits";
                case "ClientSubUnitName":
                    return "Client SubUnits";
                case "ClientAccountName":
                    return "Client Accounts";
                case "ClientAccountNumber":
                    return "Client Account";
                case "ClientSubUnitTravelerType":
                    return "Client SubUnit Traveler Types";
                case "Business Rules Group Name":
                    return "Business Rules Groups";
                case "Category":
                    return "Categories";
                case "TravelerType":
                    return "Traveler Types";
                default:
                    return "Items";
            }
        }

        public SelectList GetHierarchyPropertyOptions(string groupName, string selectedValue = "")
        {

            TablesDomainHierarchyLevelRepository tablesDomainHierarchyLevelRepository = new TablesDomainHierarchyLevelRepository();

            List<SelectListItem> hierarchyTypesList = tablesDomainHierarchyLevelRepository.GetDomainHierarchiesForHierarchySearch(groupName).ToList();

            SelectList hierarchyTypes = new SelectList(hierarchyTypesList, "Value", "Text", !string.IsNullOrEmpty(selectedValue) ? selectedValue : "");

            return hierarchyTypes;
        }
    }
}

