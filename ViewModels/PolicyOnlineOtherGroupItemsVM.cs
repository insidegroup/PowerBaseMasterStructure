using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using System.Collections.Generic;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
	public class PolicyOnlineOtherGroupItemsVM : CWTBaseViewModel
	{

		public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyOnlineOtherGroupItems_v1Result> PolicyOnlineOtherGroupItems { get; set; }
		public PolicyGroup PolicyGroup { get; set; }

        public PolicyOnlineOtherGroupItemsVM()
        {
          
        }

		public PolicyOnlineOtherGroupItemsVM(
			CWTPaginatedList<spDesktopDataAdmin_SelectPolicyOnlineOtherGroupItems_v1Result> policyOnlineOtherGroupItems,
			PolicyGroup policyGroup)
        {
			PolicyOnlineOtherGroupItems = policyOnlineOtherGroupItems;
			PolicyGroup = policyGroup;
        }
    }
}