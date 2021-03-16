using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using System.Collections.Generic;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
	public class PolicyAirParameterGroupItemLanguagesVM : CWTBaseViewModel
	{

		public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyAirParameterGroupItemLanguages_v1Result> PolicyAirParameterGroupItemLanguages { get; set; }
		public PolicyAirParameterGroupItem PolicyAirParameterGroupItem { get; set; }
		public PolicyGroup PolicyGroup { get; set; }
		public bool HasWriteAccess { get; set; }

        public PolicyAirParameterGroupItemLanguagesVM()
        {
			HasWriteAccess = false;
        }

		public PolicyAirParameterGroupItemLanguagesVM(
			CWTPaginatedList<spDesktopDataAdmin_SelectPolicyAirParameterGroupItemLanguages_v1Result> policyAirParameterGroupItemLanguages,
			PolicyAirParameterGroupItem policyAirParameterGroupItem,
			PolicyGroup policyGroup)
        {
			PolicyAirParameterGroupItemLanguages = policyAirParameterGroupItemLanguages;
			PolicyAirParameterGroupItem = policyAirParameterGroupItem;
			PolicyGroup = policyGroup;
        }
    }
}