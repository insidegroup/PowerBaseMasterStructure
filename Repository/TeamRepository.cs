using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Linq.Dynamic;

namespace CWTDesktopDatabase.Repository
{
    public class TeamRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get a Page of Teams - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectTeams_v1Result> PageTeams(int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectTeams_v1(filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectTeams_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        public List<HierarchyJSON> GetTeamByName(string searchText)
        {
            var result = from n in db.Teams
                         where n.TeamName.Trim().Equals(searchText)
                         orderby n.TeamName
                         select
                             new HierarchyJSON
                             {
                                 HierarchyName = n.TeamName.Trim(),
                                 HierarchyCode = n.TeamId.ToString()
                             };
            return result.ToList();
        }

     
        //List of All Systemusers of a Team - Sortable
        public IQueryable<fnDesktopDataAdmin_SelectTeamSystemUsers_v1Result> GetTeamSystemUsers(string filter, int id, string sortField, int sortOrder)
        {
            if (sortOrder == 0)
            {
                sortField = sortField + " ascending";
            }
            else
            {
                sortField = sortField + " descending";
            }

            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            if (filter == "")
            {
                return db.fnDesktopDataAdmin_SelectTeamSystemUsers_v1(id, adminUserGuid).OrderBy(sortField);
            }
            else
            {
                return db.fnDesktopDataAdmin_SelectTeamSystemUsers_v1(id, adminUserGuid).OrderBy(sortField).Where(c => c.LastName.Contains(filter));
            }
        }

		//Get a Page of System Team Users - for Page Listings
		public CWTPaginatedList<spDesktopDataAdmin_SelectTeamSystemUsers_v1Result> PageTeamSystemUsers(int teamId, string filter, int page, string sortField, int sortOrder)
		{
			//get a page of records
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectTeamSystemUsers_v1(teamId, adminUserGuid, page, sortField, sortOrder, filter).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}

			//put into page object
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectTeamSystemUsers_v1Result>(result, page, totalRecords);

			//return to user
			return paginatedView;
		}
		
		//Get a Page of PolicyGroups - for Page Listings
		public CWTPaginatedList<spDesktopDataAdmin_SelectTeamClientSubUnits_v1Result> PageTeamClientSubUnits(int teamId, int page, string sortField, int sortOrder)
		{
			//get a page of records
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectTeamClientSubUnits_v1(teamId, sortField, sortOrder, page, adminUserGuid).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}

			//put into page object
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectTeamClientSubUnits_v1Result>(result, page, totalRecords);

			//return to user
			return paginatedView;
		}


        public List<spDesktopDataAdmin_SelectTeamSystemUserList_v1Result> GetAllSystemUsers(int teamId)
        {
            return db.spDesktopDataAdmin_SelectTeamSystemUserList_v1(teamId).ToList();
        }

        //Get one Team
        public Team GetTeam(int id)
        {
            return (from n in db.spDesktopDataAdmin_SelectTeam_v1(id)
                    select
                    new Team
                    {
                        TeamId = n.TeamId,
                        TeamName = n.TeamName,
                        TeamEmail   = n.TeamEmail,
                        TeamPhoneNumber = n.TeamPhoneNumber,
                        TeamQueue = n.TeamQueue,
                        TeamTypeCode = n.TeamTypeCode,
                        CityCode = n.CityCode,
                        VersionNumber = n.VersionNumber
                    }).FirstOrDefault();
        }

