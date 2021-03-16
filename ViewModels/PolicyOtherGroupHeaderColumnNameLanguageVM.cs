using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using System.Collections.Generic;

namespace CWTDesktopDatabase.ViewModels
{
	[Bind(Exclude = "CreationTimestamp")]
	public class PolicyOtherGroupHeaderColumnNameLanguageVM : CWTBaseViewModel
	{
		public PolicyOtherGroupHeaderColumnNameLanguage PolicyOtherGroupHeaderColumnNameLanguage { get; set; }
		public PolicyOtherGroupHeader PolicyOtherGroupHeader { get; set; }
		public PolicyOtherGroupHeaderColumnName PolicyOtherGroupHeaderColumnName { get; set; }
		public PolicyOtherGroupHeaderTableName PolicyOtherGroupHeaderTableName { get; set; }
		public IEnumerable<SelectListItem> Languages { get; set; }

        public PolicyOtherGroupHeaderColumnNameLanguageVM()
        {
          
        }

		public PolicyOtherGroupHeaderColumnNameLanguageVM(
			PolicyOtherGroupHeaderColumnNameLanguage policyOtherGroupHeaderColumnNameLanguage,
			PolicyOtherGroupHeaderColumnName policyOtherGroupHeaderColumnName,
			PolicyOtherGroupHeaderTableName policyOtherGroupHeaderTableName,
			PolicyOtherGroupHeader policyOtherGroupHeader,
			IEnumerable<SelectListItem> languages)
        {
			PolicyOtherGroupHeaderColumnName = policyOtherGroupHeaderColumnName;
			PolicyOtherGroupHeaderTableName = policyOtherGroupHeaderTableName;
			PolicyOtherGroupHeaderColumnNameLanguage = policyOtherGroupHeaderColumnNameLanguage;
			PolicyOtherGroupHeader = policyOtherGroupHeader;
			Languages = languages;
        }
    }
}