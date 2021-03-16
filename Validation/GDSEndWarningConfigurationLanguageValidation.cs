using CWTDesktopDatabase.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.Validation
{
	public class GDSEndWarningConfigurationLanguageValidation
    {
		[Required(ErrorMessage = "Required")]
		public string LanguageCode { get; set; }

		[RegularExpression(@"^([\w\s()*\-\,\.äáâåé]+)$", ErrorMessage = "Special character entered is not allowed")]
		[Required(ErrorMessage = "Required")]
		public string AdviceMessage { get; set; }
    }
}