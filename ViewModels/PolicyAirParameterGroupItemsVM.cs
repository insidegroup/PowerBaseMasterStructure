using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class PolicyAirParameterGroupItemsVM : CWTBaseViewModel
    {
        public PolicyGroup PolicyGroup { get; set; }
        public bool HasWriteAccess { get; set; }
        public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyAirParameterGroupItems_v1Result>PolicyAirParameterGroupItems { get; set; }
        
        public PolicyAirParameterGroupItemsVM()
        {
            HasWriteAccess = false;
        }
        
		public PolicyAirParameterGroupItemsVM(PolicyGroup policyGroup, bool hasWriteAccess, CWTPaginatedList<spDesktopDataAdmin_SelectPolicyAirParameterGroupItems_v1Result> policyAirParameterGroupItems)
        {
            PolicyGroup = policyGroup;
			PolicyAirParameterGroupItems = policyAirParameterGroupItems;
            HasWriteAccess = hasWriteAccess;
        }
    }
}