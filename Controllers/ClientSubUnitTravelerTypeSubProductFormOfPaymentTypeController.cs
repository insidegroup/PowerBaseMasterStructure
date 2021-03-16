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
    public class ClientSubUnitTravelerTypeSubProductFormOfPaymentTypeController : Controller
    {
        FormOfPaymentTypeRepository formOfPaymentTypeRepository = new FormOfPaymentTypeRepository();
        SubProductRepository subProductRepository = new SubProductRepository();
        ClientDetailRepository clientDetailRepository = new ClientDetailRepository();
        ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
        TravelerTypeRepository travelerTypeRepository = new TravelerTypeRepository();
        ClientDetailClientSubUnitTravelerTypeRepository clientDetailClientSubUnitTravelerTypeRepository = new ClientDetailClientSubUnitTravelerTypeRepository();
        ClientDetailSubProductFormOfPaymentTypeRepository clientDetailSubProductFormOfPaymentTypeRepository = new ClientDetailSubProductFormOfPaymentTypeRepository();

        // GET: /List
        public ActionResult List(int id, int? page)
        {
            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(id);

            //Check Exists
            if (clientDetail == null)
            {
                ViewData["ActionMethod"] = "List";
                return View("RecordDoesNotExistError");
            }

            ClientDetailClientSubUnitTravelerType clientDetailClientSubUnitTravelerType = new ClientDetailClientSubUnitTravelerType();
            clientDetailClientSubUnitTravelerType = clientDetailClientSubUnitTravelerTypeRepository.GetClientDetailClientSubUnitTravelerType(id);

            //Check Exists
            if (clientDetailClientSubUnitTravelerType == null)
            {
                ViewData["ActionMethod"] = "List";
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
            ClientSubUnitTravelerTypeSubProductFormOfPaymentTypesVM clientSubUnitTravelerTypeSubProductFormOfPaymentTypesVM = new ClientSubUnitTravelerTypeSubProductFormOfPaymentTypesVM();
            clientSubUnitTravelerTypeSubProductFormOfPaymentTypesVM.SubProductFormOfPaymentTypes = clientDetailRepository.ListClientDetailSubProductFormOfPaymentTypes(id,page ?? 1);
            clientSubUnitTravelerTypeSubProductFormOfPaymentTypesVM.ClientSubUnit = clientSubUnit;
            clientSubUnitTravelerTypeSubProductFormOfPaymentTypesVM.ClientDetail = clientDetail;

            return View(clientSubUnitTravelerTypeSubProductFormOfPaymentTypesVM);
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

            ClientSubUnitTravelerTypeSubProductFormOfPaymentTypeVM clientSubUnitTravelerTypeSubProductFormOfPaymentTypeVM = new ClientSubUnitTravelerTypeSubProductFormOfPaymentTypeVM();
            clientSubUnitTravelerTypeSubProductFormOfPaymentTypeVM.ClientSubUnit = clientSubUnit;
            clientSubUnitTravelerTypeSubProductFormOfPaymentTypeVM.ClientDetail = clientDetail;

            TravelerType travelerType = new TravelerType();
            travelerType = travelerTypeRepository.GetTravelerType(tt);
            clientSubUnitTravelerTypeSubProductFormOfPaymentTypeVM.TravelerType = travelerType;

            ClientDetailSubProductFormOfPaymentType clientDetailSubProductFormOfPaymentType = new ClientDetailSubProductFormOfPaymentType();
            clientSubUnitTravelerTypeSubProductFormOfPaymentTypeVM.ClientDetailSubProductFormOfPaymentType = clientDetailSubProductFormOfPaymentType;

            FormOfPaymentTypeRepository formOfPaymentTypeRepository = new FormOfPaymentTypeRepository();
            clientSubUnitTravelerTypeSubProductFormOfPaymentTypeVM.FormOfPaymentTypes = new SelectList(formOfPaymentTypeRepository.GetAllFormOfPaymentTypes().ToList(), "FormOfPaymentTypeId", "FormOfPaymentTypeDescription");

            clientSubUnitTravelerTypeSubProductFormOfPaymentTypeVM.SubProducts = new SelectList(clientDetailSubProductFormOfPaymentTypeRepository.GetUnUsedSubProducts(id).ToList(), "SubProductId", "SubProductName");

            return View(clientSubUnitTravelerTypeSubProductFormOfPaymentTypeVM);
        }
        
        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClientSubUnitTravelerTypeSubProductFormOfPaymentTypeVM clientSubUnitTravelerTypeSubProductFormOfPaymentTypeVM)
        {
            int clientDetailId = clientSubUnitTravelerTypeSubProductFormOfPaymentTypeVM.ClientDetail.ClientDetailId;
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
                TryUpdateModel<ClientDetailSubProductFormOfPaymentType>(clientSubUnitTravelerTypeSubProductFormOfPaymentTypeVM.ClientDetailSubProductFormOfPaymentType, "ClientDetailSubProductFormOfPaymentType");
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
                clientDetailSubProductFormOfPaymentTypeRepository.Add(clientDetail, clientSubUnitTravelerTypeSubProductFormOfPaymentTypeVM.ClientDetailSubProductFormOfPaymentType);
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
		public ActionResult Delete(int id, int fpt, int sp)
       {
           ClientDetailSubProductFormOfPaymentType clientDetailSubProductFormOfPaymentType = new ClientDetailSubProductFormOfPaymentType();
           clientDetailSubProductFormOfPaymentType = clientDetailSubProductFormOfPaymentTypeRepository.GetClientDetailSubProductFormOfPaymentType(sp,id);

           //Check Exists
           if (clientDetailSubProductFormOfPaymentType == null)
           {
               ViewData["ActionMethod"] = "DeleteGet";
               return View("RecordDoesNotExistError");
           }

           ClientDetailClientSubUnitTravelerType clientDetailClientSubUnitTravelerType = new ClientDetailClientSubUnitTravelerType();
           clientDetailClientSubUnitTravelerType = clientDetailClientSubUnitTravelerTypeRepository.GetClientDetailClientSubUnitTravelerType(id);

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

           ClientSubUnitTravelerTypeSubProductFormOfPaymentTypeVM clientSubUnitTravelerTypeSubProductFormOfPaymentTypeVM = new ClientSubUnitTravelerTypeSubProductFormOfPaymentTypeVM();
           clientSubUnitTravelerTypeSubProductFormOfPaymentTypeVM.ClientSubUnit = clientSubUnit;

           clientDetailSubProductFormOfPaymentTypeRepository.EditForDisplay(clientDetailSubProductFormOfPaymentType);
           clientSubUnitTravelerTypeSubProductFormOfPaymentTypeVM.ClientDetailSubProductFormOfPaymentType = clientDetailSubProductFormOfPaymentType;

           TravelerType travelerType = new TravelerType();
           travelerType = travelerTypeRepository.GetTravelerType(tt);
           clientSubUnitTravelerTypeSubProductFormOfPaymentTypeVM.TravelerType = travelerType;

           ClientDetail clientDetail = new ClientDetail();
           clientDetail = clientDetailRepository.GetGroup(id);
           clientSubUnitTravelerTypeSubProductFormOfPaymentTypeVM.ClientDetail = clientDetail;

           return View(clientSubUnitTravelerTypeSubProductFormOfPaymentTypeVM);
       }

       // POST: /Delete
       [HttpPost]
       [ValidateAntiForgeryToken]
       public ActionResult Delete(int id, int fpt, int sp, FormCollection collection)
       {
           ClientDetailSubProductFormOfPaymentType clientDetailSubProductFormOfPaymentType = new ClientDetailSubProductFormOfPaymentType();
           clientDetailSubProductFormOfPaymentType = clientDetailSubProductFormOfPaymentTypeRepository.GetClientDetailSubProductFormOfPaymentType(sp,id);

           //Check Exists
           if (clientDetailSubProductFormOfPaymentType == null)
           {
               ViewData["ActionMethod"] = "DeletePost";
               return View("RecordDoesNotExistError");
           }

           int clientDetailId = clientDetailSubProductFormOfPaymentType.ClientDetailId;
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
               clientDetailSubProductFormOfPaymentType.VersionNumber = Int32.Parse(collection["ClientDetailSubProductFormOfPaymentType.VersionNumber"]);
               clientDetailSubProductFormOfPaymentTypeRepository.Delete(clientDetailSubProductFormOfPaymentType);
           }
           catch (SqlException ex)
           {
               //Versioning Error - go to standard versionError page
               if (ex.Message == "SQLVersioningError")
               {
                   ViewData["ReturnURL"] = "/ClientSubUnitTravelerTypeSubProductFormOfPaymentType.mvc/Delete/?id=" + id.ToString() + "&sp=" + sp.ToString() + "&fpt=" + fpt.ToString();
                   return View("VersionError");
               }
               //Generic Error
               ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
               return View("Error");
           }


           return RedirectToAction("List", new { id = clientDetailId });
       }

    }
}
