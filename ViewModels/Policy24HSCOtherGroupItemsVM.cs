using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using System.Collections.Generic;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
	public class Policy24HSCOtherGroupItemsVM : CWTBaseViewModel
	{

		public CWTPaginatedList<spDesktopDataAdmin_SelectPolicy24HSCOtherGroupItems_v1Result> Policy24HSCVendorGroupItems { get; set; }
		public PolicyGroup PolicyGroup { get; set; }

        public Policy24HSCOtherGroupItemsVM()
        {
          
        }

		public Policy24HSCOtherGroupItemsVM(
			CWTPaginatedList<spDesktopDataAdmin_SelectPolicy24HSCOtherGroupItems_v1Result> policy24HSCVendorGroupItems,
			PolicyGroup policyGroup)
        {
			Policy24HSCVendorGroupItems = policy24HSCVendorGroupItems;
			PolicyGroup = policyGroup;
        }
    }
}