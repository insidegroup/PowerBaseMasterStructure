using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using System.Collections.Generic;
using CWTDesktopDatabase.Helpers;
using System.Data;

namespace CWTDesktopDatabase.ViewModels
{
	[Bind(Exclude = "CreationTimestamp")]
	public class Policy24HSCOtherGroupItemDataTableItemsVM : CWTBaseViewModel
	{
		public DataTable Policy24HSCOtherGroupItemDataTableItems { get; set; }
		public Policy24HSCOtherGroupItem Policy24HSCOtherGroupItem { get; set; }
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

		public Policy24HSCOtherGroupItemDataTableItemsVM()
		{
			HasWriteAccess = false;
		}

		public Policy24HSCOtherGroupItemDataTableItemsVM(
			DataTable policy24HSCOtherGroupItemDataTableItems,
			Policy24HSCOtherGroupItem policy24HSCOtherGroupItem,
			PolicyOtherGroupHeader policyOtherGroupHeader,
			PolicyGroup policyGroup
		)
		{
			Policy24HSCOtherGroupItemDataTableItems = policy24HSCOtherGroupItemDataTableItems;
			Policy24HSCOtherGroupItem = policy24HSCOtherGroupItem;
			PolicyOtherGroupHeader = policyOtherGroupHeader;
			PolicyGroup = policyGroup;
		}
	}
}