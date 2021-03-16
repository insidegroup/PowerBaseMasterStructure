using System.Collections.Generic;
using System.Web.Mvc;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
    public class MeetingAdviceLanguageVM : CWTBaseViewModel
    {
		public Meeting Meeting { get; set; }
		public MeetingAdviceLanguage MeetingAdviceLanguage { get; set; }
	    public IEnumerable<SelectListItem> MeetingAdviceLanguages { get; set; }

	    public MeetingAdviceLanguageVM()
        {
        }

		public MeetingAdviceLanguageVM(Meeting meeting, MeetingAdviceLanguage meetingAdviceLanguage)
        {
			MeetingAdviceLanguage = meetingAdviceLanguage;
			Meeting = meeting;
        }
    }
}