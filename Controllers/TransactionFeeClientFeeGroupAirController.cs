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
    public class TransactionFeeClientFeeGroupAirController : Controller
    {

        HierarchyRepository hierarchyRepository = new HierarchyRepository();
        TransactionFeeClientFeeGroupRepository transactionFeeClientFeeGroupRepository = new TransactionFeeClientFeeGroupRepository();
        TransactionFeeAirRepository transactionFeeAirRepository = new TransactionFeeAirRepository();
        ClientFeeGroupRepository clientFeeGroupRepository = new ClientFeeGroupRepository();
        PolicyRoutingRepository policyRoutingRepository = new PolicyRoutingRepository();
        private string groupName = "ClientFee";

        // GET: View A TransactionFeeClientFeeGroup
        public ActionResult View(int cid, int tid)
        {
            //Get Item From Database
            TransactionFeeClientFeeGroup transactionFeeClientFeeGroup = new TransactionFeeClientFeeGroup();
            transactionFeeClientFeeGroup = transactionFeeClientFeeGroupRepository.GetItem(cid, tid);

            //Check Exists
            if (transactionFeeClientFeeGroup == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            ClientFeeGroupTransactionFeeAirVM clientFeeGroupTransactionFeeAirVM = new ClientFeeGroupTransactionFeeAirVM();

            ClientFeeGroup clientFeeGroup = new ClientFeeGroup();
            clientFeeGroup = clientFeeGroupRepository.GetGroup(cid);
            clientFeeGroupTransactionFeeAirVM.ClientFeeGroup = clientFeeGroup;

            TransactionFeeAir transactionFeeAir = new TransactionFeeAir();
            transactionFeeAir = transactionFeeAirRepository.GetItem(tid);
            transactionFeeAirRepository.EditForDisplay(transactionFeeAir);
            clientFeeGroupTransactionFeeAirVM.TransactionFeeAir = transactionFeeAir;

            PolicyRouting policyRouting = new PolicyRouting();
            policyRouting = policyRoutingRepository.GetPolicyRouting((int)transactionFeeAir.PolicyRoutingId);
            policyRoutingRepository.EditForDisplay(policyRouting);
            clientFeeGroupTransactionFeeAirVM.PolicyRouting = policyRouting;

            return View(clientFeeGroupTransactionFeeAirVM);
        }

        // GET: View A TransactionFeeClientFeeGroup
		[HttpGet]
		public ActionResult Delete(int cid, int tid)
        {
            //Get Item From Database
            TransactionFeeClientFeeGroup transactionFeeClientFeeGroup = new TransactionFeeClientFeeGroup();
            transactionFeeClientFeeGroup = transactionFeeClientFeeGroupRepository.GetItem(cid, tid);

            //Check Exists
            if (transactionFeeClientFeeGroup == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            ClientFeeGroupTransactionFeeAirVM clientFeeGroupTransactionFeeAirVM = new ClientFeeGroupTransactionFeeAirVM();
            clientFeeGroupTransactionFeeAirVM.TransactionFeeClientFeeGroup = transactionFeeClientFeeGroup;

            ClientFeeGroup clientFeeGroup = new ClientFeeGroup();
            clientFeeGroup = clientFeeGroupRepository.GetGroup(cid);
            clientFeeGroupTransactionFeeAirVM.ClientFeeGroup = clientFeeGroup;

            TransactionFeeAir transactionFeeAir = new TransactionFeeAir();
            transactionFeeAir = transactionFeeAirRepository.GetItem(tid);
            transactionFeeAirRepository.EditForDisplay(transactionFeeAir);
            clientFeeGroupTransactionFeeAirVM.TransactionFeeAir = transactionFeeAir;

            PolicyRouting policyRouting = new PolicyRouting();
            policyRouting = policyRoutingRepository.GetPolicyRouting((int)transactionFeeAir.PolicyRoutingId);
            policyRoutingRepository.EditForDisplay(policyRouting);
            clientFeeGroupTransactionFeeAirVM.PolicyRouting = policyRouting;

            return View(clientFeeGroupTransactionFeeAirVM);
        }

        // POST: //Delete/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(TransactionFeeClientFeeGroup transactionFeeClientFeeGroup)
        {
            //Check Access Rights to Domain
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Get Item From Database
            int cid = transactionFeeClientFeeGroup.ClientFeeGroupId;
            int tid = transactionFeeClientFeeGroup.TransactionFeeId;
            transactionFeeClientFeeGroup = transactionFeeClientFeeGroupRepository.GetItem(cid, tid);

            //Check Exists
            if (transactionFeeClientFeeGroup == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            //Delete Item
            try
            {
                transactionFeeClientFeeGroupRepository.Delete(transactionFeeClientFeeGroup);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/TransactionFeeClientFeeGroupAir.mvc/Delete?tid=" + tid.ToString() + "&cid=" + cid.ToString();
                    return View("VersionError");
                }
                //Restraint Error - go to standard DeleteError page
                if (ex.Message == "SQLDeleteError")
                {
                    ViewData["ReturnURL"] = "/TransactionFeeClientFeeGroupAir.mvc/Delete?tid=" + tid.ToString() + "&cid=" + cid.ToString();
                    return View("DeleteError");
                }

				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            //Return
            return RedirectToAction("List", "TransactionFeeClientFeeGroup", new {id= cid });
        }
    
    }
}
