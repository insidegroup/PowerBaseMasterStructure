using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
	public class AddressValidation
	{
		[Required(ErrorMessage = "Address Line 1 Required")]
		public string FirstAddressLine { get; set; }

		[Required(ErrorMessage = "City Required")]
		public string CityName { get; set; }

		[Range(-999.9999999, 999.9999999, ErrorMessage = "Latitude must be between -999.9999999 and 999.9999999")]
		public decimal LatitudeDecimal { get; set; }

		[Range(-999.9999999, 999.9999999, ErrorMessage = "Longitude must be between -999.9999999 and 999.9999999")]
		public decimal LongitudeDecimal { get; set; }

		[Required(ErrorMessage = "Postal Code Required")]
		public string PostalCode { get; set; }
	}
}