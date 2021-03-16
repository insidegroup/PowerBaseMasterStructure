using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
    
    public class TeamWizardVM : CWTBaseViewModel
    {
        /*
         * We use SystemUser instead of SystemUserTeam because we need the IncludeInClientDroplistFlag value + VersionNumber
         *  for ClientSubUnitTeam, but not for SystemUserTeam
         */
        public Team Team { get; set; }
        public List<SystemUser> SystemUsersAdded { get; set; }
        public List<SystemUser> SystemUsersRemoved { get; set; }
        public List<ClientSubUnitTeam> ClientSubUnitsAdded { get; set; }
        public List<ClientSubUnitTeam> ClientSubUnitsRemoved { get; set; }
        public List<ClientSubUnitTeam> ClientSubUnitsAltered { get; set; }
        
        public TeamWizardVM()
        {
        }
        public TeamWizardVM(Team team, List<SystemUser> systemUsersAdded, List<SystemUser> systemUsersRemoved, List<ClientSubUnitTeam> clientSubUnitsAdded, List<ClientSubUnitTeam> clientSubUnitsRemoved, List<ClientSubUnitTeam> clientSubUnitsAltered)
        {
            Team = team;
            SystemUsersAdded = systemUsersAdded;
            SystemUsersRemoved = systemUsersRemoved;
            ClientSubUnitsAdded = clientSubUnitsAdded;
            ClientSubUnitsRemoved = clientSubUnitsRemoved;
            ClientSubUnitsAltered = clientSubUnitsAltered;
        }
    }
}
