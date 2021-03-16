using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Repository;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using System.Data.SqlClient;

namespace CWTDesktopDatabase.Controllers
{
    public class PolicyAirMissedSavingsThresholdGroupItemController : Controller
    {
        //main repositories
        PolicyAirMissedSavingsThresholdGroupItemRepository policyAirMissedSavingsThresholdGroupItemRepository = new PolicyAirMissedSavingsThresholdGroupItemRepository();
        PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();

        //GET: A list of PolicyAirMissedSavingsThresholdGroupItems for this PolicyGroup
        public ActionResult List(int id, int? page, string sortField, int? sortOrder)
        {
            //Check Parent Exists
            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(id);
            if (policyGroup == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }

            //SortField + SortOrder settings
            if (string.IsNullOrEmpty(sortField))
            {
                sortField = "MissedThresholdAmount";
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

            PolicyAirMissedSavingsThresholdGroupItemsVM policyAirMissedSavingsThresholdGroupItemsVM = new PolicyAirMissedSavingsThresholdGroupItemsVM();
            policyAirMissedSavingsThresholdGroupItemsVM.PolicyAirMissedSavingsThresholdGroupItems = policyAirMissedSavingsThresholdGroupItemRepository.GetPolicyAirMissedSavingsThresholdGroupItems(id, sortField, sortOrder ?? 0, page ?? 1);
            policyAirMissedSavingsThresholdGroupItemsVM.PolicyGroup = policyGroup;

            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToPolicyGroup(id))
            {
                policyAirMissedSavingsThresholdGroupItemsVM.HasWriteAccess = true;
            }

            return View(policyAirMissedSavingsThresholdGroupItemsVM);
        }

