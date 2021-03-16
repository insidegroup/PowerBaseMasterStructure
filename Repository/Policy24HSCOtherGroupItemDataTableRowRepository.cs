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
	public class Policy24HSCOtherGroupItemDataTableRowRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//Get one Item
		public Policy24HSCOtherGroupItemDataTableRow GetPolicy24HSCOtherGroupItemDataTableRow(int policy24HSCOtherGroupItemDataTableRowId)
		{
			return db.Policy24HSCOtherGroupItemDataTableRows.SingleOrDefault(c => c.Policy24HSCOtherGroupItemDataTableRowId == policy24HSCOtherGroupItemDataTableRowId);
		}

		//Get items
		public List<Policy24HSCOtherGroupItemDataTableItem> GetPolicy24HSCOtherGroupItemDataTableItems(int policy24HSCOtherGroupItemDataTableRowId, int policyOtherGroupHeaderId)
		{

			List<Policy24HSCOtherGroupItemDataTableItem> policy24HSCOtherGroupItemDataTableItems = new List<Policy24HSCOtherGroupItemDataTableItem>();

			List<int> policyOtherGroupHeaderColumnNameIds = new List<int>();

			//Get Completed Columns
			List<Policy24HSCOtherGroupItemDataTableItem> policy24HSCOtherGroupItemDataTableItemsCompleted = new List<Policy24HSCOtherGroupItemDataTableItem>();
			policy24HSCOtherGroupItemDataTableItemsCompleted = db.Policy24HSCOtherGroupItemDataTableItems.Where(
				c => c.Policy24HSCOtherGroupItemDataTableRowId == policy24HSCOtherGroupItemDataTableRowId
			).ToList();

			foreach(Policy24HSCOtherGroupItemDataTableItem policy24HSCOtherGroupItemDataTableItem in policy24HSCOtherGroupItemDataTableItemsCompleted) {
				policy24HSCOtherGroupItemDataTableItems.Add(policy24HSCOtherGroupItemDataTableItem);
				policyOtherGroupHeaderColumnNameIds.Add(policy24HSCOtherGroupItemDataTableItem.PolicyOtherGroupHeaderColumnNameId);
			}

			//Get Empty Columns
			List<PolicyOtherGroupHeaderColumnName> policyOtherGroupHeaderColumnNames = new List<PolicyOtherGroupHeaderColumnName>();
			PolicyOtherGroupHeaderColumnNameRepository policyOtherGroupHeaderColumnNameRepository = new PolicyOtherGroupHeaderColumnNameRepository();
			policyOtherGroupHeaderColumnNames = policyOtherGroupHeaderColumnNameRepository.GetIncompletePolicyOtherGroupHeaderColumnNames(
					policyOtherGroupHeaderId,
					policyOtherGroupHeaderColumnNameIds);

			foreach(PolicyOtherGroupHeaderColumnName item in policyOtherGroupHeaderColumnNames) {
				Policy24HSCOtherGroupItemDataTableItem policy24HSCOtherGroupItemDataTableItem = new Policy24HSCOtherGroupItemDataTableItem()
					{
						PolicyOtherGroupHeaderColumnNameId = item.PolicyOtherGroupHeaderColumnNameId,
						PolicyOtherGroupHeaderColumnName = item
					};
					policy24HSCOtherGroupItemDataTableItems.Add(policy24HSCOtherGroupItemDataTableItem);
			}

			return policy24HSCOtherGroupItemDataTableItems; 
		}
    }
}
