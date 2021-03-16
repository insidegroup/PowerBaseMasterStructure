using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Web.Mvc;
using System.Collections.Generic;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientFeeGroupMerchantFeeVM : CWTBaseViewModel
    {

        public MerchantFeeClientFeeGroup MerchantFeeClientFeeGroup { get; set; }
        public ClientFeeGroup ClientFeeGroup { get; set; }
        public IEnumerable<SelectListItem> MerchantFees { get; set; }
        public MerchantFee MerchantFee { get; set; }
        public int OriginalMerchantFeeId { get; set; }
        public string FeeTypeDisplayName { get; set; }
        public string FeeTypeDisplayNameShort { get; set; }
        public int FeeTypeId { get; set; }
        public bool HasDomainWriteAccess { get; set; }

        public CWTPaginatedList<spDesktopDataAdmin_SelectClientDetailContacts_v1Result> Contacts { get; set; }

        public ClientFeeGroupMerchantFeeVM()
        {
            HasDomainWriteAccess = false;
            FeeTypeId = 4;
            FeeTypeDisplayName = "Mid Office Merchant Fee Item";
            FeeTypeDisplayNameShort = "MO Merchant Fee Item";
        }
        public ClientFeeGroupMerchantFeeVM(int originalMerchantFeeId, IEnumerable<SelectListItem> merchantFees, MerchantFeeClientFeeGroup merchantFeeClientFeeGroup, ClientFeeGroup clientFeeGroup, MerchantFee merchantFee, int feeTypeId, string feeTypeDisplayName, string feeTypeDisplayNameShort, bool hasDomainWriteAccess)
        {
            MerchantFeeClientFeeGroup = merchantFeeClientFeeGroup;
            ClientFeeGroup = clientFeeGroup;
            MerchantFees = merchantFees;
            MerchantFee = merchantFee;
            FeeTypeId = feeTypeId;
            FeeTypeDisplayName = feeTypeDisplayName;
            FeeTypeDisplayNameShort = feeTypeDisplayNameShort;
            HasDomainWriteAccess = hasDomainWriteAccess;
            OriginalMerchantFeeId = originalMerchantFeeId;
        }
    }
}