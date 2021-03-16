using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientAccountContactsVM : CWTBaseViewModel
    {
        public ClientAccount ClientAccount { get; set; }
        public ClientDetail ClientDetail { get; set; }
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientDetailContacts_v1Result> Contacts { get; set; }
        
        public ClientAccountContactsVM()
        {
        }
        public ClientAccountContactsVM(ClientAccount clientAccount, ClientDetail clientDetail, CWTPaginatedList<spDesktopDataAdmin_SelectClientDetailContacts_v1Result> contacts)
        {
            ClientAccount = clientAccount;
            ClientDetail = clientDetail;
            Contacts = contacts;
        }
    }
}