using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
	public class TravelPortValidation
    {
		//Travel Port Name is a free format text box allowing a maximum of 100 alphanumeric and allowable special characters. 
		//Allowable special characters are: space, dash, underscore, right and left parentheses, period, apostrophe (O’Reily), ampersand, slash and accented characters. 
		[Required(ErrorMessage = "Travel Port Name Required")]
		[RegularExpression(@"^([À-ÿ\w\s()-_&.'\\\/]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string TravelportName { get; set; }

		//Travel Port Code is a free format text box allowing a maximum of 50 alphanumeric and allowable special characters. 
		//Allowable special characters are: space, dash, underscore, right and left parentheses, period, apostrophe (O’Reily), ampersand, slash and accented characters.
		[Required(ErrorMessage = "Travel Port Code Required")]
		[RegularExpression(@"^([À-ÿ\w\s()-_&.'\\\/]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string TravelPortCode { get; set; }

		[Required(ErrorMessage = "Travel Port Type Required")]
		public string TravelPortTypeId { get; set; }

		//Metropolitan Area is a free format text box allowing a maximum of 50 alphanumeric and allowable special characters. 
		//Allowable special characters are: space, dash, underscore, right and left parentheses, period, apostrophe (O’Reily), ampersand, slash and accented characters.
		[RegularExpression(@"^([À-ÿ\w\s()-_&.'\\\/]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string MetropolitanArea { get; set; }

		//City Name is a type ahead box
		[Required(ErrorMessage = "Travel Port City Required")]
		public string CityName { get; set; }

		//City Code is a hidden field
		[Required(ErrorMessage = "Travel Port City Required")]
		public string CityCode { get; set; }

		//Country Name is a type ahead box
		[Required(ErrorMessage = "Country Required")]
		public string CountryName { get; set; }

		//Country Code is a hidden field
		[Required(ErrorMessage = "Country Required")]
		public string CountryCode { get; set; }

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

	}
}