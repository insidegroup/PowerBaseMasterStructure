using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using System.Collections.Generic;

namespace CWTDesktopDatabase.ViewModels
{
	[Bind(Exclude = "CreationTimestamp")]
	public class PolicyAirParameterGroupItemLanguageVM : CWTBaseViewModel
	{
		public PolicyAirParameterGroupItemLanguage PolicyAirParameterGroupItemLanguage { get; set; }
		public PolicyAirParameterGroupItem PolicyAirParameterGroupItem { get; set; }
		public PolicyGroup PolicyGroup { get; set; }
		public IEnumerable<SelectListItem> Languages { get; set; }


        public PolicyAirParameterGroupItemLanguageVM()
        {
          
        }

		public PolicyAirParameterGroupItemLanguageVM(
			PolicyAirParameterGroupItemLanguage policyAirParameterGroupItemLanguage,
			PolicyAirParameterGroupItem policyAirParameterGroupItem,
			PolicyGroup policyGroup,
			IEnumerable<SelectListItem> languages)
		{
			PolicyAirParameterGroupItemLanguage = policyAirParameterGroupItemLanguage;
			PolicyAirParameterGroupItem = policyAirParameterGroupItem;
			PolicyGroup = policyGroup;
			Languages = languages;
		}
    }
}