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
    public class GDSThirdPartyVendorController : Controller
    {
        GDSThirdPartyVendorRepository thirdPartyVendorRepository = new GDSThirdPartyVendorRepository();
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
                sortField = "GDSThirdPartyVendorName";
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
            GDSThirdPartyVendorsVM thirdPartyVendorsVM = new GDSThirdPartyVendorsVM();

            var getGDSThirdPartyVendors = thirdPartyVendorRepository.GetGDSThirdPartyVendors(sortField, sortOrder ?? 0, page ?? 1);
            if (getGDSThirdPartyVendors != null)
            {
                thirdPartyVendorsVM.GDSThirdPartyVendors = getGDSThirdPartyVendors;
            }

            return View(thirdPartyVendorsVM);
        }

        public ActionResult Create()
        {
            //Set Access Rights
            ViewData["Access"] = "";
            if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Access"] = "WriteAccess";
            }
           
            GDSThirdPartyVendorVM thirdPartyVendorVM = new GDSThirdPartyVendorVM();
			GDSThirdPartyVendor thirdPartyVendor = new GDSThirdPartyVendor();
			thirdPartyVendorVM.GDSThirdPartyVendor = thirdPartyVendor;

            return View(thirdPartyVendorVM);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GDSThirdPartyVendorVM thirdPartyVendorVM)
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
                TryUpdateModel<GDSThirdPartyVendorVM>(thirdPartyVendorVM, "GDSThirdPartyVendorVM");
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
                thirdPartyVendorRepository.Add(thirdPartyVendorVM);
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
			
			GDSThirdPartyVendor thirdPartyVendor = thirdPartyVendorRepository.GetGDSThirdPartyVendor(id);

            //Check Exists
			if (thirdPartyVendor == null)
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
			
			GDSThirdPartyVendorVM thirdPartyVendorVM = new GDSThirdPartyVendorVM();
			thirdPartyVendorVM.GDSThirdPartyVendor = thirdPartyVendor;

			return View(thirdPartyVendorVM);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GDSThirdPartyVendorVM thirdPartyVendorVM)
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
                TryUpdateModel<GDSThirdPartyVendor>(thirdPartyVendorVM.GDSThirdPartyVendor, "GDSThirdPartyVendor");
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
                thirdPartyVendorRepository.Update(thirdPartyVendorVM);
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
			GDSThirdPartyVendor thirdPartyVendor = new GDSThirdPartyVendor();
			thirdPartyVendor = thirdPartyVendorRepository.GetGDSThirdPartyVendor(id);

			//Check Exists
			if (thirdPartyVendor == null)
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

			GDSThirdPartyVendorVM thirdPartyVendorVM = new GDSThirdPartyVendorVM();
			thirdPartyVendorVM.AllowDelete = true;

			//Attached Items
			List<GDSThirdPartyVendorReference> thirdPartyVendorReferences = thirdPartyVendorRepository.GetGDSThirdPartyVendorReferences(thirdPartyVendor.GDSThirdPartyVendorId);
			if (thirdPartyVendorReferences.Count > 0)
			{
				thirdPartyVendorVM.AllowDelete = false;
				thirdPartyVendorVM.GDSThirdPartyVendorReferences = thirdPartyVendorReferences;
			}

			thirdPartyVendorVM.GDSThirdPartyVendor = thirdPartyVendor;

			return View(thirdPartyVendorVM);
		}

		// POST: /Delete
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(GDSThirdPartyVendorVM thirdPartyVendorVM, FormCollection collection)
		{
			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			} 
			
			//Get Item From Database
			GDSThirdPartyVendor thirdPartyVendor = new GDSThirdPartyVendor();
			thirdPartyVendor = thirdPartyVendorRepository.GetGDSThirdPartyVendor(thirdPartyVendorVM.GDSThirdPartyVendor.GDSThirdPartyVendorId);

			//Check Exists
			if (thirdPartyVendor == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			} 
			
			//Delete Item
			try
			{
				thirdPartyVendorRepository.Delete(thirdPartyVendorVM);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/GDSThirdPartyVendor.mvc/Delete/" + thirdPartyVendor.GDSThirdPartyVendorId;
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