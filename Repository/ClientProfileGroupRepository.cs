using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.Repository
{
	public class ClientProfileGroupRepository
	{
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//Get a Page of Client Profile Groups
		public CWTPaginatedList<spDesktopDataAdmin_SelectClientProfileGroups_v1Result> PageClientProfileGroups(bool deleted, int page, string filter, string sortField, int sortOrder)
		{
			//get a page of records
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectClientProfileGroups_v1(deleted, filter, sortField, sortOrder, page, adminUserGuid).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}

			//put into page object
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectClientProfileGroups_v1Result>(result, page, totalRecords);

			//return to user
			return paginatedView;
		}

		//Add Group
		public void Add(ClientProfileGroup group)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_InsertClientProfileGroup_v1(
				group.ClientProfileGroupName,
				group.HierarchyType,
				group.HierarchyCode,
				group.GDSCode,
				group.BackOfficeSytemId,
				group.PseudoCityOrOfficeId,
				group.UniqueName,
				adminUserGuid
			);
		}

		//Edit Group
		//Can only edit Profile Name
		public void Edit(ClientProfileGroup group)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdateClientProfileGroup_v1(
				group.ClientProfileGroupId,
				group.ClientProfileGroupName,
				group.UniqueName,
				adminUserGuid,
				group.VersionNumber
			);
		}

		//Get one ClientProfileGroup
		public ClientProfileGroup GetGroup(int id)
		{
			return db.ClientProfileGroups.SingleOrDefault(c => c.ClientProfileGroupId == id);
		}

		//Add Data From Linked Tables for Display
		public void EditGroupForDisplay(ClientProfileGroup clientProfileGroup)
		{
			//Set Hierarchy if ClientTopUnit
			ClientProfileGroupClientTopUnit clientProfileGroupClientTopUnit = clientProfileGroup.ClientProfileGroupClientTopUnits.SingleOrDefault();
			if (clientProfileGroupClientTopUnit != null)
			{
				ClientTopUnitRepository clientTopUnitRepository = new ClientTopUnitRepository();
				ClientTopUnit clientTopUnit = new ClientTopUnit();
				clientTopUnit = clientTopUnitRepository.GetClientTopUnit(clientProfileGroupClientTopUnit.ClientTopUnitGuid);
				if (clientTopUnit != null)
				{
					clientProfileGroup.HierarchyItem = clientTopUnit.ClientTopUnitName;
					clientProfileGroup.HierarchyCode = clientTopUnit.ClientTopUnitGuid;
					clientProfileGroup.HierarchyType = "ClientTopUnit";
				}
			}

			//Set Hierarchy if ClientSubUnit
			ClientProfileGroupClientSubUnit clientProfileGroupClientSubUnit = clientProfileGroup.ClientProfileGroupClientSubUnits.SingleOrDefault();
			if (clientProfileGroupClientSubUnit != null)
			{
				ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
				ClientSubUnit clientSubUnit = new ClientSubUnit();
				clientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientProfileGroupClientSubUnit.ClientSubUnitGuid);
				if (clientSubUnit != null)
				{
					clientProfileGroup.HierarchyItem = clientSubUnit.ClientSubUnitName;
					clientProfileGroup.HierarchyCode = clientSubUnit.ClientSubUnitGuid;
					clientProfileGroup.HierarchyType = "ClientSubUnit";
				}
			}
		}

		//Change the deleted status on an item
		public void UpdateGroupDeletedStatus(ClientProfileGroup group)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdateClientProfileGroupDeletedStatus_v1(
					group.ClientProfileGroupId,
					group.DeletedFlag,
					adminUserGuid,
					group.VersionNumber
					);

		}

		//Update the publish date on an item
		public void UpdateGroupPublishDate(ClientProfileGroup group)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdateClientProfilePublishDate_v1(
					group.ClientProfileGroupId,
					adminUserGuid,
					group.VersionNumber
					);

		}
	}
}