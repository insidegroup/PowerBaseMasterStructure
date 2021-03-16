using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using System.Collections.Generic;

namespace CWTDesktopDatabase.ViewModels
{
	[Bind(Exclude = "CreationTimestamp")]
	public class PolicyPriceTrackingOtherGroupItemLanguageVM : CWTBaseViewModel
	{
		public PolicyPriceTrackingOtherGroupItemLanguage PolicyPriceTrackingOtherGroupItemLanguage { get; set; }
		public PolicyOtherGroupHeader PolicyOtherGroupHeader { get; set; }
		public PolicyGroup PolicyGroup { get; set; }
		public IEnumerable<SelectListItem> Languages { get; set; }


        public PolicyPriceTrackingOtherGroupItemLanguageVM()
        {
          
        }

		public PolicyPriceTrackingOtherGroupItemLanguageVM(
			PolicyPriceTrackingOtherGroupItemLanguage policyPriceTrackingOtherGroupItemLanguage,
			PolicyOtherGroupHeader policyOtherGroupHeader,
			PolicyGroup policyGroup,
			IEnumerable<SelectListItem> languages)
		{
			PolicyPriceTrackingOtherGroupItemLanguage = policyPriceTrackingOtherGroupItemLanguage;
			PolicyOtherGroupHeader = policyOtherGroupHeader;
			PolicyGroup = policyGroup;
			Languages = languages;
		}
    }
}