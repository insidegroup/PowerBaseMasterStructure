using System.ComponentModel.DataAnnotations;
using Foolproof;
using CWTDesktopDatabase.Helpers; 

namespace CWTDesktopDatabase.Validation
{
	public class GDSRequestTypeValidation
    {
		[Required(ErrorMessage = "GDS Request Type Required")]
		[RegularExpression(@"^([\w\s\/*-_.()]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string GDSRequestTypeName { get; set; }
    }
}
