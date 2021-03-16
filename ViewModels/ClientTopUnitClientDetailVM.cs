using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientTopUnitClientDetailVM : CWTBaseViewModel
      {
        public ClientTopUnit ClientTopUnit { get; set; }
        public ClientDetail ClientDetail { get; set; }
        public IEnumerable<SelectListItem> TripTypes { get; set; }
        
        public ClientTopUnitClientDetailVM()
        {
        }
        public ClientTopUnitClientDetailVM(ClientTopUnit clientTopUnit, ClientDetail clientDetail, IEnumerable<SelectListItem> tripTypes)
        {
            ClientTopUnit = clientTopUnit;
            ClientDetail = clientDetail;
            TripTypes = tripTypes;
        }
    }
}