using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
    public class LocationDeleteVM : CWTBaseViewModel
   {
        public Location Location { get; set; }
		public Address Address { get; set; }
        public List<spDDAWizard_SelectLocationSystemUsers_v1Result> SystemUsers { get; set; }
        public List<spDDAWizard_SelectLocationTeams_v1Result> Teams { get; set; }
        public LocationLinkedItemsVM LinkedItems { get; set; }
        
        
        
        public LocationDeleteVM()
        {
        }

        public LocationDeleteVM(
			Location location,
			Address address,
            List<spDDAWizard_SelectLocationSystemUsers_v1Result> systemUsers,
            List<spDDAWizard_SelectLocationTeams_v1Result> teams,
            LocationLinkedItemsVM linkedItems
            )
        {
			Location = location;
			Address = address;
            Teams = teams;
            SystemUsers = systemUsers;
            LinkedItems = linkedItems;
        }
    }
}
