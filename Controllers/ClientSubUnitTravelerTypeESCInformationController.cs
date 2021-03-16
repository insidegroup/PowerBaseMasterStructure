using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Repository;
using CWTDesktopDatabase.Helpers;
using System.Data.SqlClient;

namespace CWTDesktopDatabase.Controllers
{
    public class ClientSubUnitTravelerTypeESCInformationController : Controller
    {
        ClientDetailRepository clientDetailRepository = new ClientDetailRepository();
        TravelerTypeRepository travelerTypeRepository = new TravelerTypeRepository();
        ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
        ClientDetailClientSubUnitTravelerTypeRepository clientDetailClientSubUnitTravelerTypeRepository = new ClientDetailClientSubUnitTravelerTypeRepository();
        ClientDetailESCInformationRepository clientDetailESCInformationRepository = new ClientDetailESCInformationRepository();

        // GET: /List
        public ActionResult List(int id, int? page)
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

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);

            //Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToClientSubUnit(csu))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //Populate View Model
            //Populate View Model
            ClientSubUnitTravelerTypeESCInformationVM clientSubUnitTravelerTypeESCInformationVM = new ClientSubUnitTravelerTypeESCInformationVM();
            clientSubUnitTravelerTypeESCInformationVM.ClientDetailESCInformation = clientDetailRepository.GetClientDetailESCInformation(id);
            clientSubUnitTravelerTypeESCInformationVM.ClientSubUnit = clientSubUnit;
            clientSubUnitTravelerTypeESCInformationVM.ClientDetail = clientDetail;

            TravelerType travelerType = new TravelerType();
            travelerType = travelerTypeRepository.GetTravelerType(tt);
            clientSubUnitTravelerTypeESCInformationVM.TravelerType = travelerType;

