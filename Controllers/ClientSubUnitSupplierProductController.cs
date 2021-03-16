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
    public class ClientSubUnitSupplierProductController : Controller
    {
        ClientDetailRepository clientDetailRepository = new ClientDetailRepository();
        ClientSubUnitRepository clientTopUnitRepository = new ClientSubUnitRepository();
        ClientDetailClientSubUnitRepository clientDetailClientSubUnitRepository = new ClientDetailClientSubUnitRepository();
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

            ClientDetailClientSubUnit clientDetailClientSubUnit = new ClientDetailClientSubUnit();
            clientDetailClientSubUnit = clientDetailClientSubUnitRepository.GetClientDetailClientSubUnit(id);

            //Check Exists
            if (clientDetailClientSubUnit == null)
            {
                ViewData["ActionMethod"] = "List";
                return View("RecordDoesNotExistError");
            }

            string clientTopUnitGuid = clientDetailClientSubUnit.ClientSubUnitGuid;
            ClientSubUnit clientTopUnit = new ClientSubUnit();
            clientTopUnit = clientTopUnitRepository.GetClientSubUnit(clientTopUnitGuid);

            //Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToClientSubUnit(clientTopUnitGuid))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //Populate View Model
            ClientSubUnitSupplierProductsVM clientTopUnitSupplierProductsVM = new ClientSubUnitSupplierProductsVM();
            clientTopUnitSupplierProductsVM.SupplierProducts = clientDetailRepository.ListClientDetailSupplierProducts(id, page ?? 1);
            clientTopUnitSupplierProductsVM.ClientSubUnit = clientTopUnit;
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

            ClientDetailClientSubUnit clientDetailClientSubUnit = new ClientDetailClientSubUnit();
            clientDetailClientSubUnit = clientDetailClientSubUnitRepository.GetClientDetailClientSubUnit(id);

            //Check Exists
            if (clientDetailClientSubUnit == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            string clientTopUnitGuid = clientDetailClientSubUnit.ClientSubUnitGuid;
            ClientSubUnit clientTopUnit = new ClientSubUnit();
            clientTopUnit = clientTopUnitRepository.GetClientSubUnit(clientTopUnitGuid);

            //Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToClientSubUnit(clientTopUnitGuid))
            {
                ViewData["Access"] = "WriteAccess";
            }

            ClientSubUnitSupplierProductVM clientTopUnitSupplierProductVM = new ClientSubUnitSupplierProductVM();
            clientTopUnitSupplierProductVM.ClientSubUnit = clientTopUnit;
            clientTopUnitSupplierProductVM.ClientDetail = clientDetail;

            ProductRepository productRepository = new ProductRepository();
            clientTopUnitSupplierProductVM.Products = new SelectList(productRepository.GetAllProducts().ToList(), "ProductId", "ProductName");

            return View(clientTopUnitSupplierProductVM);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClientSubUnitSupplierProductVM clientTopUnitSupplierProductVM)
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

            ClientDetailClientSubUnit clientDetailClientSubUnit = new ClientDetailClientSubUnit();
            clientDetailClientSubUnit = clientDetailClientSubUnitRepository.GetClientDetailClientSubUnit(clientDetailId);

            //Check Exists
            if (clientDetailClientSubUnit == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            string clientTopUnitGuid = clientDetailClientSubUnit.ClientSubUnitGuid;
            ClientSubUnit clientTopUnit = new ClientSubUnit();
            clientTopUnit = clientTopUnitRepository.GetClientSubUnit(clientTopUnitGuid);

            //Check Exists
            if (clientTopUnit == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(clientTopUnitGuid))
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
            ClientDetailClientSubUnit clientDetailClientSubUnit = new ClientDetailClientSubUnit();
            clientDetailClientSubUnit = clientDetailClientSubUnitRepository.GetClientDetailClientSubUnit(clientDetailId);

            //Check Exists
            if (clientDetailClientSubUnit == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            string clientTopUnitGuid = clientDetailClientSubUnit.ClientSubUnitGuid;
            ClientSubUnit clientTopUnit = new ClientSubUnit();
            clientTopUnit = clientTopUnitRepository.GetClientSubUnit(clientTopUnitGuid);

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(clientTopUnitGuid))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ClientSubUnitSupplierProductVM clientTopUnitSupplierProductVM = new ClientSubUnitSupplierProductVM();
            clientTopUnitSupplierProductVM.ClientSubUnit = clientTopUnit;

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

            ClientDetailClientSubUnit clientDetailClientSubUnit = new ClientDetailClientSubUnit();
            clientDetailClientSubUnit = clientDetailClientSubUnitRepository.GetClientDetailClientSubUnit(id);

            //Check Exists
            if (clientDetailClientSubUnit == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            string clientTopUnitGuid = clientDetailClientSubUnit.ClientSubUnitGuid;
            ClientSubUnit clientTopUnit = new ClientSubUnit();
            clientTopUnit = clientTopUnitRepository.GetClientSubUnit(clientTopUnitGuid);

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(clientTopUnitGuid))
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
                    ViewData["ReturnURL"] = "/ClientSubUnitSupplierProduct.mvc/Delete?id=" + id.ToString() + "&sc=" + sc + "&pid=" + pid;
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
