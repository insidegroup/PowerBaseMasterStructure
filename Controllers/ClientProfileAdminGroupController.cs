using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Repository;
using CWTDesktopDatabase.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Controllers
{
	public class ClientProfileAdminGroupController : Controller
	{
		// Main repository
		ClientProfileAdminGroupRepository clientProfileAdminGroupRepository = new ClientProfileAdminGroupRepository();
		HierarchyRepository hierarchyRepository = new HierarchyRepository();

		private string groupName = "Client Profile Builder Administrator";

		//
		// GET: /ClientProfileAdminGroup/ListUnDeleted

		public ActionResult ListUnDeleted(string filter, int? page, string sortField, int? sortOrder)
		{
			//SortField
			if (sortField == string.Empty)
			{
				sortField = "ClientProfileAdminGroupName";
			}
			ViewData["CurrentSortField"] = sortField;

			//SortOrder
			if (sortOrder == 1)
			{
				ViewData["NewSortOrder"] = 0;
				ViewData["CurrentSortOrder"] = 1;
			}
			else
			{
				ViewData["NewSortOrder"] = 1;
				ViewData["CurrentSortOrder"] = 0;
				sortOrder = 0;
			}

			ClientProfileAdminGroupsVM clientProfileAdminGroupsVM = new ClientProfileAdminGroupsVM();

			//Set Access Rights
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				clientProfileAdminGroupsVM.HasDomainWriteAccess = true;
			}

			if (clientProfileAdminGroupRepository != null)
			{
				var clientProfileAdminGroups = clientProfileAdminGroupRepository.PageClientProfileAdminGroups(false, page ?? 1, filter ?? "", sortField, sortOrder ?? 0);
				
				if (clientProfileAdminGroups != null)
				{
					clientProfileAdminGroupsVM.ClientProfileAdminGroups = clientProfileAdminGroups;
				}
			}

			//return items
			return View(clientProfileAdminGroupsVM);
		}

		// GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id)
		{
			//Get Item From Database
			ClientProfileAdminGroup clientProfileAdminGroup = new ClientProfileAdminGroup();
			clientProfileAdminGroup = clientProfileAdminGroupRepository.GetGroup(id);

			//Check Exists
			if (clientProfileAdminGroup == null || clientProfileAdminGroup.DeletedFlag == true)
			{
				ViewData["ActionMethod"] = "DeleteGet";
				return View("RecordDoesNotExistError");
			}
			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToClientProfileAdminGroup(id))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			ClientProfileAdminGroupVM clientProfileAdminGroupVM = new ClientProfileAdminGroupVM();
			clientProfileAdminGroupRepository.EditGroupForDisplay(clientProfileAdminGroup);
			clientProfileAdminGroupVM.ClientProfileAdminGroup = clientProfileAdminGroup;

			return View(clientProfileAdminGroupVM);
		}

		// POST: /Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(ClientProfileAdminGroupVM clientProfileAdminGroupVM)
		{
			//Check Valid Item passed in Form       
			if (clientProfileAdminGroupVM.ClientProfileAdminGroup == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Get Item From Database
			ClientProfileAdminGroup clientProfileAdminGroup = new ClientProfileAdminGroup();
			clientProfileAdminGroup = clientProfileAdminGroupRepository.GetGroup(clientProfileAdminGroupVM.ClientProfileAdminGroup.ClientProfileAdminGroupId);

			//Check Exists in Databsase
			if (clientProfileAdminGroup == null || clientProfileAdminGroup.DeletedFlag == true)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToClientProfileAdminGroup(clientProfileAdminGroup.ClientProfileAdminGroupId))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Delete Form Item
			try
			{
				clientProfileAdminGroupVM.ClientProfileAdminGroup.DeletedFlag = true;
				clientProfileAdminGroupRepository.UpdateGroupDeletedStatus(clientProfileAdminGroupVM.ClientProfileAdminGroup);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/ClientProfileAdminGroup.mvc/Delete/" + clientProfileAdminGroup.ClientProfileAdminGroupId;
					return View("VersionError");
				}

				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			return RedirectToAction("ListUnDeleted");
		}

		//
		// GET: /ClientProfileAdminGroup/ListDeleted

		public ActionResult ListDeleted(string filter, int? page, string sortField, int? sortOrder)
		{
			//SortField
			if (sortField == string.Empty)
			{
				sortField = "UniqueID";
			}
			ViewData["CurrentSortField"] = sortField;

			//SortOrder
			if (sortOrder == 1)
			{
				ViewData["NewSortOrder"] = 0;
				ViewData["CurrentSortOrder"] = 1;
			}
			else
			{
				ViewData["NewSortOrder"] = 1;
				ViewData["CurrentSortOrder"] = 0;
				sortOrder = 0;
			}

			ClientProfileAdminGroupsVM clientProfileAdminGroupsVM = new ClientProfileAdminGroupsVM();

			//Set Access Rights
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				clientProfileAdminGroupsVM.HasDomainWriteAccess = true;
			}

			//return items
			clientProfileAdminGroupsVM.ClientProfileAdminGroups = clientProfileAdminGroupRepository.PageClientProfileAdminGroups(true, page ?? 1, filter ?? "", sortField, sortOrder ?? 0);

			return View(clientProfileAdminGroupsVM);
		}

		//
		// GET: /ClientProfileAdminGroup/Create

		public ActionResult Create()
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			ClientProfileAdminGroupVM clientProfileAdminGroupVM = new ClientProfileAdminGroupVM();

			//GDS List
			GDSRepository gDSRepository = new GDSRepository();
			SelectList gDSs = new SelectList(gDSRepository.GetClientProfileBuilderGDSs().ToList(), "GDSCode", "GDSName");
			clientProfileAdminGroupVM.GDSs = gDSs;

			//BackOfficeSystem List

			//Only show All option if user has a global role
			string adminUserGuid = System.Web.HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			//if adminUserGuid is global
            RolesRepository rolesRepository = new RolesRepository();
            BackOfficeSystemRepository backOfficeSystemRepository = new BackOfficeSystemRepository();
            if(rolesRepository.HasWriteAccessToGroupHierarchyLevel(groupName, "Global")){
			    SelectList backOffices = new SelectList(backOfficeSystemRepository.GetAllBackOfficeSystems().ToList(), "BackOfficeSytemId", "BackOfficeSystemDescription");
			    clientProfileAdminGroupVM.BackOffices = backOffices;
            }else{
			    SelectList backOffices = new SelectList(backOfficeSystemRepository.GetBackOfficeSystemsExceptAll().ToList(), "BackOfficeSytemId", "BackOfficeSystemDescription");
			    clientProfileAdminGroupVM.BackOffices = backOffices;
            }
			

			//Hierarchy List
			TablesDomainHierarchyLevelRepository tablesDomainHierarchyLevelRepository = new TablesDomainHierarchyLevelRepository();
			SelectList hierarchyTypesList = new SelectList(tablesDomainHierarchyLevelRepository.GetDomainHierarchies(groupName).ToList(), "HierarchyLevelTableName", "HierarchyLevelTableName");
			clientProfileAdminGroupVM.HierarchyTypes = hierarchyTypesList;

			ClientProfileAdminGroup clientProfileAdminGroup = new ClientProfileAdminGroup();
			clientProfileAdminGroupVM.ClientProfileAdminGroup = clientProfileAdminGroup;

			return View(clientProfileAdminGroupVM);
		}
		
		// POST: /Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(ClientProfileAdminGroupVM clientProfileAdminGroupVM)
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//We need to extract group from groupVM
			ClientProfileAdminGroup clientProfileAdminGroup = new ClientProfileAdminGroup();
			clientProfileAdminGroup = clientProfileAdminGroupVM.ClientProfileAdminGroup;
			if (clientProfileAdminGroup == null)
			{
				ViewData["Message"] = "ValidationError : missing item"; ;
				return View("Error");
			}

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel<ClientProfileAdminGroup>(clientProfileAdminGroup, "ClientProfileAdminGroup");
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
				clientProfileAdminGroupRepository.Add(clientProfileAdminGroup);
			}
			catch (SqlException ex)
			{
				//Non-Unique Name
				if (ex.Message == "NonUniqueName")
				{
					return View("NonUniqueNameError");
				}
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			ViewData["NewSortOrder"] = 0;
			return RedirectToAction("ListUnDeleted");
		}

		// GET: /UnDelete
		public ActionResult UnDelete(int id)
		{
			//Get Item From Database
			ClientProfileAdminGroup clientProfileAdminGroup = new ClientProfileAdminGroup();
			clientProfileAdminGroup = clientProfileAdminGroupRepository.GetGroup(id);

			//Check Exists
			if (clientProfileAdminGroup == null || clientProfileAdminGroup.DeletedFlag == false)
			{
				ViewData["ActionMethod"] = "DeleteGet";
				return View("RecordDoesNotExistError");
			}
			
			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToClientProfileAdminGroup(id))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			ClientProfileAdminGroupVM clientProfileAdminGroupVM = new ClientProfileAdminGroupVM();

			clientProfileAdminGroupRepository.EditGroupForDisplay(clientProfileAdminGroup);
			clientProfileAdminGroupVM.ClientProfileAdminGroup = clientProfileAdminGroup;

			return View(clientProfileAdminGroupVM);
		}

		// POST: /UnDelete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult UnDelete(ClientProfileAdminGroupVM clientProfileAdminGroupVM)
		{
			//Check Valid Item passed in Form       
			if (clientProfileAdminGroupVM.ClientProfileAdminGroup == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Get Item From Database
			ClientProfileAdminGroup clientProfileAdminGroup = new ClientProfileAdminGroup();
			clientProfileAdminGroup = clientProfileAdminGroupRepository.GetGroup(clientProfileAdminGroupVM.ClientProfileAdminGroup.ClientProfileAdminGroupId);

			//Check Exists in Databsase
			if (clientProfileAdminGroup == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToClientProfileAdminGroup(clientProfileAdminGroup.ClientProfileAdminGroupId))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Delete Form Item
			try
			{
				clientProfileAdminGroupVM.ClientProfileAdminGroup.DeletedFlag = false;
				clientProfileAdminGroupRepository.UpdateGroupDeletedStatus(clientProfileAdminGroupVM.ClientProfileAdminGroup);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/ClientProfileAdminGroup.mvc/UnDelete/" + clientProfileAdminGroup.ClientProfileAdminGroupId;
					return View("VersionError");
				}

				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			return RedirectToAction("ListDeleted");
		}
	}
}
