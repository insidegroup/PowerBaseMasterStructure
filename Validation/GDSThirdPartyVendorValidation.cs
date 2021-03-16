using System.ComponentModel.DataAnnotations;
using Foolproof;
using CWTDesktopDatabase.Helpers; 

namespace CWTDesktopDatabase.Validation
{
	public class GDSThirdPartyVendorValidation
    {
		/*
		 * The field will allow up to 100 Characters including alphanumeric, all accented characters and allowed special characters 
		 * forward slash (/), asterisk (*), dash (-), underscore (_), space, period (.), right and left parenthesis (()) and ampersand (&)
		 */
		[Required(ErrorMessage = "Third Party Vendor Name Required")]
		[RegularExpression(@"^([A-zÀ-ÿ\w\s/*-_.()\&]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string GDSThirdPartyVendorName { get; set; }

    }
}
