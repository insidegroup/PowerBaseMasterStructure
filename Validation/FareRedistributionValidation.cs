using System.ComponentModel.DataAnnotations;
using Foolproof;
using CWTDesktopDatabase.Helpers; 

namespace CWTDesktopDatabase.Validation
{
	public class FareRedistributionValidation
    {
		[Required(ErrorMessage = "Fare Redistribution Required")]
		[RegularExpression(@"^([\w\s\/*-_.()]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string FareRedistributionName { get; set; }

		[Required(ErrorMessage = "GDS Required")]
		public string GDSCode { get; set; }
	}
}
