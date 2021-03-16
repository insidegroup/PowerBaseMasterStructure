using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class PolicyAirMissedSavingsThresholdGroupItemsVM : CWTBaseViewModel
    {
        public PolicyGroup PolicyGroup { get; set; }
        public bool HasWriteAccess { get; set; }
        public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyAirMissedSavingsThresholdGroupItems_v1Result> PolicyAirMissedSavingsThresholdGroupItems { get; set; }
        
        public PolicyAirMissedSavingsThresholdGroupItemsVM()
        {
            HasWriteAccess = false; //default
        }
        public PolicyAirMissedSavingsThresholdGroupItemsVM(PolicyGroup policyGroup, bool hasWriteAccess, CWTPaginatedList<spDesktopDataAdmin_SelectPolicyAirMissedSavingsThresholdGroupItems_v1Result> policyAirMissedSavingsThresholdGroupItems)
        {
            PolicyGroup = policyGroup;
            PolicyAirMissedSavingsThresholdGroupItems = policyAirMissedSavingsThresholdGroupItems;
            HasWriteAccess = hasWriteAccess;
        }
    }
}