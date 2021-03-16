using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using System.Collections.Generic;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
	public class PolicyPriceTrackingOtherGroupItemLanguagesVM : CWTBaseViewModel
	{

		public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyPriceTrackingOtherGroupItemLanguages_v1Result> PolicyPriceTrackingOtherGroupItemLanguages { get; set; }
		public PolicyOtherGroupHeader PolicyOtherGroupHeader { get; set; }
		public PolicyGroup PolicyGroup { get; set; }
		public bool HasWriteAccess { get; set; }

        public PolicyPriceTrackingOtherGroupItemLanguagesVM()
        {
			HasWriteAccess = false;
        }

		public PolicyPriceTrackingOtherGroupItemLanguagesVM(
			CWTPaginatedList<spDesktopDataAdmin_SelectPolicyPriceTrackingOtherGroupItemLanguages_v1Result> policyPriceTrackingOtherGroupItemLanguages,
			PolicyOtherGroupHeader policyOtherGroupHeader,
			PolicyGroup policyGroup)
        {
			PolicyPriceTrackingOtherGroupItemLanguages = policyPriceTrackingOtherGroupItemLanguages;
			PolicyOtherGroupHeader = policyOtherGroupHeader;
			PolicyGroup = policyGroup;
        }
    }
}