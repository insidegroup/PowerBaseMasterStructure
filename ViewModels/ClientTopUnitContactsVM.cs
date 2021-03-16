using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientTopUnitContactsVM : CWTBaseViewModel
   {
        public ClientTopUnit ClientTopUnit { get; set; }
        public ClientDetail ClientDetail { get; set; }
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientDetailContacts_v1Result> Contacts { get; set; }
        
        public ClientTopUnitContactsVM()
        {
        }
        public ClientTopUnitContactsVM(ClientTopUnit clientTopUnit, ClientDetail clientDetail, CWTPaginatedList<spDesktopDataAdmin_SelectClientDetailContacts_v1Result> contacts)
        {
            ClientTopUnit = clientTopUnit;
            ClientDetail = clientDetail;
            Contacts = contacts;
        }
    }
}