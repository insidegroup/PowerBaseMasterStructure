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
    public class ApprovalItemRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//Get a Page of ApprovalItems - for Page Listings
		public CWTPaginatedList<spDesktopDataAdmin_SelectApprovalItems_v1Result> PageApprovalItems(int id, int page, string filter, string sortField, int sortOrder)
		{
			//get a page of records
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectApprovalItems_v1(id, filter, sortField, sortOrder, page, adminUserGuid).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}

			//put into page object
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectApprovalItems_v1Result>(result, page, totalRecords);

			//return to user
			return paginatedView;
		}

		//Get One ApprovalItem
		public ApprovalItem ApprovalItem(int id)
		{
			return db.ApprovalItems.SingleOrDefault(c => c.ApprovalItemId == id);
		}

		//Add Data From Linked Tables for Display
		public void EditForDisplay(ApprovalItem ApprovalItem)
		{

		}

		//Add to DB
		public void Add(ApprovalItem approvalItem)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			int? approvalItemId = new Int32();

			db.spDesktopDataAdmin_InsertApprovalItem_v1(
				ref approvalItemId,
				approvalItem.ApproverDescription,
				approvalItem.RemarkText,
				approvalItem.ApprovalGroupId,
				adminUserGuid
			);

			approvalItem.ApprovalItemId = (int)approvalItemId;
		}

		//Update in DB
		public void Update(ApprovalItem approvalItem)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdateApprovalItem_v1(
				approvalItem.ApprovalItemId,
				approvalItem.ApproverDescription,
				approvalItem.RemarkText,
				adminUserGuid,
				approvalItem.VersionNumber
			);
		}

		//Delete From DB
		public void Delete(ApprovalItem ApprovalItem)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeleteApprovalItem_v1(
				ApprovalItem.ApprovalItemId,
				adminUserGuid,
				ApprovalItem.VersionNumber
			);
		}

	}
}
