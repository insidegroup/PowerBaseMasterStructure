using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;

namespace CWTDesktopDatabase.Repository
{
	public class OptionalFieldGroupRepository
	{
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//Get a Page of OptionalFieldGroups
		public CWTPaginatedList<spDesktopDataAdmin_SelectOptionalFieldGroups_v1Result> PageOptionalFieldGroups(bool deleted, int page, string filter, string sortField, int sortOrder)
		{
			//get a page of records
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectOptionalFieldGroups_v1(deleted, filter, sortField, sortOrder, page, adminUserGuid).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}

			//put into page object
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectOptionalFieldGroups_v1Result>(result, page, totalRecords);

			//return to user
			return paginatedView;
		}

		//Get a Page of OptionalFieldGroups (Orphaned) - for Page Listings
		public CWTPaginatedList<spDesktopDataAdmin_SelectOptionalFieldGroupsOrphaned_v1Result> PageOrphanedOptionalFieldGroups(int page, string filter, string sortField, int sortOrder)
		{
			//get a page of records
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectOptionalFieldGroupsOrphaned_v1(filter, sortField, sortOrder, page, adminUserGuid).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}

			//put into page object
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectOptionalFieldGroupsOrphaned_v1Result>(result, page, totalRecords);

			//return to user
			return paginatedView;
		}

		//Get one OptionalFieldGroup
		public OptionalFieldGroup GetGroup(int id)
		{
			return db.OptionalFieldGroups.SingleOrDefault(c => c.OptionalFieldGroupId == id);
		}
		
		//Add Data From Linked Tables for Display
		public void EditGroupForDisplay(OptionalFieldGroup group)
		{
			HierarchyRepository hierarchyRepository = new HierarchyRepository();

			List<fnDesktopDataAdmin_SelectOptionalFieldGroupHierarchy_v1Result> hierarchy = new List<fnDesktopDataAdmin_SelectOptionalFieldGroupHierarchy_v1Result>();
			hierarchy = GetGroupHierarchy(group.OptionalFieldGroupId);
			group.OptionalFieldGroupName = Regex.Replace(group.OptionalFieldGroupName, @"[^\w\-()*]", "-");

			if (hierarchy.Count > 0)
			{
				if (hierarchy.Count == 1)
				{
					group.HierarchyCode = hierarchy[0].HierarchyCode;
					group.HierarchyItem = hierarchy[0].HierarchyName.Trim();
					group.HierarchyType = hierarchy[0].HierarchyType;

				}
				else
				{
					group.HierarchyCode = "Multiple"; //Placeholder
					group.HierarchyItem = "Multiple"; //Placeholder
					group.HierarchyType = "Multiple";
				}

			}
			group.IsMultipleHierarchy = (hierarchy.Count > 1);
		}

		//Get Hierarchy Details
		public List<fnDesktopDataAdmin_SelectOptionalFieldGroupHierarchy_v1Result> GetGroupHierarchy(int id)
		{
			var result = db.fnDesktopDataAdmin_SelectOptionalFieldGroupHierarchy_v1(id);
			return result.ToList();
		}

		//Edit Group
		public void Edit(OptionalFieldGroup group)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdateOptionalFieldGroup_v1(
				group.OptionalFieldGroupId,
				group.OptionalFieldGroupName,
				group.EnabledFlag,
				group.EnabledDate,
				group.ExpiryDate,
				group.HierarchyType,
				group.HierarchyCode,
				adminUserGuid,
				group.VersionNumber
			);
		}

		//Add Group
		public void Add(OptionalFieldGroup optionalFieldGroup)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_InsertOptionalFieldGroup_v1(
				optionalFieldGroup.OptionalFieldGroupName,
				optionalFieldGroup.EnabledFlag,
				optionalFieldGroup.EnabledDate,
				optionalFieldGroup.ExpiryDate,
				optionalFieldGroup.HierarchyType,
				optionalFieldGroup.HierarchyCode,
				adminUserGuid
			);
		}

		
		//Change the deleted status on an item
		public void UpdateGroupDeletedStatus(OptionalFieldGroup group)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdateOptionalFieldGroupDeletedStatus_v1(
					group.OptionalFieldGroupId,
					group.DeletedFlag,
					adminUserGuid,
					group.VersionNumber
					);

		}

		//Change the deleted status on an item
		public void UpdateLinkedHierarchy(GroupHierarchyVM groupHierarchyVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdateOptionalFieldGroupLinkedHierarchy_v1(
					groupHierarchyVM.GroupId,
					groupHierarchyVM.HierarchyType,
					groupHierarchyVM.HierarchyCode,
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


