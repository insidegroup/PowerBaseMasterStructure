using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using System.Collections.Generic;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
	public class PolicyPriceTrackingOtherGroupItemsVM : CWTBaseViewModel
	{

		public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyPriceTrackingOtherGroupItems_v1Result> PolicyPriceTrackingOtherGroupItems { get; set; }
		public PolicyGroup PolicyGroup { get; set; }

        public PolicyPriceTrackingOtherGroupItemsVM()
        {
          
        }

		public PolicyPriceTrackingOtherGroupItemsVM(
			CWTPaginatedList<spDesktopDataAdmin_SelectPolicyPriceTrackingOtherGroupItems_v1Result> policyPriceTrackingOtherGroupItems,
			PolicyGroup policyGroup)
        {
			PolicyPriceTrackingOtherGroupItems = policyPriceTrackingOtherGroupItems;
			PolicyGroup = policyGroup;
        }
    }
}