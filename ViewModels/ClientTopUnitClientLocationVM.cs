using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientTopUnitClientLocationVM : CWTBaseViewModel
    {
        public ClientTopUnit ClientTopUnit { get; set; }
		public ClientTopUnitClientLocation ClientTopUnitClientLocation { get; set; }
		public IEnumerable<SelectListItem> Countries { get; set; }

        public ClientTopUnitClientLocationVM()
        {
        }
		public ClientTopUnitClientLocationVM(ClientTopUnit clientTopUnit, ClientTopUnitClientLocation clientTopUnitClientLocation, IEnumerable<SelectListItem> countries)
        {
            ClientTopUnit = clientTopUnit;
			ClientTopUnitClientLocation = clientTopUnitClientLocation;
			Countries = countries;
        }
    }
}