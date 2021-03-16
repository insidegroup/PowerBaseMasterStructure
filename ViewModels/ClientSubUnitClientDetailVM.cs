using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientSubUnitClientDetailVM : CWTBaseViewModel
    {
        public ClientSubUnit ClientSubUnit { get; set; }
        public ClientDetail ClientDetail { get; set; }
        public IEnumerable<SelectListItem> TripTypes { get; set; }
        
        public ClientSubUnitClientDetailVM()
        {
        }
        public ClientSubUnitClientDetailVM(ClientSubUnit clientSubUnit, ClientDetail clientDetail, IEnumerable<SelectListItem> tripTypes)
        {
            ClientSubUnit = clientSubUnit;
            ClientDetail = clientDetail;
            TripTypes = tripTypes;
        }
    }
}