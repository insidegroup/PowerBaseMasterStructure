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
    public class AirlineClassCabinController : Controller
    {
        //main repository
        AirlineClassCabinRepository airlineClassCabinRepository = new AirlineClassCabinRepository();
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

            if (sortField != "AirlineClassCode" && sortField != "Name" && sortField != "SupplierName")
            {
                sortField = "PolicyRoutingSequenceNumber";
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
                sortOrder = 0;
            }

            //return items
            var cwtPaginatedList = airlineClassCabinRepository.PageAirlineClassCabins(page ?? 1, filter ?? "", sortField, sortOrder ?? 0);
            return View(cwtPaginatedList);
        }

        // GET: /View
        public ActionResult View(string id, string sCode, int rId = 0)
        {
            //Check Parameters
			if (id == null || rId == null || sCode == null)
			{
				ViewData["ActionMethod"] = "ViewGet";
				return View("RecordDoesNotExistError");
			}
			
			//Check Exists
            AirlineClassCabin airlineClassCabin = new AirlineClassCabin();
            airlineClassCabin = airlineClassCabinRepository.GetAirlineClassCabin(id, sCode, rId);
            if (airlineClassCabin == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            //
            airlineClassCabinRepository.EditForDisplay(airlineClassCabin);

            //add the PolicyRouting information
            PolicyRouting policyRouting = new PolicyRouting();
            policyRouting = policyRoutingRepository.GetPolicyRouting(airlineClassCabin.PolicyRoutingId);

            AirlineClassCabinViewModel airlineClassCabinViewModel = new AirlineClassCabinViewModel(airlineClassCabin, policyRouting);

            //Show 'Create' Form
            return View(airlineClassCabinViewModel);
        }

        // GET: /Create
        public ActionResult Create()
        {
            AirlineClassCabin airlineClassCabin = new AirlineClassCabin();

            //Check Exists
            if (airlineClassCabin == null)
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

            AirlineClassCabinViewModel airlineClassCabinViewModel = new AirlineClassCabinViewModel(airlineClassCabin, policyRouting);

            //Show 'Create' Form
            return View(airlineClassCabinViewModel);
        }


        // GET: /Edit
        public ActionResult Edit(string id, string sCode, int? rId)
        {
            //Check Parameters
			int policyRoutingId;
			if (!Int32.TryParse(rId.ToString(), out policyRoutingId))
			{
				ViewData["Message"] = "Incorrect Paramters";
				return View("Error");
			}
			
			//Check Exists
            AirlineClassCabin airlineClassCabin = new AirlineClassCabin();
			airlineClassCabin = airlineClassCabinRepository.GetAirlineClassCabin(id, sCode, policyRoutingId);
            if (airlineClassCabin == null)
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

            //Add Linked Table Info
            airlineClassCabinRepository.EditForDisplay(airlineClassCabin);

            //add the PolicyRouting information
            PolicyRouting policyRouting = new PolicyRouting();
            policyRouting = policyRoutingRepository.GetPolicyRouting(airlineClassCabin.PolicyRoutingId);

            AirlineClassCabinViewModel airlineClassCabinViewModel = new AirlineClassCabinViewModel(airlineClassCabin, policyRouting);
            return View(airlineClassCabinViewModel);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, string sCode, int rId, FormCollection collection)
        {

            //Check Exists
            AirlineClassCabin airlineClassCabin = new AirlineClassCabin();
            airlineClassCabin = airlineClassCabinRepository.GetAirlineClassCabin(id, sCode, rId);
            if (airlineClassCabin == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }



            //Update Item from Form
            try
            {
                UpdateModel(airlineClassCabin, "AirlineClassCabin");
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

            //add the PolicyRouting information
            PolicyRouting policyRouting = new PolicyRouting();
            policyRouting = policyRoutingRepository.GetPolicyRouting(airlineClassCabin.PolicyRoutingId);
            try
            {
                UpdateModel(policyRouting, "PolicyRouting");
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

			//Edit Routing
			policyRoutingRepository.EditPolicyRouting(policyRouting);

            try
            {
                airlineClassCabinRepository.Update(airlineClassCabin, policyRouting);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/AirlineClassCabin.mvc/Edit/" + airlineClassCabin.AirlineClassCode;
                    return View("VersionError");
                }

                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");            }

            //Success
            return RedirectToAction("List");

        }
       
        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AirlineClassCabinViewModel airlineClassCabinViewModel, string btnSubmit)
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
            policyRouting = airlineClassCabinViewModel.PolicyRouting;

            //Get PolicyAirlineGroupItem Info
            AirlineClassCabin airlineClassCabin = new AirlineClassCabin();
            airlineClassCabin = airlineClassCabinViewModel.AirlineClassCabin;

            //Edit Routing
            policyRoutingRepository.EditPolicyRouting(policyRouting);

            try
            {
                airlineClassCabinRepository.Add(airlineClassCabin, policyRouting);
            }
            catch (SqlException ex)
            {

                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List");

            //Send to new form based on submit button pressed
            //switch (btnSubmit)
            //{
            //    case "Save":
           //         return RedirectToAction("List");
           //     default:
           //         return RedirectToAction("CreatePolicyRouting", new { id = airlineClassCabin.AirlineClassCode });
           // }

        }

        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(string id, string sCode, int rId)
        {
            //Check Exists
            AirlineClassCabin airlineClassCabin = new AirlineClassCabin();
            airlineClassCabin = airlineClassCabinRepository.GetAirlineClassCabin(id, sCode, rId);
            if (airlineClassCabin == null)
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

            //Add Linked Table Info
            airlineClassCabinRepository.EditForDisplay(airlineClassCabin);

            //add the PolicyRouting information
            PolicyRouting policyRouting = new PolicyRouting();
            policyRouting = policyRoutingRepository.GetPolicyRouting(airlineClassCabin.PolicyRoutingId);

            AirlineClassCabinViewModel airlineClassCabinViewModel = new AirlineClassCabinViewModel(airlineClassCabin, policyRouting);

            //Show 'Create' Form
            return View(airlineClassCabinViewModel);
        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, string sCode, int rId, FormCollection collection)
        {
            //Check Exists
            AirlineClassCabin airlineClassCabin = new AirlineClassCabin();
            airlineClassCabin = airlineClassCabinRepository.GetAirlineClassCabin(id, sCode, rId);
            if (airlineClassCabin == null)
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
                airlineClassCabin.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                airlineClassCabinRepository.Delete(airlineClassCabin);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/AirlineClassCabin.mvc/Delete/" + airlineClassCabin.AirlineCabinCode;
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
