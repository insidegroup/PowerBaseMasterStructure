using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
	public class FormOfPaymentAdviceMessageGroupItemValidation
    {
		[Required(ErrorMessage = "Language Required")]
		public string LanguageCode { get; set; }

		[Required(ErrorMessage = "Product Required")]
		public string ProductId { get; set; }

		[Required(ErrorMessage = "Supplier Required")]
		public string SupplierCode { get; set; }

		[Required(ErrorMessage = "Country Required")]
		public string CountryCode { get; set; }

		[Required(ErrorMessage = "Travel Indicator Required")]
		public string TravelIndicator { get; set; }

		[Required(ErrorMessage = "FOP Type Required")]
		public string FormofPaymentTypeID { get; set; }

        [Required(ErrorMessage = "FOP Advice Message Required")]
		[RegularExpression(@"^([À-ÿ\w\s\/\*\-_.(),\u0022\u2018\u2019\“\'\%\$\=\+\?\!\:\;\@\<\>\[\]]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string FormOfPaymentAdviceMessage { get; set; }
    }
}
