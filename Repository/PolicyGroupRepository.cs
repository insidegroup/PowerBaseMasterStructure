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
    public class PolicyGroupRepository
    {
        private PolicyGroupDC db = new PolicyGroupDC(Settings.getConnectionString());

        //Get a Page of PolicyGroups - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyGroups_v1Result> PagePolicyGroups(bool deleted, int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectPolicyGroups_v1(deleted, filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPolicyGroups_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get a Page of PolicyGroups (Orphaned) - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyGroupsOrphaned_v1Result> PageOrphanedPolicyGroups(int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectPolicyGroupsOrphaned_v1(filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPolicyGroupsOrphaned_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get Policy Group SubMenu Counts
        public PolicyGroupSubMenuVM CreateSubMenuViewModel(int policyGroupId)
        {
            //get a page of records
            var result = from n in db.spDesktopDataAdmin_SelectPolicyGroupSubMenuCounts_v1(policyGroupId)
                         select new PolicyGroupSubMenuVM
                         {
							PolicyAirCabinGroupItemCount = (int)n.PolicyAirCabinGroupItemCount,
							PolicyAirVendorGroupItemCount = (int)n.PolicyAirVendorGroupItemCount,
							PolicyAirMissedSavingsThresholdGroupItemCount = (int)n.PolicyAirMissedSavingsThresholdGroupItemCount,
							PolicyCarTypeGroupItemCount = (int)n.PolicyCarTypeGroupItemCount,
							PolicyCarVendorGroupItemCount = (int)n.PolicyCarVendorGroupItemCount,
							PolicyCityGroupItemCount = (int)n.PolicyCityGroupItemCount,
							PolicyCountryGroupItemCount = (int)n.PolicyCountryGroupItemCount,
							PolicyHotelCapRateGroupItemCount = (int)n.PolicyHotelCapRateGroupItemCount,
							PolicyHotelPropertyItemCount = (int)n.PolicyHotelPropertyItemCount,
							PolicyHotelVendorGroupItemCount = (int)n.PolicyHotelVendorGroupItemCount,
							PolicySupplierDealCodeCount = (int)n.PolicySupplierDealCodeCount,
							PolicyMessageGroupItemCount = (int)n.PolicyMessageGroupItemCount,
							PolicySupplierServiceInformationCount = (int)n.PolicySupplierServiceInformationCount,

							Policy24HSCOtherGroupItemCount = (int)n.Policy24HSCOtherGroupItemCount,
							PolicyAirOtherGroupItemCount = (int)n.PolicyAirOtherGroupItemCount,
							PolicyAllOtherGroupItemCount = (int)n.PolicyAllOtherGroupItemCount,
							PolicyCarOtherGroupItemCount = (int)n.PolicyCarOtherGroupItemCount,
							PolicyHotelOtherGroupItemCount = (int)n.PolicyHotelOtherGroupItemCount,
							PolicyOtherGroupItemCount = (int)n.PolicyOtherGroupItemCount
						};


            //return to user
            return result.FirstOrDefault();
        }

		//Get Policy Group SubMenu Counts
		public List<PolicyType> GetPolicyTypes(int policyId)
        {
            //get a page of records
			var result = from n in db.spDesktopDataAdmin_SelectPolicyTypes_v1(policyId)
						 select new PolicyType
						 {
							 PolicyTypeTableName = n.PolicyTypeTableName,
							 NavigationLinkPolicyTypeName = n.NavigationLinkPolicyTypeName,
							 ItemCount = n.ItemCount
						 };

			return result.ToList();
        }

        //Get one PolicyGroup
        public PolicyGroup GetGroup(int policyGroupId)
        {
            return db.PolicyGroups.SingleOrDefault(c => c.PolicyGroupId == policyGroupId);
        }

        //Add Data From Linked Tables for Display
        public void EditGroupForDisplay(PolicyGroup group)
        {
			//Trip Types
			TripTypeRepository tripTypeRepository = new TripTypeRepository();
            TripType tripType = new TripType();
            tripType = tripTypeRepository.GetTripType(group.TripTypeId);
            if (tripType != null)
            {
                group.TripType = tripType.TripTypeDescription;
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

            group.PolicyGroupName = Regex.Replace(group.PolicyGroupName, @"[^\w\-()*]", "-");

            //Hierarchy
            HierarchyRepository hierarchyRepository = new HierarchyRepository();

            List<fnDesktopDataAdmin_SelectPolicyGroupHierarchy_v1Result> hierarchy = GetGroupHierarchy(group.PolicyGroupId);
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
                    foreach (fnDesktopDataAdmin_SelectPolicyGroupHierarchy_v1Result item in hierarchy)
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
        public List<fnDesktopDataAdmin_SelectPolicyGroupHierarchy_v1Result> GetGroupHierarchy(int id)
        {
            var result = db.fnDesktopDataAdmin_SelectPolicyGroupHierarchy_v1(id);
            return result.ToList();
        }

        //Edit Group
        public void Edit(PolicyGroup group)
        {

            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdatePolicyGroup_v1(

                group.PolicyGroupId,
                group.PolicyGroupName,
                group.EnabledFlag,
                group.EnabledDate,
                group.ExpiryDate,
                group.InheritFromParentFlag,
                group.TripTypeId,
				group.MeetingID,
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
        public void Add(PolicyGroup group)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertPolicyGroup_v1(
                group.PolicyGroupName,
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

        //Change the deleted status on an item
        public void UpdateGroupDeletedStatus(PolicyGroup group)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdatePolicyGroupDeletedStatus_v1(
                group.PolicyGroupId,
                group.DeletedFlag,
                adminUserGuid,
                group.VersionNumber
                );
        }

        //Change the deleted status on an item
        public void UpdateLinkedClientSubUnit(int policyGroupId, string clientSubUnitGuid)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdatePolicyGroupLinkedClientSubUnit_v1(
                    policyGroupId,
                    clientSubUnitGuid,
                    adminUserGuid
                    );

        }

        //Get one ClientSubUnits Linked to a PolicyGroup
        public List<ClientSubUnitCountryVM> GetLinkedClientSubUnits(int policyGroupId, bool linked)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            var result = from n in db.spDesktopDataAdmin_SelectPolicyGroupLinkedClientSubUnits_v1(policyGroupId, adminUserGuid, linked)
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

