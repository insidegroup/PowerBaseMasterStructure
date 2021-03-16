using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
	public class PolicyOtherGroupHeaderTableNameLanguageValidation
	{
		[RegularExpression(@"^([\w\s()*\-_\.äáâåé]+)$", ErrorMessage = "Special character entered is not allowed")]
		[Required(ErrorMessage = "Translation Required")]
		public string TableNameTranslation { get; set; }

		[Required(ErrorMessage = "Language Required")]
		public string LanguageCode { get; set; }
	}
}