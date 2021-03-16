using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
	public class AdditionalBookingCommentValidation
	{
		[Required(ErrorMessage = "Langauge Required")]
		public string LanguageCode { get; set; }

		[Required(ErrorMessage = "Additional Booking Comment Required")]
		public string AdditionalBookingCommentDescription { get; set; }

	}
}