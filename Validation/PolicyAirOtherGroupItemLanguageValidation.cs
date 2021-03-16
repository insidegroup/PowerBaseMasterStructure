using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Validation
{
	public class PolicyAirOtherGroupItemLanguageValidation
	{
		[Required(ErrorMessage = "Translation Required")]
		public string Translation { get; set; }

		[Required(ErrorMessage = "Language Required")]
		public string LanguageCode { get; set; }
	}
}