            return View(clientSubUnitTravelerTypeESCInformationVM);
        }

        // GET: /Create
        public ActionResult Create(int id)
        {
            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(id);

            //Check Exists
            if (clientDetail == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            ClientDetailClientSubUnitTravelerType clientDetailClientSubUnitTravelerType = new ClientDetailClientSubUnitTravelerType();
            clientDetailClientSubUnitTravelerType = clientDetailClientSubUnitTravelerTypeRepository.GetClientDetailClientSubUnitTravelerType(id);

            //Check Exists
            if (clientDetailClientSubUnitTravelerType == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            string csu = clientDetailClientSubUnitTravelerType.ClientSubUnitGuid;
            string tt = clientDetailClientSubUnitTravelerType.TravelerTypeGuid;        
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);

            //Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToClientSubUnit(csu))
            {
                ViewData["Access"] = "WriteAccess";
            }

            ClientSubUnitTravelerTypeESCInformationVM clientSubUnitTravelerTypeESCInformationVM = new ClientSubUnitTravelerTypeESCInformationVM();
            clientSubUnitTravelerTypeESCInformationVM.ClientSubUnit = clientSubUnit;
            clientSubUnitTravelerTypeESCInformationVM.ClientDetail = clientDetail;

            TravelerType travelerType = new TravelerType();
            travelerType = travelerTypeRepository.GetTravelerType(tt);
            clientSubUnitTravelerTypeESCInformationVM.TravelerType = travelerType;

            ClientDetailESCInformation clientDetailESCInformation = new ClientDetailESCInformation();
            clientSubUnitTravelerTypeESCInformationVM.ClientDetailESCInformation = clientDetailESCInformation;

            return View(clientSubUnitTravelerTypeESCInformationVM);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClientSubUnitTravelerTypeESCInformationVM clientSubUnitTravelerTypeESCInformationVM)
        {
            int clientDetailId = clientSubUnitTravelerTypeESCInformationVM.ClientDetail.ClientDetailId;
            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(clientDetailId);

            //Check Exists
            if (clientDetail == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            ClientDetailClientSubUnitTravelerType clientDetailClientSubUnitTravelerType = new ClientDetailClientSubUnitTravelerType();
            clientDetailClientSubUnitTravelerType = clientDetailClientSubUnitTravelerTypeRepository.GetClientDetailClientSubUnitTravelerType(clientDetailId);

            //Check Exists
            if (clientDetailClientSubUnitTravelerType == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            string csu = clientDetailClientSubUnitTravelerType.ClientSubUnitGuid;
            string tt = clientDetailClientSubUnitTravelerType.TravelerTypeGuid;
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);

            //Check Exists
            if (clientSubUnit == null)
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


            //Update  Model from Form
            try
            {
                TryUpdateModel<ClientDetailESCInformation>(clientSubUnitTravelerTypeESCInformationVM.ClientDetailESCInformation, "ClientDetailESCInformation");
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
                clientDetailESCInformationRepository.Add(clientSubUnitTravelerTypeESCInformationVM.ClientDetail, clientSubUnitTravelerTypeESCInformationVM.ClientDetailESCInformation);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            return RedirectToAction("List", new { id = clientDetailId });
        }

        // GET: /Edit
        public ActionResult Edit(int id)
        {
            ClientDetailESCInformation clientDetailESCInformation = new ClientDetailESCInformation();
            clientDetailESCInformation = clientDetailESCInformationRepository.GetClientDetailESCInformation(id);
            
            //Check Exists
            if (clientDetailESCInformation == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            int clientDetailId = clientDetailESCInformation.ClientDetailId;
            ClientDetailClientSubUnitTravelerType clientDetailClientSubUnitTravelerType = new ClientDetailClientSubUnitTravelerType();
            clientDetailClientSubUnitTravelerType = clientDetailClientSubUnitTravelerTypeRepository.GetClientDetailClientSubUnitTravelerType(clientDetailId);

            //Check Exists
            if (clientDetailClientSubUnitTravelerType == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            string csu = clientDetailClientSubUnitTravelerType.ClientSubUnitGuid;
            string tt = clientDetailClientSubUnitTravelerType.TravelerTypeGuid;
            

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(csu))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ClientSubUnitTravelerTypeESCInformationVM clientSubUnitTravelerTypeESCInformationVM = new ClientSubUnitTravelerTypeESCInformationVM();
            clientSubUnitTravelerTypeESCInformationVM.ClientSubUnit = clientSubUnit;
            
            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(id);

            //Check Exists
            if (clientDetail == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }
            clientSubUnitTravelerTypeESCInformationVM.ClientDetail = clientDetail;

            TravelerType travelerType = new TravelerType();
            travelerType = travelerTypeRepository.GetTravelerType(tt);
            clientSubUnitTravelerTypeESCInformationVM.TravelerType = travelerType;
            clientSubUnitTravelerTypeESCInformationVM.ClientDetailESCInformation = clientDetailESCInformation;

            return View(clientSubUnitTravelerTypeESCInformationVM);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ClientSubUnitTravelerTypeESCInformationVM clientSubUnitTravelerTypeESCInformationVM)
        {
            int clientDetailId = clientSubUnitTravelerTypeESCInformationVM.ClientDetailESCInformation.ClientDetailId;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(clientDetailId);

            //Check Exists
            if (clientDetail == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            ClientDetailClientSubUnitTravelerType clientDetailClientSubUnitTravelerType = new ClientDetailClientSubUnitTravelerType();
            clientDetailClientSubUnitTravelerType = clientDetailClientSubUnitTravelerTypeRepository.GetClientDetailClientSubUnitTravelerType(clientDetailId);

            //Check Exists
            if (clientDetailClientSubUnitTravelerType == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            string csu = clientDetailClientSubUnitTravelerType.ClientSubUnitGuid;
            string tt = clientDetailClientSubUnitTravelerType.TravelerTypeGuid;
            

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);

            //Check Exists
            if (clientSubUnit == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(csu))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }


            //Update  Model from Form
            try
            {
                TryUpdateModel<ClientDetailESCInformation>(clientSubUnitTravelerTypeESCInformationVM.ClientDetailESCInformation, "ClientDetailESCInformation");
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
                clientDetailESCInformationRepository.Edit(clientSubUnitTravelerTypeESCInformationVM.ClientDetailESCInformation);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            return RedirectToAction("List", new { id = clientDetailId });
        }

        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id)
        {
            ClientDetailESCInformation clientDetailESCInformation = new ClientDetailESCInformation();
            clientDetailESCInformation = clientDetailESCInformationRepository.GetClientDetailESCInformation(id);

            //Check Exists
            if (clientDetailESCInformation == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            int clientDetailId = clientDetailESCInformation.ClientDetailId;
            ClientDetailClientSubUnitTravelerType clientDetailClientSubUnitTravelerType = new ClientDetailClientSubUnitTravelerType();
            clientDetailClientSubUnitTravelerType = clientDetailClientSubUnitTravelerTypeRepository.GetClientDetailClientSubUnitTravelerType(clientDetailId);

            //Check Exists
            if (clientDetailClientSubUnitTravelerType == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            string csu = clientDetailClientSubUnitTravelerType.ClientSubUnitGuid;
            string tt = clientDetailClientSubUnitTravelerType.TravelerTypeGuid;          
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(csu))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ClientSubUnitTravelerTypeESCInformationVM clientSubUnitTravelerTypeESCInformationVM = new ClientSubUnitTravelerTypeESCInformationVM();
            clientSubUnitTravelerTypeESCInformationVM.ClientSubUnit = clientSubUnit;
            clientSubUnitTravelerTypeESCInformationVM.ClientDetailESCInformation = clientDetailESCInformation;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(clientDetailId);
            clientSubUnitTravelerTypeESCInformationVM.ClientDetail = clientDetail;

            TravelerType travelerType = new TravelerType();
            travelerType = travelerTypeRepository.GetTravelerType(tt);
            clientSubUnitTravelerTypeESCInformationVM.TravelerType = travelerType;
            
            return View(clientSubUnitTravelerTypeESCInformationVM);
        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            ClientDetailESCInformation clientDetailESCInformation = new ClientDetailESCInformation();
            clientDetailESCInformation = clientDetailESCInformationRepository.GetClientDetailESCInformation(id);

            //Check Exists
            if (clientDetailESCInformation == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            int clientDetailId = clientDetailESCInformation.ClientDetailId;
            ClientDetailClientSubUnitTravelerType clientDetailClientSubUnitTravelerType = new ClientDetailClientSubUnitTravelerType();
            clientDetailClientSubUnitTravelerType = clientDetailClientSubUnitTravelerTypeRepository.GetClientDetailClientSubUnitTravelerType(clientDetailId);

            //Check Exists
            if (clientDetailClientSubUnitTravelerType == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            string csu = clientDetailClientSubUnitTravelerType.ClientSubUnitGuid;
            string tt = clientDetailClientSubUnitTravelerType.TravelerTypeGuid;
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(csu))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Delete Item
            try
            {
                clientDetailESCInformation.VersionNumber = Int32.Parse(collection["ClientDetailESCInformation.VersionNumber"]);
                clientDetailESCInformationRepository.Delete(clientDetailESCInformation);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/ClientSubUnitTravelerTypeESCInformation.mvc/Delete/" + id.ToString();
                    return View("VersionError");
                }
                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            return RedirectToAction("List", new { id = clientDetailId });
        }

        // GET: /View
        public ActionResult View(int id)
        {
            ClientDetailESCInformation clientDetailESCInformation = new ClientDetailESCInformation();
            clientDetailESCInformation = clientDetailESCInformationRepository.GetClientDetailESCInformation(id);

            //Check Exists
            if (clientDetailESCInformation == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            int clientDetailId = clientDetailESCInformation.ClientDetailId;
            ClientDetailClientSubUnitTravelerType clientDetailClientSubUnitTravelerType = new ClientDetailClientSubUnitTravelerType();
            clientDetailClientSubUnitTravelerType = clientDetailClientSubUnitTravelerTypeRepository.GetClientDetailClientSubUnitTravelerType(clientDetailId);

            //Check Exists
            if (clientDetailClientSubUnitTravelerType == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            string csu = clientDetailClientSubUnitTravelerType.ClientSubUnitGuid;
            string tt = clientDetailClientSubUnitTravelerType.TravelerTypeGuid;
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);

            ClientSubUnitTravelerTypeESCInformationVM clientSubUnitTravelerTypeESCInformationVM = new ClientSubUnitTravelerTypeESCInformationVM();
            clientSubUnitTravelerTypeESCInformationVM.ClientSubUnit = clientSubUnit;
            clientSubUnitTravelerTypeESCInformationVM.ClientDetailESCInformation = clientDetailESCInformation;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(clientDetailId);
            clientSubUnitTravelerTypeESCInformationVM.ClientDetail = clientDetail;

            TravelerType travelerType = new TravelerType();
            travelerType = travelerTypeRepository.GetTravelerType(tt);
            clientSubUnitTravelerTypeESCInformationVM.TravelerType = travelerType;

            return View(clientSubUnitTravelerTypeESCInformationVM);
        }
    }
}
