using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
	public class PolicyCarOtherGroupItemLanguageValidation
	{
		[Required(ErrorMessage = "Translation Required")]
		public string Translation { get; set; }

		[Required(ErrorMessage = "Language Required")]
		public string LanguageCode { get; set; }
	}
}