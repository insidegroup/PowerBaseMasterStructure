using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using System.Web.Mvc;

namespace CWTDesktopDatabase.ViewModels
{
    
    public class SystemUserWizardVM : CWTBaseViewModel
    {
        public SystemUser SystemUser { get; set; }
        public SystemUserLocation SystemUserLocation { get; set; }
		public List<ExternalSystemLoginSystemUserCountry> ExternalSystemLoginSystemUserCountries { get; set; }
        public List<Team> TeamsAdded { get; set; }
        public List<Team> TeamsRemoved { get; set; }
        public List<fnDesktopDataAdmin_SelectSystemUserGDSs_v1Result> SystemUserGDSs { get; set; }
        public List<GDS> GDSs { get; set; }
		public bool GDSChanged { get; set; }
		public IEnumerable<SelectListItem> Countries { get; set; }
		public bool HasWriteAccessToLocation { get; set; }
        
        public SystemUserWizardVM()
        {
        }

        public SystemUserWizardVM(
			SystemUser systemUser, 
			SystemUserLocation systemUserLocation,
			List<ExternalSystemLoginSystemUserCountry> externalSystemLoginSystemUserCountries, 
			List<Team> teamsAdded, 
			List<Team> teamsRemoved, 
			List<fnDesktopDataAdmin_SelectSystemUserGDSs_v1Result> systemUserGDSs,
			List<GDS> gdss, 
			bool gdsChanged, 
			bool hasWriteAccessToLocation
		)
        {
            SystemUser = systemUser;
            SystemUserLocation = systemUserLocation;
			ExternalSystemLoginSystemUserCountries = externalSystemLoginSystemUserCountries;
            TeamsAdded = teamsAdded;
            TeamsRemoved = teamsRemoved;
            SystemUserGDSs = systemUserGDSs;
            GDSs = gdss;
            GDSChanged = gdsChanged;
            HasWriteAccessToLocation = hasWriteAccessToLocation;
        }
    }
}
