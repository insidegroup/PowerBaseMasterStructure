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
    public class PseudoCityOrOfficeDefinedRegionController : Controller
    {
        PseudoCityOrOfficeDefinedRegionRepository pseudoCityOrOfficeDefinedRegionRepository = new PseudoCityOrOfficeDefinedRegionRepository();
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
				sortField = "GlobalRegionName";
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
            PseudoCityOrOfficeDefinedRegionsVM pseudoCityOrOfficeDefinedRegionsVM = new PseudoCityOrOfficeDefinedRegionsVM();

            var getPseudoCityOrOfficeDefinedRegions = pseudoCityOrOfficeDefinedRegionRepository.GetPseudoCityOrOfficeDefinedRegions(sortField, sortOrder ?? 0, page ?? 1);
            if (getPseudoCityOrOfficeDefinedRegions != null)
            {
                pseudoCityOrOfficeDefinedRegionsVM.PseudoCityOrOfficeDefinedRegions = getPseudoCityOrOfficeDefinedRegions;
            }

            return View(pseudoCityOrOfficeDefinedRegionsVM);
        }

        public ActionResult Create()
        {
            //Set Access Rights
            ViewData["Access"] = "";
            if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Access"] = "WriteAccess";
            }
           
            PseudoCityOrOfficeDefinedRegionVM pseudoCityOrOfficeDefinedRegionVM = new PseudoCityOrOfficeDefinedRegionVM();
			PseudoCityOrOfficeDefinedRegion pseudoCityOrOfficeDefinedRegion = new PseudoCityOrOfficeDefinedRegion();
			pseudoCityOrOfficeDefinedRegionVM.PseudoCityOrOfficeDefinedRegion = pseudoCityOrOfficeDefinedRegion;

			//GlobalRegions
			pseudoCityOrOfficeDefinedRegionVM.GlobalRegions = new SelectList(hierarchyRepository.GetAllGlobalRegions().ToList(), "GlobalRegionCode", "GlobalRegionName");

            return View(pseudoCityOrOfficeDefinedRegionVM);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PseudoCityOrOfficeDefinedRegionVM pseudoCityOrOfficeDefinedRegionVM)
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
                TryUpdateModel<PseudoCityOrOfficeDefinedRegionVM>(pseudoCityOrOfficeDefinedRegionVM, "GDSPseudoCityOrOfficeDefinedRegionVM");
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
                pseudoCityOrOfficeDefinedRegionRepository.Add(pseudoCityOrOfficeDefinedRegionVM);
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
			
			PseudoCityOrOfficeDefinedRegion pseudoCityOrOfficeDefinedRegion = pseudoCityOrOfficeDefinedRegionRepository.GetPseudoCityOrOfficeDefinedRegion(id);

            //Check Exists
			if (pseudoCityOrOfficeDefinedRegion == null)
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
			
			PseudoCityOrOfficeDefinedRegionVM pseudoCityOrOfficeDefinedRegionVM = new PseudoCityOrOfficeDefinedRegionVM();
			pseudoCityOrOfficeDefinedRegionVM.PseudoCityOrOfficeDefinedRegion = pseudoCityOrOfficeDefinedRegion;

			//GlobalRegions
			pseudoCityOrOfficeDefinedRegionVM.GlobalRegions = new SelectList(hierarchyRepository.GetAllGlobalRegions().ToList(), "GlobalRegionCode", "GlobalRegionName", pseudoCityOrOfficeDefinedRegion.GlobalRegionCode);

			return View(pseudoCityOrOfficeDefinedRegionVM);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PseudoCityOrOfficeDefinedRegionVM pseudoCityOrOfficeDefinedRegionVM)
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
                TryUpdateModel<PseudoCityOrOfficeDefinedRegion>(pseudoCityOrOfficeDefinedRegionVM.PseudoCityOrOfficeDefinedRegion, "PseudoCityOrOfficeDefinedRegion");
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
                pseudoCityOrOfficeDefinedRegionRepository.Update(pseudoCityOrOfficeDefinedRegionVM);
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
			PseudoCityOrOfficeDefinedRegion pseudoCityOrOfficeDefinedRegion = new PseudoCityOrOfficeDefinedRegion();
			pseudoCityOrOfficeDefinedRegion = pseudoCityOrOfficeDefinedRegionRepository.GetPseudoCityOrOfficeDefinedRegion(id);

			//Check Exists
			if (pseudoCityOrOfficeDefinedRegion == null)
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

			PseudoCityOrOfficeDefinedRegionVM pseudoCityOrOfficeDefinedRegionVM = new PseudoCityOrOfficeDefinedRegionVM();
			pseudoCityOrOfficeDefinedRegionVM.AllowDelete = true;

			//Attached Items
			List<PseudoCityOrOfficeDefinedRegionReference> pseudoCityOrOfficeDefinedRegionReferences = pseudoCityOrOfficeDefinedRegionRepository.GetPseudoCityOrOfficeDefinedRegionReferences(pseudoCityOrOfficeDefinedRegion.PseudoCityOrOfficeDefinedRegionId);
			if (pseudoCityOrOfficeDefinedRegionReferences.Count > 0)
			{
				pseudoCityOrOfficeDefinedRegionVM.AllowDelete = false;
				pseudoCityOrOfficeDefinedRegionVM.PseudoCityOrOfficeDefinedRegionReferences = pseudoCityOrOfficeDefinedRegionReferences;
			}

			pseudoCityOrOfficeDefinedRegionVM.PseudoCityOrOfficeDefinedRegion = pseudoCityOrOfficeDefinedRegion;

			return View(pseudoCityOrOfficeDefinedRegionVM);
		}

		// POST: /Delete
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(PseudoCityOrOfficeDefinedRegionVM pseudoCityOrOfficeDefinedRegionVM, FormCollection collection)
		{
			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			} 
			
			//Get Item From Database
			PseudoCityOrOfficeDefinedRegion pseudoCityOrOfficeDefinedRegion = new PseudoCityOrOfficeDefinedRegion();
			pseudoCityOrOfficeDefinedRegion = pseudoCityOrOfficeDefinedRegionRepository.GetPseudoCityOrOfficeDefinedRegion(pseudoCityOrOfficeDefinedRegionVM.PseudoCityOrOfficeDefinedRegion.PseudoCityOrOfficeDefinedRegionId);

			//Check Exists
			if (pseudoCityOrOfficeDefinedRegion == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			} 
			
			//Delete Item
			try
			{
				pseudoCityOrOfficeDefinedRegionRepository.Delete(pseudoCityOrOfficeDefinedRegionVM);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/PseudoCityOrOfficeDefinedRegion.mvc/Delete/" + pseudoCityOrOfficeDefinedRegion.PseudoCityOrOfficeDefinedRegionId;
					return View("VersionError");
				}
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			return RedirectToAction("List");
		}

		// POST:  CountryRegions of a Country for SelectList
		[HttpPost]
		public JsonResult GetPseudoCityOrOfficeDefinedRegions(string globalRegionCode)
		{
			var result = pseudoCityOrOfficeDefinedRegionRepository.GetPseudoCityOrOfficeDefinedRegions(globalRegionCode);
			return Json(result);
		}
    }
}