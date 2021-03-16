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
	public class GDSRequestTypeController : Controller
	{
		GDSRequestTypeRepository gdsRequestTypeRepository = new GDSRequestTypeRepository();
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
				sortField = "GDSRequestType";
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
			GDSRequestTypesVM gdsRequestTypesVM = new GDSRequestTypesVM();

			var getGDSRequestTypes = gdsRequestTypeRepository.GetGDSRequestTypes(sortField, sortOrder ?? 0, page ?? 1);
			if (getGDSRequestTypes != null)
			{
				gdsRequestTypesVM.GDSRequestTypes = getGDSRequestTypes;
			}

			return View(gdsRequestTypesVM);
		}

		public ActionResult Create()
		{
			//Set Access Rights
			ViewData["Access"] = "";
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Access"] = "WriteAccess";
			}

			GDSRequestTypeVM gdsRequestTypeVM = new GDSRequestTypeVM();
			GDSRequestType gdsRequestType = new GDSRequestType();
			gdsRequestTypeVM.GDSRequestType = gdsRequestType;

			return View(gdsRequestTypeVM);
		}

		// POST: /Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(GDSRequestTypeVM gdsRequestTypeVM)
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
				TryUpdateModel<GDSRequestTypeVM>(gdsRequestTypeVM, "GDSGDSRequestTypeVM");
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
				gdsRequestTypeRepository.Add(gdsRequestTypeVM);
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

			GDSRequestType gdsRequestType = gdsRequestTypeRepository.GetGDSRequestType(id);

			//Check Exists
			if (gdsRequestType == null)
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

			GDSRequestTypeVM gdsRequestTypeVM = new GDSRequestTypeVM();
			gdsRequestTypeVM.GDSRequestType = gdsRequestType;

			return View(gdsRequestTypeVM);
		}

		// POST: /Edit
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(GDSRequestTypeVM gdsRequestTypeVM)
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
				TryUpdateModel<GDSRequestType>(gdsRequestTypeVM.GDSRequestType, "GDSRequestType");
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
				gdsRequestTypeRepository.Update(gdsRequestTypeVM);
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
			GDSRequestType gdsRequestType = new GDSRequestType();
			gdsRequestType = gdsRequestTypeRepository.GetGDSRequestType(id);

			//Check Exists
			if (gdsRequestType == null)
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

			GDSRequestTypeVM gdsRequestTypeVM = new GDSRequestTypeVM();
			gdsRequestTypeVM.AllowDelete = true;

			//Attached Items
			List<GDSRequestTypeReference> gdsRequestTypeReferences = gdsRequestTypeRepository.GetGDSRequestTypeReferences(gdsRequestType.GDSRequestTypeId);
			if (gdsRequestTypeReferences.Count > 0)
			{
				gdsRequestTypeVM.AllowDelete = false;
				gdsRequestTypeVM.GDSRequestTypeReferences = gdsRequestTypeReferences;
			}

			gdsRequestTypeVM.GDSRequestType = gdsRequestType;

			return View(gdsRequestTypeVM);
		}

		// POST: /Delete
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(GDSRequestTypeVM gdsRequestTypeVM, FormCollection collection)
		{
			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Get Item From Database
			GDSRequestType gdsRequestType = new GDSRequestType();
			gdsRequestType = gdsRequestTypeRepository.GetGDSRequestType(gdsRequestTypeVM.GDSRequestType.GDSRequestTypeId);

			//Check Exists
			if (gdsRequestType == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Delete Item
			try
			{
				gdsRequestTypeRepository.Delete(gdsRequestTypeVM);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/GDSRequestType.mvc/Delete/" + gdsRequestType.GDSRequestTypeId;
					return View("VersionError");
				}
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			return RedirectToAction("List");
		}
	}
}