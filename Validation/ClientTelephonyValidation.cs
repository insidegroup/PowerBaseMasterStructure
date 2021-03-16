using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
	public class ClientTelephonyValidation
    {
		[Required(ErrorMessage = "Phone Number Required")]
		[RegularExpression(@"^[0-9]+$", ErrorMessage = "Special character entered is not allowed")]
		public string PhoneNumber { get; set; }

		[Required(ErrorMessage = "Country Required")]
		public string CountryCode { get; set; }

		[Required(ErrorMessage = "Hierarchy Required")]
		public string HierarchyType { get; set; }

		[Required(ErrorMessage = "Hierarchy Required")]
		public string HierarchyName { get; set; }

		[Required(ErrorMessage = "Telephony Type Required")]
		public string TelephonyTypeId { get; set; }

        [Required(ErrorMessage = "Back Office Type Required")]
        public string TravelerBackOfficeTypeCode { get; set; }

        [Required(ErrorMessage = "Indicator Required")]
        public int CallerEnteredDigitDefinitionTypeId { get; set; }

		/*
		Client S&S Button Text is a text field that will allow 25 alphanumeric characters.  
		 * Allowable special characters are: Ampersand (&), Asterisk (*), Backslash (\), Colon (:), Comma (,), 
		 * Hash or pound (#), Slash (/), Period (.), Apostrophe ('), Space ( ), Dash (-), Underscore (_), 
		 * and right and left Parenthesis (()) and all accented characters including but not limited to: 
		 * ä á â å é ()_-*.ç è ù à ô ü Äß Ø Å &*\:,#/.' -_()
		 */
		[RegularExpression(@"^([À-ÿ\w\s-()\*\.\&\'\’_\,\:\,\#\\\/]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string ClientSnSButtonText { get; set; }

		/* Client S&S Description is a text field that will allow 45 alphanumeric characters.  
		 * Allowable special characters are: Ampersand (&), Asterisk (*), Backslash (\), Colon (:), Comma (,), 
		 * Hash or pound (#), Slash (/), Period (.), Apostrophe ('), Space ( ), Dash (-), Underscore (_), 
		 * and right and left Parenthesis (()) and all accented characters including but not limited 
		 * to: ä á â å é ()_-*.ç è ù à ô ü Äß Ø Å &*\:,#/.' -_()
		 */
		[RegularExpression(@"^([À-ÿ\w\s-()\*\.\&\'\’_\,\:\,\#\\\/]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string ClientSnSDescription { get; set; }
    }
}