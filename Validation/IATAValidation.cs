using System.ComponentModel.DataAnnotations;
using Foolproof;
using CWTDesktopDatabase.Helpers; 

namespace CWTDesktopDatabase.Validation
{
	public class IATAValidation
    {
		[Required(ErrorMessage = "IATA Required")]
		[RegularExpression(@"^[0-9]{8}$", ErrorMessage = "Please enter exactly 8 digits")]
		public string IATANumber { get; set; }

		[Required(ErrorMessage = "IATA Branch or GL String Required")]
		[RegularExpression(@"^[0-9-]*$", ErrorMessage = "Please enter only digits and dashes")]
		public string IATABranchOrGLString { get; set; }
	}
}
