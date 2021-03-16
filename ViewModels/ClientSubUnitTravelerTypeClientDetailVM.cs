using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientSubUnitTravelerTypeClientDetailVM : CWTBaseViewModel
    {
        public ClientSubUnit ClientSubUnit { get; set; }
        public TravelerType TravelerType { get; set; }
        public ClientDetail ClientDetail { get; set; }
        public IEnumerable<SelectListItem> TripTypes { get; set; }
        
        public ClientSubUnitTravelerTypeClientDetailVM()
        {
        }
        public ClientSubUnitTravelerTypeClientDetailVM(ClientSubUnit clientSubUnit, TravelerType travelerType, ClientDetail clientDetail, IEnumerable<SelectListItem> tripTypes)
        {
            ClientSubUnit = clientSubUnit;
            TravelerType = travelerType;
            ClientDetail = clientDetail;
            TripTypes = tripTypes;
        }
    }
}