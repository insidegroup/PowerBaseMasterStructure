using System.ComponentModel.DataAnnotations;
using Foolproof;

namespace CWTDesktopDatabase.Validation
{
	public class MeetingPNROutputValidation
    {
		[RegularExpression(@"^([^0-9]*)$", ErrorMessage = "Numbers are not allowed")]
		public string GDSRemarkQualifier { get; set; }

		[Required(ErrorMessage = "Field Required")]
		public string GDSCode { get; set; }

		[Required(ErrorMessage = "Field Required")]
		public int PNROutputRemarkTypeCode { get; set; }

		[Required(ErrorMessage = "Field Required")]
		public string DefaultRemark { get; set; }

		[Required(ErrorMessage = "Field Required")]
		public string DefaultLanguageCode { get; set; }
	}
}