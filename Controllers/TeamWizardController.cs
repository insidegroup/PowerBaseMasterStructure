using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Repository;
using CWTDesktopDatabase.ViewModels;
using System.Data.SqlClient;
using System.Web.Util;

namespace CWTDesktopDatabase.Controllers
{
    [AjaxTimeOutCheck]
    public class TeamWizardController : Controller
    {
        //main repositories
        TeamRepository teamRepository = new TeamRepository();
        TeamWizardRepository teamWizardRepository = new TeamWizardRepository();
        HierarchyRepository hierarchyRepository = new HierarchyRepository();

        private string groupName = "Role Based Access Team";

        //WRAPPER PAGE FOR WIZARD- Returns View
        public ActionResult Index()
        {
            return View();
        }

        //WIZARD STEP 1: Show a list of Teams  - Returns PartialView
		[HttpPost]
		public ActionResult TeamSelectionScreen(string filter)
        {

            //Check Access Rights
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                return PartialView("Error", "You Do Not Have Access to Teams. Please Contact an Administrator");
            }

            filter = System.Web.HttpUtility.UrlDecode(filter);
            var teams = teamWizardRepository.GetTeams(filter ?? "");
            return PartialView("TeamSelectionScreen", teams);
        }

        //WIZARD STEP 2A: Show a Team for Adding/Editing/Deleting - Returns PartialView
        [HttpPost]
        public ActionResult TeamDetailsScreen(int teamId = 0)
        {
            try
            {
                Team team = new Team();

                if (teamId > 0)
                {
                    team = teamRepository.GetTeam(teamId);

                    //Check Exists
                    if (team == null)
                    {
                        return PartialView("Error", "Team Does Not Exist");
                    }

                    //Check AccessRights
                    RolesRepository rolesRepository = new RolesRepository();
                    if (!rolesRepository.HasWriteAccessToTeam(teamId))
                    {
                        return PartialView("Error", "You Do Not Have Access to This Team");
                    }

                    //Add linked information
                    teamRepository.EditGroupForDisplay(team);
                }

                TeamTypeRepository teamTypeRepository = new TeamTypeRepository();
                SelectList teamTypesList = new SelectList(teamTypeRepository.GetAllTeamTypes().ToList(), "TeamTypeCode", "TeamTypeDescription");
                ViewData["TeamTypes"] = teamTypesList;

                TablesDomainHierarchyLevelRepository tablesDomainHierarchyLevelRepository = new TablesDomainHierarchyLevelRepository();
                SelectList hierarchyTypesList = new SelectList(tablesDomainHierarchyLevelRepository.GetDomainHierarchies(groupName).ToList(), "HierarchyLevelTableName", "HierarchyLevelTableName");
                ViewData["HierarchyTypes"] = hierarchyTypesList;

                team.SystemUserCount = teamWizardRepository.GetTeamSystemUsers(teamId).Count;
                team.ClientSubUnitCount = teamWizardRepository.GetTeamClientSubUnits(teamId).Count;

                return PartialView("TeamDetailsScreen", team);
            }
            catch (Exception ex)
            {
                return PartialView("Error", ex.Message);
            }
        }

