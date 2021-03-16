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
    public class PolicyAirCabinGroupItemController : Controller
    {
        //main repositories
        PolicyAirCabinGroupItemRepository policyAirCabinGroupItemRepository = new PolicyAirCabinGroupItemRepository();
        PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();
        PolicyRoutingRepository policyRoutingRepository = new PolicyRoutingRepository();


        //GET: A list of PolicyGroupAirCabin Items for this PolicyGroup
        public ActionResult List(int id, int? page, string sortField, int? sortOrder)
        {
            //Check Parent Exists
            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(id);
            if (policyGroup == null)
            {
                return View("Error");
            }

            //Set Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToPolicyGroup(id))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //Parent Information
            ViewData["PolicyGroupID"] = id;
            ViewData["PolicyGroupName"] = policyGroupRepository.GetGroup(id).PolicyGroupName;


            //SortField + SortOrder settings
            if (string.IsNullOrEmpty(sortField))
            {
                sortField = "Name";
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

            var items = policyAirCabinGroupItemRepository.GetPolicyAirCabinGroupItems(id, sortField, sortOrder ?? 0, page ?? 1);
            return View(items);
        }

        // GET: /Create
        public ActionResult Create(int id)
        {
            PolicyGroup policyGroup = policyGroupRepository.GetGroup(id);

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

            PolicyAirCabinGroupItem policyAirCabinGroupItem = new PolicyAirCabinGroupItem();


            //Populate List of AirlineCabins
            AirlineCabinRepository airlineCabinRepository = new AirlineCabinRepository();
            SelectList airlineCabins = new SelectList(airlineCabinRepository.GetAllAirlineCabins().ToList(), "AirlineCabinCode", "AirlineCabinDefaultDescription");
            ViewData["AirlineCabinCodeList"] = airlineCabins;

            //populate new PolicyAirCabinGroupItem with known PolicyGroup Information           
            policyAirCabinGroupItem.PolicyGroupId = id;
            policyAirCabinGroupItem.PolicyGroupName = policyGroup.PolicyGroupName;

            PolicyRouting policyRouting = new PolicyRouting();
            policyRouting.FromGlobalFlag = false;
            policyRouting.ToGlobalFlag = false;

            PolicyAirCabinGroupItemViewModel policyAirCabinGroupItemViewModel = new PolicyAirCabinGroupItemViewModel(policyGroup, policyAirCabinGroupItem, policyRouting);

            //Show 'Create' Form
            return View(policyAirCabinGroupItemViewModel);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PolicyAirCabinGroupItemViewModel policyAirCabinGroupItemViewModel, string btnSubmit)
        {
            //Get PolicyRouting Info
            PolicyRouting policyRouting = new PolicyRouting();
            policyRouting = policyAirCabinGroupItemViewModel.PolicyRouting;

            //Get PolicyAirCabinGroupItem Info
            PolicyAirCabinGroupItem policyAirCabinGroupItem = new PolicyAirCabinGroupItem();
            policyAirCabinGroupItem = policyAirCabinGroupItemViewModel.PolicyAirCabinGroupItem;

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyAirCabinGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update from+to fields from form to correct properties
            policyRoutingRepository.EditPolicyRouting(policyRouting);
            try
            {
                policyAirCabinGroupItemRepository.Add(policyAirCabinGroupItem, policyRouting);
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
                    return RedirectToAction("List", new { id = policyAirCabinGroupItem.PolicyGroupId });
                default:
                    return RedirectToAction("CreatePolicyRouting", new { id = policyAirCabinGroupItem.PolicyGroupId, policyAirCabinGroupItemId = policyAirCabinGroupItem.PolicyAirCabinGroupItemId });
            }

        }

        // GET: /CreatePolicyRouting
        public ActionResult CreatePolicyRouting(int id, int policyAirCabinGroupItemId)
        {

            //Get PolicyAirVendorGroupItem
            PolicyAirCabinGroupItem policyAirCabinGroupItem = new PolicyAirCabinGroupItem();
            policyAirCabinGroupItem = policyAirCabinGroupItemRepository.GetPolicyAirCabinGroupItem(policyAirCabinGroupItemId);

            //Check Exists
            if (policyAirCabinGroupItem == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            //populate new PolicyAirVendorGroupItem with known PolicyGroup Information
            policyAirCabinGroupItemRepository.EditItemForDisplay(policyAirCabinGroupItem);

            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(policyAirCabinGroupItem.PolicyGroupId);

            PolicyRouting policyRouting = new PolicyRouting();
            PolicyAirCabinGroupItemViewModel policyAirCabinGroupItemViewModel = new PolicyAirCabinGroupItemViewModel(policyGroup, policyAirCabinGroupItem, policyRouting);

            //Show 'Create' Form
            return View(policyAirCabinGroupItemViewModel);
        }

        // POST: /CreatePolicyRouting
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePolicyRouting(int id, int PolicyAirCabinGroupItemId, PolicyRouting policyRouting, string btnSubmit)
        {

            //Get PolicyAirVendorGroupItem (Original)
            PolicyAirCabinGroupItem policyAirCabinGroupItemOriginal = new PolicyAirCabinGroupItem();
            policyAirCabinGroupItemOriginal = policyAirCabinGroupItemRepository.GetPolicyAirCabinGroupItem(PolicyAirCabinGroupItemId);

            //Check Exists
            if (policyAirCabinGroupItemOriginal == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }


            //Update from+to fields from form to correct properties
            policyRoutingRepository.EditPolicyRouting(policyRouting);

            //Copy fareRestriction from original
            PolicyAirCabinGroupItem policyAirCabinGroupItem = new PolicyAirCabinGroupItem();
            policyAirCabinGroupItem.PolicyGroupId = policyAirCabinGroupItemOriginal.PolicyGroupId;
            policyAirCabinGroupItem.AirlineCabinCode = policyAirCabinGroupItemOriginal.AirlineCabinCode;
            policyAirCabinGroupItem.FlightDurationAllowedMin = policyAirCabinGroupItemOriginal.FlightDurationAllowedMin;
            policyAirCabinGroupItem.FlightDurationAllowedMax = policyAirCabinGroupItemOriginal.FlightDurationAllowedMax;
            policyAirCabinGroupItem.FlightMileageAllowedMin = policyAirCabinGroupItemOriginal.FlightMileageAllowedMin;
            policyAirCabinGroupItem.FlightMileageAllowedMax = policyAirCabinGroupItemOriginal.FlightMileageAllowedMax;
            policyAirCabinGroupItem.PolicyProhibitedFlag = policyAirCabinGroupItemOriginal.PolicyProhibitedFlag;
            policyAirCabinGroupItem.EnabledFlag = policyAirCabinGroupItemOriginal.EnabledFlag;
            policyAirCabinGroupItem.EnabledDate = policyAirCabinGroupItemOriginal.EnabledDate;
            policyAirCabinGroupItem.ExpiryDate = policyAirCabinGroupItemOriginal.ExpiryDate;
            policyAirCabinGroupItem.TravelDateValidFrom = policyAirCabinGroupItemOriginal.TravelDateValidFrom;
            policyAirCabinGroupItem.TravelDateValidTo = policyAirCabinGroupItemOriginal.TravelDateValidTo;

            //Save policyAirVendorGroupItem to DB
            try
            {
                policyAirCabinGroupItemRepository.Add(policyAirCabinGroupItem, policyRouting);
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
                    return RedirectToAction("List", new { id = id });
                default:
                    return RedirectToAction("CreatePolicyRouting", new { id = id, policyAirCabinGroupItemId = PolicyAirCabinGroupItemId });
            }

        }


        // GET: /Edit
        public ActionResult Edit(int id)
        {
            //Get PolicyAirCabinGroupItem
            PolicyAirCabinGroupItem policyAirCabinGroupItem = new PolicyAirCabinGroupItem();
            policyAirCabinGroupItem = policyAirCabinGroupItemRepository.GetPolicyAirCabinGroupItem(id);

            //Check Exists
            if (policyAirCabinGroupItem == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyAirCabinGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Populate List of AirlineCabins
            AirlineCabinRepository airlineCabinRepository = new AirlineCabinRepository();
            SelectList airlineCabins = new SelectList(airlineCabinRepository.GetAllAirlineCabins().ToList(), "AirlineCabinCode", "AirlineCabinDefaultDescription");
            ViewData["AirlineCabinCodeList"] = airlineCabins;

            policyAirCabinGroupItemRepository.EditItemForDisplay(policyAirCabinGroupItem);
            PolicyGroup policyGroup = policyGroupRepository.GetGroup(policyAirCabinGroupItem.PolicyGroupId);


            //Add the PolicyRouting information
            PolicyRouting policyRouting = new PolicyRouting();
            if (policyAirCabinGroupItem.PolicyRoutingId != null)
            {
                policyRouting = policyRoutingRepository.GetPolicyRouting((int)policyAirCabinGroupItem.PolicyRoutingId);
                policyRoutingRepository.EditPolicyRouting(policyRouting);
            }

            //Show Edit Form
            PolicyAirCabinGroupItemViewModel policyAirCabinGroupItemViewModel = new PolicyAirCabinGroupItemViewModel(policyGroup, policyAirCabinGroupItem, policyRouting);
            return View(policyAirCabinGroupItemViewModel);
        }

        // POST /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection collection)
        {
            PolicyAirCabinGroupItem policyAirCabinGroupItem = new PolicyAirCabinGroupItem();
            policyAirCabinGroupItem = policyAirCabinGroupItemRepository.GetPolicyAirCabinGroupItem(id);

            //Check Exists
            if (policyAirCabinGroupItem == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyAirCabinGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update Item from Form
            try
            {
                UpdateModel<PolicyAirCabinGroupItem>(policyAirCabinGroupItem, "PolicyAirCabinGroupItem");
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
            //add the PolicyRouting information
            PolicyRouting policyRouting = new PolicyRouting();
            if (policyAirCabinGroupItem.PolicyRoutingId != null)
            {

                policyRouting = policyRoutingRepository.GetPolicyRouting((int)policyAirCabinGroupItem.PolicyRoutingId);
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
                policyRoutingRepository.EditPolicyRouting(policyRouting);
            }


            //Database Update
            try
            {
                
                policyAirCabinGroupItemRepository.Update(policyAirCabinGroupItem, policyRouting);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/PolicyAirCabinGroupItem.mvc/Delete/" + policyAirCabinGroupItem.PolicyAirCabinGroupItemId;
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            //Return to Form
            return RedirectToAction("List", new { id = policyAirCabinGroupItem.PolicyGroupId });
        }

        // GET: /View
        public ActionResult View(int id)
        {
            //Get PolicyAirCabinGroupItem
            PolicyAirCabinGroupItem policyAirCabinGroupItem = new PolicyAirCabinGroupItem();
            policyAirCabinGroupItem = policyAirCabinGroupItemRepository.GetPolicyAirCabinGroupItem(id);

            //Check Exists
            if (policyAirCabinGroupItem == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            //populate new PolicyAirCabinGroupItem with known PolicyGroup Information
            policyAirCabinGroupItemRepository.EditItemForDisplay(policyAirCabinGroupItem);

            //Add the PolicyRouting information
            PolicyRouting policyRouting = new PolicyRouting();
            if (policyAirCabinGroupItem.PolicyRoutingId != null)
            {
                policyRouting = policyRoutingRepository.GetPolicyRouting((int)policyAirCabinGroupItem.PolicyRoutingId);
                policyRoutingRepository.EditForDisplay(policyRouting);
            }

            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(policyAirCabinGroupItem.PolicyGroupId);

            //Show Form
            PolicyAirCabinGroupItemViewModel policyAirCabinGroupItemViewModel = new PolicyAirCabinGroupItemViewModel(policyGroup, policyAirCabinGroupItem, policyRouting);
            return View(policyAirCabinGroupItemViewModel);
        }

        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id)
        {
            //Get PolicyAirCabinGroupItem
            PolicyAirCabinGroupItem policyAirCabinGroupItem = new PolicyAirCabinGroupItem();
            policyAirCabinGroupItem = policyAirCabinGroupItemRepository.GetPolicyAirCabinGroupItem(id);

            //Check Exists
            if (policyAirCabinGroupItem == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }
            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyAirCabinGroupItem.PolicyGroupId))
            {
                return View("Error");
            }
            //populate new PolicyAirCabinGroupItem with known PolicyGroup Information
            policyAirCabinGroupItemRepository.EditItemForDisplay(policyAirCabinGroupItem);

            //Add the PolicyRouting information
            PolicyRouting policyRouting = new PolicyRouting();
            if (policyAirCabinGroupItem.PolicyRoutingId != null)
            {
                policyRouting = policyRoutingRepository.GetPolicyRouting((int)policyAirCabinGroupItem.PolicyRoutingId);
                policyRoutingRepository.EditForDisplay(policyRouting);
            }
            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(policyAirCabinGroupItem.PolicyGroupId);

            //Show Form
            PolicyAirCabinGroupItemViewModel policyAirCabinGroupItemViewModel = new PolicyAirCabinGroupItemViewModel(policyGroup, policyAirCabinGroupItem, policyRouting);
            return View(policyAirCabinGroupItemViewModel);
        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            //Get PolicyAirCabinGroupItem
            PolicyAirCabinGroupItem policyAirCabinGroupItem = new PolicyAirCabinGroupItem();
            policyAirCabinGroupItem = policyAirCabinGroupItemRepository.GetPolicyAirCabinGroupItem(id);

            //Check Exists
            if (policyAirCabinGroupItem == null)
            {
                ViewData["ActionMethod"] = "PostDelete";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyAirCabinGroupItem.PolicyGroupId))
            {
                return View("Error");
            }

            //Delete Item
            try
            {
                policyAirCabinGroupItem.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                policyAirCabinGroupItemRepository.Delete(policyAirCabinGroupItem);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/PolicyAirCabinGroupItem.mvc/Delete/" + policyAirCabinGroupItem.PolicyAirCabinGroupItemId;
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }



            //Return
            return RedirectToAction("List", new { id = policyAirCabinGroupItem.PolicyGroupId });
        }


    }
}
