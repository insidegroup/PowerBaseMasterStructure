using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using System.Collections.Generic;

namespace CWTDesktopDatabase.ViewModels
{
	[Bind(Exclude = "CreationTimestamp")]
	public class PolicyAirOtherGroupItemDataTableItemVM : CWTBaseViewModel
	{
		public PolicyAirOtherGroupItemDataTableRow PolicyAirOtherGroupItemDataTableRow { get; set; }
		public PolicyAirOtherGroupItem PolicyAirOtherGroupItem { get; set; }
		public PolicyOtherGroupHeader PolicyOtherGroupHeader { get; set; }
		public PolicyGroup PolicyGroup { get; set; }
		public List<PolicyAirOtherGroupItemDataTableItem> PolicyAirOtherGroupItemDataTableItems { get; set; }

		public PolicyAirOtherGroupItemDataTableItemVM()
		{

		}

		public PolicyAirOtherGroupItemDataTableItemVM(
			PolicyAirOtherGroupItemDataTableRow policyAirOtherGroupItemDataTableRow,
			PolicyAirOtherGroupItem policyAirOtherGroupItem,
			PolicyOtherGroupHeader policyOtherGroupHeader,
			PolicyGroup policyGroup,
			List<PolicyAirOtherGroupItemDataTableItem> policyAirOtherGroupItemDataTableItems
		)
		{
			PolicyAirOtherGroupItemDataTableRow = policyAirOtherGroupItemDataTableRow;
			PolicyAirOtherGroupItem = policyAirOtherGroupItem;
			PolicyOtherGroupHeader = policyOtherGroupHeader;
			PolicyGroup = policyGroup;
			PolicyAirOtherGroupItemDataTableItems = policyAirOtherGroupItemDataTableItems;
		}
	}
}