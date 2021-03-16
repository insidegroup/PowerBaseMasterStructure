using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using System.Collections.Generic;
using CWTDesktopDatabase.Helpers;
using System.Data;

namespace CWTDesktopDatabase.ViewModels
{
	[Bind(Exclude = "CreationTimestamp")]
	public class PolicyHotelOtherGroupItemDataTableItemsVM : CWTBaseViewModel
	{
		public DataTable PolicyHotelOtherGroupItemDataTableItems { get; set; }
		public PolicyHotelOtherGroupItem PolicyHotelOtherGroupItem { get; set; }
		public PolicyOtherGroupHeader PolicyOtherGroupHeader { get; set; }
		public PolicyGroup PolicyGroup { get; set; }
		public bool HasWriteAccess { get; set; }

		//Custom Paging as uses DataTable and not CWTPagination
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
		public bool HasPreviousPage { get; set; }
		public bool HasNextPage { get; set; }

		public PolicyHotelOtherGroupItemDataTableItemsVM()
		{
			HasWriteAccess = false;
		}

		public PolicyHotelOtherGroupItemDataTableItemsVM(
			DataTable policyHotelOtherGroupItemDataTableItems,
			PolicyHotelOtherGroupItem policyHotelOtherGroupItem,
			PolicyOtherGroupHeader policyOtherGroupHeader,
			PolicyGroup policyGroup
		)
		{
			PolicyHotelOtherGroupItemDataTableItems = policyHotelOtherGroupItemDataTableItems;
			PolicyHotelOtherGroupItem = policyHotelOtherGroupItem;
			PolicyOtherGroupHeader = policyOtherGroupHeader;
			PolicyGroup = policyGroup;
		}
	}
}