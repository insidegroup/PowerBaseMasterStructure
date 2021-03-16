using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using System.Collections.Generic;

namespace CWTDesktopDatabase.ViewModels
{
	[Bind(Exclude = "CreationTimestamp")]
	public class PolicyAllOtherGroupItemDataTableItemVM : CWTBaseViewModel
	{
		public PolicyAllOtherGroupItemDataTableRow PolicyAllOtherGroupItemDataTableRow { get; set; }
		public PolicyAllOtherGroupItem PolicyAllOtherGroupItem { get; set; }
		public PolicyOtherGroupHeader PolicyOtherGroupHeader { get; set; }
		public PolicyGroup PolicyGroup { get; set; }
		public List<PolicyAllOtherGroupItemDataTableItem> PolicyAllOtherGroupItemDataTableItems { get; set; }

		public PolicyAllOtherGroupItemDataTableItemVM()
		{

		}

		public PolicyAllOtherGroupItemDataTableItemVM(
			PolicyAllOtherGroupItemDataTableRow policyAllOtherGroupItemDataTableRow,
			PolicyAllOtherGroupItem policyAllOtherGroupItem,
			PolicyOtherGroupHeader policyOtherGroupHeader,
			PolicyGroup policyGroup,
			List<PolicyAllOtherGroupItemDataTableItem> policyAllOtherGroupItemDataTableItems
		)
		{
			PolicyAllOtherGroupItemDataTableRow = policyAllOtherGroupItemDataTableRow;
			PolicyAllOtherGroupItem = policyAllOtherGroupItem;
			PolicyOtherGroupHeader = policyOtherGroupHeader;
			PolicyGroup = policyGroup;
			PolicyAllOtherGroupItemDataTableItems = policyAllOtherGroupItemDataTableItems;
		}
	}
}