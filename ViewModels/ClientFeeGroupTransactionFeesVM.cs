using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientFeeGroupTransactionFeesVM : CWTBaseViewModel
    {
        public ClientFeeGroup ClientFeeGroup { get; set; }
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientFeeGroupTransactionFees_v1Result> ClientFeeGroupTransactionFees { get; set; }
        public string FeeTypeDisplayName { get; set; }
        public string FeeTypeDisplayNameShort { get; set; }
        public int FeeTypeId { get; set; }
        public bool HasDomainWriteAccess { get; set; }

        public ClientFeeGroupTransactionFeesVM()
        {
            HasDomainWriteAccess = false;
            FeeTypeId = 3;
            FeeTypeDisplayName = "Mid Office Transaction Fee Item";
            FeeTypeDisplayNameShort = "MO Transaction Fee Item";
        }
        public ClientFeeGroupTransactionFeesVM(ClientFeeGroup clientFeeGroup, int feeTypeId, string feeTypeDisplayName, string feeTypeDisplayNameShort, bool hasDomainWriteAccess, CWTPaginatedList<spDesktopDataAdmin_SelectClientFeeGroupTransactionFees_v1Result> clientFeeGroupTransactionFees)
        {
            ClientFeeGroupTransactionFees = clientFeeGroupTransactionFees;
            ClientFeeGroup = clientFeeGroup;
            FeeTypeId = feeTypeId;
            FeeTypeDisplayName = feeTypeDisplayName;
            FeeTypeDisplayNameShort = feeTypeDisplayNameShort;
            HasDomainWriteAccess = hasDomainWriteAccess;
            
        }
    }
}