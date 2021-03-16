using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Foolproof;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.Validation
{
    public class TransactionFeeAirValidation
    {
        [Required(ErrorMessage = "Description Required")]
        [RegularExpression(@"^([À-ÿ\w\s-()*]+)$", ErrorMessage = "Special character entered is not allowed")]
        public string TransactionFeeDescription { get; set; }

        [Required(ErrorMessage = "Travel Indicator Required")]
        public int TravelIndicator { get; set; }

        [Required(ErrorMessage = "Booking Source Required")]
        public int BookingSourceCode { get; set; }

        [Required(ErrorMessage = "Booking Origination Required")]
        public int BookingOriginationCode { get; set; }

        [Required(ErrorMessage = "Charge Type Required")]
        public int ChargeTypeCode { get; set; }

        [Required(ErrorMessage = "Transaction Type Required")]
        public string TransactionTypeCode { get; set; }

        [CWTRequiredIfStartsWith("TransactionTypeCode", "User Defined", ErrorMessage = "Fee Category Required")]
        public string FeeCategory { get; set; }

        [CWTRequiredIf("FeeCategory", "Traveler", ErrorMessage = "Fee Category Required")]
        public string TravelerClassCode { get; set; }

        [Required(ErrorMessage = "Supplier Required")]
        public string SupplierName { get; set; }

        [CWTRequiredIf("FeeCategory", "Sector", ErrorMessage = "Minimum Fee Quantity Required")]
        [Range(0, 20, ErrorMessage = "Minimum Fee Quantity must be between 0 and 20")]
        public decimal? MinimumFeeCategoryQuantity { get; set; }

        [CWTRequiredIf("FeeCategory", "Sector", ErrorMessage = "Maximum Fee Quantity Required")]
        [Range(0,20, ErrorMessage = "Maximum Fee Quantity must be between 0 and 20")]
        public decimal? MaximumFeeCategoryQuantity { get; set; }

        [CWTRequiredIf("FeeCategory", "Ticket", ErrorMessage = "Minimum Ticket Price Required")]
        [RegularExpression(@"^\d{0,6}.\d{0,2}$", ErrorMessage = "Minimum Ticket Price should be in format 000000.00 (maximum 2 decimal places)")]
        public decimal? MinimumTicketPrice { get; set; }
        
        [CWTRequiredIf("FeeCategory", "Ticket", ErrorMessage = "Maximum Ticket Price Required")]
        [RegularExpression(@"^\d{0,6}.\d{0,2}$", ErrorMessage = "Maximum Ticket Price should be in format 000000.00 (maximum 2 decimal places)")]
        public decimal? MaximumTicketPrice { get; set; }
        
        //Ticket Price Currency (drop list with all currency – from Currency table) - Mandatory if Minimum Ticket Price provided
        [CWTRequiredIfNot("MinimumTicketPrice", "", ErrorMessage = "Ticket Price Currency Required")]
        public string TicketPriceCurrencyCode { get; set; }

        //Fee Amount (input text box, numeric only) - optional if Fee Percent provided (either Fee Amount or FeePercent is required)
        [AtLeastOneRequired("FeeAmount", "FeePercent", ErrorMessage = "FeeAmount and/or FeePercent Required")]
        [RegularExpression(@"^\d{0,6}.\d{0,2}$", ErrorMessage = "Maximum Ticket Price should be in format 000000.00 (maximum 2 decimal places)")]
        public decimal? FeeAmount { get; set; }

        //Fee Currency (drop list with all currency – from Currency table) – required if Fee Amount entered
        [CWTRequiredIfNot("FeeAmount", "", ErrorMessage = "Fee Currency Required")]
        public string FeeCurrencyCode { get; set; }

        //Fee Percent (input text box, numeric only) – optional if FeeAmount provided (either Fee Amount or FeePercent is required)
        [AtLeastOneRequired("FeePercent", "FeeAmount", ErrorMessage = "FeeAmount and/or FeePercent Required")]
        [RegularExpression(@"^\d{0,3}.\d{0,2}$", ErrorMessage = "Maximum Ticket Price should be in format 000000.00 (maximum 2 decimal places)")]
        public decimal? FeePercent { get; set; }

    }
}