using CWTDesktopDatabase.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.Validation
{
	public class GDSEndWarningConfigurationValidation
	{
		[Required(ErrorMessage = "Required")]
		public string GDSCode { get; set; }

		[Required(ErrorMessage = "Required")]
		public string IdentifyingWarningMessage { get; set; }

		[Required(ErrorMessage = "Required")]
		public string GDSEndWarningBehaviorTypeId { get; set; }

	}
}