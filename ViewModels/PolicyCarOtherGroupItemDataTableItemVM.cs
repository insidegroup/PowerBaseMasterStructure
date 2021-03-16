using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using System.Collections.Generic;

namespace CWTDesktopDatabase.ViewModels
{
	[Bind(Exclude = "CreationTimestamp")]
	public class PolicyCarOtherGroupItemDataTableItemVM : CWTBaseViewModel
	{
		public PolicyCarOtherGroupItemDataTableRow PolicyCarOtherGroupItemDataTableRow { get; set; }
		public PolicyCarOtherGroupItem PolicyCarOtherGroupItem { get; set; }
		public PolicyOtherGroupHeader PolicyOtherGroupHeader { get; set; }
		public PolicyGroup PolicyGroup { get; set; }
		public List<PolicyCarOtherGroupItemDataTableItem> PolicyCarOtherGroupItemDataTableItems { get; set; }

		public PolicyCarOtherGroupItemDataTableItemVM()
		{

		}

		public PolicyCarOtherGroupItemDataTableItemVM(
			PolicyCarOtherGroupItemDataTableRow policyCarOtherGroupItemDataTableRow,
			PolicyCarOtherGroupItem policyCarOtherGroupItem,
			PolicyOtherGroupHeader policyOtherGroupHeader,
			PolicyGroup policyGroup,
			List<PolicyCarOtherGroupItemDataTableItem> policyCarOtherGroupItemDataTableItems
		)
		{
			PolicyCarOtherGroupItemDataTableRow = policyCarOtherGroupItemDataTableRow;
			PolicyCarOtherGroupItem = policyCarOtherGroupItem;
			PolicyOtherGroupHeader = policyOtherGroupHeader;
			PolicyGroup = policyGroup;
			PolicyCarOtherGroupItemDataTableItems = policyCarOtherGroupItemDataTableItems;
		}
	}
}