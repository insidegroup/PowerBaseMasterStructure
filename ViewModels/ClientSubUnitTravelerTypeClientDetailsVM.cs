using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientSubUnitTravelerTypeClientDetailsVM : CWTBaseViewModel
    {
        public ClientSubUnit ClientSubUnit { get; set; }
        public TravelerType TravelerType { get; set; }
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientSubUnitTravelerTypeClientDetails_v1Result> ClientDetails { get; set; }
        
        public ClientSubUnitTravelerTypeClientDetailsVM()
        {
        }
        public ClientSubUnitTravelerTypeClientDetailsVM(TravelerType travelerType, ClientSubUnit clientSubUnit, CWTPaginatedList<spDesktopDataAdmin_SelectClientSubUnitTravelerTypeClientDetails_v1Result> clientDetails)
        {
            TravelerType = travelerType;
            ClientSubUnit = clientSubUnit;
            ClientDetails = clientDetails;
        }
    }
}