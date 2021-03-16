using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientFeeGroupMerchantFeesVM : CWTBaseViewModel
    {
        public ClientFeeGroup ClientFeeGroup { get; set; }
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientFeeGroupMerchantFees_v1Result> ClientFeeGroupMerchantFees { get; set; }
        public string FeeTypeDisplayName { get; set; }
        public string FeeTypeDisplayNameShort { get; set; }
        public int FeeTypeId { get; set; }
        public bool HasDomainWriteAccess { get; set; }

        public ClientFeeGroupMerchantFeesVM()
        {
            HasDomainWriteAccess = false;
            FeeTypeId = 4;
            FeeTypeDisplayName = "Mid Office Merchant Fee Item";
            FeeTypeDisplayNameShort = "MO Merchant Fee Item";
        }
        public ClientFeeGroupMerchantFeesVM(ClientFeeGroup clientFeeGroup, int feeTypeId, string feeTypeDisplayName, string feeTypeDisplayNameShort, bool hasDomainWriteAccess, CWTPaginatedList<spDesktopDataAdmin_SelectClientFeeGroupMerchantFees_v1Result> clientFeeGroupMerchantFees)
        {
            ClientFeeGroupMerchantFees = clientFeeGroupMerchantFees;
            ClientFeeGroup = clientFeeGroup;
            FeeTypeId = feeTypeId;
            FeeTypeDisplayName = feeTypeDisplayName;
            FeeTypeDisplayNameShort = feeTypeDisplayNameShort;
            HasDomainWriteAccess = hasDomainWriteAccess;
            
        }
    }
}