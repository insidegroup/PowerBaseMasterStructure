using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Repository;
using CWTDesktopDatabase.Helpers;
using System.Data.SqlClient;

namespace CWTDesktopDatabase.Controllers
{
    public class ClientDetailClientSubUnitController : Controller
    {
        ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
        ClientDetailClientSubUnitRepository clientDetailClientSubUnitRepository = new ClientDetailClientSubUnitRepository();
        ClientDetailRepository clientDetailRepository = new ClientDetailRepository();
        HierarchyRepository hierarchyRepository = new HierarchyRepository();

        // GET: /ListSubMenu
        public ActionResult ListSubMenu(int id)
        {
            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(id);

            //Check Exists
            if (clientDetail == null)
            {
                ViewData["ActionMethod"] = "ListSubMenu";
                return View("RecordDoesNotExistError");
            }

            ClientDetailClientSubUnit clientDetailClientSubUnit = new ClientDetailClientSubUnit();
            clientDetailClientSubUnit = clientDetailClientSubUnitRepository.GetClientDetailClientSubUnit(id);

            //Check Exists
            if (clientDetailClientSubUnit == null)
            {
                ViewData["ActionMethod"] = "ListSubMenu";
                return View("RecordDoesNotExistError");
            }

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientDetailClientSubUnit.ClientSubUnitGuid);

            ClientSubUnitClientDetailVM clientAccountClientDetailVM = new ClientSubUnitClientDetailVM();
            clientAccountClientDetailVM.ClientSubUnit = clientSubUnit;
            clientAccountClientDetailVM.ClientDetail = clientDetail;

            return View(clientAccountClientDetailVM);
        }


        // GET: /ListUnDeleted
        public ActionResult ListUnDeleted(string id, string filter, int? page, string sortField, int? sortOrder)
        {
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(id);

            //Check Exists
            if (clientSubUnit == null)
            {
                ViewData["ActionMethod"] = "ListUnDeleted";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToClientSubUnit(id))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //SortField
            if (sortField !="EnabledDate")
            {
                sortField = "ClientDetailName";
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
            ClientSubUnitClientDetailsVM clientSubUnitClientDetailsVM = new ClientSubUnitClientDetailsVM();
            clientSubUnitClientDetailsVM.ClientDetails = clientDetailClientSubUnitRepository.GetClientDetailsByDeletedFlag(id, filter ?? "", false, sortField, sortOrder ?? 0, page ?? 1);
            clientSubUnitClientDetailsVM.ClientSubUnit = clientSubUnit;
            return View(clientSubUnitClientDetailsVM);
        }

        // GET: /ListUnDeleted
        public ActionResult ListDeleted(string id, string filter, int? page, string sortField, int? sortOrder)
        {
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(id);

            //Check Exists
            if (clientSubUnit == null)
            {
                ViewData["ActionMethod"] = "ListUnDeleted";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToClientSubUnit(id))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //SortField
            if (sortField != "EnabledDate")
            {
                sortField = "ClientDetailName";
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
            ClientSubUnitClientDetailsVM clientSubUnitClientDetailsVM = new ClientSubUnitClientDetailsVM();
            clientSubUnitClientDetailsVM.ClientDetails = clientDetailClientSubUnitRepository.GetClientDetailsByDeletedFlag(id, filter ?? "", true, sortField, sortOrder ?? 0, page ?? 1);
            clientSubUnitClientDetailsVM.ClientSubUnit = clientSubUnit;
            return View(clientSubUnitClientDetailsVM);
        }
        
        // GET: /Create
        public ActionResult Create(string id)
        {
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(id);

            //Check Exists
            if (clientSubUnit == null)
            {
                ViewData["ActionMethod"] = "ListUnDeleted";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(id))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ClientSubUnitClientDetailVM clientAccountClientDetailVM = new ClientSubUnitClientDetailVM();

            TripTypeRepository tripTypeRepository = new TripTypeRepository(); 
            clientAccountClientDetailVM.TripTypes = new SelectList(tripTypeRepository.GetAllTripTypes().ToList(), "TripTypeId", "TripTypeDescription");
            clientAccountClientDetailVM.ClientSubUnit = clientSubUnit;

            return View(clientAccountClientDetailVM);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClientSubUnitClientDetailVM clientAccountClientDetailVM)
        {

            string clientSubUnitGuid = clientAccountClientDetailVM.ClientSubUnit.ClientSubUnitGuid;

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnitGuid))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update Model From Form + Validate against DB
            try
            {
                UpdateModel(clientAccountClientDetailVM.ClientDetail);
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
                clientDetailClientSubUnitRepository.Add(clientAccountClientDetailVM);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }
            ViewData["NewSortOrder"] = 0;
            return RedirectToAction("ListUnDeleted", new { id = clientSubUnitGuid });
        }

