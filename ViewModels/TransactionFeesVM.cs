using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class TransactionFeesVM : CWTBaseViewModel
    {
        public CWTPaginatedList<spDesktopDataAdmin_SelectTransactionFees_v1Result> TransactionFees { get; set; }
        public bool HasWriteAccess { get; set; }

        public TransactionFeesVM()
        {
            HasWriteAccess = false;
        }
        public TransactionFeesVM(CWTPaginatedList<spDesktopDataAdmin_SelectTransactionFees_v1Result> transactionFees, bool hasWriteAccess)
        {
            TransactionFees = transactionFees;
            HasWriteAccess = hasWriteAccess;
        }
    }
}