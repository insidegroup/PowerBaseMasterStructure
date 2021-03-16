using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class PolicyAirVendorGroupItemsVM : CWTBaseViewModel
    {
        public PolicyGroup PolicyGroup { get; set; }
        public bool HasWriteAccess { get; set; }
        public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyAirVendorGroupItems_v1Result> PolicyAirVendorGroupItems { get; set; }
        
        public PolicyAirVendorGroupItemsVM()
        {
            HasWriteAccess = false;
        }
        public PolicyAirVendorGroupItemsVM(PolicyGroup policyGroup, bool hasWriteAccess, CWTPaginatedList<spDesktopDataAdmin_SelectPolicyAirVendorGroupItems_v1Result> policyAirVendorGroupItems)
        {
            PolicyGroup = policyGroup;
            PolicyAirVendorGroupItems = policyAirVendorGroupItems;
            HasWriteAccess = hasWriteAccess;
        }
    }
}