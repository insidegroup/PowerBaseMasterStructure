using System.ComponentModel.DataAnnotations;
using Foolproof;

namespace CWTDesktopDatabase.Validation
{
	public class PriceTrackingHandlingFeeItemValidation
	{
        [Required(ErrorMessage = "Price Tracking System Required")]
        public int PriceTrackingSystemId { get; set; }

        [Required(ErrorMessage = "Product Required")]
        public int ProductId { get; set; }

		[RegularExpression(@"^(\d{1,3}|\d{0,3}\.\d{1,2})$", ErrorMessage = "Saving Amount Percentage should be in format 000.00 (maximum 2 decimal places)")]
		public decimal? SavingAmountPercentage { get; set; }

		[RegularExpression(@"^(\d{1,5}|\d{0,5}\.\d{1,2})$", ErrorMessage = "Handling Fee should be in format 00000.00 (maximum 2 decimal places)")]
		public decimal? HandlingFee { get; set; }
	}
}
