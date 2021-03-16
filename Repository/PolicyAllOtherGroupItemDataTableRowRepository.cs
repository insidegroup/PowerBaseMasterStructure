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
	public class PolicyAllOtherGroupItemDataTableRowRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//Get one Item
		public PolicyAllOtherGroupItemDataTableRow GetPolicyAllOtherGroupItemDataTableRow(int policyAllOtherGroupItemDataTableRowId)
		{
			return db.PolicyAllOtherGroupItemDataTableRows.SingleOrDefault(c => c.PolicyAllOtherGroupItemDataTableRowId == policyAllOtherGroupItemDataTableRowId);
		}

		//Get items
		public List<PolicyAllOtherGroupItemDataTableItem> GetPolicyAllOtherGroupItemDataTableItems(int policyAllOtherGroupItemDataTableRowId, int policyOtherGroupHeaderId)
		{

			List<PolicyAllOtherGroupItemDataTableItem> policyAllOtherGroupItemDataTableItems = new List<PolicyAllOtherGroupItemDataTableItem>();

			List<int> policyOtherGroupHeaderColumnNameIds = new List<int>();

			//Get Completed Columns
			List<PolicyAllOtherGroupItemDataTableItem> policyAllOtherGroupItemDataTableItemsCompleted = new List<PolicyAllOtherGroupItemDataTableItem>();
			policyAllOtherGroupItemDataTableItemsCompleted = db.PolicyAllOtherGroupItemDataTableItems.Where(
				c => c.PolicyAllOtherGroupItemDataTableRowId == policyAllOtherGroupItemDataTableRowId
			).ToList();

			foreach(PolicyAllOtherGroupItemDataTableItem policyAllOtherGroupItemDataTableItem in policyAllOtherGroupItemDataTableItemsCompleted) {
				policyAllOtherGroupItemDataTableItems.Add(policyAllOtherGroupItemDataTableItem);
				policyOtherGroupHeaderColumnNameIds.Add(policyAllOtherGroupItemDataTableItem.PolicyOtherGroupHeaderColumnNameId);
			}

			//Get Empty Columns
			List<PolicyOtherGroupHeaderColumnName> policyOtherGroupHeaderColumnNames = new List<PolicyOtherGroupHeaderColumnName>();
			PolicyOtherGroupHeaderColumnNameRepository policyOtherGroupHeaderColumnNameRepository = new PolicyOtherGroupHeaderColumnNameRepository();
			policyOtherGroupHeaderColumnNames = policyOtherGroupHeaderColumnNameRepository.GetIncompletePolicyOtherGroupHeaderColumnNames(
					policyOtherGroupHeaderId,
					policyOtherGroupHeaderColumnNameIds);

			foreach(PolicyOtherGroupHeaderColumnName item in policyOtherGroupHeaderColumnNames) {
				PolicyAllOtherGroupItemDataTableItem policyAllOtherGroupItemDataTableItem = new PolicyAllOtherGroupItemDataTableItem()
					{
						PolicyOtherGroupHeaderColumnNameId = item.PolicyOtherGroupHeaderColumnNameId,
						PolicyOtherGroupHeaderColumnName = item
					};
					policyAllOtherGroupItemDataTableItems.Add(policyAllOtherGroupItemDataTableItem);
			}

			return policyAllOtherGroupItemDataTableItems; 
		}
    }
}
