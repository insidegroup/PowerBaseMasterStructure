using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Web.Mvc;
using System.Collections.Generic;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientFeeGroupTransactionFeeAirVM : CWTBaseViewModel
    {

        public TransactionFeeClientFeeGroup TransactionFeeClientFeeGroup { get; set; }
        public ClientFeeGroup ClientFeeGroup { get; set; }
        public TransactionFeeAir TransactionFeeAir { get; set; }
        public PolicyRouting PolicyRouting { get; set; }
        public string FeeTypeDisplayName { get; set; }
        public string FeeTypeDisplayNameShort { get; set; }
        public int FeeTypeId { get; set; }

        public CWTPaginatedList<spDesktopDataAdmin_SelectClientDetailContacts_v1Result> Contacts { get; set; }

        public ClientFeeGroupTransactionFeeAirVM()
        {
            FeeTypeId = 4;
            FeeTypeDisplayName = "Mid Office Transaction Fee Item";
            FeeTypeDisplayNameShort = "MO Transaction Fee Item";
        }
        public ClientFeeGroupTransactionFeeAirVM(TransactionFeeClientFeeGroup transactionFeeClientFeeGroup, ClientFeeGroup clientFeeGroup, TransactionFeeAir transactionFeeAir, PolicyRouting policyRouting, int feeTypeId, string feeTypeDisplayName, string feeTypeDisplayNameShort)
        {
            TransactionFeeClientFeeGroup = transactionFeeClientFeeGroup;
            ClientFeeGroup = clientFeeGroup;
            TransactionFeeAir = transactionFeeAir;
            PolicyRouting = policyRouting;
            FeeTypeId = feeTypeId;
            FeeTypeDisplayName = feeTypeDisplayName;
            FeeTypeDisplayNameShort = feeTypeDisplayNameShort;
        }
    }
}