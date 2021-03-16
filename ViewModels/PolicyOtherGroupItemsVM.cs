using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using System.Collections.Generic;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
	public class PolicyOtherGroupItemsVM : CWTBaseViewModel
	{

		public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyOtherGroupItems_v1Result> PolicyVendorGroupItems { get; set; }
		public PolicyGroup PolicyGroup { get; set; }

        public PolicyOtherGroupItemsVM()
        {
          
        }

		public PolicyOtherGroupItemsVM(
			CWTPaginatedList<spDesktopDataAdmin_SelectPolicyOtherGroupItems_v1Result> policyVendorGroupItems,
			PolicyGroup policyGroup)
        {
			PolicyVendorGroupItems = policyVendorGroupItems;
			PolicyGroup = policyGroup;
        }
    }
}