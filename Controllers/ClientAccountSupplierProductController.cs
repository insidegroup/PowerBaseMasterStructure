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
    public class ClientAccountSupplierProductController : Controller
    {
        ClientDetailRepository clientDetailRepository = new ClientDetailRepository();
        ClientAccountRepository clientAccountRepository = new ClientAccountRepository();
        ClientDetailClientAccountRepository clientDetailClientAccountRepository = new ClientDetailClientAccountRepository();
        ClientDetailSupplierProductRepository clientDetailSupplierProductRepository = new ClientDetailSupplierProductRepository();
        SupplierRepository supplierRepository = new SupplierRepository();
        ProductRepository productRepository = new ProductRepository();

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

            ClientDetailClientAccount clientDetailClientAccount = new ClientDetailClientAccount();
            clientDetailClientAccount = clientDetailClientAccountRepository.GetClientDetailClientAccount(id);

            //Check Exists
            if (clientDetailClientAccount == null)
            {
                ViewData["ActionMethod"] = "List";
                return View("RecordDoesNotExistError");
            }

            string can = clientDetailClientAccount.ClientAccountNumber;
            string ssc = clientDetailClientAccount.SourceSystemCode;

            ClientAccount clientAccount = new ClientAccount();
            clientAccount = clientAccountRepository.GetClientAccount(can, ssc);

            //Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToClientAccount(can, ssc))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //Populate View Model
            ClientAccountSupplierProductsVM clientAccountSupplierProductsVM = new ClientAccountSupplierProductsVM();
            clientAccountSupplierProductsVM.SupplierProducts = clientDetailRepository.ListClientDetailSupplierProducts(id, page ?? 1);
            clientAccountSupplierProductsVM.ClientAccount = clientAccount;
            clientAccountSupplierProductsVM.ClientDetail = clientDetail;

            //Return Form
            return View(clientAccountSupplierProductsVM);
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
            clientAccount = clientAccountRepository.GetClientAccount(can, ssc);

            //Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToClientAccount(can, ssc))
            {
                ViewData["Access"] = "WriteAccess";
            }

            ClientAccountSupplierProductVM clientAccountSupplierProductVM = new ClientAccountSupplierProductVM();
            clientAccountSupplierProductVM.ClientAccount = clientAccount;
            clientAccountSupplierProductVM.ClientDetail = clientDetail;

            ProductRepository productRepository = new ProductRepository();
            clientAccountSupplierProductVM.Products = new SelectList(productRepository.GetAllProducts().ToList(), "ProductId", "ProductName");

            return View(clientAccountSupplierProductVM);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClientAccountSupplierProductVM clientAccountSupplierProductVM)
        {
            int clientDetailId = clientAccountSupplierProductVM.ClientDetail.ClientDetailId;

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
            clientAccount = clientAccountRepository.GetClientAccount(can, ssc);

            //Check Exists
            if (clientAccount == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientAccount(can, ssc))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }


            //Update  Model from Form
            try
            {
                TryUpdateModel<ClientDetailSupplierProduct>(clientAccountSupplierProductVM.SupplierProduct, "SupplierProduct");
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
                clientDetailSupplierProductRepository.Add(clientAccountSupplierProductVM.ClientDetail, clientAccountSupplierProductVM.SupplierProduct);
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
		public ActionResult Delete(int id, int pid, string sc)
        {
            ClientDetailSupplierProduct clientDetailSupplierProduct = new ClientDetailSupplierProduct();
            clientDetailSupplierProduct = clientDetailSupplierProductRepository.GetClientDetailSupplierProduct(id,pid,sc);

            //Check Exists
            if (clientDetailSupplierProduct == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            int clientDetailId = clientDetailSupplierProduct.ClientDetailId;
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
            clientAccount = clientAccountRepository.GetClientAccount(can, ssc);

            //Check Exists
            if (clientAccount == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientAccount(can, ssc))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
            
            ClientAccountSupplierProductVM clientAccountSupplierProductVM = new ClientAccountSupplierProductVM();
            clientAccountSupplierProductVM.ClientAccount = clientAccount;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(clientDetailId);
            clientAccountSupplierProductVM.ClientDetail = clientDetail;

            clientDetailSupplierProductRepository.EditForDisplay(clientDetailSupplierProduct);
            clientAccountSupplierProductVM.SupplierProduct = clientDetailSupplierProduct;

            return View(clientAccountSupplierProductVM);
        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, int pid, string sc, FormCollection collection)
        {
            ClientDetailSupplierProduct clientDetailSupplierProduct = new ClientDetailSupplierProduct();
            clientDetailSupplierProduct = clientDetailSupplierProductRepository.GetClientDetailSupplierProduct(id, pid, sc);

            //Check Exists
            if (clientDetailSupplierProduct == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            ClientDetailClientAccount clientDetailClientAccount = new ClientDetailClientAccount();
            clientDetailClientAccount = clientDetailClientAccountRepository.GetClientDetailClientAccount(id);

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
            if (!rolesRepository.HasWriteAccessToClientAccount(can, ssc))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Delete Item
            try
            {
                clientDetailSupplierProduct.VersionNumber = Int32.Parse(collection["SupplierProduct.VersionNumber"]);
                clientDetailSupplierProductRepository.Delete(clientDetailSupplierProduct);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/ClientAccountSupplierProduct.mvc/Delete?id=" + id.ToString() + "&sc=" + sc + "&pid=" + pid;
                    return View("VersionError");
                }
                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            return RedirectToAction("List", new { id = id });
        }
    }
}
