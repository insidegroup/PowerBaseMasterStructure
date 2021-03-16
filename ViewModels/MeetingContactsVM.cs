using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class MeetingContactsVM : CWTBaseViewModel
   {
        public Meeting Meeting { get; set; }
		public CWTPaginatedList<spDesktopDataAdmin_SelectMeetingContacts_v1Result> Contacts { get; set; }
        
        public MeetingContactsVM()
        {
        }

		public MeetingContactsVM(Meeting meeting, CWTPaginatedList<spDesktopDataAdmin_SelectMeetingContacts_v1Result> contacts)
        {
            Meeting = meeting;
            Contacts = contacts;
        }
    }
}