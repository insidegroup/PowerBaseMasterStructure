using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Repository;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Models;
using System.Data.SqlClient;

namespace CWTDesktopDatabase.Controllers
{
    public class ClientTopUnitMatrixDPCodeController : Controller
    {
        ClientTopUnitMatrixDPCodeRepository clientTopUnitMatrixDPCodeRepository = new ClientTopUnitMatrixDPCodeRepository();
        ClientTopUnitRepository clientTopUnitRepository = new ClientTopUnitRepository();
        HierarchyRepository hierarchyRepository = new HierarchyRepository();

        private string groupName = "Matrix DP Code Administrator";

        // GET: /List
        public ActionResult List(int? page, string id, string sortField, int? sortOrder)
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
                sortField = "HierarchyItem ";
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
            }

            ClientTopUnitMatrixDPCodesVM clientTopUnitClientTopUnitMatrixDPCodesVM = new ClientTopUnitMatrixDPCodesVM();
            clientTopUnitClientTopUnitMatrixDPCodesVM.MatrixDPCodes = clientTopUnitMatrixDPCodeRepository.PageClientTopUnitMatrixDPCodes(page ?? 1, id, sortField, sortOrder);

            ClientTopUnit clientTopUnit = new ClientTopUnit();
            clientTopUnit = clientTopUnitRepository.GetClientTopUnit(id);

			//Check clientTopUnit
			if (clientTopUnit == null)
			{
				ViewData["ActionMethod"] = "List";
				return View("RecordDoesNotExistError");
			}

            clientTopUnitClientTopUnitMatrixDPCodesVM.ClientTopUnit = clientTopUnit;

            //return items
            return View(clientTopUnitClientTopUnitMatrixDPCodesVM);
        }

        // GET: /Create
        public ActionResult Create(string id)
        {
            //Check Access Rights to Domain
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            SelectList hierarchyTypesList = new SelectList(clientTopUnitMatrixDPCodeRepository.GetClientTopUnitMatrixDPCodeHierarchies(), "Key", "Value");
            ViewData["HierarchyTypes"] = hierarchyTypesList;

            ClientTopUnit clientTopUnit = new ClientTopUnit();
            clientTopUnit = clientTopUnitRepository.GetClientTopUnit(id);

            //Check clientTopUnit
            if (clientTopUnit == null)
            {
                ViewData["ActionMethod"] = "List";
                return View("RecordDoesNotExistError");
            }

            ClientTopUnitMatrixDPCode group = new ClientTopUnitMatrixDPCode();
            group.ClientTopUnit = clientTopUnit;

            return View(group);
        }

        // POST: /Create/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClientTopUnitMatrixDPCode group)
        {
            //Check Access Rights to Domain
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update Model From Form + Validate against DB
            try
            {
                UpdateModel(group);
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
                clientTopUnitMatrixDPCodeRepository.Add(group);
            }
            catch (SqlException ex)
            {
                //Non-Unique Name
                if (ex.Message == "NonUniqueName")
                {
                    return View("NonUniqueNameError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List", new { id = group.ClientTopUnit.ClientTopUnitGuid } );
        }

        // GET: /Edit
        public ActionResult Edit(string id, string hierarchyCode, string hierarchyType)
        {
            ClientTopUnit clientTopUnit = new ClientTopUnit();
            clientTopUnit = clientTopUnitRepository.GetClientTopUnit(id);

            //Check clientTopUnit
            if (clientTopUnit == null)
            {
                ViewData["ActionMethod"] = "List";
                return View("RecordDoesNotExistError");
            }
            
            //Get Item From Database
            ClientTopUnitMatrixDPCode group = new ClientTopUnitMatrixDPCode();
            group = clientTopUnitMatrixDPCodeRepository.GetGroup(hierarchyCode, hierarchyType);

            //Check Exists
            if (group == null)
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

            clientTopUnitMatrixDPCodeRepository.EditGroupForDisplay(group);

            group.ClientTopUnit = clientTopUnit;

            return View(group);
        }

        // POST: /Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, string hierarchyCode, string hierarchyType, FormCollection collection)
        {
            ClientTopUnit clientTopUnit = new ClientTopUnit();
            clientTopUnit = clientTopUnitRepository.GetClientTopUnit(id);

            //Check clientTopUnit
            if (clientTopUnit == null)
            {
                ViewData["ActionMethod"] = "List";
                return View("RecordDoesNotExistError");
            }
            
            //Get Item From Database
            ClientTopUnitMatrixDPCode group = new ClientTopUnitMatrixDPCode();
            group = clientTopUnitMatrixDPCodeRepository.GetGroup(hierarchyCode, hierarchyType);

            //Check Exists
            if (group == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update Model From Form + Validate against DB
            try
            {
                UpdateModel(group);
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
                clientTopUnitMatrixDPCodeRepository.Edit(group);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/ClientTopUnitMatrixDPCode.mvc/Edit/" + group.HierarchyCode.ToString();
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }
            return RedirectToAction("List", new { id = group.ClientTopUnit.ClientTopUnitGuid });
        }

        // GET: /Delete
        [HttpGet]
        public ActionResult Delete(string id, string hierarchyCode, string hierarchyType)
        {
            ClientTopUnit clientTopUnit = new ClientTopUnit();
            clientTopUnit = clientTopUnitRepository.GetClientTopUnit(id);

            //Check clientTopUnit
            if (clientTopUnit == null)
            {
                ViewData["ActionMethod"] = "List";
                return View("RecordDoesNotExistError");
            }
            
            //Get Item From Database
            ClientTopUnitMatrixDPCode group = new ClientTopUnitMatrixDPCode();
            group = clientTopUnitMatrixDPCodeRepository.GetGroup(hierarchyCode, hierarchyType);

            //Check Exists
            if (group == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }
            
            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            clientTopUnitMatrixDPCodeRepository.EditGroupForDisplay(group);

            group.ClientTopUnit = clientTopUnit;

            return View(group);
        }

        // POST: /Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, string hierarchyType, string hierarchyCode, FormCollection collection)
        {
            ClientTopUnit clientTopUnit = new ClientTopUnit();
            clientTopUnit = clientTopUnitRepository.GetClientTopUnit(id);

            //Check clientTopUnit
            if (clientTopUnit == null)
            {
                ViewData["ActionMethod"] = "List";
                return View("RecordDoesNotExistError");
            }
            
            //Get Item From Database
            ClientTopUnitMatrixDPCode group = new ClientTopUnitMatrixDPCode();
            group = clientTopUnitMatrixDPCodeRepository.GetGroup(hierarchyCode, hierarchyType);

            //Check Exists
            if (group == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            group.ClientTopUnit = clientTopUnit;

            //Delete Item
            try
            {
                clientTopUnitMatrixDPCodeRepository.Delete(group);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/ClientTopUnitMatrixDPCode.mvc/Delete/" + group.HierarchyCode;
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List", new { id = group.ClientTopUnit.ClientTopUnitGuid });
        }
    }
}
