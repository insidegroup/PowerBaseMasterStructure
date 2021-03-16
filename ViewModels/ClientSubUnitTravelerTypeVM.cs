using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientSubUnitTravelerTypeVM : CWTBaseViewModel
    {
        public ClientSubUnit ClientSubUnit { get; set; }
        public TravelerType TravelerType { get; set; }

        public ClientSubUnitTravelerTypeVM()
        {
        }
        public ClientSubUnitTravelerTypeVM(ClientSubUnit clientSubUnit, TravelerType travelerType)
        {
            ClientSubUnit = clientSubUnit;
            TravelerType = travelerType;

        }
    }
}