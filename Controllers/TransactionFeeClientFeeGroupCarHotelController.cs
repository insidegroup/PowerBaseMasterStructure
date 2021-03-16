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
    public class TransactionFeeClientFeeGroupCarHotelController : Controller
    {

        HierarchyRepository hierarchyRepository = new HierarchyRepository();
        TransactionFeeClientFeeGroupRepository transactionFeeClientFeeGroupRepository = new TransactionFeeClientFeeGroupRepository();
        TransactionFeeCarHotelRepository transactionFeeCarHotelRepository = new TransactionFeeCarHotelRepository();
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

            ClientFeeGroupTransactionFeeCarHotelVM clientFeeGroupTransactionFeeCarHotelVM = new ClientFeeGroupTransactionFeeCarHotelVM();

            ClientFeeGroup clientFeeGroup = new ClientFeeGroup();
            clientFeeGroup = clientFeeGroupRepository.GetGroup(cid);
            clientFeeGroupTransactionFeeCarHotelVM.ClientFeeGroup = clientFeeGroup;

            TransactionFeeCarHotel transactionFeeCarHotel = new TransactionFeeCarHotel();
            transactionFeeCarHotel = transactionFeeCarHotelRepository.GetItem(tid);
            transactionFeeCarHotelRepository.EditForDisplay(transactionFeeCarHotel);
            clientFeeGroupTransactionFeeCarHotelVM.TransactionFeeCarHotel = transactionFeeCarHotel;


            return View(clientFeeGroupTransactionFeeCarHotelVM);
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

            ClientFeeGroupTransactionFeeCarHotelVM clientFeeGroupTransactionFeeCarHotelVM = new ClientFeeGroupTransactionFeeCarHotelVM();
            clientFeeGroupTransactionFeeCarHotelVM.TransactionFeeClientFeeGroup = transactionFeeClientFeeGroup;

            ClientFeeGroup clientFeeGroup = new ClientFeeGroup();
            clientFeeGroup = clientFeeGroupRepository.GetGroup(cid);
            clientFeeGroupTransactionFeeCarHotelVM.ClientFeeGroup = clientFeeGroup;

            TransactionFeeCarHotel transactionFeeCarHotel = new TransactionFeeCarHotel();
            transactionFeeCarHotel = transactionFeeCarHotelRepository.GetItem(tid);
            transactionFeeCarHotelRepository.EditForDisplay(transactionFeeCarHotel);
            clientFeeGroupTransactionFeeCarHotelVM.TransactionFeeCarHotel = transactionFeeCarHotel;

            return View(clientFeeGroupTransactionFeeCarHotelVM);
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
                    ViewData["ReturnURL"] = "/TransactionFeeClientFeeGroupCarHotel.mvc/Delete?tid=" + tid.ToString() + "&cid=" + cid.ToString();
                    return View("VersionError");
                }
                //Restraint Error - go to standard DeleteError page
                if (ex.Message == "SQLDeleteError")
                {
                    ViewData["ReturnURL"] = "/TransactionFeeClientFeeGroupCarHotel.mvc/Delete?tid=" + tid.ToString() + "&cid=" + cid.ToString();
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
