using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using System.Collections.Generic;

namespace CWTDesktopDatabase.ViewModels
{
	[Bind(Exclude = "CreationTimestamp")]
	public class PolicyOnlineOtherGroupItemLanguageVM : CWTBaseViewModel
	{
		public PolicyOnlineOtherGroupItemLanguage PolicyOnlineOtherGroupItemLanguage { get; set; }
		public PolicyOtherGroupHeader PolicyOtherGroupHeader { get; set; }
		public PolicyGroup PolicyGroup { get; set; }
		public IEnumerable<SelectListItem> Languages { get; set; }


        public PolicyOnlineOtherGroupItemLanguageVM()
        {
          
        }

		public PolicyOnlineOtherGroupItemLanguageVM(
			PolicyOnlineOtherGroupItemLanguage PolicyOnlineOtherGroupItemLanguage,
			PolicyOtherGroupHeader policyOtherGroupHeader,
			PolicyGroup policyGroup,
			IEnumerable<SelectListItem> languages)
		{
			PolicyOnlineOtherGroupItemLanguage = PolicyOnlineOtherGroupItemLanguage;
			PolicyOtherGroupHeader = policyOtherGroupHeader;
			PolicyGroup = policyGroup;
			Languages = languages;
		}
    }
}