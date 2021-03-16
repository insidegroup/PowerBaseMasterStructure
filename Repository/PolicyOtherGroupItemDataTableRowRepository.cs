using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using System.Linq.Dynamic;
using CWTDesktopDatabase.ViewModels;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace CWTDesktopDatabase.Repository
{
	public class PolicyOtherGroupItemDataTableRowRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//Get one Item
		public PolicyOtherGroupItemDataTableRow GetPolicyOtherGroupItemDataTableRow(int policyOtherGroupItemDataTableRowId)
		{
			return db.PolicyOtherGroupItemDataTableRows.SingleOrDefault(c => c.PolicyOtherGroupItemDataTableRowId == policyOtherGroupItemDataTableRowId);
		}

		//Get items
		public List<PolicyOtherGroupItemDataTableItem> GetPolicyOtherGroupItemDataTableItems(int policyOtherGroupItemDataTableRowId, int policyOtherGroupHeaderId)
		{

			List<PolicyOtherGroupItemDataTableItem> policyOtherGroupItemDataTableItems = new List<PolicyOtherGroupItemDataTableItem>();

			List<int> policyOtherGroupHeaderColumnNameIds = new List<int>();

			//Get Completed Columns
			List<PolicyOtherGroupItemDataTableItem> policyOtherGroupItemDataTableItemsCompleted = new List<PolicyOtherGroupItemDataTableItem>();
			policyOtherGroupItemDataTableItemsCompleted = db.PolicyOtherGroupItemDataTableItems.Where(
				c => c.PolicyOtherGroupItemDataTableRowId == policyOtherGroupItemDataTableRowId
			).ToList();

			foreach(PolicyOtherGroupItemDataTableItem policyOtherGroupItemDataTableItem in policyOtherGroupItemDataTableItemsCompleted) {
				policyOtherGroupItemDataTableItems.Add(policyOtherGroupItemDataTableItem);
				policyOtherGroupHeaderColumnNameIds.Add(policyOtherGroupItemDataTableItem.PolicyOtherGroupHeaderColumnNameId);
			}

			//Get Empty Columns
			List<PolicyOtherGroupHeaderColumnName> policyOtherGroupHeaderColumnNames = new List<PolicyOtherGroupHeaderColumnName>();
			PolicyOtherGroupHeaderColumnNameRepository policyOtherGroupHeaderColumnNameRepository = new PolicyOtherGroupHeaderColumnNameRepository();
			policyOtherGroupHeaderColumnNames = policyOtherGroupHeaderColumnNameRepository.GetIncompletePolicyOtherGroupHeaderColumnNames(
					policyOtherGroupHeaderId,
					policyOtherGroupHeaderColumnNameIds);

			foreach (PolicyOtherGroupHeaderColumnName item in policyOtherGroupHeaderColumnNames)
			{
				PolicyOtherGroupItemDataTableItem policyOtherGroupItemDataTableItem = new PolicyOtherGroupItemDataTableItem()
					{
						PolicyOtherGroupHeaderColumnNameId = item.PolicyOtherGroupHeaderColumnNameId,
						PolicyOtherGroupHeaderColumnName = item
					};
				policyOtherGroupItemDataTableItems.Add(policyOtherGroupItemDataTableItem);
			}

			return policyOtherGroupItemDataTableItems.OrderBy(x => x.PolicyOtherGroupHeaderColumnName.ColumnName).ToList(); 
		}
    }
}
