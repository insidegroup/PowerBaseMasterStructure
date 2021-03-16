using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using System.Linq.Dynamic;
using System.Web.Security;

namespace CWTDesktopDatabase.Repository
{
    public class TravelerTypeSponsorRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get one Item
        public TravelerTypeSponsor GetTravelerTypeSponsor(string id)
        {
            return db.TravelerTypeSponsors.SingleOrDefault(c => c.TravelerTypeGuid == id);
        }

        public void Add(TravelerTypeSponsor travelerTypeSponsor)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertTravelerTypeSponsor_v1(
                travelerTypeSponsor.TravelerTypeGuid,
                travelerTypeSponsor.IsSponsorFlag,
                travelerTypeSponsor.RequiresSponsorFlag,
                adminUserGuid
            );
        }
       
        public void Edit(TravelerTypeSponsor travelerTypeSponsor)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateTravelerTypeSponsor_v1(
                travelerTypeSponsor.TravelerTypeGuid,
                travelerTypeSponsor.IsSponsorFlag,
                travelerTypeSponsor.RequiresSponsorFlag,
                adminUserGuid,
                travelerTypeSponsor.VersionNumber
            );
        }
    }
}
