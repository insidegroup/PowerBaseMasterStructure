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

namespace CWTDesktopDatabase.Controllers
{
	public class PolicyAirAdvancePurchaseGroupItemController : Controller
    {
        //Repositories
        PolicyAirParameterGroupItemRepository policyAirParameterGroupItemRepository = new PolicyAirParameterGroupItemRepository();
        PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();
        PolicyRoutingRepository policyRoutingRepository = new PolicyRoutingRepository();

		private int PolicyAirParameterTypeId = 2;

		//GET: A list of PolicyAirParameterGroup Items for this PolicyGroup
        public ActionResult List(int id, string filter, int? page, string sortField, int? sortOrder)
        {
            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(id); ;

            //Check Exists
            if (policyGroup == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }

            //SortField + SortOrder settings
            if (string.IsNullOrEmpty(sortField))
            {
				sortField = "PolicyAirParameterValue";
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
            
			PolicyAirParameterGroupItemsVM policyAirParameterGroupItemsVM = new PolicyAirParameterGroupItemsVM();
			policyAirParameterGroupItemsVM.PolicyAirParameterGroupItems = policyAirParameterGroupItemRepository.GetPolicyAirParameterGroupItems(id, PolicyAirParameterTypeId, filter ?? "", sortField, sortOrder ?? 0, page ?? 1);
            policyAirParameterGroupItemsVM.PolicyGroup = policyGroup;

            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToPolicyGroup(id))
            {
                policyAirParameterGroupItemsVM.HasWriteAccess = true;
            }

            return View(policyAirParameterGroupItemsVM);
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


            PolicyAirParameterGroupItem policyAirParameterGroupItem = new PolicyAirParameterGroupItem();

            //populate new PolicyAirParameterGroupItem with known PolicyGroup Information           
            policyAirParameterGroupItem.PolicyGroupId = id;
            policyAirParameterGroupItem.PolicyGroupName = policyGroup.PolicyGroupName;
			policyAirParameterGroupItem.PolicyAirParameterTypeId = PolicyAirParameterTypeId;

            PolicyRouting policyRouting = new PolicyRouting();
            policyRouting.FromGlobalFlag = false;
            policyRouting.ToGlobalFlag = false;

            PolicyAirParameterGroupItemVM policyAirParameterGroupItemViewModel = new PolicyAirParameterGroupItemVM(policyGroup, policyAirParameterGroupItem, policyRouting);
            
            //Show 'Create' Form
            return View(policyAirParameterGroupItemViewModel);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PolicyAirParameterGroupItemVM policyAirParameterGroupItemViewModel,string btnSubmit)
        {
            //Get PolicyRouting Info
            PolicyRouting policyRouting = new PolicyRouting();
            policyRouting = policyAirParameterGroupItemViewModel.PolicyRouting;
            
            //Get PolicyAirParameterGroupItem Info
            PolicyAirParameterGroupItem policyAirParameterGroupItem = new PolicyAirParameterGroupItem();
            policyAirParameterGroupItem = policyAirParameterGroupItemViewModel.PolicyAirParameterGroupItem;

            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(policyAirParameterGroupItem.PolicyGroupId);

            //Check Exists
            if (policyGroup == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyAirParameterGroupItem.PolicyGroupId))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Edit Routing
            policyRoutingRepository.EditPolicyRouting(policyRouting);

            //Save To DB
            try
            {
                policyAirParameterGroupItemRepository.Add(policyAirParameterGroupItem, policyRouting);
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
            //Get PolicyAirParameterGroupItem
            PolicyAirParameterGroupItem policyAirParameterGroupItem = new PolicyAirParameterGroupItem();
            policyAirParameterGroupItem = policyAirParameterGroupItemRepository.GetPolicyAirParameterGroupItem(id);

            //Check Exists
            if (policyAirParameterGroupItem == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyAirParameterGroupItem.PolicyGroupId))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
             
            //populate new PolicyAirParameterGroupItem with known PolicyGroup Information
            policyAirParameterGroupItemRepository.EditItemForDisplay(policyAirParameterGroupItem);

            //Policy Group
            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(policyAirParameterGroupItem.PolicyGroupId);

            //Add the PolicyRouting information
            PolicyRouting policyRouting = policyRoutingRepository.GetPolicyRouting(policyAirParameterGroupItem.PolicyRoutingId ?? 0);
            PolicyAirParameterGroupItemVM policyAirParameterGroupItemViewModel = new PolicyAirParameterGroupItemVM(policyGroup, policyAirParameterGroupItem, policyRouting);

