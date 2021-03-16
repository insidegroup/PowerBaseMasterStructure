using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Repository;
using CWTDesktopDatabase.Helpers;
using System.Data.SqlClient;
using CWTDesktopDatabase.ViewModels;

namespace CWTDesktopDatabase.Controllers
{
    public class ApprovalGroupApprovalTypeController : Controller
    {
        ApprovalGroupApprovalTypeRepository approvalGroupApprovalTypeRepository = new ApprovalGroupApprovalTypeRepository();
        HierarchyRepository hierarchyRepository = new HierarchyRepository();

        private string groupName = "System Data Administrator";

        // GET: /List
        public ActionResult List(int? page)
        {
            //Set Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Access"] = "WriteAccess";
            }
            ViewData["CurrentSortField"] = "ApprovalGroupApprovalTypeId";
            ViewData["CurrentSortOrder"] = 0;

            //return items
            var cwtPaginatedList = approvalGroupApprovalTypeRepository.PageApprovalGroupApprovalTypes(page ?? 1);
            return View(cwtPaginatedList);
        }

        // GET: /Create
        public ActionResult Create()
        {
            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ApprovalGroupApprovalType approvalGroupApprovalType = new ApprovalGroupApprovalType();
            return View(approvalGroupApprovalType);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ApprovalGroupApprovalType approvalGroupApprovalType)
        {
            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update  Model from Form
            try
            {
                UpdateModel(approvalGroupApprovalType);
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
                approvalGroupApprovalTypeRepository.Add(approvalGroupApprovalType);
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
            ApprovalGroupApprovalType approvalGroupApprovalType = new ApprovalGroupApprovalType();
            approvalGroupApprovalType = approvalGroupApprovalTypeRepository.GetApprovalGroupApprovalType(id);

            if (approvalGroupApprovalType == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Id can be changed so need to set old id to pass into edit
            approvalGroupApprovalType.NewApprovalGroupApprovalTypeId = approvalGroupApprovalType.ApprovalGroupApprovalTypeId;

            return View(approvalGroupApprovalType);
        }

        // POST: /Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection collection)
        {

            //Get Item From Database
            ApprovalGroupApprovalType approvalGroupApprovalType = new ApprovalGroupApprovalType();
            approvalGroupApprovalType = approvalGroupApprovalTypeRepository.GetApprovalGroupApprovalType(id);

            //Check Exists
            if (approvalGroupApprovalType == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update Item from Form
            try
            {
                UpdateModel(approvalGroupApprovalType);
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
                approvalGroupApprovalType.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                approvalGroupApprovalTypeRepository.Update(approvalGroupApprovalType);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/ApprovalGroupApprovalType.mvc/Edit/" + id.ToString();
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
            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ApprovalGroupApprovalTypeVM approvalGroupApprovalTypeVM = new ApprovalGroupApprovalTypeVM();
            approvalGroupApprovalTypeVM.AllowDelete = true;

            ApprovalGroupApprovalType approvalGroupApprovalType = new ApprovalGroupApprovalType();
            approvalGroupApprovalType = approvalGroupApprovalTypeRepository.GetApprovalGroupApprovalType(id);

            approvalGroupApprovalTypeVM.ApprovalGroupApprovalType = approvalGroupApprovalType;

            //Attached Items
            List<ApprovalGroupApprovalTypeReference> approvalGroupApprovalTypeReferences = approvalGroupApprovalTypeRepository.GetApprovalGroupApprovalTypeReferences(approvalGroupApprovalType.ApprovalGroupApprovalTypeId);
            if (approvalGroupApprovalTypeReferences.Count > 0)
            {
                approvalGroupApprovalTypeVM.AllowDelete = false;
                approvalGroupApprovalTypeVM.ApprovalGroupApprovalTypeReferences = approvalGroupApprovalTypeReferences;
            }

            return View(approvalGroupApprovalTypeVM);
        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(ApprovalGroupApprovalTypeVM approvalGroupApprovalTypeVM, FormCollection collection)
        {
            //Get Item From Database
            ApprovalGroupApprovalType approvalGroupApprovalType = new ApprovalGroupApprovalType();
            approvalGroupApprovalType = approvalGroupApprovalTypeRepository.GetApprovalGroupApprovalType(approvalGroupApprovalTypeVM.ApprovalGroupApprovalType.ApprovalGroupApprovalTypeId);

            //Check Exists
            if (approvalGroupApprovalType == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Delete Item
            try
            {
                approvalGroupApprovalType.VersionNumber = Int32.Parse(collection["ApprovalGroupApprovalType.VersionNumber"]);
                approvalGroupApprovalTypeRepository.Delete(approvalGroupApprovalType);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/ApprovalGroupApprovalType.mvc/Delete/" + approvalGroupApprovalTypeVM.ApprovalGroupApprovalType.ApprovalGroupApprovalTypeId.ToString();
                    return View("VersionError");
                }
                if (ex.Message == "SQLDeleteError")
                {
                    ViewData["ReturnURL"] = "/ApprovalGroupApprovalType.mvc/Delete/" + approvalGroupApprovalTypeVM.ApprovalGroupApprovalType.ApprovalGroupApprovalTypeId.ToString();
                    return View("DeleteError");
                }

                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            //Return
            return RedirectToAction("List");
        }
    }
}