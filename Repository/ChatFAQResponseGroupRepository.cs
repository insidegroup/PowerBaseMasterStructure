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
    public class ChatFAQResponseGroupRepository
    {
        private ChatFAQResponseGroupDC db = new ChatFAQResponseGroupDC(Settings.getConnectionString());

        //Get a Page of Chat FAQ Response Groups - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectChatFAQResponseGroups_v1Result> PageChatFAQResponseGroups(bool deleted, int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectChatFAQResponseGroups_v1(deleted, filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectChatFAQResponseGroups_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get one Chat FAQ Response Group
        public ChatFAQResponseGroup GetGroup(int id)
        {
            return db.ChatFAQResponseGroups.SingleOrDefault(c => c.ChatFAQResponseGroupId == id);
        }

        //Get ClientSubUnits Linked to a Chat FAQ Response Group
        public List<ChatFAQResponseGroupClientSubUnitCountryVM> GetLinkedClientSubUnits(int chatFAQResponseGroupid, bool linked)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            var result = from n in db.spDesktopDataAdmin_SelectChatFAQResponseGroupLinkedClientSubUnits_v1(chatFAQResponseGroupid, adminUserGuid, linked)
                         select new ChatFAQResponseGroupClientSubUnitCountryVM
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
        public void UpdateLinkedClientSubUnit(int chatFAQResponseGroupId, string clientSubUnitGuid)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateChatFAQResponseGroupLinkedClientSubUnit_v1(
                    chatFAQResponseGroupId,
                    clientSubUnitGuid,
                    adminUserGuid
                    );

        }

        //Add Data From Linked Tables for Display
        public void EditGroupForDisplay(ChatFAQResponseGroup group)
        {
            group.ChatFAQResponseGroupName = Regex.Replace(group.ChatFAQResponseGroupName, @"[^\w\-()*]", "-");

            //Hierarchy
            HierarchyRepository hierarchyRepository = new HierarchyRepository();

            List<fnDesktopDataAdmin_SelectChatFAQResponseGroupHierarchy_v1Result> hierarchy = GetGroupHierarchy(group.ChatFAQResponseGroupId);
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
                    foreach (fnDesktopDataAdmin_SelectChatFAQResponseGroupHierarchy_v1Result item in hierarchy)
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
        public List<fnDesktopDataAdmin_SelectChatFAQResponseGroupHierarchy_v1Result> GetGroupHierarchy(int id)
        {
            var result = db.fnDesktopDataAdmin_SelectChatFAQResponseGroupHierarchy_v1(id);
            return result.ToList();
        }

        //Change the deleted status on an item
        public void UpdateGroupDeletedStatus(ChatFAQResponseGroup group)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateChatFAQResponseGroupDeletedStatus_v1(
                    group.ChatFAQResponseGroupId,
                    group.DeletedFlag,
                    adminUserGuid,
                    group.VersionNumber
                    );

        }

        //Edit Group
        public void Edit(ChatFAQResponseGroup group)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateChatFAQResponseGroup_v1(
                adminUserGuid,
                group.ChatFAQResponseGroupId,
                group.ChatFAQResponseGroupName,
                group.EnabledFlag,
                group.EnabledDate,
                group.ExpiryDate,
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
        public void Add(ChatFAQResponseGroup group)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertChatFAQResponseGroup_v1(
                adminUserGuid,
                group.ChatFAQResponseGroupName,
                group.EnabledFlag,
                group.EnabledDate,
                group.ExpiryDate,
                group.HierarchyType,
                group.HierarchyCode,
                group.TravelerTypeGuid,
                group.ClientSubUnitGuid,
                group.SourceSystemCode,
                adminUserGuid
            );
        }
    }
}
