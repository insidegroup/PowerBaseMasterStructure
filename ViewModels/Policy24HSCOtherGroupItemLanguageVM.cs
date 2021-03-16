using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using System.Collections.Generic;

namespace CWTDesktopDatabase.ViewModels
{
	[Bind(Exclude = "CreationTimestamp")]
	public class Policy24HSCOtherGroupItemLanguageVM : CWTBaseViewModel
	{
		public Policy24HSCOtherGroupItemLanguage Policy24HSCOtherGroupItemLanguage { get; set; }
		public PolicyOtherGroupHeader PolicyOtherGroupHeader { get; set; }
		public PolicyGroup PolicyGroup { get; set; }
		public IEnumerable<SelectListItem> Languages { get; set; }


        public Policy24HSCOtherGroupItemLanguageVM()
        {
          
        }

		public Policy24HSCOtherGroupItemLanguageVM(
			Policy24HSCOtherGroupItemLanguage policy24HSCOtherGroupItemLanguage,
			PolicyOtherGroupHeader policyOtherGroupHeader,
			PolicyGroup policyGroup,
			IEnumerable<SelectListItem> languages)
		{
			Policy24HSCOtherGroupItemLanguage = policy24HSCOtherGroupItemLanguage;
			PolicyOtherGroupHeader = policyOtherGroupHeader;
			PolicyGroup = policyGroup;
			Languages = languages;
		}
    }
}