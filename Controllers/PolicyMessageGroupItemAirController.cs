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
    public class PolicyMessageGroupItemAirController : Controller
    {
        PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();
        PolicyMessageGroupItemRepository policyMessageGroupItemRepository = new PolicyMessageGroupItemRepository();
        PolicyMessageGroupItemAirRepository policyMessageGroupItemAirRepository = new PolicyMessageGroupItemAirRepository();
        PolicyRoutingRepository policyRoutingRepository = new PolicyRoutingRepository();
        
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

            PolicyMessageGroupItemAirVM policyMessageGroupItemAirVM = new PolicyMessageGroupItemAirVM();
            policyMessageGroupItemAirVM.PolicyGroupName = policyGroup.PolicyGroupName;
            policyMessageGroupItemAirVM.PolicyGroupId = policyGroup.PolicyGroupId;

            PolicyMessageGroupItemAir policyMessageGroupItemAir = new PolicyMessageGroupItemAir();
            policyMessageGroupItemAir.ProductId = 1; //Air
            policyMessageGroupItemAir.PolicyGroupId = policyGroup.PolicyGroupId;
            policyMessageGroupItemAirVM.PolicyMessageGroupItemAir = policyMessageGroupItemAir;

            //Populate List of PolicyLocations
            PolicyLocationRepository policyLocationRepository = new PolicyLocationRepository();
            SelectList policyLocations = new SelectList(policyLocationRepository.GetAllPolicyLocations().ToList(), "PolicyLocationId", "PolicyLocationName");

            PolicyRouting policyRouting = new PolicyRouting();
            policyRouting.FromGlobalFlag = false;
            policyRouting.ToGlobalFlag = false;
            policyMessageGroupItemAir.PolicyRouting = policyRouting;

            return View(policyMessageGroupItemAirVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PolicyMessageGroupItemAirVM policyMessageGroupItemAirVM)
        {
            int policyGroupId = policyMessageGroupItemAirVM.PolicyMessageGroupItemAir.PolicyGroupId;

            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(policyGroupId); ;

            //Check Exists
            if (policyGroup == null)
            {
                ViewData["ActionMethod"] = "CreatePosy";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroupMessages(policyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Edit Routing
            PolicyRouting policyRouting = new PolicyRouting();
            policyRouting = policyMessageGroupItemAirVM.PolicyRouting;
            policyRoutingRepository.EditPolicyRouting(policyRouting);

            try
            {
                policyMessageGroupItemAirRepository.Add(policyMessageGroupItemAirVM.PolicyMessageGroupItemAir, policyRouting);
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
            PolicyMessageGroupItemAir policyMessageGroupItemAir = new PolicyMessageGroupItemAir();
            policyMessageGroupItemAir = policyMessageGroupItemAirRepository.GetPolicyMessageGroupItemAir(id); ;

            //Check Exists
            if (policyMessageGroupItemAir == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(policyMessageGroupItemAir.PolicyGroupId);

            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroupMessages(policyGroup.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            PolicyMessageGroupItemAirVM policyMessageGroupItemAirVM = new PolicyMessageGroupItemAirVM();
            policyMessageGroupItemAirVM.PolicyGroupName = policyGroup.PolicyGroupName;
            policyMessageGroupItemAirVM.PolicyGroupId = policyGroup.PolicyGroupId;
            policyMessageGroupItemAirVM.PolicyMessageGroupItemAir = policyMessageGroupItemAir;

            //add the PolicyRouting information
            PolicyRouting policyRouting = policyRoutingRepository.GetPolicyRouting((int)policyMessageGroupItemAir.PolicyRoutingId);
            policyRoutingRepository.EditForDisplay(policyRouting);
            policyMessageGroupItemAirVM.PolicyRouting = policyRouting;

            return View(policyMessageGroupItemAirVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PolicyMessageGroupItemAirVM policyMessageGroupItemAirVM)
        {
            int policyGroupId = policyMessageGroupItemAirVM.PolicyMessageGroupItemAir.PolicyGroupId;
            int policyMessageGroupItemId = policyMessageGroupItemAirVM.PolicyMessageGroupItemAir.PolicyMessageGroupItemId;

            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(policyGroupId); ;

            PolicyMessageGroupItemAir policyMessageGroupItemAir = new PolicyMessageGroupItemAir();
            policyMessageGroupItemAir = policyMessageGroupItemAirRepository.GetPolicyMessageGroupItemAir(policyMessageGroupItemId); ;

            PolicyRouting policyRouting = new PolicyRouting();
            policyRouting = policyMessageGroupItemAirVM.PolicyRouting;

            //Check Exists
            if (policyMessageGroupItemAir == null || policyRouting == null)
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

            //Edit Routing
            policyRoutingRepository.EditPolicyRouting(policyRouting);

            try
            {
                policyMessageGroupItemAirRepository.Edit(policyMessageGroupItemAirVM.PolicyMessageGroupItemAir, policyRouting);
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
            PolicyMessageGroupItemAir policyMessageGroupItemAir = new PolicyMessageGroupItemAir();
            policyMessageGroupItemAir = policyMessageGroupItemAirRepository.GetPolicyMessageGroupItemAir(id); ;

            //Check Exists
            if (policyMessageGroupItemAir == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(policyMessageGroupItemAir.PolicyGroupId);

            PolicyMessageGroupItemAirVM policyMessageGroupItemAirVM = new PolicyMessageGroupItemAirVM();
            policyMessageGroupItemAirVM.PolicyGroupName = policyGroup.PolicyGroupName;
            policyMessageGroupItemAirVM.PolicyGroupId = policyGroup.PolicyGroupId;
            policyMessageGroupItemAirVM.PolicyMessageGroupItemAir = policyMessageGroupItemAir;

            //add the PolicyRouting information
            PolicyRouting policyRouting = policyRoutingRepository.GetPolicyRouting((int)policyMessageGroupItemAir.PolicyRoutingId);
            policyRoutingRepository.EditForDisplay(policyRouting);
            policyMessageGroupItemAirVM.PolicyRouting = policyRouting;


            return View(policyMessageGroupItemAirVM);
        }

		[HttpGet]
		public ActionResult Delete(int id)
        {
            PolicyMessageGroupItemAir policyMessageGroupItemAir = new PolicyMessageGroupItemAir();
            policyMessageGroupItemAir = policyMessageGroupItemAirRepository.GetPolicyMessageGroupItemAir(id); ;

            //Check Exists
            if (policyMessageGroupItemAir == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(policyMessageGroupItemAir.PolicyGroupId);

            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroupMessages(policyGroup.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            PolicyMessageGroupItemAirVM policyMessageGroupItemAirVM = new PolicyMessageGroupItemAirVM();
            policyMessageGroupItemAirVM.PolicyGroupName = policyGroup.PolicyGroupName;
            policyMessageGroupItemAirVM.PolicyGroupId = policyGroup.PolicyGroupId;
            policyMessageGroupItemAirVM.PolicyMessageGroupItemAir = policyMessageGroupItemAir;

            //add the PolicyRouting information
            PolicyRouting policyRouting = policyRoutingRepository.GetPolicyRouting((int)policyMessageGroupItemAir.PolicyRoutingId);
            policyRoutingRepository.EditForDisplay(policyRouting);
            policyMessageGroupItemAirVM.PolicyRouting = policyRouting;


            return View(policyMessageGroupItemAirVM);
        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            PolicyMessageGroupItemAir policyMessageGroupItemAir = new PolicyMessageGroupItemAir();
            policyMessageGroupItemAir = policyMessageGroupItemAirRepository.GetPolicyMessageGroupItemAir(id); ;

            //Check Exists
            if (policyMessageGroupItemAir == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            int policyGroupId = policyMessageGroupItemAir.PolicyGroupId;

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
                policyMessageGroupItemAir.VersionNumber = Int32.Parse(collection["PolicyMessageGroupItemAir.VersionNumber"]);
                policyMessageGroupItemAirRepository.Delete(policyMessageGroupItemAir);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/PolicyMessageGroupItemAir.mvc/Delete/" + policyMessageGroupItemAir.PolicyMessageGroupItemId.ToString();
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
