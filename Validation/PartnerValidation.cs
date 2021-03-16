using System.ComponentModel.DataAnnotations;
using Foolproof;
using CWTDesktopDatabase.Helpers; 

namespace CWTDesktopDatabase.Validation
{
	public class PartnerValidation
    {
		/*
		 * The field will allow up to 100 Characters including alphanumeric, all accented characters and allowed special characters 
		 * forward slash (/), asterisk (*), dash (-), underscore (_), space, period (.) and right and left parenthesis (()) and ampersand (&)
		 */
		[Required(ErrorMessage = "Partner Name Required")]
		[RegularExpression(@"^([A-zÀ-ÿ\w\s/*-_.()\&]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string PartnerName { get; set; }

		[Required(ErrorMessage = "Country Required")]
		public string CountryCode { get; set; }

    }
}
