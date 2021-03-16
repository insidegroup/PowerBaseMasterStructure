using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using System.Collections.Generic;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
	public class PolicyAllOtherGroupItemsVM : CWTBaseViewModel
	{

		public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyAllOtherGroupItems_v1Result> PolicyAllVendorGroupItems { get; set; }
		public PolicyGroup PolicyGroup { get; set; }

        public PolicyAllOtherGroupItemsVM()
        {
          
        }

		public PolicyAllOtherGroupItemsVM(
			CWTPaginatedList<spDesktopDataAdmin_SelectPolicyAllOtherGroupItems_v1Result> policyAllVendorGroupItems,
			PolicyGroup policyGroup)
        {
			PolicyAllVendorGroupItems = policyAllVendorGroupItems;
			PolicyGroup = policyGroup;
        }
    }
}