using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using System.Collections.Generic;

namespace CWTDesktopDatabase.ViewModels
{
	[Bind(Exclude = "CreationTimestamp")]
	public class PolicyOtherGroupHeaderColumnNameVM : CWTBaseViewModel
	{
		public PolicyOtherGroupHeaderColumnName PolicyOtherGroupHeaderColumnName { get; set; }
		public PolicyOtherGroupHeaderTableName PolicyOtherGroupHeaderTableName { get; set; }
		public PolicyOtherGroupHeader PolicyOtherGroupHeader { get; set; }
		
        public PolicyOtherGroupHeaderColumnNameVM()
        {
          
        }

		public PolicyOtherGroupHeaderColumnNameVM(
			PolicyOtherGroupHeaderColumnName policyOtherGroupHeaderColumnName,
			PolicyOtherGroupHeaderTableName policyOtherGroupHeaderTableName,
			PolicyOtherGroupHeader policyOtherGroupHeader)
        {
			PolicyOtherGroupHeaderColumnName = policyOtherGroupHeaderColumnName;
			PolicyOtherGroupHeaderTableName = policyOtherGroupHeaderTableName;
			PolicyOtherGroupHeader = policyOtherGroupHeader;
        }
    }
}