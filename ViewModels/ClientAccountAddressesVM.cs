using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientAccountAddressesVM : CWTBaseViewModel
    {
        public ClientAccount ClientAccount { get; set; }
        public ClientDetail ClientDetail { get; set; }
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientDetailAddresses_v1Result> Addresses { get; set; }
        
        public ClientAccountAddressesVM()
        {
        }
        public ClientAccountAddressesVM(ClientAccount clientAccount, ClientDetail clientDetail, CWTPaginatedList<spDesktopDataAdmin_SelectClientDetailAddresses_v1Result> addresses)
        {
            ClientAccount = clientAccount;
            ClientDetail = clientDetail;
            Addresses = addresses;
        }
    }
}