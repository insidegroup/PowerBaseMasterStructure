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
    public class ClientFeeController : Controller
    {
        HierarchyRepository hierarchyRepository = new HierarchyRepository();
        ClientFeeRepository clientFeeRepository = new ClientFeeRepository();
        ClientFeeOutputRepository clientFeeOutputRepository = new ClientFeeOutputRepository();
        private string groupName = "ClientFee";

        //GET: A list of ClientFees
        public ActionResult List(string filter, int? page, string sortField, int? sortOrder)
        {
            //Check Access Rights to Domain
            ViewData["Access"] = "";
            if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //SortField + SortOrder settings
            if (sortField != "FeeTypeDescription" && sortField != "ContextName" && sortField != "GDSName")
            {
                sortField = "ClientFeeDescription";
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

            var items = clientFeeRepository.PageClientFees(page ?? 1, filter ?? "", sortField, sortOrder ?? 0);
            return View(items);
        }

        // GET: Create A Single ClientFee
        public ActionResult Create()
        {
            //Check Access Rights to Domain
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ClientFeeVM clientFeeVM = new ClientFeeVM();
            ClientFee clientFee = new ClientFee();

            FeeType feeType = new FeeType();
            clientFee.FeeType = feeType;
            clientFeeVM.ClientFee = clientFee;

            FeeTypeRepository feeTypeRepository = new FeeTypeRepository();
            clientFeeVM.FeeTypes = feeTypeRepository.GetAllFeeTypes().ToList();
            
            GDSRepository gdsRepository = new GDSRepository();
            clientFeeVM.GDSs = new SelectList(gdsRepository.GetAllGDSs().ToList(), "GDSCode", "GDSName");

            ContextRepository contextRepository = new ContextRepository();
            clientFeeVM.Contexts = new SelectList(contextRepository.GetAllContexts().ToList(), "ContextId", "ContextName");

            return View(clientFeeVM);
        }

        // POST: //Create/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClientFeeVM clientFeeVM)
        {
            //Check Access Rights to Domain
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update  Model from Form
            try
            {
                UpdateModel(clientFeeVM);
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
                clientFeeRepository.Add(clientFeeVM);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List");
        }

        // GET: Edit A Single ClientFee
        public ActionResult Edit(int id)
        {
            //Check Access Rights to Domain
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ClientFee clientFee = new ClientFee();
            clientFee = clientFeeRepository.GetItem(id);

            //Check Exists
            if (clientFee == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Change DisplayText for Transaction Fees
            FeeType feeType = new FeeType();
            feeType = clientFee.FeeType;
            if (feeType.FeeTypeDescription == "Client Fee")
            {
                feeType.FeeTypeDescription = "Transaction Fee";
            }

            ClientFeeVM clientFeeVM = new ClientFeeVM();
            clientFeeVM.ClientFee = clientFee;

           

            GDSRepository gdsRepository = new GDSRepository();
            clientFeeVM.GDSs = new SelectList(gdsRepository.GetAllGDSs().ToList(), "GDSCode", "GDSName", clientFee.GDSCode);

            ContextRepository contextRepository = new ContextRepository();
            clientFeeVM.Contexts = new SelectList(contextRepository.GetAllContexts().ToList(), "ContextId", "ContextName", clientFee.ContextId);

            //Check for missing GDS
            if (clientFee.GDS == null)
            {
                GDS gds = new GDS();
                clientFee.GDS = gds;
            }

            ClientFeeOutput clientFeeOutput = new ClientFeeOutput();
            if (clientFee.ClientFeeOutputs.Count > 0)
            {
                clientFeeOutput = clientFeeOutputRepository.GetItem(clientFee.ClientFeeOutputs[0].ClientFeeOutputId);
            }
            clientFeeVM.ClientFeeOutput = clientFeeOutput;

            return View(clientFeeVM);
        }

        // POST: //Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ClientFeeVM clientFeeVM)
        {
            //Check Access Rights to Domain
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ClientFee clientFee = new ClientFee();
            clientFee = clientFeeRepository.GetItem(clientFeeVM.ClientFee.ClientFeeId);

            //Check Exists
            if (clientFee == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //Update  Model from Form
            try
            {
                UpdateModel(clientFeeVM);
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
                clientFeeRepository.Update(clientFeeVM);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List");
        }

        // GET: View A Single ClientFee
        public ActionResult View(int id)
        {
            ClientFee clientFee = new ClientFee();
            clientFee = clientFeeRepository.GetItem(id);

            //Check Exists
            if (clientFee == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            //Check for missing GDS
            if (clientFee.GDS == null)
            {
                GDS gds = new GDS();
                clientFee.GDS = gds;
            }

            //Change DisplayText for Transaction Fees
            FeeType feeType = new FeeType();
            feeType = clientFee.FeeType;
            if (feeType.FeeTypeDescription == "Client Fee")
            {
                feeType.FeeTypeDescription = "Transaction Fee";
            }

            ClientFeeVM clientFeeVM = new ClientFeeVM();
            clientFeeVM.ClientFee = clientFee;

            ClientFeeOutput clientFeeOutput = new ClientFeeOutput();
            if (clientFee.ClientFeeOutputs.Count > 0)
            {
                clientFeeOutput = clientFeeOutputRepository.GetItem(clientFee.ClientFeeOutputs[0].ClientFeeOutputId);
            }
            clientFeeVM.ClientFeeOutput = clientFeeOutput;

            return View(clientFeeVM);
        }

        // GET: Delete A Single ClientFee
		[HttpGet]
		public ActionResult Delete(int id)
        {
            //Check Access Rights to Domain
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ClientFee clientFee = new ClientFee();
            clientFee = clientFeeRepository.GetItem(id);

            //Check Exists
            if (clientFee == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            //Check for missing GDS
            if (clientFee.GDS == null)
            {
                GDS gds = new GDS();
                clientFee.GDS = gds;
            }

            ClientFeeVM clientFeeVM = new ClientFeeVM();
            clientFeeVM.ClientFee = clientFee;

            ClientFeeOutput clientFeeOutput = new ClientFeeOutput();
            if (clientFee.ClientFeeOutputs.Count > 0)
            {
                clientFeeOutput = clientFeeOutputRepository.GetItem(clientFee.ClientFeeOutputs[0].ClientFeeOutputId);
            }
            clientFeeVM.ClientFeeOutput = clientFeeOutput;

            return View(clientFeeVM);
        }

        // POST: //Delete/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            //Check Access Rights to Domain
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ClientFee clientFee = new ClientFee();
            clientFee = clientFeeRepository.GetItem(id);

            //Check Exists
            if (clientFee == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            //Delete Item
            try
            {
                clientFee.VersionNumber = Int32.Parse(collection["ClientFee.VersionNumber"]);
                clientFeeRepository.Delete(clientFee);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/ClientFee.mvc/Delete/" + clientFee.ClientFeeId.ToString();
                    return View("VersionError");
                }
                //Restraint Error - go to standard DeleteError page
                if (ex.Message == "SQLDeleteError")
                {
                    ViewData["ReturnURL"] = "/ClientFee.mvc/Delete/" + clientFee.ClientFeeId.ToString();
                    return View("DeleteError");
                }
                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");            
            }

            //Return
            return RedirectToAction("List");
        }
    }
}
