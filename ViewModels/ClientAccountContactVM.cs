using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientAccountContactVM : CWTBaseViewModel
    {
        public ClientAccount ClientAccount { get; set; }
        public ClientDetail ClientDetail { get; set; }
        public Contact Contact { get; set; }
        public IEnumerable<SelectListItem> ContactTypes { get; set; }

        public ClientAccountContactVM()
        {
        }
        public ClientAccountContactVM(ClientAccount clientAccount, ClientDetail clientDetail, Contact contact, IEnumerable<SelectListItem> contactTypes)
        {
            ClientAccount = clientAccount;
            ClientDetail = clientDetail;
            Contact = contact;
            ContactTypes = contactTypes;
        }
    }
}