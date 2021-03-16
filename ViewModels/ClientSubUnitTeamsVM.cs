using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientSubUnitTeamsVM : CWTBaseViewModel
    {
        public ClientSubUnit ClientSubUnit { get; set; }
        public List<spDesktopDataAdmin_SelectClientSubUnitTeams_v1Result> Teams { get; set; }
        
        public ClientSubUnitTeamsVM()
        {
        }
        public ClientSubUnitTeamsVM(ClientSubUnit clientSubUnit, List<spDesktopDataAdmin_SelectClientSubUnitTeams_v1Result> teams)
        {
            ClientSubUnit = clientSubUnit;
            Teams = teams;
        }
    }
}
