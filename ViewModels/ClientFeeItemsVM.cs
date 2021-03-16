using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientFeeItemsVM : CWTBaseViewModel
   {
        public ClientFeeGroup ClientFeeGroup { get; set; }
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientFeeGroupClientFeeItems_v1Result> ClientFeeItems { get; set; }
        public string FeeTypeDisplayName { get; set; }
        public FeeType FeeType { get; set; }
        public bool HasWriteAccess { get; set; }
 
        public ClientFeeItemsVM()
        {
            HasWriteAccess = false;
        }
        public ClientFeeItemsVM(ClientFeeGroup clientFeeGroup, CWTPaginatedList<spDesktopDataAdmin_SelectClientFeeGroupClientFeeItems_v1Result> clientFeeItems, FeeType feeType, string feeTypeDisplayName, bool hasWriteAccess)
        {
            ClientFeeGroup = clientFeeGroup;
            ClientFeeItems = clientFeeItems;
            FeeType = feeType;
            FeeTypeDisplayName = feeTypeDisplayName;
            HasWriteAccess = hasWriteAccess;
        }
    }
}