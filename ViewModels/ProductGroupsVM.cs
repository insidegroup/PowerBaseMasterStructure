using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ProductGroupsVM : CWTBaseViewModel
   {
        public CWTPaginatedList<spDesktopDataAdmin_SelectProductGroups_v1Result> ProductGroups { get; set; }
        public CWTPaginatedList<spDesktopDataAdmin_SelectProductGroupsOrphaned_v1Result> ProductGroupsOrphaned { get; set; }
        public bool HasDomainWriteAccess { get; set; }
 
        public ProductGroupsVM()
        {
            HasDomainWriteAccess = false;
        }
        public ProductGroupsVM(CWTPaginatedList<spDesktopDataAdmin_SelectProductGroups_v1Result> productGroups, 
            CWTPaginatedList<spDesktopDataAdmin_SelectProductGroupsOrphaned_v1Result> productGroupsOrphaned, 
            int feeTypeId, string feeTypeDisplayName, string feeTypeDisplayNameShort, bool hasDomainWriteAccess)
        {
            ProductGroups = productGroups;
            ProductGroupsOrphaned = productGroupsOrphaned;
            HasDomainWriteAccess = hasDomainWriteAccess;
        }
    }
}