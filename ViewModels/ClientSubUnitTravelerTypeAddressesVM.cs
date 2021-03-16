using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientSubUnitTravelerTypeAddressesVM : CWTBaseViewModel
    {
        public ClientSubUnit ClientSubUnit { get; set; }
        public TravelerType TravelerType { get; set; }
        public ClientDetail ClientDetail { get; set; }
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientDetailAddresses_v1Result> Addresses { get; set; }
        
        public ClientSubUnitTravelerTypeAddressesVM()
        {
        }
        public ClientSubUnitTravelerTypeAddressesVM(ClientSubUnit clientSubUnit, TravelerType travelerType, ClientDetail clientDetail, CWTPaginatedList<spDesktopDataAdmin_SelectClientDetailAddresses_v1Result> addresses)
        {
            ClientSubUnit = clientSubUnit;
            TravelerType = travelerType;
            ClientDetail = clientDetail;
            Addresses = addresses;
        }
    }
}