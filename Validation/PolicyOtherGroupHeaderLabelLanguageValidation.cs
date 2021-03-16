using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
	public class PolicyOtherGroupHeaderLabelLanguageValidation
	{
		[RegularExpression(@"^([\w\s()*\-_\.äáâåé]+)$", ErrorMessage = "Special character entered is not allowed")]
		[Required(ErrorMessage = "Translation Required")]
		public string LabelTranslation { get; set; }

		[Required(ErrorMessage = "Language Required")]
		public string LanguageCode { get; set; }
	}
}