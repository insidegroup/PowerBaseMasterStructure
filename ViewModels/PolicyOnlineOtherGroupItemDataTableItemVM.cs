using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using System.Collections.Generic;

namespace CWTDesktopDatabase.ViewModels
{
	[Bind(Exclude = "CreationTimestamp")]
	public class PolicyOnlineOtherGroupItemDataTableItemVM : CWTBaseViewModel
	{
		public PolicyOnlineOtherGroupItemDataTableRow PolicyOnlineOtherGroupItemDataTableRow { get; set; }
		public PolicyOnlineOtherGroupItem PolicyOnlineOtherGroupItem { get; set; }
		public PolicyOtherGroupHeader PolicyOtherGroupHeader { get; set; }
		public PolicyGroup PolicyGroup { get; set; }
		public List<PolicyOnlineOtherGroupItemDataTableItem> PolicyOnlineOtherGroupItemDataTableItems { get; set; }

		public PolicyOnlineOtherGroupItemDataTableItemVM()
		{

		}

		public PolicyOnlineOtherGroupItemDataTableItemVM(
			PolicyOnlineOtherGroupItemDataTableRow PolicyOnlineOtherGroupItemDataTableRow,
			PolicyOnlineOtherGroupItem PolicyOnlineOtherGroupItem,
			PolicyOtherGroupHeader policyOtherGroupHeader,
			PolicyGroup policyGroup,
			List<PolicyOnlineOtherGroupItemDataTableItem> PolicyOnlineOtherGroupItemDataTableItems
		)
		{
			PolicyOnlineOtherGroupItemDataTableRow = PolicyOnlineOtherGroupItemDataTableRow;
			PolicyOnlineOtherGroupItem = PolicyOnlineOtherGroupItem;
			PolicyOtherGroupHeader = policyOtherGroupHeader;
			PolicyGroup = policyGroup;
			PolicyOnlineOtherGroupItemDataTableItems = PolicyOnlineOtherGroupItemDataTableItems;
		}
	}
}