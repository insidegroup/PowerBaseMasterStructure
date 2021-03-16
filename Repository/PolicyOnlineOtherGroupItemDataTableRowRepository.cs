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
	public class PolicyOnlineOtherGroupItemDataTableRowRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//Get one Item
		public PolicyOnlineOtherGroupItemDataTableRow GetPolicyOnlineOtherGroupItemDataTableRow(int PolicyOnlineOtherGroupItemDataTableRowId)
		{
			return db.PolicyOnlineOtherGroupItemDataTableRows.SingleOrDefault(c => c.PolicyOnlineOtherGroupItemDataTableRowId == PolicyOnlineOtherGroupItemDataTableRowId);
		}

		//Get items
		public List<PolicyOnlineOtherGroupItemDataTableItem> GetPolicyOnlineOtherGroupItemDataTableItems(int PolicyOnlineOtherGroupItemDataTableRowId, int policyOtherGroupHeaderId)
		{

			List<PolicyOnlineOtherGroupItemDataTableItem> PolicyOnlineOtherGroupItemDataTableItems = new List<PolicyOnlineOtherGroupItemDataTableItem>();

			List<int> policyOtherGroupHeaderColumnNameIds = new List<int>();

			//Get Completed Columns
			List<PolicyOnlineOtherGroupItemDataTableItem> PolicyOnlineOtherGroupItemDataTableItemsCompleted = new List<PolicyOnlineOtherGroupItemDataTableItem>();
			PolicyOnlineOtherGroupItemDataTableItemsCompleted = db.PolicyOnlineOtherGroupItemDataTableItems.Where(
				c => c.PolicyOnlineOtherGroupItemDataTableRowId == PolicyOnlineOtherGroupItemDataTableRowId
			).ToList();

			foreach(PolicyOnlineOtherGroupItemDataTableItem PolicyOnlineOtherGroupItemDataTableItem in PolicyOnlineOtherGroupItemDataTableItemsCompleted) {
				PolicyOnlineOtherGroupItemDataTableItems.Add(PolicyOnlineOtherGroupItemDataTableItem);
				policyOtherGroupHeaderColumnNameIds.Add(PolicyOnlineOtherGroupItemDataTableItem.PolicyOtherGroupHeaderColumnNameId);
			}

			//Get Empty Columns
			List<PolicyOtherGroupHeaderColumnName> policyOtherGroupHeaderColumnNames = new List<PolicyOtherGroupHeaderColumnName>();
			PolicyOtherGroupHeaderColumnNameRepository policyOtherGroupHeaderColumnNameRepository = new PolicyOtherGroupHeaderColumnNameRepository();
			policyOtherGroupHeaderColumnNames = policyOtherGroupHeaderColumnNameRepository.GetIncompletePolicyOtherGroupHeaderColumnNames(
					policyOtherGroupHeaderId,
					policyOtherGroupHeaderColumnNameIds);

			foreach(PolicyOtherGroupHeaderColumnName item in policyOtherGroupHeaderColumnNames) {
				PolicyOnlineOtherGroupItemDataTableItem PolicyOnlineOtherGroupItemDataTableItem = new PolicyOnlineOtherGroupItemDataTableItem()
					{
						PolicyOtherGroupHeaderColumnNameId = item.PolicyOtherGroupHeaderColumnNameId,
						PolicyOtherGroupHeaderColumnName = item
					};
					PolicyOnlineOtherGroupItemDataTableItems.Add(PolicyOnlineOtherGroupItemDataTableItem);
			}

			PolicyOnlineOtherGroupItemDataTableItems = PolicyOnlineOtherGroupItemDataTableItems.OrderBy(x => x.PolicyOtherGroupHeaderColumnName.DisplayOrder).ToList();

			return PolicyOnlineOtherGroupItemDataTableItems; 
		}
    }
}
