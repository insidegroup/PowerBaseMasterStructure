using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class PolicyCityGroupItemsVM : CWTBaseViewModel
    {
        public PolicyGroup PolicyGroup { get; set; }
        public bool HasWriteAccess { get; set; }
        public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyCityGroupItems_v1Result> PolicyCityGroupItems { get; set; }
        
        public PolicyCityGroupItemsVM()
        {
            HasWriteAccess = false; //default
        }
        public PolicyCityGroupItemsVM(PolicyGroup policyGroup, bool hasWriteAccess, CWTPaginatedList<spDesktopDataAdmin_SelectPolicyCityGroupItems_v1Result> policyCityGroupItems)
        {
            PolicyGroup = policyGroup;
            PolicyCityGroupItems = policyCityGroupItems;
            HasWriteAccess = hasWriteAccess;
        }
    }
}