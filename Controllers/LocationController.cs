using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Repository;
using System.Data.SqlClient;
using System.Linq.Dynamic;

namespace CWTDesktopDatabase.Controllers
{
    public class LocationController : Controller
    {
        //main repository
        LocationRepository locationRepository = new LocationRepository();
        HierarchyRepository hierarchyRepository = new HierarchyRepository();
        private string groupName = "Location";

        // GET: /List/
        public ActionResult List(string filter, int? page, string sortField, int? sortOrder)
        {

            //Set Access Rights
            ViewData["Access"] = "";
            if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Access"] = "WriteAccess";
            }

          
            //sortfield
            if (sortField != "CountryName" && sortField !="CountryRegionName")
            {
                sortField = "LocationName";
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
            }

            //return items
            var cwtPaginatedList = locationRepository.PageLocations(page ?? 1, filter ?? "", sortField, sortOrder ?? 0);
            return View(cwtPaginatedList);
        }

   
        // GET: /View
        public ActionResult View(int id)
        {
            //Check Exists
            Location location = new Location();
            location = locationRepository.GetLocation(id);
            if (location == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

			AddressLocationVM addressLocationVM = new AddressLocationVM();
			addressLocationVM.Location = location;
			addressLocationVM.Address = new Address();
			
			Address address = locationRepository.GetLocationAddress(id);
			if (address != null)
			{
                AddressRepository addressRepository = new AddressRepository();
                addressRepository.EditForDisplay(address);
                addressLocationVM.Address = address;
            }

            return View(addressLocationVM);

        }

        // GET: /Create
        public ActionResult Create()
        {
            //Check Access Rights to Domain
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ViewData["CountryRegions"] = new SelectListItem();

			AddressLocationVM addressLocationVM = new AddressLocationVM();

			Location location = new Location();
			addressLocationVM.Location = location;

			Address address = new Address();
			addressLocationVM.Address = address;

            //StateProvince SelectList
            StateProvinceRepository stateProvinceRepository = new StateProvinceRepository();
            addressLocationVM.StateProvinces = new SelectList(stateProvinceRepository.GetStateProvincesByCountryCode(address.CountryCode).ToList(), "StateProvinceCode", "Name");

            return View(addressLocationVM);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
		public ActionResult Create(AddressLocationVM addressLocationVM)
        {			
			//Update  Model from Form
            try
            {
				UpdateModel(addressLocationVM);
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
                locationRepository.Add(addressLocationVM);
            }
            catch (SqlException ex)
            {

                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            ViewData["NewSortOrder"] = 0;
            return RedirectToAction("List");
        }

        // GET: /Edit
        public ActionResult Edit(int id)
        {
            //Check Exists
            Location location = new Location();
            location = locationRepository.GetLocation(id);
            if (location == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToLocation(id))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

			AddressLocationVM addressLocationVM = new AddressLocationVM();
			locationRepository.EditForDisplay(location);
			addressLocationVM.Location = location;

			Address address = locationRepository.GetLocationAddress(id);
			if (address != null)
			{
                AddressRepository addressRepository = new AddressRepository();
                addressRepository.EditForDisplay(address);
				addressLocationVM.Address = address;
			}
			
            CountryRepository countryRepository = new CountryRepository();
            SelectList countryRegionList = new SelectList(countryRepository.GetCountryRegions(location.CountryRegion.Country.CountryCode).ToList(), "CountryRegionId", "CountryRegionName");
            ViewData["CountryRegions"] = countryRegionList;

            //StateProvince SelectList
            StateProvinceRepository stateProvinceRepository = new StateProvinceRepository();
            addressLocationVM.StateProvinces = new SelectList(stateProvinceRepository.GetStateProvincesByCountryCode(location.CountryCode).ToList(), "StateProvinceCode", "Name", address.StateProvinceCode);

            return View(addressLocationVM);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
		public ActionResult Edit(AddressLocationVM addressLocationVM)
        {
            //Get Item From Database
            Location location = new Location();
			location = locationRepository.GetLocation(addressLocationVM.Location.LocationId);

            //Check Exists
            if (location == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToLocation(location.LocationId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update Item from Form
            try
            {
				UpdateModel(addressLocationVM);
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
				locationRepository.Update(addressLocationVM);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/Location.mvc/Edit/" + location.LocationId.ToString();
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

        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id)
        {
            //Check Exists
            Location location = new Location();
            location = locationRepository.GetLocation(id);
            if (location == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToLocation(id))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            locationRepository.EditForDisplay(location);

            LocationDeleteVM locationDeleteVM = new LocationDeleteVM();
			locationDeleteVM.Address = new Address();

			Address address = locationRepository.GetLocationAddress(id);
			if (address != null)
			{
                AddressRepository addressRepository = new AddressRepository();
                addressRepository.EditForDisplay(address);
                locationDeleteVM.Address = address;
			}
			
			LocationWizardRepository locationWizardRepository = new LocationWizardRepository();
            locationDeleteVM.Location = location;
            locationDeleteVM.Teams = locationWizardRepository.GetLocationTeams(id);
            locationDeleteVM.SystemUsers = locationWizardRepository.GetLocationSystemUsers(id);
            
            LocationLinkedItemsVM locationLinkedItemsVM = new LocationLinkedItemsVM();
            locationLinkedItemsVM.Location = location;
            locationWizardRepository.AddLinkedItems(locationLinkedItemsVM);
            locationDeleteVM.LinkedItems = locationLinkedItemsVM;

            return View(locationDeleteVM);
        }

        // GET: /ConfirmDelete
        public ActionResult ConfirmDelete(int id)
        {
            //Check Exists
            Location location = new Location();
            location = locationRepository.GetLocation(id);
            if (location == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToLocation(id))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            locationRepository.EditForDisplay(location);

            LocationWizardRepository locationWizardRepository = new LocationWizardRepository();
            LocationLinkedItemsVM locationLinkedItemsViewModel = new LocationLinkedItemsVM();
            locationLinkedItemsViewModel.Location = location;
           
            locationWizardRepository.AddLinkedItems(locationLinkedItemsViewModel);
            return View(locationLinkedItemsViewModel);
        }
        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmDelete(int id, FormCollection collection)
        {
            //Check Exists
            Location location = new Location();
            location = locationRepository.GetLocation(id);
            if (location == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }


            //Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToLocation(id))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Delete Item
            try
            {
                location.VersionNumber = Int32.Parse(collection["Location.VersionNumber"]);
                locationRepository.Delete(location);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/Location.mvc/Delete/" + location.LocationId.ToString();
                    return View("VersionError");
                }

                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List");
        }

        //Validation
        [HttpPost]
        public JsonResult IsAvailableLocation(string locationName, int countryRegionId, int? locationId)
        {

            var result = locationRepository.IsAvailableLocation(locationName, countryRegionId, locationId);
            return Json(result);
        }
    }
}
