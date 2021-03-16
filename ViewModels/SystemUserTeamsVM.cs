using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
    public class SystemUserTeamsVM : CWTBaseViewModel
    {
        public SystemUser SystemUser { get; set; }
        public List<spDDAWizard_SelectSystemUserTeams_v1Result> Teams { get; set; }
        
        public SystemUserTeamsVM()
        {
        }
        public SystemUserTeamsVM(SystemUser systemUser, List<spDDAWizard_SelectSystemUserTeams_v1Result> teams)
        {
            SystemUser = systemUser;
            Teams = teams;
        }
    }
}
