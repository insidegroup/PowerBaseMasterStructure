using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
	public class MeetingAdviceLanguageValidation
	{
		[Required(ErrorMessage = "Language Required")]
		public string LanguageCode { get; set; }

		[Required(ErrorMessage = "Meeting Advice Required")]
		public string MeetingAdvice { get; set; }
	}
}