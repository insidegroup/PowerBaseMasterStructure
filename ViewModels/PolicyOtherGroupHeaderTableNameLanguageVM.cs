using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using System.Collections.Generic;

namespace CWTDesktopDatabase.ViewModels
{
	[Bind(Exclude = "CreationTimestamp")]
	public class PolicyOtherGroupHeaderTableNameLanguageVM : CWTBaseViewModel
	{
		public PolicyOtherGroupHeaderTableNameLanguage PolicyOtherGroupHeaderTableNameLanguage { get; set; }
		public PolicyOtherGroupHeaderTableName PolicyOtherGroupHeaderTableName { get; set; }
		public PolicyOtherGroupHeader PolicyOtherGroupHeader { get; set; }
		public IEnumerable<SelectListItem> Languages { get; set; }

        public PolicyOtherGroupHeaderTableNameLanguageVM()
        {
          
        }

		public PolicyOtherGroupHeaderTableNameLanguageVM(
			PolicyOtherGroupHeaderTableNameLanguage policyOtherGroupHeaderTableNameLanguage,
			PolicyOtherGroupHeaderTableName policyOtherGroupHeaderTableName,
			PolicyOtherGroupHeader policyOtherGroupHeader,
			IEnumerable<SelectListItem> languages)
        {
			PolicyOtherGroupHeaderTableNameLanguage = policyOtherGroupHeaderTableNameLanguage;
			PolicyOtherGroupHeaderTableName = policyOtherGroupHeaderTableName;
			PolicyOtherGroupHeader = policyOtherGroupHeader;
			Languages = languages;
        }
    }
}