using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using System.Collections.Generic;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
	public class PolicyCarOtherGroupItemLanguagesVM : CWTBaseViewModel
	{

		public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyCarOtherGroupItemLanguages_v1Result> PolicyCarVendorGroupItemLanguages { get; set; }
		public PolicyOtherGroupHeader PolicyOtherGroupHeader { get; set; }
		public PolicyGroup PolicyGroup { get; set; }
		public bool HasWriteAccess { get; set; }

        public PolicyCarOtherGroupItemLanguagesVM()
        {
			HasWriteAccess = false;
        }

		public PolicyCarOtherGroupItemLanguagesVM(
			CWTPaginatedList<spDesktopDataAdmin_SelectPolicyCarOtherGroupItemLanguages_v1Result> policyCarVendorGroupItemLanguages,
			PolicyOtherGroupHeader policyOtherGroupHeader,
			PolicyGroup policyGroup)
        {
			PolicyCarVendorGroupItemLanguages = policyCarVendorGroupItemLanguages;
			PolicyOtherGroupHeader = policyOtherGroupHeader;
			PolicyGroup = policyGroup;
        }
    }
}