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
    public class ClientTopUnitSubProductFormOfPaymentTypeController : Controller
    {
        FormOfPaymentTypeRepository formOfPaymentTypeRepository = new FormOfPaymentTypeRepository();
        SubProductRepository subProductRepository = new SubProductRepository();
        ClientDetailRepository clientDetailRepository = new ClientDetailRepository();
        ClientTopUnitRepository clientTopUnitRepository = new ClientTopUnitRepository();
        ClientDetailClientTopUnitRepository clientDetailClientTopUnitRepository = new ClientDetailClientTopUnitRepository();
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

            ClientDetailClientTopUnit clientDetailClientTopUnit = new ClientDetailClientTopUnit();
            clientDetailClientTopUnit = clientDetailClientTopUnitRepository.GetClientDetailClientTopUnit(id);

            //Check Exists
            if (clientDetailClientTopUnit == null)
            {
                ViewData["ActionMethod"] = "List";
                return View("RecordDoesNotExistError");
            }

            string clientTopUnitGuid = clientDetailClientTopUnit.ClientTopUnitGuid;

            ClientTopUnit clientTopUnit = new ClientTopUnit();
            clientTopUnit = clientTopUnitRepository.GetClientTopUnit(clientTopUnitGuid);

            //Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToClientTopUnit(clientTopUnitGuid))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //Populate View Model
            ClientTopUnitSubProductFormOfPaymentTypesVM clientTopUnitSubProductFormOfPaymentTypesVM = new ClientTopUnitSubProductFormOfPaymentTypesVM();
            clientTopUnitSubProductFormOfPaymentTypesVM.SubProductFormOfPaymentTypes = clientDetailRepository.ListClientDetailSubProductFormOfPaymentTypes(id,page ?? 1);
            clientTopUnitSubProductFormOfPaymentTypesVM.ClientTopUnit = clientTopUnit;
            clientTopUnitSubProductFormOfPaymentTypesVM.ClientDetail = clientDetail;

            return View(clientTopUnitSubProductFormOfPaymentTypesVM);
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

            ClientDetailClientTopUnit clientDetailClientTopUnit = new ClientDetailClientTopUnit();
            clientDetailClientTopUnit = clientDetailClientTopUnitRepository.GetClientDetailClientTopUnit(id);

            //Check Exists
            if (clientDetailClientTopUnit == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            string ctu = clientDetailClientTopUnit.ClientTopUnitGuid;
            ClientTopUnit clientTopUnit = new ClientTopUnit();
            clientTopUnit = clientTopUnitRepository.GetClientTopUnit(ctu);

            //Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToClientTopUnit(ctu))
            {
                ViewData["Access"] = "WriteAccess";
            }

            ClientTopUnitSubProductFormOfPaymentTypeVM clientTopUnitSubProductFormOfPaymentTypeVM = new ClientTopUnitSubProductFormOfPaymentTypeVM();
            clientTopUnitSubProductFormOfPaymentTypeVM.ClientTopUnit = clientTopUnit;
            clientTopUnitSubProductFormOfPaymentTypeVM.ClientDetail = clientDetail;

            ClientDetailSubProductFormOfPaymentType clientDetailSubProductFormOfPaymentType = new ClientDetailSubProductFormOfPaymentType();
            clientTopUnitSubProductFormOfPaymentTypeVM.ClientDetailSubProductFormOfPaymentType = clientDetailSubProductFormOfPaymentType;

            FormOfPaymentTypeRepository formOfPaymentTypeRepository = new FormOfPaymentTypeRepository();
            clientTopUnitSubProductFormOfPaymentTypeVM.FormOfPaymentTypes = new SelectList(formOfPaymentTypeRepository.GetAllFormOfPaymentTypes().ToList(), "FormOfPaymentTypeId", "FormOfPaymentTypeDescription");

            clientTopUnitSubProductFormOfPaymentTypeVM.SubProducts = new SelectList(clientDetailSubProductFormOfPaymentTypeRepository.GetUnUsedSubProducts(id).ToList(), "SubProductId", "SubProductName");

            return View(clientTopUnitSubProductFormOfPaymentTypeVM);
        }
        
        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClientTopUnitSubProductFormOfPaymentTypeVM clientTopUnitSubProductFormOfPaymentTypeVM)
        {
            int clientDetailId = clientTopUnitSubProductFormOfPaymentTypeVM.ClientDetail.ClientDetailId;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(clientDetailId);

            //Check Exists
            if (clientDetail == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            ClientDetailClientTopUnit clientDetailClientTopUnit = new ClientDetailClientTopUnit();
            clientDetailClientTopUnit = clientDetailClientTopUnitRepository.GetClientDetailClientTopUnit(clientDetailId);

            //Check Exists
            if (clientDetailClientTopUnit == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            string ctu = clientDetailClientTopUnit.ClientTopUnitGuid;


            ClientTopUnit clientTopUnit = new ClientTopUnit();
            clientTopUnit = clientTopUnitRepository.GetClientTopUnit(ctu);

            //Check Exists
            if (clientTopUnit == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientTopUnit(ctu))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }


            //Update  Model from Form
            try
            {
                TryUpdateModel<ClientDetailSubProductFormOfPaymentType>(clientTopUnitSubProductFormOfPaymentTypeVM.ClientDetailSubProductFormOfPaymentType, "ClientDetailSubProductFormOfPaymentType");
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
                clientDetailSubProductFormOfPaymentTypeRepository.Add(clientDetail, clientTopUnitSubProductFormOfPaymentTypeVM.ClientDetailSubProductFormOfPaymentType);
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

           ClientDetailClientTopUnit clientDetailClientTopUnit = new ClientDetailClientTopUnit();
           clientDetailClientTopUnit = clientDetailClientTopUnitRepository.GetClientDetailClientTopUnit(id);

           //Check Exists
           if (clientDetailClientTopUnit == null)
           {
               ViewData["ActionMethod"] = "DeleteGet";
               return View("RecordDoesNotExistError");
           }

           string clientTopUnitGuid = clientDetailClientTopUnit.ClientTopUnitGuid;
           ClientTopUnit clientTopUnit = new ClientTopUnit();
           clientTopUnit = clientTopUnitRepository.GetClientTopUnit(clientTopUnitGuid);

           //Access Rights
           RolesRepository rolesRepository = new RolesRepository();
           if (!rolesRepository.HasWriteAccessToClientTopUnit(clientTopUnitGuid))
           {
               ViewData["Message"] = "You do not have access to this item";
               return View("Error");
           }

           ClientTopUnitSubProductFormOfPaymentTypeVM clientTopUnitSubProductFormOfPaymentTypeVM = new ClientTopUnitSubProductFormOfPaymentTypeVM();
           clientTopUnitSubProductFormOfPaymentTypeVM.ClientTopUnit = clientTopUnit;

           clientDetailSubProductFormOfPaymentTypeRepository.EditForDisplay(clientDetailSubProductFormOfPaymentType);
           clientTopUnitSubProductFormOfPaymentTypeVM.ClientDetailSubProductFormOfPaymentType = clientDetailSubProductFormOfPaymentType;

           ClientDetail clientDetail = new ClientDetail();
           clientDetail = clientDetailRepository.GetGroup(id);
           clientTopUnitSubProductFormOfPaymentTypeVM.ClientDetail = clientDetail;

           return View(clientTopUnitSubProductFormOfPaymentTypeVM);
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
           ClientDetailClientTopUnit clientDetailClientTopUnit = new ClientDetailClientTopUnit();
           clientDetailClientTopUnit = clientDetailClientTopUnitRepository.GetClientDetailClientTopUnit(clientDetailId);

           //Check Exists
           if (clientDetailClientTopUnit == null)
           {
               ViewData["ActionMethod"] = "DeletePost";
               return View("RecordDoesNotExistError");
           }

           string clientTopUnitGuid = clientDetailClientTopUnit.ClientTopUnitGuid;


           ClientTopUnit clientTopUnit = new ClientTopUnit();
           clientTopUnit = clientTopUnitRepository.GetClientTopUnit(clientTopUnitGuid);

           //Access Rights
           RolesRepository rolesRepository = new RolesRepository();
           if (!rolesRepository.HasWriteAccessToClientTopUnit(clientTopUnitGuid))
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
                   ViewData["ReturnURL"] = "/ClientTopUnitSubProductFormOfPaymentType.mvc/Delete/?id=" + id.ToString() + "&sp=" + sp.ToString() + "&fpt=" + fpt.ToString();
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
