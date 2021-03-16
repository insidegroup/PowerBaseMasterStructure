using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using System.Collections.Generic;

namespace CWTDesktopDatabase.ViewModels
{
	[Bind(Exclude = "CreationTimestamp")]
	public class PolicyHotelOtherGroupItemDataTableItemVM : CWTBaseViewModel
	{
		public PolicyHotelOtherGroupItemDataTableRow PolicyHotelOtherGroupItemDataTableRow { get; set; }
		public PolicyHotelOtherGroupItem PolicyHotelOtherGroupItem { get; set; }
		public PolicyOtherGroupHeader PolicyOtherGroupHeader { get; set; }
		public PolicyGroup PolicyGroup { get; set; }
		public List<PolicyHotelOtherGroupItemDataTableItem> PolicyHotelOtherGroupItemDataTableItems { get; set; }

		public PolicyHotelOtherGroupItemDataTableItemVM()
		{

		}

		public PolicyHotelOtherGroupItemDataTableItemVM(
			PolicyHotelOtherGroupItemDataTableRow policyHotelOtherGroupItemDataTableRow,
			PolicyHotelOtherGroupItem policyHotelOtherGroupItem,
			PolicyOtherGroupHeader policyOtherGroupHeader,
			PolicyGroup policyGroup,
			List<PolicyHotelOtherGroupItemDataTableItem> policyHotelOtherGroupItemDataTableItems
		)
		{
			PolicyHotelOtherGroupItemDataTableRow = policyHotelOtherGroupItemDataTableRow;
			PolicyHotelOtherGroupItem = policyHotelOtherGroupItem;
			PolicyOtherGroupHeader = policyOtherGroupHeader;
			PolicyGroup = policyGroup;
			PolicyHotelOtherGroupItemDataTableItems = policyHotelOtherGroupItemDataTableItems;
		}
	}
}