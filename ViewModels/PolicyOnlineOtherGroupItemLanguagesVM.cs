using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using System.Collections.Generic;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
	public class PolicyOnlineOtherGroupItemLanguagesVM : CWTBaseViewModel
	{

		public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyOnlineOtherGroupItemLanguages_v1Result> PolicyOnlineOtherGroupItemLanguages { get; set; }
		public PolicyOtherGroupHeader PolicyOtherGroupHeader { get; set; }
		public PolicyGroup PolicyGroup { get; set; }
		public bool HasWriteAccess { get; set; }

        public PolicyOnlineOtherGroupItemLanguagesVM()
        {
			HasWriteAccess = false;
        }

		public PolicyOnlineOtherGroupItemLanguagesVM(
			CWTPaginatedList<spDesktopDataAdmin_SelectPolicyOnlineOtherGroupItemLanguages_v1Result> policyOnlineOtherGroupItemLanguages,
			PolicyOtherGroupHeader policyOtherGroupHeader,
			PolicyGroup policyGroup)
        {
			PolicyOnlineOtherGroupItemLanguages = policyOnlineOtherGroupItemLanguages;
			PolicyOtherGroupHeader = policyOtherGroupHeader;
			PolicyGroup = policyGroup;
        }
    }
}