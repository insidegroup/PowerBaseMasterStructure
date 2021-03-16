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
    public class ClientTopUnitSupplierProductController : Controller
    {
        ClientDetailRepository clientDetailRepository = new ClientDetailRepository();
        ClientTopUnitRepository clientTopUnitRepository = new ClientTopUnitRepository();
        ClientDetailClientTopUnitRepository clientDetailClientTopUnitRepository = new ClientDetailClientTopUnitRepository();
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
            ClientTopUnitSupplierProductsVM clientTopUnitSupplierProductsVM = new ClientTopUnitSupplierProductsVM();
            clientTopUnitSupplierProductsVM.SupplierProducts = clientDetailRepository.ListClientDetailSupplierProducts(id, page ?? 1);
            clientTopUnitSupplierProductsVM.ClientTopUnit = clientTopUnit;
            clientTopUnitSupplierProductsVM.ClientDetail = clientDetail;

            //Return Form
            return View(clientTopUnitSupplierProductsVM);
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

            ClientTopUnitSupplierProductVM clientTopUnitSupplierProductVM = new ClientTopUnitSupplierProductVM();
            clientTopUnitSupplierProductVM.ClientTopUnit = clientTopUnit;
            clientTopUnitSupplierProductVM.ClientDetail = clientDetail;

            ProductRepository productRepository = new ProductRepository();
            clientTopUnitSupplierProductVM.Products = new SelectList(productRepository.GetAllProducts().ToList(), "ProductId", "ProductName");

            return View(clientTopUnitSupplierProductVM);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClientTopUnitSupplierProductVM clientTopUnitSupplierProductVM)
        {
            int clientDetailId = clientTopUnitSupplierProductVM.ClientDetail.ClientDetailId;

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

            string clientTopUnitGuid = clientDetailClientTopUnit.ClientTopUnitGuid;
            ClientTopUnit clientTopUnit = new ClientTopUnit();
            clientTopUnit = clientTopUnitRepository.GetClientTopUnit(clientTopUnitGuid);

            //Check Exists
            if (clientTopUnit == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientTopUnit(clientTopUnitGuid))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }


            //Update  Model from Form
            try
            {
                TryUpdateModel<ClientDetailSupplierProduct>(clientTopUnitSupplierProductVM.SupplierProduct, "SupplierProduct");
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
                clientDetailSupplierProductRepository.Add(clientTopUnitSupplierProductVM.ClientDetail, clientTopUnitSupplierProductVM.SupplierProduct);
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
            ClientDetailClientTopUnit clientDetailClientTopUnit = new ClientDetailClientTopUnit();
            clientDetailClientTopUnit = clientDetailClientTopUnitRepository.GetClientDetailClientTopUnit(clientDetailId);

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

            ClientTopUnitSupplierProductVM clientTopUnitSupplierProductVM = new ClientTopUnitSupplierProductVM();
            clientTopUnitSupplierProductVM.ClientTopUnit = clientTopUnit;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(clientDetailId);
            clientTopUnitSupplierProductVM.ClientDetail = clientDetail;

            clientDetailSupplierProductRepository.EditForDisplay(clientDetailSupplierProduct);
            clientTopUnitSupplierProductVM.SupplierProduct = clientDetailSupplierProduct;

            return View(clientTopUnitSupplierProductVM);
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

            ClientDetailClientTopUnit clientDetailClientTopUnit = new ClientDetailClientTopUnit();
            clientDetailClientTopUnit = clientDetailClientTopUnitRepository.GetClientDetailClientTopUnit(id);

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
                clientDetailSupplierProduct.VersionNumber = Int32.Parse(collection["SupplierProduct.VersionNumber"]);
                clientDetailSupplierProductRepository.Delete(clientDetailSupplierProduct);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/ClientTopUnitSupplierProduct.mvc/Delete?id=" + id.ToString() + "&sc=" + sc + "&pid=" + pid;
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
