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
    public class TravelerTypeSupplierProductController : Controller
    {
        ClientDetailRepository clientDetailRepository = new ClientDetailRepository();
        TravelerTypeRepository travelerTypeRepository = new TravelerTypeRepository();
        ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
        ClientSubUnitTravelerTypeRepository clientSubUnitTravelerTypeRepository = new ClientSubUnitTravelerTypeRepository();
        ClientDetailTravelerTypeRepository clientDetailTravelerTypeRepository = new ClientDetailTravelerTypeRepository();
        ClientDetailSupplierProductRepository clientDetailSupplierProductRepository = new ClientDetailSupplierProductRepository();
        SupplierRepository supplierRepository = new SupplierRepository();
        ProductRepository productRepository = new ProductRepository();

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

            //Populate View Model
            TravelerTypeSupplierProductsVM travelerTypeSupplierProductsVM = new TravelerTypeSupplierProductsVM();
            travelerTypeSupplierProductsVM.SupplierProducts = clientDetailRepository.ListClientDetailSupplierProducts(id, page ?? 1);

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);
            travelerTypeSupplierProductsVM.ClientSubUnit = clientSubUnit;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(id);
            travelerTypeSupplierProductsVM.ClientDetail = clientDetail;

            TravelerType travelerType = new TravelerType();
            travelerType = travelerTypeRepository.GetTravelerType(tt);
            travelerTypeSupplierProductsVM.TravelerType = travelerType;

            //Return Form
            return View(travelerTypeSupplierProductsVM);
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

            //View Model
            TravelerTypeSupplierProductVM travelerTypeSupplierProductVM = new TravelerTypeSupplierProductVM();

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);
            travelerTypeSupplierProductVM.ClientSubUnit = clientSubUnit;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(id);
            travelerTypeSupplierProductVM.ClientDetail = clientDetail;

            TravelerType travelerType = new TravelerType();
            travelerType = travelerTypeRepository.GetTravelerType(tt);
            travelerTypeSupplierProductVM.TravelerType = travelerType;

            ProductRepository productRepository = new ProductRepository();
            travelerTypeSupplierProductVM.Products = new SelectList(productRepository.GetAllProducts().ToList(), "ProductId", "ProductName");

            return View(travelerTypeSupplierProductVM);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TravelerTypeSupplierProductVM travelerTypeSupplierProductVM)
        {
            int clientDetailId = travelerTypeSupplierProductVM.ClientDetail.ClientDetailId;

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

            string csu = travelerTypeSupplierProductVM.ClientSubUnit.ClientSubUnitGuid;
            string tt = travelerTypeSupplierProductVM.TravelerType.TravelerTypeGuid;

            ClientSubUnitTravelerType clientSubUnitTravelerType = new ClientSubUnitTravelerType();
            clientSubUnitTravelerType = clientSubUnitTravelerTypeRepository.GetClientSubUnitTravelerType(csu, tt);

            //Check Exists
            if (clientSubUnitTravelerType == null)
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
                TryUpdateModel<ClientDetailSupplierProduct>(travelerTypeSupplierProductVM.SupplierProduct, "SupplierProduct");
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
                clientDetailSupplierProductRepository.Add(travelerTypeSupplierProductVM.ClientDetail, travelerTypeSupplierProductVM.SupplierProduct);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            return RedirectToAction("List", new { id = clientDetailId, tt=tt, csu=csu });
        }

        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(string csu, int id, int pid, string sc)
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
            ClientDetailTravelerType clientDetailTravelerType = new ClientDetailTravelerType();
            clientDetailTravelerType = clientDetailTravelerTypeRepository.GetClientDetailTravelerType(clientDetailId);

            //Check Exists
            if (clientDetailTravelerType == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            string tt = clientDetailTravelerType.TravelerTypeGuid;
            ClientSubUnitTravelerType clientSubUnitTravelerType = new ClientSubUnitTravelerType();
            clientSubUnitTravelerType = clientSubUnitTravelerTypeRepository.GetClientSubUnitTravelerType(csu, tt);

            //Check Exists
            if (clientDetailTravelerType == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(csu))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            TravelerTypeSupplierProductVM travelerTypeSupplierProductVM = new TravelerTypeSupplierProductVM();

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);
            travelerTypeSupplierProductVM.ClientSubUnit = clientSubUnit;

            ClientDetail clientDetail = new ClientDetail();
            clientDetail = clientDetailRepository.GetGroup(clientDetailId);
            travelerTypeSupplierProductVM.ClientDetail = clientDetail;

            TravelerType travelerType = new TravelerType();
            travelerType = travelerTypeRepository.GetTravelerType(tt);
            travelerTypeSupplierProductVM.TravelerType = travelerType;

            clientDetailSupplierProductRepository.EditForDisplay(clientDetailSupplierProduct);
            travelerTypeSupplierProductVM.SupplierProduct = clientDetailSupplierProduct;

            return View(travelerTypeSupplierProductVM);
        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(TravelerTypeSupplierProductVM travelerTypeSupplierProductVM, FormCollection collection)
        {
            int clientDetailId = travelerTypeSupplierProductVM.ClientDetail.ClientDetailId;                
            ClientDetailTravelerType clientDetailTravelerType = new ClientDetailTravelerType();
            clientDetailTravelerType = clientDetailTravelerTypeRepository.GetClientDetailTravelerType(clientDetailId);

            //Check Exists
            if (clientDetailTravelerType == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            int pid = travelerTypeSupplierProductVM.SupplierProduct.ProductId;
            string sc = travelerTypeSupplierProductVM.SupplierProduct.SupplierCode;

            ClientDetailSupplierProduct clientDetailSupplierProduct = new ClientDetailSupplierProduct();
            clientDetailSupplierProduct = clientDetailSupplierProductRepository.GetClientDetailSupplierProduct(clientDetailId, pid, sc);

            //Check Exists
            if (clientDetailSupplierProduct == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            string csu = travelerTypeSupplierProductVM.ClientSubUnit.ClientSubUnitGuid;
            string tt = travelerTypeSupplierProductVM.TravelerType.TravelerTypeGuid;

            ClientSubUnitTravelerType clientSubUnitTravelerType = new ClientSubUnitTravelerType();
            clientSubUnitTravelerType = clientSubUnitTravelerTypeRepository.GetClientSubUnitTravelerType(csu, tt);

            //Check Exists
            if (clientSubUnitTravelerType == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
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
                    ViewData["ReturnURL"] = "/TravelerTypeSupplierProduct.mvc/Delete?id=" + clientDetailId.ToString() + "&pid=" + pid + "&sc=" + sc + "&csu=" + csu;
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