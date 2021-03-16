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
	public class PolicyHotelOtherGroupItemDataTableRowRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//Get one Item
		public PolicyHotelOtherGroupItemDataTableRow GetPolicyHotelOtherGroupItemDataTableRow(int policyHotelOtherGroupItemDataTableRowId)
		{
			return db.PolicyHotelOtherGroupItemDataTableRows.SingleOrDefault(c => c.PolicyHotelOtherGroupItemDataTableRowId == policyHotelOtherGroupItemDataTableRowId);
		}

		//Get items
		public List<PolicyHotelOtherGroupItemDataTableItem> GetPolicyHotelOtherGroupItemDataTableItems(int policyHotelOtherGroupItemDataTableRowId, int policyOtherGroupHeaderId)
		{

			List<PolicyHotelOtherGroupItemDataTableItem> policyHotelOtherGroupItemDataTableItems = new List<PolicyHotelOtherGroupItemDataTableItem>();

			List<int> policyOtherGroupHeaderColumnNameIds = new List<int>();

			//Get Completed Columns
			List<PolicyHotelOtherGroupItemDataTableItem> policyHotelOtherGroupItemDataTableItemsCompleted = new List<PolicyHotelOtherGroupItemDataTableItem>();
			policyHotelOtherGroupItemDataTableItemsCompleted = db.PolicyHotelOtherGroupItemDataTableItems.Where(
				c => c.PolicyHotelOtherGroupItemDataTableRowId == policyHotelOtherGroupItemDataTableRowId
			).ToList();

			foreach(PolicyHotelOtherGroupItemDataTableItem policyHotelOtherGroupItemDataTableItem in policyHotelOtherGroupItemDataTableItemsCompleted) {
				policyHotelOtherGroupItemDataTableItems.Add(policyHotelOtherGroupItemDataTableItem);
				policyOtherGroupHeaderColumnNameIds.Add(policyHotelOtherGroupItemDataTableItem.PolicyOtherGroupHeaderColumnNameId);
			}

			//Get Empty Columns
			List<PolicyOtherGroupHeaderColumnName> policyOtherGroupHeaderColumnNames = new List<PolicyOtherGroupHeaderColumnName>();
			PolicyOtherGroupHeaderColumnNameRepository policyOtherGroupHeaderColumnNameRepository = new PolicyOtherGroupHeaderColumnNameRepository();
			policyOtherGroupHeaderColumnNames = policyOtherGroupHeaderColumnNameRepository.GetIncompletePolicyOtherGroupHeaderColumnNames(
					policyOtherGroupHeaderId,
					policyOtherGroupHeaderColumnNameIds);

			foreach(PolicyOtherGroupHeaderColumnName item in policyOtherGroupHeaderColumnNames) {
				PolicyHotelOtherGroupItemDataTableItem policyHotelOtherGroupItemDataTableItem = new PolicyHotelOtherGroupItemDataTableItem()
					{
						PolicyOtherGroupHeaderColumnNameId = item.PolicyOtherGroupHeaderColumnNameId,
						PolicyOtherGroupHeaderColumnName = item
					};
					policyHotelOtherGroupItemDataTableItems.Add(policyHotelOtherGroupItemDataTableItem);
			}

			return policyHotelOtherGroupItemDataTableItems; 
		}
    }
}
