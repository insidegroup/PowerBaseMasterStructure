using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
	public class GSTIdentificationNumberValidation
    {
        [Required(ErrorMessage = "Client Top Name Required")]
        public string ClientTopUnitName { get; set; }

        //No spaces, no alphabetic, accented or special characters
        [Required(ErrorMessage = "Client Entity Name Required")]
		[RegularExpression(@"^([a-zA-Z0-9\w\s]+)$", ErrorMessage = "Special character entered is not allowed")]
        public string ClientEntityName { get; set; }

        //No spaces, no alphabetic, accented or special characters
        [Required(ErrorMessage = "GST Identification Number Required")]
        [RegularExpression(@"^([a-zA-Z0-9]+)$", ErrorMessage = "Special character entered is not allowed")]
        public string GSTIdentificationNumber1 { get; set; }

        //No spaces, no alphabetic, accented or special characters
        [Required(ErrorMessage = "Business Phone Number Required")]
        [RegularExpression(@"^([0-9]+)$", ErrorMessage = "Special character entered is not allowed")]
        public string BusinessPhoneNumber { get; set; }

        //Allowable special characters are: dash, underscore, right and left paranthesis, period, apostrophe (O’Reily), 
        //ampersand and commercial at symbol(@). The commercial at symbol(@) is a required field
        [Required(ErrorMessage = "Business Email Address Required")]
        [RegularExpression(@"^([a-zA-Z0-9-_().'&]+)(\@)([a-zA-Z0-9-_().'&]+)$", ErrorMessage = "Must contain one @ symbol. Special character entered is not allowed")]
        public string BusinessEmailAddress { get; set; }

        //Allowable special characters are: Ampersand (&), Asterisk (*), Backslash (\), Colon (:), Comma (,), Degree (°) , 
        //Hash or pound (#), Slash (/), Period (.), Apostrophe (‘), Space ( ) and accented characters.
        [Required(ErrorMessage = "First Address Line Required")]
        [RegularExpression(@"^([À-ÿ\w\s&*()\\:,°#\/.']+)$", ErrorMessage = "Special character entered is not allowed")]
        public string FirstAddressLine { get; set; }

        //Allowable special characters are: Ampersand (&), Asterisk (*), Backslash (\), Colon (:), Comma (,), Degree (°) , 
        //Hash or pound (#), Slash (/), Period (.), Apostrophe (‘), Space ( ) and accented characters.
		[RegularExpression(@"^([À-ÿ\w\s&*()\\:,°#\/.']+)$", ErrorMessage = "Special character entered is not allowed")]
        public string SecondAddressLine { get; set; }

        //Allowable special characters are: space, dash, underscore, right and left paranthesis, period, apostrophe (O’Reily), 
        //ampersand and accented characters.
        [Required(ErrorMessage = "City Required")]
        [RegularExpression(@"^([À-ÿ\w\s-_().'&]+)$", ErrorMessage = "Special character entered is not allowed")]
        public string CityName { get; set; }

        [Required(ErrorMessage = "Country Required")]
        public string CountryCode { get; set; }

        //Allowable special characters are: space, dash, underscore, right and left paranthesis, period, apostrophe (O’Reily), 
        //ampersand and accented characters.
        [Required(ErrorMessage = "Postal Code Required")]
        [RegularExpression(@"^([À-ÿ\w\s-_().'&]+)$", ErrorMessage = "Special character entered is not allowed")]
        public string PostalCode { get; set; }
    }
}