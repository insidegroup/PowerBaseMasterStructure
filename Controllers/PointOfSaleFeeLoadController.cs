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
using System.Xml;
using System.Collections;

namespace CWTDesktopDatabase.Controllers
{
    public class PointOfSaleFeeLoadController : Controller
    {
        //main repository
        PointOfSaleFeeLoadRepository pointOfSaleFeeLoadRepository = new PointOfSaleFeeLoadRepository();
        HierarchyRepository hierarchyRepository = new HierarchyRepository();

        private string groupName = "Point of Sale Fee Administrator";

        // GET: /List
        public ActionResult List(
            int? page,
            string sortField,
            int? sortOrder,
            string clientTopUnitGuid,
            string clientTopUnitName,
            string clientSubUnitGuid,
            string clientSubUnitName,
            string travelerTypeGuid,
            string travelerTypeName)
        {

            //SortField
            if (string.IsNullOrEmpty(sortField))
            {
                sortField = "ClientTopUnitName";
            }
            ViewData["CurrentSortField"] = sortField;

            //SortOrder
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

            PointOfSaleFeeLoadsVM pointOfSaleFeeLoadsVM = new PointOfSaleFeeLoadsVM();

            //Set Access Rights
            if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                pointOfSaleFeeLoadsVM.HasDomainWriteAccess = true;
            }

            if (pointOfSaleFeeLoadRepository != null)
            {
                var pointOfSaleFeeLoads = pointOfSaleFeeLoadRepository.PagePointOfSaleFeeLoads(
                    !string.IsNullOrEmpty(clientTopUnitGuid) ? clientTopUnitGuid : null,
                    !string.IsNullOrEmpty(clientSubUnitGuid) ? clientSubUnitGuid : null,
                    !string.IsNullOrEmpty(travelerTypeGuid) ? travelerTypeGuid : null,
                    page ?? 1,
                    sortField,
                    sortOrder ?? 0
                );

                if (pointOfSaleFeeLoads != null)
                {
                    pointOfSaleFeeLoadsVM.PointOfSaleFeeLoads = pointOfSaleFeeLoads;
                }
            }

            //Fields
            pointOfSaleFeeLoadsVM.ClientTopUnitGuid = clientTopUnitGuid ?? "";
            pointOfSaleFeeLoadsVM.ClientTopUnitName = clientTopUnitName ?? "";

            pointOfSaleFeeLoadsVM.ClientSubUnitGuid = clientSubUnitGuid ?? "";
            pointOfSaleFeeLoadsVM.ClientSubUnitName = clientSubUnitName ?? "";

            pointOfSaleFeeLoadsVM.TravelerTypeGuid = travelerTypeGuid ?? "";
            pointOfSaleFeeLoadsVM.TravelerTypeName = travelerTypeName ?? "";

            return View(pointOfSaleFeeLoadsVM);
        }


