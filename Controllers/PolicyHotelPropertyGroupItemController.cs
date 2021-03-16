using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Repository;
using System.Data.SqlClient;

namespace CWTDesktopDatabase.Controllers
{
    public class PolicyHotelPropertyGroupItemController : Controller
    {
        //main repositories
        PolicyHotelPropertyGroupItemRepository policyHotelPropertyGroupItemRepository = new PolicyHotelPropertyGroupItemRepository();
        PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();
        PolicyRoutingRepository policyRoutingRepository = new PolicyRoutingRepository();

        //GET: A list of PolicyGroupHotelProperty Items for this PolicyGroup
        public ActionResult List(int id, int? page, string sortField, int? sortOrder)
        {
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
            if (sortField !="PolicyHotelStatus")
            {
                sortField = "SequenceNumber";
            }
            ViewData["CurrentSortField"] = sortField;

            if (sortField == null || sortOrder == 1)
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

            //Get data
			var paginatedView = policyHotelPropertyGroupItemRepository.GetPolicyHotelPropertyGroupItemsByPolicyGroup(id, page ?? 1, sortField, sortOrder ?? 0);

            //Return View
            return View(paginatedView);
        }

        // GET: /Create
        public ActionResult Create(int id)
        {

            PolicyGroup policyGroup = policyGroupRepository.GetGroup(id);
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

            PolicyHotelPropertyGroupItem policyHotelPropertyGroupItem = new PolicyHotelPropertyGroupItem();

            //Populate List of PolicyHotelStatuses
            PolicyHotelStatusRepository policyHotelStatusRepository = new PolicyHotelStatusRepository();
            SelectList policyHotelStatuses = new SelectList(policyHotelStatusRepository.GetAllPolicyHotelStatuses().ToList(), "PolicyHotelStatusId", "PolicyHotelStatusDescription");
            ViewData["PolicyHotelStatusList"] = policyHotelStatuses;

            //Populate List of HarpHotels 
            HarpHotelRepository harpHotelRepository = new HarpHotelRepository();
            SelectList harpHotels = new SelectList(harpHotelRepository.GetAllHarpHotels().ToList(), "HarpHotelId", "HarpHotelName");
            ViewData["HarpHotelList"] = harpHotels;

            //populate new PolicyHotelPropertyGroupItem with known PolicyGroup Information           
            policyHotelPropertyGroupItem.PolicyGroupId = id;
            policyHotelPropertyGroupItem.PolicyGroupName = policyGroup.PolicyGroupName;

            //Show 'Create' Form
            return View(policyHotelPropertyGroupItem);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PolicyHotelPropertyGroupItem policyHotelPropertyGroupItem)
        {

            PolicyGroup policyGroup = policyGroupRepository.GetGroup(policyHotelPropertyGroupItem.PolicyGroupId);
            if (policyGroup == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }
            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyHotelPropertyGroupItem.PolicyGroupId))
            {
                return View("Error");
            }

            //Update Model from Form
            try
            {
                UpdateModel(policyHotelPropertyGroupItem);
            }
            catch
            {
                return View("Error");
            }

            //Add PolicyHotelPropertyGroupItem to database
            try
            {
                policyHotelPropertyGroupItemRepository.Add(policyHotelPropertyGroupItem);
            }
            catch
            {
                //Could not insert to database
                return View("Error");
            }

