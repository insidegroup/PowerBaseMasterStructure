using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
    public class TeamClientSubUnitsVM : CWTBaseViewModel
    {
        public Team Team { get; set; }
        public List<spDDAWizard_SelectTeamClientSubUnits_v1Result> ClientSubUnits { get; set; }
       //public List<spDDAWizard_SelectClientSubUnitsFiltered_v1Result> AvailableClientSubUnits { get; set; }
        
        public TeamClientSubUnitsVM()
        {
        }
        public TeamClientSubUnitsVM(Team team, List<spDDAWizard_SelectTeamClientSubUnits_v1Result> clientSubUnits)
        {
            Team = team;
            ClientSubUnits = clientSubUnits;
        }
    }
}