        //WIZARD STEP 2B: Validate Team on Submit (no save)- Returns JSON
        [HttpPost]
        public ActionResult ValidateTeam(Team team)
        {
            try
            {
                //Check Access Rights
                if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
                {
                    return PartialView("Error", "You Do Not Have Access to Teams. Please Contact an Administrator");
                }

                //Validate Team data against Table
                if (!ModelState.IsValid)
                {

                    TeamTypeRepository teamTypeRepository = new TeamTypeRepository();
                    SelectList teamTypesList = new SelectList(teamTypeRepository.GetAllTeamTypes().ToList(), "TeamTypeCode", "TeamTypeDescription");
                    ViewData["TeamTypes"] = teamTypesList;

                    TablesDomainHierarchyLevelRepository tablesDomainHierarchyLevelRepository = new TablesDomainHierarchyLevelRepository();
                    SelectList hierarchyTypesList = new SelectList(tablesDomainHierarchyLevelRepository.GetDomainHierarchies(groupName).ToList(), "HierarchyLevelTableName", "HierarchyLevelTableName");
                    ViewData["HierarchyTypes"] = hierarchyTypesList;

                    return Json(new WizardJSONResponse
                    {
                        html = ControllerExtension.RenderPartialViewToString(this, "TeamDetailsScreen", team),
                        message = "ValidationError",
                        success = false
                    });
                }

                TeamSystemUsersVM teamUsersScreen = new TeamSystemUsersVM();
                teamUsersScreen.Team = team;
                if (team.TeamId > 0)
                {
                    teamUsersScreen.SystemUsers = teamWizardRepository.GetTeamSystemUsers(team.TeamId);
                }
                else
                {
                    // for Team with no Systemusers
                    List<spDDAWizard_SelectTeamSystemUsers_v1Result> systemUsers = new List<spDDAWizard_SelectTeamSystemUsers_v1Result>();
                    teamUsersScreen.SystemUsers = systemUsers;
                }
                return Json(new WizardJSONResponse
                {
                    html = ControllerExtension.RenderPartialViewToString(this, "TeamUsersScreen", teamUsersScreen),
                    message = "Success",
                    success = true
                });
            }
            catch (Exception ex)
            {
                 return Json(new WizardJSONResponse
                 {
                     html = ControllerExtension.RenderPartialViewToString(this, "Error", ex.Message),
                     message = "UnhandledError",
                     success = false
                 });
            }
        }

        //Show Popup When a User tries To Delete (PART OF STEP 2)- Returns PartialView
        public ActionResult ShowConfirmDelete(int teamId = 0)
        {
            Team team = new Team();
            team = teamRepository.GetTeam(teamId);

            //Check Exists
            if (team == null)
            {
                return PartialView("Error", "Team Does Not Exist");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToTeam(teamId))
            {
                return PartialView("Error", "You Do Not Have Access to This Team");
            }

            TeamClientSubUnitsAndSystemUsersVM teamUsersAndClientSubUnits = new TeamClientSubUnitsAndSystemUsersVM();
            teamUsersAndClientSubUnits.ClientSubUnits = teamWizardRepository.GetTeamClientSubUnits(teamId);
            teamUsersAndClientSubUnits.SystemUsers = teamWizardRepository.GetTeamSystemUsers(teamId);
            teamUsersAndClientSubUnits.Team = team;

            return PartialView("ConfirmDeletePopup", teamUsersAndClientSubUnits);


        }

        //Show 2nd Popup When a User tries To Delete (PART OF STEP 2)- Returns PartialView
        public ActionResult ShowConfirmDelete2(int teamId)
        {

            Team team = new Team();
            team = teamRepository.GetTeam(teamId);

            //Check Exists
            if (team == null)
            {
                return PartialView("Error", "Team Does Not Exist");
            }
            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToTeam(teamId))
            {
                return PartialView("Error", "You Do Not Have Access to This Team");
            }

            TeamLinkedItemsVM teamLinkedItemsScreenViewModel = new TeamLinkedItemsVM();
            teamLinkedItemsScreenViewModel.Team = team;
            teamWizardRepository.AddLinkedItems(teamLinkedItemsScreenViewModel);


