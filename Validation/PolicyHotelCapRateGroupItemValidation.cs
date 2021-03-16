using System.ComponentModel.DataAnnotations;
namespace CWTDesktopDatabase.Validation
{
    public class PolicyHotelCapRateGroupItemValidation
    {
		[Required(ErrorMessage = "Policy Location Required")]
		public string PolicyLocationId { get; set; }

		[Required(ErrorMessage = "Cap Rate Required")]
		public double? CapRate { get; set; }

		[Required(ErrorMessage = "Currency Required")]
		public string CurrencyCode { get; set; }

    }
}
