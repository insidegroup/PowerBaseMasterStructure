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

namespace CWTDesktopDatabase.Controllers
{
    public class ServiceAccountController : Controller
    {
        //main repositories
        ServiceAccountRepository serviceAccountRepository = new ServiceAccountRepository();
		HierarchyRepository hierarchyRepository = new HierarchyRepository();		
        RolesRepository rolesRepository = new RolesRepository();

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
			if (rolesRepository.HasWriteAccessToServiceAccounts())
			{
				ViewData["Access"] = "WriteAccess";
			}

			if (string.IsNullOrEmpty(sortField))
			{
				sortField = "ServiceAccountName";
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

			var items = serviceAccountRepository.PageServiceAccounts(
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

			Dictionary<string, string> filters = serviceAccountRepository.GetThirdPartyUserFilters();

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
            if (rolesRepository.HasWriteAccessToServiceAccounts())
            {
                ViewData["Access"] = "WriteAccess";
            }

            if (string.IsNullOrEmpty(sortField))
            {
                sortField = "ServiceAccountName";
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

            var items = serviceAccountRepository.PageServiceAccounts(
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

			Dictionary<string, string> filters = serviceAccountRepository.GetThirdPartyUserFilters();

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
			if (rolesRepository.HasWriteAccessToServiceAccounts())
			{
				ViewData["Access"] = "WriteAccess";
			}

			ServiceAccountVM serviceAccountVM = new ServiceAccountVM();
			
			ServiceAccount serviceAccount = new ServiceAccount();
			serviceAccount.RoboticUserFlag = true;
			serviceAccount.ThirdPartyUserType = "Internal";
			serviceAccount.IsActiveFlag = true;
			serviceAccountVM.ServiceAccount = serviceAccount;

            //CubaPseudoCityOrOfficeFlag
            //Only a user with the Compliance Administrator for Global can check or uncheck this box
            ViewData["ComplianceAdministratorAccess"] = "";
			if (rolesRepository.HasWriteAccessToServiceAccountCubaBookingAllowanceIndicator())
            {
                ViewData["ComplianceAdministratorAccess"] = "WriteAccess";
            }

            //GovernmentPseudoCityOrOfficeFlag
            //Only a user with the GDS Government Administrator role for Global can check or uncheck this box
            ViewData["GDSGovernmentAdministratorAccess"] = "";
			if (rolesRepository.HasWriteAccessToServiceAccountMilitaryAndGovernmentUserFlag())
            {
                ViewData["GDSGovernmentAdministratorAccess"] = "WriteAccess";
            }

			return View(serviceAccountVM);
		}

		// POST: /Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(ServiceAccountVM serviceAccountVM)
		{
			//Set Access Rights
			ViewData["Access"] = "";
			if (rolesRepository.HasWriteAccessToServiceAccounts())
			{
				ViewData["Access"] = "WriteAccess";
			}

			//Check Access
			if (!rolesRepository.HasWriteAccessToServiceAccounts())
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Update  Model from Form
			try
			{
				TryUpdateModel<ServiceAccountVM>(serviceAccountVM, "GDSServiceAccountVM");
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
				serviceAccountRepository.Add(serviceAccountVM);
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

        // GET: /View
        public ActionResult View(string id)
        {
            //Check Exists
            ServiceAccount serviceAccount = new ServiceAccount();
            serviceAccount = serviceAccountRepository.GetServiceAccount(id);
            if (serviceAccount == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            //serviceAccountRepository.EditForDisplay(serviceAccount);

            return View(serviceAccount);
        }

        // GET: /Edit
        public ActionResult Edit(string id)
        {
            ServiceAccount serviceAccount = new ServiceAccount();
            serviceAccount = serviceAccountRepository.GetServiceAccount(id);

            //Check Exists
            if (serviceAccount == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            serviceAccountRepository.EditForDisplay(serviceAccount);

            ServiceAccountVM serviceAccountVM = new ServiceAccountVM();

			//CubaPseudoCityOrOfficeFlag
			//Only a user with the Compliance Administrator for Global can check or uncheck this box
			ViewData["ComplianceAdministratorAccess"] = "";
			if (rolesRepository.HasWriteAccessToServiceAccountCubaBookingAllowanceIndicator())
			{
				ViewData["ComplianceAdministratorAccess"] = "WriteAccess";
			}

			//GovernmentPseudoCityOrOfficeFlag
			//Only a user with the GDS Government Administrator role for Global can check or uncheck this box
			ViewData["GDSGovernmentAdministratorAccess"] = "";
			if (rolesRepository.HasWriteAccessToServiceAccountMilitaryAndGovernmentUserFlag())
			{
				ViewData["GDSGovernmentAdministratorAccess"] = "WriteAccess";
			}

            //thirdPartyUserRepository.EditForDisplay(serviceAccount);
            serviceAccountVM.ServiceAccount = serviceAccount;

            return View(serviceAccountVM);
        }


        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ServiceAccountVM serviceAccountVM)
        {
            //Set Access Rights
			if (!rolesRepository.HasWriteAccessToServiceAccounts())
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Only a user with the correct roles can change these options
            if (!rolesRepository.HasWriteAccessToThirdPartyUserCubaBookingAllowanceIndicator() || !rolesRepository.HasWriteAccessToThirdPartyUserMilitaryAndGovernmentUserFlag())
            {

                ServiceAccount serviceAccount = new ServiceAccount();
                serviceAccount = serviceAccountRepository.GetServiceAccount(serviceAccountVM.ServiceAccount.ServiceAccountId);

                //Check Exists
                if (serviceAccount == null)
                {
                    ViewData["ActionMethod"] = "EditGet";
                    return View("RecordDoesNotExistError");
                }

                serviceAccountRepository.EditForDisplay(serviceAccount);

                //CubaPseudoCityOrOfficeFlag
                //Only a user with the Compliance Administrator for Global can check or uncheck this box
                if (!rolesRepository.HasWriteAccessToThirdPartyUserCubaBookingAllowanceIndicator())
                {
                    serviceAccountVM.ServiceAccount.CubaBookingAllowanceIndicatorNonNullable = serviceAccount.CubaBookingAllowanceIndicatorNonNullable;
                }

                //GovernmentPseudoCityOrOfficeFlag
                //Only a user with the GDS Government Administrator role for Global can check or uncheck this box
                if (!rolesRepository.HasWriteAccessToThirdPartyUserMilitaryAndGovernmentUserFlag())
                {
                    serviceAccountVM.ServiceAccount.MilitaryAndGovernmentUserFlagNonNullable = serviceAccount.MilitaryAndGovernmentUserFlagNonNullable;
                }
            }
            //Update  Model from Form
            try
            {
                TryUpdateModel<ServiceAccount>(serviceAccountVM.ServiceAccount, "ServiceAccount");
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
                serviceAccountRepository.Update(serviceAccountVM);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

			string redirectAction = (serviceAccountVM.ServiceAccount.DeletedFlag == true) ? "ListDeleted" : "ListUnDeleted";

			return RedirectToAction(redirectAction);
        }

        // GET: /Delete
        [HttpGet]
        public ActionResult Delete(string id)
        {
            ServiceAccount serviceAccount = new ServiceAccount();
            serviceAccount = serviceAccountRepository.GetServiceAccount(id);

            //Check Exists
            if (serviceAccount == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
			if (!rolesRepository.HasWriteAccessToServiceAccounts())
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ServiceAccountVM serviceAccountVM = new ServiceAccountVM();
            serviceAccountVM.ServiceAccount = serviceAccount;

            return View(serviceAccountVM);
        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(ServiceAccountVM serviceAccountVM, FormCollection collection)
        {
            //Check Access
			if (!rolesRepository.HasWriteAccessToServiceAccounts())
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Get Item From Database
            ServiceAccount serviceAccount = new ServiceAccount();
            serviceAccount = serviceAccountRepository.GetServiceAccount(serviceAccountVM.ServiceAccount.ServiceAccountId);

            //Check Exists
            if (serviceAccount == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            //Delete Item
            try
            {
                serviceAccountVM.ServiceAccount.DeletedFlag = true;
                serviceAccountRepository.UpdateDeletedStatus(serviceAccountVM);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/ThirdPartyUser.mvc/Delete/" + serviceAccount.ServiceAccountId;
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
        public ActionResult UnDelete(string id)
        {
            ServiceAccount serviceAccount = new ServiceAccount();
            serviceAccount = serviceAccountRepository.GetServiceAccount(id);

            //Check Exists
            if (serviceAccount == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
			if (!rolesRepository.HasWriteAccessToServiceAccounts())
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ServiceAccountVM serviceAccountVM = new ServiceAccountVM();
            serviceAccountVM.ServiceAccount = serviceAccount;

            return View(serviceAccountVM);
        }

        // POST: /UnDelete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UnDelete(ServiceAccountVM serviceAccountVM, FormCollection collection)
        {
            //Check Access
			if (!rolesRepository.HasWriteAccessToServiceAccounts())
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Get Item From Database
            ServiceAccount serviceAccount = new ServiceAccount();
            serviceAccount = serviceAccountRepository.GetServiceAccount(serviceAccountVM.ServiceAccount.ServiceAccountId);

            //Check Exists
            if (serviceAccount == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            //Delete Item
            try
            {
                serviceAccountVM.ServiceAccount.DeletedFlag = false;
                serviceAccountRepository.UpdateDeletedStatus(serviceAccountVM);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/ThirdPartyUser.mvc/Delete/" + serviceAccount.ServiceAccountId;
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
            byte[] csvData = serviceAccountRepository.Export(
                filterField_1,
                filterValue_1,
                filterField_2,
                filterValue_2,
                filterField_3,
                filterValue_3
            );

            return File(csvData, "text/csv", "Service Account Export.csv");
        }

		//Get PseudoCityOrOfficeIds by GDSCode
		[HttpPost]
		public JsonResult GetServiceAccountPseudoCityOrOfficeIdsByGDSCode(string gdsCode, string serviceAccountId)
		{
			var result = serviceAccountRepository.GetServiceAccountPseudoCityOrOfficeIdsByGDSCode(gdsCode, serviceAccountId);
			return Json(result);
		}

    }
}
