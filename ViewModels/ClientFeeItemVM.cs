using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientFeeItemVM : CWTBaseViewModel
    {
        public ClientFeeItem ClientFeeItem { get; set; }
        public FeeType FeeType { get; set; }
        public IEnumerable<SelectListItem> ClientFees { get; set; }

        public ClientFeeItemVM()
        {
        }
        public ClientFeeItemVM(ClientFeeItem clientFeeItem, FeeType feeType, IEnumerable<SelectListItem> clientFees)
        {
            ClientFeeItem = clientFeeItem;
            FeeType = feeType;
            ClientFees = clientFees;
        }
    }
}