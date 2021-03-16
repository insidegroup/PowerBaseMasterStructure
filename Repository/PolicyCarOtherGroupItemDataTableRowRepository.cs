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
	public class PolicyCarOtherGroupItemDataTableRowRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//Get one Item
		public PolicyCarOtherGroupItemDataTableRow GetPolicyCarOtherGroupItemDataTableRow(int policyCarOtherGroupItemDataTableRowId)
		{
			return db.PolicyCarOtherGroupItemDataTableRows.SingleOrDefault(c => c.PolicyCarOtherGroupItemDataTableRowId == policyCarOtherGroupItemDataTableRowId);
		}

		//Get items
		public List<PolicyCarOtherGroupItemDataTableItem> GetPolicyCarOtherGroupItemDataTableItems(int policyCarOtherGroupItemDataTableRowId, int policyOtherGroupHeaderId)
		{

			List<PolicyCarOtherGroupItemDataTableItem> policyCarOtherGroupItemDataTableItems = new List<PolicyCarOtherGroupItemDataTableItem>();

			List<int> policyOtherGroupHeaderColumnNameIds = new List<int>();

			//Get Completed Columns
			List<PolicyCarOtherGroupItemDataTableItem> policyCarOtherGroupItemDataTableItemsCompleted = new List<PolicyCarOtherGroupItemDataTableItem>();
			policyCarOtherGroupItemDataTableItemsCompleted = db.PolicyCarOtherGroupItemDataTableItems.Where(
				c => c.PolicyCarOtherGroupItemDataTableRowId == policyCarOtherGroupItemDataTableRowId
			).ToList();

			foreach(PolicyCarOtherGroupItemDataTableItem policyCarOtherGroupItemDataTableItem in policyCarOtherGroupItemDataTableItemsCompleted) {
				policyCarOtherGroupItemDataTableItems.Add(policyCarOtherGroupItemDataTableItem);
				policyOtherGroupHeaderColumnNameIds.Add(policyCarOtherGroupItemDataTableItem.PolicyOtherGroupHeaderColumnNameId);
			}

			//Get Empty Columns
			List<PolicyOtherGroupHeaderColumnName> policyOtherGroupHeaderColumnNames = new List<PolicyOtherGroupHeaderColumnName>();
			PolicyOtherGroupHeaderColumnNameRepository policyOtherGroupHeaderColumnNameRepository = new PolicyOtherGroupHeaderColumnNameRepository();
			policyOtherGroupHeaderColumnNames = policyOtherGroupHeaderColumnNameRepository.GetIncompletePolicyOtherGroupHeaderColumnNames(
					policyOtherGroupHeaderId,
					policyOtherGroupHeaderColumnNameIds);

			foreach(PolicyOtherGroupHeaderColumnName item in policyOtherGroupHeaderColumnNames) {
				PolicyCarOtherGroupItemDataTableItem policyCarOtherGroupItemDataTableItem = new PolicyCarOtherGroupItemDataTableItem()
					{
						PolicyOtherGroupHeaderColumnNameId = item.PolicyOtherGroupHeaderColumnNameId,
						PolicyOtherGroupHeaderColumnName = item
					};
					policyCarOtherGroupItemDataTableItems.Add(policyCarOtherGroupItemDataTableItem);
			}

			return policyCarOtherGroupItemDataTableItems; 
		}
    }
}
