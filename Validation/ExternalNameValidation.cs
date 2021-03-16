using System.ComponentModel.DataAnnotations;
using Foolproof;
using CWTDesktopDatabase.Helpers; 

namespace CWTDesktopDatabase.Validation
{
	public class ExternalNameValidation
    {
		[Required(ErrorMessage = "External Name Required")]
		[RegularExpression(@"^([A-zÀ-ÿ\w\s/*-_.()]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string ExternalName1 { get; set; }

    }
}
