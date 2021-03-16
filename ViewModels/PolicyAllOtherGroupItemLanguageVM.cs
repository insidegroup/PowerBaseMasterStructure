using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using System.Collections.Generic;

namespace CWTDesktopDatabase.ViewModels
{
	[Bind(Exclude = "CreationTimestamp")]
	public class PolicyAllOtherGroupItemLanguageVM : CWTBaseViewModel
	{
		public PolicyAllOtherGroupItemLanguage PolicyAllOtherGroupItemLanguage { get; set; }
		public PolicyOtherGroupHeader PolicyOtherGroupHeader { get; set; }
		public PolicyGroup PolicyGroup { get; set; }
		public IEnumerable<SelectListItem> Languages { get; set; }


        public PolicyAllOtherGroupItemLanguageVM()
        {
          
        }

		public PolicyAllOtherGroupItemLanguageVM(
			PolicyAllOtherGroupItemLanguage policyAllOtherGroupItemLanguage,
			PolicyOtherGroupHeader policyOtherGroupHeader,
			PolicyGroup policyGroup,
			IEnumerable<SelectListItem> languages)
		{
			PolicyAllOtherGroupItemLanguage = policyAllOtherGroupItemLanguage;
			PolicyOtherGroupHeader = policyOtherGroupHeader;
			PolicyGroup = policyGroup;
			Languages = languages;
		}
    }
}