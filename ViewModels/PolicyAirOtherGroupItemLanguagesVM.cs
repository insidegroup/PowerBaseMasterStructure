using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using System.Collections.Generic;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
	public class PolicyAirOtherGroupItemLanguagesVM : CWTBaseViewModel
	{

		public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyAirOtherGroupItemLanguages_v1Result> PolicyAirVendorGroupItemLanguages { get; set; }
		public PolicyOtherGroupHeader PolicyOtherGroupHeader { get; set; }
		public PolicyGroup PolicyGroup { get; set; }
		public bool HasWriteAccess { get; set; }

        public PolicyAirOtherGroupItemLanguagesVM()
        {
			HasWriteAccess = false;
        }

		public PolicyAirOtherGroupItemLanguagesVM(
			CWTPaginatedList<spDesktopDataAdmin_SelectPolicyAirOtherGroupItemLanguages_v1Result> policyAirVendorGroupItemLanguages,
			PolicyOtherGroupHeader policyOtherGroupHeader,
			PolicyGroup policyGroup)
        {
			PolicyAirVendorGroupItemLanguages = policyAirVendorGroupItemLanguages;
			PolicyOtherGroupHeader = policyOtherGroupHeader;
			PolicyGroup = policyGroup;
        }
    }
}