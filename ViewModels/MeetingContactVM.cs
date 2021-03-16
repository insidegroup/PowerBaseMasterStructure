using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class MeetingContactVM : CWTBaseViewModel
    {
        public Meeting Meeting { get; set; }
		public MeetingContact MeetingContact { get; set; }
		public IEnumerable<SelectListItem> ContactTypes { get; set; }
		public IEnumerable<SelectListItem> Countries { get; set; }

        public MeetingContactVM()
        {
        }
		public MeetingContactVM(Meeting meeting, MeetingContact meetingcontact, IEnumerable<SelectListItem> contactTypes)
        {
            Meeting = meeting;
			MeetingContact = meetingcontact;
            ContactTypes = contactTypes;
        }
    }
}