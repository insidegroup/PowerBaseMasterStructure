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
    public class PseudoCityOrOfficeAddressController : Controller
    {
        PseudoCityOrOfficeAddressRepository pseudoCityOrOfficeAddressRepository = new PseudoCityOrOfficeAddressRepository();
        HierarchyRepository hierarchyRepository = new HierarchyRepository();
		CountryRepository countryRepository = new CountryRepository();
		StateProvinceRepository stateProvinceRepository = new StateProvinceRepository();

		private string groupName = "GDS Administrator";

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
            PseudoCityOrOfficeAddressesVM pseudoCityOrOfficeAddresssVM = new PseudoCityOrOfficeAddressesVM();

			var getPseudoCityOrOfficeAddresses = pseudoCityOrOfficeAddressRepository.GetPseudoCityOrOfficeAddresses(sortField, sortOrder ?? 0, filter ?? "", page ?? 1);
            if (getPseudoCityOrOfficeAddresses != null)
            {
                pseudoCityOrOfficeAddresssVM.PseudoCityOrOfficeAddresses = getPseudoCityOrOfficeAddresses;
            }

            return View(pseudoCityOrOfficeAddresssVM);
        }

        public ActionResult Create()
        {
            //Set Access Rights
            ViewData["Access"] = "";
            if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Access"] = "WriteAccess";
            }
           
            PseudoCityOrOfficeAddressVM pseudoCityOrOfficeAddressVM = new PseudoCityOrOfficeAddressVM();
			PseudoCityOrOfficeAddress pseudoCityOrOfficeAddress = new PseudoCityOrOfficeAddress();
			pseudoCityOrOfficeAddressVM.PseudoCityOrOfficeAddress = pseudoCityOrOfficeAddress;

			//Countries
			pseudoCityOrOfficeAddressVM.Countries = new SelectList(countryRepository.GetAllCountries().ToList(), "CountryCode", "CountryName");

			//StateProvince SelectList
			pseudoCityOrOfficeAddressVM.StateProvinces = new SelectList(
				stateProvinceRepository.GetStateProvincesByCountryCode(
					pseudoCityOrOfficeAddress.CountryCode).ToList(), 
					"StateProvinceCode", 
					"Name", 
					pseudoCityOrOfficeAddress.StateProvinceCode
				);

            return View(pseudoCityOrOfficeAddressVM);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PseudoCityOrOfficeAddressVM pseudoCityOrOfficeAddressVM)
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
                TryUpdateModel<PseudoCityOrOfficeAddressVM>(pseudoCityOrOfficeAddressVM, "GDSPseudoCityOrOfficeAddressVM");
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
                pseudoCityOrOfficeAddressRepository.Add(pseudoCityOrOfficeAddressVM);
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
			
			PseudoCityOrOfficeAddress pseudoCityOrOfficeAddress = pseudoCityOrOfficeAddressRepository.GetPseudoCityOrOfficeAddress(id);

            //Check Exists
			if (pseudoCityOrOfficeAddress == null)
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
			
			PseudoCityOrOfficeAddressVM pseudoCityOrOfficeAddressVM = new PseudoCityOrOfficeAddressVM();
			pseudoCityOrOfficeAddressVM.PseudoCityOrOfficeAddress = pseudoCityOrOfficeAddress;

			//Countries
			pseudoCityOrOfficeAddressVM.Countries = new SelectList(countryRepository.GetAllCountries().ToList(), "CountryCode", "CountryName", pseudoCityOrOfficeAddress.CountryCode);

			//StateProvince SelectList
			pseudoCityOrOfficeAddressVM.StateProvinces = new SelectList(
				stateProvinceRepository.GetStateProvincesByCountryCode(
					pseudoCityOrOfficeAddress.CountryCode).ToList(),
					"StateProvinceCode",
					"Name",
					pseudoCityOrOfficeAddress.StateProvinceCode
				);

			return View(pseudoCityOrOfficeAddressVM);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PseudoCityOrOfficeAddressVM pseudoCityOrOfficeAddressVM)
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
                TryUpdateModel<PseudoCityOrOfficeAddress>(pseudoCityOrOfficeAddressVM.PseudoCityOrOfficeAddress, "PseudoCityOrOfficeAddress");
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
                pseudoCityOrOfficeAddressRepository.Update(pseudoCityOrOfficeAddressVM);
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
			PseudoCityOrOfficeAddress pseudoCityOrOfficeAddress = new PseudoCityOrOfficeAddress();
			pseudoCityOrOfficeAddress = pseudoCityOrOfficeAddressRepository.GetPseudoCityOrOfficeAddress(id);

			//Check Exists
			if (pseudoCityOrOfficeAddress == null)
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

			PseudoCityOrOfficeAddressVM pseudoCityOrOfficeAddressVM = new PseudoCityOrOfficeAddressVM();
			pseudoCityOrOfficeAddressVM.AllowDelete = true;

			StateProvinceRepository stateProvinceRepository = new StateProvinceRepository();
			StateProvince stateProvince = stateProvinceRepository.GetStateProvinceByCountry(pseudoCityOrOfficeAddress.CountryCode, pseudoCityOrOfficeAddress.StateProvinceCode);
			pseudoCityOrOfficeAddress.StateProvinceName = "";
			if (stateProvince != null)
			{
				pseudoCityOrOfficeAddress.StateProvinceName = stateProvince.Name;
			}
			
			//Attached Items
			List<PseudoCityOrOfficeAddressReference> pseudoCityOrOfficeAddressReferences = pseudoCityOrOfficeAddressRepository.GetPseudoCityOrOfficeAddressReferences(pseudoCityOrOfficeAddress.PseudoCityOrOfficeAddressId);
			if (pseudoCityOrOfficeAddressReferences.Count > 0)
			{
				pseudoCityOrOfficeAddressVM.AllowDelete = false;
				pseudoCityOrOfficeAddressVM.PseudoCityOrOfficeAddressReferences = pseudoCityOrOfficeAddressReferences;
			}

			pseudoCityOrOfficeAddressVM.PseudoCityOrOfficeAddress = pseudoCityOrOfficeAddress;

			return View(pseudoCityOrOfficeAddressVM);
		}

		// POST: /Delete
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(PseudoCityOrOfficeAddressVM pseudoCityOrOfficeAddressVM, FormCollection collection)
		{
			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			} 
			
			//Get Item From Database
			PseudoCityOrOfficeAddress pseudoCityOrOfficeAddress = new PseudoCityOrOfficeAddress();
			pseudoCityOrOfficeAddress = pseudoCityOrOfficeAddressRepository.GetPseudoCityOrOfficeAddress(pseudoCityOrOfficeAddressVM.PseudoCityOrOfficeAddress.PseudoCityOrOfficeAddressId);

			//Check Exists
			if (pseudoCityOrOfficeAddress == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			} 
			
			//Delete Item
			try
			{
				pseudoCityOrOfficeAddressRepository.Delete(pseudoCityOrOfficeAddressVM);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/PseudoCityOrOfficeAddress.mvc/Delete/" + pseudoCityOrOfficeAddress.PseudoCityOrOfficeAddressId;
					return View("VersionError");
				}
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			return RedirectToAction("List");
		}

		//Get Address
		[HttpPost]
		public JsonResult GetPseudoCityOrOfficeAddress(string addressId)
		{
			int pseudoCityOrOfficeAddress = 0;
			if (!Int32.TryParse(addressId, out pseudoCityOrOfficeAddress))
			{
				return Json(false);
			}
			var result = pseudoCityOrOfficeAddressRepository.GetPseudoCityOrOfficeAddressCountryGlobalRegion(pseudoCityOrOfficeAddress);
			return Json(result);
		}
    }
}