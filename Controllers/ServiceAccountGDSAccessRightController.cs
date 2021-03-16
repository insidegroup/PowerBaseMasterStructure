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
    public class ServiceAccountGDSAccessRightController : Controller
    {
        ServiceAccountGDSAccessRightRepository serviceAccountGDSAccessRightRepository = new ServiceAccountGDSAccessRightRepository();
        GDSRepository gdsRepository = new GDSRepository();
        GDSAccessTypeRepository gdsAccessTypeRepository = new GDSAccessTypeRepository();
        ServiceAccountRepository serviceAccountRepository = new ServiceAccountRepository();

        HierarchyRepository hierarchyRepository = new HierarchyRepository();
		RolesRepository rolesRepository = new RolesRepository();

        // GET: /List
        public ActionResult ListUnDeleted(string id, int? page, string filter, string sortField, int? sortOrder)
        {
            //Set Access Rights
            ViewData["Access"] = "";
			if (rolesRepository.HasWriteAccessToServiceAccounts())
            {
                ViewData["Access"] = "WriteAccess";
            }

            //SortField
            if (string.IsNullOrEmpty(sortField))
            {
                sortField = "GDSName";
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
            ServiceAccountGDSAccessRightsVM serviceAccountGDSAccessRightsVM = new ServiceAccountGDSAccessRightsVM();

            var getServiceAccountGDSAccessRights = serviceAccountGDSAccessRightRepository.PageServiceAccountGDSAccessRights(id, filter ?? "", sortField, sortOrder ?? 0, page ?? 1, false);
            if (getServiceAccountGDSAccessRights != null)
            {
                serviceAccountGDSAccessRightsVM.ServiceAccountGDSAccessRights = getServiceAccountGDSAccessRights;
            }

            //ServiceAccount
            ServiceAccount serviceAccount = new ServiceAccount();
            serviceAccount = serviceAccountRepository.GetServiceAccount(id);
            if (serviceAccount != null)
            {
                serviceAccountGDSAccessRightsVM.ServiceAccount = serviceAccount;
            }

            return View(serviceAccountGDSAccessRightsVM);
        }

        // GET: /List
        public ActionResult ListDeleted(string id, int? page, string filter, string sortField, int? sortOrder)
        {
            //Set Access Rights
            ViewData["Access"] = "";
			if (rolesRepository.HasWriteAccessToServiceAccounts())
            {
                ViewData["Access"] = "WriteAccess";
            }

            //SortField
            if (string.IsNullOrEmpty(sortField))
            {
                sortField = "GDSName";
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
            ServiceAccountGDSAccessRightsVM serviceAccountGDSAccessRightsVM = new ServiceAccountGDSAccessRightsVM();

            var getServiceAccountGDSAccessRights = serviceAccountGDSAccessRightRepository.PageServiceAccountGDSAccessRights(id, filter ?? "", sortField, sortOrder ?? 0, page ?? 1, true);
            if (getServiceAccountGDSAccessRights != null)
            {
                serviceAccountGDSAccessRightsVM.ServiceAccountGDSAccessRights = getServiceAccountGDSAccessRights;
            }

            //ServiceAccount
            ServiceAccount serviceAccount = new ServiceAccount();
            serviceAccount = serviceAccountRepository.GetServiceAccount(id);
            if (serviceAccount != null)
            {
                serviceAccountGDSAccessRightsVM.ServiceAccount = serviceAccount;
            }

            return View(serviceAccountGDSAccessRightsVM);
        }

        public ActionResult Create(string id)
        {
            //Set Access Rights
            ViewData["Access"] = "";
			if (rolesRepository.HasWriteAccessToServiceAccounts())
            {
                ViewData["Access"] = "WriteAccess";
            }

            ServiceAccountGDSAccessRightVM serviceAccountGDSAccessRightVM = new ServiceAccountGDSAccessRightVM();

            ServiceAccountGDSAccessRight serviceAccountGDSAccessRight = new ServiceAccountGDSAccessRight();
            serviceAccountGDSAccessRightVM.ServiceAccountGDSAccessRight = serviceAccountGDSAccessRight;

            //GDS
            serviceAccountGDSAccessRightVM.GDSs = new SelectList(gdsRepository.GetAllGDSs().ToList(), "GDSCode", "GDSName");

            //GDSAccessTypes
            serviceAccountGDSAccessRightVM.GDSAccessTypes = new SelectList(gdsAccessTypeRepository.GetAllGDSAccessTypes().ToList(), "GDSAccessTypeId", "GDSAccessTypeName");

            //ServiceAccount
            ServiceAccount serviceAccount = new ServiceAccount();
            serviceAccount = serviceAccountRepository.GetServiceAccount(id);
            if (serviceAccount != null)
            {
                serviceAccountGDSAccessRightVM.ServiceAccount = serviceAccount;
                serviceAccountGDSAccessRightVM.ServiceAccountGDSAccessRight.ServiceAccountId = serviceAccount.ServiceAccountId;
            }

            return View(serviceAccountGDSAccessRightVM);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ServiceAccountGDSAccessRightVM serviceAccountGDSAccessRightVM)
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
                TryUpdateModel<ServiceAccountGDSAccessRightVM>(serviceAccountGDSAccessRightVM, "ServiceAccountGDSAccessRightVM");
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
                serviceAccountGDSAccessRightRepository.Add(serviceAccountGDSAccessRightVM);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("ListUnDeleted", new { id = serviceAccountGDSAccessRightVM.ServiceAccountGDSAccessRight.ServiceAccountId });
        }

        // GET: /Edit
        public ActionResult Edit(int id)
        {
            //Set Access Rights
            ViewData["Access"] = "";
            if (rolesRepository.HasWriteAccessToServiceAccounts())
            {
                ViewData["Access"] = "WriteAccess";
            }

            ServiceAccountGDSAccessRight serviceAccountGDSAccessRight = serviceAccountGDSAccessRightRepository.GetServiceAccountGDSAccessRight(id);

            //Check Exists
            if (serviceAccountGDSAccessRight == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Check Access
            if (!rolesRepository.HasWriteAccessToServiceAccounts())
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ServiceAccountGDSAccessRightVM serviceAccountGDSAccessRightVM = new ServiceAccountGDSAccessRightVM();
            serviceAccountGDSAccessRightVM.ServiceAccountGDSAccessRight = serviceAccountGDSAccessRight;

            //GDS
            serviceAccountGDSAccessRightVM.GDSs = new SelectList(gdsRepository.GetAllGDSs().ToList(), "GDSCode", "GDSName", serviceAccountGDSAccessRight.GDSCode);

            //GDSAccessTypes
            serviceAccountGDSAccessRightVM.GDSAccessTypes = new SelectList(gdsAccessTypeRepository.GetGDSAccessTypesByGDSCode(serviceAccountGDSAccessRight.GDSCode).ToList(), "GDSAccessTypeId", "GDSAccessTypeName", serviceAccountGDSAccessRight.GDSAccessTypeId);

            //PseudoCityOrOfficeIds
            serviceAccountGDSAccessRightVM.PseudoCityOrOfficeIds = new SelectList(
				serviceAccountRepository.GetServiceAccountPseudoCityOrOfficeIdsByGDSCode(serviceAccountGDSAccessRight.GDSCode, serviceAccountGDSAccessRight.ServiceAccountId).ToList(), 
				"PseudoCityOrOfficeId", 
				"PseudoCityOrOfficeId", 
				serviceAccountGDSAccessRight.PseudoCityOrOfficeId
			);

            //System User
            ServiceAccount serviceAccount = new ServiceAccount();
            serviceAccount = serviceAccountRepository.GetServiceAccount(serviceAccountGDSAccessRight.ServiceAccountId);
            if (serviceAccount != null)
            {
                serviceAccountGDSAccessRightVM.ServiceAccount = serviceAccount;
            }

            return View(serviceAccountGDSAccessRightVM);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ServiceAccountGDSAccessRightVM serviceAccountGDSAccessRightVM)
        {
            if (!rolesRepository.HasWriteAccessToServiceAccounts())
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update  Model from Form
            try
            {
                TryUpdateModel<ServiceAccountGDSAccessRight>(serviceAccountGDSAccessRightVM.ServiceAccountGDSAccessRight, "ServiceAccountGDSAccessRight");
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
                serviceAccountGDSAccessRightRepository.Update(serviceAccountGDSAccessRightVM);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("ListUnDeleted", new { id = serviceAccountGDSAccessRightVM.ServiceAccountGDSAccessRight.ServiceAccountId });
        }

        // GET: /View
        [HttpGet]
        public ActionResult View(int id)
        {
            ServiceAccountGDSAccessRight serviceAccountGDSAccessRight = new ServiceAccountGDSAccessRight();
            serviceAccountGDSAccessRight = serviceAccountGDSAccessRightRepository.GetServiceAccountGDSAccessRight(id);

            //Check Exists
            if (serviceAccountGDSAccessRight == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            ServiceAccountGDSAccessRightVM serviceAccountGDSAccessRightVM = new ServiceAccountGDSAccessRightVM();

            //System User
            ServiceAccount serviceAccount = new ServiceAccount();
            serviceAccount = serviceAccountRepository.GetServiceAccount(serviceAccountGDSAccessRight.ServiceAccountId);
            if (serviceAccount != null)
            {
                serviceAccountGDSAccessRightVM.ServiceAccount = serviceAccount;
            }

            serviceAccountGDSAccessRightVM.ServiceAccountGDSAccessRight = serviceAccountGDSAccessRight;

            return View(serviceAccountGDSAccessRightVM);
        }

        // GET: /Delete
        [HttpGet]
        public ActionResult Delete(int id)
        {
            ServiceAccountGDSAccessRight serviceAccountGDSAccessRight = new ServiceAccountGDSAccessRight();
            serviceAccountGDSAccessRight = serviceAccountGDSAccessRightRepository.GetServiceAccountGDSAccessRight(id);

            //Check Exists
            if (serviceAccountGDSAccessRight == null)
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

            ServiceAccountGDSAccessRightVM serviceAccountGDSAccessRightVM = new ServiceAccountGDSAccessRightVM();

            //System User
            ServiceAccount serviceAccount = new ServiceAccount();
            serviceAccount = serviceAccountRepository.GetServiceAccount(serviceAccountGDSAccessRight.ServiceAccountId);
            if (serviceAccount != null)
            {
                serviceAccountGDSAccessRightVM.ServiceAccount = serviceAccount;
            }

            serviceAccountGDSAccessRightVM.ServiceAccountGDSAccessRight = serviceAccountGDSAccessRight;

            return View(serviceAccountGDSAccessRightVM);
        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(ServiceAccountGDSAccessRightVM serviceAccountGDSAccessRightVM, FormCollection collection)
        {
            //Check Access
            if (!rolesRepository.HasWriteAccessToServiceAccounts())
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Get Item From Database
            ServiceAccountGDSAccessRight serviceAccountGDSAccessRight = new ServiceAccountGDSAccessRight();
            serviceAccountGDSAccessRight = serviceAccountGDSAccessRightRepository.GetServiceAccountGDSAccessRight(
                serviceAccountGDSAccessRightVM.ServiceAccountGDSAccessRight.ServiceAccountGDSAccessRightId
            );

            //Check Exists
            if (serviceAccountGDSAccessRight == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            //Delete Item
            try
            {
                serviceAccountGDSAccessRightVM.ServiceAccountGDSAccessRight.DeletedFlag = true;
                serviceAccountGDSAccessRightRepository.UpdateDeletedStatus(serviceAccountGDSAccessRightVM);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/ServiceAccountGDSAccessRight.mvc/Delete/" + serviceAccountGDSAccessRight.ServiceAccountGDSAccessRightId;
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }
            return RedirectToAction("ListUnDeleted", new { id = serviceAccountGDSAccessRightVM.ServiceAccountGDSAccessRight.ServiceAccountId });
        }

        // GET: /UnDelete
        [HttpGet]
        public ActionResult UnDelete(int id)
        {
            ServiceAccountGDSAccessRight serviceAccountGDSAccessRight = new ServiceAccountGDSAccessRight();
            serviceAccountGDSAccessRight = serviceAccountGDSAccessRightRepository.GetServiceAccountGDSAccessRight(id);

            //Check Exists
            if (serviceAccountGDSAccessRight == null)
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

            ServiceAccountGDSAccessRightVM serviceAccountGDSAccessRightVM = new ServiceAccountGDSAccessRightVM();

            //System User
            ServiceAccount serviceAccount = new ServiceAccount();
            serviceAccount = serviceAccountRepository.GetServiceAccount(serviceAccountGDSAccessRight.ServiceAccountId);
            if (serviceAccount != null)
            {
                serviceAccountGDSAccessRightVM.ServiceAccount = serviceAccount;
            }

            serviceAccountGDSAccessRightVM.ServiceAccountGDSAccessRight = serviceAccountGDSAccessRight;

            return View(serviceAccountGDSAccessRightVM);
        }

        // POST: /UnDelete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UnDelete(ServiceAccountGDSAccessRightVM serviceAccountGDSAccessRightVM, FormCollection collection)
        {
            //Check Access
            if (!rolesRepository.HasWriteAccessToServiceAccounts())
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Get Item From Database
            ServiceAccountGDSAccessRight serviceAccountGDSAccessRight = new ServiceAccountGDSAccessRight();
            serviceAccountGDSAccessRight = serviceAccountGDSAccessRightRepository.GetServiceAccountGDSAccessRight(
                serviceAccountGDSAccessRightVM.ServiceAccountGDSAccessRight.ServiceAccountGDSAccessRightId
            );

            //Check Exists
            if (serviceAccountGDSAccessRight == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            //Delete Item
            try
            {
                serviceAccountGDSAccessRightVM.ServiceAccountGDSAccessRight.DeletedFlag = false;
                serviceAccountGDSAccessRightRepository.UpdateDeletedStatus(serviceAccountGDSAccessRightVM);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/ServiceAccountGDSAccessRight.mvc/Delete/" + serviceAccountGDSAccessRight.ServiceAccountGDSAccessRightId;
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }
            return RedirectToAction("ListDeleted", new { id = serviceAccountGDSAccessRightVM.ServiceAccountGDSAccessRight.ServiceAccountId });
        }

        // GET: /Export
        public ActionResult Export(string id)
        {
            //Get CSV Data
            byte[] csvData = serviceAccountGDSAccessRightRepository.Export(id);
            return File(csvData, "text/csv", "GDS Access Right Export.csv");
        }
    }
}