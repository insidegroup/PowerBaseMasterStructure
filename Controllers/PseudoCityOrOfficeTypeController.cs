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
    public class PseudoCityOrOfficeTypeController : Controller
    {
        PseudoCityOrOfficeTypeRepository pseudoCityOrOfficeLocationTypeRepository = new PseudoCityOrOfficeTypeRepository();
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
				sortField = "PseudoCityOrOfficeTypeName";
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
            PseudoCityOrOfficeTypesVM pseudoCityOrOfficeLocationTypesVM = new PseudoCityOrOfficeTypesVM();

            var getPseudoCityOrOfficeTypes = pseudoCityOrOfficeLocationTypeRepository.GetPseudoCityOrOfficeTypes(sortField, sortOrder ?? 0, page ?? 1);
            if (getPseudoCityOrOfficeTypes != null)
            {
                pseudoCityOrOfficeLocationTypesVM.PseudoCityOrOfficeTypes = getPseudoCityOrOfficeTypes;
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
           
            PseudoCityOrOfficeTypeVM pseudoCityOrOfficeLocationTypeVM = new PseudoCityOrOfficeTypeVM();
			PseudoCityOrOfficeType pseudoCityOrOfficeLocationType = new PseudoCityOrOfficeType();
			pseudoCityOrOfficeLocationTypeVM.PseudoCityOrOfficeType = pseudoCityOrOfficeLocationType;

            return View(pseudoCityOrOfficeLocationTypeVM);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PseudoCityOrOfficeTypeVM pseudoCityOrOfficeLocationTypeVM)
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
                TryUpdateModel<PseudoCityOrOfficeTypeVM>(pseudoCityOrOfficeLocationTypeVM, "GDSPseudoCityOrOfficeTypeVM");
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
			
			PseudoCityOrOfficeType pseudoCityOrOfficeLocationType = pseudoCityOrOfficeLocationTypeRepository.GetPseudoCityOrOfficeType(id);

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
			
			PseudoCityOrOfficeTypeVM pseudoCityOrOfficeLocationTypeVM = new PseudoCityOrOfficeTypeVM();
			pseudoCityOrOfficeLocationTypeVM.PseudoCityOrOfficeType = pseudoCityOrOfficeLocationType;

			return View(pseudoCityOrOfficeLocationTypeVM);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PseudoCityOrOfficeTypeVM pseudoCityOrOfficeLocationTypeVM)
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
                TryUpdateModel<PseudoCityOrOfficeType>(pseudoCityOrOfficeLocationTypeVM.PseudoCityOrOfficeType, "PseudoCityOrOfficeType");
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
			PseudoCityOrOfficeType pseudoCityOrOfficeLocationType = new PseudoCityOrOfficeType();
			pseudoCityOrOfficeLocationType = pseudoCityOrOfficeLocationTypeRepository.GetPseudoCityOrOfficeType(id);

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

			PseudoCityOrOfficeTypeVM pseudoCityOrOfficeLocationTypeVM = new PseudoCityOrOfficeTypeVM();
			pseudoCityOrOfficeLocationTypeVM.AllowDelete = true;

			//Attached Items
			List<PseudoCityOrOfficeTypeReference> pseudoCityOrOfficeLocationTypeReferences = pseudoCityOrOfficeLocationTypeRepository.GetPseudoCityOrOfficeTypeReferences(pseudoCityOrOfficeLocationType.PseudoCityOrOfficeTypeId);
			if (pseudoCityOrOfficeLocationTypeReferences.Count > 0)
			{
				pseudoCityOrOfficeLocationTypeVM.AllowDelete = false;
				pseudoCityOrOfficeLocationTypeVM.PseudoCityOrOfficeTypeReferences = pseudoCityOrOfficeLocationTypeReferences;
			}

			pseudoCityOrOfficeLocationTypeVM.PseudoCityOrOfficeType = pseudoCityOrOfficeLocationType;

			return View(pseudoCityOrOfficeLocationTypeVM);
		}

		// POST: /Delete
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(PseudoCityOrOfficeTypeVM pseudoCityOrOfficeLocationTypeVM, FormCollection collection)
		{
			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			} 
			
			//Get Item From Database
			PseudoCityOrOfficeType pseudoCityOrOfficeLocationType = new PseudoCityOrOfficeType();
			pseudoCityOrOfficeLocationType = pseudoCityOrOfficeLocationTypeRepository.GetPseudoCityOrOfficeType(pseudoCityOrOfficeLocationTypeVM.PseudoCityOrOfficeType.PseudoCityOrOfficeTypeId);

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
					ViewData["ReturnURL"] = "/PseudoCityOrOfficeType.mvc/Delete/" + pseudoCityOrOfficeLocationType.PseudoCityOrOfficeTypeId;
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