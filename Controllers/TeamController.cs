using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Repository;
using System.Data.SqlClient;
using System.Xml;

namespace CWTDesktopDatabase.Controllers
{
     [AjaxTimeOutCheck]
    public class TeamController : Controller
    {
        //main repository
        TeamRepository teamRepository = new TeamRepository();
        HierarchyRepository hierarchyRepository = new HierarchyRepository();
        private string groupName = "Role Based Access Team";

        //sortable List
        public ActionResult List(string filter, int? page, string sortField, int? sortOrder)
        {
            //Set Access Rights
            ViewData["Access"] = "";
            if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Access"] = "WriteAccess";
            }   

            if (sortField != "HierarchyType")
            {
                sortField = "TeamName";
            }

            if (sortOrder == 1)
            {
                ViewData["NewSortOrder"] = 0;
                ViewData["CurrentSortOrder"] = 1;
            }
            else
            {
                ViewData["NewSortOrder"] = 1;
                ViewData["CurrentSortOrder"] = 0;
            }

            //return items
            var cwtPaginatedList = teamRepository.PageTeams(page ?? 1, filter ?? "", sortField, sortOrder ?? 0);
            return View(cwtPaginatedList);  

        }

        //sortable List of Team's members
        public ActionResult ListSystemUsers(int id, string filter, int? page, string sortField, int? sortOrder)
        {

            //Get Team
            Team team = new Team();
            team = teamRepository.GetTeam(id);

            //Check Exists
            if (team == null)
            {
                return View("Error");
            }

            //Set Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToTeam(id))
            {
                ViewData["Access"] = "WriteAccess";
            }

            if (sortField == null || sortField == "Name")
            {
                sortField = "LastName";
            }

            ViewData["DeletedFlag"] = false;


            if (sortOrder == 1)
            {
                ViewData["NewSortOrder"] = 0;
                ViewData["CurrentSortOrder"] = 1;
            }
            else
            {
                ViewData["NewSortOrder"] = 1;
                ViewData["CurrentSortOrder"] = 0;
            }

            ViewData["TeamId"] = team.TeamId;
            ViewData["TeamName"] = team.TeamName;

			var cwtPaginatedList = teamRepository.PageTeamSystemUsers(id, filter ?? "", page ?? 1, sortField, sortOrder ?? 0);
			return View(cwtPaginatedList);
        }

        //sortable List of Team's ClientSubUnits
        public ActionResult ListClientSubUnits(int id, int? page, string sortField, int? sortOrder)
        {

            //Get Team
            Team team = new Team();
            team = teamRepository.GetTeam(id);

            //Check Exists
            if (team == null)
            {
                return View("Error");
            }

            //Set Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToTeam(id))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //always the same
            sortField = "ClientSubUnitName";


            if (sortOrder == 1)
            {
                ViewData["NewSortOrder"] = 0;
                ViewData["CurrentSortOrder"] = 1;
            }
            else
            {
                ViewData["NewSortOrder"] = 1;
                ViewData["CurrentSortOrder"] = 0;
            }

            ViewData["TeamId"] = team.TeamId;
            ViewData["TeamName"] = team.TeamName;

            //return items
            var cwtPaginatedList = teamRepository.PageTeamClientSubUnits(id, page ?? 1, sortField, sortOrder ?? 0);
            return View(cwtPaginatedList);
        }


        // GET: /Create (add a ClientSubUnit to a Team)
        public ActionResult CreateClientSubUnit(int id)
        {
            //Check Exists
            Team team = new Team();
            team = teamRepository.GetTeam(id);
            if (team == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }
            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToTeam(id))
            {
                return View("Error");
            }

            ClientSubUnitTeam clientSubUnitTeam = new ClientSubUnitTeam();
            clientSubUnitTeam.TeamId = team.TeamId;
            clientSubUnitTeam.TeamName = team.TeamName;

