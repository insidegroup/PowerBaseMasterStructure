using System.ComponentModel.DataAnnotations;
using Foolproof;
using CWTDesktopDatabase.Helpers; 

namespace CWTDesktopDatabase.Validation
{
	public class GDSOrderDetailValidation
    {
		[Required(ErrorMessage = "GDS Order Detail Required")]
		[RegularExpression(@"^([A-zÀ-ÿ\w\s\/*-_.()]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string GDSOrderDetailName { get; set; }
	}
}
