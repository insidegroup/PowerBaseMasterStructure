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
    public class PolicyCarTypeGroupItemController : Controller
    {
        //main repositories
        PolicyCarTypeGroupItemRepository policyCarTypeGroupItemRepository = new PolicyCarTypeGroupItemRepository();
        PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();
        PolicyRoutingRepository policyRoutingRepository = new PolicyRoutingRepository();

        //GET: A list of PolicyGroupCarType Items for this PolicyGroup
        public ActionResult List(int id, int? page, string sortField, int? sortOrder)
        {
            PolicyGroup group = new PolicyGroup();
            group = policyGroupRepository.GetGroup(id); ;

            //Check Exists
            if (group == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
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
            ViewData["PolicyGroupName"] = group.PolicyGroupName;


            //SortField + SortOrder settings
            if (string.IsNullOrEmpty(sortField))
            {
                sortField = "PolicyLocationName";
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

            var items = policyCarTypeGroupItemRepository.GetPolicyCarTypeGroupItems(id, sortField, sortOrder ?? 0, page ?? 1);
            return View(items);
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

            

            //Populate List of PolicyLocations
            PolicyLocationRepository policyLocationRepository = new PolicyLocationRepository();
            SelectList policyLocations = new SelectList(policyLocationRepository.GetAllPolicyLocations().ToList(), "PolicyLocationId", "PolicyLocationName");
            ViewData["PolicyLocationList"] = policyLocations;

            //Populate List of PolicyCarStatuses 
            PolicyCarStatusRepository policyCarStatusRepository = new PolicyCarStatusRepository();
            SelectList carStatuses = new SelectList(policyCarStatusRepository.GetAllPolicyCarStatuses().ToList(), "PolicyCarStatusId", "PolicyCarStatusDescription");
            ViewData["PolicyCarStatusList"] = carStatuses;

            //Populate List of CarTypeCategories
            CarTypeCategoryRepository carTypeCategoryRepository = new CarTypeCategoryRepository();
            SelectList carTypes = new SelectList(carTypeCategoryRepository.GetAllCarTypeCategories().ToList(), "CarTypeCategoryId", "CarTypeCategoryName");
            ViewData["CarTypeCategoryList"] = carTypes;

            //populate new PolicyCarTypeGroupItem with known PolicyGroup Information           
            PolicyCarTypeGroupItem policyCarTypeGroupItem = new PolicyCarTypeGroupItem();
            policyCarTypeGroupItem.PolicyGroupId = id;
            policyCarTypeGroupItem.PolicyGroupName = policyGroup.PolicyGroupName;

            //Show 'Create' Form
            return View(policyCarTypeGroupItem);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PolicyCarTypeGroupItem policyCarTypeGroupItem)
        {

            PolicyGroup policyGroup = policyGroupRepository.GetGroup(policyCarTypeGroupItem.PolicyGroupId);
            if (policyGroup == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }
            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyCarTypeGroupItem.PolicyGroupId))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
            //Update Model from Form
            try
            {
                UpdateModel(policyCarTypeGroupItem);
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
                policyCarTypeGroupItemRepository.Add(policyCarTypeGroupItem);
            }
            catch (SqlException ex)
            {

                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            return RedirectToAction("List", new { id = policyCarTypeGroupItem.PolicyGroupId });
        }


        // GET: /Edit
        public ActionResult Edit(int id)
        {
            //Get PolicyCarTypeGroupItem
            PolicyCarTypeGroupItem policyCarTypeGroupItem = new PolicyCarTypeGroupItem();
            policyCarTypeGroupItem = policyCarTypeGroupItemRepository.GetPolicyCarTypeGroupItem(id);

            //Check Exists
            if (policyCarTypeGroupItem == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyCarTypeGroupItem.PolicyGroupId))
            {
                return View("Error");
            }

            //Populate List of PolicyLocations
            PolicyLocationRepository policyLocationRepository = new PolicyLocationRepository();
            SelectList policyLocations = new SelectList(policyLocationRepository.GetAllPolicyLocations().ToList(), "PolicyLocationId", "PolicyLocationName");
            ViewData["PolicyLocationList"] = policyLocations;

            //Populate List of PolicyCarStatuses 
            PolicyCarStatusRepository policyCarStatusRepository = new PolicyCarStatusRepository();
            SelectList carStatuses = new SelectList(policyCarStatusRepository.GetAllPolicyCarStatuses().ToList(), "PolicyCarStatusId", "PolicyCarStatusDescription");
            ViewData["PolicyCarStatusList"] = carStatuses;

            //Populate List of CarTypeCategories
            CarTypeCategoryRepository carTypeCategoryRepository = new CarTypeCategoryRepository();
            SelectList carTypes = new SelectList(carTypeCategoryRepository.GetAllCarTypeCategories().ToList(), "CarTypeCategoryId", "CarTypeCategoryName");
            ViewData["CarTypeCategoryList"] = carTypes;

            //Populate new PolicyCarTypeGroupItem with known PolicyGroup Information
            PolicyGroup policyGroup = policyGroupRepository.GetGroup(policyCarTypeGroupItem.PolicyGroupId);
            policyCarTypeGroupItem.PolicyGroupName = policyGroup.PolicyGroupName;

            //Show 'Edit' Form
            policyCarTypeGroupItemRepository.EditItemForDisplay(policyCarTypeGroupItem);
            return View(policyCarTypeGroupItem);
        }

        // POST /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection collection)
        {
            PolicyCarTypeGroupItem policyCarTypeGroupItem = new PolicyCarTypeGroupItem();
            policyCarTypeGroupItem = policyCarTypeGroupItemRepository.GetPolicyCarTypeGroupItem(id);

            //Check Exists
            if (policyCarTypeGroupItem == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyCarTypeGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update PolicyCarTypeGroupItem Model From Form
            try
            {
                UpdateModel(policyCarTypeGroupItem);
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
                policyCarTypeGroupItemRepository.Update(policyCarTypeGroupItem);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/PolicyCarTypeGroupItem.mvc/Edit/" + policyCarTypeGroupItem.PolicyCarTypeGroupItemId.ToString();
                    return View("VersionError");
                }

                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");            }

            //Return to Form
            return RedirectToAction("List", new { id = policyCarTypeGroupItem.PolicyGroupId });

        }

        // GET: /View
        public ActionResult View(int id)
        {
            //Get PolicyCarTypeGroupItem
            PolicyCarTypeGroupItem policyCarTypeGroupItem = new PolicyCarTypeGroupItem();
            policyCarTypeGroupItem = policyCarTypeGroupItemRepository.GetPolicyCarTypeGroupItem(id);

            //Check Exists
            if (policyCarTypeGroupItem == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            //populate new PolicyCarTypeGroupItem with known PolicyGroup Information
            policyCarTypeGroupItemRepository.EditItemForDisplay(policyCarTypeGroupItem);

            //Show 'View' Form
            return View(policyCarTypeGroupItem);
        }

        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id)
        {
            //Get PolicyCarTypeGroupItem
            PolicyCarTypeGroupItem policyCarTypeGroupItem = new PolicyCarTypeGroupItem();
            policyCarTypeGroupItem = policyCarTypeGroupItemRepository.GetPolicyCarTypeGroupItem(id);

            //Check Exists
            if (policyCarTypeGroupItem == null)
            {
                ViewData["ActionMethod"] = "GetDelete";
                return View("RecordDoesNotExistError");
            }


            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyCarTypeGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //populate new PolicyCarTypeGroupItem with known PolicyGroup Information
            policyCarTypeGroupItemRepository.EditItemForDisplay(policyCarTypeGroupItem);

            //Show 'Create' Form
            return View(policyCarTypeGroupItem);
        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            //Get PolicyCarTypeGroupItem
            PolicyCarTypeGroupItem policyCarTypeGroupItem = new PolicyCarTypeGroupItem();
            policyCarTypeGroupItem = policyCarTypeGroupItemRepository.GetPolicyCarTypeGroupItem(id);

            //Check Exists
            if (policyCarTypeGroupItem == null)
            {
                ViewData["ActionMethod"] = "PostDelete";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyCarTypeGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
            //Delete Item
            try
            {
                policyCarTypeGroupItem.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                policyCarTypeGroupItemRepository.Delete(policyCarTypeGroupItem);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/PolicyCarTypeGroupItem.mvc/Delete/" + policyCarTypeGroupItem.PolicyCarTypeGroupItemId.ToString();
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            //Return
            return RedirectToAction("List", new { id = policyCarTypeGroupItem.PolicyGroupId });
        }
    }
}
