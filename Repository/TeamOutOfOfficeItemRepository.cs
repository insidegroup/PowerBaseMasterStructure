using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Linq.Dynamic;
using System.Text.RegularExpressions;

namespace CWTDesktopDatabase.Repository
{
	public class TeamOutOfOfficeItemRepository
	{
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());
		
		//Get a Page of Team Out of Office Items - for Page Listings
		public CWTPaginatedList<spDesktopDataAdmin_SelectTeamOutOfOfficeItems_v1Result> PageTeamOutOfOfficeItems(int id, int page, string filter, string sortField, int sortOrder)
		{
			//get a page of records
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectTeamOutOfOfficeItems_v1(id, sortField, sortOrder, page, adminUserGuid).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}

			//put into page object
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectTeamOutOfOfficeItems_v1Result>(result, page, totalRecords);

			//return to user
			return paginatedView;
		}

		//Get one Team Out of Office Item
		public TeamOutOfOfficeItem GetItem(int id)
		{
			return db.TeamOutOfOfficeItems.SingleOrDefault(c => c.TeamOutOfOfficeItemId == id);
		}

		//Add Data From Linked Tables for Display
		public void EditItemForDisplay(TeamOutOfOfficeItem item)
		{
            TeamOutOfOfficeGroupRepository teamOutOfOfficeGroupRepository = new TeamOutOfOfficeGroupRepository();

			//PrimaryTeam (from CSU)
            item.HasPrimaryTeam = false;

            //Get TeamOutOfOfficeGroup
			if (item.TeamOutOfOfficeGroup == null)
			{
				item.TeamOutOfOfficeGroup = teamOutOfOfficeGroupRepository.GetGroup(item.TeamOutOfOfficeGroupId);
			}

            //Check TeamOutOfOfficeGroup Exists
			if (item.TeamOutOfOfficeGroup != null)
            {
                //Populate Hierarchy
                teamOutOfOfficeGroupRepository.EditGroupForDisplay(item.TeamOutOfOfficeGroup);

                //Get Primary Team
				Team primaryTeam = GetTeamOutOfOfficeItemPrimaryTeam(item.TeamOutOfOfficeGroup.HierarchyCode);
                if (primaryTeam != null && primaryTeam.TeamId > 0)
                {
                    item.HasPrimaryTeam = true;
                    item.PrimaryTeam = primaryTeam;
                }
            }

            //Team 1
            if (item.Team != null)
            {
                item.PrimaryBackupTeam = new TeamOutOfOfficeItemBackupTeam()
                {
                    TeamId = item.Team.TeamId,
                    TeamName = item.Team.TeamName
                };
            }

            //Team 2
            if (item.Team1 != null)
            {
                item.SecondaryBackupTeam = new TeamOutOfOfficeItemBackupTeam()
                {
                    TeamId = item.Team1.TeamId,
                    TeamName = item.Team1.TeamName
                };
            }

            //Team 3
            if (item.Team2 != null)
            {
                item.TertiaryBackupTeam = new TeamOutOfOfficeItemBackupTeam()
                {
                    TeamId = item.Team2.TeamId,
                    TeamName = item.Team2.TeamName
                };
            }

        }

		public Team GetTeamOutOfOfficeItemPrimaryTeam(string clientSubUnitGuid)
		{
			Team primaryTeam = new Team(); 

			//Get Primary Team
			ClientSubUnitTeamRepository clientSubUnitTeamRepository = new ClientSubUnitTeamRepository();
			Team clientSubUnitPrimaryTeam = clientSubUnitTeamRepository.GetClientSubUnitPrimaryTeam(clientSubUnitGuid);
			if (clientSubUnitPrimaryTeam != null && clientSubUnitPrimaryTeam.TeamId > 0)
			{
				primaryTeam = clientSubUnitPrimaryTeam;
			}

			return primaryTeam;
		}

		//Edit Item
		public void Edit(TeamOutOfOfficeItem item)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdateTeamOutOfOfficeItem_v1(
				item.TeamOutOfOfficeGroupId,
				item.PrimaryBackupTeam.TeamId,
				item.SecondaryBackupTeam.TeamId,
                item.TertiaryBackupTeam.TeamId,
				adminUserGuid,
                item.VersionNumber
			);
		}
	}
}
