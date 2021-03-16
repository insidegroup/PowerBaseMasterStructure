using System.ComponentModel.DataAnnotations;
using Foolproof;
using CWTDesktopDatabase.Helpers; 

namespace CWTDesktopDatabase.Validation
{
	public class PseudoCityOrOfficeTypeValidation
    {
		[Required(ErrorMessage = "Pseudo City/Office ID Type Required")]
		[RegularExpression(@"^([\w\s\/*-_.()]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string PseudoCityOrOfficeTypeName { get; set; }

    }
}