        // GET: /Create		
        public ActionResult Create()
        {
            //Check Access Rights to Domain
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            PointOfSaleFeeLoadVM pointOfSaleFeeLoadVM = new PointOfSaleFeeLoadVM();

            //AgentInitiatedFlag Default Checked
            PointOfSaleFeeLoad pointOfSaleFeeLoad = new PointOfSaleFeeLoad();
            pointOfSaleFeeLoad.AgentInitiatedFlag = true;

            pointOfSaleFeeLoadVM.PointOfSaleFeeLoad = pointOfSaleFeeLoad;

            //FeeLoadDescriptionTypeCodes
            FeeLoadDescriptionTypeCodeRepository feeLoadDescriptionTypeCodeRepository = new FeeLoadDescriptionTypeCodeRepository();
            SelectList feeLoadDescriptionTypeCodes = new SelectList(feeLoadDescriptionTypeCodeRepository.GetAllFeeLoadDescriptionTypeCodes().ToList(), "FeeLoadDescriptionTypeCode", "FeeLoadDescriptionTypeCode");
            pointOfSaleFeeLoadVM.FeeLoadDescriptionTypeCodes = feeLoadDescriptionTypeCodes;

            //Products
            ProductRepository productRepository = new ProductRepository();
            SelectList products = new SelectList(productRepository.GetAllProducts().ToList(), "ProductId", "ProductName");
            pointOfSaleFeeLoadVM.Products = products;

            //TravelIndicators
            TravelIndicatorRepository TravelIndicatorRepository = new TravelIndicatorRepository();
            SelectList travelIndicators = new SelectList(TravelIndicatorRepository.GetAllTravelIndicators().OrderBy(x => x.TravelIndicatorDescription).ToList(), "TravelIndicator1", "TravelIndicatorDescription");
            pointOfSaleFeeLoadVM.TravelIndicators = travelIndicators;

            //Currencies
            CurrencyRepository currencyRepository = new CurrencyRepository();
            SelectList currencies = new SelectList(currencyRepository.GetAllCurrencies().ToList(), "CurrencyCode", "Name");
            pointOfSaleFeeLoadVM.Currencies = currencies;

            return View(pointOfSaleFeeLoadVM);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PointOfSaleFeeLoadVM pointOfSaleFeeLoadVM, FormCollection formCollection)
        {
            //Check Access Rights to Domain
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //We need to extract group from groupVM
            PointOfSaleFeeLoad pointOfSaleFeeLoad = new PointOfSaleFeeLoad();
            pointOfSaleFeeLoad = pointOfSaleFeeLoadVM.PointOfSaleFeeLoad;
            if (pointOfSaleFeeLoad == null)
            {
                ViewData["Message"] = "ValidationError : missing item"; ;
                return View("Error");
            }

            //Update Model From Form + Validate against DB
            try
            {
                UpdateModel<PointOfSaleFeeLoad>(pointOfSaleFeeLoad, "PointOfSaleFeeLoad");
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
                pointOfSaleFeeLoadRepository.Add(pointOfSaleFeeLoadVM);
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

            ViewData["NewSortOrder"] = 0;

            return RedirectToAction("List");
        }

        // GET: /Edit
        public ActionResult Edit(int id)
        {
            //Get Item From Database
            PointOfSaleFeeLoad pointOfSaleFeeLoad = new PointOfSaleFeeLoad();
            pointOfSaleFeeLoad = pointOfSaleFeeLoadRepository.GetGroup(id);

            //Check Exists
            if (pointOfSaleFeeLoad == null)
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

            PointOfSaleFeeLoadVM pointOfSaleFeeLoadVM = new PointOfSaleFeeLoadVM();

            pointOfSaleFeeLoadRepository.EditGroupForDisplay(pointOfSaleFeeLoad);

            pointOfSaleFeeLoadVM.PointOfSaleFeeLoad = pointOfSaleFeeLoad;

            //FeeLoadDescriptionTypeCodes
            FeeLoadDescriptionTypeCodeRepository feeLoadDescriptionTypeCodeRepository = new FeeLoadDescriptionTypeCodeRepository();
            SelectList feeLoadDescriptionTypeCodes = new SelectList(feeLoadDescriptionTypeCodeRepository.GetAllFeeLoadDescriptionTypeCodes().ToList(), "FeeLoadDescriptionTypeCode", "FeeLoadDescriptionTypeCode", pointOfSaleFeeLoad.FeeLoadDescriptionTypeCode);
            pointOfSaleFeeLoadVM.FeeLoadDescriptionTypeCodes = feeLoadDescriptionTypeCodes;

            //Products
            ProductRepository productRepository = new ProductRepository();
            SelectList products = new SelectList(productRepository.GetAllProducts().ToList(), "ProductId", "ProductName", pointOfSaleFeeLoad.ProductId);
            pointOfSaleFeeLoadVM.Products = products;

            //TravelIndicators
            TravelIndicatorRepository TravelIndicatorRepository = new TravelIndicatorRepository();
            SelectList travelIndicators = new SelectList(TravelIndicatorRepository.GetAllTravelIndicators().OrderBy(x => x.TravelIndicatorDescription).ToList(), "TravelIndicator1", "TravelIndicatorDescription", pointOfSaleFeeLoad.TravelIndicator);
            pointOfSaleFeeLoadVM.TravelIndicators = travelIndicators;

            //Currencies
            CurrencyRepository currencyRepository = new CurrencyRepository();
            SelectList currencies = new SelectList(currencyRepository.GetAllCurrencies().ToList(), "CurrencyCode", "Name", pointOfSaleFeeLoad.FeeLoadCurrencyCode);
            pointOfSaleFeeLoadVM.Currencies = currencies;

            return View(pointOfSaleFeeLoadVM);
        }

        // POST: /Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PointOfSaleFeeLoadVM pointOfSaleFeeLoadVM, FormCollection formCollection)
        {
            //Get Item
            PointOfSaleFeeLoad pointOfSaleFeeLoad = new PointOfSaleFeeLoad();
            pointOfSaleFeeLoad = pointOfSaleFeeLoadRepository.GetGroup(pointOfSaleFeeLoadVM.PointOfSaleFeeLoad.PointOfSaleFeeLoadId);

            //Check Exists
            if (pointOfSaleFeeLoad == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //Check Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPointOfSaleFeeLoad(pointOfSaleFeeLoadVM.PointOfSaleFeeLoad.PointOfSaleFeeLoadId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update Model From Form + Validate against DB
            try
            {
                UpdateModel<PointOfSaleFeeLoad>(pointOfSaleFeeLoadVM.PointOfSaleFeeLoad, "PointOfSaleFeeLoad");
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
                pointOfSaleFeeLoadRepository.Edit(pointOfSaleFeeLoadVM);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/PointOfSaleFeeLoad.mvc/Edit/" + pointOfSaleFeeLoadVM.PointOfSaleFeeLoad.PointOfSaleFeeLoadId;
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List");
        }

        // GET: /Delete
        [HttpGet]
        public ActionResult Delete(int id)
        {
            //Get Item From Database
            PointOfSaleFeeLoad pointOfSaleFeeLoad = new PointOfSaleFeeLoad();
            pointOfSaleFeeLoad = pointOfSaleFeeLoadRepository.GetGroup(id);

            //Check Exists
            if (pointOfSaleFeeLoad == null)
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

            PointOfSaleFeeLoadVM pointOfSaleFeeLoadVM = new PointOfSaleFeeLoadVM();

            pointOfSaleFeeLoadRepository.EditGroupForDisplay(pointOfSaleFeeLoad);

            pointOfSaleFeeLoadVM.PointOfSaleFeeLoad = pointOfSaleFeeLoad;

            return View(pointOfSaleFeeLoadVM);
        }

        // POST: /Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(PointOfSaleFeeLoadVM pointOfSaleFeeLoadVM)
        {
            //Check Valid Item passed in Form       
            if (pointOfSaleFeeLoadVM.PointOfSaleFeeLoad == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            //Get Item From Database
            PointOfSaleFeeLoad pointOfSaleFeeLoad = new PointOfSaleFeeLoad();
            pointOfSaleFeeLoad = pointOfSaleFeeLoadRepository.GetGroup(pointOfSaleFeeLoadVM.PointOfSaleFeeLoad.PointOfSaleFeeLoadId);

            //Check Exists in Databsase
            if (pointOfSaleFeeLoad == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPointOfSaleFeeLoad(pointOfSaleFeeLoadVM.PointOfSaleFeeLoad.PointOfSaleFeeLoadId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Delete Form Item
            try
            {
                pointOfSaleFeeLoadRepository.Delete(pointOfSaleFeeLoadVM.PointOfSaleFeeLoad);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/PointOfSaleFeeLoad.mvc/Delete/" + pointOfSaleFeeLoadVM.PointOfSaleFeeLoad.PointOfSaleFeeLoadId;
                    return View("VersionError");
                }

                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }
            return RedirectToAction("List");
        }

        //AutoComplete Client SubUnits based on Client TopUnit
        [HttpPost]
        public JsonResult AutoCompleteClientTopUnitClientSubUnits(string searchText, string clientTopUnitGuid)
        {
            HierarchyRepository hierarchyRepository = new HierarchyRepository();

            int maxResults = 1000;
            var result = hierarchyRepository.LookUpSystemUserClientTopUnitClientSubUnits(searchText, maxResults, clientTopUnitGuid, groupName);
            return Json(result);
        }

        [HttpPost]
        public JsonResult AutoCompleteClientSubUnitTravelerTypes(string searchText, string hierarchyItem, string filterText)
        {
            HierarchyRepository hierarchyRepository = new HierarchyRepository();

            int maxResults = 15;
            var result = hierarchyRepository.LookUpSystemUserClientSubUnitTravelerTypes(searchText, hierarchyItem, maxResults, groupName, filterText);
            return Json(result);
        }

        [HttpPost]
        public JsonResult AutoCompleteClientTopUnitTravelerTypes(string searchText, string clientTopUnitGuid)
        {
            HierarchyRepository hierarchyRepository = new HierarchyRepository();

            int maxResults = 15;
            var result = hierarchyRepository.LookUpSystemUserClientTopUnitTravelerTypes(searchText, maxResults, clientTopUnitGuid, groupName);
            return Json(result);
        }
    }
}
