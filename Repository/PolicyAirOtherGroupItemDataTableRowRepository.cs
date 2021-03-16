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
	public class PolicyAirOtherGroupItemDataTableRowRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//Get one Item
		public PolicyAirOtherGroupItemDataTableRow GetPolicyAirOtherGroupItemDataTableRow(int policyAirOtherGroupItemDataTableRowId)
		{
			return db.PolicyAirOtherGroupItemDataTableRows.SingleOrDefault(c => c.PolicyAirOtherGroupItemDataTableRowId == policyAirOtherGroupItemDataTableRowId);
		}

		//Get items
		public List<PolicyAirOtherGroupItemDataTableItem> GetPolicyAirOtherGroupItemDataTableItems(int policyAirOtherGroupItemDataTableRowId, int policyOtherGroupHeaderId)
		{

			List<PolicyAirOtherGroupItemDataTableItem> policyAirOtherGroupItemDataTableItems = new List<PolicyAirOtherGroupItemDataTableItem>();

			List<int> policyOtherGroupHeaderColumnNameIds = new List<int>();

			//Get Completed Columns
			List<PolicyAirOtherGroupItemDataTableItem> policyAirOtherGroupItemDataTableItemsCompleted = new List<PolicyAirOtherGroupItemDataTableItem>();
			policyAirOtherGroupItemDataTableItemsCompleted = db.PolicyAirOtherGroupItemDataTableItems.Where(
				c => c.PolicyAirOtherGroupItemDataTableRowId == policyAirOtherGroupItemDataTableRowId
			).ToList();

			foreach(PolicyAirOtherGroupItemDataTableItem policyAirOtherGroupItemDataTableItem in policyAirOtherGroupItemDataTableItemsCompleted) {
				policyAirOtherGroupItemDataTableItems.Add(policyAirOtherGroupItemDataTableItem);
				policyOtherGroupHeaderColumnNameIds.Add(policyAirOtherGroupItemDataTableItem.PolicyOtherGroupHeaderColumnNameId);
			}

			//Get Empty Columns
			List<PolicyOtherGroupHeaderColumnName> policyOtherGroupHeaderColumnNames = new List<PolicyOtherGroupHeaderColumnName>();
			PolicyOtherGroupHeaderColumnNameRepository policyOtherGroupHeaderColumnNameRepository = new PolicyOtherGroupHeaderColumnNameRepository();
			policyOtherGroupHeaderColumnNames = policyOtherGroupHeaderColumnNameRepository.GetIncompletePolicyOtherGroupHeaderColumnNames(
					policyOtherGroupHeaderId,
					policyOtherGroupHeaderColumnNameIds);

			foreach(PolicyOtherGroupHeaderColumnName item in policyOtherGroupHeaderColumnNames) {
				PolicyAirOtherGroupItemDataTableItem policyAirOtherGroupItemDataTableItem = new PolicyAirOtherGroupItemDataTableItem()
					{
						PolicyOtherGroupHeaderColumnNameId = item.PolicyOtherGroupHeaderColumnNameId,
						PolicyOtherGroupHeaderColumnName = item
					};
					policyAirOtherGroupItemDataTableItems.Add(policyAirOtherGroupItemDataTableItem);
			}

			policyAirOtherGroupItemDataTableItems = policyAirOtherGroupItemDataTableItems.OrderBy(x => x.PolicyOtherGroupHeaderColumnName.DisplayOrder).ToList();

			return policyAirOtherGroupItemDataTableItems; 
		}
    }
}
