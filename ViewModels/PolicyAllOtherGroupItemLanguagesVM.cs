using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using System.Collections.Generic;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
	public class PolicyAllOtherGroupItemLanguagesVM : CWTBaseViewModel
	{

		public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyAllOtherGroupItemLanguages_v1Result> PolicyAllVendorGroupItemLanguages { get; set; }
		public PolicyOtherGroupHeader PolicyOtherGroupHeader { get; set; }
		public PolicyGroup PolicyGroup { get; set; }
		public bool HasWriteAccess { get; set; }

        public PolicyAllOtherGroupItemLanguagesVM()
        {
			HasWriteAccess = false;
        }

		public PolicyAllOtherGroupItemLanguagesVM(
			CWTPaginatedList<spDesktopDataAdmin_SelectPolicyAllOtherGroupItemLanguages_v1Result> policyAllVendorGroupItemLanguages,
			PolicyOtherGroupHeader policyOtherGroupHeader,
			PolicyGroup policyGroup)
        {
			PolicyAllVendorGroupItemLanguages = policyAllVendorGroupItemLanguages;
			PolicyOtherGroupHeader = policyOtherGroupHeader;
			PolicyGroup = policyGroup;
        }
    }
}