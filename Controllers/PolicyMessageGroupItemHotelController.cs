using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Repository;
using System.Data.SqlClient;

namespace CWTDesktopDatabase.Controllers
{
    public class PolicyMessageGroupItemHotelController : Controller
    {
        PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();
        PolicyMessageGroupItemRepository policyMessageGroupItemRepository = new PolicyMessageGroupItemRepository();
        PolicyMessageGroupItemHotelRepository policyMessageGroupItemHotelRepository = new PolicyMessageGroupItemHotelRepository();

        public ActionResult Create(int id )
        {
            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(id); ;

            //Check Exists
            if (policyGroup == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroupMessages(id))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            PolicyMessageGroupItemHotelVM policyMessageGroupItemHotelVM = new PolicyMessageGroupItemHotelVM();
            policyMessageGroupItemHotelVM.PolicyGroupName = policyGroup.PolicyGroupName;
            policyMessageGroupItemHotelVM.PolicyGroupId = policyGroup.PolicyGroupId;

            PolicyMessageGroupItemHotel policyMessageGroupItemHotel = new PolicyMessageGroupItemHotel();
            policyMessageGroupItemHotel.ProductId = 2; //Hotel
            policyMessageGroupItemHotel.PolicyGroupId = policyGroup.PolicyGroupId;
            policyMessageGroupItemHotelVM.PolicyMessageGroupItemHotel = policyMessageGroupItemHotel;

            //Populate List of PolicyLocations
            PolicyLocationRepository policyLocationRepository = new PolicyLocationRepository();
            SelectList policyLocations = new SelectList(policyLocationRepository.GetAllPolicyLocations().ToList(), "PolicyLocationId", "PolicyLocationName");
            policyMessageGroupItemHotelVM.PolicyLocations = policyLocations;

            return View(policyMessageGroupItemHotelVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PolicyMessageGroupItemHotelVM policyMessageGroupItemHotelVM)
        {
            int policyGroupId = policyMessageGroupItemHotelVM.PolicyMessageGroupItemHotel.PolicyGroupId;

            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(policyGroupId); ;

            //Check Exists
            if (policyGroup == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroupMessages(policyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update Model from Form
            try
            {
                UpdateModel<PolicyMessageGroupItemHotel>(policyMessageGroupItemHotelVM.PolicyMessageGroupItemHotel, "PolicyMessageGroupItemHotel");
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
                policyMessageGroupItemHotelRepository.Add(policyMessageGroupItemHotelVM.PolicyMessageGroupItemHotel);
            }
            catch (SqlException ex)
            {

                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            return RedirectToAction("List","PolicyMessageGroupItem", new { id = policyGroupId });
        }

        public ActionResult Edit(int id)
        {
            PolicyMessageGroupItemHotel policyMessageGroupItemHotel = new PolicyMessageGroupItemHotel();
            policyMessageGroupItemHotel = policyMessageGroupItemHotelRepository.GetPolicyMessageGroupItemHotel(id); ;

            //Check Exists
            if (policyMessageGroupItemHotel == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(policyMessageGroupItemHotel.PolicyGroupId);

            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroupMessages(policyGroup.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            PolicyMessageGroupItemHotelVM policyMessageGroupItemHotelVM = new PolicyMessageGroupItemHotelVM();
            policyMessageGroupItemHotelVM.PolicyGroupName = policyGroup.PolicyGroupName;
            policyMessageGroupItemHotelVM.PolicyGroupId = policyGroup.PolicyGroupId;
            policyMessageGroupItemHotelVM.PolicyMessageGroupItemHotel = policyMessageGroupItemHotel;

            //Populate List of PolicyLocations
            PolicyLocationRepository policyLocationRepository = new PolicyLocationRepository();
            SelectList policyLocations = new SelectList(policyLocationRepository.GetAllPolicyLocations().ToList(), "PolicyLocationId", "PolicyLocationName", policyMessageGroupItemHotel.PolicyLocationId);
            policyMessageGroupItemHotelVM.PolicyLocations = policyLocations;

            return View(policyMessageGroupItemHotelVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PolicyMessageGroupItemHotelVM policyMessageGroupItemHotelVM)
        {
            int policyGroupId = policyMessageGroupItemHotelVM.PolicyMessageGroupItemHotel.PolicyGroupId;
            int policyMessageGroupItemId = policyMessageGroupItemHotelVM.PolicyMessageGroupItemHotel.PolicyMessageGroupItemId;

            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(policyGroupId); ;

            PolicyMessageGroupItemHotel policyMessageGroupItemHotel = new PolicyMessageGroupItemHotel();
            policyMessageGroupItemHotel = policyMessageGroupItemHotelRepository.GetPolicyMessageGroupItemHotel(policyMessageGroupItemId); ;

            //Check Exists
            if (policyMessageGroupItemHotel == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroupMessages(policyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update Model from Form
            try
            {
                UpdateModel<PolicyMessageGroupItemHotel>(policyMessageGroupItemHotelVM.PolicyMessageGroupItemHotel, "PolicyMessageGroupItemHotel");
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
                policyMessageGroupItemHotelRepository.Edit(policyMessageGroupItemHotelVM.PolicyMessageGroupItemHotel);
            }
            catch (SqlException ex)
            {

                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            return RedirectToAction("List", "PolicyMessageGroupItem", new { id = policyGroupId });
        }

        public ActionResult View(int id)
        {
            PolicyMessageGroupItemHotel policyMessageGroupItemHotel = new PolicyMessageGroupItemHotel();
            policyMessageGroupItemHotel = policyMessageGroupItemHotelRepository.GetPolicyMessageGroupItemHotel(id); ;

            //Check Exists
            if (policyMessageGroupItemHotel == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(policyMessageGroupItemHotel.PolicyGroupId);

            PolicyMessageGroupItemHotelVM policyMessageGroupItemHotelVM = new PolicyMessageGroupItemHotelVM();
            policyMessageGroupItemHotelVM.PolicyGroupName = policyGroup.PolicyGroupName;
            policyMessageGroupItemHotelVM.PolicyGroupId = policyGroup.PolicyGroupId;
            policyMessageGroupItemHotelVM.PolicyMessageGroupItemHotel = policyMessageGroupItemHotel;

            //Populate List of PolicyLocations
            PolicyLocationRepository policyLocationRepository = new PolicyLocationRepository();
            SelectList policyLocations = new SelectList(policyLocationRepository.GetAllPolicyLocations().ToList(), "PolicyLocationId", "PolicyLocationName", policyMessageGroupItemHotel.PolicyLocationId);
            policyMessageGroupItemHotelVM.PolicyLocations = policyLocations;

            return View(policyMessageGroupItemHotelVM);
        }

		[HttpGet]
		public ActionResult Delete(int id)
        {
            PolicyMessageGroupItemHotel policyMessageGroupItemHotel = new PolicyMessageGroupItemHotel();
            policyMessageGroupItemHotel = policyMessageGroupItemHotelRepository.GetPolicyMessageGroupItemHotel(id); ;

            //Check Exists
            if (policyMessageGroupItemHotel == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(policyMessageGroupItemHotel.PolicyGroupId);

            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroupMessages(policyGroup.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            PolicyMessageGroupItemHotelVM policyMessageGroupItemHotelVM = new PolicyMessageGroupItemHotelVM();
            policyMessageGroupItemHotelVM.PolicyGroupName = policyGroup.PolicyGroupName;
            policyMessageGroupItemHotelVM.PolicyGroupId = policyGroup.PolicyGroupId;
            policyMessageGroupItemHotelVM.PolicyMessageGroupItemHotel = policyMessageGroupItemHotel;

            //Populate List of PolicyLocations
            PolicyLocationRepository policyLocationRepository = new PolicyLocationRepository();
            SelectList policyLocations = new SelectList(policyLocationRepository.GetAllPolicyLocations().ToList(), "PolicyLocationId", "PolicyLocationName", policyMessageGroupItemHotel.PolicyLocationId);
            policyMessageGroupItemHotelVM.PolicyLocations = policyLocations;

            return View(policyMessageGroupItemHotelVM);
        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            PolicyMessageGroupItemHotel policyMessageGroupItemHotel = new PolicyMessageGroupItemHotel();
            policyMessageGroupItemHotel = policyMessageGroupItemHotelRepository.GetPolicyMessageGroupItemHotel(id); ;

            //Check Exists
            if (policyMessageGroupItemHotel == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            int policyGroupId = policyMessageGroupItemHotel.PolicyGroupId;

            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Delete Item
            try
            {
                policyMessageGroupItemHotel.VersionNumber = Int32.Parse(collection["PolicyMessageGroupItemHotel.VersionNumber"]);
                policyMessageGroupItemHotelRepository.Delete(policyMessageGroupItemHotel);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/PolicyMessageGroupItemHotel.mvc/Delete/" + policyMessageGroupItemHotel.PolicyMessageGroupItemId.ToString();
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            //Return
            return RedirectToAction("List", "PolicyMessageGroupItem", new { id = policyGroupId });

        }
    }
}