            return View(clientSubUnitTeam);
        }

        // GET: /Create
        public ActionResult Create()
        {
            //Check Access Rights to Domain
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            TeamTypeRepository teamTypeRepository = new TeamTypeRepository();
            SelectList teamTypesList = new SelectList(teamTypeRepository.GetAllTeamTypes().ToList(), "TeamTypeCode", "TeamTypeDescription");
            ViewData["TeamTypes"] = teamTypesList;

            TablesDomainHierarchyLevelRepository tablesDomainHierarchyLevelRepository = new TablesDomainHierarchyLevelRepository();
            SelectList hierarchyTypesList = new SelectList(tablesDomainHierarchyLevelRepository.GetDomainHierarchies(groupName).ToList(), "HierarchyLevelTableName", "HierarchyLevelTableName");
            ViewData["HierarchyTypes"] = hierarchyTypesList;

            Team team = new Team();
            return View(team);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Team group)
        {
            //Check Access Rights to Domain
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
            //Check Access Rights to Domain Hierarchy
            if (!hierarchyRepository.AdminHasDomainHierarchyWriteAccess(group.HierarchyType, group.HierarchyCode, group.SourceSystemCode, groupName))
            {
                ViewData["Message"] = "You cannot add to this hierarchy item";
                return View("Error");
            }

           //Update Model From Form + Validate against DB
	        try
            {
                UpdateModel(group);
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
            

            //ClientSubUnitTravelerType has extra field
            string hierarchyCode = group.HierarchyCode;
            if (group.HierarchyType == "ClientSubUnitTravelerType")
            {
                group.ClientSubUnitGuid = hierarchyCode;  //ClientSubUnitTravelerType has 2 primarykeys
            }

            //Database Update
            try
            {
                teamRepository.Add(group);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }            

            ViewData["NewSortOrder"] = 0;
            return RedirectToAction("List");
        }

        // GET: /View
        public ActionResult View(int id)
        {
            Team group = new Team();
            group = teamRepository.GetTeam(id);

            //Check Exists
            if (group == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            teamRepository.EditGroupForDisplay(group);
            return View(group);
        }

        // GET: /Edit
        public ActionResult Edit(int id)
        {

            Team group = new Team();
            group = teamRepository.GetTeam(id);

            //Check Exists
            if (group == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            
            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToTeam(id))
            {
                ViewData["Message"] = "You cannot add to this hierarchy item";
                return View("Error");
            }

            TeamTypeRepository teamTypeRepository = new TeamTypeRepository();
            SelectList teamTypesList = new SelectList(teamTypeRepository.GetAllTeamTypes().ToList(), "TeamTypeCode", "TeamTypeDescription");
            ViewData["TeamTypes"] = teamTypesList;

            TablesDomainHierarchyLevelRepository tablesDomainHierarchyLevelRepository = new TablesDomainHierarchyLevelRepository();
            SelectList hierarchyTypesList = new SelectList(tablesDomainHierarchyLevelRepository.GetDomainHierarchies(groupName).ToList(), "HierarchyLevelTableName", "HierarchyLevelTableName");
            ViewData["HierarchyTypes"] = hierarchyTypesList;


            teamRepository.EditGroupForDisplay(group);
            return View(group);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection collection)
        {
            //Update Model from Form
            Team group = teamRepository.GetTeam(id);
            if (group == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }
            try
            {
                UpdateModel(group);
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

            //ClientSubUnitTravelerType has extra field
            string hierarchyCode = group.HierarchyCode;
            if (group.HierarchyType == "ClientSubUnitTravelerType")
            {
                group.ClientSubUnitGuid = hierarchyCode;  //ClientSubUnitTravelerType has 2 primarykeys
            }
            //Check Access Rights to PolicyGroup
            HierarchyRepository hierarchyRepository = new HierarchyRepository();
            if (!hierarchyRepository.AdminHasDomainHierarchyWriteAccess(group.HierarchyType, hierarchyCode, group.SourceSystemCode, groupName))
            {
                ViewData["Message"] = "You cannot add to this hierarchy item";
                return View("KnownError");
            }


            //Database Update
            try
            {
                teamRepository.Update(group);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/Team.mvc/Edit/" + group.TeamId.ToString();
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List");

        }

        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id)
        {
            Team group = new Team();
            group = teamRepository.GetTeam(id);

            //Check Exists
            if (group == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }
            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToTeam(id))
            {
                ViewData["Message"] = "You cannot add to this hierarchy item";
                return View("Error");
            }

            teamRepository.EditGroupForDisplay(group);

            TeamWizardRepository teamWizardRepository = new TeamWizardRepository();
            group.SystemUsers = teamWizardRepository.GetTeamSystemUsers(group.TeamId);
            group.ClientSubUnits = teamWizardRepository.GetTeamClientSubUnits(group.TeamId);

            return View(group);
        }

        // GET: /ConfirmDelete
        public ActionResult ConfirmDelete(int id)
        {
            Team group = new Team();
            group = teamRepository.GetTeam(id);

            //Check Exists
            if (group == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }
            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToTeam(id))
            {
                ViewData["Message"] = "You cannot add to this hierarchy item";
                return View("Error");
            }

            teamRepository.EditGroupForDisplay(group);

            TeamWizardRepository teamWizardRepository = new TeamWizardRepository();
            TeamLinkedItemsVM teamLinkedItemsViewModel = new TeamLinkedItemsVM();
            teamLinkedItemsViewModel.Team = group;

            teamWizardRepository.AddLinkedItems(teamLinkedItemsViewModel);
            return View(teamLinkedItemsViewModel);
        }

        // POST: /Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmDelete(int id, FormCollection collection)
        {
            
            Team group = new Team();
            group = teamRepository.GetTeam(id);

            //Check Exists
            if (group == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }
            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToTeam(id))
            {
                ViewData["Message"] = "You cannot add to this hierarchy item";
                return View("Error");
            }

            //Delete Item
            try
            {
                group.VersionNumber = Int32.Parse(collection["Team.VersionNumber"]);
                teamRepository.Delete(group);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/Team.mvc/Delete/" + group.TeamId.ToString();
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }           
            return RedirectToAction("List");
        }

        // POST: AutoComplete Hierarchies
        [HttpPost]
        public JsonResult AutoCompleteHierarchies(string searchText, string hierarchyItem)
        {
            HierarchyRepository hierarchyRepository = new HierarchyRepository();

            int maxResults = 15;
            var result = hierarchyRepository.LookUpSystemUserHierarchies(searchText, hierarchyItem, maxResults, groupName, true);
            return Json(result);
        }

        [HttpPost]
        public JsonResult AutoCompleteClientSubUnitTravelerTypes(string searchText, string hierarchyItem, string filterText)
        {
            HierarchyRepository hierarchyRepository = new HierarchyRepository();

            int maxResults = 15;
            var result = hierarchyRepository.LookUpSystemUserClientSubUnitTravelerTypes(searchText, hierarchyItem, maxResults, groupName, filterText);
            return Json(result);
        }

        //AutoComplete CLientSubUnits on TeamClientSubUnit pages
        [HttpPost]
        public JsonResult AutoCompleteClientSubUnits(string searchText)
        {
            HierarchyRepository hierarchyRepository = new HierarchyRepository();

            string roleName = "Client Detail";
            int maxResults = 15;
            var result = hierarchyRepository.LookUpSystemUserClientSubUnits(searchText, maxResults, roleName);
            return Json(result);
        }

        //GET -Show form allowing you to select a Team to copy users to
        public ActionResult MoveCopyUsersStep1(int id)
        {

            //Get PolicyAirVendorGroupItem
            Team group = new Team();
            group = teamRepository.GetTeam(id);

            //Check Exists
            if (group == null)
            {
                ViewData["ActionMethod"] = "CopyUsersStep1Get";
                return View("RecordDoesNotExistError");
            }
            //Check AccessRights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToTeam(id))
            {
                return View("Error");
            }
            ViewData["Access"] = "WriteAccess";

            SelectList teamList = new SelectList(teamRepository.GetMoveToTeams(group.TeamId).ToList(), "TeamId", "TeamName");
            ViewData["TeamList"] = teamList;

            return View(group);
        }

        // POST: /CopyUsersStep1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MoveCopyUsersStep1(int teamId, int newTeamId)
        {

            //Get Teams
            Team team = new Team();
            team = teamRepository.GetTeam(teamId);
            Team newTeam = new Team();
            newTeam = teamRepository.GetTeam(newTeamId);

            //Check Exists
            if (team == null || newTeam == null)
            {
                ViewData["ActionMethod"] = "CopySomeUsersStep2Post";
                return View("RecordDoesNotExistError");
            }
            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToTeam(teamId) || !rolesRepository.HasWriteAccessToTeam(newTeamId))
            {
                return View("Error");
            }

            return RedirectToAction("MoveCopyUsersStep2", new { teamId = teamId, newTeamId = newTeamId });

        }

         //GET -Show form allowing you to choose between all/some
        public ActionResult MoveCopyUsersStep2(int teamId, int newTeamId)
        {
            //Get Teams
            Team fromTeam = new Team();
            fromTeam = teamRepository.GetTeam(teamId);
            Team toTeam = new Team();
            toTeam = teamRepository.GetTeam(newTeamId);

            //Check Exists
            if (fromTeam == null || toTeam == null)
            {
                ViewData["ActionMethod"] = "CopyUsersStep2Get";
                return View("RecordDoesNotExistError");
            }
            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToTeam(teamId) || !rolesRepository.HasWriteAccessToTeam(newTeamId))
            {
                return View("Error");
            }

            ViewData["TeamName"] = fromTeam.TeamName;
            ViewData["TeamId"] = fromTeam.TeamId;
            ViewData["NewTeamName"] = toTeam.TeamName;
            ViewData["NewTeamId"] = toTeam.TeamId;
            ViewData["Access"] = "WriteAccess";

            return View();
        }

        // POST: /CopyUsersStep2
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MoveCopyUsersStep2(int teamId, int newTeamId, string membersToCopy)
        {

            //Get Teams
            Team fromTeam = new Team();
            fromTeam = teamRepository.GetTeam(teamId);
            Team toTeam = new Team();
            toTeam = teamRepository.GetTeam(newTeamId);

            //Check Exists
            if (fromTeam == null || toTeam == null)
            {
                ViewData["ActionMethod"] = "CopyUsersStep2Post";
                return View("RecordDoesNotExistError");
            }


            if (membersToCopy == "CopyAll")
            {
                return RedirectToAction("CopyAllUsersStep3", new { teamId = teamId, newTeamId = newTeamId });
            }
            if (membersToCopy == "CopySome")
            {
                return RedirectToAction("CopySomeUsersStep3", new { teamId = teamId, newTeamId = newTeamId });
            }
            if (membersToCopy == "MoveAll")
            {
                return RedirectToAction("MoveAllUsersStep3", new { teamId = teamId, newTeamId = newTeamId });
            }
            if (membersToCopy == "MoveSome")
            {
                return RedirectToAction("MoveSomeUsersStep3", new { teamId = teamId, newTeamId = newTeamId });
            }
            return View("Error");
        }

        #region Copy All Users Step 3
        //GET - confirm button page
        public ActionResult CopyAllUsersStep3(int teamId, int newTeamId)
        {
            //Get Teams
            Team fromTeam = new Team();
            fromTeam = teamRepository.GetTeam(teamId);
            Team toTeam = new Team();
            toTeam = teamRepository.GetTeam(newTeamId);

            //Check Exists
            if (fromTeam == null || toTeam == null)
            {
                ViewData["ActionMethod"] = "CopyAllUsersStep3Get";
                return View("RecordDoesNotExistError");
            }
            //Check AccessRights
             RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToTeam(teamId) || !rolesRepository.HasWriteAccessToTeam(newTeamId))
            {
                return View("Error");
            }

            ViewData["TeamName"] = fromTeam.TeamName;
            ViewData["TeamId"] = fromTeam.TeamId;
            ViewData["NewTeamName"] = toTeam.TeamName;
            ViewData["NewTeamId"] = toTeam.TeamId;
            ViewData["Access"] = "WriteAccess";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CopyAllUsersStep3(int teamId, int newTeamId, FormCollection collection)
        {
            //Get Teams
            Team fromTeam = new Team();
            fromTeam = teamRepository.GetTeam(teamId);
            Team toTeam = new Team();
            toTeam = teamRepository.GetTeam(newTeamId);

            //Check Exists
            if (fromTeam == null || toTeam == null)
            {
                ViewData["ActionMethod"] = "CopyAllUsersStep3Post";
                return View("RecordDoesNotExistError");
            }
            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToTeam(teamId) || !rolesRepository.HasWriteAccessToTeam(newTeamId))
            {
                return View("Error");
            }
            try
            {
                teamRepository.CopyAllTeamUsers(teamId, newTeamId);
            }
            catch
            {
                return View("Error");
            }


            string msg = "You have successfully copied all users from " + fromTeam.TeamName + " to " + toTeam.TeamName;
            return RedirectToAction("MoveCopyUsersCompleted", new { teamId = teamId, newTeamId = newTeamId, msg = msg });
        }
        #endregion

        #region Copy Some Users Step 3
        //GET - confirm button page
        public ActionResult CopySomeUsersStep3(int teamId, int newTeamId)
        {
            //Get Teams
            Team fromTeam = new Team();
            fromTeam = teamRepository.GetTeam(teamId);
            Team toTeam = new Team();
            toTeam = teamRepository.GetTeam(newTeamId);

            //Check Exists
            if (fromTeam == null || toTeam == null)
            {
                ViewData["ActionMethod"] = "CopySomeUsersStep3Get";
                return View("RecordDoesNotExistError");
            }
            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToTeam(teamId) || !rolesRepository.HasWriteAccessToTeam(newTeamId))
            {
                return View("Error");
            }
            var result = teamRepository.GetAllSystemUsers(fromTeam.TeamId).ToList();

            ViewData["TeamName"] = fromTeam.TeamName;
            ViewData["TeamId"] = fromTeam.TeamId;
            ViewData["NewTeamName"] = toTeam.TeamName;
            ViewData["NewTeamId"] = toTeam.TeamId;
            ViewData["Access"] = "WriteAccess";

            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CopySomeUsersStep3(int teamId, int newTeamId, FormCollection collection)
        {
            //Get Teams
            Team fromTeam = new Team();
            fromTeam = teamRepository.GetTeam(teamId);
            Team toTeam = new Team();
            toTeam = teamRepository.GetTeam(newTeamId);

            //Check Exists
            if (fromTeam == null || toTeam == null)
            {
                ViewData["ActionMethod"] = "CopySomeUsersStep3Post";
                return View("RecordDoesNotExistError");
            }
            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToTeam(teamId) || !rolesRepository.HasWriteAccessToTeam(newTeamId))
            {
                return View("Error");
            }

            string[] sequences = collection["SystemUsers"].Split(new char[] { ',' });

			XmlDocument doc = new XmlDocument();
			XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
			XmlElement root = doc.DocumentElement;
			doc.InsertBefore(xmlDeclaration, root);
			XmlElement rootElement = doc.CreateElement(string.Empty, "SystemUserXML", string.Empty);
			doc.AppendChild(rootElement);

			foreach (string s in sequences)
			{
				XmlElement systemUserGuidElement = doc.CreateElement(string.Empty, "SystemUserGuid", string.Empty);
				XmlCDataSection systemUserGuidText = doc.CreateCDataSection(HttpUtility.HtmlEncode(s.ToString()));
				systemUserGuidElement.AppendChild(systemUserGuidText);
				rootElement.AppendChild(systemUserGuidElement);
			} 
			
			try
            {
                teamRepository.CopySomeTeamUsers(doc.OuterXml, teamId, newTeamId);
            }
            catch
            {
                return View("Error");
            }


            string msg = "You have successfully copied " + sequences.Length + " user(s) from " + fromTeam.TeamName + " to " + toTeam.TeamName;
            return RedirectToAction("MoveCopyUsersCompleted", new { teamId = teamId, newTeamId = newTeamId, msg = msg });
        }

        #endregion

        #region Move All users Step 3 
        //GET - confirm button page
        public ActionResult MoveAllUsersStep3(int teamId, int newTeamId)
        {
            //Get Teams
            Team fromTeam = new Team();
            fromTeam = teamRepository.GetTeam(teamId);
            Team toTeam = new Team();
            toTeam = teamRepository.GetTeam(newTeamId);

            //Check Exists
            if (fromTeam == null || toTeam == null)
            {
                ViewData["ActionMethod"] = "MoveAllUsersStep3Get";
                return View("RecordDoesNotExistError");
            }
            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToTeam(teamId) || !rolesRepository.HasWriteAccessToTeam(newTeamId))
            {
                return View("Error");
            }

            ViewData["TeamName"] = fromTeam.TeamName;
            ViewData["TeamId"] = fromTeam.TeamId;
            ViewData["NewTeamName"] = toTeam.TeamName;
            ViewData["NewTeamId"] = toTeam.TeamId;
            ViewData["Access"] = "WriteAccess";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MoveAllUsersStep3(int teamId, int newTeamId, FormCollection collection)
        {
            //Get Teams
            Team fromTeam = new Team();
            fromTeam = teamRepository.GetTeam(teamId);
            Team toTeam = new Team();
            toTeam = teamRepository.GetTeam(newTeamId);

            //Check Exists
            if (fromTeam == null || toTeam == null)
            {
                ViewData["ActionMethod"] = "MoveAllUsersStep3Post";
                return View("RecordDoesNotExistError");
            }
            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToTeam(teamId) || !rolesRepository.HasWriteAccessToTeam(newTeamId))
            {
                return View("Error");
            }
            try
            {
                teamRepository.MoveAllTeamUsers(teamId, newTeamId);
            }
            catch
            {
                return View("Error");
            }


            string msg = "You have successfully moved all users from " + fromTeam.TeamName + " to " + toTeam.TeamName;
            return RedirectToAction("MoveCopyUsersCompleted", new { teamId = teamId, newTeamId = newTeamId, msg = msg });
        }
