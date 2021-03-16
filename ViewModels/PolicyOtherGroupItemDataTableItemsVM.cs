using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using System.Collections.Generic;
using CWTDesktopDatabase.Helpers;
using System.Data;

namespace CWTDesktopDatabase.ViewModels
{
	[Bind(Exclude = "CreationTimestamp")]
	public class PolicyOtherGroupItemDataTableItemsVM : CWTBaseViewModel
	{
		public DataTable PolicyOtherGroupItemDataTableItems { get; set; }
		public PolicyOtherGroupItem PolicyOtherGroupItem { get; set; }
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

		public PolicyOtherGroupItemDataTableItemsVM()
		{
			HasWriteAccess = false;
		}

		public PolicyOtherGroupItemDataTableItemsVM(
			DataTable policyOtherGroupItemDataTableItems,
			PolicyOtherGroupItem policyOtherGroupItem,
			PolicyOtherGroupHeader policyOtherGroupHeader,
			PolicyGroup policyGroup
		)
		{
			PolicyOtherGroupItemDataTableItems = policyOtherGroupItemDataTableItems;
			PolicyOtherGroupItem = policyOtherGroupItem;
			PolicyOtherGroupHeader = policyOtherGroupHeader;
			PolicyGroup = policyGroup;
		}
	}
}