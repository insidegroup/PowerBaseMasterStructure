using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
	public class SupplierValidation
    {
		//Supplier Code is a free format text box allowing a maximum of 50 alphanumeric and allowable special characters. 
		//Allowable special characters are: space, dash, underscore, right and left parentheses, period, apostrophe (O’Reily), ampersand, slash and accented characters.
		[Required(ErrorMessage = "Supplier Code Required")]
		[RegularExpression(@"^([À-ÿ\w\s()-_&.'\\\/]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string SupplierCode { get; set; }

		//Supplier Name is a free format text box allowing a maximum of 100 alphanumeric and allowable special characters. 
		//Allowable special characters are: space, dash, underscore, right and left parentheses, period, apostrophe (O’Reily), ampersand, slash and accented characters.
		[Required(ErrorMessage = "Supplier Name Required")]
		[RegularExpression(@"^([À-ÿ\w\s()-_&.'\\\/]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string SupplierName { get; set; }

		[Required(ErrorMessage = "Product Name Required")]
		public string ProductId { get; set; }
	
	}
}