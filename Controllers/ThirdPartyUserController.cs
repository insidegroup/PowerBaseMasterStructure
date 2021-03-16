using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Repository;
using System.Data.SqlClient;
using System.Collections;
using CWTDesktopDatabase.ViewModels;
using System.Net;
using System.Text;

namespace CWTDesktopDatabase.Controllers
{
    public class ThirdPartyUserController : Controller
    {
        //main repositories
		HierarchyRepository hierarchyRepository = new HierarchyRepository();
		RolesRepository rolesRepository = new RolesRepository();

		ThirdPartyUserRepository thirdPartyUserRepository = new ThirdPartyUserRepository();
		ThirdPartyUserTypeRepository thirdPartyUserTypeRepository = new ThirdPartyUserTypeRepository();
		GDSThirdPartyVendorRepository gdsThirdPartyVendorRepository = new GDSThirdPartyVendorRepository();
		CountryRepository countryRepository = new CountryRepository();
		StateProvinceRepository stateProvinceRepository = new StateProvinceRepository();
		
		private string thirdPartyGroupName = "GDS Third Party Administrator";
		private string governmentGroupName = "GDS Government Administrator";

		// GET: /List
		public ActionResult ListUnDeleted(
			int? page,
			string sortField,
			int? sortOrder,
			string filterField_1 = "",
			string filterValue_1 = "",
			string filterField_2 = "",
			string filterValue_2 = "",
			string filterField_3 = "",
			string filterValue_3 = "")
		{

			//Check AccessRights
			ViewData["Access"] = "";
			if (hierarchyRepository.AdminHasDomainWriteAccess(thirdPartyGroupName) || hierarchyRepository.AdminHasDomainWriteAccess(governmentGroupName))
			{
				ViewData["Access"] = "WriteAccess";
			}

			if (string.IsNullOrEmpty(sortField))
			{
				sortField = "ThirdPartyName";
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
			if (page < 1)
			{
				page = 1;
			}

			var items = thirdPartyUserRepository.PageThirdPartyUsers(
				page ?? 1,
				sortField,
				sortOrder ?? 0,
				false,
				filterField_1,
				filterValue_1,
				filterField_2,
				filterValue_2,
				filterField_3,
				filterValue_3
			);

			Dictionary<string, string> filters = thirdPartyUserRepository.GetThirdPartyUserFilters();
			
			//Default Filters
			ViewData["FilterField_1"] = filterField_1 ?? "";
			ViewData["FilterValue_1"] = filterValue_1 ?? "";
			ViewData["FilterField_2"] = filterField_2 ?? "";
			ViewData["FilterValue_2"] = filterValue_2 ?? "";
			ViewData["FilterField_3"] = filterField_3 ?? "";
			ViewData["FilterValue_3"] = filterValue_3 ?? "";

			//Filter Dropdowns
			ViewData["Filters_1"] = new SelectList(filters, "Key", "Value", filterField_1);
			ViewData["Filters_2"] = new SelectList(filters, "Key", "Value", filterField_2);
			ViewData["Filters_3"] = new SelectList(filters, "Key", "Value", filterField_3);

			return View(items);
		}

		// GET: /List
		public ActionResult ListDeleted(
			int? page,
			string sortField,
			int? sortOrder,
			string filterField_1 = "",
			string filterValue_1 = "",
			string filterField_2 = "",
			string filterValue_2 = "",
			string filterField_3 = "",
			string filterValue_3 = "")
		{

			//Check AccessRights
			ViewData["Access"] = "";
			if (hierarchyRepository.AdminHasDomainWriteAccess(thirdPartyGroupName) || hierarchyRepository.AdminHasDomainWriteAccess(governmentGroupName))
			{
				ViewData["Access"] = "WriteAccess";
			}

			if (string.IsNullOrEmpty(sortField))
			{
				sortField = "ThirdPartyName";
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
			if (page < 1)
			{
				page = 1;
			}

			var items = thirdPartyUserRepository.PageThirdPartyUsers(
				page ?? 1,
				sortField,
				sortOrder ?? 0,
				true,
				filterField_1,
				filterValue_1,
				filterField_2,
				filterValue_2,
				filterField_3,
				filterValue_3
			);

			Dictionary<string, string> filters = thirdPartyUserRepository.GetThirdPartyUserFilters();

			//Default Filters
			ViewData["FilterField_1"] = filterField_1 ?? "";
			ViewData["FilterValue_1"] = filterValue_1 ?? "";
			ViewData["FilterField_2"] = filterField_2 ?? "";
			ViewData["FilterValue_2"] = filterValue_2 ?? "";
			ViewData["FilterField_3"] = filterField_3 ?? "";
			ViewData["FilterValue_3"] = filterValue_3 ?? "";

			//Filter Dropdowns
			ViewData["Filters_1"] = new SelectList(filters, "Key", "Value", filterField_1);
			ViewData["Filters_2"] = new SelectList(filters, "Key", "Value", filterField_2);
			ViewData["Filters_3"] = new SelectList(filters, "Key", "Value", filterField_3);

			return View(items);
		}

		public ActionResult Create()
		{
			//Set Access Rights
			ViewData["Access"] = "";
			if (hierarchyRepository.AdminHasDomainWriteAccess(thirdPartyGroupName) || hierarchyRepository.AdminHasDomainWriteAccess(governmentGroupName))
			{
				ViewData["Access"] = "WriteAccess";
			}

			ThirdPartyUserVM thirdPartyUserVM = new ThirdPartyUserVM();
			
			ThirdPartyUser thirdPartyUser = new ThirdPartyUser();
			thirdPartyUser.IsActiveFlagNonNullable = true;
			thirdPartyUser.RoboticUserFlagNonNullable = false;
			thirdPartyUserVM.ThirdPartyUser = thirdPartyUser;

			//CubaPseudoCityOrOfficeFlag
			//Only a user with the Compliance Administrator for Global can check or uncheck this box
			ViewData["ComplianceAdministratorAccess"] = "";
			if (rolesRepository.HasWriteAccessToThirdPartyUserCubaBookingAllowanceIndicator())
			{
				ViewData["ComplianceAdministratorAccess"] = "WriteAccess";
			}

			//GovernmentPseudoCityOrOfficeFlag
			//Only a user with the GDS Government Administrator role for Global can check or uncheck this box
			ViewData["GDSGovernmentAdministratorAccess"] = "";
			if (rolesRepository.HasWriteAccessToThirdPartyUserMilitaryAndGovernmentUserFlag())
			{
				ViewData["GDSGovernmentAdministratorAccess"] = "WriteAccess";
			}

			//ThirdPartyUserTypes
			thirdPartyUserVM.ThirdPartyUserTypes = new SelectList(thirdPartyUserTypeRepository.GetAllThirdPartyUserTypes().ToList(), "ThirdPartyUserTypeId", "ThirdPartyUserTypeName");
			
			//GDSThirdPartyVendors
			thirdPartyUserVM.GDSThirdPartyVendors = new SelectList(gdsThirdPartyVendorRepository.GetAllGDSThirdPartyVendors().ToList(), "GDSThirdPartyVendorId", "GDSThirdPartyVendorName");

			//StateProvinces
			thirdPartyUserVM.StateProvinces = new SelectList(stateProvinceRepository.GetStateProvincesByCountryCode(thirdPartyUser.CountryCode).ToList(), "StateProvinceCode", "Name");

			return View(thirdPartyUserVM);
		}

		// POST: /Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(ThirdPartyUserVM thirdPartyUserVM)
		{
			//Set Access Rights
			ViewData["Access"] = "";
			if (hierarchyRepository.AdminHasDomainWriteAccess(thirdPartyGroupName) || hierarchyRepository.AdminHasDomainWriteAccess(governmentGroupName))
			{
				ViewData["Access"] = "WriteAccess";
			}

			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!hierarchyRepository.AdminHasDomainWriteAccess(thirdPartyGroupName) && !hierarchyRepository.AdminHasDomainWriteAccess(governmentGroupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Update  Model from Form
			try
			{
				TryUpdateModel<ThirdPartyUserVM>(thirdPartyUserVM, "ThirdPartyUserVM");
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
				thirdPartyUserRepository.Add(thirdPartyUserVM);
			}
			catch (SqlException ex)
			{
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("ListUnDeleted");
		}
		
		// GET: /Edit
		public ActionResult Edit(int id)
		{
			ThirdPartyUser thirdPartyUser = new ThirdPartyUser();
			thirdPartyUser = thirdPartyUserRepository.GetThirdPartyUser(id);

			//Check Exists
			if (thirdPartyUser == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			ThirdPartyUserVM thirdPartyUserVM = new ThirdPartyUserVM();

			//CubaPseudoCityOrOfficeFlag
			//Only a user with the Compliance Administrator for Global can check or uncheck this box
			ViewData["ComplianceAdministratorAccess"] = "";
			if (rolesRepository.HasWriteAccessToThirdPartyUserCubaBookingAllowanceIndicator())
			{
				ViewData["ComplianceAdministratorAccess"] = "WriteAccess";
			}

			//GovernmentPseudoCityOrOfficeFlag
			//Only a user with the GDS Government Administrator role for Global can check or uncheck this box
			ViewData["GDSGovernmentAdministratorAccess"] = "";
			if (rolesRepository.HasWriteAccessToThirdPartyUserMilitaryAndGovernmentUserFlag())
			{
				ViewData["GDSGovernmentAdministratorAccess"] = "WriteAccess";
			}

			//ThirdPartyUserTypes
			thirdPartyUserVM.ThirdPartyUserTypes = new SelectList(thirdPartyUserTypeRepository.GetAllThirdPartyUserTypes().ToList(), "ThirdPartyUserTypeId", "ThirdPartyUserTypeName", thirdPartyUser.ThirdPartyUserTypeId);

			//GDSThirdPartyVendors
			thirdPartyUserVM.GDSThirdPartyVendors = new SelectList(gdsThirdPartyVendorRepository.GetAllGDSThirdPartyVendors().ToList(), "GDSThirdPartyVendorId", "GDSThirdPartyVendorName", thirdPartyUser.GDSThirdPartyVendorId);

			//StateProvinces
			thirdPartyUserVM.StateProvinces = new SelectList(stateProvinceRepository.GetStateProvincesByCountryCode(thirdPartyUser.CountryCode).ToList(), "StateProvinceCode", "Name", thirdPartyUser.StateProvinceCode);

			thirdPartyUserRepository.EditForDisplay(thirdPartyUser);
			thirdPartyUserVM.ThirdPartyUser = thirdPartyUser;

			return View(thirdPartyUserVM);
		}


		// POST: /Edit
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(ThirdPartyUserVM thirdPartyUserVM)
		{
			//Set Access Rights
			RolesRepository rolesRepository = new RolesRepository();
			if (!hierarchyRepository.AdminHasDomainWriteAccess(thirdPartyGroupName) && !hierarchyRepository.AdminHasDomainWriteAccess(governmentGroupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

            //Only a user with the correct roles can change these options
            if (!rolesRepository.HasWriteAccessToThirdPartyUserCubaBookingAllowanceIndicator() || !rolesRepository.HasWriteAccessToThirdPartyUserMilitaryAndGovernmentUserFlag()) {

                ThirdPartyUser thirdPartyUser = new ThirdPartyUser();
                thirdPartyUser = thirdPartyUserRepository.GetThirdPartyUser(thirdPartyUserVM.ThirdPartyUser.ThirdPartyUserId);

                //Check Exists
                if (thirdPartyUser == null)
                {
                    ViewData["ActionMethod"] = "EditGet";
                    return View("RecordDoesNotExistError");
                }

                thirdPartyUserRepository.EditForDisplay(thirdPartyUser);

                //CubaPseudoCityOrOfficeFlag
                //Only a user with the Compliance Administrator for Global can check or uncheck this box
                if (!rolesRepository.HasWriteAccessToThirdPartyUserCubaBookingAllowanceIndicator())
                {
                    thirdPartyUserVM.ThirdPartyUser.CubaBookingAllowanceIndicatorNonNullable = thirdPartyUser.CubaBookingAllowanceIndicatorNonNullable;
                }

                //GovernmentPseudoCityOrOfficeFlag
                //Only a user with the GDS Government Administrator role for Global can check or uncheck this box
                if (!rolesRepository.HasWriteAccessToThirdPartyUserMilitaryAndGovernmentUserFlag())
                {
                    thirdPartyUserVM.ThirdPartyUser.MilitaryAndGovernmentUserFlagNonNullable = thirdPartyUser.MilitaryAndGovernmentUserFlagNonNullable; 
                }
            }

            //Update  Model from Form
            try
			{
				TryUpdateModel<ThirdPartyUser>(thirdPartyUserVM.ThirdPartyUser, "ThirdPartyUser");
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
				thirdPartyUserRepository.Update(thirdPartyUserVM);
			}
			catch (SqlException ex)
			{
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			string redirectAction = (thirdPartyUserVM.ThirdPartyUser.DeletedFlag == true) ? "ListDeleted" : "ListUnDeleted";

			return RedirectToAction(redirectAction);
		}


		// GET: /View
		public ActionResult View(int id)
		{
			//Check Exists
			ThirdPartyUser thirdPartyUser = new ThirdPartyUser();
			thirdPartyUser = thirdPartyUserRepository.GetThirdPartyUser(id);
			if (thirdPartyUser == null)
			{
				ViewData["ActionMethod"] = "ViewGet";
				return View("RecordDoesNotExistError");
			}

			thirdPartyUserRepository.EditForDisplay(thirdPartyUser);

			return View(thirdPartyUser);
		}

		// GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id)
		{
			ThirdPartyUser thirdPartyUser = new ThirdPartyUser();
			thirdPartyUser = thirdPartyUserRepository.GetThirdPartyUser(id);

			//Check Exists
			if (thirdPartyUser == null)
			{
				ViewData["ActionMethod"] = "DeleteGet";
				return View("RecordDoesNotExistError");
			}

			//Check AccessRights
			RolesRepository rolesRepository = new RolesRepository();
			if(!hierarchyRepository.AdminHasDomainWriteAccess(thirdPartyGroupName) && !hierarchyRepository.AdminHasDomainWriteAccess(governmentGroupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			ThirdPartyUserVM thirdPartyUserVM = new ThirdPartyUserVM();
			thirdPartyUserRepository.EditForDisplay(thirdPartyUser);
			thirdPartyUserVM.ThirdPartyUser = thirdPartyUser;
			return View(thirdPartyUserVM);
		}

		// POST: /Delete
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(ThirdPartyUserVM thirdPartyUserVM, FormCollection collection)
		{
			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!hierarchyRepository.AdminHasDomainWriteAccess(thirdPartyGroupName) && !hierarchyRepository.AdminHasDomainWriteAccess(governmentGroupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Get Item From Database
			ThirdPartyUser thirdPartyUser = new ThirdPartyUser();
			thirdPartyUser = thirdPartyUserRepository.GetThirdPartyUser(thirdPartyUserVM.ThirdPartyUser.ThirdPartyUserId);

			//Check Exists
			if (thirdPartyUser == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Delete Item
			try
			{
				thirdPartyUserVM.ThirdPartyUser.DeletedFlag = true;
				thirdPartyUserRepository.UpdateDeletedStatus(thirdPartyUserVM);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/ThirdPartyUser.mvc/Delete/" + thirdPartyUser.ThirdPartyUserId;
					return View("VersionError");
				}
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("ListUnDeleted");
		}

		// GET: /UnDelete
		[HttpGet]
		public ActionResult UnDelete(int id)
		{
			ThirdPartyUser thirdPartyUser = new ThirdPartyUser();
			thirdPartyUser = thirdPartyUserRepository.GetThirdPartyUser(id);

			//Check Exists
			if (thirdPartyUser == null)
			{
				ViewData["ActionMethod"] = "DeleteGet";
				return View("RecordDoesNotExistError");
			}

			//Check AccessRights
			RolesRepository rolesRepository = new RolesRepository();
			if (!hierarchyRepository.AdminHasDomainWriteAccess(thirdPartyGroupName) && !hierarchyRepository.AdminHasDomainWriteAccess(governmentGroupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			ThirdPartyUserVM thirdPartyUserVM = new ThirdPartyUserVM();
			thirdPartyUserRepository.EditForDisplay(thirdPartyUser);
			thirdPartyUserVM.ThirdPartyUser = thirdPartyUser;

			return View(thirdPartyUserVM);
		}

		// POST: /UnDelete
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult UnDelete(ThirdPartyUserVM thirdPartyUserVM, FormCollection collection)
		{
			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!hierarchyRepository.AdminHasDomainWriteAccess(thirdPartyGroupName) && !hierarchyRepository.AdminHasDomainWriteAccess(governmentGroupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Get Item From Database
			ThirdPartyUser thirdPartyUser = new ThirdPartyUser();
			thirdPartyUser = thirdPartyUserRepository.GetThirdPartyUser(thirdPartyUserVM.ThirdPartyUser.ThirdPartyUserId);

			//Check Exists
			if (thirdPartyUser == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Delete Item
			try
			{
				thirdPartyUserVM.ThirdPartyUser.DeletedFlag = false;
				thirdPartyUserRepository.UpdateDeletedStatus(thirdPartyUserVM);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/ThirdPartyUser.mvc/Delete/" + thirdPartyUser.ThirdPartyUserId;
					return View("VersionError");
				}
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("ListDeleted");
		}

		// GET: /Export
		public ActionResult Export(
			string filterField_1 = "",
			string filterValue_1 = "",
			string filterField_2 = "",
			string filterValue_2 = "",
			string filterField_3 = "",
			string filterValue_3 = ""
		)
		{
			//Get CSV Data
			byte[] csvData = thirdPartyUserRepository.Export(
				filterField_1,
				filterValue_1,
				filterField_2,
				filterValue_2,
				filterField_3,
				filterValue_3
			);

			return File(csvData, "text/csv", "Third Party User Export.csv");
		}

		//AutoComplete Client SubUnits based on Client TopUnit
		[HttpPost]
		public JsonResult AutoCompleteClientTopUnitClientSubUnits(string searchText, string clientTopUnitGuid)
		{
			HierarchyRepository hierarchyRepository = new HierarchyRepository();

			string roleName = "GDS Third Party Administrator";
			int maxResults = 1000;
			var result = hierarchyRepository.LookUpSystemUserClientTopUnitClientSubUnits(searchText, maxResults, clientTopUnitGuid, roleName);
			return Json(result);
		}

		//Get PseudoCityOrOfficeIds by GDSCode
		[HttpPost]
		public JsonResult GetThirdPartyUserPseudoCityOrOfficeIdsByGDSCode(string gdsCode, int thirdPartyUserId)
		{
			var result = thirdPartyUserRepository.GetThirdPartyUserPseudoCityOrOfficeIdsByGDSCode(gdsCode, thirdPartyUserId);
			return Json(result);
		}

		//Log Create Error in Application Usage
		[HttpPost]
		public JsonResult LogCreateError(string thirdPartyName, string firstName, string lastName, string email)
		{
			var result = LogError(108, thirdPartyName, firstName, lastName, email);
			return Json(result);
		}

		//Log Update Error in Application Usage
		[HttpPost]
		public JsonResult LogUpdateError(string tisUserId, string thirdPartyName, string firstName, string lastName, string email)
		{
			var result = LogError(109, thirdPartyName, firstName, lastName, email, tisUserId);
			return Json(result);
		}

		public string LogError(int applicationEventId, string thirdPartyName, string firstName, string lastName, string email, string tisUserId = "")
		{
			StringBuilder additionalInformation = new StringBuilder();

			if (!string.IsNullOrEmpty(tisUserId))
			{
				additionalInformation.AppendFormat("TISUserId: {0}; ", tisUserId);
			}

			if (!string.IsNullOrEmpty(thirdPartyName))
			{
				additionalInformation.AppendFormat("ThirdPartyName: {0}; ", thirdPartyName);
			}

			if (!string.IsNullOrEmpty(firstName))
			{
				additionalInformation.AppendFormat("FirstName: {0}; ", firstName);
			}
			if (!string.IsNullOrEmpty(lastName))
			{
				additionalInformation.AppendFormat("LastName: {0}; ", lastName);
			}

			if (!string.IsNullOrEmpty(email))
			{
				additionalInformation.AppendFormat("Email: {0};", email);
			}

			LogRepository logRepository = new LogRepository();
			logRepository.LogApplicationUsage(
				applicationEventId,
				null,
				null,
				additionalInformation.ToString(),
				null,
				null,
				false
			);

			return "Error Logged";
		}
    }
}
