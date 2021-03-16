using CWTDesktopDatabase.Models;
using System.Web.Mvc;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;

namespace CWTDesktopDatabase.ViewModels
{
	[Bind(Exclude = "CreationTimestamp")]
	public class MeetingPNROutputVM : CWTBaseViewModel
	{
		public MeetingPNROutput MeetingPNROutput { get; set; }
		public Meeting Meeting { get; set; }

		public IEnumerable<SelectListItem> GDSList { get; set; }
		public IEnumerable<SelectListItem> PNROutputRemarkTypeCodes { get; set; }
		public IEnumerable<SelectListItem> Languages { get; set; }
		public IEnumerable<SelectListItem> Countries { get; set; }

		public MeetingPNROutputVM()
		{
		}

		public MeetingPNROutputVM(
			MeetingPNROutput meetingPNROutput, 
			Meeting meeting)
		{
			MeetingPNROutput = meetingPNROutput;
		}
	}
}