using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientSubUnitContactVM : CWTBaseViewModel
    {
        public ClientSubUnit ClientSubUnit { get; set; }
        public ClientDetail ClientDetail { get; set; }
        public Contact Contact { get; set; }
        public IEnumerable<SelectListItem> ContactTypes { get; set; }

        public ClientSubUnitContactVM()
        {
        }
        public ClientSubUnitContactVM(ClientSubUnit clientSubUnit, ClientDetail clientDetail, Contact contact, IEnumerable<SelectListItem> contactTypes)
        {
            ClientSubUnit = clientSubUnit;
            ClientDetail = clientDetail;
            Contact = contact;
            ContactTypes = contactTypes;
        }
    }
}