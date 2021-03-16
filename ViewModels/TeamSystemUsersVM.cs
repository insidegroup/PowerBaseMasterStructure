using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
    public class TeamSystemUsersVM : CWTBaseViewModel
    {
        public Team Team { get; set; }
        public List<spDDAWizard_SelectTeamSystemUsers_v1Result> SystemUsers { get; set; }
        //public List<spDDAWizard_SelectSystemUsersFiltered_v1Result> AvailableSystemUsers { get; set; }
        
        public TeamSystemUsersVM()
        {
        }
        public TeamSystemUsersVM(Team team, List<spDDAWizard_SelectTeamSystemUsers_v1Result> systemUsers)
        {
            Team = team;
            SystemUsers = systemUsers;
        }
    }
}
