using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using CWTDesktopDatabase.Models;
using System.Linq.Dynamic;

namespace CWTDesktopDatabase.Repository
{
    public class SystemUserLocationRepository
    {
        private SystemUserLocationDC db = new SystemUserLocationDC(Settings.getConnectionString());

        //Get a user
        public SystemUserLocation GetSystemUserLocation(string guid)
        {
            return (from u in db.SystemUserLocations where u.SystemUserGuid == guid select u).FirstOrDefault();
        }


        //Edit For Display
        public void EditForDisplay(SystemUserLocation systemUserLocation)
        {
            LocationRepository locationRepository = new LocationRepository();
            Location location = new Location();
            location = locationRepository.GetLocation(systemUserLocation.LocationId);

            systemUserLocation.LocationName = location.LocationName;
        }



        //Edit SystemUserLocation
        public void Update(SystemUserLocation systemUserLocation)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateSystemUserLocation_v1(
                systemUserLocation.SystemUserGuid,
                systemUserLocation.LocationId,
                adminUserGuid,
                systemUserLocation.VersionNumber
            );
        }

        //Add SystemUserLocation
        public void Add(SystemUserLocation systemUserLocation)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertSystemUserLocation_v1(
                systemUserLocation.SystemUserGuid,
                systemUserLocation.LocationId,
                adminUserGuid);
        }
    }
}
