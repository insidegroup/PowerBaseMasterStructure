using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Repository;
using CWTDesktopDatabase.ViewModels;
using System.Data.SqlClient;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Controllers
{
    public class TransactionFeeAirController : Controller
    {
        HierarchyRepository hierarchyRepository = new HierarchyRepository();
        TransactionFeeAirRepository transactionFeeAirRepository = new TransactionFeeAirRepository();
        PolicyRoutingRepository policyRoutingRepository = new PolicyRoutingRepository();
        private string groupName = "ClientFee";


        // POST:  CountryRegions of a Values for ServicingOptionItem
        [HttpPost]
        public JsonResult GetTransactionFeeAirs()
        {
            var result = transactionFeeAirRepository.GetAllTransactionFeeAirs();
            return Json(result);
        }


        // GET: Create A Single ClientFee
        public ActionResult Create()
        {
            int productId = 1;

            //Check Access Rights to Domain
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ProductRepository productRepository = new ProductRepository();
            Product product = new Product();
            product = productRepository.GetProduct(productId);

            TransactionFeeAirVM transactionFeeVM = new TransactionFeeAirVM();
            TransactionFeeAir transactionFee = new TransactionFeeAir();
            transactionFee.ProductName = product.ProductName;
            transactionFee.ProductId = product.ProductId;
            transactionFee.IncursGSTFlagNonNullable = false;
            transactionFeeVM.TransactionFee = transactionFee;

            TravelIndicatorRepository travelIndicatorRepository = new TravelIndicatorRepository();
            transactionFeeVM.TravelIndicators = new SelectList(travelIndicatorRepository.GetAllTravelIndicators().ToList(), "TravelIndicator1", "TravelIndicatorDescription");

            BookingSourceRepository bookingSourceRepository = new BookingSourceRepository();
            transactionFeeVM.BookingSources = new SelectList(bookingSourceRepository.GetAllBookingSources().ToList(), "BookingSourceCode", "BookingSourceCode");

            BookingOriginationRepository bookingOriginationRepository = new BookingOriginationRepository();
            transactionFeeVM.BookingOriginations = new SelectList(bookingOriginationRepository.GetAllBookingOriginations().ToList(), "BookingOriginationCode", "BookingOriginationCode");

            ChargeTypeRepository chargeTypeRepository = new ChargeTypeRepository();
            transactionFeeVM.ChargeTypes = new SelectList(chargeTypeRepository.GetAllChargeTypes().ToList(), "ChargeTypeCode", "ChargeTypeDescription");

            TransactionTypeRepository transactionTypeRepository = new TransactionTypeRepository();
            transactionFeeVM.TransactionTypes = new SelectList(transactionTypeRepository.GetAllTransactionTypes().ToList(), "TransactionTypeCode", "TransactionTypeCode");

            FeeCategoryRepository feeCategoryRepository = new FeeCategoryRepository();
            transactionFeeVM.FeeCategories = new SelectList(feeCategoryRepository.GetAllFeeCategories().ToList(), "FeeCategory1", "FeeCategory1");

            TravelerBackOfficeTypeRepository travelerBackOfficeTypeRepository = new TravelerBackOfficeTypeRepository();
            transactionFeeVM.TravelerBackOfficeTypes = new SelectList(travelerBackOfficeTypeRepository.GetAllTravelerBackOfficeTypes().ToList(), "TravelerBackOfficeTypeCode", "TravelerBackOfficeTypeDescription");

            TripTypeClassificationRepository tripTypeClassificationRepository = new TripTypeClassificationRepository();
            transactionFeeVM.TripTypeClassifications = new SelectList(tripTypeClassificationRepository.GetAllTripTypeClassifications().ToList(), "TripTypeClassificationId", "TripTypeClassificationDescription");
           
            CurrencyRepository currencyRepository = new CurrencyRepository();
            transactionFeeVM.TicketPriceCurrencies = new SelectList(currencyRepository.GetAllCurrencies().ToList(), "CurrencyCode", "Name");
            transactionFeeVM.FeeCurrencies = new SelectList(currencyRepository.GetAllCurrencies().ToList(), "CurrencyCode", "Name");

            PolicyRouting policyRouting = new PolicyRouting();
            policyRouting.FromGlobalFlag = false;
            policyRouting.ToGlobalFlag = false;
            transactionFeeVM.PolicyRouting = policyRouting;

            return View(transactionFeeVM);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TransactionFeeAirVM transactionFeeAirVM)
        {
            //Check Access Rights to Domain
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }


            //Get PolicyRouting Info
            PolicyRouting policyRouting = new PolicyRouting();
            policyRouting = transactionFeeAirVM.PolicyRouting;

            //Get TransactionFee Info
            TransactionFeeAir transactionFee = new TransactionFeeAir();
            transactionFee = transactionFeeAirVM.TransactionFee;

            //Edit Routing
            policyRoutingRepository.EditPolicyRouting(policyRouting);

            //Database Update
            try
            {
                transactionFeeAirRepository.Add(transactionFee, policyRouting);
            }
            catch (SqlException ex)
            {
                //Non-Unique Name
                if (ex.Message == "NonUniqueName")
                {
                    return View("NonUniqueNameError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List", "TransactionFee");
        }

        // GET: Create TransactionFeeAir
        public ActionResult Edit(int id)
        {
            //Check Access Rights to Domain
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            TransactionFeeAir transactionFeeAir = new TransactionFeeAir();
            transactionFeeAir = transactionFeeAirRepository.GetItem(id);
            if (transactionFeeAir == null || transactionFeeAir.ProductId != 1)
            {
                ViewData["ActionMethod"] = "EditAirGet";
                return View("RecordDoesNotExistError");
            }

			transactionFeeAir.IncursGSTFlagNonNullable = ((bool)transactionFeeAir.IncursGSTFlag == true) ? true : false;

            TransactionFeeAirVM transactionFeeVM = new TransactionFeeAirVM();
            transactionFeeVM.TransactionFee = transactionFeeAir;

            TravelIndicatorRepository travelIndicatorRepository = new TravelIndicatorRepository();
            transactionFeeVM.TravelIndicators = new SelectList(travelIndicatorRepository.GetAllTravelIndicators().ToList(), "TravelIndicator1", "TravelIndicatorDescription");

            BookingSourceRepository bookingSourceRepository = new BookingSourceRepository();
            transactionFeeVM.BookingSources = new SelectList(bookingSourceRepository.GetAllBookingSources().ToList(), "BookingSourceCode", "BookingSourceCode");

            BookingOriginationRepository bookingOriginationRepository = new BookingOriginationRepository();
            transactionFeeVM.BookingOriginations = new SelectList(bookingOriginationRepository.GetAllBookingOriginations().ToList(), "BookingOriginationCode", "BookingOriginationCode");

            ChargeTypeRepository chargeTypeRepository = new ChargeTypeRepository();
            transactionFeeVM.ChargeTypes = new SelectList(chargeTypeRepository.GetAllChargeTypes().ToList(), "ChargeTypeCode", "ChargeTypeDescription");

            TransactionTypeRepository transactionTypeRepository = new TransactionTypeRepository();
            transactionFeeVM.TransactionTypes = new SelectList(transactionTypeRepository.GetAllTransactionTypes().ToList(), "TransactionTypeCode", "TransactionTypeCode");

            FeeCategoryRepository feeCategoryRepository = new FeeCategoryRepository();
            transactionFeeVM.FeeCategories = new SelectList(feeCategoryRepository.GetAllFeeCategories().ToList(), "FeeCategory1", "FeeCategory1");

            TravelerBackOfficeTypeRepository travelerBackOfficeTypeRepository = new TravelerBackOfficeTypeRepository();
            transactionFeeVM.TravelerBackOfficeTypes = new SelectList(travelerBackOfficeTypeRepository.GetAllTravelerBackOfficeTypes().ToList(), "TravelerBackOfficeTypeCode", "TravelerBackOfficeTypeDescription");

            TripTypeClassificationRepository tripTypeClassificationRepository = new TripTypeClassificationRepository();
            transactionFeeVM.TripTypeClassifications = new SelectList(tripTypeClassificationRepository.GetAllTripTypeClassifications().ToList(), "TripTypeClassificationId", "TripTypeClassificationDescription");

            CurrencyRepository currencyRepository = new CurrencyRepository();
            transactionFeeVM.TicketPriceCurrencies = new SelectList(currencyRepository.GetAllCurrencies().ToList(), "CurrencyCode", "Name");
            transactionFeeVM.FeeCurrencies = new SelectList(currencyRepository.GetAllCurrencies().ToList(), "CurrencyCode", "Name");

            PolicyRouting policyRouting = new PolicyRouting();
            if (transactionFeeAir.PolicyRoutingId != null)
            {
                policyRouting = policyRoutingRepository.GetPolicyRouting((int)transactionFeeAir.PolicyRoutingId);
                policyRoutingRepository.EditPolicyRouting(policyRouting);
            }
            //
            transactionFeeVM.PolicyRouting = policyRouting;

            return View(transactionFeeVM);
        }

        // POST: //Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TransactionFeeAirVM transactionFeeAirVM)
        {
            //Check Access Rights to Domain
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            TransactionFeeAir transactionFee = new TransactionFeeAir();
            transactionFee = transactionFeeAirRepository.GetItem(transactionFeeAirVM.TransactionFee.TransactionFeeId);

            PolicyRouting policyRouting = new PolicyRouting();
            policyRouting = transactionFeeAirVM.PolicyRouting;

            //Check Exists
            if (transactionFee == null || policyRouting == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //Update  Model from Form
            try
            {
                UpdateModel(transactionFee, "TransactionFee");
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

            //add the PolicyRouting information
            //PolicyRouting policyRouting = new PolicyRouting();
            //policyRouting = transactionFeeAirVM.PolicyRouting;
            /*
            policyRouting = policyRoutingRepository.GetPolicyRouting((int)transactionFee.PolicyRoutingId);
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
            */
            //Edit Routing
            policyRoutingRepository.EditPolicyRouting(policyRouting);

            //Database Update
            try
            {
                transactionFeeAirRepository.Update(transactionFee, policyRouting);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List", "TransactionFee");
        }

        // GET: View A Single TransactionFeeAir
        public ActionResult View(int id)
        {
            //Get Item From Database
            TransactionFeeAir transactionFee = new TransactionFeeAir();
            transactionFee = transactionFeeAirRepository.GetItem(id);

            //Check Exists
            if (transactionFee == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            TransactionFeeAirVM transactionFeeAirVM = new TransactionFeeAirVM();
            transactionFeeAirRepository.EditForDisplay(transactionFee);
            transactionFeeAirVM.TransactionFee = transactionFee;

            PolicyRouting policyRouting = new PolicyRouting();
            if (transactionFee.PolicyRoutingId != null)
            {
                policyRouting = policyRoutingRepository.GetPolicyRouting((int)transactionFee.PolicyRoutingId);
                policyRoutingRepository.EditPolicyRouting(policyRouting);
            }
           
            transactionFeeAirVM.PolicyRouting = policyRouting;

            return View(transactionFeeAirVM);
        }

        // GET: Delete A Single TransactionFeeAir
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
            TransactionFeeAir transactionFee = new TransactionFeeAir();
            transactionFee = transactionFeeAirRepository.GetItem(id);

            //Check Exists
            if (transactionFee == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            TransactionFeeAirVM transactionFeeAirVM = new TransactionFeeAirVM();
            transactionFeeAirRepository.EditForDisplay(transactionFee);
            transactionFeeAirVM.TransactionFee = transactionFee;

            PolicyRouting policyRouting = new PolicyRouting();
            if (transactionFee.PolicyRoutingId != null)
            {
                policyRouting = policyRoutingRepository.GetPolicyRouting((int)transactionFee.PolicyRoutingId);
                policyRoutingRepository.EditPolicyRouting(policyRouting);
            }
            transactionFeeAirVM.PolicyRouting = policyRouting;

            return View(transactionFeeAirVM);
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
            TransactionFeeAir transactionFee = new TransactionFeeAir();
            transactionFee = transactionFeeAirRepository.GetItem(id);

            //Check Exists
            if (transactionFee == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            //Delete Item
            try
            {
                transactionFee.VersionNumber = Int32.Parse(collection["TransactionFee.VersionNumber"]);
                transactionFeeAirRepository.Delete(transactionFee);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/TransactionFeeAir.mvc/Delete/" + transactionFee.TransactionFeeId.ToString();
                    return View("VersionError");
                }
                //Restraint Error - go to standard DeleteError page
                if (ex.Message == "SQLDeleteError")
                {
                    ViewData["ReturnURL"] = "/TransactionFeeAir.mvc/Delete/" + transactionFee.TransactionFeeId.ToString();
                    return View("DeleteError");
                }
				
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            //Return
            return RedirectToAction("List", "TransactionFee");
        }
    }
}
