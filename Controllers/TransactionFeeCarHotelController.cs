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
    public class TransactionFeeCarHotelController : Controller
    {
        HierarchyRepository hierarchyRepository = new HierarchyRepository();
        TransactionFeeCarHotelRepository transactionFeeCarHotelRepository = new TransactionFeeCarHotelRepository();
        private string groupName = "ClientFee";

        // GET: Create A Single Transaction Fee
        public ActionResult Create(int productId)
        {
            //Check Access Rights to Domain
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ProductRepository productRepository = new ProductRepository();
            Product product = new Product();
            product = productRepository.GetProduct(productId);
            if (product == null || productId < 2 || productId > 3)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            TransactionFeeCarHotelVM transactionFeeVM = new TransactionFeeCarHotelVM();
            TransactionFeeCarHotel transactionFeeCarHotel = new TransactionFeeCarHotel();
            transactionFeeCarHotel.ProductName = product.ProductName;
            transactionFeeCarHotel.ProductId = product.ProductId;
            transactionFeeCarHotel.IncursGSTFlagNonNullable = false;
            transactionFeeVM.TransactionFee = transactionFeeCarHotel;

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

            //Location is used in CarHotel only
            PolicyLocationRepository policyLocationRepository = new PolicyLocationRepository();
            transactionFeeVM.PolicyLocations = new SelectList(policyLocationRepository.GetAllPolicyLocations().ToList(), "PolicyLocationId", "PolicyLocationName");

            return View(transactionFeeVM);
        }


        // POST: Create Transaction Fee
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TransactionFeeCarHotelVM transactionFeeCarHotelVM)
        {
            //Check Access Rights to Domain
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Get TransactionFee Info
            TransactionFeeCarHotel transactionFeeCarHotel = new TransactionFeeCarHotel();
            transactionFeeCarHotel = transactionFeeCarHotelVM.TransactionFee;

            //Update Model From Form + Validate against DB
            try
            {
                UpdateModel<TransactionFee>(transactionFeeCarHotel, "TransactionFee");
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
                transactionFeeCarHotelRepository.Add(transactionFeeCarHotel);
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

        // GET: Create Transaction Fee
        public ActionResult Edit(int id)
        {
            //Check Access Rights to Domain
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            TransactionFeeCarHotel transactionFeeCarHotel = new TransactionFeeCarHotel();
            transactionFeeCarHotel = (TransactionFeeCarHotel)transactionFeeCarHotelRepository.GetItem(id);
            if (transactionFeeCarHotel == null || transactionFeeCarHotel.ProductId < 2 || transactionFeeCarHotel.ProductId > 3)
            {
                ViewData["ActionMethod"] = "EditCarHotelGet";
                return View("RecordDoesNotExistError");
            }

			transactionFeeCarHotel.IncursGSTFlagNonNullable = ((bool)transactionFeeCarHotel.IncursGSTFlag == true) ? true : false;

            TransactionFeeCarHotelVM transactionFeeVM = new TransactionFeeCarHotelVM();
            transactionFeeVM.TransactionFee = transactionFeeCarHotel;

            TravelIndicatorRepository travelIndicatorRepository = new TravelIndicatorRepository();
            transactionFeeVM.TravelIndicators = new SelectList(travelIndicatorRepository.GetAllTravelIndicators().ToList(), "TravelIndicator1", "TravelIndicatorDescription");

            BookingSourceRepository bookingSourceRepository = new BookingSourceRepository();
            transactionFeeVM.BookingSources = new SelectList(bookingSourceRepository.GetAllBookingSources().ToList(), "BookingSourceCode", "BookingSourceCode", transactionFeeCarHotel.BookingSourceCode);

            BookingOriginationRepository bookingOriginationRepository = new BookingOriginationRepository();
            transactionFeeVM.BookingOriginations = new SelectList(bookingOriginationRepository.GetAllBookingOriginations().ToList(), "BookingOriginationCode", "BookingOriginationCode", transactionFeeCarHotel.BookingOriginationCode);

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

            //Location is used in CarHotel only
            PolicyLocationRepository policyLocationRepository = new PolicyLocationRepository();
            transactionFeeVM.PolicyLocations = new SelectList(policyLocationRepository.GetAllPolicyLocations().ToList(), "PolicyLocationId", "PolicyLocationName");

            return View(transactionFeeVM);
        }

        // POST: //Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TransactionFeeCarHotelVM transactionFeeVM)
        {
            //Check Access Rights to Domain
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            TransactionFeeCarHotel transactionFee = new TransactionFeeCarHotel();
            transactionFee = transactionFeeCarHotelRepository.GetItem(transactionFeeVM.TransactionFee.TransactionFeeId);

            //Check Exists
            if (transactionFee == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //Update  Model from Form
            try
            {
                UpdateModel(transactionFee, "TransactionFee");
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
                transactionFeeCarHotelRepository.Update(transactionFee);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List","TransactionFee");
        }


        // GET: View A Single TransactionFeeCarHotel
        public ActionResult View(int id)
        {
            //Get Item From Database
            TransactionFeeCarHotel transactionFee = new TransactionFeeCarHotel();
            transactionFee = transactionFeeCarHotelRepository.GetItem(id);

            //Check Exists
            if (transactionFee == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            TransactionFeeCarHotelVM transactionFeeCarHotelVM = new TransactionFeeCarHotelVM();

            transactionFeeCarHotelRepository.EditForDisplay(transactionFee);
            transactionFeeCarHotelVM.TransactionFee = transactionFee;

            return View(transactionFeeCarHotelVM);
        }


        // GET: Delete A Single TransactionFeeCarHotel
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
            TransactionFeeCarHotel transactionFee = new TransactionFeeCarHotel();
            transactionFee = transactionFeeCarHotelRepository.GetItem(id);

            //Check Exists
            if (transactionFee == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            TransactionFeeCarHotelVM transactionFeeCarHotelVM = new TransactionFeeCarHotelVM();

            transactionFeeCarHotelRepository.EditForDisplay(transactionFee);
            transactionFeeCarHotelVM.TransactionFee = transactionFee;

            return View(transactionFeeCarHotelVM);
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
            TransactionFeeCarHotel transactionFee = new TransactionFeeCarHotel();
            transactionFee = transactionFeeCarHotelRepository.GetItem(id);

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
                transactionFeeCarHotelRepository.Delete(transactionFee);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/TransactionFeeCarHotel.mvc/Delete/" + transactionFee.TransactionFeeId.ToString();
                    return View("VersionError");
                }
                //Restraint Error - go to standard DeleteError page
                if (ex.Message == "SQLDeleteError")
                {
                    ViewData["ReturnURL"] = "/TransactionFeeCarHotel.mvc/Delete/" + transactionFee.TransactionFeeId.ToString();
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
