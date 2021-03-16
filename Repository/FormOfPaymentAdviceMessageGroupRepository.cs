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
	public class FormOfPaymentAdviceMessageGroupRepository
	{
		private FormOfPaymentAdviceMessageGroupDC db = new FormOfPaymentAdviceMessageGroupDC(Settings.getConnectionString());

		//Get a Page of Price Tracking Handling Fee Groups - for Page Listings
		public CWTPaginatedList<spDesktopDataAdmin_SelectFormOfPaymentAdviceMessageGroups_v1Result> PageFormOfPaymentAdviceMessageGroups(bool deleted, int page, string filter, string sortField, int sortOrder)
		{
			//get a page of records
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectFormOfPaymentAdviceMessageGroups_v1(deleted, filter, sortField, sortOrder, page, adminUserGuid).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}

			//put into page object
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectFormOfPaymentAdviceMessageGroups_v1Result>(result, page, totalRecords);

			//return to user
			return paginatedView;
		}

		//Get one Price Tracking Handling Fee Group
		public FormOfPaymentAdviceMessageGroup GetGroup(int id)
		{
			return db.FormOfPaymentAdviceMessageGroups.SingleOrDefault(c => c.FormOfPaymentAdviceMessageGroupID == id);
		}

		//Change the deleted status on an item
		public void UpdateGroupDeletedStatus(FormOfPaymentAdviceMessageGroup group)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdateFormOfPaymentAdviceMessageGroupDeletedStatus_v1(
				group.FormOfPaymentAdviceMessageGroupID,
				group.DeletedFlag,
				adminUserGuid,
				group.VersionNumber
			);
		}

		//Add Data From Linked Tables for Display
		public void EditGroupForDisplay(FormOfPaymentAdviceMessageGroup group)
		{

			HierarchyRepository hierarchyRepository = new HierarchyRepository();

			List<fnDesktopDataAdmin_SelectFormOfPaymentAdviceMessageGroupHierarchy_v1Result> hierarchy = new List<fnDesktopDataAdmin_SelectFormOfPaymentAdviceMessageGroupHierarchy_v1Result>();
			hierarchy = GetGroupHierarchy(group.FormOfPaymentAdviceMessageGroupID);
			group.FormOfPaymentAdviceMessageGroupName = Regex.Replace(group.FormOfPaymentAdviceMessageGroupName, @"[^\w\-()*]", "-");

			if (hierarchy.Count > 0)
			{
				group.HierarchyType = hierarchy[0].HierarchyType;
				group.HierarchyCode = hierarchy[0].HierarchyCode.ToString();
				group.HierarchyItem = hierarchy[0].HierarchyName.Trim();

				if (hierarchy[0].HierarchyType == "ClientSubUnitTravelerType")
				{
					group.ClientSubUnitGuid = hierarchy[0].HierarchyCode.ToString();
					group.ClientSubUnitName = hierarchy[0].HierarchyName.Trim();
					group.TravelerTypeGuid = hierarchy[0].TravelerTypeGuid;
					group.TravelerTypeName = hierarchy[0].TravelerTypeName.Trim();
                    if (!string.IsNullOrEmpty(group.ClientSubUnitGuid))
                    {
                        ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
                        ClientSubUnit clientSubUnit = clientSubUnitRepository.GetClientSubUnit(group.ClientSubUnitGuid);
                        if (clientSubUnit != null && clientSubUnit.ClientTopUnit != null)
                        {
                            group.ClientTopUnitName = clientSubUnit.ClientTopUnit.ClientTopUnitName;
                        }
                    }
                }

				if (hierarchy[0].HierarchyType == "ClientSubUnit" || hierarchy[0].HierarchyType == "TravelerType")
				{
					if (hierarchy[0].ClientTopUnitName != null)
					{
						group.ClientTopUnitName = hierarchy[0].ClientTopUnitName.Trim();
					}
				}

				if (hierarchy[0].HierarchyType == "ClientAccount")
				{
					group.SourceSystemCode = hierarchy[0].SourceSystemCode;
				}
			}

			//Single hierarchy
			group.IsMultipleHierarchy = false;
		}

		//Get Hierarchy Details
		public List<fnDesktopDataAdmin_SelectFormOfPaymentAdviceMessageGroupHierarchy_v1Result> GetGroupHierarchy(int id)
		{
			var result = db.fnDesktopDataAdmin_SelectFormOfPaymentAdviceMessageGroupHierarchy_v1(id);
			return result.ToList();
		}

		//Edit Group
		public void Edit(FormOfPaymentAdviceMessageGroup group)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdateFormOfPaymentAdviceMessageGroup_v1(
				group.FormOfPaymentAdviceMessageGroupID,
				group.FormOfPaymentAdviceMessageGroupName,
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
		public void Add(FormOfPaymentAdviceMessageGroup group)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_InsertFormOfPaymentAdviceMessageGroup_v1(
				group.FormOfPaymentAdviceMessageGroupName,
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
