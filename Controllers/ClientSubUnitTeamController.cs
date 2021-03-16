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
    public class ClientSubUnitTeamController : Controller
    {
        ClientSubUnitTeamRepository clientSubUnitTeamRepository = new ClientSubUnitTeamRepository();
        TeamRepository teamRepository = new TeamRepository();
        HierarchyRepository hierarchyRepository = new HierarchyRepository();

        // GET: /DeleteTeam
        public ActionResult CreateClientSubUnitForTeam(int id)
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
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Show Create Form
            ClientSubUnitTeam clientSubUnitTeam = new ClientSubUnitTeam();
            clientSubUnitTeam.TeamId = id;
			
			//Set Default Value
			clientSubUnitTeam.IncludeInClientDroplistFlag = true;

            clientSubUnitTeamRepository.EditForDisplay(clientSubUnitTeam);
            return View(clientSubUnitTeam);
        }

        // POST: /Create/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateClientSubUnitForTeam(int teamId, FormCollection collection)
        {

            ClientSubUnitTeam clientSubUnitTeam = new ClientSubUnitTeam();

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToTeam(teamId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

           //Update Model From Form + Validate against DB
	        try
            {
                UpdateModel(clientSubUnitTeam);
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
                clientSubUnitTeamRepository.Add(clientSubUnitTeam);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            ViewData["NewSortOrder"] = 0;
            return RedirectToRoute("Default", new { controller = "Team", action = "ListClientSubUnits", id = teamId });
        }


        // GET: /DeleteTeam
		[HttpGet]
		public ActionResult Delete(int id, string clientSubUnitGuid, string from)
        {
            //Show Create Form
            ClientSubUnitTeam clientSubUnitTeam = new ClientSubUnitTeam();
            clientSubUnitTeam = clientSubUnitTeamRepository.GetItem(id, clientSubUnitGuid);

            //Check Exists
            if (clientSubUnitTeam == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }
            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToTeam(id))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ViewData["From"] = from;
            clientSubUnitTeamRepository.EditForDisplay(clientSubUnitTeam);
            return View(clientSubUnitTeam);
        }

        // POST: /Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, string from, string clientSubUnitGuid, FormCollection collection)
        {
            //Show Create Form
            ClientSubUnitTeam clientSubUnitTeam = new ClientSubUnitTeam();
            clientSubUnitTeam = clientSubUnitTeamRepository.GetItem(id, clientSubUnitGuid);

            //Check Exists
            if (clientSubUnitTeam == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }
            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToTeam(id))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Delete Item
            try
            {
                clientSubUnitTeam.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                clientSubUnitTeamRepository.Delete(clientSubUnitTeam);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/SystemUserTeam.mvc/Delete/" + clientSubUnitTeam.TeamId.ToString() + "&clientSubUnitGuid=" + clientSubUnitTeam.ClientSubUnitGuid.ToString();
                    return View("VersionError");
                }
                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");            }
            if (from == "Team")
            {
                return RedirectToRoute("Default", new { controller = "Team", action = "ListClientSubUnits", id = id });
            }
            else
            {
                return RedirectToRoute("Main", new { controller = "ClientSubUnit", action = "ListTeams", id = clientSubUnitGuid });
            }
        }

        // GET: /DeleteTeam
        public ActionResult Edit(int id, string clientSubUnitGuid, string from)
        {
            //Show Create Form
            ClientSubUnitTeam clientSubUnitTeam = new ClientSubUnitTeam();
            clientSubUnitTeam = clientSubUnitTeamRepository.GetItem(id, clientSubUnitGuid);

            //Check Exists
            if (clientSubUnitTeam == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }
            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToTeam(id))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ViewData["From"] = from;
            clientSubUnitTeamRepository.EditForDisplay(clientSubUnitTeam);
            return View(clientSubUnitTeam);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, string from, string clientSubUnitGuid, FormCollection collection)
        {
            //Show Create Form
            ClientSubUnitTeam clientSubUnitTeam = new ClientSubUnitTeam();
            clientSubUnitTeam = clientSubUnitTeamRepository.GetItem(id, clientSubUnitGuid);

            //Check Exists
            if (clientSubUnitTeam == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }
            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToTeam(id))
            {
                ViewData["Message"] = "You do not have access to this Team";
                return View("Error");
            }

            //Update Item from Form
            try
            {
                UpdateModel(clientSubUnitTeam);
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
                clientSubUnitTeamRepository.Update(clientSubUnitTeam);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/ClientSubUnitTeam.mvc/Edit/"+ id+"?clientSubUnitGuid="+ clientSubUnitGuid+"&from=" + from;
                    return View("VersionError");
                }

                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");            }

            if (from == "Team")
            {
                return RedirectToRoute("Default", new { controller = "Team", action = "ListClientSubUnits", id = id });
            }
            else
            {
                return RedirectToRoute("Main", new { controller = "ClientSubUnit", action = "ListTeams", id = clientSubUnitGuid });
            }
        }

        //Check if this user can be added to team
        //must be valid guid and not already a member of team
        [HttpPost]
        public JsonResult IsValidClientSubUnitForTeam(string id, int teamid)
        {
            var result = clientSubUnitTeamRepository.IsValidClientSubUnitForTeam(id, teamid);
            return Json(result);
        }

    }
}

