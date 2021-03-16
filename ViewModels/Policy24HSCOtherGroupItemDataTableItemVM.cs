using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using System.Collections.Generic;

namespace CWTDesktopDatabase.ViewModels
{
	[Bind(Exclude = "CreationTimestamp")]
	public class Policy24HSCOtherGroupItemDataTableItemVM : CWTBaseViewModel
	{
		public Policy24HSCOtherGroupItemDataTableRow Policy24HSCOtherGroupItemDataTableRow { get; set; }
		public Policy24HSCOtherGroupItem Policy24HSCOtherGroupItem { get; set; }
		public PolicyOtherGroupHeader PolicyOtherGroupHeader { get; set; }
		public PolicyGroup PolicyGroup { get; set; }
		public List<Policy24HSCOtherGroupItemDataTableItem> Policy24HSCOtherGroupItemDataTableItems { get; set; }

		public Policy24HSCOtherGroupItemDataTableItemVM()
		{

		}

		public Policy24HSCOtherGroupItemDataTableItemVM(
			Policy24HSCOtherGroupItemDataTableRow policy24HSCOtherGroupItemDataTableRow,
			Policy24HSCOtherGroupItem policy24HSCOtherGroupItem,
			PolicyOtherGroupHeader policyOtherGroupHeader,
			PolicyGroup policyGroup,
			List<Policy24HSCOtherGroupItemDataTableItem> policy24HSCOtherGroupItemDataTableItems
		)
		{
			Policy24HSCOtherGroupItemDataTableRow = policy24HSCOtherGroupItemDataTableRow;
			Policy24HSCOtherGroupItem = policy24HSCOtherGroupItem;
			PolicyOtherGroupHeader = policyOtherGroupHeader;
			PolicyGroup = policyGroup;
			Policy24HSCOtherGroupItemDataTableItems = policy24HSCOtherGroupItemDataTableItems;
		}
	}
}