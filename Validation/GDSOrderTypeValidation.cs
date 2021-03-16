using System.ComponentModel.DataAnnotations;
using Foolproof;
using CWTDesktopDatabase.Helpers; 

namespace CWTDesktopDatabase.Validation
{
	public class GDSOrderTypeValidation
    {
		[Required(ErrorMessage = "GDS Order Type Required")]
		[RegularExpression(@"^([A-zÀ-ÿ\w\s\/*-_.()]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string GDSOrderTypeName { get; set; }
	}
}
