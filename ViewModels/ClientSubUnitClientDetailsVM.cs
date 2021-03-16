using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;


namespace CWTDesktopDatabase.ViewModels
{
    public class ClientSubUnitClientDetailsVM : CWTBaseViewModel
    {
        public ClientSubUnit ClientSubUnit { get; set; }
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientSubUnitClientDetails_v1Result> ClientDetails { get; set; }
        
        public ClientSubUnitClientDetailsVM()
        {
        }
        public ClientSubUnitClientDetailsVM(ClientSubUnit clientSubUnit, CWTPaginatedList<spDesktopDataAdmin_SelectClientSubUnitClientDetails_v1Result> clientDetails)
        {
            ClientSubUnit = clientSubUnit;
            ClientDetails = clientDetails;
        }
    }
}