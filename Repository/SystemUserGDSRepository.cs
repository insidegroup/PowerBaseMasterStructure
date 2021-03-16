using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using CWTDesktopDatabase.Models;
using System.Linq.Dynamic;


namespace CWTDesktopDatabase.Repository
{
    public class SystemUserGDSRepository
    {
        //data
        private SystemUserGDSDC db = new SystemUserGDSDC(Settings.getConnectionString());
        //private string groupName = "Maintenance Admin";


        //List of All SystemUser's GDSs
        public IQueryable<fnDesktopDataAdmin_SelectSystemUserGDSs_v1Result> GetSystemUserGDSs(string systemUserGuid)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            return db.fnDesktopDataAdmin_SelectSystemUserGDSs_v1(systemUserGuid, adminUserGuid).OrderBy(c => c.GDSName);
        }

        public SystemUserGDS GetSystemUserGDS(string systemUserGuid, string gdsCode)
        {
            var result = (from n in db.spDesktopDataAdmin_SelectSystemUserGDS_v1(systemUserGuid, gdsCode)
                         select
                         new SystemUserGDS
                         {
                             GDSCode = n.GDSCode,
                             SystemUserGuid = n.SystemUserGuid,
                             DefaultGDS = n.DefaultGDS,
                             VersionNumber = n.VersionNumber,
                             PseudoCityOrOfficeId = n.PseudoCityOrOfficeId,
                             GDSSignOn = n.GDSSignOn
                         }).FirstOrDefault();
            return result;
        }

        //Add Data From Linked Tables for Display
        public void EditForDisplay(SystemUserGDS systemUserGDS)
        {
            SystemUserRepository systemUserRepository = new SystemUserRepository();
            SystemUser systemUser = new SystemUser();
            systemUser = systemUserRepository.GetUserBySystemUserGuid(systemUserGDS.SystemUserGuid);
            if (systemUser != null)
            {
                systemUserGDS.SystemUserName = (systemUser.LastName + ", " + systemUser.FirstName + " " + systemUser.MiddleName).Replace("  ", " ");
            }

            GDSRepository gdsRepository = new GDSRepository();
            GDS gds = new GDS();
            gds = gdsRepository.GetGDS(systemUserGDS.GDSCode);
            if (gds != null)
            {
                systemUserGDS.GDSName = gds.GDSName;
            }
        }

        //Add to DB
        public void Add(SystemUserGDS systemUserGDS)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertSystemUserGDS_v1(
                systemUserGDS.GDSCode,
                systemUserGDS.SystemUserGuid,
                systemUserGDS.DefaultGDS,
                systemUserGDS.PseudoCityOrOfficeId,
                systemUserGDS.GDSSignOn,
                adminUserGuid
            );

        }

        //Delete from DB
        public void Delete(SystemUserGDS systemUserGDS)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeleteSystemUserGDS_v1(
                systemUserGDS.GDSCode,
                systemUserGDS.SystemUserGuid,
                adminUserGuid,
                systemUserGDS.VersionNumber
            );

        }

        //Update DB
        public void Update(SystemUserGDS systemUserGDS)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateSystemUserGDS_v1(
                systemUserGDS.GDSCode,
                systemUserGDS.SystemUserGuid,
                systemUserGDS.DefaultGDS,
                systemUserGDS.PseudoCityOrOfficeId,
                systemUserGDS.GDSSignOn,
                adminUserGuid,
                systemUserGDS.VersionNumber
            );

        }

        //GDS that user is not linked to 
        public List<SystemUserGDS> GetUnUsedGDSs(string id)
        {

            var result = from n in db.spDesktopDataAdmin_SelectSystemUserAvailableGDSs_v1(id)
                         select new SystemUserGDS
                         {
                             GDSCode = n.GDSCode,
                             GDSName = n.GDSName
                         };
            return result.ToList();
        }

    }
}