            //Show 'Edit' Form
            return View(policyAirParameterGroupItemViewModel);
        }

        // POST /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, PolicyAirParameterGroupItemVM policyAirParameterGroupItemViewModel, FormCollection collection)
        {
            PolicyAirParameterGroupItem policyAirParameterGroupItem = new PolicyAirParameterGroupItem();
            policyAirParameterGroupItem = policyAirParameterGroupItemRepository.GetPolicyAirParameterGroupItem(id);

            PolicyRouting policyRouting = new PolicyRouting();
            policyRouting = policyAirParameterGroupItemViewModel.PolicyRouting;

            //Check Exists
            if (policyAirParameterGroupItem == null || policyRouting == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyAirParameterGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update Item from Form
            try
            {
                UpdateModel(policyAirParameterGroupItem, "PolicyAirParameterGroupItem");
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

            //Database Update
            try
            {
                policyAirParameterGroupItemRepository.Update(policyAirParameterGroupItem, policyRouting);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/PolicyAirParameterGroupItem.mvc/Edit/" + policyAirParameterGroupItem.PolicyAirParameterGroupItemId;
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }



            //Return to Form
            return RedirectToAction("List", new { id = policyAirParameterGroupItem.PolicyGroupId });
        }

        // GET: /View
        public ActionResult View(int id)
        {
            //Get PolicyAirParameterGroupItem
            PolicyAirParameterGroupItem policyAirParameterGroupItem = new PolicyAirParameterGroupItem();
            policyAirParameterGroupItem = policyAirParameterGroupItemRepository.GetPolicyAirParameterGroupItem(id);

            //Check Exists
            if (policyAirParameterGroupItem == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }


            //populate new PolicyAirParameterGroupItem with known PolicyGroup Information
            policyAirParameterGroupItemRepository.EditItemForDisplay(policyAirParameterGroupItem);

            //Policy Group
            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(policyAirParameterGroupItem.PolicyGroupId);

            //add the PolicyRouting information
            PolicyRouting policyRouting = policyRoutingRepository.GetPolicyRouting(policyAirParameterGroupItem.PolicyRoutingId ?? 0);
            policyRoutingRepository.EditForDisplay(policyRouting);
            PolicyAirParameterGroupItemVM policyAirParameterGroupItemViewModel = new PolicyAirParameterGroupItemVM(policyGroup, policyAirParameterGroupItem, policyRouting);

            //Show 'Create' Form
            return View(policyAirParameterGroupItemViewModel);
        }

        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id)
        {
            //Get PolicyAirParameterGroupItem
            PolicyAirParameterGroupItem policyAirParameterGroupItem = new PolicyAirParameterGroupItem();
            policyAirParameterGroupItem = policyAirParameterGroupItemRepository.GetPolicyAirParameterGroupItem(id);

            //Check Exists
            if (policyAirParameterGroupItem == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyAirParameterGroupItem.PolicyGroupId))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
          
            //populate new PolicyAirParameterGroupItem with known PolicyGroup Information
            policyAirParameterGroupItemRepository.EditItemForDisplay(policyAirParameterGroupItem);

            //Policy Group
            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(policyAirParameterGroupItem.PolicyGroupId);

            //add the PolicyRouting information
            PolicyRouting policyRouting = policyRoutingRepository.GetPolicyRouting(policyAirParameterGroupItem.PolicyRoutingId ?? 0);
            policyRoutingRepository.EditForDisplay(policyRouting);
            PolicyAirParameterGroupItemVM policyAirParameterGroupItemViewModel = new PolicyAirParameterGroupItemVM(policyGroup, policyAirParameterGroupItem, policyRouting);           

            //Show 'Create' Form
            return View(policyAirParameterGroupItemViewModel);
        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            //Get PolicyAirParameterGroupItem
            PolicyAirParameterGroupItem policyAirParameterGroupItem = new PolicyAirParameterGroupItem();
            policyAirParameterGroupItem = policyAirParameterGroupItemRepository.GetPolicyAirParameterGroupItem(id);

            //Check Exists
            if (policyAirParameterGroupItem == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyAirParameterGroupItem.PolicyGroupId))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Delete
            //Delete Item
            try
            {
                policyAirParameterGroupItem.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                policyAirParameterGroupItemRepository.Delete(policyAirParameterGroupItem);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/PolicyAirParameterGroupItem.mvc/Delete/" + policyAirParameterGroupItem.PolicyAirParameterGroupItemId;
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }
            //Return
            return RedirectToAction("List", new { id = policyAirParameterGroupItem.PolicyGroupId});
        }
    }
}
