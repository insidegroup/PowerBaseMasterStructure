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
	public class GDSAccessTypeController : Controller
	{
		GDSAccessTypeRepository gdsAccessTypeRepository = new GDSAccessTypeRepository();
		GDSRepository gdsRepository = new GDSRepository();
		HierarchyRepository hierarchyRepository = new HierarchyRepository();

		private string groupName = "GDS Reference Info Administrator";

		// GET: /List
		public ActionResult List(int? page, string filter, string sortField, int? sortOrder)
		{
			//Set Access Rights
			ViewData["Access"] = "";
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
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
			GDSAccessTypesVM gdsAccessTypesVM = new GDSAccessTypesVM();

			var getGDSAccessTypes = gdsAccessTypeRepository.PageGDSAccessTypes(filter ?? "", sortField, sortOrder ?? 0, page ?? 1);
			if (getGDSAccessTypes != null)
			{
				gdsAccessTypesVM.GDSAccessTypes = getGDSAccessTypes;
			}

			return View(gdsAccessTypesVM);
		}

		public ActionResult Create()
		{
			//Set Access Rights
			ViewData["Access"] = "";
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Access"] = "WriteAccess";
			}

			GDSAccessTypeVM gdsAccessTypeVM = new GDSAccessTypeVM();
			
			GDSAccessType gdsAccessType = new GDSAccessType();
			gdsAccessTypeVM.GDSAccessType = gdsAccessType;

			//GDS
			gdsAccessTypeVM.GDSs = new SelectList(gdsRepository.GetAllGDSs().ToList(), "GDSCode", "GDSName");

			return View(gdsAccessTypeVM);
		}

		// POST: /Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(GDSAccessTypeVM gdsAccessTypeVM)
		{
			//Set Access Rights
			ViewData["Access"] = "";
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Access"] = "WriteAccess";
			}

			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Update  Model from Form
			try
			{
				TryUpdateModel<GDSAccessTypeVM>(gdsAccessTypeVM, "GDSGDSAccessTypeVM");
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
				gdsAccessTypeRepository.Add(gdsAccessTypeVM);
			}
			catch (SqlException ex)
			{
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("List");
		}

		// GET: /Edit
		public ActionResult Edit(int id)
		{
			//Set Access Rights
			ViewData["Access"] = "";
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Access"] = "WriteAccess";
			}

			GDSAccessType gdsAccessType = gdsAccessTypeRepository.GetGDSAccessType(id);

			//Check Exists
			if (gdsAccessType == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			GDSAccessTypeVM gdsAccessTypeVM = new GDSAccessTypeVM();
			gdsAccessTypeVM.GDSAccessType = gdsAccessType;

			//GDS
			gdsAccessTypeVM.GDSs = new SelectList(gdsRepository.GetAllGDSs().ToList(), "GDSCode", "GDSName", gdsAccessType.GDSCode);

			return View(gdsAccessTypeVM);
		}

		// POST: /Edit
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(GDSAccessTypeVM gdsAccessTypeVM)
		{
			RolesRepository rolesRepository = new RolesRepository();
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Update  Model from Form
			try
			{
				TryUpdateModel<GDSAccessType>(gdsAccessTypeVM.GDSAccessType, "GDSAccessType");
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
				gdsAccessTypeRepository.Update(gdsAccessTypeVM);
			}
			catch (SqlException ex)
			{
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
			GDSAccessType gdsAccessType = new GDSAccessType();
			gdsAccessType = gdsAccessTypeRepository.GetGDSAccessType(id);

			//Check Exists
			if (gdsAccessType == null)
			{
				ViewData["ActionMethod"] = "DeleteGet";
				return View("RecordDoesNotExistError");
			}

			//Check AccessRights
			RolesRepository rolesRepository = new RolesRepository();
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			GDSAccessTypeVM gdsAccessTypeVM = new GDSAccessTypeVM();
			gdsAccessTypeVM.AllowDelete = true;

			//Attached Items
			List<GDSAccessTypeReference> gdsAccessTypeReferences = gdsAccessTypeRepository.GetGDSAccessTypeReferences(gdsAccessType.GDSAccessTypeId);
			if (gdsAccessTypeReferences.Count > 0)
			{
				gdsAccessTypeVM.AllowDelete = false;
				gdsAccessTypeVM.GDSAccessTypeReferences = gdsAccessTypeReferences;
			}

			gdsAccessTypeVM.GDSAccessType = gdsAccessType;

			return View(gdsAccessTypeVM);
		}

		// POST: /Delete
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(GDSAccessTypeVM gdsAccessTypeVM, FormCollection collection)
		{
			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Get Item From Database
			GDSAccessType gdsAccessType = new GDSAccessType();
			gdsAccessType = gdsAccessTypeRepository.GetGDSAccessType(gdsAccessTypeVM.GDSAccessType.GDSAccessTypeId);

			//Check Exists
			if (gdsAccessType == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Delete Item
			try
			{
				gdsAccessTypeRepository.Delete(gdsAccessTypeVM);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/GDSAccessType.mvc/Delete/" + gdsAccessType.GDSAccessTypeId;
					return View("VersionError");
				}
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			return RedirectToAction("List");
		}

		//Get GDSAccessTypes by GDSCode
		[HttpPost]
		public JsonResult GetGDSAccessTypesByGDSCode(string gdsCode)
		{
			var result = gdsAccessTypeRepository.GetGDSAccessTypesByGDSCode(gdsCode);
			return Json(result);
		}
	}
}