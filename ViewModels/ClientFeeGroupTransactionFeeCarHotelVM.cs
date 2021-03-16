using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Web.Mvc;
using System.Collections.Generic;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientFeeGroupTransactionFeeCarHotelVM : CWTBaseViewModel
    {

        public TransactionFeeClientFeeGroup TransactionFeeClientFeeGroup { get; set; }
        public ClientFeeGroup ClientFeeGroup { get; set; }
        public TransactionFeeCarHotel TransactionFeeCarHotel { get; set; }
        public string FeeTypeDisplayName { get; set; }
        public string FeeTypeDisplayNameShort { get; set; }
        public int FeeTypeId { get; set; }

        public CWTPaginatedList<spDesktopDataAdmin_SelectClientDetailContacts_v1Result> Contacts { get; set; }

        public ClientFeeGroupTransactionFeeCarHotelVM()
        {
            FeeTypeId = 4;
            FeeTypeDisplayName = "Mid Office Transaction Fee Item";
            FeeTypeDisplayNameShort = "MO Transaction Fee Item";
        }
        public ClientFeeGroupTransactionFeeCarHotelVM(TransactionFeeClientFeeGroup transactionFeeClientFeeGroup, ClientFeeGroup clientFeeGroup, TransactionFeeCarHotel transactionFeeCarHotel, int feeTypeId, string feeTypeDisplayName, string feeTypeDisplayNameShort)
        {
            TransactionFeeClientFeeGroup = transactionFeeClientFeeGroup;
            ClientFeeGroup = clientFeeGroup;
            TransactionFeeCarHotel = transactionFeeCarHotel;
            FeeTypeId = feeTypeId;
            FeeTypeDisplayName = feeTypeDisplayName;
            FeeTypeDisplayNameShort = feeTypeDisplayNameShort;
        }
    }
}