using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;


namespace CWTDesktopDatabase.ViewModels
{
    public class TravelerTypeClientDetailsVM : CWTBaseViewModel
    {
        public TravelerType TravelerType { get; set; }
        public ClientSubUnit ClientSubUnit { get; set; }
        public CWTPaginatedList<spDesktopDataAdmin_SelectTravelerTypeClientDetails_v1Result> ClientDetails { get; set; }
        
        public TravelerTypeClientDetailsVM()
        {
        }
        public TravelerTypeClientDetailsVM(TravelerType travelerType, ClientSubUnit clientSubUnit, CWTPaginatedList<spDesktopDataAdmin_SelectTravelerTypeClientDetails_v1Result> clientDetails)
        {
            TravelerType = travelerType;
            ClientSubUnit = clientSubUnit;
            ClientDetails = clientDetails;
        }
    }
}