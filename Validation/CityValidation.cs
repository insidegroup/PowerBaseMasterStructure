using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
	public class CityValidation
    {
		//City Name is a free format text box allowing a maximum of 100 alphanumeric and allowable special characters.
		//Allowable special characters are: space, dash, underscore, right and left parentheses, period, ampersand, and accented characters.
		[Required(ErrorMessage = "Name Required")]
		[RegularExpression(@"^([À-ÿ\w\s()-_&.]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string Name { get; set; }

		//City Code is a free format text box allowing a maximum of five alphanumeric and allowable special characters. 
		//Allowable special characters are: space, dash, underscore, right and left parentheses, period, ampersand, slash and accented characters. 
		[Required(ErrorMessage = "City Code Required")]
		[RegularExpression(@"^([À-ÿ\w\s()-_&.\\\/]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string CityCode { get; set; }

		//Country Name 
		[Required(ErrorMessage = "Country Name Required")]
		public string CountryName { get; set; }

		//Latitude is a text box supporting decimal format as per the Latitude column in the City table
		//Allowing positive and negative values of three places prior to the decimal and five places following the decimal. 
		[Required(ErrorMessage = "Latitude Required")]
		[Range(-999.99999, 999.99999, ErrorMessage = "Latitude must be between -999.99999 and 999.99999")]
		public decimal LatitudeDecimal { get; set; }

		//Longitude is a text box supporting decimal format as per the Latitude column in the City table
		//Allowing positive and negative values of three places prior to the decimal and five places following the decimal. 
		[Required(ErrorMessage = "Longitude Required")]
		[Range(-999.99999, 999.99999, ErrorMessage = "Longitude must be between -999.99999 and 999.99999")]
		public decimal LongitudeDecimal { get; set; }

        //TimeZoneRuleCode
        [Required(ErrorMessage = "Time Zone Required")]
        public string TimeZoneRuleCode { get; set; }
    }
}