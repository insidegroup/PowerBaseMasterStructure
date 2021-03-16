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
    public class ClientDetailTravelerTypeController : Controller
    {
        ClientSubUnitTravelerTypeRepository clientSubUnitTravelerTypeRepository = new ClientSubUnitTravelerTypeRepository();
        TravelerTypeRepository travelerTypeRepository = new TravelerTypeRepository();
        ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
        ClientDetailTravelerTypeRepository clientDetailTravelerTypeRepository = new ClientDetailTravelerTypeRepository();
        ClientDetailRepository clientDetailRepository = new ClientDetailRepository();
        HierarchyRepository hierarchyRepository = new HierarchyRepository();

        // GET: /ListSubMenu
        public ActionResult ListSubMenu(int id, string tt, string csu)
        {
            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(id);

            //Check Exists
            if (clientDetail == null)
            {
                ViewData["ActionMethod"] = "ListSubMenu";
                return View("RecordDoesNotExistError");
            }

            ClientDetailTravelerType clientDetailTravelerType = new ClientDetailTravelerType();
            clientDetailTravelerType = clientDetailTravelerTypeRepository.GetClientDetailTravelerType(id);

            //Check Exists
            if (clientDetailTravelerType == null)
            {
                ViewData["ActionMethod"] = "ListSubMenu";
                return View("RecordDoesNotExistError");
            }

            TravelerType travelerType = new TravelerType();
            travelerType = travelerTypeRepository.GetTravelerType(clientDetailTravelerType.TravelerTypeGuid);

            TravelerTypeClientDetailVM travelerTypeClientDetailVM = new TravelerTypeClientDetailVM();
            travelerTypeClientDetailVM.TravelerType = travelerType;
            travelerTypeClientDetailVM.ClientDetail = clientDetail;

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);
            travelerTypeClientDetailVM.ClientSubUnit = clientSubUnit;

            return View(travelerTypeClientDetailVM);
        }


        // GET: /ListUnDeleted
        public ActionResult ListUnDeleted(string csu, string tt, string filter, int? page, string sortField, int? sortOrder)
        {
            ClientSubUnitTravelerType clientSubUnitTravelerType = new ClientSubUnitTravelerType();
            clientSubUnitTravelerType = clientSubUnitTravelerTypeRepository.GetClientSubUnitTravelerType(csu, tt);

            //Check Exists
            if (clientSubUnitTravelerType == null)
            {
                ViewData["ActionMethod"] = "ListUnDeleted";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToClientSubUnit(csu))
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
            TravelerTypeClientDetailsVM travelerTypeClientDetailsVM = new TravelerTypeClientDetailsVM();
            TravelerType travelerType = new TravelerType();
            travelerType = travelerTypeRepository.GetTravelerType(tt);
            travelerTypeClientDetailsVM.TravelerType = travelerType;

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);
            travelerTypeClientDetailsVM.ClientSubUnit = clientSubUnit;

            travelerTypeClientDetailsVM.ClientDetails = clientDetailTravelerTypeRepository.GetClientDetailsByDeletedFlag(tt, filter ?? "", false, sortField, sortOrder ?? 0, page ?? 1);
            
            return View(travelerTypeClientDetailsVM);
        }

        // GET: /ListDeleted
        public ActionResult ListDeleted(string csu, string tt, string filter, int? page, string sortField, int? sortOrder)
        {
            ClientSubUnitTravelerType clientSubUnitTravelerType = new ClientSubUnitTravelerType();
            clientSubUnitTravelerType = clientSubUnitTravelerTypeRepository.GetClientSubUnitTravelerType(csu, tt);

            //Check Exists
            if (clientSubUnitTravelerType == null)
            {
                ViewData["ActionMethod"] = "ListDeleted";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToClientSubUnit(csu))
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
            TravelerTypeClientDetailsVM travelerTypeClientDetailsVM = new TravelerTypeClientDetailsVM();
            TravelerType travelerType = new TravelerType();
            travelerType = travelerTypeRepository.GetTravelerType(tt);
            travelerTypeClientDetailsVM.TravelerType = travelerType;

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);
            travelerTypeClientDetailsVM.ClientSubUnit = clientSubUnit;

            travelerTypeClientDetailsVM.ClientDetails = clientDetailTravelerTypeRepository.GetClientDetailsByDeletedFlag(tt, filter ?? "", true, sortField, sortOrder ?? 0, page ?? 1);

            return View(travelerTypeClientDetailsVM);
        }

        // GET: /Create
        public ActionResult Create(string csu, string tt)
        {
            ClientSubUnitTravelerType clientSubUnitTravelerType = new ClientSubUnitTravelerType();
            clientSubUnitTravelerType = clientSubUnitTravelerTypeRepository.GetClientSubUnitTravelerType(csu, tt);

            //Check Exists
            if (clientSubUnitTravelerType == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToClientSubUnit(csu))
            {
                ViewData["Access"] = "WriteAccess";
            }

            

            TravelerTypeClientDetailVM travelerTypeClientDetailVM = new TravelerTypeClientDetailVM();
            TravelerType travelerType = new TravelerType();
            travelerType = travelerTypeRepository.GetTravelerType(tt);
            travelerTypeClientDetailVM.TravelerType = travelerType;

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);
            travelerTypeClientDetailVM.ClientSubUnit = clientSubUnit;

            TripTypeRepository tripTypeRepository = new TripTypeRepository();
            travelerTypeClientDetailVM.TripTypes = new SelectList(tripTypeRepository.GetAllTripTypes().ToList(), "TripTypeId", "TripTypeDescription");
            

            return View(travelerTypeClientDetailVM);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TravelerTypeClientDetailVM travelerTypeClientDetailVM)
        {
            string tt = travelerTypeClientDetailVM.TravelerType.TravelerTypeGuid;
            string csu = travelerTypeClientDetailVM.ClientSubUnit.ClientSubUnitGuid;

            ClientSubUnitTravelerType clientSubUnitTravelerType = new ClientSubUnitTravelerType();
            clientSubUnitTravelerType = clientSubUnitTravelerTypeRepository.GetClientSubUnitTravelerType(csu, tt);

            //Check Exists
            if (clientSubUnitTravelerType == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToClientSubUnit(csu))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //Update Model From Form + Validate against DB
            try
            {
                UpdateModel(travelerTypeClientDetailVM.ClientDetail);
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
                clientDetailTravelerTypeRepository.Add(travelerTypeClientDetailVM);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }
            ViewData["NewSortOrder"] = 0;
            return RedirectToAction("ListUnDeleted", new { csu=csu , tt=tt });
        }

        // GET: /Edit
        public ActionResult Edit(string csu, string tt, int id)
        {
            //always use this
            ClientDetailTravelerType clientDetailTravelerType = new ClientDetailTravelerType();
            clientDetailTravelerType = clientDetailTravelerTypeRepository.GetClientDetailTravelerType(id);

            //Check Exists
            if (clientDetailTravelerType == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            ClientSubUnitTravelerType clientSubUnitTravelerType = new ClientSubUnitTravelerType();
            clientSubUnitTravelerType = clientSubUnitTravelerTypeRepository.GetClientSubUnitTravelerType(csu,tt);

            //Check Exists
            if (clientSubUnitTravelerType == null)
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

            TravelerTypeClientDetailVM travelerTypeClientDetailVM = new TravelerTypeClientDetailVM();
            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(id);
            travelerTypeClientDetailVM.ClientDetail = clientDetail;

            TravelerType travelerType = new TravelerType();
            travelerType = travelerTypeRepository.GetTravelerType(tt);
            travelerTypeClientDetailVM.TravelerType = travelerType;

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);
            travelerTypeClientDetailVM.ClientSubUnit = clientSubUnit;

            TripTypeRepository tripTypeRepository = new TripTypeRepository();
            travelerTypeClientDetailVM.TripTypes = new SelectList(tripTypeRepository.GetAllTripTypes().ToList(), "TripTypeId", "TripTypeDescription", clientDetail.TripTypeId);
            

            return View(travelerTypeClientDetailVM);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TravelerTypeClientDetailVM travelerTypeClientDetailVM, FormCollection collection)
        {
            int id = travelerTypeClientDetailVM.ClientDetail.ClientDetailId;
            ClientDetailTravelerType clientDetailTravelerType = new ClientDetailTravelerType();
            clientDetailTravelerType = clientDetailTravelerTypeRepository.GetClientDetailTravelerType(id);

            //Check Exists
            if (clientDetailTravelerType == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            string csu = travelerTypeClientDetailVM.ClientSubUnit.ClientSubUnitGuid;
            string tt = travelerTypeClientDetailVM.TravelerType.TravelerTypeGuid;
            ClientSubUnitTravelerType clientSubUnitTravelerType = new ClientSubUnitTravelerType();
            clientSubUnitTravelerType = clientSubUnitTravelerTypeRepository.GetClientSubUnitTravelerType(csu, tt);

            //Check Exists
            if (clientSubUnitTravelerType == null)
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
                UpdateModel(travelerTypeClientDetailVM);
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
                travelerTypeClientDetailVM.ClientDetail.VersionNumber = Int32.Parse(collection["ClientDetail.VersionNumber"]);
                clientDetailTravelerTypeRepository.Edit(travelerTypeClientDetailVM);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/ClientDetailTravelerType.mvc/Edit?id=" + id + "&tt=" + tt + "&csu=" + csu;
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }
            return RedirectToAction("ListUnDeleted", new { csu = csu, tt=tt });
        }

        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(string csu, string tt, int id)
        {
            //always use this
            ClientDetailTravelerType clientDetailTravelerType = new ClientDetailTravelerType();
            clientDetailTravelerType = clientDetailTravelerTypeRepository.GetClientDetailTravelerType(id);

            //Check Exists
            if (clientDetailTravelerType == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            ClientSubUnitTravelerType clientSubUnitTravelerType = new ClientSubUnitTravelerType();
            clientSubUnitTravelerType = clientSubUnitTravelerTypeRepository.GetClientSubUnitTravelerType(csu, tt);

            //Check Exists
            if (clientSubUnitTravelerType == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            //Get Item
            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(id);

            //Check Exists
            if (clientDetail == null || clientDetail.DeletedFlag == true)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(csu))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            TravelerTypeClientDetailVM travelerTypeClientDetailVM = new TravelerTypeClientDetailVM();
            clientDetailRepository.EditGroupForDisplay(clientDetail);
            travelerTypeClientDetailVM.ClientDetail = clientDetail;

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);
            travelerTypeClientDetailVM.ClientSubUnit = clientSubUnit;

            TravelerType travelerType = new TravelerType();
            travelerType = travelerTypeRepository.GetTravelerType(tt);
            travelerTypeClientDetailVM.TravelerType = travelerType;

            return View(travelerTypeClientDetailVM);
        }


        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(TravelerTypeClientDetailVM travelerTypeClientDetailVM, FormCollection collection)
        {
            int id = travelerTypeClientDetailVM.ClientDetail.ClientDetailId;
            ClientDetailTravelerType clientDetailTravelerType = new ClientDetailTravelerType();
            clientDetailTravelerType = clientDetailTravelerTypeRepository.GetClientDetailTravelerType(id);

            //Check Exists
            if (clientDetailTravelerType == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            string csu = travelerTypeClientDetailVM.ClientSubUnit.ClientSubUnitGuid;
            string tt = travelerTypeClientDetailVM.TravelerType.TravelerTypeGuid;
            ClientSubUnitTravelerType clientSubUnitTravelerType = new ClientSubUnitTravelerType();
            clientSubUnitTravelerType = clientSubUnitTravelerTypeRepository.GetClientSubUnitTravelerType(csu, tt);

            //Check Exists
            if (clientSubUnitTravelerType == null)
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

            //Get Item
            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(id);

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
                    ViewData["ReturnURL"] = "/ClientDetailTravelerType.mvc/Delete?id=" + id + "&tt=" + tt + "&csu=" + csu;
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("ListUnDeleted", new { csu = csu, tt=tt });
        }

        // GET: /UnDelete
        public ActionResult UnDelete(string csu, string tt, int id)
        {
            ClientDetailTravelerType clientDetailTravelerType = new ClientDetailTravelerType();
            clientDetailTravelerType = clientDetailTravelerTypeRepository.GetClientDetailTravelerType(id);

            //Check Exists
            if (clientDetailTravelerType == null)
            {
                ViewData["ActionMethod"] = "UnDeleteGet";
                return View("RecordDoesNotExistError");
            }

            ClientSubUnitTravelerType clientSubUnitTravelerType = new ClientSubUnitTravelerType();
            clientSubUnitTravelerType = clientSubUnitTravelerTypeRepository.GetClientSubUnitTravelerType(csu, tt);

            //Check Exists
            if (clientSubUnitTravelerType == null)
            {
                ViewData["ActionMethod"] = "UnDeleteGet";
                return View("RecordDoesNotExistError");
            }

            //Get Item
            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(id);

            //Check Exists
            if (clientDetail == null || clientDetail.DeletedFlag == false)
            {
                ViewData["ActionMethod"] = "UnDeleteGet";
                return View("RecordDoesNotExistError");
            }

            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(csu))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            TravelerTypeClientDetailVM travelerTypeClientDetailVM = new TravelerTypeClientDetailVM();
            clientDetailRepository.EditGroupForDisplay(clientDetail);
            travelerTypeClientDetailVM.ClientDetail = clientDetail;

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);
            travelerTypeClientDetailVM.ClientSubUnit = clientSubUnit;

            TravelerType travelerType = new TravelerType();
            travelerType = travelerTypeRepository.GetTravelerType(tt);
            travelerTypeClientDetailVM.TravelerType = travelerType;

            return View(travelerTypeClientDetailVM);
        }

        // POST: /UnDelete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UnDelete(TravelerTypeClientDetailVM travelerTypeClientDetailVM, FormCollection collection)
        {
            int id = travelerTypeClientDetailVM.ClientDetail.ClientDetailId;
            ClientDetailTravelerType clientDetailTravelerType = new ClientDetailTravelerType();
            clientDetailTravelerType = clientDetailTravelerTypeRepository.GetClientDetailTravelerType(id);

            //Check Exists
            if (clientDetailTravelerType == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            string csu = travelerTypeClientDetailVM.ClientSubUnit.ClientSubUnitGuid;
            string tt = travelerTypeClientDetailVM.TravelerType.TravelerTypeGuid;
            ClientSubUnitTravelerType clientSubUnitTravelerType = new ClientSubUnitTravelerType();
            clientSubUnitTravelerType = clientSubUnitTravelerTypeRepository.GetClientSubUnitTravelerType(csu, tt);

            //Check Exists
            if (clientSubUnitTravelerType == null)
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

            //Get Item
            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(id);

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
                    ViewData["ReturnURL"] = "/ClientDetailTravelerType.mvc/UnDelete?id=" + id + "&tt=" + tt +"&csu=" + csu;
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("ListDeleted", new { csu = csu, tt = tt });
        }

        // GET: /View
        public ActionResult View(string csu, string tt, int id)
        {
            ClientDetailTravelerType clientDetailTravelerType = new ClientDetailTravelerType();
            clientDetailTravelerType = clientDetailTravelerTypeRepository.GetClientDetailTravelerType(id);

            //Check Exists
            if (clientDetailTravelerType == null)
            {
                ViewData["ActionMethod"] = "UnDeleteGet";
                return View("RecordDoesNotExistError");
            }

            ClientSubUnitTravelerType clientSubUnitTravelerType = new ClientSubUnitTravelerType();
            clientSubUnitTravelerType = clientSubUnitTravelerTypeRepository.GetClientSubUnitTravelerType(csu, tt);

            //Check Exists
            if (clientSubUnitTravelerType == null)
            {
                ViewData["ActionMethod"] = "UnDeleteGet";
                return View("RecordDoesNotExistError");
            }

            //Get Item
            TravelerTypeClientDetailVM travelerTypeClientDetailVM = new TravelerTypeClientDetailVM();
            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(id);
            clientDetailRepository.EditGroupForDisplay(clientDetail); 
            travelerTypeClientDetailVM.ClientDetail = clientDetail;

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);
            travelerTypeClientDetailVM.ClientSubUnit = clientSubUnit;

            TravelerType travelerType = new TravelerType();
            travelerType = travelerTypeRepository.GetTravelerType(tt);
            travelerTypeClientDetailVM.TravelerType = travelerType;

            return View(travelerTypeClientDetailVM);
        }
    }
}
