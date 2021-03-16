using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Repository;
using System.Data.SqlClient;

namespace CWTDesktopDatabase.Controllers
{
    public class FareRestrictionController : Controller
    {
        //main repository
        FareRestrictionRepository fareRestrictionRepository = new FareRestrictionRepository();
        PolicyRoutingRepository policyRoutingRepository = new PolicyRoutingRepository();

        // GET: /List/
        public ActionResult List(string filter, int? page, string sortField, int? sortOrder)
        {
            //Set Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToReferenceInfo())
            {
                ViewData["Access"] = "WriteAccess";
            }

            if (sortField != "Name")
            {
                sortField = "FareBasis";
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

            //return items
            var cwtPaginatedList = fareRestrictionRepository.PageFareRestrictions(page ?? 1, filter ?? "", sortField, sortOrder ?? 0);
            return View(cwtPaginatedList);

        }

        // GET: /View
        public ActionResult View(int id)
        {
            //Check Exists
            FareRestriction fareRestriction = new FareRestriction();
            fareRestriction = fareRestrictionRepository.GetFareRestriction(id);
            if (fareRestriction == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            fareRestrictionRepository.EditForDisplay(fareRestriction);

            //add the PolicyRouting information
            PolicyRouting policyRouting = new PolicyRouting();
            if (fareRestriction.RoutingId != null)
            {
                policyRouting = policyRoutingRepository.GetPolicyRouting((int)fareRestriction.RoutingId);
                policyRoutingRepository.EditForDisplay(policyRouting);
            }
            FareRestrictionViewModel fareRestrictionViewModel = new FareRestrictionViewModel(fareRestriction, policyRouting);

            //Show View
            return View(fareRestrictionViewModel);
        }

        // GET: /Create
        public ActionResult Create()
        {
            FareRestriction fareRestriction = new FareRestriction();

            //Check Exists
            if (fareRestriction == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToReferenceInfo())
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Populate List of Languages
            LanguageRepository languageRepository = new LanguageRepository();
            SelectList languages = new SelectList(languageRepository.GetAllLanguages().ToList(), "LanguageCode", "LanguageName");
            ViewData["LanguageList"] = languages;

            //Populate List of Products
            ProductRepository productRepository = new ProductRepository();
            SelectList products = new SelectList(productRepository.GetAllProducts().ToList(), "ProductId", "ProductName");
            ViewData["ProductList"] = products;

            PolicyRouting policyRouting = new PolicyRouting();
            policyRouting.FromGlobalFlag = false;
            policyRouting.ToGlobalFlag = false;

            FareRestrictionViewModel fareRestrictionViewModel = new FareRestrictionViewModel(fareRestriction, policyRouting);

            //Show 'Create' Form
            return View(fareRestrictionViewModel);
        }

        // GET: /CreatePolicyRouting
        public ActionResult CreatePolicyRouting(int id)
        {

            //Get PolicyAirVendorGroupItem
            FareRestriction fareRestriction = new FareRestriction();
            fareRestriction = fareRestrictionRepository.GetFareRestriction(id);

            //Check Exists
            if (fareRestriction == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToReferenceInfo())
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //populate new PolicyAirVendorGroupItem with known PolicyGroup Information
            fareRestrictionRepository.EditForDisplay(fareRestriction);

            PolicyRouting policyRouting = new PolicyRouting();
            FareRestrictionViewModel fareRestrictionViewModel = new FareRestrictionViewModel(fareRestriction, policyRouting);

            //Show 'Create' Form
            return View(fareRestrictionViewModel);
        }

        // POST: /CreatePolicyRouting
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePolicyRouting(int id, PolicyRouting policyRouting, string btnSubmit)
        {
            //Get PolicyAirVendorGroupItem (Original)
            FareRestriction fareRestrictionOriginal = new FareRestriction();
            fareRestrictionOriginal = fareRestrictionRepository.GetFareRestriction(id);

            //Check Exists
            if (fareRestrictionOriginal == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToReferenceInfo())
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update from+to fields from form to correct properties
            policyRoutingRepository.EditPolicyRouting(policyRouting);

            //Copy fareRestriction from original
            FareRestriction fareRestriction = new FareRestriction();
            fareRestriction.Changes = fareRestrictionOriginal.Changes;
            fareRestriction.Cancellations = fareRestrictionOriginal.Cancellations;
            fareRestriction.ReRoute = fareRestrictionOriginal.ReRoute;
            fareRestriction.ValidOn = fareRestrictionOriginal.ValidOn;
            fareRestriction.MinimumStay = fareRestrictionOriginal.MinimumStay;
            fareRestriction.MaximumStay = fareRestrictionOriginal.MaximumStay;
            fareRestriction.FareBasis = fareRestrictionOriginal.FareBasis;
            fareRestriction.BookingClass = fareRestrictionOriginal.BookingClass;
            fareRestriction.ProductId = fareRestrictionOriginal.ProductId;
            fareRestriction.SupplierCode = fareRestrictionOriginal.SupplierCode;
            fareRestriction.LanguageCode = fareRestrictionOriginal.LanguageCode;
            fareRestriction.DefaultChecked = fareRestrictionOriginal.DefaultChecked;

            //Save policyAirVendorGroupItem to DB
            try
            {
                fareRestrictionRepository.Add(fareRestriction, policyRouting);
            }
            catch (SqlException ex)
            {

                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            //Send to new form based on submit button pressed
            switch (btnSubmit)
            {
                case "Save":
                    return RedirectToAction("List");
                default:
                    return RedirectToAction("CreatePolicyRouting", new { id = id });
            }

        }

        // GET: /Edit
        public ActionResult Edit(int id)
        {
            //Check Exists
            FareRestriction fareRestriction = new FareRestriction();
            fareRestriction = fareRestrictionRepository.GetFareRestriction(id);
            if (fareRestriction == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToReferenceInfo())
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Populate List of Languages
            LanguageRepository languageRepository = new LanguageRepository();
            SelectList languages = new SelectList(languageRepository.GetAllLanguages().ToList(), "LanguageCode", "LanguageName");
            ViewData["LanguageList"] = languages;

            //Populate List of Products
            ProductRepository productRepository = new ProductRepository();
            SelectList products = new SelectList(productRepository.GetAllProducts().ToList(), "ProductId", "ProductName");
            ViewData["ProductList"] = products;

            fareRestrictionRepository.EditForDisplay(fareRestriction);

            //add the PolicyRouting information
            PolicyRouting policyRouting = new PolicyRouting();
            if (fareRestriction.RoutingId != null)
            {
                policyRouting = policyRoutingRepository.GetPolicyRouting((int)fareRestriction.RoutingId);
                policyRoutingRepository.EditPolicyRouting(policyRouting);
            }
            FareRestrictionViewModel fareRestrictionViewModel = new FareRestrictionViewModel(fareRestriction, policyRouting);
            return View(fareRestrictionViewModel);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FareRestrictionViewModel fareRestrictionViewModel)
        {
            //Get PolicyRouting Info
            PolicyRouting policyRouting = new PolicyRouting();
            policyRouting = fareRestrictionViewModel.PolicyRouting;
            policyRoutingRepository.EditPolicyRouting(policyRouting);

            FareRestriction fareRestriction = new FareRestriction();
            fareRestriction = fareRestrictionRepository.GetFareRestriction(fareRestrictionViewModel.FareRestriction.FareRestrictionId);

            //Check Exists
            if (fareRestriction == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToReferenceInfo())
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
        


            try{
               
                fareRestrictionRepository.Update(fareRestriction, policyRouting);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/FareRestriction.mvc/Edit/" + fareRestriction.FareRestrictionId.ToString();
                    return View("VersionError");
                }

                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");            
            }

            //Success
            return RedirectToAction("List");

        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FareRestrictionViewModel fareRestrictionViewModel, string btnSubmit) 
        {
            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToReferenceInfo())
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Get PolicyRouting Info
            PolicyRouting policyRouting = new PolicyRouting();
            policyRouting = fareRestrictionViewModel.PolicyRouting;

            //Get PolicyAirVendorGroupItem Info
            FareRestriction fareRestriction = new FareRestriction();
            fareRestriction = fareRestrictionViewModel.FareRestriction;

            //Edit Routing
            policyRoutingRepository.EditPolicyRouting(policyRouting);

            try
            {
                fareRestrictionRepository.Add(fareRestriction, policyRouting);
            }
            catch (SqlException ex)
            {

                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            //Send to new form based on submit button pressed
            switch (btnSubmit)
            {
                case "Save":
                    return RedirectToAction("List");
                default:
                    return RedirectToAction("CreatePolicyRouting", new { id = fareRestriction.FareRestrictionId });
            }

        }

        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id)
        {
            //Check Exists
            FareRestriction fareRestriction = new FareRestriction();
            fareRestriction = fareRestrictionRepository.GetFareRestriction(id);
            if (fareRestriction == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToReferenceInfo())
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //EditForDisplay
            fareRestrictionRepository.EditForDisplay(fareRestriction);

            //add the PolicyRouting information
            PolicyRouting policyRouting = new PolicyRouting();
            if (fareRestriction.RoutingId != null)
            {
                policyRouting = policyRoutingRepository.GetPolicyRouting((int)fareRestriction.RoutingId);
                policyRoutingRepository.EditForDisplay(policyRouting);
            }
            FareRestrictionViewModel fareRestrictionViewModel = new FareRestrictionViewModel(fareRestriction, policyRouting);

            //Show 'Create' Form
            return View(fareRestrictionViewModel);
        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            //Check Exists
            FareRestriction fareRestriction = new FareRestriction();
            fareRestriction = fareRestrictionRepository.GetFareRestriction(id);
            if (fareRestriction == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToReferenceInfo())
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Delete Item
            try
            {
                fareRestriction.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                fareRestrictionRepository.Delete(fareRestriction);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/FareRestriction.mvc/Delete/" + fareRestriction.FareRestrictionId.ToString();
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List");
        }
    }
}
