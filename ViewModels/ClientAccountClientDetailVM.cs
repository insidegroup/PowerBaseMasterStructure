using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientAccountClientDetailVM : CWTBaseViewModel
   {
        public ClientAccount ClientAccount { get; set; }
        public ClientDetail ClientDetail { get; set; }
        public IEnumerable<SelectListItem> TripTypes { get; set; }
        
        public ClientAccountClientDetailVM()
        {
        }
        public ClientAccountClientDetailVM(ClientAccount clientAccount, ClientDetail clientDetail, IEnumerable<SelectListItem> tripTypes)
        {
            ClientAccount = clientAccount;
            ClientDetail = clientDetail;
            TripTypes = tripTypes;
        }
    }
}