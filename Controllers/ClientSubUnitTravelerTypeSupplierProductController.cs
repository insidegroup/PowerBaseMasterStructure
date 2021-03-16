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
    public class ClientSubUnitTravelerTypeSupplierProductController : Controller
    {
        ClientDetailRepository clientDetailRepository = new ClientDetailRepository();
        TravelerTypeRepository travelerTypeRepository = new TravelerTypeRepository();
        ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
        ClientDetailClientSubUnitTravelerTypeRepository clientDetailClientSubUnitTravelerTypeRepository = new ClientDetailClientSubUnitTravelerTypeRepository();
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
            ClientSubUnitTravelerTypeSupplierProductsVM clientSubUnitTravelerTypeSupplierProductsVM = new ClientSubUnitTravelerTypeSupplierProductsVM();
            clientSubUnitTravelerTypeSupplierProductsVM.SupplierProducts = clientDetailRepository.ListClientDetailSupplierProducts(id, page ?? 1);
            clientSubUnitTravelerTypeSupplierProductsVM.ClientSubUnit = clientSubUnit;
            clientSubUnitTravelerTypeSupplierProductsVM.ClientDetail = clientDetail;

            TravelerType travelerType = new TravelerType();
            travelerType = travelerTypeRepository.GetTravelerType(tt);
            clientSubUnitTravelerTypeSupplierProductsVM.TravelerType = travelerType;

            //Return Form
            return View(clientSubUnitTravelerTypeSupplierProductsVM);
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

            ClientSubUnitTravelerTypeSupplierProductVM clientSubUnitTravelerTypeSupplierProductVM = new ClientSubUnitTravelerTypeSupplierProductVM();
            clientSubUnitTravelerTypeSupplierProductVM.ClientSubUnit = clientSubUnit;
            clientSubUnitTravelerTypeSupplierProductVM.ClientDetail = clientDetail;

            TravelerType travelerType = new TravelerType();
            travelerType = travelerTypeRepository.GetTravelerType(tt);
            clientSubUnitTravelerTypeSupplierProductVM.TravelerType = travelerType;

            ProductRepository productRepository = new ProductRepository();
            clientSubUnitTravelerTypeSupplierProductVM.Products = new SelectList(productRepository.GetAllProducts().ToList(), "ProductId", "ProductName");

            return View(clientSubUnitTravelerTypeSupplierProductVM);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClientSubUnitTravelerTypeSupplierProductVM clientSubUnitTravelerTypeSupplierProductVM)
        {
            int clientDetailId = clientSubUnitTravelerTypeSupplierProductVM.ClientDetail.ClientDetailId;

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
                TryUpdateModel<ClientDetailSupplierProduct>(clientSubUnitTravelerTypeSupplierProductVM.SupplierProduct, "SupplierProduct");
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
                clientDetailSupplierProductRepository.Add(clientSubUnitTravelerTypeSupplierProductVM.ClientDetail, clientSubUnitTravelerTypeSupplierProductVM.SupplierProduct);
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

            ClientSubUnitTravelerTypeSupplierProductVM clientSubUnitTravelerTypeSupplierProductVM = new ClientSubUnitTravelerTypeSupplierProductVM();
            clientSubUnitTravelerTypeSupplierProductVM.ClientSubUnit = clientSubUnit;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(clientDetailId);
            clientSubUnitTravelerTypeSupplierProductVM.ClientDetail = clientDetail;

            TravelerType travelerType = new TravelerType();
            travelerType = travelerTypeRepository.GetTravelerType(tt);
            clientSubUnitTravelerTypeSupplierProductVM.TravelerType = travelerType;

            clientDetailSupplierProductRepository.EditForDisplay(clientDetailSupplierProduct);
            clientSubUnitTravelerTypeSupplierProductVM.SupplierProduct = clientDetailSupplierProduct;

            return View(clientSubUnitTravelerTypeSupplierProductVM);
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

            ClientDetailClientSubUnitTravelerType clientDetailClientSubUnitTravelerType = new ClientDetailClientSubUnitTravelerType();
            clientDetailClientSubUnitTravelerType = clientDetailClientSubUnitTravelerTypeRepository.GetClientDetailClientSubUnitTravelerType(id);

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
                    ViewData["ReturnURL"] = "/ClientSubUnitTravelerTypeSupplierProduct.mvc/Delete?id=" + id.ToString() + "&sc=" + sc + "&pid=" + pid;
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