#endregion

        #region Move Some Users Step 3
        //GET - confirm button page
        public ActionResult MoveSomeUsersStep3(int teamId, int newTeamId)
        {
            //Get Teams
            Team fromTeam = new Team();
            fromTeam = teamRepository.GetTeam(teamId);
            Team toTeam = new Team();
            toTeam = teamRepository.GetTeam(newTeamId);

            //Check Exists
            if (fromTeam == null || toTeam == null)
            {
                ViewData["ActionMethod"] = "MoveSomeUsersStep3Get";
                return View("RecordDoesNotExistError");
            }
            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToTeam(teamId) || !rolesRepository.HasWriteAccessToTeam(newTeamId))
            {
                return View("Error");
            }
            var result = teamRepository.GetAllSystemUsers(fromTeam.TeamId).ToList();

            ViewData["TeamName"] = fromTeam.TeamName;
            ViewData["TeamId"] = fromTeam.TeamId;
            ViewData["NewTeamName"] = toTeam.TeamName;
            ViewData["NewTeamId"] = toTeam.TeamId;
            ViewData["Access"] = "WriteAccess";

            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MoveSomeUsersStep3(int teamId, int newTeamId, FormCollection collection)
        {
            //Get Teams
            Team fromTeam = new Team();
            fromTeam = teamRepository.GetTeam(teamId);
            Team toTeam = new Team();
            toTeam = teamRepository.GetTeam(newTeamId);

            //Check Exists
            if (fromTeam == null || toTeam == null)
            {
                ViewData["ActionMethod"] = "MoveSomeUsersStep3Post";
                return View("RecordDoesNotExistError");
            }
            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToTeam(teamId) || !rolesRepository.HasWriteAccessToTeam(newTeamId))
            {
                return View("Error");
            }

            string[] sequences = collection["SystemUsers"].Split(new char[] { ',' });

			XmlDocument doc = new XmlDocument();
			XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
			XmlElement root = doc.DocumentElement;
			doc.InsertBefore(xmlDeclaration, root);
			XmlElement rootElement = doc.CreateElement(string.Empty, "SystemUserXML", string.Empty);
			doc.AppendChild(rootElement);

			foreach (string s in sequences)
			{
				XmlElement systemUserGuidElement = doc.CreateElement(string.Empty, "SystemUserGuid", string.Empty);
				XmlCDataSection systemUserGuidText = doc.CreateCDataSection(HttpUtility.HtmlEncode(s.ToString()));
				systemUserGuidElement.AppendChild(systemUserGuidText);
				rootElement.AppendChild(systemUserGuidElement);
			} 

            try
            {
                teamRepository.MoveSomeTeamUsers(doc.OuterXml, teamId, newTeamId);
            }
            catch
            {
                return View("Error");
            }


            string msg = "You have successfully moved " + sequences.Length + " users from " + fromTeam.TeamName + " to " + toTeam.TeamName;
            return RedirectToAction("MoveCopyUsersCompleted", new { teamId = teamId, newTeamId = newTeamId, msg = msg });
        }

        #endregion

        

        
        //show finished message to user
        public ActionResult MoveCopyUsersCompleted(int teamId, int newTeamId, string msg)
        {
            //Get Teams
            Team fromTeam = new Team();
            fromTeam = teamRepository.GetTeam(teamId);
            Team toTeam = new Team();
            toTeam = teamRepository.GetTeam(newTeamId);

            //Check Exists
            if (fromTeam == null || toTeam == null)
            {
                ViewData["ActionMethod"] = "CopyUsersStep2Get";
                return View("RecordDoesNotExistError");
            }
            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToTeam(teamId) || !rolesRepository.HasWriteAccessToTeam(newTeamId))
            {
                return View("Error");
            }


            ViewData["Message"] =msg;
            return View();
        }
    }
}
