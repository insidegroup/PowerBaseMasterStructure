using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Repository;
using System.Data.SqlClient;

namespace CWTDesktopDatabase.Controllers
{
    public class TripTypeController : Controller
    {
        //main repository
        TripTypeRepository tripTypeRepository = new TripTypeRepository();

		HierarchyRepository hierarchyRepository = new HierarchyRepository();
		private string groupName = "Trip Types";
		
		// GET: /List
        public ActionResult List(string filter, int? page, string sortField, int? sortOrder)
        {
			//Set Access Rights
			ViewData["Access"] = "";
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Access"] = "WriteAccess";
			} 
			
			ViewData["CurrentSortField"] = sortField;
            if (sortField != "BackOfficeTripTypeCode")
            {
                sortField = "TripTypeDescription";
                ViewData["CurrentSortField"] = "TripTypeDescription";
            }


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

            //return items
            var cwtPaginatedList = tripTypeRepository.PageTripTypes(page ?? 1, filter ?? "", sortField, sortOrder ?? 0);
            return View(cwtPaginatedList);
        }

        // GET: /Create
        public ActionResult Create()
        {
			//Set Access Rights
			ViewData["Access"] = "";
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			} 
			
			TripType tripType = new TripType();
            return View(tripType);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TripType tripType)
        {
            //Update  Model from Form
            try
            {
                UpdateModel(tripType);
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


            //Database Update
            try
            {
                tripTypeRepository.Add(tripType);
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
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			} 
			
			//Check Exists
            TripType tripType = new TripType();
            tripType = tripTypeRepository.GetTripType(id);
            if (tripType == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            return View(tripType);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection collection)
        {
            //Get TripType
            TripType tripType = new TripType();
            tripType = tripTypeRepository.GetTripType(id);

            if (tripType == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Update Item from Form
            try
            {
                UpdateModel(tripType);
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

            //Database Update
            try
            {
                tripType.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                tripTypeRepository.Update(tripType);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/TripType.mvc/Edit/" + id.ToString();
                    return View("VersionError");
                }
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
			//Set Access Rights
			ViewData["Access"] = "";
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}  
			
			//Get TripType
            TripType tripType = new TripType();
            tripType = tripTypeRepository.GetTripType(id);

            if (tripType == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            //Return
            return View(tripType);
        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            //Get TripType
            TripType tripType = new TripType();
            tripType = tripTypeRepository.GetTripType(id);

            if (tripType == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            //Delete Item
            try
            {
                tripType.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                tripTypeRepository.Delete(tripType);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/TripType.mvc/Delete/" + id.ToString();
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            //Return
            return RedirectToAction("List");
        }
    }
}
