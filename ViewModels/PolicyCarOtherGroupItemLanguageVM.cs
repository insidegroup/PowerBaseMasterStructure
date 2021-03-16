using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using System.Collections.Generic;

namespace CWTDesktopDatabase.ViewModels
{
	[Bind(Exclude = "CreationTimestamp")]
	public class PolicyCarOtherGroupItemLanguageVM : CWTBaseViewModel
	{
		public PolicyCarOtherGroupItemLanguage PolicyCarOtherGroupItemLanguage { get; set; }
		public PolicyOtherGroupHeader PolicyOtherGroupHeader { get; set; }
		public PolicyGroup PolicyGroup { get; set; }
		public IEnumerable<SelectListItem> Languages { get; set; }


        public PolicyCarOtherGroupItemLanguageVM()
        {
          
        }

		public PolicyCarOtherGroupItemLanguageVM(
			PolicyCarOtherGroupItemLanguage policyCarOtherGroupItemLanguage,
			PolicyOtherGroupHeader policyOtherGroupHeader,
			PolicyGroup policyGroup,
			IEnumerable<SelectListItem> languages)
		{
			PolicyCarOtherGroupItemLanguage = policyCarOtherGroupItemLanguage;
			PolicyOtherGroupHeader = policyOtherGroupHeader;
			PolicyGroup = policyGroup;
			Languages = languages;
		}
    }
}