using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using System.Collections.Generic;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
	public class PolicyHotelOtherGroupItemsVM : CWTBaseViewModel
	{

		public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyHotelOtherGroupItems_v1Result> PolicyHotelVendorGroupItems { get; set; }
		public PolicyGroup PolicyGroup { get; set; }

        public PolicyHotelOtherGroupItemsVM()
        {
          
        }

		public PolicyHotelOtherGroupItemsVM(
			CWTPaginatedList<spDesktopDataAdmin_SelectPolicyHotelOtherGroupItems_v1Result> policyHotelVendorGroupItems,
			PolicyGroup policyGroup)
        {
			PolicyHotelVendorGroupItems = policyHotelVendorGroupItems;
			PolicyGroup = policyGroup;
        }
    }
}