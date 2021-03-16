using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.ViewModels
{
	public class PolicyOtherGroupHeaderTableNameLanguagesVM : CWTBaseViewModel
	{
		public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyOtherGroupHeaderTableNameLanguages_v1Result> PolicyOtherGroupHeaderTableNameLanguages { get; set; }
		public PolicyOtherGroupHeader PolicyOtherGroupHeader { get; set; }
        public bool HasWriteAccess { get; set; }
 
        public PolicyOtherGroupHeaderTableNameLanguagesVM()
        {
            HasWriteAccess = false;
        }

		public PolicyOtherGroupHeaderTableNameLanguagesVM(
			PolicyOtherGroupHeader policyOtherGroupHeader,
			CWTPaginatedList<spDesktopDataAdmin_SelectPolicyOtherGroupHeaderTableNameLanguages_v1Result> policyOtherGroupHeaderTableNameLanguages, 
			bool hasWriteAccess)
        {
			PolicyOtherGroupHeader = policyOtherGroupHeader;
			PolicyOtherGroupHeaderTableNameLanguages = policyOtherGroupHeaderTableNameLanguages;
			HasWriteAccess = hasWriteAccess;
        }
    }
}