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
    public class ClientDetailClientSubUnitTravelerTypeController : Controller
    {
        ClientSubUnitTravelerTypeRepository clientSubUnitTravelerTypeRepository = new ClientSubUnitTravelerTypeRepository();
        ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
        TravelerTypeRepository travelerTypeRepository = new TravelerTypeRepository();
        ClientDetailClientSubUnitTravelerTypeRepository clientDetailClientSubUnitTravelerTypeRepository = new ClientDetailClientSubUnitTravelerTypeRepository();
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

            ClientDetailClientSubUnitTravelerType clientDetailClientSubUnitTravelerType = new ClientDetailClientSubUnitTravelerType();
            clientDetailClientSubUnitTravelerType = clientDetailClientSubUnitTravelerTypeRepository.GetClientDetailClientSubUnitTravelerType(id);

            //Check Exists
            if (clientDetailClientSubUnitTravelerType == null)
            {
                ViewData["ActionMethod"] = "ListSubMenu";
                return View("RecordDoesNotExistError");
            }

            string csu = clientDetailClientSubUnitTravelerType.ClientSubUnitGuid;
            string tt = clientDetailClientSubUnitTravelerType.TravelerTypeGuid;

            

            ClientSubUnitTravelerTypeClientDetailVM clientSubUnitTravelerTypeClientDetailVM = new ClientSubUnitTravelerTypeClientDetailVM();
            clientSubUnitTravelerTypeClientDetailVM.ClientDetail = clientDetail;

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu); 
            clientSubUnitTravelerTypeClientDetailVM.ClientSubUnit = clientSubUnit;

            TravelerType travelerType = new TravelerType();
            travelerType = travelerTypeRepository.GetTravelerType(tt);
            clientSubUnitTravelerTypeClientDetailVM.TravelerType = travelerType;

            return View(clientSubUnitTravelerTypeClientDetailVM);
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
            ClientSubUnitTravelerTypeClientDetailsVM clientAccountClientDetailsVM = new ClientSubUnitTravelerTypeClientDetailsVM();
            clientAccountClientDetailsVM.ClientDetails = clientDetailClientSubUnitTravelerTypeRepository.GetClientDetailsByDeletedFlag(csu,tt, filter ?? "", false, sortField, sortOrder ?? 0, page ?? 1);
            
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);
            clientAccountClientDetailsVM.ClientSubUnit = clientSubUnit;

            TravelerType travelerType = new TravelerType();
            travelerType = travelerTypeRepository.GetTravelerType(tt);
            clientAccountClientDetailsVM.TravelerType = travelerType;

            return View(clientAccountClientDetailsVM);
        }

        // GET: /ListUnDeleted
        public ActionResult ListDeleted(string csu, string tt, string filter, int? page, string sortField, int? sortOrder)
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
            ClientSubUnitTravelerTypeClientDetailsVM clientAccountClientDetailsVM = new ClientSubUnitTravelerTypeClientDetailsVM();
            clientAccountClientDetailsVM.ClientDetails = clientDetailClientSubUnitTravelerTypeRepository.GetClientDetailsByDeletedFlag(csu, tt, filter ?? "", true, sortField, sortOrder ?? 0, page ?? 1);

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);
            clientAccountClientDetailsVM.ClientSubUnit = clientSubUnit;

            TravelerType travelerType = new TravelerType();
            travelerType = travelerTypeRepository.GetTravelerType(tt);
            clientAccountClientDetailsVM.TravelerType = travelerType;

            return View(clientAccountClientDetailsVM);
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
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(csu))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ClientSubUnitTravelerTypeClientDetailVM clientSubUnitTravelerTypeClientDetailVM = new ClientSubUnitTravelerTypeClientDetailVM();

            TripTypeRepository tripTypeRepository = new TripTypeRepository(); 
            clientSubUnitTravelerTypeClientDetailVM.TripTypes = new SelectList(tripTypeRepository.GetAllTripTypes().ToList(), "TripTypeId", "TripTypeDescription");

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);
            clientSubUnitTravelerTypeClientDetailVM.ClientSubUnit = clientSubUnit;

            TravelerType travelerType = new TravelerType();
            travelerType = travelerTypeRepository.GetTravelerType(tt);
            clientSubUnitTravelerTypeClientDetailVM.TravelerType = travelerType;

            return View(clientSubUnitTravelerTypeClientDetailVM);
        }

        
        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClientSubUnitTravelerTypeClientDetailVM clientSubUnitTravelerTypeClientDetailVM)
        {

            string csu = clientSubUnitTravelerTypeClientDetailVM.ClientSubUnit.ClientSubUnitGuid;
            string tt = clientSubUnitTravelerTypeClientDetailVM.TravelerType.TravelerTypeGuid;

            //Check Exists
            ClientSubUnitTravelerType clientSubUnitTravelerType = new ClientSubUnitTravelerType();
            clientSubUnitTravelerType = clientSubUnitTravelerTypeRepository.GetClientSubUnitTravelerType(csu, tt);
            if (clientSubUnitTravelerType == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
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
                UpdateModel(clientSubUnitTravelerTypeClientDetailVM.ClientDetail);
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
                clientDetailClientSubUnitTravelerTypeRepository.Add(clientSubUnitTravelerTypeClientDetailVM);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }
            ViewData["NewSortOrder"] = 0;
            return RedirectToAction("ListUnDeleted", new { csu = csu, tt=tt});
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
            string csu = clientDetail.ClientSubUnitGuid;
            string tt = clientDetail.TravelerTypeGuid;

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

            ClientSubUnitTravelerTypeClientDetailVM clientSubUnitTravelerTypeClientDetailVM = new ClientSubUnitTravelerTypeClientDetailVM();
            clientSubUnitTravelerTypeClientDetailVM.ClientDetail = clientDetail;

            TripTypeRepository tripTypeRepository = new TripTypeRepository();
            clientSubUnitTravelerTypeClientDetailVM.TripTypes = new SelectList(tripTypeRepository.GetAllTripTypes().ToList(), "TripTypeId", "TripTypeDescription", clientDetail.TripTypeId);

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);
            clientSubUnitTravelerTypeClientDetailVM.ClientSubUnit = clientSubUnit;

            TravelerType travelerType = new TravelerType();
            travelerType = travelerTypeRepository.GetTravelerType(tt);
            clientSubUnitTravelerTypeClientDetailVM.TravelerType = travelerType;

            return View(clientSubUnitTravelerTypeClientDetailVM);
        }
        
        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ClientSubUnitTravelerTypeClientDetailVM clientSubUnitTravelerTypeClientDetailVM, FormCollection collection)
        {
            string csu = clientSubUnitTravelerTypeClientDetailVM.ClientSubUnit.ClientSubUnitGuid;
            string tt = clientSubUnitTravelerTypeClientDetailVM.TravelerType.TravelerTypeGuid;

            //Check Exists
            ClientSubUnitTravelerType clientSubUnitTravelerType = new ClientSubUnitTravelerType();
            clientSubUnitTravelerType = clientSubUnitTravelerTypeRepository.GetClientSubUnitTravelerType(csu, tt);
            if (clientSubUnitTravelerType == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            int id = clientSubUnitTravelerTypeClientDetailVM.ClientDetail.ClientDetailId;
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
                UpdateModel(clientSubUnitTravelerTypeClientDetailVM);
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
                clientSubUnitTravelerTypeClientDetailVM.ClientDetail.VersionNumber = Int32.Parse(collection["ClientDetail.VersionNumber"]);
                clientDetailClientSubUnitTravelerTypeRepository.Edit(clientSubUnitTravelerTypeClientDetailVM);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/ClientDetailClientSubUnitTravelerType.mvc/Edit?id=" + id + "&csu=" + csu +"&tt=" + tt;
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
		public ActionResult Delete(int id)
        {
            //Get Item
            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(id);

            //Check Exists
            if (clientDetail == null || clientDetail.DeletedFlag == true)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }
            clientDetailRepository.EditGroupForDisplay(clientDetail);
            string csu = clientDetail.ClientSubUnitGuid;
            string tt = clientDetail.TravelerTypeGuid;

            //Check Exists
            ClientSubUnitTravelerType clientSubUnitTravelerType = new ClientSubUnitTravelerType();
            clientSubUnitTravelerType = clientSubUnitTravelerTypeRepository.GetClientSubUnitTravelerType(csu, tt);
            if (clientSubUnitTravelerType == null)
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


            ClientSubUnitTravelerTypeClientDetailVM clientSubUnitTravelerTypeClientDetailVM = new ClientSubUnitTravelerTypeClientDetailVM();
            clientSubUnitTravelerTypeClientDetailVM.ClientDetail = clientDetail;

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);
            clientSubUnitTravelerTypeClientDetailVM.ClientSubUnit = clientSubUnit;

            TravelerType travelerType = new TravelerType();
            travelerType = travelerTypeRepository.GetTravelerType(tt);
            clientSubUnitTravelerTypeClientDetailVM.TravelerType = travelerType;

            return View(clientSubUnitTravelerTypeClientDetailVM);
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
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }
            clientDetailRepository.EditGroupForDisplay(clientDetail);
            string csu = clientDetail.ClientSubUnitGuid;
            string tt = clientDetail.TravelerTypeGuid;

            //Check Exists
            ClientSubUnitTravelerType clientSubUnitTravelerType = new ClientSubUnitTravelerType();
            clientSubUnitTravelerType = clientSubUnitTravelerTypeRepository.GetClientSubUnitTravelerType(csu, tt);
            if (clientSubUnitTravelerType == null)
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
                    ViewData["ReturnURL"] = "/ClientDetailClientSubUnitTravelerType.mvc/Delete?id=" + id + "&csu=" + csu + "&tt=" + tt;
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
        public ActionResult UnDelete(int id)
        {
            //Get Item
            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(id);

            //Check Exists
            if (clientDetail == null || clientDetail.DeletedFlag == false)
            {
                ViewData["ActionMethod"] = "UnDeleteGet";
                return View("RecordDoesNotExistError");
            }
            clientDetailRepository.EditGroupForDisplay(clientDetail);
            string csu = clientDetail.ClientSubUnitGuid;
            string tt = clientDetail.TravelerTypeGuid;

            //Check Exists
            ClientSubUnitTravelerType clientSubUnitTravelerType = new ClientSubUnitTravelerType();
            clientSubUnitTravelerType = clientSubUnitTravelerTypeRepository.GetClientSubUnitTravelerType(csu, tt);
            if (clientSubUnitTravelerType == null)
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


            ClientSubUnitTravelerTypeClientDetailVM clientSubUnitTravelerTypeClientDetailVM = new ClientSubUnitTravelerTypeClientDetailVM();
            clientSubUnitTravelerTypeClientDetailVM.ClientDetail = clientDetail;

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);
            clientSubUnitTravelerTypeClientDetailVM.ClientSubUnit = clientSubUnit;

            TravelerType travelerType = new TravelerType();
            travelerType = travelerTypeRepository.GetTravelerType(tt);
            clientSubUnitTravelerTypeClientDetailVM.TravelerType = travelerType;

            return View(clientSubUnitTravelerTypeClientDetailVM);
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
                ViewData["ActionMethod"] = "UnDeleteGet";
                return View("RecordDoesNotExistError");
            }
            clientDetailRepository.EditGroupForDisplay(clientDetail);
            string csu = clientDetail.ClientSubUnitGuid;
            string tt = clientDetail.TravelerTypeGuid;

            //Check Exists
            ClientSubUnitTravelerType clientSubUnitTravelerType = new ClientSubUnitTravelerType();
            clientSubUnitTravelerType = clientSubUnitTravelerTypeRepository.GetClientSubUnitTravelerType(csu, tt);
            if (clientSubUnitTravelerType == null)
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
                    ViewData["ReturnURL"] = "/ClientDetailClientSubUnitTravelerType.mvc/UnDelete?id=" + id + "&csu=" + csu + "&tt=" + tt;
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
            string csu = clientDetail.ClientSubUnitGuid;
            string tt = clientDetail.TravelerTypeGuid;

            //Check Exists
            ClientSubUnitTravelerType clientSubUnitTravelerType = new ClientSubUnitTravelerType();
            clientSubUnitTravelerType = clientSubUnitTravelerTypeRepository.GetClientSubUnitTravelerType(csu, tt);
            if (clientSubUnitTravelerType == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            ClientSubUnitTravelerTypeClientDetailVM clientSubUnitTravelerTypeClientDetailVM = new ClientSubUnitTravelerTypeClientDetailVM();
            clientSubUnitTravelerTypeClientDetailVM.ClientDetail = clientDetail;

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);
            clientSubUnitTravelerTypeClientDetailVM.ClientSubUnit = clientSubUnit;

            TravelerType travelerType = new TravelerType();
            travelerType = travelerTypeRepository.GetTravelerType(tt);
            clientSubUnitTravelerTypeClientDetailVM.TravelerType = travelerType;

            return View(clientSubUnitTravelerTypeClientDetailVM);
        }
    }
}
