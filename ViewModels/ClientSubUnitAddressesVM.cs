using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientSubUnitAddressesVM : CWTBaseViewModel
    {
        public ClientSubUnit ClientSubUnit { get; set; }
        public ClientDetail ClientDetail { get; set; }
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientDetailAddresses_v1Result> Addresses { get; set; }
        
        public ClientSubUnitAddressesVM()
        {
        }
        public ClientSubUnitAddressesVM(ClientSubUnit clientSubUnit, ClientDetail clientDetail, CWTPaginatedList<spDesktopDataAdmin_SelectClientDetailAddresses_v1Result> addresses)
        {
            ClientSubUnit = clientSubUnit;
            ClientDetail = clientDetail;
            Addresses = addresses;
        }
    }
}