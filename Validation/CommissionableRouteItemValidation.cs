using System.ComponentModel.DataAnnotations;
using Foolproof;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.Validation
{
	public class CommissionableRouteItemValidation
    {
        
        [Required(ErrorMessage = "Product Required")]
        public int ProductId { get; set; }

		[Required(ErrorMessage = "Supplier Required")]
		public string SupplierName { get; set; }

		[Required(ErrorMessage = "Travel Indicator Required")]
		public string TravelIndicator { get; set; }

		[Required(ErrorMessage = "ClassOfTravel Required - Default Value: *")]
		[RegularExpression(@"^([a-zA-Z0-9\\*]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string ClassOfTravel { get; set; }

		[RegularExpression(@"^(\d{0,3}(\.\d{1,2})?)$", ErrorMessage = "Please enter a value with up to and including 3 numbers and optional 2 decimal places")]
		public decimal? BSPCommission { get; set; }

		[RegularExpression(@"^(\d{0,3}(\.\d{1,2})?)$", ErrorMessage = "A value with up to and including 3 numbers is required and optional 2 decimal places")]
		[CWTRequiredIfNot("CommissionableTaxCodes", "", ErrorMessage = "Required if Commission on Tax is specified")]
		public decimal? CommissionOnTaxes { get; set; }

		[RegularExpression(@"^([\w\s-_()*]+)$", ErrorMessage = "Special character entered is not allowed")]
		[CWTRequiredIfNot("CommissionOnTaxes", "", ErrorMessage = "Required if Commission on Tax is specified")]
		public string CommissionableTaxCodes { get; set; }

		[RegularExpression(@"^([\w\s-()*_]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string RemarksOrRoute { get; set; }

		[RegularExpression(@"^(\d{0,6}(\.\d{1,2})?)$", ErrorMessage = "Please enter a value with up to and including 6 numbers and optional 2 decimal places")]
		[DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
		public decimal? CommissionAmount { get; set; }

    }
}
