using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
    public class TeamClientSubUnitsAndSystemUsersVM : CWTBaseViewModel
    {
        public Team Team { get; set; }
        public List<spDDAWizard_SelectTeamSystemUsers_v1Result> SystemUsers { get; set; }
        public List<spDDAWizard_SelectTeamClientSubUnits_v1Result> ClientSubUnits { get; set; }
        
        public TeamClientSubUnitsAndSystemUsersVM()
        {
        }
        public TeamClientSubUnitsAndSystemUsersVM(Team team, List<spDDAWizard_SelectTeamSystemUsers_v1Result> systemUsers, List<spDDAWizard_SelectTeamClientSubUnits_v1Result> clientSubUnits)
        {
            Team = team;
            SystemUsers = systemUsers;
            ClientSubUnits = clientSubUnits;
        }
    }
}
