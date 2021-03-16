using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.ViewModels
{
	public class PolicyOtherGroupHeaderColumnNamesVM : CWTBaseViewModel
	{
		public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyOtherGroupHeaderColumnNames_v1Result> PolicyOtherGroupHeaderColumnNames { get; set; }
		public PolicyOtherGroupHeader PolicyOtherGroupHeader { get; set; }
		public bool HasWriteAccess { get; set; }
 
        public PolicyOtherGroupHeaderColumnNamesVM()
        {
            HasWriteAccess = false;
        }
		public PolicyOtherGroupHeaderColumnNamesVM(
			PolicyOtherGroupHeader policyOtherGroupHeader,
			CWTPaginatedList<spDesktopDataAdmin_SelectPolicyOtherGroupHeaderColumnNames_v1Result> policyOtherGroupHeaderColumnNames, 
			bool hasWriteAccess)
        {
			PolicyOtherGroupHeader = policyOtherGroupHeader;
			PolicyOtherGroupHeaderColumnNames = policyOtherGroupHeaderColumnNames;
			HasWriteAccess = hasWriteAccess;
        }
    }
}