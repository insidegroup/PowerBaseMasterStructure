using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.ViewModels
{
	public class PolicyOtherGroupHeaderColumnNameLanguagesVM : CWTBaseViewModel
	{
		public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyOtherGroupHeaderColumnNameLanguages_v1Result> PolicyOtherGroupHeaderColumnNameLanguages { get; set; }
		public PolicyOtherGroupHeader PolicyOtherGroupHeader { get; set; }
		public PolicyOtherGroupHeaderColumnName PolicyOtherGroupHeaderColumnName { get; set; }
		public PolicyOtherGroupHeaderTableName PolicyOtherGroupHeaderTableName { get; set; }
		public bool HasWriteAccess { get; set; }
 
        public PolicyOtherGroupHeaderColumnNameLanguagesVM()
        {
            HasWriteAccess = false;
        }

		public PolicyOtherGroupHeaderColumnNameLanguagesVM(
			PolicyOtherGroupHeader policyOtherGroupHeader,
			CWTPaginatedList<spDesktopDataAdmin_SelectPolicyOtherGroupHeaderColumnNameLanguages_v1Result> policyOtherGroupHeaderColumnNameLanguages,
			PolicyOtherGroupHeaderColumnName policyOtherGroupHeaderColumnName,
			PolicyOtherGroupHeaderTableName policyOtherGroupHeaderTableName,
			bool hasWriteAccess)
        {
			PolicyOtherGroupHeader = policyOtherGroupHeader;
			PolicyOtherGroupHeaderColumnName = policyOtherGroupHeaderColumnName;
			PolicyOtherGroupHeaderTableName = policyOtherGroupHeaderTableName;
			PolicyOtherGroupHeaderColumnNameLanguages = policyOtherGroupHeaderColumnNameLanguages;
			HasWriteAccess = hasWriteAccess;
        }
    }
}