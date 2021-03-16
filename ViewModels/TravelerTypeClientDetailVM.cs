using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;


namespace CWTDesktopDatabase.ViewModels
{
    public class TravelerTypeClientDetailVM : CWTBaseViewModel
    {
        public TravelerType TravelerType { get; set; }
        public ClientSubUnit ClientSubUnit { get; set; }
        public ClientDetail ClientDetail { get; set; }
        public IEnumerable<SelectListItem> TripTypes { get; set; }
        
        public TravelerTypeClientDetailVM()
        {
        }
        public TravelerTypeClientDetailVM(TravelerType travelerType, ClientSubUnit clientSubUnit, ClientDetail clientDetail, IEnumerable<SelectListItem> tripTypes)
        {
            TravelerType = travelerType;
            ClientSubUnit = clientSubUnit;
            ClientDetail = clientDetail;
            TripTypes = tripTypes;
        }
    }
}