using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
    
    public class LocationWizardVM : CWTBaseViewModel
    {
        public Location Location { get; set; }
        public Address Address { get; set; }
        public IEnumerable<SelectListItem> MappingQualityCodes { get; set; }
        public List<SystemUser> SystemUsersAdded { get; set; }
        public List<SystemUser> SystemUsersRemoved { get; set; }
        public int SystemUserCount { get; set; }
        public IEnumerable<SelectListItem> StateProvinces { get; set; }

        public LocationWizardVM()
        {
        }
        public LocationWizardVM(Location location, Address address, IEnumerable<SelectListItem> mappingQualityCodes, List<SystemUser> systemUsersAdded, List<SystemUser> systemUsersRemoved, int systemUserCount)
        {
            Location = location;
            Address = address;
            MappingQualityCodes = mappingQualityCodes;
            SystemUsersAdded = systemUsersAdded;
            SystemUsersRemoved = systemUsersRemoved;
            SystemUserCount = systemUserCount;
        }
    }
}
