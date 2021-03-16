using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using System.Collections.Generic;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
	public class PolicyAirOtherGroupItemsVM : CWTBaseViewModel
	{

		public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyAirOtherGroupItems_v1Result> PolicyAirVendorGroupItems { get; set; }
		public PolicyGroup PolicyGroup { get; set; }

        public PolicyAirOtherGroupItemsVM()
        {
          
        }

		public PolicyAirOtherGroupItemsVM(
			CWTPaginatedList<spDesktopDataAdmin_SelectPolicyAirOtherGroupItems_v1Result> policyAirVendorGroupItems,
			PolicyGroup policyGroup)
        {
			PolicyAirVendorGroupItems = policyAirVendorGroupItems;
			PolicyGroup = policyGroup;
        }
    }
}