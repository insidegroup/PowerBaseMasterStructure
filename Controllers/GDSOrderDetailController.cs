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
	public class GDSOrderDetailController : Controller
	{
		GDSOrderDetailRepository gdsOrderDetailRepository = new GDSOrderDetailRepository();
		GDSRepository gdsRepository = new GDSRepository();
		HierarchyRepository hierarchyRepository = new HierarchyRepository();
		RolesRepository rolesRepository = new RolesRepository();

		// GET: /List
		public ActionResult List(int? page, string filter, string sortField, int? sortOrder)
		{
			//Set Access Rights
			ViewData["Access"] = "";
			if (rolesRepository.HasWriteAccessToGDSOrderDetail())
			{
				ViewData["Access"] = "WriteAccess";
			}

			//SortField
			if (string.IsNullOrEmpty(sortField))
			{
				sortField = "GDSOrderDetailName";
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
			GDSOrderDetailsVM gdsOrderDetailsVM = new GDSOrderDetailsVM();

			var getGDSOrderDetails = gdsOrderDetailRepository.PageGDSOrderDetails(filter ?? "", sortField, sortOrder ?? 0, page ?? 1);
			if (getGDSOrderDetails != null)
			{
				gdsOrderDetailsVM.GDSOrderDetails = getGDSOrderDetails;
			}

			return View(gdsOrderDetailsVM);
		}

		public ActionResult Create()
		{
			//Set Access Rights
			ViewData["Access"] = "";
			if (rolesRepository.HasWriteAccessToGDSOrderDetail())
			{
				ViewData["Access"] = "WriteAccess";
			}

			GDSOrderDetailVM gdsOrderDetailVM = new GDSOrderDetailVM();
			
			GDSOrderDetail gdsOrderDetail = new GDSOrderDetail();
			gdsOrderDetailVM.GDSOrderDetail = gdsOrderDetail;

			return View(gdsOrderDetailVM);
		}

		// POST: /Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(GDSOrderDetailVM gdsOrderDetailVM)
		{
			//Set Access Rights
			ViewData["Access"] = "";
			if (rolesRepository.HasWriteAccessToGDSOrderDetail())
			{
				ViewData["Access"] = "WriteAccess";
			}

			//Check Access
			if (!rolesRepository.HasWriteAccessToGDSOrderDetail())
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Update  Model from Form
			try
			{
				TryUpdateModel<GDSOrderDetailVM>(gdsOrderDetailVM, "GDSGDSOrderDetailVM");
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
				gdsOrderDetailRepository.Add(gdsOrderDetailVM);
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
			if (rolesRepository.HasWriteAccessToGDSOrderDetail())
			{
				ViewData["Access"] = "WriteAccess";
			}

			GDSOrderDetail gdsOrderDetail = gdsOrderDetailRepository.GetGDSOrderDetail(id);

			//Check Exists
			if (gdsOrderDetail == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			//Check Access
			if (!rolesRepository.HasWriteAccessToGDSOrderDetail())
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			GDSOrderDetailVM gdsOrderDetailVM = new GDSOrderDetailVM();
			gdsOrderDetailRepository.EditForDisplay(gdsOrderDetail);
			gdsOrderDetailVM.GDSOrderDetail = gdsOrderDetail;

			return View(gdsOrderDetailVM);
		}

		// POST: /Edit
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(GDSOrderDetailVM gdsOrderDetailVM)
		{
			if (!rolesRepository.HasWriteAccessToGDSOrderDetail())
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Update  Model from Form
			try
			{
				TryUpdateModel<GDSOrderDetail>(gdsOrderDetailVM.GDSOrderDetail, "GDSOrderDetail");
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
				gdsOrderDetailRepository.Update(gdsOrderDetailVM);
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
			GDSOrderDetail gdsOrderDetail = new GDSOrderDetail();
			gdsOrderDetail = gdsOrderDetailRepository.GetGDSOrderDetail(id);

			//Check Exists
			if (gdsOrderDetail == null)
			{
				ViewData["ActionMethod"] = "DeleteGet";
				return View("RecordDoesNotExistError");
			}

			//Check AccessRights
			if (!rolesRepository.HasWriteAccessToGDSOrderDetail())
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			GDSOrderDetailVM gdsOrderDetailVM = new GDSOrderDetailVM();
			gdsOrderDetailVM.AllowDelete = true;

			//Attached Items
			List<GDSOrderDetailReference> gdsOrderDetailReferences = gdsOrderDetailRepository.GetGDSOrderDetailReferences(gdsOrderDetail.GDSOrderDetailId);
			if (gdsOrderDetailReferences.Count > 0)
			{
				gdsOrderDetailVM.AllowDelete = false;
				gdsOrderDetailVM.GDSOrderDetailReferences = gdsOrderDetailReferences;
			}

			gdsOrderDetailVM.GDSOrderDetail = gdsOrderDetail;

			return View(gdsOrderDetailVM);
		}

		// POST: /Delete
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(GDSOrderDetailVM gdsOrderDetailVM, FormCollection collection)
		{
			//Check Access
			if (!rolesRepository.HasWriteAccessToGDSOrderDetail())
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Get Item From Database
			GDSOrderDetail gdsOrderDetail = new GDSOrderDetail();
			gdsOrderDetail = gdsOrderDetailRepository.GetGDSOrderDetail(gdsOrderDetailVM.GDSOrderDetail.GDSOrderDetailId);

			//Check Exists
			if (gdsOrderDetail == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Delete Item
			try
			{
				gdsOrderDetailRepository.Delete(gdsOrderDetailVM);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/GDSOrderDetail.mvc/Delete/" + gdsOrderDetail.GDSOrderDetailId;
					return View("VersionError");
				}
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			return RedirectToAction("List");
		}

		//Get PseudoCityOrOfficeIds by GDSCode
		[HttpPost]
		public JsonResult GetGDSOrderDetailsByGDSCode(string gdsCode)
		{
			var result = gdsOrderDetailRepository.GetGDSOrderDetailsByGDSCode(gdsCode);
			return Json(result);
		}
	}
}