using System.ComponentModel.DataAnnotations;
using Foolproof;
using CWTDesktopDatabase.Helpers; 

namespace CWTDesktopDatabase.Validation
{
	public class PseudoCityOrOfficeDefinedRegionValidation
    {
		[Required(ErrorMessage = "Pseudo City/Office ID Location Type Required")]
		[RegularExpression(@"^([\w\s\/*-_.()&]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string PseudoCityOrOfficeDefinedRegionName { get; set; }
		
		[Required(ErrorMessage = "Global Region Required")]
		public string GlobalRegionCode { get; set; }

    }
}
