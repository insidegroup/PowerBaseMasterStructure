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
    public class TravelerTypeSubProductFormOfPaymentTypeController : Controller
    {
        FormOfPaymentTypeRepository formOfPaymentTypeRepository = new FormOfPaymentTypeRepository();
        SubProductRepository subProductRepository = new SubProductRepository();
        ClientDetailRepository clientDetailRepository = new ClientDetailRepository();
        ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
        ClientSubUnitTravelerTypeRepository clientSubUnitTravelerTypeRepository = new ClientSubUnitTravelerTypeRepository();
        TravelerTypeRepository travelerTypeRepository = new TravelerTypeRepository();
        ClientDetailTravelerTypeRepository clientDetailTravelerTypeRepository = new ClientDetailTravelerTypeRepository();
        ClientDetailSubProductFormOfPaymentTypeRepository clientDetailSubProductFormOfPaymentTypeRepository = new ClientDetailSubProductFormOfPaymentTypeRepository();

        // GET: /List
        public ActionResult List(string csu, string tt, int id, int? page)
        {
            ClientDetailTravelerType clientDetailTravelerType = new ClientDetailTravelerType();
            clientDetailTravelerType = clientDetailTravelerTypeRepository.GetClientDetailTravelerType(id);

            //Check Exists
            if (clientDetailTravelerType == null)
            {
                ViewData["ActionMethod"] = "List";
                return View("RecordDoesNotExistError");
            }

            ClientSubUnitTravelerType clientSubUnitTravelerType = new ClientSubUnitTravelerType();
            clientSubUnitTravelerType = clientSubUnitTravelerTypeRepository.GetClientSubUnitTravelerType(csu, tt);

            //Check Exists
            if (clientDetailTravelerType == null)
            {
                ViewData["ActionMethod"] = "List";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToClientSubUnit(csu))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //Populate View Model
            TravelerTypeSubProductFormOfPaymentTypesVM travelerTypeSubProductFormOfPaymentTypesVM = new TravelerTypeSubProductFormOfPaymentTypesVM();
            travelerTypeSubProductFormOfPaymentTypesVM.SubProductFormOfPaymentTypes = clientDetailRepository.ListClientDetailSubProductFormOfPaymentTypes(id,page ?? 1);

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);
            travelerTypeSubProductFormOfPaymentTypesVM.ClientSubUnit = clientSubUnit;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(id);
            travelerTypeSubProductFormOfPaymentTypesVM.ClientDetail = clientDetail;

            TravelerType travelerType = new TravelerType();
            travelerType = travelerTypeRepository.GetTravelerType(tt);
            travelerTypeSubProductFormOfPaymentTypesVM.TravelerType = travelerType;

            return View(travelerTypeSubProductFormOfPaymentTypesVM);
        }
        
        // GET: /Create
        public ActionResult Create(int id, string csu, string tt)
        {
            ClientDetailTravelerType clientDetailTravelerType = new ClientDetailTravelerType();
            clientDetailTravelerType = clientDetailTravelerTypeRepository.GetClientDetailTravelerType(id);

            //Check Exists
            if (clientDetailTravelerType == null)
            {
                ViewData["ActionMethod"] = "ListSubMenu";
                return View("RecordDoesNotExistError");
            }

            ClientSubUnitTravelerType clientSubUnitTravelerType = new ClientSubUnitTravelerType();
            clientSubUnitTravelerType = clientSubUnitTravelerTypeRepository.GetClientSubUnitTravelerType(csu, tt);

            //Check Exists
            if (clientDetailTravelerType == null)
            {
                ViewData["ActionMethod"] = "ListSubMenu";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToClientSubUnit(csu))
            {
                ViewData["Access"] = "WriteAccess";
            }

            TravelerTypeSubProductFormOfPaymentTypeVM travelerTypeSubProductFormOfPaymentTypeVM = new TravelerTypeSubProductFormOfPaymentTypeVM();

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);
            travelerTypeSubProductFormOfPaymentTypeVM.ClientSubUnit = clientSubUnit;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(id);
            travelerTypeSubProductFormOfPaymentTypeVM.ClientDetail = clientDetail;

            TravelerType travelerType = new TravelerType();
            travelerType = travelerTypeRepository.GetTravelerType(tt);
            travelerTypeSubProductFormOfPaymentTypeVM.TravelerType = travelerType;

            ClientDetailSubProductFormOfPaymentType clientDetailSubProductFormOfPaymentType = new ClientDetailSubProductFormOfPaymentType();
            travelerTypeSubProductFormOfPaymentTypeVM.ClientDetailSubProductFormOfPaymentType = clientDetailSubProductFormOfPaymentType;

            FormOfPaymentTypeRepository formOfPaymentTypeRepository = new FormOfPaymentTypeRepository();
            travelerTypeSubProductFormOfPaymentTypeVM.FormOfPaymentTypes = new SelectList(formOfPaymentTypeRepository.GetAllFormOfPaymentTypes().ToList(), "FormOfPaymentTypeId", "FormOfPaymentTypeDescription");

            travelerTypeSubProductFormOfPaymentTypeVM.SubProducts = new SelectList(clientDetailSubProductFormOfPaymentTypeRepository.GetUnUsedSubProducts(id).ToList(), "SubProductId", "SubProductName");

            return View(travelerTypeSubProductFormOfPaymentTypeVM);
        }
        
        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TravelerTypeSubProductFormOfPaymentTypeVM travelerTypeSubProductFormOfPaymentTypeVM)
        {
            int clientDetailId = travelerTypeSubProductFormOfPaymentTypeVM.ClientDetail.ClientDetailId;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(clientDetailId);

            //Check Exists
            if (clientDetail == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            ClientDetailTravelerType clientDetailTravelerType = new ClientDetailTravelerType();
            clientDetailTravelerType = clientDetailTravelerTypeRepository.GetClientDetailTravelerType(clientDetailId);

            //Check Exists
            if (clientDetailTravelerType == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            string csu = travelerTypeSubProductFormOfPaymentTypeVM.ClientSubUnit.ClientSubUnitGuid;
            string tt = travelerTypeSubProductFormOfPaymentTypeVM.TravelerType.TravelerTypeGuid;

            ClientSubUnitTravelerType clientSubUnitTravelerType = new ClientSubUnitTravelerType();
            clientSubUnitTravelerType = clientSubUnitTravelerTypeRepository.GetClientSubUnitTravelerType(csu, tt);

            //Check Exists
            if (clientSubUnitTravelerType == null)
            {
                ViewData["ActionMethod"] = "ListSubMenu";
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
                TryUpdateModel<ClientDetailSubProductFormOfPaymentType>(travelerTypeSubProductFormOfPaymentTypeVM.ClientDetailSubProductFormOfPaymentType, "ClientDetailSubProductFormOfPaymentType");
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
                clientDetailSubProductFormOfPaymentTypeRepository.Add(clientDetail, travelerTypeSubProductFormOfPaymentTypeVM.ClientDetailSubProductFormOfPaymentType);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            return RedirectToAction("List", new { id = clientDetailId, csu = csu, tt = tt });
        }
         
       // GET: /Delete
		[HttpGet]
		public ActionResult Delete(string csu, int id, int fpt, int sp)
       {
           ClientDetailSubProductFormOfPaymentType clientDetailSubProductFormOfPaymentType = new ClientDetailSubProductFormOfPaymentType();
           clientDetailSubProductFormOfPaymentType = clientDetailSubProductFormOfPaymentTypeRepository.GetClientDetailSubProductFormOfPaymentType(sp,id);

           //Check Exists
           if (clientDetailSubProductFormOfPaymentType == null)
           {
               ViewData["ActionMethod"] = "DeleteGet";
               return View("RecordDoesNotExistError");
           }

           int clientDetailId = clientDetailSubProductFormOfPaymentType.ClientDetailId;
           ClientDetailTravelerType clientDetailTravelerType = new ClientDetailTravelerType();
           clientDetailTravelerType = clientDetailTravelerTypeRepository.GetClientDetailTravelerType(clientDetailId);

           //Check Exists
           if (clientDetailTravelerType == null)
           {
               ViewData["ActionMethod"] = "DeleteGet";
               return View("RecordDoesNotExistError");
           }

           string tt = clientDetailTravelerType.TravelerTypeGuid;
           ClientSubUnitTravelerType clientSubUnitTravelerType = new ClientSubUnitTravelerType();
           clientSubUnitTravelerType = clientSubUnitTravelerTypeRepository.GetClientSubUnitTravelerType(csu, tt);

           //Check Exists
           if (clientDetailTravelerType == null)
           {
               ViewData["ActionMethod"] = "DeleteGet";
               return View("RecordDoesNotExistError");
           }

           //Access Rights
           RolesRepository rolesRepository = new RolesRepository();
           if (!rolesRepository.HasWriteAccessToClientSubUnit(csu))
           {
               ViewData["Message"] = "You do not have access to this item";
               return View("Error");
           }

           TravelerTypeSubProductFormOfPaymentTypeVM travelerTypeSubProductFormOfPaymentTypeVM = new TravelerTypeSubProductFormOfPaymentTypeVM();

           clientDetailSubProductFormOfPaymentTypeRepository.EditForDisplay(clientDetailSubProductFormOfPaymentType);
           travelerTypeSubProductFormOfPaymentTypeVM.ClientDetailSubProductFormOfPaymentType = clientDetailSubProductFormOfPaymentType;

           ClientSubUnit clientSubUnit = new ClientSubUnit();
           clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);
           travelerTypeSubProductFormOfPaymentTypeVM.ClientSubUnit = clientSubUnit;

           ClientDetail clientDetail = new ClientDetail();
           clientDetail = clientDetailRepository.GetGroup(clientDetailId);
           travelerTypeSubProductFormOfPaymentTypeVM.ClientDetail = clientDetail;

           TravelerType travelerType = new TravelerType();
           travelerType = travelerTypeRepository.GetTravelerType(tt);
           travelerTypeSubProductFormOfPaymentTypeVM.TravelerType = travelerType;

           return View(travelerTypeSubProductFormOfPaymentTypeVM);
       }

       // POST: /Delete
       [HttpPost]
       [ValidateAntiForgeryToken]
       public ActionResult Delete(string csu, int id, int fpt, int sp, FormCollection collection)
       {
           ClientDetailSubProductFormOfPaymentType clientDetailSubProductFormOfPaymentType = new ClientDetailSubProductFormOfPaymentType();
           clientDetailSubProductFormOfPaymentType = clientDetailSubProductFormOfPaymentTypeRepository.GetClientDetailSubProductFormOfPaymentType(sp, id);

           //Check Exists
           if (clientDetailSubProductFormOfPaymentType == null)
           {
               ViewData["ActionMethod"] = "DeleteGet";
               return View("RecordDoesNotExistError");
           }

           int clientDetailId = clientDetailSubProductFormOfPaymentType.ClientDetailId;
           ClientDetailTravelerType clientDetailTravelerType = new ClientDetailTravelerType();
           clientDetailTravelerType = clientDetailTravelerTypeRepository.GetClientDetailTravelerType(clientDetailId);

           //Check Exists
           if (clientDetailTravelerType == null)
           {
               ViewData["ActionMethod"] = "DeleteGet";
               return View("RecordDoesNotExistError");
           }

           string tt = clientDetailTravelerType.TravelerTypeGuid;
           ClientSubUnitTravelerType clientSubUnitTravelerType = new ClientSubUnitTravelerType();
           clientSubUnitTravelerType = clientSubUnitTravelerTypeRepository.GetClientSubUnitTravelerType(csu, tt);

           //Check Exists
           if (clientDetailTravelerType == null)
           {
               ViewData["ActionMethod"] = "DeleteGet";
               return View("RecordDoesNotExistError");
           }

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
                   ViewData["ReturnURL"] = "/TravelerTypeSubProductFormOfPaymentType.mvc/Delete/?id=" + id.ToString() + "&sp=" + sp.ToString() + "&fpt=" + fpt.ToString() + "&csu="  + csu;
                   return View("VersionError");
               }
               //Generic Error
               ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
               return View("Error");
           }


           return RedirectToAction("List", new { id = clientDetailId, csu = csu, tt = tt });
       }

    }
}
