using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientTopUnitClientLocationsVM : CWTBaseViewModel
   {
        public ClientTopUnit ClientTopUnit { get; set; }
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientTopUnitClientLocations_v1Result> ClientLocations { get; set; }
        
        public ClientTopUnitClientLocationsVM()
        {
        }
		public ClientTopUnitClientLocationsVM(
			ClientTopUnit clientTopUnit,
			CWTPaginatedList<spDesktopDataAdmin_SelectClientTopUnitClientLocations_v1Result> clientLocations)
        {
            ClientTopUnit = clientTopUnit;
            ClientLocations = clientLocations;
        }
    }
}