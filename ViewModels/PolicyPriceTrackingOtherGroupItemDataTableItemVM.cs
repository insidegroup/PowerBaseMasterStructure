using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using System.Collections.Generic;

namespace CWTDesktopDatabase.ViewModels
{
	[Bind(Exclude = "CreationTimestamp")]
	public class PolicyPriceTrackingOtherGroupItemDataTableItemVM : CWTBaseViewModel
	{
		public PolicyPriceTrackingOtherGroupItemDataTableRow PolicyPriceTrackingOtherGroupItemDataTableRow { get; set; }
		public PolicyPriceTrackingOtherGroupItem PolicyPriceTrackingOtherGroupItem { get; set; }
		public PolicyOtherGroupHeader PolicyOtherGroupHeader { get; set; }
		public PolicyGroup PolicyGroup { get; set; }
		public List<PolicyPriceTrackingOtherGroupItemDataTableItem> PolicyPriceTrackingOtherGroupItemDataTableItems { get; set; }

		public PolicyPriceTrackingOtherGroupItemDataTableItemVM()
		{

		}

		public PolicyPriceTrackingOtherGroupItemDataTableItemVM(
			PolicyPriceTrackingOtherGroupItemDataTableRow policyPriceTrackingOtherGroupItemDataTableRow,
			PolicyPriceTrackingOtherGroupItem policyPriceTrackingOtherGroupItem,
			PolicyOtherGroupHeader policyOtherGroupHeader,
			PolicyGroup policyGroup,
			List<PolicyPriceTrackingOtherGroupItemDataTableItem> policyPriceTrackingOtherGroupItemDataTableItems
		)
		{
			PolicyPriceTrackingOtherGroupItemDataTableRow = policyPriceTrackingOtherGroupItemDataTableRow;
			PolicyPriceTrackingOtherGroupItem = policyPriceTrackingOtherGroupItem;
			PolicyOtherGroupHeader = policyOtherGroupHeader;
			PolicyGroup = policyGroup;
			PolicyPriceTrackingOtherGroupItemDataTableItems = policyPriceTrackingOtherGroupItemDataTableItems;
		}
	}
}