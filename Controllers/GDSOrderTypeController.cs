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
	public class GDSOrderTypeController : Controller
	{
		GDSOrderTypeRepository gdsOrderTypeRepository = new GDSOrderTypeRepository();
		GDSRepository gdsRepository = new GDSRepository();
		HierarchyRepository hierarchyRepository = new HierarchyRepository();
		RolesRepository rolesRepository = new RolesRepository();

		// GET: /List
		public ActionResult List(int? page, string filter, string sortField, int? sortOrder)
		{
			//Set Access Rights
			ViewData["Access"] = "";
			if (rolesRepository.HasWriteAccessToGDSOrderType())
			{
				ViewData["Access"] = "WriteAccess";
			}

			//SortField
			if (string.IsNullOrEmpty(sortField))
			{
				sortField = "GDSOrderTypeName";
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
			GDSOrderTypesVM gdsOrderTypesVM = new GDSOrderTypesVM();

			var getGDSOrderTypes = gdsOrderTypeRepository.PageGDSOrderTypes(filter ?? "", sortField, sortOrder ?? 0, page ?? 1);
			if (getGDSOrderTypes != null)
			{
				gdsOrderTypesVM.GDSOrderTypes = getGDSOrderTypes;
			}

			return View(gdsOrderTypesVM);
		}

		public ActionResult Create()
		{
			//Set Access Rights
			ViewData["Access"] = "";
			if (rolesRepository.HasWriteAccessToGDSOrderType())
			{
				ViewData["Access"] = "WriteAccess";
			}

			GDSOrderTypeVM gdsOrderTypeVM = new GDSOrderTypeVM();
			
			GDSOrderType gdsOrderType = new GDSOrderType();
			gdsOrderTypeVM.GDSOrderType = gdsOrderType;

			return View(gdsOrderTypeVM);
		}

		// POST: /Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(GDSOrderTypeVM gdsOrderTypeVM)
		{
			//Set Access Rights
			ViewData["Access"] = "";
			if (rolesRepository.HasWriteAccessToGDSOrderType())
			{
				ViewData["Access"] = "WriteAccess";
			}

			//Check Access
			if (!rolesRepository.HasWriteAccessToGDSOrderType())
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Update  Model from Form
			try
			{
				TryUpdateModel<GDSOrderTypeVM>(gdsOrderTypeVM, "GDSGDSOrderTypeVM");
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
				gdsOrderTypeRepository.Add(gdsOrderTypeVM);
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
			if (rolesRepository.HasWriteAccessToGDSOrderType())
			{
				ViewData["Access"] = "WriteAccess";
			}

			GDSOrderType gdsOrderType = gdsOrderTypeRepository.GetGDSOrderType(id);

			//Check Exists
			if (gdsOrderType == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			//Check Access
			if (!rolesRepository.HasWriteAccessToGDSOrderType())
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			GDSOrderTypeVM gdsOrderTypeVM = new GDSOrderTypeVM();
			gdsOrderTypeRepository.EditForDisplay(gdsOrderType);
			gdsOrderTypeVM.GDSOrderType = gdsOrderType;

			return View(gdsOrderTypeVM);
		}

		// POST: /Edit
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(GDSOrderTypeVM gdsOrderTypeVM)
		{
			if (!rolesRepository.HasWriteAccessToGDSOrderType())
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Update  Model from Form
			try
			{
				TryUpdateModel<GDSOrderType>(gdsOrderTypeVM.GDSOrderType, "GDSOrderType");
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
				gdsOrderTypeRepository.Update(gdsOrderTypeVM);
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
			GDSOrderType gdsOrderType = new GDSOrderType();
			gdsOrderType = gdsOrderTypeRepository.GetGDSOrderType(id);

			//Check Exists
			if (gdsOrderType == null)
			{
				ViewData["ActionMethod"] = "DeleteGet";
				return View("RecordDoesNotExistError");
			}

			//Check AccessRights
			if (!rolesRepository.HasWriteAccessToGDSOrderType())
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			GDSOrderTypeVM gdsOrderTypeVM = new GDSOrderTypeVM();
			gdsOrderTypeVM.AllowDelete = true;

			//Attached Items
			List<GDSOrderTypeReference> gdsOrderTypeReferences = gdsOrderTypeRepository.GetGDSOrderTypeReferences(gdsOrderType.GDSOrderTypeId);
			if (gdsOrderTypeReferences.Count > 0)
			{
				gdsOrderTypeVM.AllowDelete = false;
				gdsOrderTypeVM.GDSOrderTypeReferences = gdsOrderTypeReferences;
			}

			gdsOrderTypeVM.GDSOrderType = gdsOrderType;

			return View(gdsOrderTypeVM);
		}

		// POST: /Delete
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(GDSOrderTypeVM gdsOrderTypeVM, FormCollection collection)
		{
			//Check Access
			if (!rolesRepository.HasWriteAccessToGDSOrderType())
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Get Item From Database
			GDSOrderType gdsOrderType = new GDSOrderType();
			gdsOrderType = gdsOrderTypeRepository.GetGDSOrderType(gdsOrderTypeVM.GDSOrderType.GDSOrderTypeId);

			//Check Exists
			if (gdsOrderType == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Delete Item
			try
			{
				gdsOrderTypeRepository.Delete(gdsOrderTypeVM);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/GDSOrderType.mvc/Delete/" + gdsOrderType.GDSOrderTypeId;
					return View("VersionError");
				}
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			return RedirectToAction("List");
		}

		//Is GDSOrderType ThirdPartyMandatory
		[HttpPost]
		public JsonResult IsGDSOrderTypeThirdPartyMandatory(int gdsOrderTypeId)
		{
			return Json(gdsOrderTypeRepository.IsGDSOrderTypeThirdPartyMandatory(gdsOrderTypeId));
		}

		//GDSOrderType ShowData Flag
		[HttpPost]
		public JsonResult GDSOrderTypeShowDataFlag(int gdsOrderTypeId)
		{
			return Json(gdsOrderTypeRepository.GDSOrderTypeShowDataFlag(gdsOrderTypeId));
		}
	}
}