using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
namespace CWTDesktopDatabase.ViewModels
{
    public class TravelerTypeAddressesVM : CWTBaseViewModel
    {
        public TravelerType TravelerType { get; set; }
        public ClientSubUnit ClientSubUnit { get; set; }
        public ClientDetail ClientDetail { get; set; }
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientDetailAddresses_v1Result> Addresses { get; set; }

        public TravelerTypeAddressesVM()
        {
        }
        public TravelerTypeAddressesVM(TravelerType travelerType, ClientSubUnit clientSubUnit, ClientDetail clientDetail, CWTPaginatedList<spDesktopDataAdmin_SelectClientDetailAddresses_v1Result> addresses)
        {
            TravelerType = travelerType;
            ClientSubUnit = clientSubUnit;
            ClientDetail = clientDetail;
            Addresses = addresses;
        }
    }
}