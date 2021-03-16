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
    public class PolicyMessageGroupItemCarController : Controller
    {
        PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();
        PolicyMessageGroupItemRepository policyMessageGroupItemRepository = new PolicyMessageGroupItemRepository();
        PolicyMessageGroupItemCarRepository policyMessageGroupItemCarRepository = new PolicyMessageGroupItemCarRepository();

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

            PolicyMessageGroupItemCarVM policyMessageGroupItemCarVM = new PolicyMessageGroupItemCarVM();
            policyMessageGroupItemCarVM.PolicyGroupName = policyGroup.PolicyGroupName;
            policyMessageGroupItemCarVM.PolicyGroupId = policyGroup.PolicyGroupId;

            PolicyMessageGroupItemCar policyMessageGroupItemCar = new PolicyMessageGroupItemCar();
            policyMessageGroupItemCar.ProductId = 3; //Car
            policyMessageGroupItemCar.PolicyGroupId = policyGroup.PolicyGroupId;
            policyMessageGroupItemCarVM.PolicyMessageGroupItemCar = policyMessageGroupItemCar;

            //Populate List of PolicyLocations
            PolicyLocationRepository policyLocationRepository = new PolicyLocationRepository();
            SelectList policyLocations = new SelectList(policyLocationRepository.GetAllPolicyLocations().ToList(), "PolicyLocationId", "PolicyLocationName");
            policyMessageGroupItemCarVM.PolicyLocations = policyLocations;

            return View(policyMessageGroupItemCarVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PolicyMessageGroupItemCarVM policyMessageGroupItemCarVM)
        {
            int policyGroupId = policyMessageGroupItemCarVM.PolicyMessageGroupItemCar.PolicyGroupId;

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
                UpdateModel<PolicyMessageGroupItemCar>(policyMessageGroupItemCarVM.PolicyMessageGroupItemCar, "PolicyMessageGroupItemCar");
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
                policyMessageGroupItemCarRepository.Add(policyMessageGroupItemCarVM.PolicyMessageGroupItemCar);
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
            PolicyMessageGroupItemCar policyMessageGroupItemCar = new PolicyMessageGroupItemCar();
            policyMessageGroupItemCar = policyMessageGroupItemCarRepository.GetPolicyMessageGroupItemCar(id); ;

            //Check Exists
            if (policyMessageGroupItemCar == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(policyMessageGroupItemCar.PolicyGroupId);

            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroupMessages(policyGroup.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            PolicyMessageGroupItemCarVM policyMessageGroupItemCarVM = new PolicyMessageGroupItemCarVM();
            policyMessageGroupItemCarVM.PolicyGroupName = policyGroup.PolicyGroupName;
            policyMessageGroupItemCarVM.PolicyGroupId = policyGroup.PolicyGroupId;
            policyMessageGroupItemCarVM.PolicyMessageGroupItemCar = policyMessageGroupItemCar;

            //Populate List of PolicyLocations
            PolicyLocationRepository policyLocationRepository = new PolicyLocationRepository();
            SelectList policyLocations = new SelectList(policyLocationRepository.GetAllPolicyLocations().ToList(), "PolicyLocationId", "PolicyLocationName", policyMessageGroupItemCar.PolicyLocationId);
            policyMessageGroupItemCarVM.PolicyLocations = policyLocations;

            return View(policyMessageGroupItemCarVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PolicyMessageGroupItemCarVM policyMessageGroupItemCarVM)
        {
            int policyGroupId = policyMessageGroupItemCarVM.PolicyMessageGroupItemCar.PolicyGroupId;
            int policyMessageGroupItemId = policyMessageGroupItemCarVM.PolicyMessageGroupItemCar.PolicyMessageGroupItemId;

            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(policyGroupId); ;

            PolicyMessageGroupItemCar policyMessageGroupItemCar = new PolicyMessageGroupItemCar();
            policyMessageGroupItemCar = policyMessageGroupItemCarRepository.GetPolicyMessageGroupItemCar(policyMessageGroupItemId); ;

            //Check Exists
            if (policyMessageGroupItemCar == null)
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
                UpdateModel<PolicyMessageGroupItemCar>(policyMessageGroupItemCarVM.PolicyMessageGroupItemCar, "PolicyMessageGroupItemCar");
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
                policyMessageGroupItemCarRepository.Edit(policyMessageGroupItemCarVM.PolicyMessageGroupItemCar);
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
            PolicyMessageGroupItemCar policyMessageGroupItemCar = new PolicyMessageGroupItemCar();
            policyMessageGroupItemCar = policyMessageGroupItemCarRepository.GetPolicyMessageGroupItemCar(id); ;

            //Check Exists
            if (policyMessageGroupItemCar == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(policyMessageGroupItemCar.PolicyGroupId);

            PolicyMessageGroupItemCarVM policyMessageGroupItemCarVM = new PolicyMessageGroupItemCarVM();
            policyMessageGroupItemCarVM.PolicyGroupName = policyGroup.PolicyGroupName;
            policyMessageGroupItemCarVM.PolicyGroupId = policyGroup.PolicyGroupId;
            policyMessageGroupItemCarVM.PolicyMessageGroupItemCar = policyMessageGroupItemCar;

            return View(policyMessageGroupItemCarVM);
        }

		[HttpGet]
		public ActionResult Delete(int id)
        {
            PolicyMessageGroupItemCar policyMessageGroupItemCar = new PolicyMessageGroupItemCar();
            policyMessageGroupItemCar = policyMessageGroupItemCarRepository.GetPolicyMessageGroupItemCar(id); ;

            //Check Exists
            if (policyMessageGroupItemCar == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(policyMessageGroupItemCar.PolicyGroupId);

            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroupMessages(policyGroup.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            PolicyMessageGroupItemCarVM policyMessageGroupItemCarVM = new PolicyMessageGroupItemCarVM();
            policyMessageGroupItemCarVM.PolicyGroupName = policyGroup.PolicyGroupName;
            policyMessageGroupItemCarVM.PolicyGroupId = policyGroup.PolicyGroupId;
            policyMessageGroupItemCarVM.PolicyMessageGroupItemCar = policyMessageGroupItemCar;

            return View(policyMessageGroupItemCarVM);
        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            PolicyMessageGroupItemCar policyMessageGroupItemCar = new PolicyMessageGroupItemCar();
            policyMessageGroupItemCar = policyMessageGroupItemCarRepository.GetPolicyMessageGroupItemCar(id); ;

            //Check Exists
            if (policyMessageGroupItemCar == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            int policyGroupId = policyMessageGroupItemCar.PolicyGroupId;

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
                policyMessageGroupItemCar.VersionNumber = Int32.Parse(collection["PolicyMessageGroupItemCar.VersionNumber"]);
                policyMessageGroupItemCarRepository.Delete(policyMessageGroupItemCar);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/PolicyMessageGroupItemCar.mvc/Delete/" + policyMessageGroupItemCar.PolicyMessageGroupItemId.ToString();
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
