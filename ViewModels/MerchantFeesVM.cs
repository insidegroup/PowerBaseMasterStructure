using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class MerchantFeesVM : CWTBaseViewModel
    {
        public CWTPaginatedList<spDesktopDataAdmin_SelectMerchantFees_v1Result> MerchantFees { get; set; }
        public bool HasWriteAccess { get; set; }

        public MerchantFeesVM()
        {
            HasWriteAccess = false;
        }
        public MerchantFeesVM(CWTPaginatedList<spDesktopDataAdmin_SelectMerchantFees_v1Result> merchantFees, bool hasWriteAccess)
        {
            MerchantFees = merchantFees;
            HasWriteAccess = hasWriteAccess;
        }
    }
}