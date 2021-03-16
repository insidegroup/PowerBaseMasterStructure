using CWTDesktopDatabase.Models;
using System.Web.Mvc;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;

namespace CWTDesktopDatabase.ViewModels
{
	[Bind(Exclude = "CreationTimestamp")]
	public class MeetingPNROutputLanguageVM : CWTBaseViewModel
	{
		public MeetingPNROutputLanguage MeetingPNROutputLanguage { get; set; }
		public MeetingPNROutput MeetingPNROutput { get; set; }

		public ClientSubUnit ClientSubUnit { get; set; }
		public IEnumerable<SelectListItem> Languages { get; set; }

		public MeetingPNROutputLanguageVM()
		{
		}

		public MeetingPNROutputLanguageVM(
			MeetingPNROutputLanguage meetingPNROutputLanguage,
			MeetingPNROutput meetingPNROutput)
		{
			MeetingPNROutputLanguage = MeetingPNROutputLanguage;
			MeetingPNROutput = meetingPNROutput;
		}
	}
}