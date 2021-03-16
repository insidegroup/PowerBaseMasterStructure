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
	public class FareRedistributionController : Controller
	{
		FareRedistributionRepository fareRedistributionRepository = new FareRedistributionRepository();
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
				sortField = "FareRedistributionName";
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
			FareRedistributionsVM fareRedistributionsVM = new FareRedistributionsVM();

			var getFareRedistributions = fareRedistributionRepository.GetFareRedistributions(filter ?? "", sortField, sortOrder ?? 0, page ?? 1);
			if (getFareRedistributions != null)
			{
				fareRedistributionsVM.FareRedistributions = getFareRedistributions;
			}

			return View(fareRedistributionsVM);
		}

		public ActionResult Create()
		{
			//Set Access Rights
			ViewData["Access"] = "";
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Access"] = "WriteAccess";
			}

			FareRedistributionVM fareRedistributionVM = new FareRedistributionVM();
			
			FareRedistribution fareRedistribution = new FareRedistribution();
			fareRedistributionVM.FareRedistribution = fareRedistribution;

			//GDS
			fareRedistributionVM.GDSs = new SelectList(gdsRepository.GetAllGDSs().ToList(), "GDSCode", "GDSName");

			return View(fareRedistributionVM);
		}

		// POST: /Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(FareRedistributionVM fareRedistributionVM)
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
				TryUpdateModel<FareRedistributionVM>(fareRedistributionVM, "GDSFareRedistributionVM");
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
				fareRedistributionRepository.Add(fareRedistributionVM);
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

			FareRedistribution fareRedistribution = fareRedistributionRepository.GetFareRedistribution(id);

			//Check Exists
			if (fareRedistribution == null)
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

			FareRedistributionVM fareRedistributionVM = new FareRedistributionVM();
			fareRedistributionVM.FareRedistribution = fareRedistribution;

			//GDS
			fareRedistributionVM.GDSs = new SelectList(gdsRepository.GetAllGDSs().ToList(), "GDSCode", "GDSName", fareRedistribution.GDSCode);

			return View(fareRedistributionVM);
		}

		// POST: /Edit
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(FareRedistributionVM fareRedistributionVM)
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
				TryUpdateModel<FareRedistribution>(fareRedistributionVM.FareRedistribution, "FareRedistribution");
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
				fareRedistributionRepository.Update(fareRedistributionVM);
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
			FareRedistribution fareRedistribution = new FareRedistribution();
			fareRedistribution = fareRedistributionRepository.GetFareRedistribution(id);

			//Check Exists
			if (fareRedistribution == null)
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

			FareRedistributionVM fareRedistributionVM = new FareRedistributionVM();
			fareRedistributionVM.AllowDelete = true;

			//Attached Items
			List<FareRedistributionReference> fareRedistributionReferences = fareRedistributionRepository.GetFareRedistributionReferences(fareRedistribution.FareRedistributionId);
			if (fareRedistributionReferences.Count > 0)
			{
				fareRedistributionVM.AllowDelete = false;
				fareRedistributionVM.FareRedistributionReferences = fareRedistributionReferences;
			}

			fareRedistributionVM.FareRedistribution = fareRedistribution;

			return View(fareRedistributionVM);
		}

		// POST: /Delete
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(FareRedistributionVM fareRedistributionVM, FormCollection collection)
		{
			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Get Item From Database
			FareRedistribution fareRedistribution = new FareRedistribution();
			fareRedistribution = fareRedistributionRepository.GetFareRedistribution(fareRedistributionVM.FareRedistribution.FareRedistributionId);

			//Check Exists
			if (fareRedistribution == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Delete Item
			try
			{
				fareRedistributionRepository.Delete(fareRedistributionVM);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/FareRedistribution.mvc/Delete/" + fareRedistribution.FareRedistributionId;
					return View("VersionError");
				}
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			return RedirectToAction("List");
		}

		//Get GetFareRedistributions By GDSCode
		[HttpPost]
		public JsonResult GetFareRedistributionsByGDSCode(string gdsCode)
		{
			if (gdsCode == "")
			{
				return Json(false);
			}
			var result = fareRedistributionRepository.GetFareRedistributionsByGDSCode(gdsCode);
			return Json(result);
		}
	}
}