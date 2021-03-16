using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
	public class OptionalFieldLanguageValidation
	{
		[Required(ErrorMessage = "Language Required")]
		public string LanguageCode { get; set; }

		[Required(ErrorMessage = "Label Required")]
		[RegularExpression(@"^([À-ÿ\s\w-()*]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string OptionalFieldLabel { get; set; }

		[RegularExpression(@"^([À-ÿ\s\w-()*]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string OptionalFieldTooltip { get; set; }

		[Required(ErrorMessage = "Default Text Required")]
		public string OptionalFieldDefaultText { get; set; }

		public string OptionalFieldValidationRegularExpression { get; set; }

		[RegularExpression(@"^([À-ÿ\s\w-()*]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string OptionalFieldValidationFailureMessage { get; set; }

	}
}