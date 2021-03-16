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
    public class PolicyLocationController : Controller
    {
        //main repositories
        PolicyLocationRepository policyLocationRepository = new PolicyLocationRepository();

		HierarchyRepository hierarchyRepository = new HierarchyRepository();
		RolesRepository rolesRepository = new RolesRepository();

		private string groupName = "Policy Location Administrator";

        // GET: /List/
        public ActionResult List(string filter, int? page, string sortField, int? sortOrder)
        {
            //Set Access Rights
			ViewData["Access"] = "";
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Access"] = "WriteAccess";
			}

			if (string.IsNullOrEmpty(sortField))
			{
				sortField = "PolicyLocationName";
			}

            ViewData["CurrentSortField"] = sortField;

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

			if (policyLocationRepository == null)
			{
				ViewData["ActionMethod"] = "ListGet";
				return View("Error");
			}

			var cwtPaginatedList = policyLocationRepository.PagePolicyLocations(page ?? 1, filter ?? "", sortField, sortOrder ?? 0);

			if (cwtPaginatedList == null)
			{
				ViewData["ActionMethod"] = "ListGet";
				return View("Error");
			} 
						
			//return items
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
			
			TravelPortTypeRepository travelPortTypeRepository = new TravelPortTypeRepository();
            SelectList travelPortTypes = new SelectList(travelPortTypeRepository.GetAllTravelPortTypes().ToList(), "TravelPortTypeId", "TravelPortTypeDescription");
            ViewData["TravelPortTypeList"] = travelPortTypes;

            PolicyLocation policyLocation = new PolicyLocation();
            return View(policyLocation);
        }

        
        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PolicyLocation policyLocation)
        {
		    //Update  Model from Form
            try
            {
                policyLocationRepository.EditPolicyLocationLocation(policyLocation);
                UpdateModel(policyLocation);
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
                policyLocationRepository.Add(policyLocation);
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
			
			PolicyLocation policyLocation = new PolicyLocation();
            policyLocation = policyLocationRepository.GetPolicyLocation(id);
            if (policyLocation == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            TravelPortTypeRepository travelPortTypeRepository = new TravelPortTypeRepository();
            SelectList travelPortTypes = new SelectList(travelPortTypeRepository.GetAllTravelPortTypes().ToList(), "TravelPortTypeId", "TravelPortTypeDescription");
            ViewData["TravelPortTypeList"] = travelPortTypes;

            policyLocationRepository.EditForDisplay(policyLocation);
            return View(policyLocation);
        }

        // POST: /Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection collection)
        {
			//Get Item From Database
            PolicyLocation policyLocation = new PolicyLocation();
            policyLocation = policyLocationRepository.GetPolicyLocation(id);

            //Check Exists
            if (policyLocation == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //Update Item from Form
            try
            {
                UpdateModel(policyLocation);
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
				policyLocationRepository.EditPolicyLocationLocation(policyLocation);
				policyLocationRepository.Update(policyLocation);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/PolicyLocation.mvc/Edit/" + policyLocation.PolicyLocationId;
                    return View("VersionError");
                }

                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
      
            }

            //Success
            return RedirectToAction("List");

        }

        // GET: /Edit
        public ActionResult View(int id)
        {
            PolicyLocation policyLocation = new PolicyLocation();
            policyLocation = policyLocationRepository.GetPolicyLocation(id);
            if (policyLocation == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            TravelPortTypeRepository travelPortTypeRepository = new TravelPortTypeRepository();
            SelectList travelPortTypes = new SelectList(travelPortTypeRepository.GetAllTravelPortTypes().ToList(), "TravelPortTypeId", "TravelPortTypeDescription");
            ViewData["TravelPortTypeList"] = travelPortTypes;

            policyLocationRepository.EditForDisplay(policyLocation);
            return View(policyLocation);
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
			
			PolicyLocation policyLocation = new PolicyLocation();
            policyLocation = policyLocationRepository.GetPolicyLocation(id);
            if (policyLocation == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            TravelPortTypeRepository travelPortTypeRepository = new TravelPortTypeRepository();
            SelectList travelPortTypes = new SelectList(travelPortTypeRepository.GetAllTravelPortTypes().ToList(), "TravelPortTypeId", "TravelPortTypeDescription");
            ViewData["TravelPortTypeList"] = travelPortTypes;

            policyLocationRepository.EditForDisplay(policyLocation);
            return View(policyLocation);
        }

        // POST: /Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            //Get Item From Database
            PolicyLocation policyLocation = new PolicyLocation();
            policyLocation = policyLocationRepository.GetPolicyLocation(id);
            if (policyLocation == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Delete Item
            try
            {
                policyLocation.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                policyLocationRepository.Delete(policyLocation);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/PolicyLocation.mvc/Delete/" + id.ToString();
                    return View("VersionError");
                }

                //Generic Error
				ViewData["Message"] = "The policy location cannot be deleted while it is in use by a policy item.";
                return View("Error");
            }

            return RedirectToAction("List");
        }


        //Autocomplete
        [HttpPost]
        public JsonResult AutoCompleteLocation(string searchText, int maxResults)
        {
            var result = policyLocationRepository.LookUpPolicyLocationLocations(searchText, maxResults);
            return Json(result);
        }

        //Autocomplete
        [HttpPost]
        public JsonResult AutoCompleteTravelPortName(string searchText, int typeId, int maxResults)
        {
            TravelPortRepository travelPortRepository = new TravelPortRepository();
            var result = travelPortRepository.LookUpTravelPortNames(typeId, searchText, maxResults);
            return Json(result);
        }

        // POST: Form Validation of Location
        [HttpPost]
        public JsonResult IsValidLocation(string locationName)
        {
            LocationRepository locationRepository = new LocationRepository();
            var result = policyLocationRepository.GetPolicyLocationLocationByName(locationName);
            return Json(result);
        }
		
		// POST: Form Validation of Location
        [HttpPost]
		public JsonResult IsAvailablePolicyLocationCode(string locationCode, string locationType, int policyLocationID = 0)
        {
			PolicyLocationRepository policyLocationRepository = new PolicyLocationRepository();
			var result = policyLocationRepository.GetPolicyLocationByLocationCode(locationCode, locationType, policyLocationID);
			string isAvailable = result.Count == 0 ? "true" : "false";
			return Json(isAvailable);
        }
    }
}
