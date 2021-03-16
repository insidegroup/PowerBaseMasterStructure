using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ContactsVM : CWTBaseViewModel
   {
        public ClientSubUnit ClientSubUnit { get; set; }
		public CWTPaginatedList<spDesktopDataAdmin_SelectClientSubUnitContacts_v1Result> Contacts { get; set; }
        
        public ContactsVM()
        {
        }
		public ContactsVM(ClientSubUnit clientSubUnit, CWTPaginatedList<spDesktopDataAdmin_SelectClientSubUnitContacts_v1Result> contacts)
        {
            ClientSubUnit = clientSubUnit;
            Contacts = contacts;
        }
    }
}