using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class PolicyMessageGroupItemsVM : CWTBaseViewModel
    {
        public PolicyGroup PolicyGroup { get; set; }
        public bool HasWriteAccess { get; set; }
        public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyMessageGroupItems_v1Result> PolicyMessageGroupItems { get; set; }
        
        public PolicyMessageGroupItemsVM()
        {
            HasWriteAccess = false; //default
        }
        public PolicyMessageGroupItemsVM(PolicyGroup policyGroup, bool hasWriteAccess, CWTPaginatedList<spDesktopDataAdmin_SelectPolicyMessageGroupItems_v1Result> policyMessageGroupItems)
        {
            PolicyGroup = policyGroup;
            PolicyMessageGroupItems = policyMessageGroupItems;
            HasWriteAccess = hasWriteAccess;
        }
    }
}