using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using CWTDesktopDatabase.Models;
using System.Linq.Dynamic;


namespace CWTDesktopDatabase.Repository
{
    public class ClientSubUnitTeamRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Return All Teams Linked to a ClientSubUnit
        public List<spDesktopDataAdmin_SelectClientSubUnitTeams_v1Result> GetClientSubUnitTeams(string clientSubUnitGuid)
        {
            return db.spDesktopDataAdmin_SelectClientSubUnitTeams_v1(clientSubUnitGuid).ToList();
        }
        
        //Return Primary Team for a ClientSubUnit
        public Team GetClientSubUnitPrimaryTeam(string clientSubUnitGuid)
        {
            Team team = new Team();

            var result = db.spDesktopDataAdmin_SelectClientSubUnitTeams_v1(clientSubUnitGuid).Where(x => x.IsPrimaryTeamForSub == true).FirstOrDefault();

            if (result != null)
            {
                if (result.TeamId > 0)
                {
                    TeamRepository teamRepository = new TeamRepository();
                    team = teamRepository.GetTeam(result.TeamId);
                }
            }

            return team;
        }

        public bool IsValidClientSubUnitForTeam(string id, int teamid)
        {
            var result = from n in db.spDesktopDataAdmin_SelectTeamAvailableClientSubUnits_v1(teamid)
                         where  n.ClientSubUnitGuid.Equals(id)
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
        public ClientSubUnitTeam GetItem(int id, string clientSubUnitGuid)
        {
            return db.ClientSubUnitTeams.SingleOrDefault(c => (c.ClientSubUnitGuid == clientSubUnitGuid)
                    && (c.TeamId == id));
        }

        //Add Data From Linked Tables for Display
        public void EditForDisplay(ClientSubUnitTeam clientSubUnitTeam)
        {
            ClientSubUnitRepository clientSubUnitTeamRepository = new ClientSubUnitRepository();
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitTeamRepository.GetClientSubUnit(clientSubUnitTeam.ClientSubUnitGuid);
            if (clientSubUnit != null)
            {
                clientSubUnitTeam.ClientSubUnitName = clientSubUnit.ClientSubUnitName;
            }

            TeamRepository teamRepository = new TeamRepository();
            Team team = new Team();
            team = teamRepository.GetTeam(clientSubUnitTeam.TeamId);
            if (team != null)
            {
                clientSubUnitTeam.TeamName = team.TeamName;
            }
        }

        //Add to DB
        public void Add(ClientSubUnitTeam systemUserTeam)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertClientSubUnitTeam_v1(
                systemUserTeam.ClientSubUnitGuid,
                systemUserTeam.TeamId,
                systemUserTeam.IncludeInClientDroplistFlag,
                adminUserGuid
            );

        }

        //Edit in DB
        public void Update(ClientSubUnitTeam systemUserTeam)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateClientSubUnitTeam_v1(
                systemUserTeam.TeamId,
                systemUserTeam.ClientSubUnitGuid,
                systemUserTeam.IncludeInClientDroplistFlag,
                adminUserGuid,
                systemUserTeam.VersionNumber
            );

        }

        //Delete from +DB
        public void Delete(ClientSubUnitTeam systemUserTeam)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeleteClientSubUnitTeam_v1(
                systemUserTeam.ClientSubUnitGuid,
                systemUserTeam.TeamId,
                adminUserGuid,
                systemUserTeam.VersionNumber
            );
        }
    }
}