        //returns a list of Teams that a user can move to
        //returns Teams that user has access to, except for the current team
        public List<spDesktopDataAdmin_SelectAdminUserTeams_v1Result> GetMoveToTeams(int teamId)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result =  from c in db.spDesktopDataAdmin_SelectAdminUserTeams_v1(adminUserGuid).Where(c => c.TeamId != teamId) select c;
            return result.ToList();
        }

        //Add to DB
        public void Add(Team team)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            int? teamId = -1;

            teamId = db.spDesktopDataAdmin_InsertTeam_v1(
                ref teamId,
                team.TeamName,
                team.TeamEmail,
                team.TeamPhoneNumber,
                team.TeamQueue,
                team.TeamTypeCode,
                team.CityCode,
                team.HierarchyType,
                team.HierarchyCode,
                adminUserGuid
            );
        }

        //Change the deleted status on an item
        public void Update(Team team)
        {
           string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

           db.spDesktopDataAdmin_UpdateTeam_v1(
                team.TeamId,
                team.TeamName,
                team.TeamEmail,
                team.TeamPhoneNumber,
                team.TeamQueue,
                team.TeamTypeCode,
                team.CityCode,
                team.HierarchyType,
                team.HierarchyCode,
                adminUserGuid,
                team.VersionNumber
            );
            

        }

        //Delete From DB
        public void Delete(Team team)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeleteTeam_v1(
                team.TeamId,
                adminUserGuid,
                team.VersionNumber
            );
        }

        //Copy Users from one team to another
        public void CopyAllTeamUsers(int teamId, int newTeamId)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];  
            db.spDesktopDataAdmin_CopyTeamSystemUsersToTeam_v1(null, teamId, newTeamId, adminUserGuid);
        }

        //Copy Users from one team to another
        public void CopySomeTeamUsers(string xmlElement, int teamId, int newTeamId)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_CopyTeamSystemUsersToTeam_v1(System.Xml.Linq.XElement.Parse(xmlElement), teamId, newTeamId, adminUserGuid);
        }

        //Copy Users from one team to another
        public void MoveAllTeamUsers(int teamId, int newTeamId)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            db.spDesktopDataAdmin_MoveTeamSystemUsersToTeam_v1(null, teamId, newTeamId, adminUserGuid);
        }

        //Copy Users from one team to another
        public void MoveSomeTeamUsers(string xmlElement, int teamId, int newTeamId)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_MoveTeamSystemUsersToTeam_v1(System.Xml.Linq.XElement.Parse(xmlElement), teamId, newTeamId, adminUserGuid);
        }

   
        //Add Data From Linked Tables for Display
        public void EditGroupForDisplay(Team group)
        {

            TeamTypeRepository teamTypeRepository = new TeamTypeRepository();
            TeamType teamType = new TeamType();
            teamType = teamTypeRepository.GetTeamType(group.TeamTypeCode);
            if (teamType != null)
            {
                group.TeamTypeDescription = teamType.TeamTypeDescription;
            }


            fnDesktopDataAdmin_SelectTeamHierarchy_v1Result hierarchy = new fnDesktopDataAdmin_SelectTeamHierarchy_v1Result();
            hierarchy = GetGroupHierarchy(group.TeamId);
            if (hierarchy != null)
            {
                group.HierarchyType = hierarchy.HierarchyType;
                group.HierarchyCode = hierarchy.HierarchyCode.ToString();
                group.HierarchyItem = hierarchy.HierarchyName.Trim();

                if (hierarchy.HierarchyType == "ClientSubUnitTravelerType")
                {
                    group.ClientSubUnitGuid = hierarchy.HierarchyCode.ToString();
                    group.ClientSubUnitName = hierarchy.HierarchyName.Trim();
                    group.TravelerTypeGuid = hierarchy.TravelerTypeGuid;
                    group.TravelerTypeName = hierarchy.TravelerTypeName.Trim();
                }
            }
        }

        //Get Hierarchy Details
        public fnDesktopDataAdmin_SelectTeamHierarchy_v1Result GetGroupHierarchy(int id)
        {
            var result = db.fnDesktopDataAdmin_SelectTeamHierarchy_v1(id).FirstOrDefault();
            return result;
        }

        //REMOVED: List of All Teams - Sortable
        /*public IQueryable<fnDesktopDataAdmin_SelectTeams_v1Result> GetTeams(string filter, string sortField, int sortOrder)
        {
            if (sortOrder == 0)
            {
                sortField = sortField + " ascending";
            }
            else
            {
                sortField = sortField + " descending";
            }

            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            if (filter == "")
            {
                return db.fnDesktopDataAdmin_SelectTeams_v1(adminUserGuid).OrderBy(sortField);
            }
            else
            {
                return db.fnDesktopDataAdmin_SelectTeams_v1(adminUserGuid).OrderBy(sortField).Where(c => c.TeamName.Contains(filter));
            }
        }*/

		//Get TeamItemsByCityCode
		public List<Team> GetTeamItemsByCityCode(string cityCode)
		{
			return db.Teams.Where(c => c.CityCode == cityCode).ToList();
		}
    }
}
