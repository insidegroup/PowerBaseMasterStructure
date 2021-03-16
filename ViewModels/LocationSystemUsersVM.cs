using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
    public class LocationSystemUsersVM : CWTBaseViewModel
    {
        public Location Location { get; set; }
        public List<spDDAWizard_SelectLocationSystemUsers_v1Result> SystemUsers { get; set; }
        public int LocationTeamCount { get; set; }  //used to show Next button in Wizard
        
        public LocationSystemUsersVM()
        {
        }
        public LocationSystemUsersVM(Location location, int locationTeamCount, List<spDDAWizard_SelectLocationSystemUsers_v1Result> systemUsers)
        {
            Location = location;
            SystemUsers = systemUsers;
            LocationTeamCount = locationTeamCount;
        }
    }
}
