using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientTopUnitContactVM : CWTBaseViewModel
    {
        public ClientTopUnit ClientTopUnit { get; set; }
        public ClientDetail ClientDetail { get; set; }
        public Contact Contact { get; set; }
        public IEnumerable<SelectListItem> ContactTypes { get; set; }

        public ClientTopUnitContactVM()
        {
        }
        public ClientTopUnitContactVM(ClientTopUnit clientTopUnit, ClientDetail clientDetail, Contact contact, IEnumerable<SelectListItem> contactTypes)
        {
            ClientTopUnit = clientTopUnit;
            ClientDetail = clientDetail;
            Contact = contact;
            ContactTypes = contactTypes;
        }
    }
}