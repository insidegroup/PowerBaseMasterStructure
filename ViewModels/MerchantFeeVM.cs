using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class MerchantFeeVM : CWTBaseViewModel
    {
        public MerchantFee MerchantFee { get; set; }
        public IEnumerable<SelectListItem> Countries { get; set; }
        public IEnumerable<SelectListItem> CreditCardVendors { get; set; }
        public IEnumerable<SelectListItem> Products { get; set; }

        public MerchantFeeVM()
        {
        }
        public MerchantFeeVM(MerchantFee merchantFee, IEnumerable<SelectListItem> clientFees, IEnumerable<SelectListItem> countries, IEnumerable<SelectListItem> creditCardVendors, IEnumerable<SelectListItem> products)
        {
            MerchantFee = merchantFee;
            Countries = countries;
            CreditCardVendors = creditCardVendors;
            Products = products;
        }
    }
}