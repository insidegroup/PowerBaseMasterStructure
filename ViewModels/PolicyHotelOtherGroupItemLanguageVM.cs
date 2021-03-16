using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using System.Collections.Generic;

namespace CWTDesktopDatabase.ViewModels
{
	[Bind(Exclude = "CreationTimestamp")]
	public class PolicyHotelOtherGroupItemLanguageVM : CWTBaseViewModel
	{
		public PolicyHotelOtherGroupItemLanguage PolicyHotelOtherGroupItemLanguage { get; set; }
		public PolicyOtherGroupHeader PolicyOtherGroupHeader { get; set; }
		public PolicyGroup PolicyGroup { get; set; }
		public IEnumerable<SelectListItem> Languages { get; set; }


        public PolicyHotelOtherGroupItemLanguageVM()
        {
          
        }

		public PolicyHotelOtherGroupItemLanguageVM(
			PolicyHotelOtherGroupItemLanguage policyHotelOtherGroupItemLanguage,
			PolicyOtherGroupHeader policyOtherGroupHeader,
			PolicyGroup policyGroup,
			IEnumerable<SelectListItem> languages)
		{
			PolicyHotelOtherGroupItemLanguage = policyHotelOtherGroupItemLanguage;
			PolicyOtherGroupHeader = policyOtherGroupHeader;
			PolicyGroup = policyGroup;
			Languages = languages;
		}
    }
}