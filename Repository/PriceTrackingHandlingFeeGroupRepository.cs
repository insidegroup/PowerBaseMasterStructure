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
	public class PriceTrackingHandlingFeeGroupRepository
	{
		private PriceTrackingDC db = new PriceTrackingDC(Settings.getConnectionString());

		//Get a Page of Price Tracking Handling Fee Groups - for Page Listings
		public CWTPaginatedList<spDesktopDataAdmin_SelectPriceTrackingHandlingFeeGroups_v1Result> PagePriceTrackingHandlingFeeGroups(bool deleted, int page, string filter, string sortField, int sortOrder)
		{
			//get a page of records
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectPriceTrackingHandlingFeeGroups_v1(deleted, filter, sortField, sortOrder, page, adminUserGuid).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}

			//put into page object
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPriceTrackingHandlingFeeGroups_v1Result>(result, page, totalRecords);

			//return to user
			return paginatedView;
		}

		//Get one Price Tracking Handling Fee Group
		public PriceTrackingHandlingFeeGroup GetGroup(int id)
		{
			return db.PriceTrackingHandlingFeeGroups.SingleOrDefault(c => c.PriceTrackingHandlingFeeGroupId == id);
		}

		//Change the deleted status on an item
		public void UpdateGroupDeletedStatus(PriceTrackingHandlingFeeGroup group)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdatePriceTrackingHandlingFeeGroupDeletedStatus_v1(
					group.PriceTrackingHandlingFeeGroupId,
					group.DeletedFlag,
					adminUserGuid,
					group.VersionNumber
					);

		}

		//Add Data From Linked Tables for Display
		public void EditGroupForDisplay(PriceTrackingHandlingFeeGroup group)
		{

			HierarchyRepository hierarchyRepository = new HierarchyRepository();

			List<fnDesktopDataAdmin_SelectPriceTrackingHandlingFeeGroupHierarchy_v1Result> hierarchy = new List<fnDesktopDataAdmin_SelectPriceTrackingHandlingFeeGroupHierarchy_v1Result>();
			hierarchy = GetGroupHierarchy(group.PriceTrackingHandlingFeeGroupId);
			group.PriceTrackingHandlingFeeGroupName = Regex.Replace(group.PriceTrackingHandlingFeeGroupName, @"[^\w\-()*]", "-");

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
		public List<fnDesktopDataAdmin_SelectPriceTrackingHandlingFeeGroupHierarchy_v1Result> GetGroupHierarchy(int id)
		{
			var result = db.fnDesktopDataAdmin_SelectPriceTrackingHandlingFeeGroupHierarchy_v1(id);
			return result.ToList();
		}

		//Edit Group
		public void Edit(PriceTrackingHandlingFeeGroup group)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdatePriceTrackingHandlingFeeGroup_v1(
				adminUserGuid,
				group.PriceTrackingHandlingFeeGroupId,
				group.PriceTrackingHandlingFeeGroupName,
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
		public void Add(PriceTrackingHandlingFeeGroup group)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_InsertPriceTrackingHandlingFeeGroup_v1(
				adminUserGuid,
				group.PriceTrackingHandlingFeeGroupName,
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
        //Get ClientSubUnits Linked to a PriceTrackingHandlingFeeGroup
        public List<ClientSubUnitCountryVM> GetLinkedClientSubUnits(int priceTrackingGroupid, bool linked)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            var result = from n in db.spDesktopDataAdmin_SelectPriceTrackingHandlingFeeGroupLinkedClientSubUnits_v1(priceTrackingGroupid, adminUserGuid, linked)
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
        public void UpdateLinkedClientSubUnit(int priceTrackingHandlingFeeGroupGroupId, string clientSubUnitGuid)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdatePriceTrackingHandlingFeeGroupLinkedClientSubUnit_v1(
                    priceTrackingHandlingFeeGroupGroupId,
                    clientSubUnitGuid,
                    adminUserGuid
                    );

        }
    }
}
