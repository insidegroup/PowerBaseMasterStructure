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
    public class ClientDetailClientAccountController : Controller
    {
        ClientAccountRepository clientAccountRepository = new ClientAccountRepository();
        ClientDetailClientAccountRepository clientDetailClientAccountRepository = new ClientDetailClientAccountRepository();
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

            ClientDetailClientAccount clientDetailClientAccount = new ClientDetailClientAccount();
            clientDetailClientAccount = clientDetailClientAccountRepository.GetClientDetailClientAccount(id);

            //Check Exists
            if (clientDetailClientAccount == null)
            {
                ViewData["ActionMethod"] = "ListSubMenu";
                return View("RecordDoesNotExistError");
            }

            ClientAccount clientAccount = new ClientAccount();
            clientAccount = clientAccountRepository.GetClientAccount(clientDetailClientAccount.ClientAccountNumber, clientDetailClientAccount.SourceSystemCode);

            ClientAccountClientDetailVM clientAccountClientDetailVM = new ClientAccountClientDetailVM();
            clientAccountClientDetailVM.ClientAccount = clientAccount;
            clientAccountClientDetailVM.ClientDetail = clientDetail;

            return View(clientAccountClientDetailVM);
        }


        // GET: /ListUnDeleted
        public ActionResult ListUnDeleted(string can, string ssc, string filter, int? page, string sortField, int? sortOrder)
        {
            ClientAccount clientAccount = new ClientAccount();
            clientAccount = clientAccountRepository.GetClientAccount(can, ssc);

            //Check Exists
            if (clientAccount == null)
            {
                ViewData["ActionMethod"] = "ListUnDeleted";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToClientAccount(can, ssc))
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
            ClientAccountClientDetailsVM clientAccountClientDetailsVM = new ClientAccountClientDetailsVM();
            clientAccountClientDetailsVM.ClientDetails = clientDetailClientAccountRepository.GetClientDetailsByDeletedFlag(can, ssc, filter ?? "", false, sortField, sortOrder ?? 0, page ?? 1);
            clientAccountClientDetailsVM.ClientAccount = clientAccount;
            return View(clientAccountClientDetailsVM);
        }

        // GET: /ListUnDeleted
        public ActionResult ListDeleted(string can, string ssc, string filter, int? page, string sortField, int? sortOrder)
        {
            ClientAccount clientAccount = new ClientAccount();
            clientAccount = clientAccountRepository.GetClientAccount(can, ssc);

            //Check Exists
            if (clientAccount == null)
            {
                ViewData["ActionMethod"] = "ListUnDeleted";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToClientAccount(can, ssc))
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
            ClientAccountClientDetailsVM clientAccountClientDetailsVM = new ClientAccountClientDetailsVM();
            clientAccountClientDetailsVM.ClientDetails = clientDetailClientAccountRepository.GetClientDetailsByDeletedFlag(can, ssc, filter ?? "", true, sortField, sortOrder ?? 0, page ?? 1);
            clientAccountClientDetailsVM.ClientAccount = clientAccount;
            return View(clientAccountClientDetailsVM);
        }
        // GET: /Create
        public ActionResult Create(string can, string ssc)
        {
            ClientAccount clientAccount = new ClientAccount();
            clientAccount = clientAccountRepository.GetClientAccount(can, ssc);

            //Check Exists
            if (clientAccount == null)
            {
                ViewData["ActionMethod"] = "ListUnDeleted";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientAccount(can, ssc))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ClientAccountClientDetailVM clientAccountClientDetailVM = new ClientAccountClientDetailVM();

            TripTypeRepository tripTypeRepository = new TripTypeRepository(); 
            clientAccountClientDetailVM.TripTypes = new SelectList(tripTypeRepository.GetAllTripTypes().ToList(), "TripTypeId", "TripTypeDescription");
            clientAccountClientDetailVM.ClientAccount = clientAccount;

            return View(clientAccountClientDetailVM);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClientAccountClientDetailVM clientAccountClientDetailVM)
        {

            string can = clientAccountClientDetailVM.ClientAccount.ClientAccountNumber;
            string ssc = clientAccountClientDetailVM.ClientAccount.SourceSystemCode;

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientAccount(can, ssc))
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
                clientDetailClientAccountRepository.Add(clientAccountClientDetailVM);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }
            ViewData["NewSortOrder"] = 0;
            return RedirectToAction("ListUnDeleted", new { can = can, ssc = ssc});
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
            string can = clientDetail.HierarchyCode;
            string ssc = clientDetail.SourceSystemCode;

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientAccount(can, ssc))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ClientAccountClientDetailVM clientAccountClientDetailVM = new ClientAccountClientDetailVM();
            clientAccountClientDetailVM.ClientDetail = clientDetail;

            TripTypeRepository tripTypeRepository = new TripTypeRepository();
            clientAccountClientDetailVM.TripTypes = new SelectList(tripTypeRepository.GetAllTripTypes().ToList(), "TripTypeId", "TripTypeDescription",clientDetail.TripTypeId);

            ClientAccount clientAccount = new ClientAccount();
            clientAccount = clientAccountRepository.GetClientAccount(can, ssc); 
            clientAccountClientDetailVM.ClientAccount = clientAccount;

            return View(clientAccountClientDetailVM);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ClientAccountClientDetailVM clientAccountClientDetailVM, FormCollection collection)
        {
            string can = clientAccountClientDetailVM.ClientAccount.ClientAccountNumber;
            string ssc = clientAccountClientDetailVM.ClientAccount.SourceSystemCode;

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
            if (!rolesRepository.HasWriteAccessToClientAccount(can, ssc))
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
                clientDetailClientAccountRepository.Edit(clientAccountClientDetailVM);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/ClientDetailClientAccount.mvc/Edit/" + id.ToString();
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }
            return RedirectToAction("ListUnDeleted", new { can = can, ssc = ssc });
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
            string can = clientDetail.HierarchyCode;
            string ssc = clientDetail.SourceSystemCode;

            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientAccount(can,ssc))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }


            ClientAccountClientDetailVM clientAccountClientDetailVM = new ClientAccountClientDetailVM();
            clientDetailRepository.EditGroupForDisplay(clientDetail); 
            clientAccountClientDetailVM.ClientDetail = clientDetail;

            ClientAccount clientAccount = new ClientAccount();
            clientAccount = clientAccountRepository.GetClientAccount(can, ssc);
            clientAccountClientDetailVM.ClientAccount = clientAccount;

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
            string can = clientDetail.HierarchyCode;
            string ssc = clientDetail.SourceSystemCode;

            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientAccount(can, ssc))
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
                    ViewData["ReturnURL"] = "/ClientDetailClientAccount.mvc/Delete/" + id.ToString();
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("ListUnDeleted", new {can =can, ssc=ssc});
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
            string can = clientDetail.HierarchyCode;
            string ssc = clientDetail.SourceSystemCode;

            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientAccount(can, ssc))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }


            ClientAccountClientDetailVM clientAccountClientDetailVM = new ClientAccountClientDetailVM();
            clientDetailRepository.EditGroupForDisplay(clientDetail);
            clientAccountClientDetailVM.ClientDetail = clientDetail;

            ClientAccount clientAccount = new ClientAccount();
            clientAccount = clientAccountRepository.GetClientAccount(can, ssc);
            clientAccountClientDetailVM.ClientAccount = clientAccount;

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
            string can = clientDetail.HierarchyCode;
            string ssc = clientDetail.SourceSystemCode;

            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientAccount(can, ssc))
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
                    ViewData["ReturnURL"] = "/ClientDetailClientAccount.mvc/UnDelete/" + id.ToString();
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("ListDeleted", new { can = can, ssc = ssc });
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
            string can = clientDetail.HierarchyCode;
            string ssc = clientDetail.SourceSystemCode;

            //Check Exists
            ClientAccount clientAccount = new ClientAccount();
            clientAccount = clientAccountRepository.GetClientAccount(can, ssc);
            if (clientAccount == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            ClientAccountClientDetailVM clientAccountClientDetailVM = new ClientAccountClientDetailVM();
            clientAccountClientDetailVM.ClientDetail = clientDetail;


            return View(clientAccountClientDetailVM);
        }
    }
}
