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
    public class ClientAccountSubProductFormOfPaymentTypeController : Controller
    {
        FormOfPaymentTypeRepository formOfPaymentTypeRepository = new FormOfPaymentTypeRepository();
        SubProductRepository subProductRepository = new SubProductRepository();
        ClientDetailRepository clientDetailRepository = new ClientDetailRepository();
        ClientAccountRepository clientAccountRepository = new ClientAccountRepository();
        ClientDetailClientAccountRepository clientDetailClientAccountRepository = new ClientDetailClientAccountRepository();
        ClientDetailSubProductFormOfPaymentTypeRepository clientDetailSubProductFormOfPaymentTypeRepository = new ClientDetailSubProductFormOfPaymentTypeRepository();

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

            ClientDetailClientAccount clientDetailClientAccount = new ClientDetailClientAccount();
            clientDetailClientAccount = clientDetailClientAccountRepository.GetClientDetailClientAccount(id);

            //Check Exists
            if (clientDetailClientAccount == null)
            {
                ViewData["ActionMethod"] = "ListSubMenu";
                return View("RecordDoesNotExistError");
            }

            string can = clientDetailClientAccount.ClientAccountNumber;
            string ssc = clientDetailClientAccount.SourceSystemCode;
            ClientAccount clientAccount = new ClientAccount();
            clientAccount = clientAccountRepository.GetClientAccount(can, ssc);

            //Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToClientSubUnit(can))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //Populate View Model
            ClientAccountSubProductFormOfPaymentTypesVM clientAccountSubProductFormOfPaymentTypesVM = new ClientAccountSubProductFormOfPaymentTypesVM();
            clientAccountSubProductFormOfPaymentTypesVM.SubProductFormOfPaymentTypes = clientDetailRepository.ListClientDetailSubProductFormOfPaymentTypes(id,page ?? 1);
            clientAccountSubProductFormOfPaymentTypesVM.ClientAccount = clientAccount;
            clientAccountSubProductFormOfPaymentTypesVM.ClientDetail = clientDetail;

            return View(clientAccountSubProductFormOfPaymentTypesVM);
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

            ClientDetailClientAccount clientDetailClientAccount = new ClientDetailClientAccount();
            clientDetailClientAccount = clientDetailClientAccountRepository.GetClientDetailClientAccount(id);

            //Check Exists
            if (clientDetailClientAccount == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            string can = clientDetailClientAccount.ClientAccountNumber;
            string ssc = clientDetailClientAccount.SourceSystemCode;
            ClientAccount clientAccount = new ClientAccount();
            clientAccount = clientAccountRepository.GetClientAccount(can,ssc);

            //Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToClientSubUnit(can))
            {
                ViewData["Access"] = "WriteAccess";
            }

            ClientAccountSubProductFormOfPaymentTypeVM clientAccountSubProductFormOfPaymentTypeVM = new ClientAccountSubProductFormOfPaymentTypeVM();
            clientAccountSubProductFormOfPaymentTypeVM.ClientAccount = clientAccount;
            clientAccountSubProductFormOfPaymentTypeVM.ClientDetail = clientDetail;

            ClientDetailSubProductFormOfPaymentType clientDetailSubProductFormOfPaymentType = new ClientDetailSubProductFormOfPaymentType();
            clientAccountSubProductFormOfPaymentTypeVM.ClientDetailSubProductFormOfPaymentType = clientDetailSubProductFormOfPaymentType;

            FormOfPaymentTypeRepository formOfPaymentTypeRepository = new FormOfPaymentTypeRepository();
            clientAccountSubProductFormOfPaymentTypeVM.FormOfPaymentTypes = new SelectList(formOfPaymentTypeRepository.GetAllFormOfPaymentTypes().ToList(), "FormOfPaymentTypeId", "FormOfPaymentTypeDescription");

            clientAccountSubProductFormOfPaymentTypeVM.SubProducts = new SelectList(clientDetailSubProductFormOfPaymentTypeRepository.GetUnUsedSubProducts(id).ToList(), "SubProductId", "SubProductName");

            return View(clientAccountSubProductFormOfPaymentTypeVM);
        }
        
        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClientAccountSubProductFormOfPaymentTypeVM clientAccountSubProductFormOfPaymentTypeVM)
        {
            int clientDetailId = clientAccountSubProductFormOfPaymentTypeVM.ClientDetail.ClientDetailId;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(clientDetailId);

            //Check Exists
            if (clientDetail == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            ClientDetailClientAccount clientDetailClientAccount = new ClientDetailClientAccount();
            clientDetailClientAccount = clientDetailClientAccountRepository.GetClientDetailClientAccount(clientDetailId);

            //Check Exists
            if (clientDetailClientAccount == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            string can = clientDetailClientAccount.ClientAccountNumber;
            string ssc = clientDetailClientAccount.SourceSystemCode;
            ClientAccount clientAccount = new ClientAccount();
            clientAccount = clientAccountRepository.GetClientAccount(can,ssc);

            //Check Exists
            if (clientAccount == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(can))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }


            //Update  Model from Form
            try
            {
                TryUpdateModel<ClientDetailSubProductFormOfPaymentType>(clientAccountSubProductFormOfPaymentTypeVM.ClientDetailSubProductFormOfPaymentType, "ClientDetailSubProductFormOfPaymentType");
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
                clientDetailSubProductFormOfPaymentTypeRepository.Add(clientDetail, clientAccountSubProductFormOfPaymentTypeVM.ClientDetailSubProductFormOfPaymentType);
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

           ClientDetailClientAccount clientDetailClientAccount = new ClientDetailClientAccount();
           clientDetailClientAccount = clientDetailClientAccountRepository.GetClientDetailClientAccount(id);

           //Check Exists
           if (clientDetailClientAccount == null)
           {
               ViewData["ActionMethod"] = "DeleteGet";
               return View("RecordDoesNotExistError");
           }

           string can = clientDetailClientAccount.ClientAccountNumber;
           string ssc = clientDetailClientAccount.SourceSystemCode;
           ClientAccount clientAccount = new ClientAccount();
           clientAccount = clientAccountRepository.GetClientAccount(can,ssc);

           //Access Rights
           RolesRepository rolesRepository = new RolesRepository();
           if (!rolesRepository.HasWriteAccessToClientSubUnit(can))
           {
               ViewData["Message"] = "You do not have access to this item";
               return View("Error");
           }

           ClientAccountSubProductFormOfPaymentTypeVM clientAccountSubProductFormOfPaymentTypeVM = new ClientAccountSubProductFormOfPaymentTypeVM();
           clientAccountSubProductFormOfPaymentTypeVM.ClientAccount = clientAccount;

           clientDetailSubProductFormOfPaymentTypeRepository.EditForDisplay(clientDetailSubProductFormOfPaymentType);
           clientAccountSubProductFormOfPaymentTypeVM.ClientDetailSubProductFormOfPaymentType = clientDetailSubProductFormOfPaymentType;

           ClientDetail clientDetail = new ClientDetail();
           clientDetail = clientDetailRepository.GetGroup(id);
           clientAccountSubProductFormOfPaymentTypeVM.ClientDetail = clientDetail;

           return View(clientAccountSubProductFormOfPaymentTypeVM);
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
           ClientDetailClientAccount clientDetailClientAccount = new ClientDetailClientAccount();
           clientDetailClientAccount = clientDetailClientAccountRepository.GetClientDetailClientAccount(clientDetailId);

           //Check Exists
           if (clientDetailClientAccount == null)
           {
               ViewData["ActionMethod"] = "DeletePost";
               return View("RecordDoesNotExistError");
           }

           string can = clientDetailClientAccount.ClientAccountNumber;
           string ssc = clientDetailClientAccount.SourceSystemCode;
           ClientAccount clientAccount = new ClientAccount();
           clientAccount = clientAccountRepository.GetClientAccount(can, ssc);

           //Access Rights
           RolesRepository rolesRepository = new RolesRepository();
           if (!rolesRepository.HasWriteAccessToClientSubUnit(can))
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
                   ViewData["ReturnURL"] = "/ClientSubUnitSubProductFormOfPaymentType.mvc/Delete/?id=" + id.ToString() + "&sp=" + sp.ToString() + "&fpt=" + fpt.ToString();
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
