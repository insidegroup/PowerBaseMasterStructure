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
    public class PseudoCityOrOfficeLocationTypeController : Controller
    {
        PseudoCityOrOfficeLocationTypeRepository pseudoCityOrOfficeLocationTypeRepository = new PseudoCityOrOfficeLocationTypeRepository();
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
				sortField = "PseudoCityOrOfficeLocationTypeName";
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
            PseudoCityOrOfficeLocationTypesVM pseudoCityOrOfficeLocationTypesVM = new PseudoCityOrOfficeLocationTypesVM();

            var getPseudoCityOrOfficeLocationTypes = pseudoCityOrOfficeLocationTypeRepository.GetPseudoCityOrOfficeLocationTypes(sortField, sortOrder ?? 0, page ?? 1);
            if (getPseudoCityOrOfficeLocationTypes != null)
            {
                pseudoCityOrOfficeLocationTypesVM.PseudoCityOrOfficeLocationTypes = getPseudoCityOrOfficeLocationTypes;
            }

            return View(pseudoCityOrOfficeLocationTypesVM);
        }

        public ActionResult Create()
        {
            //Set Access Rights
            ViewData["Access"] = "";
            if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Access"] = "WriteAccess";
            }
           
            PseudoCityOrOfficeLocationTypeVM pseudoCityOrOfficeLocationTypeVM = new PseudoCityOrOfficeLocationTypeVM();
			PseudoCityOrOfficeLocationType pseudoCityOrOfficeLocationType = new PseudoCityOrOfficeLocationType();
			pseudoCityOrOfficeLocationTypeVM.PseudoCityOrOfficeLocationType = pseudoCityOrOfficeLocationType;

            return View(pseudoCityOrOfficeLocationTypeVM);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PseudoCityOrOfficeLocationTypeVM pseudoCityOrOfficeLocationTypeVM)
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
                TryUpdateModel<PseudoCityOrOfficeLocationTypeVM>(pseudoCityOrOfficeLocationTypeVM, "GDSPseudoCityOrOfficeLocationTypeVM");
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
                pseudoCityOrOfficeLocationTypeRepository.Add(pseudoCityOrOfficeLocationTypeVM);
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
			
			PseudoCityOrOfficeLocationType pseudoCityOrOfficeLocationType = pseudoCityOrOfficeLocationTypeRepository.GetPseudoCityOrOfficeLocationType(id);

            //Check Exists
			if (pseudoCityOrOfficeLocationType == null)
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
			
			PseudoCityOrOfficeLocationTypeVM pseudoCityOrOfficeLocationTypeVM = new PseudoCityOrOfficeLocationTypeVM();
			pseudoCityOrOfficeLocationTypeVM.PseudoCityOrOfficeLocationType = pseudoCityOrOfficeLocationType;

			return View(pseudoCityOrOfficeLocationTypeVM);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PseudoCityOrOfficeLocationTypeVM pseudoCityOrOfficeLocationTypeVM)
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
                TryUpdateModel<PseudoCityOrOfficeLocationType>(pseudoCityOrOfficeLocationTypeVM.PseudoCityOrOfficeLocationType, "PseudoCityOrOfficeLocationType");
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
                pseudoCityOrOfficeLocationTypeRepository.Update(pseudoCityOrOfficeLocationTypeVM);
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
			PseudoCityOrOfficeLocationType pseudoCityOrOfficeLocationType = new PseudoCityOrOfficeLocationType();
			pseudoCityOrOfficeLocationType = pseudoCityOrOfficeLocationTypeRepository.GetPseudoCityOrOfficeLocationType(id);

			//Check Exists
			if (pseudoCityOrOfficeLocationType == null)
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

			PseudoCityOrOfficeLocationTypeVM pseudoCityOrOfficeLocationTypeVM = new PseudoCityOrOfficeLocationTypeVM();
			pseudoCityOrOfficeLocationTypeVM.AllowDelete = true;

			//Attached Items
			List<PseudoCityOrOfficeLocationTypeReference> pseudoCityOrOfficeLocationTypeReferences = pseudoCityOrOfficeLocationTypeRepository.GetPseudoCityOrOfficeLocationTypeReferences(pseudoCityOrOfficeLocationType.PseudoCityOrOfficeLocationTypeId);
			if (pseudoCityOrOfficeLocationTypeReferences.Count > 0)
			{
				pseudoCityOrOfficeLocationTypeVM.AllowDelete = false;
				pseudoCityOrOfficeLocationTypeVM.PseudoCityOrOfficeLocationTypeReferences = pseudoCityOrOfficeLocationTypeReferences;
			}

			pseudoCityOrOfficeLocationTypeVM.PseudoCityOrOfficeLocationType = pseudoCityOrOfficeLocationType;

			return View(pseudoCityOrOfficeLocationTypeVM);
		}

		// POST: /Delete
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(PseudoCityOrOfficeLocationTypeVM pseudoCityOrOfficeLocationTypeVM, FormCollection collection)
		{
			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			} 
			
			//Get Item From Database
			PseudoCityOrOfficeLocationType pseudoCityOrOfficeLocationType = new PseudoCityOrOfficeLocationType();
			pseudoCityOrOfficeLocationType = pseudoCityOrOfficeLocationTypeRepository.GetPseudoCityOrOfficeLocationType(pseudoCityOrOfficeLocationTypeVM.PseudoCityOrOfficeLocationType.PseudoCityOrOfficeLocationTypeId);

			//Check Exists
			if (pseudoCityOrOfficeLocationType == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			} 
			
			//Delete Item
			try
			{
				pseudoCityOrOfficeLocationTypeRepository.Delete(pseudoCityOrOfficeLocationTypeVM);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/PseudoCityOrOfficeLocationType.mvc/Delete/" + pseudoCityOrOfficeLocationType.PseudoCityOrOfficeLocationTypeId;
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