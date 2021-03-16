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
	public class SystemUserGDSAccessRightController : Controller
	{
		SystemUserGDSAccessRightRepository SystemUserGDSAccessRightRepository = new SystemUserGDSAccessRightRepository();
		GDSRepository gdsRepository = new GDSRepository();
		GDSAccessTypeRepository gdsAccessTypeRepository = new GDSAccessTypeRepository();
		SystemUserRepository systemUserRepository = new SystemUserRepository();

		HierarchyRepository hierarchyRepository = new HierarchyRepository();
		RolesRepository rolesRepository = new RolesRepository();

		// GET: /List
		public ActionResult ListUnDeleted(string id, int? page, string filter, string sortField, int? sortOrder)
		{
			//Set Access Rights
			ViewData["Access"] = "";
			if (rolesRepository.HasWriteAccessToSystemUserGDSAccessRights(id))
			{
				ViewData["Access"] = "WriteAccess";
			}

			//SortField
			if (string.IsNullOrEmpty(sortField))
			{
				sortField = "GDSName";
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

			//Populate View Model
			SystemUserGDSAccessRightsVM SystemUserGDSAccessRightsVM = new SystemUserGDSAccessRightsVM();

			var getSystemUserGDSAccessRights = SystemUserGDSAccessRightRepository.PageSystemUserGDSAccessRights(id, filter ?? "", sortField, sortOrder ?? 0, page ?? 1, false);
			if (getSystemUserGDSAccessRights != null)
			{
				SystemUserGDSAccessRightsVM.SystemUserGDSAccessRights = getSystemUserGDSAccessRights;
			}

			//SystemUser
			SystemUser systemUser = new SystemUser();
			systemUser = systemUserRepository.GetUserBySystemUserGuid(id);
			if (systemUser != null)
			{
				SystemUserGDSAccessRightsVM.SystemUser = systemUser;
			}

			return View(SystemUserGDSAccessRightsVM);
		}

		// GET: /List
		public ActionResult ListDeleted(string id, int? page, string filter, string sortField, int? sortOrder)
		{
			//Set Access Rights
			ViewData["Access"] = "";
			if (rolesRepository.HasWriteAccessToSystemUserGDSAccessRights(id))
			{
				ViewData["Access"] = "WriteAccess";
			}

			//SortField
			if (string.IsNullOrEmpty(sortField))
			{
				sortField = "GDSName";
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

			//Populate View Model
			SystemUserGDSAccessRightsVM SystemUserGDSAccessRightsVM = new SystemUserGDSAccessRightsVM();

			var getSystemUserGDSAccessRights = SystemUserGDSAccessRightRepository.PageSystemUserGDSAccessRights(id, filter ?? "", sortField, sortOrder ?? 0, page ?? 1, true);
			if (getSystemUserGDSAccessRights != null)
			{
				SystemUserGDSAccessRightsVM.SystemUserGDSAccessRights = getSystemUserGDSAccessRights;
			}

			//SystemUser
			SystemUser systemUser = new SystemUser();
			systemUser = systemUserRepository.GetUserBySystemUserGuid(id);
			if (systemUser != null)
			{
				SystemUserGDSAccessRightsVM.SystemUser = systemUser;
			}

			return View(SystemUserGDSAccessRightsVM);
		}

		public ActionResult Create(string id)
		{
			//Set Access Rights
			ViewData["Access"] = "";
			if (rolesRepository.HasWriteAccessToSystemUserGDSAccessRights(id))
			{
				ViewData["Access"] = "WriteAccess";
			}

			SystemUserGDSAccessRightVM SystemUserGDSAccessRightVM = new SystemUserGDSAccessRightVM();
			
			SystemUserGDSAccessRight SystemUserGDSAccessRight = new SystemUserGDSAccessRight();
			SystemUserGDSAccessRightVM.SystemUserGDSAccessRight = SystemUserGDSAccessRight;

			//GDS
			SystemUserGDSAccessRightVM.GDSs = new SelectList(gdsRepository.GetAllGDSs().ToList(), "GDSCode", "GDSName");

			//GDSAccessTypes
			SystemUserGDSAccessRightVM.GDSAccessTypes = new SelectList(gdsAccessTypeRepository.GetAllGDSAccessTypes().ToList(), "GDSAccessTypeId", "GDSAccessTypeName");

			//SystemUser
			SystemUser systemUser = new SystemUser();
			systemUser = systemUserRepository.GetUserBySystemUserGuid(id);
			if (systemUser != null)
			{
				SystemUserGDSAccessRightVM.SystemUser = systemUser;
				SystemUserGDSAccessRightVM.SystemUserGDSAccessRight.SystemUserGuid = systemUser.SystemUserGuid;
			}

			return View(SystemUserGDSAccessRightVM);
		}

		// POST: /Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(SystemUserGDSAccessRightVM systemUserGDSAccessRightVM)
			{
			//Check Access
			if (!rolesRepository.HasWriteAccessToSystemUserGDSAccessRights(systemUserGDSAccessRightVM.SystemUserGDSAccessRight.SystemUserGuid))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Update  Model from Form
			try
			{
				TryUpdateModel<SystemUserGDSAccessRightVM>(systemUserGDSAccessRightVM, "SystemUserGDSAccessRightVM");
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

			try
			{
				SystemUserGDSAccessRightRepository.Add(systemUserGDSAccessRightVM);
			}
			catch (SqlException ex)
			{
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("ListUnDeleted", new { id = systemUserGDSAccessRightVM.SystemUserGDSAccessRight.SystemUserGuid });
		}

		// GET: /Edit
		public ActionResult Edit(int id)
		{
			SystemUserGDSAccessRight systemUserGDSAccessRight = SystemUserGDSAccessRightRepository.GetSystemUserGDSAccessRight(id);

			//Check Exists
			if (systemUserGDSAccessRight == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			//Check Access
			if (!rolesRepository.HasWriteAccessToSystemUserGDSAccessRight(id))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			SystemUserGDSAccessRightVM systemUserGDSAccessRightVM = new SystemUserGDSAccessRightVM();
			systemUserGDSAccessRightVM.SystemUserGDSAccessRight = systemUserGDSAccessRight;

			//GDS
			systemUserGDSAccessRightVM.GDSs = new SelectList(gdsRepository.GetAllGDSs().ToList(), "GDSCode", "GDSName", systemUserGDSAccessRight.GDSCode);

			//GDSAccessTypes
			systemUserGDSAccessRightVM.GDSAccessTypes = new SelectList(gdsAccessTypeRepository.GetGDSAccessTypesByGDSCode(systemUserGDSAccessRight.GDSCode).ToList(), "GDSAccessTypeId", "GDSAccessTypeName", systemUserGDSAccessRight.GDSAccessTypeId);

			//PseudoCityOrOfficeIds
			systemUserGDSAccessRightVM.PseudoCityOrOfficeIds = new SelectList(
				systemUserRepository.GetSystemUserPseudoCityOrOfficeIdsByGDSCode(systemUserGDSAccessRight.SystemUserGuid, systemUserGDSAccessRight.GDSCode).ToList(), 
				"PseudoCityOrOfficeId", 
				"PseudoCityOrOfficeId",
				systemUserGDSAccessRight.PseudoCityOrOfficeId
			);

			//System User
			SystemUser systemUser = new SystemUser();
			systemUser = systemUserRepository.GetUserBySystemUserGuid(systemUserGDSAccessRight.SystemUserGuid);
			if (systemUser != null)
			{
				systemUserGDSAccessRightVM.SystemUser = systemUser;
			}

			return View(systemUserGDSAccessRightVM);
		}

		// POST: /Edit
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(SystemUserGDSAccessRightVM systemUserGDSAccessRightVM)
		{
			if (!rolesRepository.HasWriteAccessToSystemUserGDSAccessRight(systemUserGDSAccessRightVM.SystemUserGDSAccessRight.SystemUserGDSAccessRightId))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Update  Model from Form
			try
			{
				TryUpdateModel<SystemUserGDSAccessRight>(systemUserGDSAccessRightVM.SystemUserGDSAccessRight, "SystemUserGDSAccessRight");
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

			try
			{
				SystemUserGDSAccessRightRepository.Update(systemUserGDSAccessRightVM);
			}
			catch (SqlException ex)
			{
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("ListUnDeleted", new { id = systemUserGDSAccessRightVM.SystemUserGDSAccessRight.SystemUserGuid });
		}

		// GET: /View
		[HttpGet]
		public ActionResult View(int id)
		{
			SystemUserGDSAccessRight SystemUserGDSAccessRight = new SystemUserGDSAccessRight();
			SystemUserGDSAccessRight = SystemUserGDSAccessRightRepository.GetSystemUserGDSAccessRight(id);

			//Check Exists
			if (SystemUserGDSAccessRight == null)
			{
				ViewData["ActionMethod"] = "DeleteGet";
				return View("RecordDoesNotExistError");
			}

			SystemUserGDSAccessRightVM SystemUserGDSAccessRightVM = new SystemUserGDSAccessRightVM();

			//System User
			SystemUser systemUser = new SystemUser();
			systemUser = systemUserRepository.GetUserBySystemUserGuid(SystemUserGDSAccessRight.SystemUserGuid);
			if (systemUser != null)
			{
				SystemUserGDSAccessRightVM.SystemUser = systemUser;
			}

			SystemUserGDSAccessRightVM.SystemUserGDSAccessRight = SystemUserGDSAccessRight;

			return View(SystemUserGDSAccessRightVM);
		}

		// GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id)
		{
			SystemUserGDSAccessRight SystemUserGDSAccessRight = new SystemUserGDSAccessRight();
			SystemUserGDSAccessRight = SystemUserGDSAccessRightRepository.GetSystemUserGDSAccessRight(id);

			//Check Exists
			if (SystemUserGDSAccessRight == null)
			{
				ViewData["ActionMethod"] = "DeleteGet";
				return View("RecordDoesNotExistError");
			}

			//Check AccessRights
			if (!rolesRepository.HasWriteAccessToSystemUserGDSAccessRight(id))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			SystemUserGDSAccessRightVM SystemUserGDSAccessRightVM = new SystemUserGDSAccessRightVM();

			//System User
			SystemUser systemUser = new SystemUser();
			systemUser = systemUserRepository.GetUserBySystemUserGuid(SystemUserGDSAccessRight.SystemUserGuid);
			if (systemUser != null)
			{
				SystemUserGDSAccessRightVM.SystemUser = systemUser;
			}

			SystemUserGDSAccessRightVM.SystemUserGDSAccessRight = SystemUserGDSAccessRight;

			return View(SystemUserGDSAccessRightVM);
		}

		// POST: /Delete
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(SystemUserGDSAccessRightVM systemUserGDSAccessRightVM, FormCollection collection)
		{
			//Check Access
			if (!rolesRepository.HasWriteAccessToSystemUserGDSAccessRight(systemUserGDSAccessRightVM.SystemUserGDSAccessRight.SystemUserGDSAccessRightId))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Get Item From Database
			SystemUserGDSAccessRight SystemUserGDSAccessRight = new SystemUserGDSAccessRight();
			SystemUserGDSAccessRight = SystemUserGDSAccessRightRepository.GetSystemUserGDSAccessRight(systemUserGDSAccessRightVM.SystemUserGDSAccessRight.SystemUserGDSAccessRightId);

			//Check Exists
			if (SystemUserGDSAccessRight == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Delete Item
			try
			{
				systemUserGDSAccessRightVM.SystemUserGDSAccessRight.DeletedFlag = true;
				SystemUserGDSAccessRightRepository.UpdateDeletedStatus(systemUserGDSAccessRightVM);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/SystemUserGDSAccessRight.mvc/Delete/" + SystemUserGDSAccessRight.SystemUserGDSAccessRightId;
					return View("VersionError");
				}
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			return RedirectToAction("ListUnDeleted", new { id = systemUserGDSAccessRightVM.SystemUserGDSAccessRight.SystemUserGuid });
		}

		// GET: /UnDelete
		[HttpGet]
		public ActionResult UnDelete(int id)
		{
			SystemUserGDSAccessRight SystemUserGDSAccessRight = new SystemUserGDSAccessRight();
			SystemUserGDSAccessRight = SystemUserGDSAccessRightRepository.GetSystemUserGDSAccessRight(id);

			//Check Exists
			if (SystemUserGDSAccessRight == null)
			{
				ViewData["ActionMethod"] = "DeleteGet";
				return View("RecordDoesNotExistError");
			}

			//Check AccessRights
			if (!rolesRepository.HasWriteAccessToSystemUserGDSAccessRight(id))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			SystemUserGDSAccessRightVM SystemUserGDSAccessRightVM = new SystemUserGDSAccessRightVM();

			//System User
			SystemUser systemUser = new SystemUser();
			systemUser = systemUserRepository.GetUserBySystemUserGuid(SystemUserGDSAccessRight.SystemUserGuid);
			if (systemUser != null)
			{
				SystemUserGDSAccessRightVM.SystemUser = systemUser;
			}

			SystemUserGDSAccessRightVM.SystemUserGDSAccessRight = SystemUserGDSAccessRight;

			return View(SystemUserGDSAccessRightVM);
		}

		// POST: /UnDelete
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult UnDelete(SystemUserGDSAccessRightVM systemUserGDSAccessRightVM, FormCollection collection)
		{
			//Check Access
			if (!rolesRepository.HasWriteAccessToSystemUserGDSAccessRight(systemUserGDSAccessRightVM.SystemUserGDSAccessRight.SystemUserGDSAccessRightId))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Get Item From Database
			SystemUserGDSAccessRight SystemUserGDSAccessRight = new SystemUserGDSAccessRight();
			SystemUserGDSAccessRight = SystemUserGDSAccessRightRepository.GetSystemUserGDSAccessRight(systemUserGDSAccessRightVM.SystemUserGDSAccessRight.SystemUserGDSAccessRightId);

			//Check Exists
			if (SystemUserGDSAccessRight == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Delete Item
			try
			{
				systemUserGDSAccessRightVM.SystemUserGDSAccessRight.DeletedFlag = false;
				SystemUserGDSAccessRightRepository.UpdateDeletedStatus(systemUserGDSAccessRightVM);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/SystemUserGDSAccessRight.mvc/Delete/" + SystemUserGDSAccessRight.SystemUserGDSAccessRightId;
					return View("VersionError");
				}
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			return RedirectToAction("ListDeleted", new { id = systemUserGDSAccessRightVM.SystemUserGDSAccessRight.SystemUserGuid });
		}

		// GET: /Export
		public ActionResult Export(string id)
		{
			//Get CSV Data
			byte[] csvData = SystemUserGDSAccessRightRepository.Export(id);
			return File(csvData, "text/csv", "GDS Access Right Export.csv");
		}

		//Get PseudoCityOrOfficeIds by GDSCode
		[HttpPost]
		public JsonResult GetSystemUserPseudoCityOrOfficeIdsByGDSCode(string systemUserGuid, string gdsCode)
		{
			var result = systemUserRepository.GetSystemUserPseudoCityOrOfficeIdsByGDSCode(systemUserGuid, gdsCode);
			return Json(result);
		}
	}
}