        // GET: /Edit
        public ActionResult Edit(int id)
        {
            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(id);

            //Check Exists
            if (clientDetail == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }
            clientDetailRepository.EditGroupForDisplay(clientDetail);
            string clientSubUnitGuid = clientDetail.HierarchyCode;

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnitGuid))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ClientSubUnitClientDetailVM clientAccountClientDetailVM = new ClientSubUnitClientDetailVM();
            clientAccountClientDetailVM.ClientDetail = clientDetail;

            TripTypeRepository tripTypeRepository = new TripTypeRepository();
            clientAccountClientDetailVM.TripTypes = new SelectList(tripTypeRepository.GetAllTripTypes().ToList(), "TripTypeId", "TripTypeDescription",clientDetail.TripTypeId);

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientSubUnitGuid); 
            clientAccountClientDetailVM.ClientSubUnit = clientSubUnit;

            return View(clientAccountClientDetailVM);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ClientSubUnitClientDetailVM clientAccountClientDetailVM, FormCollection collection)
        {
            string csu = clientAccountClientDetailVM.ClientSubUnit.ClientSubUnitGuid;

            //Check Exists
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);
            if (clientSubUnit == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            int id = clientAccountClientDetailVM.ClientDetail.ClientDetailId;
            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(id);

            //Check Exists
            if (clientDetail == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(csu))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update Model From Form + Validate against DB
            try
            {
                UpdateModel(clientAccountClientDetailVM);
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
                clientAccountClientDetailVM.ClientDetail.VersionNumber = Int32.Parse(collection["ClientDetail.VersionNumber"]);
                clientDetailClientSubUnitRepository.Edit(clientAccountClientDetailVM);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/ClientDetailClientSubUnit.mvc/Edit?id=" + id.ToString();
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }
            return RedirectToAction("ListUnDeleted", new { id = csu });
        }

        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id)
        {
            //Get Item
            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(id);

            //Check Exists
            if (clientDetail == null || clientDetail.DeletedFlag == true)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }
            clientDetailRepository.EditGroupForDisplay(clientDetail);
            string clientSubUnitGuid = clientDetail.HierarchyCode;

            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnitGuid))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }


            ClientSubUnitClientDetailVM clientAccountClientDetailVM = new ClientSubUnitClientDetailVM();
            clientDetailRepository.EditGroupForDisplay(clientDetail); 
            clientAccountClientDetailVM.ClientDetail = clientDetail;

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientSubUnitGuid);
            clientAccountClientDetailVM.ClientSubUnit = clientSubUnit;

            return View(clientAccountClientDetailVM);

            
        }


        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            //Get Item
            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(id);

            //Check Exists
            if (clientDetail == null || clientDetail.DeletedFlag == true)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }
            clientDetailRepository.EditGroupForDisplay(clientDetail);
            string clientSubUnitGuid = clientDetail.HierarchyCode;

            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnitGuid))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Delete Item
            try
            {
                clientDetail.VersionNumber = Int32.Parse(collection["ClientDetail.VersionNumber"]);
                clientDetail.DeletedFlag = true;
                clientDetailRepository.UpdateGroupDeletedStatus(clientDetail);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/ClientDetailClientSubUnit.mvc/Delete?id=" + id.ToString();
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("ListUnDeleted", new { id = clientSubUnitGuid });
        }

        // GET: /UnDelete
        public ActionResult UnDelete(int id)
        {
            //Get Item
            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(id);

            //Check Exists
            if (clientDetail == null || clientDetail.DeletedFlag == false)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }
            clientDetailRepository.EditGroupForDisplay(clientDetail);
            string clientSubUnitGuid = clientDetail.HierarchyCode;

            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnitGuid))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }


            ClientSubUnitClientDetailVM clientAccountClientDetailVM = new ClientSubUnitClientDetailVM();
            clientDetailRepository.EditGroupForDisplay(clientDetail);
            clientAccountClientDetailVM.ClientDetail = clientDetail;

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientSubUnitGuid);
            clientAccountClientDetailVM.ClientSubUnit = clientSubUnit;

            return View(clientAccountClientDetailVM);


        }

        // POST: /UnDelete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UnDelete(int id, FormCollection collection)
        {
            //Get Item
            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(id);

            //Check Exists
            if (clientDetail == null || clientDetail.DeletedFlag == false)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }
            clientDetailRepository.EditGroupForDisplay(clientDetail);
            string clientSubUnitGuid = clientDetail.HierarchyCode;

            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnitGuid))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Delete Item
            try
            {
                clientDetail.VersionNumber = Int32.Parse(collection["ClientDetail.VersionNumber"]);
                clientDetail.DeletedFlag = false;
                clientDetailRepository.UpdateGroupDeletedStatus(clientDetail);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/ClientDetailClientSubUnit.mvc/UnDelete/" + id.ToString();
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("ListDeleted", new { id = clientSubUnitGuid });
        }

            // GET: /View
        public ActionResult View(int id)
        {
            //Get Item
            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(id);

            //Check Exists
            if (clientDetail == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            clientDetailRepository.EditGroupForDisplay(clientDetail);
            string csu = clientDetail.HierarchyCode;

            //Check Exists
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);
            if (clientSubUnit == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            ClientSubUnitClientDetailVM clientSubUnitClientDetailVM = new ClientSubUnitClientDetailVM();
            clientSubUnitClientDetailVM.ClientDetail = clientDetail;


            return View(clientSubUnitClientDetailVM);
        }
    }
}
