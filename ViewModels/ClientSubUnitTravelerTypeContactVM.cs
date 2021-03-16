using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientSubUnitTravelerTypeContactVM : CWTBaseViewModel
    {
        public ClientSubUnit ClientSubUnit { get; set; }
        public TravelerType TravelerType { get; set; }
        public ClientDetail ClientDetail { get; set; }
        public Contact Contact { get; set; }
        public IEnumerable<SelectListItem> ContactTypes { get; set; }

        public ClientSubUnitTravelerTypeContactVM()
        {
        }
        public ClientSubUnitTravelerTypeContactVM(ClientSubUnit clientSubUnit, TravelerType travelerType, ClientDetail clientDetail, Contact contact, IEnumerable<SelectListItem> contactTypes)
        {
            ClientSubUnit = clientSubUnit;
            TravelerType = travelerType;
            ClientDetail = clientDetail;
            Contact = contact;
            ContactTypes = contactTypes;
        }
    }
}