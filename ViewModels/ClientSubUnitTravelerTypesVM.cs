using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientSubUnitTravelerTypesVM : CWTBaseViewModel
     {
        public ClientSubUnit ClientSubUnit { get; set; }
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientSubUnitTravelerTypes_v1Result> TravelerTypes { get; set; }

        public ClientSubUnitTravelerTypesVM()
        {
        }
        public ClientSubUnitTravelerTypesVM(ClientSubUnit clientSubUnit, CWTPaginatedList<spDesktopDataAdmin_SelectClientSubUnitTravelerTypes_v1Result> travelerTypes)
        {
            ClientSubUnit = clientSubUnit;
            TravelerTypes = travelerTypes;

        }
    }
}