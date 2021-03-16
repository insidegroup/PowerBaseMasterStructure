using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using System.Collections.Generic;

namespace CWTDesktopDatabase.ViewModels
{
	[Bind(Exclude = "CreationTimestamp")]
	public class PolicyOtherGroupHeaderLabelLanguageVM : CWTBaseViewModel
	{
		public PolicyOtherGroupHeaderLabelLanguage PolicyOtherGroupHeaderLabelLanguage { get; set; }
		public PolicyOtherGroupHeaderLabel PolicyOtherGroupHeaderLabel { get; set; }
		public PolicyOtherGroupHeader PolicyOtherGroupHeader { get; set; }
		public IEnumerable<SelectListItem> Languages { get; set; }

        public PolicyOtherGroupHeaderLabelLanguageVM()
        {
          
        }

		public PolicyOtherGroupHeaderLabelLanguageVM(
			PolicyOtherGroupHeaderLabelLanguage policyOtherGroupHeaderLabelLanguage,
			PolicyOtherGroupHeaderLabel policyOtherGroupHeaderLabel,
			PolicyOtherGroupHeader policyOtherGroupHeader,
			IEnumerable<SelectListItem> languages)
        {
			PolicyOtherGroupHeaderLabelLanguage = policyOtherGroupHeaderLabelLanguage;
			PolicyOtherGroupHeaderLabel = policyOtherGroupHeaderLabel;
			PolicyOtherGroupHeader = policyOtherGroupHeader;
			Languages = languages;
        }
    }
}