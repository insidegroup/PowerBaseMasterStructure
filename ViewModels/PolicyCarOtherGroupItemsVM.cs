using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using System.Collections.Generic;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
	public class PolicyCarOtherGroupItemsVM : CWTBaseViewModel
	{

		public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyCarOtherGroupItems_v1Result> PolicyCarVendorGroupItems { get; set; }
		public PolicyGroup PolicyGroup { get; set; }

        public PolicyCarOtherGroupItemsVM()
        {
          
        }

		public PolicyCarOtherGroupItemsVM(
			CWTPaginatedList<spDesktopDataAdmin_SelectPolicyCarOtherGroupItems_v1Result> policyCarVendorGroupItems,
			PolicyGroup policyGroup)
        {
			PolicyCarVendorGroupItems = policyCarVendorGroupItems;
			PolicyGroup = policyGroup;
        }
    }
}