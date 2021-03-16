using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
	public class OptionalFieldLookupValueLanguageValidation
	{
		[Required(ErrorMessage = "Language Required")]
		public string LanguageCode { get; set; }

		[Required(ErrorMessage = "Label Required")]
		[RegularExpression(@"^([À-ÿ\s\w-()*]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string OptionalFieldLookupValueLabel { get; set; }
	}
}