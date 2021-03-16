using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Foolproof;
using CWTDesktopDatabase.Helpers;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Validation
{
	[Bind(Exclude = "CreationTimestamp")]
	public class MerchantFeeValidation
    {
        [Required(ErrorMessage = "Fee Description Required")]
        [RegularExpression(@"^([\w\s-()*]+)$", ErrorMessage = "Special character entered is not allowed")]
        public string MerchantFeeDescription { get; set; }

        [Required(ErrorMessage = "Country Required")]
        public string CountryCode { get; set; }

        [Required(ErrorMessage = "Credit Card Vendor Required")]
        public string CreditCardVendorCode { get; set; }

        [Required(ErrorMessage = "Merchant Fee Percent Required")]
        public decimal? MerchantFeePercent  { get; set; } //SQL NUMERIC(6,3) eg 000.000
    }
}