            return PartialView("ConfirmDeletePopup2", teamLinkedItemsScreenViewModel);


        }
        //Delete Team (PART OF STEP 2)- Returns JSON
        [HttpPost]
        public ActionResult DeleteTeam(Team team)
        {
 
            //Check Exists
            if (team == null)
            {
                return Json(new WizardJSONResponse
                {
                    html = ControllerExtension.RenderPartialViewToString(this, "Error", "Team does not Exist"),
                    message = "NoRecordError",
                    success = false
                });
            }         
            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToTeam(team.TeamId))
            {
                return Json(new WizardJSONResponse
                {
                    html = ControllerExtension.RenderPartialViewToString(this, "Error", "You do not have access to this Team"),
                    message = "NoAccessError",
                    success = false
                });
            }
            //Delete Item
            try
            {
                teamRepository.Delete(team);
            }
            catch (SqlException ex)
            {
                if (ex.Message == "SQLVersioningError")
                {
                    return Json(new WizardJSONResponse
                    {
                        html = ControllerExtension.RenderPartialViewToString(this, "VersionError", null),
                        message = "VersionError",
                        success = false
                    });
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                return Json(new WizardJSONResponse
                {
                    html = ControllerExtension.RenderPartialViewToString(this, "Error", ex.Message),
                    message = "DBError",
                    success = false
                });
            }

            WizardMessages wizardMessages = new WizardMessages();
            wizardMessages.AddMessage("Team has been successfully deleted.", true);

            //Item Deleted - Return response
            return Json(new WizardJSONResponse
            {
                html = ControllerExtension.RenderPartialViewToString(this, "FinishedScreen", wizardMessages),
                message = "Success",
                success = true
            });
        }

        //Search for Users (PART OF STEP 3)- Returns PartialView
        public ActionResult SystemUserSearch(string filterField, string filter, int teamId)
        {
            filter = System.Web.HttpUtility.UrlDecode(filter);

            List<spDDAWizard_SelectTeamAvailableSystemUsersFiltered_v1Result> searchResults = new List<spDDAWizard_SelectTeamAvailableSystemUsersFiltered_v1Result>();
            searchResults = teamWizardRepository.GetTeamAvailableSystemUsers(filter, filterField, teamId);
            return PartialView("SystemUserSearchResults", searchResults);
        }

        //WIZARD STEP 4: Allow User to Add/Remove ClietnSubUnits  - Returns PartialView
        [HttpPost]
        public ActionResult TeamClientSubUnitsScreen(int teamId)
        {
            //filter = System.Web.HttpUtility.UrlDecode(filter);
            Team team = new Team();

            //Editing A Team
            if (teamId > 0)
            {
                team = teamRepository.GetTeam(teamId);

                //Check Exists
                if (team == null)
                {
                    return PartialView("Error", "Team Does Not Exist");
                }
                //Check AccessRights
                RolesRepository rolesRepository = new RolesRepository();
                if (!rolesRepository.HasWriteAccessToTeam(teamId))
                {
                    return PartialView("Error", "You Do Not Have Access to This Team");
                }
            }
            else //Adding a Team
            {
                //Check Access Rights
                if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
                {
                    return PartialView("Error", "You Do Not Have Access to Teams. Please Contact an Administrator");
                }
            }


            //model
            TeamClientSubUnitsVM teamClientsScreen = new TeamClientSubUnitsVM();
            teamClientsScreen.Team = team;
            if (teamId > 0)
            {
                teamClientsScreen.ClientSubUnits = teamWizardRepository.GetTeamClientSubUnits(teamId);
            }
            else
            {
                // for Added team with no Systemusers
                List<spDDAWizard_SelectTeamClientSubUnits_v1Result> clientSubUnits = new List<spDDAWizard_SelectTeamClientSubUnits_v1Result>();
                teamClientsScreen.ClientSubUnits = clientSubUnits;
            }
            

            return PartialView("TeamClientSubUnitsScreen", teamClientsScreen);
        }

        //Search for ClientSubUnits (PART OF STEP 4) - Returns PartialView
        public ActionResult ClientSubUnitSearch(string filterField, string filter)
        {
            filter = System.Web.HttpUtility.UrlDecode(filter);

            List<spDDAWizard_SelectClientSubUnitsFiltered_v1Result> searchResults = new List<spDDAWizard_SelectClientSubUnitsFiltered_v1Result>();
            searchResults = teamWizardRepository.GetTeamAvailableClientSubUnits(filter, filterField);
            return PartialView("ClientSubUnitSearchResults", searchResults);
        }

        //WIZARD STEP 5A: Show A list of Changes made by User - Returns JSON
        [HttpPost]
        public ActionResult ConfirmChangesScreen(TeamWizardVM updatedTeam)
        {
       
            WizardMessages wizardMessages = new WizardMessages();

            if (updatedTeam.Team.TeamId > 0)
            {
                Team originalTeam = new Team();
                originalTeam = teamRepository.GetTeam(updatedTeam.Team.TeamId);
                teamRepository.EditGroupForDisplay(originalTeam);

                teamWizardRepository.BuildTeamChangeMessages(wizardMessages, originalTeam, updatedTeam);
            }
            else
            {
                teamWizardRepository.BuildTeamChangeMessages(wizardMessages, null, updatedTeam);
            }

            return Json(new WizardJSONResponse
            {
                html = ControllerExtension.RenderPartialViewToString(this, "ConfirmChangesScreen", wizardMessages),
                message = "Success",
                success = true
            });
        }

        //WIZARD STEP 5B: On COMPLETION - Commit Team Update/Creation to the Database - Returns JSON
        [HttpPost]
        public ActionResult CommitChanges(TeamWizardVM teamChanges)
        {
            Team team = new Team();
            team = teamChanges.Team;

            WizardMessages wizardMessages = new WizardMessages();

            try{
                UpdateModel(team);
            }
            catch
            {   
                //Validation Error
                string msg = "";
                foreach (ModelState modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        msg += error.ErrorMessage;
                    }
                }
                return Json(new WizardJSONResponse
                {
                    html = ControllerExtension.RenderPartialViewToString(this, "Error", msg),
                    message = msg,
                    success = false
                });
            }
            //Editing A Team
            if (team.TeamId > 0)
            {
                try
                {
                    teamWizardRepository.UpdateTeam(team);
                    wizardMessages.AddMessage("Team Details successfully updated", true);
                }
                catch (SqlException ex)
                {
                    //If there is error we will continue, but store error to return to user

                    //Versioning Error
                    if (ex.Message == "SQLVersioningError")
                    {
                        wizardMessages.AddMessage("Team Detail was not updated. Another user has already changed this Team.", false);
                    }
                    else //Other Error
                    {
                        LogRepository logRepository = new LogRepository();
                        logRepository.LogError(ex.Message);

                        wizardMessages.AddMessage("Team Details were not updated, please check Event Log for details", false);
                        wizardMessages.AddMessage("There was a problem with your request, please see the log file or contact an administrator for details", false);
                    }
                }
            }
            else //Adding A Team
            {

                try
                {
                    int teamId = teamWizardRepository.AddTeam(team);
                    team = teamRepository.GetTeam(teamId);
                    teamChanges.Team = team;
                    wizardMessages.AddMessage("Team added successfully", true);
                }
                catch (SqlException ex)
                {
                    LogRepository logRepository = new LogRepository();
                    logRepository.LogError(ex.Message);

                    wizardMessages.AddMessage("Team was not added, please check Event Log for details", false);
                    wizardMessages.AddMessage("There was a problem with your request, please see the log file or contact an administrator for details", false);

                    //If we cannot add a Team , we cannot continue, so return error to User
                    return Json(new
                    {
                        html = ControllerExtension.RenderPartialViewToString(this, "FinishedScreen", wizardMessages),
                        message = "DBError",
                        success = false
                    });
                }
            }
            //If we have added a Team successfully, or edited a Team (successfully or unsuccessfully), we continue to add SystemUsers/ClientSubUnits
            try
            {
                wizardMessages = teamWizardRepository.UpdateTeamSystemUsers(teamChanges, wizardMessages);

            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                wizardMessages.AddMessage("Team SystemUser were not changed, please check Event Log for details", false);
                wizardMessages.AddMessage("There was a problem with your request, please see the log file or contact an administrator for details", false);
            }
            try
            {
                wizardMessages = teamWizardRepository.UpdateTeamClientSubUnits(teamChanges, wizardMessages);

            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                wizardMessages.AddMessage("Team ClientSubUnits were not changed, please check Event Log for details", false);
                wizardMessages.AddMessage("There was a problem with your request, please see the log file or contact an administrator for details", false);
            }
            return Json(new
            {
                html = ControllerExtension.RenderPartialViewToString(this, "FinishedScreen", wizardMessages),
                message = "Success",
                success = true
            });        
        }
       
    }
}
