using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Validation
{
	[Bind(Exclude = "CreationTimestamp")]
	public class ClientTopUnitClientLocationValidation
    {
		[Required(ErrorMessage = "Client Location Name Required")]
		[RegularExpression(@"^([À-ÿ\w\s-()\*\.\&\'\’_]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string AddressLocationName { get; set; }

		[Required(ErrorMessage = "Address Line 1 Required")]
        [RegularExpression(@"^([À-ÿ\w\s-()\*\.\&\'\’_\,\:\,\°\#\\\/]+)$", ErrorMessage = "Special character entered is not allowed")]
        public string FirstAddressLine { get; set; }

		[RegularExpression(@"^([À-ÿ\w\s-()\*\.\&\'\’_\,\:\,\°\#\\\/]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string SecondAddressLine { get; set; }

		[Required(ErrorMessage = "City Required")]
		[RegularExpression(@"^([À-ÿ\w\s-()\*\.\&\'\’_]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string CityName { get; set; }

		[RegularExpression(@"^([À-ÿ\w\s-()\*\.\&\'\’_]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string StateProvinceName { get; set; }

		[Required(ErrorMessage = "Postal Code Required")]
		[RegularExpression(@"^([À-ÿ\w\s-()\*\.\&\'\’_]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string PostalCode { get; set; }

		[Required(ErrorMessage = "Country Required")]
		public string CountryCode { get; set; }

		[Required(ErrorMessage = "Latitude Required")]
		[Range(-999.9999999, 999.9999999, ErrorMessage = "Latitude must be between -999.9999999 and 999.9999999")]
		public decimal? LatitudeDecimal { get; set; }

		[Required(ErrorMessage = "Longitude Required")]
		[Range(-999.9999999, 999.9999999, ErrorMessage = "Longitude must be between -999.9999999 and 999.9999999")]
		public decimal? LongitudeDecimal { get; set; }
    }
}