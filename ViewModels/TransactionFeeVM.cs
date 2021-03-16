using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class TransactionFeeVM : CWTBaseViewModel
    {
        public TransactionFee TransactionFee { get; set; }
        public string Name { get; set; }
        public bool FromGlobalFlag { get; set; }
        public string FromCode { get; set; }
        public bool ToGlobalFlag { get; set; }
        public string ToCode { get; set; }
        public bool RoutingViceVersaFlag { get; set; }

        public TransactionFeeVM()
        {
        }
        public TransactionFeeVM(TransactionFee transactionFee, string name, bool fromGlobalFlag, string fromCode, bool toGlobalFlag, string toCode, bool routingViceVersaFlag)
        {
            TransactionFee = transactionFee;
            Name = name;
            FromGlobalFlag = fromGlobalFlag;
            FromCode = fromCode;
            ToGlobalFlag = toGlobalFlag;
            ToCode = toCode;
            RoutingViceVersaFlag = routingViceVersaFlag;
        }
    }
}