using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace CWTDesktopDatabase.Repository
{
	public class PolicyOtherGroupHeaderColumnNameRepository
	{
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//Get a Page of PolicyOtherGroupHeaderColumnNames
		public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyOtherGroupHeaderColumnNames_v1Result> PagePolicyOtherGroupHeaderColumnNames(int id, int page, string filter, string sortField, int sortOrder)
		{
			//get a page of records
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectPolicyOtherGroupHeaderColumnNames_v1(id, filter, sortField, sortOrder, page, adminUserGuid).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}

			//put into page object
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPolicyOtherGroupHeaderColumnNames_v1Result>(result, page, totalRecords);

			//return to user
			return paginatedView;
		}

		//Get one PolicyOtherGroupHeaderColumnName
		public PolicyOtherGroupHeaderColumnName GetPolicyOtherGroupHeaderColumnName(int policyOtherGroupHeaderColumnNameId)
		{
			return db.PolicyOtherGroupHeaderColumnNames.SingleOrDefault(c => c.PolicyOtherGroupHeaderColumnNameId == policyOtherGroupHeaderColumnNameId);
		}

		//Get PolicyOtherGroupHeaderColumnNames
		public List<PolicyOtherGroupHeaderColumnName> GetPolicyOtherGroupHeaderColumnNames(int policyOtherGroupHeaderId)
		{
			return (from columns in db.PolicyOtherGroupHeaderColumnNames
					join table in db.PolicyOtherGroupHeaderTableNames on columns.PolicyOtherGroupHeaderTableNameId equals table.PolicyOtherGroupHeaderTableNameId
					where table.PolicyOtherGroupHeaderId == policyOtherGroupHeaderId
					select columns).OrderBy(x => x.DisplayOrder)
					.ToList();
		}

		//Get Incomplete PolicyOtherGroupHeaderColumnNames
		public List<PolicyOtherGroupHeaderColumnName> GetIncompletePolicyOtherGroupHeaderColumnNames(int policyOtherGroupHeaderId, List<int> policyOtherGroupHeaderColumnNameIds)
		{
			return (from columns in db.PolicyOtherGroupHeaderColumnNames
					join table in db.PolicyOtherGroupHeaderTableNames on columns.PolicyOtherGroupHeaderTableNameId equals table.PolicyOtherGroupHeaderTableNameId
					where !policyOtherGroupHeaderColumnNameIds.Contains(columns.PolicyOtherGroupHeaderColumnNameId) && table.PolicyOtherGroupHeaderId == policyOtherGroupHeaderId
					select columns).OrderBy(x => x.DisplayOrder)
					.ToList(); 
		}

		//Add PolicyOtherGroupHeaderColumnName
		public void Add(PolicyOtherGroupHeaderColumnNameVM policyOtherGroupHeaderColumnNameVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_InsertPolicyOtherGroupHeaderColumnName_v1(
				policyOtherGroupHeaderColumnNameVM.PolicyOtherGroupHeaderTableName.PolicyOtherGroupHeaderTableNameId,
				policyOtherGroupHeaderColumnNameVM.PolicyOtherGroupHeaderColumnName.ColumnName,
				adminUserGuid
			);
		}

		//Edit PolicyOtherGroupHeaderColumnName
		public void Edit(PolicyOtherGroupHeaderColumnName policyOtherGroupHeaderColumnName)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdatePolicyOtherGroupHeaderColumnName_v1(
				policyOtherGroupHeaderColumnName.PolicyOtherGroupHeaderColumnNameId,
				policyOtherGroupHeaderColumnName.ColumnName,
				adminUserGuid,
				policyOtherGroupHeaderColumnName.VersionNumber
			);
		}

		//Delete PolicyOtherGroupHeaderColumnName
		public void Delete(PolicyOtherGroupHeaderColumnName policyOtherGroupHeaderColumnName)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeletePolicyOtherGroupHeaderColumnName_v1(
				policyOtherGroupHeaderColumnName.PolicyOtherGroupHeaderColumnNameId,
				adminUserGuid,
				policyOtherGroupHeaderColumnName.VersionNumber
			);
		}

		//List of PolicyOtherGroupHeaderColumnNames Sequences - Paged
		public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyOtherGroupHeaderColumnNameSequences_v1Result> PagePolicyOtherGroupHeaderColumnNameSequences(int page, int id)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			int pageSize = 15;
			var result = db.spDesktopDataAdmin_SelectPolicyOtherGroupHeaderColumnNameSequences_v1(page, pageSize, id).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPolicyOtherGroupHeaderColumnNameSequences_v1Result>(result, page, Convert.ToInt32(totalRecords));
			return paginatedView;

		}

		//Update Sequences of PolicyOtherGroupHeaderColumnName
		public void UpdatePolicyOtherGroupHeaderColumnNameSequences(System.Xml.Linq.XElement xmlElement)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			db.spDesktopDataAdmin_UpdatePolicyOtherGroupHeaderColumnNameSequences_v1(xmlElement, adminUserGuid);

		}
	}
}