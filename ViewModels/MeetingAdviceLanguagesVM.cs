using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using System.Collections.Generic;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
	public class MeetingAdviceLanguagesVM : CWTBaseViewModel
	{

		public CWTPaginatedList<spDesktopDataAdmin_SelectMeetingAdviceLanguages_v1Result> MeetingAdviceLanguages { get; set; }
		public Meeting Meeting { get; set; }
		public bool HasWriteAccess { get; set; }

        public MeetingAdviceLanguagesVM()
        {
			HasWriteAccess = false;
        }

		public MeetingAdviceLanguagesVM(
			CWTPaginatedList<spDesktopDataAdmin_SelectMeetingAdviceLanguages_v1Result> meetingAdviceLanguages,
			Meeting meeting)
        {
			MeetingAdviceLanguages = meetingAdviceLanguages;
			Meeting = meeting;
        }
    }
}