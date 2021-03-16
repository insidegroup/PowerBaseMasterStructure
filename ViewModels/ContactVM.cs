using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ContactVM : CWTBaseViewModel
    {
        public ClientSubUnit ClientSubUnit { get; set; }
        public Contact Contact { get; set; }
		public IEnumerable<SelectListItem> ContactTypes { get; set; }
		public IEnumerable<SelectListItem> Countries { get; set; }
		public IEnumerable<SelectListItem> StateProvinces { get; set; }

        public ContactVM()
        {
        }
		public ContactVM(ClientSubUnit clientSubUnit, Contact contact, IEnumerable<SelectListItem> contactTypes)
        {
            ClientSubUnit = clientSubUnit;
            Contact = contact;
            ContactTypes = contactTypes;
        }
    }
}