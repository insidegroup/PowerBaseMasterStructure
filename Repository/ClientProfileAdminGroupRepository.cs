using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace CWTDesktopDatabase.Repository
{
	public class ClientProfileAdminGroupRepository
	{
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//Get a Page of Client Profile Groups
		public CWTPaginatedList<spDesktopDataAdmin_SelectClientProfileAdminGroups_v1Result> PageClientProfileAdminGroups(bool deleted, int page, string filter, string sortField, int sortOrder)
		{
			//get a page of records
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectClientProfileAdminGroups_v1(deleted, filter, sortField, sortOrder, page, adminUserGuid).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}

			//put into page object
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectClientProfileAdminGroups_v1Result>(result, page, totalRecords);

			//return to user
			return paginatedView;
		}

		
		//Add Group
		public void Add(ClientProfileAdminGroup group)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_InsertClientProfileAdminGroup_v1(
				group.ClientProfileGroupName,
				group.HierarchyType,
				group.HierarchyCode,
				group.GDSCode,
				group.BackOfficeSytemId,
				true,
				adminUserGuid
			);
		}

		//Get one ClientProfileAdminGroup
		public ClientProfileAdminGroup GetGroup(int id)
		{
			return db.ClientProfileAdminGroups.SingleOrDefault(c => c.ClientProfileAdminGroupId == id);
		}

		//Add Data From Linked Tables for Display
		public void EditGroupForDisplay(ClientProfileAdminGroup clientProfileAdminGroup)
		{
			HierarchyRepository hierarchyRepository = new HierarchyRepository();

			List<fnDesktopDataAdmin_SelectClientProfileAdminGroupHierarchy_v1Result> hierarchy = new List<fnDesktopDataAdmin_SelectClientProfileAdminGroupHierarchy_v1Result>();
			hierarchy = GetGroupHierarchy(clientProfileAdminGroup.ClientProfileAdminGroupId);
			if (hierarchy.Count > 0)
			{
				if (hierarchy.Count == 1)
				{
					clientProfileAdminGroup.HierarchyCode = hierarchy[0].HierarchyCode;
					clientProfileAdminGroup.HierarchyItem = hierarchy[0].HierarchyName.Trim();
					clientProfileAdminGroup.HierarchyType = hierarchy[0].HierarchyType;

				}
			}
		}
		
		//Get Hierarchy Details
		public List<fnDesktopDataAdmin_SelectClientProfileAdminGroupHierarchy_v1Result> GetGroupHierarchy(int id)
		{
			var result = db.fnDesktopDataAdmin_SelectClientProfileAdminGroupHierarchy_v1(id);
			return result.ToList();
		}

		//Change the deleted status on an item
		public void UpdateGroupDeletedStatus(ClientProfileAdminGroup group)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdateClientProfileAdminGroupDeletedStatus_v1(
					group.ClientProfileAdminGroupId,
					group.DeletedFlag,
					adminUserGuid,
					group.VersionNumber
					);

		}
	}
}