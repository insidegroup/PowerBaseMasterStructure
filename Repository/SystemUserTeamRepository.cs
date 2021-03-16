using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using CWTDesktopDatabase.Models;
using System.Linq.Dynamic;


namespace CWTDesktopDatabase.Repository
{
    public class SystemUserTeamRepository
    {
        private SystemUserTeamDC db = new SystemUserTeamDC(Settings.getConnectionString());

        public List<spDesktopDataAdmin_SelectSystemUserAvailableTeams_v1Result> LookUpNonSystemUserTeams(string id, string searchText)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            var result = from n in db.spDesktopDataAdmin_SelectSystemUserAvailableTeams_v1(id, adminUserGuid)
                         where (n.TeamName.ToUpper().Contains(searchText.ToUpper()))
                         select n;
            return result.Take(15).ToList();
        }

        public List<spDesktopDataAdmin_SelectTeamAvailableSystemUsers_v1Result> LookUpNonTeamSystemUsers(int id, string searchText)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = from n in db.spDesktopDataAdmin_SelectTeamAvailableSystemUsers_v1(id, adminUserGuid)
                         where (n.LastName.ToUpper().Contains(searchText.ToUpper()) || n.FirstName.Contains(searchText.ToUpper()))
                         select n;
            return result.Take(15).ToList();
        }

        public List<spDesktopDataAdmin_SelectSystemUserTeams_v1Result> GetSystemUserTeams(string systemUserGuid)
        {
            return db.spDesktopDataAdmin_SelectSystemUserTeams_v1(systemUserGuid).ToList();
        }

        public bool IsValidSystemUserForTeam(string id, int teamId)
        {
            var result = from n in db.SystemUserTeams
                         where (  n.SystemUserGuid.Equals(id) &&   n.TeamId == teamId)
                         select n;
            if (result.Count() == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsValidTeamForSystemUser(string id, string teamName)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = from n in db.spDesktopDataAdmin_SelectSystemUserAvailableTeams_v1(id, adminUserGuid)
                         where (n.TeamName.Equals(teamName))
                         select n;
            if (result.Count() == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Get one Item
        public SystemUserTeam GetItem(int id, string systemUserGuid)
        {
            return db.SystemUserTeams.SingleOrDefault(c => (c.SystemUserGuid == systemUserGuid)
                    && (c.TeamId == id));
        }

        //Add Data From Linked Tables for Display
        public void EditForDisplay(SystemUserTeam systemUserTeam)
        {
            SystemUserRepository systemUserRepository = new SystemUserRepository();
            SystemUser systemUser = new SystemUser();
            systemUser = systemUserRepository.GetUserBySystemUserGuid(systemUserTeam.SystemUserGuid);
            if (systemUser != null)
            {
                systemUserTeam.SystemUserName = (systemUser.LastName + ", " + systemUser.FirstName + " " + systemUser.MiddleName).Replace("  ", " ");
            }

            TeamRepository teamRepository = new TeamRepository();
            Team team = new Team();
            team = teamRepository.GetTeam(systemUserTeam.TeamId);
            if (team != null)
            {
                systemUserTeam.TeamName = team.TeamName;
            }
        }

        //Add to DB
        public void Add(SystemUserTeam systemUserTeam)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertSystemUserTeam_v1(
                systemUserTeam.SystemUserGuid,
                systemUserTeam.TeamId,
                adminUserGuid
            );

        }

        //Delete from +DB
        public void Delete(SystemUserTeam systemUserTeam)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeleteSystemUserTeam_v1(
                systemUserTeam.SystemUserGuid,
                systemUserTeam.TeamId,
                adminUserGuid,
                systemUserTeam.VersionNumber
            );
        }
    }
}
