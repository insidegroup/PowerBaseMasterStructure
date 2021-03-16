using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;


namespace CWTDesktopDatabase.ViewModels
{
    public class ClientTopUnitClientDetailsVM : CWTBaseViewModel
    {
        public ClientTopUnit ClientTopUnit { get; set; }
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientTopUnitClientDetails_v1Result> ClientDetails { get; set; }
        
        public ClientTopUnitClientDetailsVM()
        {
        }
        public ClientTopUnitClientDetailsVM(ClientTopUnit clientTopUnit, CWTPaginatedList<spDesktopDataAdmin_SelectClientTopUnitClientDetails_v1Result> clientDetails)
        {
            ClientTopUnit = clientTopUnit;
            ClientDetails = clientDetails;
        }
    }
}