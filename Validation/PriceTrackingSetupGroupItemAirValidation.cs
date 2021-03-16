using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
	public class PriceTrackingSetupGroupItemAirValidation
    {
        [Required(ErrorMessage = "Client has provided written approval Required")]
        public bool ClientHasProvidedWrittenApprovalFlag { get; set; }

        [RegularExpression(@"^(\d{1,3}|\d{0,3}\.\d{1,2})$", ErrorMessage = "This field allows up to 3 digits and 2 decimal places")]
        public decimal SharedSavingsAmount { get; set; }
        
        [RegularExpression(@"^(\d{1,10}|\d{0,10}\.\d{1,2})$", ErrorMessage = "This field allows up to 10 digits and 2 decimal places")]
        public decimal TransactionFeeAmount { get; set; }

		[RegularExpression(@"^([\w À-ÿ\*\-\.\(\)]+)$", ErrorMessage = "Special character entered is not allowed")]
        public string CentralFulfillmentBusinessHours { get; set; }

        [Required(ErrorMessage = "Number of annual transactions Required")]
        [RegularExpression(@"^(\d{1,9})$", ErrorMessage = "This field allows up to 9 digits")]
        public int AnnualTransactionCount { get; set; }

        [Required(ErrorMessage = "Annual Spend Required")]
        [RegularExpression(@"^(\d{1,13}|\d{0,13}\.\d{1,2})$", ErrorMessage = "This field allows up to 13 digits and 2 decimal places")]
        public decimal AnnualSpendAmount { get; set; }

        [RegularExpression(@"^([À-ÿ\w\s-()*.:_]+)$", ErrorMessage = "Special character entered is not allowed")]
        public string Comment { get; set; }

        [Required(ErrorMessage = "Currency Required")]
        public string CurrencyCode { get; set; }

        [RegularExpression(@"^(\d{1,10}|\d{0,10}\.\d{1,2})$", ErrorMessage = "This field allows up to 10 digits and 2 decimal places")]
        public decimal? EstimatedCWTRebookingFeeAmount { get; set; }

        [RegularExpression(@"^(\d{1,10}|\d{0,10}\.\d{1,2})$", ErrorMessage = "This field allows up to 10 digits and 2 decimal places")]
        public decimal? CWTVoidRefundFeeAmount { get; set; }

        [RegularExpression(@"^(\d{1,10}|\d{0,10}\.\d{1,2})$", ErrorMessage = "This field allows up to 10 digits and 2 decimal places")]
        public decimal? NoPenaltyVoidWindowThresholdAmount { get; set; }

        [RegularExpression(@"^(\d{1,10}|\d{0,10}\.\d{1,2})$", ErrorMessage = "This field allows up to 10 digits and 2 decimal places")]
        public decimal? OutsideVoidPenaltyThresholdAmount { get; set; }

        [RegularExpression(@"^(\d{1,3})$", ErrorMessage = "This field allows a minimum of 1 and a maximum of 3 digits")]
        public decimal? RefundableToRefundablePreDepartureDayAmount { get; set; }

        [RegularExpression(@"^(\d{1,10}|\d{0,10}\.\d{1,2})$", ErrorMessage = "This field allows up to 10 digits and 2 decimal places")]
        public decimal? NoPenaltyRefundableToPenaltyRefundableThresholdAmount { get; set; }

        [RegularExpression(@"^(\d{1,3})$", ErrorMessage = "This field allows a minimum of 1 and a maximum of 3 digits")]
        public decimal? RefundableToNonRefundablePreDepartureDayAmount { get; set; }

        [RegularExpression(@"^(\d{1,10}|\d{0,10}\.\d{1,2})$", ErrorMessage = "This field allows up to 10 digits and 2 decimal places")]
        public decimal? NoPenaltyRefundableToPenaltyNonRefundableThresholdAmount { get; set; }

        [RegularExpression(@"^([a-zA-Z0-9]+)$", ErrorMessage = "Special character entered is not allowed")]
        public string NegotiatedPricingTrackingCode1 { get; set; }

        [RegularExpression(@"^([a-zA-Z0-9]+)$", ErrorMessage = "Special character entered is not allowed")]
        public string NegotiatedPricingTrackingCode2 { get; set; }

        [RegularExpression(@"^([a-zA-Z0-9]+)$", ErrorMessage = "Special character entered is not allowed")]
        public string NegotiatedPricingTrackingCode3 { get; set; }

        [RegularExpression(@"^([a-zA-Z0-9]+)$", ErrorMessage = "Special character entered is not allowed")]
        public string NegotiatedPricingTrackingCode4 { get; set; }

        [RegularExpression(@"^([\w\s,]+)$", ErrorMessage = "Special character entered is not allowed")]
        public string CompanyProfilesRequiringFIQID { get; set; }

        [RegularExpression(@"^([\w]+)$", ErrorMessage = "Special character entered is not allowed")]
        public string RealisedSavingsCode { get; set; }

        [RegularExpression(@"^([\w]+)$", ErrorMessage = "Special character entered is not allowed")]
        public string MissedSavingsCode { get; set; }

        [RegularExpression(@"^([\w]+)$", ErrorMessage = "Special character entered is not allowed")]
        public string ImportPseudoCityOrOfficeId { get; set; }

        [RegularExpression(@"^([a-zA-Z0-9*, ]+)$", ErrorMessage = "Special character entered is not allowed")]
        public string ImportQueueInClientPseudoCityOrOfficeId { get; set; }

        [RegularExpression(@"^([\w]+)$", ErrorMessage = "Special character entered is not allowed")]
        public string PricingPseudoCityOrOfficeId { get; set; }

        [RegularExpression(@"^([a-zA-Z0-9*, ]+)$", ErrorMessage = "Special character entered is not allowed")]
        public string SavingsQueueId { get; set; }

        [RegularExpression(@"^([a-zA-Z0-9*, ]+)$", ErrorMessage = "Special character entered is not allowed")]
        public string AlphaCodeRemarkField { get; set; }
    }
}