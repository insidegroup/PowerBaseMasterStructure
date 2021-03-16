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
    public class ExternalNameController : Controller
    {
        ExternalNameRepository externalNameRepository = new ExternalNameRepository();
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
                sortField = "ExternalName";
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
            ExternalNamesVM externalNamesVM = new ExternalNamesVM();

            var getExternalNames = externalNameRepository.GetExternalNames(sortField, sortOrder ?? 0, page ?? 1);
            if (getExternalNames != null)
            {
                externalNamesVM.ExternalNames = getExternalNames;
            }

            return View(externalNamesVM);
        }

        public ActionResult Create()
        {
            //Set Access Rights
            ViewData["Access"] = "";
            if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Access"] = "WriteAccess";
            }
           
            ExternalNameVM externalNameVM = new ExternalNameVM();
			ExternalName externalName = new ExternalName();
			externalNameVM.ExternalName = externalName;

            return View(externalNameVM);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ExternalNameVM externalNameVM)
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
                TryUpdateModel<ExternalNameVM>(externalNameVM, "GDSExternalNameVM");
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
                externalNameRepository.Add(externalNameVM);
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
			
			ExternalName externalName = externalNameRepository.GetExternalName(id);

            //Check Exists
			if (externalName == null)
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
			
			ExternalNameVM externalNameVM = new ExternalNameVM();
			externalNameVM.ExternalName = externalName;

			return View(externalNameVM);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ExternalNameVM externalNameVM)
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
                TryUpdateModel<ExternalName>(externalNameVM.ExternalName, "ExternalName");
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
                externalNameRepository.Update(externalNameVM);
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
			ExternalName externalName = new ExternalName();
			externalName = externalNameRepository.GetExternalName(id);

			//Check Exists
			if (externalName == null)
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

			ExternalNameVM externalNameVM = new ExternalNameVM();
			externalNameVM.AllowDelete = true;

			//Attached Items
			List<ExternalNameReference> externalNameReferences = externalNameRepository.GetExternalNameReferences(externalName.ExternalNameId);
			if (externalNameReferences.Count > 0)
			{
				externalNameVM.AllowDelete = false;
				externalNameVM.ExternalNameReferences = externalNameReferences;
			}

			externalNameVM.ExternalName = externalName;

			return View(externalNameVM);
		}

		// POST: /Delete
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(ExternalNameVM externalNameVM, FormCollection collection)
		{
			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			} 
			
			//Get Item From Database
			ExternalName externalName = new ExternalName();
			externalName = externalNameRepository.GetExternalName(externalNameVM.ExternalName.ExternalNameId);

			//Check Exists
			if (externalName == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			} 
			
			//Delete Item
			try
			{
				externalNameRepository.Delete(externalNameVM);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/ExternalName.mvc/Delete/" + externalName.ExternalNameId;
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