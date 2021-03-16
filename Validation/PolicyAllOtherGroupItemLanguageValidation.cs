using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
	public class PolicyAllOtherGroupItemLanguageValidation
	{
		[Required(ErrorMessage = "Translation Required")]
		public string Translation { get; set; }

		[Required(ErrorMessage = "Language Required")]
		public string LanguageCode { get; set; }
	}
}