using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using System.Collections.Generic;

namespace CWTDesktopDatabase.ViewModels
{
	[Bind(Exclude = "CreationTimestamp")]
	public class PolicyOtherGroupItemDataTableItemVM : CWTBaseViewModel
	{
		public PolicyOtherGroupItemDataTableRow PolicyOtherGroupItemDataTableRow { get; set; }
		public PolicyOtherGroupItem PolicyOtherGroupItem { get; set; }
		public PolicyOtherGroupHeader PolicyOtherGroupHeader { get; set; }
		public PolicyGroup PolicyGroup { get; set; }
		public List<PolicyOtherGroupItemDataTableItem> PolicyOtherGroupItemDataTableItems { get; set; }

		public PolicyOtherGroupItemDataTableItemVM()
		{

		}

		public PolicyOtherGroupItemDataTableItemVM(
			PolicyOtherGroupItemDataTableRow policyOtherGroupItemDataTableRow,
			PolicyOtherGroupItem policyOtherGroupItem,
			PolicyOtherGroupHeader policyOtherGroupHeader,
			PolicyGroup policyGroup,
			List<PolicyOtherGroupItemDataTableItem> policyOtherGroupItemDataTableItems
		)
		{
			PolicyOtherGroupItemDataTableRow = policyOtherGroupItemDataTableRow;
			PolicyOtherGroupItem = policyOtherGroupItem;
			PolicyOtherGroupHeader = policyOtherGroupHeader;
			PolicyGroup = policyGroup;
			PolicyOtherGroupItemDataTableItems = policyOtherGroupItemDataTableItems;
		}
	}
}