        // GET: /Create
        public ActionResult Create(int id)
        {
            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(id); ;

            //Check Exists
            if (policyGroup == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(id))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Create ViewModel
            PolicyAirMissedSavingsThresholdGroupItemVM policyAirMissedSavingsThresholdGroupItemVM = new PolicyAirMissedSavingsThresholdGroupItemVM();
            policyAirMissedSavingsThresholdGroupItemVM.PolicyGroup = policyGroup;

            PolicyAirMissedSavingsThresholdGroupItem policyAirMissedSavingsThresholdGroupItem = new PolicyAirMissedSavingsThresholdGroupItem();
            policyAirMissedSavingsThresholdGroupItem.PolicyGroupId = policyGroup.PolicyGroupId;
            policyAirMissedSavingsThresholdGroupItem.PolicyProhibitedFlag = false;
            policyAirMissedSavingsThresholdGroupItem.SavingsZeroedOutFlag = false;
            policyAirMissedSavingsThresholdGroupItemVM.PolicyAirMissedSavingsThresholdGroupItem = policyAirMissedSavingsThresholdGroupItem;

            //Currencies
            CurrencyRepository currencyRepository = new CurrencyRepository();
            policyAirMissedSavingsThresholdGroupItemVM.Currencies = new SelectList(currencyRepository.GetAllCurrencies().ToList(), "CurrencyCode", "Name");
            
            //RoutingCodes
            PolicyAirMissedSavingsThresholdRoutingRepository routingRepository = new PolicyAirMissedSavingsThresholdRoutingRepository();
            policyAirMissedSavingsThresholdGroupItemVM.RoutingCodes = new SelectList(routingRepository.GetAllPolicyAirMissedSavingsThresholdRoutings().ToList(), "RoutingCode", "RoutingDescription");

            //Return Form to Users
            return View(policyAirMissedSavingsThresholdGroupItemVM);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PolicyAirMissedSavingsThresholdGroupItemVM policyAirMissedSavingsThresholdGroupItemVM)
        {

            //Get PolicyAirMissedSavingsThresholdGroupItem Info
            PolicyAirMissedSavingsThresholdGroupItem policyAirMissedSavingsThresholdGroupItem = new PolicyAirMissedSavingsThresholdGroupItem();
            policyAirMissedSavingsThresholdGroupItem = policyAirMissedSavingsThresholdGroupItemVM.PolicyAirMissedSavingsThresholdGroupItem;

            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(policyAirMissedSavingsThresholdGroupItem.PolicyGroupId);

            //Check Exists
            if (policyGroup == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyGroup.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update Model From Form + Validate against DB
            try
            {
                UpdateModel<PolicyAirMissedSavingsThresholdGroupItem>(policyAirMissedSavingsThresholdGroupItemVM.PolicyAirMissedSavingsThresholdGroupItem, "PolicyAirMissedSavingsThresholdGroupItem");
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

            //Save To DB
            try
            {
                policyAirMissedSavingsThresholdGroupItemRepository.Add(policyAirMissedSavingsThresholdGroupItem);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List", new { id = policyGroup.PolicyGroupId });

        }

        // GET: /Edit
        public ActionResult Edit(int id)
        {
            PolicyAirMissedSavingsThresholdGroupItem policyAirMissedSavingsThresholdGroupItem = new PolicyAirMissedSavingsThresholdGroupItem();
            policyAirMissedSavingsThresholdGroupItem = policyAirMissedSavingsThresholdGroupItemRepository.GetPolicyAirMissedSavingsThresholdGroupItem(id);

            //Check Exists
            if (policyAirMissedSavingsThresholdGroupItem == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyAirMissedSavingsThresholdGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Create ViewModel
            PolicyAirMissedSavingsThresholdGroupItemVM policyAirMissedSavingsThresholdGroupItemVM = new PolicyAirMissedSavingsThresholdGroupItemVM();

            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(policyAirMissedSavingsThresholdGroupItem.PolicyGroupId);
            policyAirMissedSavingsThresholdGroupItemVM.PolicyGroup = policyGroup;

            policyAirMissedSavingsThresholdGroupItemVM.PolicyAirMissedSavingsThresholdGroupItem = policyAirMissedSavingsThresholdGroupItem;

            //Currencies
            CurrencyRepository currencyRepository = new CurrencyRepository();
            policyAirMissedSavingsThresholdGroupItemVM.Currencies = new SelectList(currencyRepository.GetAllCurrencies().ToList(), "CurrencyCode", "Name", policyAirMissedSavingsThresholdGroupItem.CurrencyCode);

            //RoutingCodes
            PolicyAirMissedSavingsThresholdRoutingRepository routingRepository = new PolicyAirMissedSavingsThresholdRoutingRepository();
            policyAirMissedSavingsThresholdGroupItemVM.RoutingCodes = new SelectList(routingRepository.GetAllPolicyAirMissedSavingsThresholdRoutings().ToList(), "RoutingCode", "RoutingDescription", policyAirMissedSavingsThresholdGroupItem.RoutingCode);

            //Return Form to Users
            return View(policyAirMissedSavingsThresholdGroupItemVM);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PolicyAirMissedSavingsThresholdGroupItemVM policyAirMissedSavingsThresholdGroupItemVM)
        {

            //Get PolicyAirMissedSavingsThresholdGroupItem
            PolicyAirMissedSavingsThresholdGroupItem policyAirMissedSavingsThresholdGroupItem = new PolicyAirMissedSavingsThresholdGroupItem();
            policyAirMissedSavingsThresholdGroupItem = policyAirMissedSavingsThresholdGroupItemRepository.GetPolicyAirMissedSavingsThresholdGroupItem(policyAirMissedSavingsThresholdGroupItemVM.PolicyAirMissedSavingsThresholdGroupItem.PolicyAirMissedSavingsThresholdGroupItemId);


            //Check Exists
            if (policyAirMissedSavingsThresholdGroupItem == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyAirMissedSavingsThresholdGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update Model From Form + Validate against DB
            try
            {
                UpdateModel<PolicyAirMissedSavingsThresholdGroupItem>(policyAirMissedSavingsThresholdGroupItemVM.PolicyAirMissedSavingsThresholdGroupItem, "PolicyAirMissedSavingsThresholdGroupItem");
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

            //Save To DB
            try
            {
                policyAirMissedSavingsThresholdGroupItemRepository.Update(policyAirMissedSavingsThresholdGroupItemVM.PolicyAirMissedSavingsThresholdGroupItem);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List", new { id = policyAirMissedSavingsThresholdGroupItemVM.PolicyAirMissedSavingsThresholdGroupItem.PolicyGroupId });

        }

        // GET: /View
        public ActionResult View(int id)
        {
            PolicyAirMissedSavingsThresholdGroupItem policyAirMissedSavingsThresholdGroupItem = new PolicyAirMissedSavingsThresholdGroupItem();
            policyAirMissedSavingsThresholdGroupItem = policyAirMissedSavingsThresholdGroupItemRepository.GetPolicyAirMissedSavingsThresholdGroupItem(id);

            //Check Exists
            if (policyAirMissedSavingsThresholdGroupItem == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            //Create ViewModel
            PolicyAirMissedSavingsThresholdGroupItemVM policyAirMissedSavingsThresholdGroupItemVM = new PolicyAirMissedSavingsThresholdGroupItemVM();

            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(policyAirMissedSavingsThresholdGroupItem.PolicyGroupId);
            policyAirMissedSavingsThresholdGroupItemVM.PolicyGroup = policyGroup;

            policyAirMissedSavingsThresholdGroupItemVM.PolicyAirMissedSavingsThresholdGroupItem = policyAirMissedSavingsThresholdGroupItem;

            //Return Form to Users
            return View(policyAirMissedSavingsThresholdGroupItemVM);
        }

        // GET: //Delete
		[HttpGet]
		public ActionResult Delete(int id)
        {
            PolicyAirMissedSavingsThresholdGroupItem policyAirMissedSavingsThresholdGroupItem = new PolicyAirMissedSavingsThresholdGroupItem();
            policyAirMissedSavingsThresholdGroupItem = policyAirMissedSavingsThresholdGroupItemRepository.GetPolicyAirMissedSavingsThresholdGroupItem(id);

            //Check Exists
            if (policyAirMissedSavingsThresholdGroupItem == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyAirMissedSavingsThresholdGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Create ViewModel
            PolicyAirMissedSavingsThresholdGroupItemVM policyAirMissedSavingsThresholdGroupItemVM = new PolicyAirMissedSavingsThresholdGroupItemVM();

            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(policyAirMissedSavingsThresholdGroupItem.PolicyGroupId);
            policyAirMissedSavingsThresholdGroupItemVM.PolicyGroup = policyGroup;

            policyAirMissedSavingsThresholdGroupItemVM.PolicyAirMissedSavingsThresholdGroupItem = policyAirMissedSavingsThresholdGroupItem;

            //Return Form to Users
            return View(policyAirMissedSavingsThresholdGroupItemVM);
        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(PolicyAirMissedSavingsThresholdGroupItemVM policyAirMissedSavingsThresholdGroupItemVM)
        {
            //Get PolicyAirMissedSavingsThresholdGroupItem
            PolicyAirMissedSavingsThresholdGroupItem policyAirMissedSavingsThresholdGroupItem = new PolicyAirMissedSavingsThresholdGroupItem();
            policyAirMissedSavingsThresholdGroupItem = policyAirMissedSavingsThresholdGroupItemRepository.GetPolicyAirMissedSavingsThresholdGroupItem(policyAirMissedSavingsThresholdGroupItemVM.PolicyAirMissedSavingsThresholdGroupItem.PolicyAirMissedSavingsThresholdGroupItemId);

            //Check Exists
            if (policyAirMissedSavingsThresholdGroupItem == null)
            {
                ViewData["ActionMethod"] = "PostDelete";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyAirMissedSavingsThresholdGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
            //Delete Item
            try
            {
                policyAirMissedSavingsThresholdGroupItemRepository.Delete(policyAirMissedSavingsThresholdGroupItemVM.PolicyAirMissedSavingsThresholdGroupItem);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/PolicyAirMissedSavingsThresholdGroupItem.mvc/Delete/" + policyAirMissedSavingsThresholdGroupItem.PolicyAirMissedSavingsThresholdGroupItemId.ToString();
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            //Return
            return RedirectToAction("List", new { id = policyAirMissedSavingsThresholdGroupItem.PolicyGroupId });
        }
    }
}
