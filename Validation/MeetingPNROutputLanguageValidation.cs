using System.ComponentModel.DataAnnotations;
using Foolproof;

namespace CWTDesktopDatabase.Validation
{
	public class MeetingPNROutputLanguageValidation
    {
		[Required(ErrorMessage = "Field Required")]
		public string RemarkTranslation { get; set; }

		[Required(ErrorMessage = "Field Required")]
		public string LanguageCode { get; set; }
	}
}