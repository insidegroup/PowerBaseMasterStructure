using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using System.Collections.Generic;

namespace CWTDesktopDatabase.ViewModels
{
	[Bind(Exclude = "CreationTimestamp")]
	public class PolicyAirOtherGroupItemLanguageVM : CWTBaseViewModel
	{
		public PolicyAirOtherGroupItemLanguage PolicyAirOtherGroupItemLanguage { get; set; }
		public PolicyOtherGroupHeader PolicyOtherGroupHeader { get; set; }
		public PolicyGroup PolicyGroup { get; set; }
		public IEnumerable<SelectListItem> Languages { get; set; }


        public PolicyAirOtherGroupItemLanguageVM()
        {
          
        }

		public PolicyAirOtherGroupItemLanguageVM(
			PolicyAirOtherGroupItemLanguage policyAirOtherGroupItemLanguage,
			PolicyOtherGroupHeader policyOtherGroupHeader,
			PolicyGroup policyGroup,
			IEnumerable<SelectListItem> languages)
		{
			PolicyAirOtherGroupItemLanguage = policyAirOtherGroupItemLanguage;
			PolicyOtherGroupHeader = policyOtherGroupHeader;
			PolicyGroup = policyGroup;
			Languages = languages;
		}
    }
}