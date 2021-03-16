using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientSubUnitContactsVM : CWTBaseViewModel
   {
        public ClientSubUnit ClientSubUnit { get; set; }
        public ClientDetail ClientDetail { get; set; }
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientDetailContacts_v1Result> Contacts { get; set; }
        
        public ClientSubUnitContactsVM()
        {
        }
        public ClientSubUnitContactsVM(ClientSubUnit clientSubUnit, ClientDetail clientDetail, CWTPaginatedList<spDesktopDataAdmin_SelectClientDetailContacts_v1Result> contacts)
        {
            ClientSubUnit = clientSubUnit;
            ClientDetail = clientDetail;
            Contacts = contacts;
        }
    }
}