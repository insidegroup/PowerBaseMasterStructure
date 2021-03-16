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
	public class PolicyPriceTrackingOtherGroupItemDataTableRowRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//Get one Item
		public PolicyPriceTrackingOtherGroupItemDataTableRow GetPolicyPriceTrackingOtherGroupItemDataTableRow(int PolicyPriceTrackingOtherGroupItemDataTableRowId)
		{
			return db.PolicyPriceTrackingOtherGroupItemDataTableRows.SingleOrDefault(c => c.PolicyPriceTrackingOtherGroupItemDataTableRowId == PolicyPriceTrackingOtherGroupItemDataTableRowId);
		}

		//Get items
		public List<PolicyPriceTrackingOtherGroupItemDataTableItem> GetPolicyPriceTrackingOtherGroupItemDataTableItems(int PolicyPriceTrackingOtherGroupItemDataTableRowId, int policyOtherGroupHeaderId)
		{

			List<PolicyPriceTrackingOtherGroupItemDataTableItem> PolicyPriceTrackingOtherGroupItemDataTableItems = new List<PolicyPriceTrackingOtherGroupItemDataTableItem>();

			List<int> policyOtherGroupHeaderColumnNameIds = new List<int>();

			//Get Completed Columns
			List<PolicyPriceTrackingOtherGroupItemDataTableItem> PolicyPriceTrackingOtherGroupItemDataTableItemsCompleted = new List<PolicyPriceTrackingOtherGroupItemDataTableItem>();
			PolicyPriceTrackingOtherGroupItemDataTableItemsCompleted = db.PolicyPriceTrackingOtherGroupItemDataTableItems.Where(
				c => c.PolicyPriceTrackingOtherGroupItemDataTableRowId == PolicyPriceTrackingOtherGroupItemDataTableRowId
			).ToList();

			foreach(PolicyPriceTrackingOtherGroupItemDataTableItem PolicyPriceTrackingOtherGroupItemDataTableItem in PolicyPriceTrackingOtherGroupItemDataTableItemsCompleted) {
				PolicyPriceTrackingOtherGroupItemDataTableItems.Add(PolicyPriceTrackingOtherGroupItemDataTableItem);
				policyOtherGroupHeaderColumnNameIds.Add(PolicyPriceTrackingOtherGroupItemDataTableItem.PolicyOtherGroupHeaderColumnNameId);
			}

			//Get Empty Columns
			List<PolicyOtherGroupHeaderColumnName> policyOtherGroupHeaderColumnNames = new List<PolicyOtherGroupHeaderColumnName>();
			PolicyOtherGroupHeaderColumnNameRepository policyOtherGroupHeaderColumnNameRepository = new PolicyOtherGroupHeaderColumnNameRepository();
			policyOtherGroupHeaderColumnNames = policyOtherGroupHeaderColumnNameRepository.GetIncompletePolicyOtherGroupHeaderColumnNames(
					policyOtherGroupHeaderId,
					policyOtherGroupHeaderColumnNameIds);

			foreach(PolicyOtherGroupHeaderColumnName item in policyOtherGroupHeaderColumnNames) {
				PolicyPriceTrackingOtherGroupItemDataTableItem PolicyPriceTrackingOtherGroupItemDataTableItem = new PolicyPriceTrackingOtherGroupItemDataTableItem()
					{
						PolicyOtherGroupHeaderColumnNameId = item.PolicyOtherGroupHeaderColumnNameId,
						PolicyOtherGroupHeaderColumnName = item
					};
					PolicyPriceTrackingOtherGroupItemDataTableItems.Add(PolicyPriceTrackingOtherGroupItemDataTableItem);
			}

			PolicyPriceTrackingOtherGroupItemDataTableItems = PolicyPriceTrackingOtherGroupItemDataTableItems.OrderBy(x => x.PolicyOtherGroupHeaderColumnName.DisplayOrder).ToList();

			return PolicyPriceTrackingOtherGroupItemDataTableItems; 
		}
    }
}
