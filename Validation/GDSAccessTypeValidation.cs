using System.ComponentModel.DataAnnotations;
using Foolproof;
using CWTDesktopDatabase.Helpers; 

namespace CWTDesktopDatabase.Validation
{
	public class GDSAccessTypeValidation
    {
		[Required(ErrorMessage = "GDS Access Type Required")]
		[RegularExpression(@"^([\w\s\/*-_.()]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string GDSAccessTypeName { get; set; }

		[Required(ErrorMessage = "GDS Required")]
		public string GDSCode { get; set; }
	}
}
