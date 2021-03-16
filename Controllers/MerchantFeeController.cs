using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Repository;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Models;
using System.Data.SqlClient;

namespace CWTDesktopDatabase.Controllers
{
    public class MerchantFeeController : Controller
    {
        HierarchyRepository hierarchyRepository = new HierarchyRepository();
        MerchantFeeRepository merchantFeeRepository = new MerchantFeeRepository();
        private string groupName = "ClientFee";

        public ActionResult List(string filter, int? page, string sortField, int? sortOrder)
        {

            //SortField + SortOrder settings
            if (sortField != "CountryName" && sortField != "CreditCardVendorName" && sortField != "ProductName" && sortField != "SupplierName" && sortField != "MerchantFeePercent")
            {
                sortField = "MerchantFeeDescription";
            }
            ViewData["CurrentSortField"] = sortField;

            if (sortOrder == 1)
            {
                ViewData["NewSortOrder"] = 0;
                ViewData["CurrentSortOrder"] = 1;
            }
            else
            {
                ViewData["NewSortOrder"] = 1;
                ViewData["CurrentSortOrder"] = 0;
            }

            MerchantFeesVM merchantFeesVM = new MerchantFeesVM();

            //Check Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                merchantFeesVM.HasWriteAccess = true;
            }

            merchantFeesVM.MerchantFees = merchantFeeRepository.PageMerchantFees(page ?? 1, filter ?? "", sortField, sortOrder ?? 0);
            return View(merchantFeesVM);
        }

        // GET: Create A Single MerchantFee
        public ActionResult Create()
        {
            //Check Access Rights to Domain
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            MerchantFeeVM merchantFeeVM = new MerchantFeeVM();

            MerchantFee merchantFee = new MerchantFee();
            merchantFeeVM.MerchantFee = merchantFee;
            
            CountryRepository countryRepository = new CountryRepository();
            merchantFeeVM.Countries = new SelectList(countryRepository.GetAllCountries().ToList(), "CountryCode", "CountryName");

            CreditCardVendorRepository creditCardVendorRepository = new CreditCardVendorRepository();
            merchantFeeVM.CreditCardVendors = new SelectList(creditCardVendorRepository.GetAllCreditCardVendors().ToList(), "CreditCardVendorCode", "CreditCardVendorName");

            ProductRepository productRepository = new ProductRepository();
            merchantFeeVM.Products = new SelectList(productRepository.GetAllProducts().ToList(), "ProductId", "ProductName");

            return View(merchantFeeVM);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MerchantFeeVM merchantFeeVM)
        {
            //Check Access Rights to Domain
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update Model From Form + Validate against DB
            try
            {
                UpdateModel<MerchantFee>(merchantFeeVM.MerchantFee, "MerchantFee");
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


            //Database Update
            try
            {
                merchantFeeRepository.Add(merchantFeeVM.MerchantFee);
            }
            catch (SqlException ex)
            {
                //Non-Unique Name
                if (ex.Message == "NonUniqueName")
                {
                    ViewData["Message"] = "There is already a Merchant Fee with this Country/CreditCard/Product/Supplier combination. Please go back and change one of these items";
                    return View("Error");
                }
                
				LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List");
        }

        // GET: Edit A Single MerchantFee
        public ActionResult Edit( int id)
        {
            //Get Item From Database
            MerchantFee merchantFee = new MerchantFee();
            merchantFee = merchantFeeRepository.GetItem(id);

            //Check Exists
            if (merchantFee == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Check Access Rights to Domain
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            MerchantFeeVM merchantFeeVM = new MerchantFeeVM();
            merchantFee.MerchantFeePercentMaximum = 100;

            merchantFeeRepository.EditForDisplay(merchantFee);
            merchantFeeVM.MerchantFee = merchantFee;

            CountryRepository countryRepository = new CountryRepository();
            merchantFeeVM.Countries = new SelectList(countryRepository.GetAllCountries().ToList(), "CountryCode", "CountryName", merchantFee.CountryCode);

            CreditCardVendorRepository creditCardVendorRepository = new CreditCardVendorRepository();
            merchantFeeVM.CreditCardVendors = new SelectList(creditCardVendorRepository.GetAllCreditCardVendors().ToList(), "CreditCardVendorCode", "CreditCardVendorName", merchantFee.CreditCardVendorCode);

            ProductRepository productRepository = new ProductRepository();
            merchantFeeVM.Products = new SelectList(productRepository.GetAllProducts().ToList(), "ProductId", "ProductName", merchantFee.ProductId);

            return View(merchantFeeVM);
        }

        // POST: //Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MerchantFeeVM merchantFeeVM)
        {
            //Check Access Rights to Domain
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            MerchantFee merchantFee = new MerchantFee();
            merchantFee = merchantFeeRepository.GetItem(merchantFeeVM.MerchantFee.MerchantFeeId);

            //Check Exists
            if (merchantFee == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //Update  Model from Form
            try
            {
                UpdateModel(merchantFee, "MerchantFee");
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

            //Database Update
            try
            {
                merchantFeeRepository.Update(merchantFee);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List");
        }

        // GET: View A Single MerchantFee
        public ActionResult View(int id)
        {
            //Get Item From Database
            MerchantFee merchantFee = new MerchantFee();
            merchantFee = merchantFeeRepository.GetItem(id);

            //Check Exists
            if (merchantFee == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            MerchantFeeVM merchantFeeVM = new MerchantFeeVM();

            merchantFeeRepository.EditForDisplay(merchantFee);
            merchantFeeVM.MerchantFee = merchantFee;

            return View(merchantFeeVM);
        }

        // GET: delete A Single ClientFee
		[HttpGet]
		public ActionResult Delete(int id)
        {
            //Check Access Rights to Domain
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Get Item From Database
            MerchantFee merchantFee = new MerchantFee();
            merchantFee = merchantFeeRepository.GetItem(id);

            //Check Exists
            if (merchantFee == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            MerchantFeeVM merchantFeeVM = new MerchantFeeVM();

            merchantFeeRepository.EditForDisplay(merchantFee);
            merchantFeeVM.MerchantFee = merchantFee;

            return View(merchantFeeVM);
        }

        // POST: //Delete/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            //Check Access Rights to Domain
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Get Item From Database
            MerchantFee merchantFee = new MerchantFee();
            merchantFee = merchantFeeRepository.GetItem(id);

            //Check Exists
            if (merchantFee == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            //Delete Item
            try
            {
                merchantFee.VersionNumber = Int32.Parse(collection["MerchantFee.VersionNumber"]);
                merchantFeeRepository.Delete(merchantFee);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/MerchantFee.mvc/Delete/" + merchantFee.MerchantFeeId.ToString();
                    return View("VersionError");
                }
                //Restraint Error - go to standard DeleteError page
                if (ex.Message == "SQLDeleteError")
                {
                    ViewData["ReturnURL"] = "/MerchantFee.mvc/Delete/" + merchantFee.MerchantFeeId.ToString();
                    return View("DeleteError");
                }

				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);
				
				//Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            //Return
            return RedirectToAction("List");
        }
    }
}
