using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Web.Mvc;
using System.Collections.Generic;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientFeeGroupTransactionFeeVM : CWTBaseViewModel
    {

        public TransactionFeeClientFeeGroup TransactionFeeClientFeeGroup { get; set; }
        public ClientFeeGroup ClientFeeGroup { get; set; }
        public IEnumerable<SelectListItem> Products { get; set; }
        public IEnumerable<SelectListItem> TransactionFees { get; set; }
        public TransactionFee TransactionFee { get; set; }
        public int OriginalTransactionFeeId { get; set; }
        public string FeeTypeDisplayName { get; set; }
        public string FeeTypeDisplayNameShort { get; set; }
        public int FeeTypeId { get; set; }
        public bool HasDomainWriteAccess { get; set; }

        public CWTPaginatedList<spDesktopDataAdmin_SelectClientDetailContacts_v1Result> Contacts { get; set; }

        public ClientFeeGroupTransactionFeeVM()
        {
            HasDomainWriteAccess = false;
            FeeTypeId = 4;
            FeeTypeDisplayName = "Mid Office Transaction Fee Item";
            FeeTypeDisplayNameShort = "MO Transaction Fee Item";
        }
        public ClientFeeGroupTransactionFeeVM(IEnumerable<SelectListItem> transactionFees, int originalTransactionFeeId, IEnumerable<SelectListItem> products, TransactionFeeClientFeeGroup transactionFeeClientFeeGroup, ClientFeeGroup clientFeeGroup, TransactionFee transactionFee, int feeTypeId, string feeTypeDisplayName, string feeTypeDisplayNameShort, bool hasDomainWriteAccess)
        {
            TransactionFeeClientFeeGroup = transactionFeeClientFeeGroup;
            Products = products;
            ClientFeeGroup = clientFeeGroup;
            OriginalTransactionFeeId = originalTransactionFeeId;
            TransactionFees = transactionFees;
            TransactionFee = transactionFee;
            FeeTypeId = feeTypeId;
            FeeTypeDisplayName = feeTypeDisplayName;
            FeeTypeDisplayNameShort = feeTypeDisplayNameShort;
            HasDomainWriteAccess = hasDomainWriteAccess;
        }
    }
}