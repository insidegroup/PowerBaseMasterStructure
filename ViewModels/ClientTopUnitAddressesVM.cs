using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientTopUnitAddressesVM : CWTBaseViewModel
    {
        public ClientTopUnit ClientTopUnit { get; set; }
        public ClientDetail ClientDetail { get; set; }
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientDetailAddresses_v1Result> Addresses { get; set; }
        
        public ClientTopUnitAddressesVM()
        {
        }
        public ClientTopUnitAddressesVM(ClientTopUnit clientTopUnit, ClientDetail clientDetail, CWTPaginatedList<spDesktopDataAdmin_SelectClientDetailAddresses_v1Result> addresses)
        {
            ClientTopUnit = clientTopUnit;
            ClientDetail = clientDetail;
            Addresses = addresses;
        }
    }
}