            return RedirectToAction("List", new { id = policyHotelPropertyGroupItem.PolicyGroupId });
        }


        // GET: /Edit
        public ActionResult Edit(int id)
        {
            //Get PolicyHotelPropertyGroupItem
            PolicyHotelPropertyGroupItem policyHotelPropertyGroupItem = new PolicyHotelPropertyGroupItem();
            policyHotelPropertyGroupItem = policyHotelPropertyGroupItemRepository.GetPolicyHotelPropertyGroupItem(id);

            //Check Exists
            if (policyHotelPropertyGroupItem == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyHotelPropertyGroupItem.PolicyGroupId))
            {
                return View("Error");
            }

            //Populate List of PolicyHotelStatuses
            PolicyHotelStatusRepository policyHotelStatusRepository = new PolicyHotelStatusRepository();
            SelectList policyHotelStatuses = new SelectList(policyHotelStatusRepository.GetAllPolicyHotelStatuses().ToList(), "PolicyHotelStatusId", "PolicyHotelStatusDescription");
            ViewData["PolicyHotelStatusList"] = policyHotelStatuses;

            //Populate List of HarpHotels 
            HarpHotelRepository harpHotelRepository = new HarpHotelRepository();
            SelectList harpHotels = new SelectList(harpHotelRepository.GetAllHarpHotels().ToList(), "HarpHotelId", "HarpHotelName");
            ViewData["HarpHotelList"] = harpHotels;

            //Populate new PolicyHotelPropertyGroupItem with known PolicyGroup Information
            PolicyGroup policyGroup = policyGroupRepository.GetGroup(policyHotelPropertyGroupItem.PolicyGroupId);
            policyHotelPropertyGroupItem.PolicyGroupName = policyGroup.PolicyGroupName;

            //Show 'Edit' Form
            policyHotelPropertyGroupItemRepository.EditItemForDisplay(policyHotelPropertyGroupItem);
            return View(policyHotelPropertyGroupItem);
        }

        // POST /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection collection)
        {
            PolicyHotelPropertyGroupItem policyHotelPropertyGroupItem = new PolicyHotelPropertyGroupItem();
            policyHotelPropertyGroupItem = policyHotelPropertyGroupItemRepository.GetPolicyHotelPropertyGroupItem(id);

            //Check Exists
            if (policyHotelPropertyGroupItem == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyHotelPropertyGroupItem.PolicyGroupId))
            {
                return View("Error");
            }

            //Update PolicyHotelPropertyGroupItem Model From Form
            try
            {
                UpdateModel(policyHotelPropertyGroupItem);
            }
            catch
            {
                return View("Error");
            }

            //Database Update
            try
            {
                policyHotelPropertyGroupItemRepository.Update(policyHotelPropertyGroupItem); 
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/PolicyHotelPropertyGroupItem.mvc/Edit/" + policyHotelPropertyGroupItem.PolicyHotelPropertyGroupItemId.ToString();
                    return View("VersionError");
                }

                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");            }

            //Return to Form
            return RedirectToAction("List", new { id = policyHotelPropertyGroupItem.PolicyGroupId });

        }

        // GET: /View
        public ActionResult View(int id)
        {
            //Get PolicyHotelPropertyGroupItem
            PolicyHotelPropertyGroupItem policyHotelPropertyGroupItem = new PolicyHotelPropertyGroupItem();
            policyHotelPropertyGroupItem = policyHotelPropertyGroupItemRepository.GetPolicyHotelPropertyGroupItem(id);

            //Check Exists
            if (policyHotelPropertyGroupItem == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            //populate new PolicyHotelPropertyGroupItem with known PolicyGroup Information
            policyHotelPropertyGroupItemRepository.EditItemForDisplay(policyHotelPropertyGroupItem);

            //Show 'View' Form
            return View(policyHotelPropertyGroupItem);
        }

        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id)
        {
            //Get PolicyHotelPropertyGroupItem
            PolicyHotelPropertyGroupItem policyHotelPropertyGroupItem = new PolicyHotelPropertyGroupItem();
            policyHotelPropertyGroupItem = policyHotelPropertyGroupItemRepository.GetPolicyHotelPropertyGroupItem(id);

            //Check Exists
            if (policyHotelPropertyGroupItem == null)
            {
                ViewData["ActionMethod"] = "GetDelete";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyHotelPropertyGroupItem.PolicyGroupId))
            {
                return View("Error");
            }

            //populate new PolicyHotelPropertyGroupItem with known PolicyGroup Information
            policyHotelPropertyGroupItemRepository.EditItemForDisplay(policyHotelPropertyGroupItem);

            //Show 'Create' Form
            return View(policyHotelPropertyGroupItem);
        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            //Get PolicyHotelPropertyGroupItem
            PolicyHotelPropertyGroupItem policyHotelPropertyGroupItem = new PolicyHotelPropertyGroupItem();
            policyHotelPropertyGroupItem = policyHotelPropertyGroupItemRepository.GetPolicyHotelPropertyGroupItem(id);

            //Check Exists
            if (policyHotelPropertyGroupItem == null)
            {
                ViewData["ActionMethod"] = "PostDelete";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyHotelPropertyGroupItem.PolicyGroupId))
            {
                return View("Error");
            }
            //Delete Item
            try
            {
                policyHotelPropertyGroupItem.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                policyHotelPropertyGroupItemRepository.Delete(policyHotelPropertyGroupItem);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/PolicyHotelPropertyGroupItem.mvc/Delete/" + policyHotelPropertyGroupItem.PolicyHotelPropertyGroupItemId.ToString();
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            //Return
            return RedirectToAction("List", new { id = policyHotelPropertyGroupItem.PolicyGroupId });
        }
    }
}
