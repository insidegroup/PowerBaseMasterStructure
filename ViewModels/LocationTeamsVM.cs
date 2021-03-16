using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
    public class LocationTeamsVM : CWTBaseViewModel
    {
        public Location Location { get; set; }
        public List<spDDAWizard_SelectLocationTeams_v1Result> Teams { get; set; }
        public int LinkedItemsCount { get; set; }

        public LocationTeamsVM()
        {
        }
        public LocationTeamsVM(Location location, List<spDDAWizard_SelectLocationTeams_v1Result> teams, int linkedItemsCount)
        {
            Location = location;
            Teams = teams;
            LinkedItemsCount = linkedItemsCount;
        }
    }
}
