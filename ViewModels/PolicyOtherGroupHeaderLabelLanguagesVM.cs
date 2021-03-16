using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.ViewModels
{
	public class PolicyOtherGroupHeaderLabelLanguagesVM : CWTBaseViewModel
	{
		public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyOtherGroupHeaderLabelLanguages_v1Result> PolicyOtherGroupHeaderLabelLanguages { get; set; }
		public PolicyOtherGroupHeader PolicyOtherGroupHeader { get; set; }
        public bool HasWriteAccess { get; set; }
 
        public PolicyOtherGroupHeaderLabelLanguagesVM()
        {
            HasWriteAccess = false;
        }

		public PolicyOtherGroupHeaderLabelLanguagesVM(
			PolicyOtherGroupHeader policyOtherGroupHeader,
			CWTPaginatedList<spDesktopDataAdmin_SelectPolicyOtherGroupHeaderLabelLanguages_v1Result> policyOtherGroupHeaderLabelLanguages, 
			bool hasWriteAccess)
        {
			PolicyOtherGroupHeader = policyOtherGroupHeader;
			PolicyOtherGroupHeaderLabelLanguages = policyOtherGroupHeaderLabelLanguages;
			HasWriteAccess = hasWriteAccess;
        }
    }
}