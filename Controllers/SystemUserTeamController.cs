using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Repository;
using System.Data.SqlClient;


namespace CWTDesktopDatabase.Controllers
{
    public class SystemUserTeamController : Controller
    {
        SystemUserTeamRepository systemUserTeamRepository = new SystemUserTeamRepository();
        TeamRepository teamRepository = new TeamRepository();
        SystemUserRepository systemUserRepository = new SystemUserRepository();
        HierarchyRepository hierarchyRepository = new HierarchyRepository();

        // GET: /CreateUserForTeam
        public ActionResult CreateTeamForUser(string id)
        {
            SystemUser systemUser = new SystemUser();
            systemUser = systemUserRepository.GetUserBySystemUserGuid(id);

            //Check Exists
            if (systemUser == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            //Show Create Form
            SystemUserTeam systemUserTeam = new SystemUserTeam();
            systemUserTeam.SystemUserGuid = id;


            systemUserTeamRepository.EditForDisplay(systemUserTeam);
            return View(systemUserTeam);
        }

        // POST: /CreateUserForTeam
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTeamForUser(string systemUserGuid, int teamId)
        {

            SystemUserTeam systemUserTeam = new SystemUserTeam();

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToTeam(teamId))
            {
                return View("Error");
            }

           //Update Model From Form + Validate against DB
	        try
            {
                UpdateModel(systemUserTeam);
            }
	        catch
            {
                string n = "";
                foreach (ModelState modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        n += error.ErrorMessage;
                    }
                }
                ViewData["Message"] = "ValidationError : " + n;
                return View("Error");
            }

            //Database Update
            try
            {
                systemUserTeamRepository.Add(systemUserTeam);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            ViewData["NewSortOrder"] = 0;
            return RedirectToRoute("Main", new { controller = "SystemUser", action = "ListTeams", id = systemUserGuid });
        }

        // GET: /CreateUserForTeam
        public ActionResult CreateUserForTeam(int id)
        {
            Team group = new Team();
            group = teamRepository.GetTeam(id);

            //Check Exists
            if (group == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }
            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToTeam(id))
            {
                return View("Error");
            }

            //Show Create Form
            SystemUserTeam systemUserTeam = new SystemUserTeam();
            systemUserTeam.TeamId = id;


            systemUserTeamRepository.EditForDisplay(systemUserTeam);
            return View(systemUserTeam);
        }

        // POST: /CreateUserForTeam
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUserForTeam(string systemUserGuid, int teamId)
        {

            SystemUserTeam systemUserTeam = new SystemUserTeam();

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToTeam(teamId))
            {
                return View("Error");
            }

           //Update Model From Form + Validate against DB
	        try
            {
                UpdateModel(systemUserTeam);
            }
	        catch
            {
                string n = "";
                foreach (ModelState modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        n += error.ErrorMessage;
                    }
                }
                ViewData["Message"] = "ValidationError : " + n;
                return View("Error");
            }
            //Database Update
            try
            {
                systemUserTeamRepository.Add(systemUserTeam);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            ViewData["NewSortOrder"] = 0;
            return RedirectToRoute("Default", new { controller = "Team", action = "ListSystemUsers", id = teamId });
        }


        // GET: /DeleteTeam
		[HttpGet]
		public ActionResult Delete(int id, string systemUserGuid, string from)
        {
            //Show Create Form
            SystemUserTeam systemUserTeam = new SystemUserTeam();
            systemUserTeam = systemUserTeamRepository.GetItem(id, systemUserGuid);

            //Check Exists
            if (systemUserTeam == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }
            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToTeam(id))
            {
                return View("Error");
            }

            ViewData["From"] = from;
            systemUserTeamRepository.EditForDisplay(systemUserTeam);
            return View(systemUserTeam);
        }

        // POST: /Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, string from, string systemUserGuid, FormCollection collection)
        {
            SystemUserTeam systemUserTeam = new SystemUserTeam();
            systemUserTeam = systemUserTeamRepository.GetItem(id, systemUserGuid);

            //Check Exists
            if (systemUserTeam == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }
            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToTeam(id))
            {
                return View("Error");
            }

            //Delete Item
            try
            {
                systemUserTeam.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                systemUserTeamRepository.Delete(systemUserTeam);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/SystemUserTeam.mvc/Delete/" + systemUserTeam.TeamId.ToString() + "&systemUserGuid=" + systemUserTeam.SystemUserGuid.ToString();
                    return View("VersionError");
                }
                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");            }
            if (from == "Team")
            {
                return RedirectToRoute("Default", new { controller = "Team", action = "ListSystemUsers", id = id });
            }
            else
            {
                return RedirectToRoute("Main", new { controller = "SystemUser", action = "ListTeams", id = systemUserGuid });
            }
        }

       
        //AutoComplete SystemUsers (who are not already in team)
        [HttpPost]
        public JsonResult AutoCompleteNonTeamSystemUsers(int id, string searchText)
        {
            var result = systemUserTeamRepository.LookUpNonTeamSystemUsers(id, searchText);
            return Json(result);
        }

        //AutoComplete Teams (who aSystemUsers is not already a member of)
        [HttpPost]
        public JsonResult AutoCompleteNonSystemUserTeams(string id, string searchText)
        {
            var result = systemUserTeamRepository.LookUpNonSystemUserTeams(id, searchText);
            return Json(result);
        }

        //Check if this user can be added to team
        //must be valid guid and not already a member of team
        [HttpPost]
        public JsonResult IsValidSystemUserForTeam(string id, int teamid)
        {
            var result = systemUserTeamRepository.IsValidSystemUserForTeam(id, teamid);
            return Json(result);
        }
        //Check if this user can be added to team
        //must be valid guid and not already a member of team
        [HttpPost]
        public JsonResult IsValidTeamForSystemUser(string id, string teamName)
        {
            var result = systemUserTeamRepository.IsValidTeamForSystemUser(id, teamName);
            return Json(result);
        }

        

    }
}
