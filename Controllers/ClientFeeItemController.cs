using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Repository;
using CWTDesktopDatabase.ViewModels;
using System.Data.SqlClient;

namespace CWTDesktopDatabase.Controllers
{
    public class ClientFeeItemController : Controller
    {
        //main repository
        ClientFeeGroupRepository clientFeeGroupRepository = new ClientFeeGroupRepository();
        ClientFeeItemRepository clientFeeItemRepository = new ClientFeeItemRepository();
        ClientFeeRepository clientFeeRepository = new ClientFeeRepository();

        public ActionResult List(int id, string filter, int? page, string sortField, int? sortOrder)
        {
            //Get Item From Database
            ClientFeeGroup clientFeeGroup = new ClientFeeGroup();
            clientFeeGroup = clientFeeGroupRepository.GetGroup(id);

            //Check Exists
            if (clientFeeGroup == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }
            //SortField
            if (sortField != "OutputFormat" && sortField != "OutputDescription")
            {
                sortField = "ClientFeeDescription";
            }
            ViewData["CurrentSortField"] = sortField;

            //SortOrder
            if (sortOrder == 1)
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
            ClientFeeItemsVM clientFeeItemsVM = new ClientFeeItemsVM();
            clientFeeItemsVM.ClientFeeGroup = clientFeeGroup;

            if (clientFeeGroup.FeeTypeId == 1)
            {
                clientFeeItemsVM.FeeTypeDisplayName = "Supplemental Fee Group";
            }
            else
            {
                clientFeeItemsVM.FeeTypeDisplayName = "Transaction Fee Group";
            }

            FeeTypeRepository feeTypeRepository = new FeeTypeRepository();
            FeeType feeType = new FeeType();
            feeType = feeTypeRepository.GetFeeType((int)clientFeeGroup.FeeTypeId);
            clientFeeItemsVM.FeeType = feeType;

            clientFeeItemsVM.ClientFeeItems = clientFeeItemRepository.PageClientFeeItems(id, page ?? 1, sortField, sortOrder ?? 0);
            
             //Check Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToClientFeeGroup(clientFeeGroup.ClientFeeGroupId))
            {
                clientFeeItemsVM.HasWriteAccess = true;
            }
            
            return View(clientFeeItemsVM);  
        }
        
        public ActionResult Create(int id)
        {
             //Get Item From Database
            ClientFeeGroup clientFeeGroup = new ClientFeeGroup();
            clientFeeGroup = clientFeeGroupRepository.GetGroup(id);

             //Check Exists
            if (clientFeeGroup == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientFeeGroup(id))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ClientFeeItemVM clientFeeItemVM = new ClientFeeItemVM();

            ClientFeeItem clientFeeItem = new ClientFeeItem();
            clientFeeItem.ClientFeeGroup = clientFeeGroup;
            clientFeeItemVM.ClientFeeItem = clientFeeItem;

            FeeTypeRepository feeTypeRepository = new FeeTypeRepository();
            FeeType feeType = new FeeType();
            feeType = feeTypeRepository.GetFeeType((int)clientFeeGroup.FeeTypeId);
            clientFeeItemVM.FeeType = feeType;

            SelectList clientFees = new SelectList(clientFeeRepository.GetClientFeesByType((int)clientFeeGroup.FeeTypeId).ToList(), "ClientFeeId", "ClientFeeDescription");
            clientFeeItemVM.ClientFees = clientFees;
            
            return View(clientFeeItemVM);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClientFeeItemVM clientFeeItemVM)
        {
            //Get Item From Database
            ClientFeeGroup clientFeeGroup = new ClientFeeGroup();
            clientFeeGroup = clientFeeGroupRepository.GetGroup(clientFeeItemVM.ClientFeeItem.ClientFeeGroupId);

            //Check Exists
            if (clientFeeGroup == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }
            //Check Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientFeeGroup(clientFeeItemVM.ClientFeeItem.ClientFeeGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update Model From Form + Validate against DB
            try
            {
                UpdateModel <ClientFeeItem>(clientFeeItemVM.ClientFeeItem, "ClientFeeItem");
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
                clientFeeItemRepository.Add(clientFeeItemVM.ClientFeeItem);
            }
            catch (SqlException ex)
            {
                //Non-Unique Name
                if (ex.Message == "NonUniqueName")
                {
                    return View("NonUniqueNameError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            ViewData["NewSortOrder"] = 0;
            return RedirectToAction("List", new { id = clientFeeItemVM.ClientFeeItem.ClientFeeGroupId});
        }

        // GET: /View
        public ActionResult View(int id)
        {
            //Check Exists
            ClientFeeItem clientFeeItem = new ClientFeeItem();
            clientFeeItem = clientFeeItemRepository.GetItem(id);
            if (clientFeeItem == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }


            FeeTypeRepository feeTypeRepository = new FeeTypeRepository();
            FeeType feeType = new FeeType();
            feeType = feeTypeRepository.GetFeeType(clientFeeItem.ClientFee.FeeTypeId);

            ViewData["FeeTypeDescription"] = feeType.FeeTypeDescription;
            ViewData["ClientFeeGroupName"] = clientFeeItem.ClientFeeGroup.ClientFeeGroupName;
            ViewData["ClientFeeGroupId"] = clientFeeItem.ClientFeeGroup.ClientFeeGroupId;

            return View(clientFeeItem);
        }

        public ActionResult Edit(int id)
        {
            //Get Item From Database
            ClientFeeItem clientFeeItem = new ClientFeeItem();
            clientFeeItem = clientFeeItemRepository.GetItem(id);

            //Check Exists
            if (clientFeeItem == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientFeeGroup(clientFeeItem.ClientFeeGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ClientFeeItemVM clientFeeItemVM = new ClientFeeItemVM();
            clientFeeItemVM.ClientFeeItem = clientFeeItem;

            FeeTypeRepository feeTypeRepository = new FeeTypeRepository();
            FeeType feeType = new FeeType();
            feeType = feeTypeRepository.GetFeeType(clientFeeItem.ClientFee.FeeTypeId);
            clientFeeItemVM.FeeType = feeType;

            SelectList clientFees = new SelectList(clientFeeRepository.GetClientFeesByType(clientFeeItem.ClientFee.FeeTypeId).ToList(), "ClientFeeId", "ClientFeeDescription");
            clientFeeItemVM.ClientFees = clientFees;

            return View(clientFeeItemVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ClientFeeItemVM clientFeeItemVM)
        {

            ClientFeeItem clientFeeItem = new ClientFeeItem();
            clientFeeItem = clientFeeItemRepository.GetItem(clientFeeItemVM.ClientFeeItem.ClientFeeItemId);

            //Check Exists
            if (clientFeeItem == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //Update  Model from Form
            try
            {
                UpdateModel<ClientFeeItem>(clientFeeItem, "ClientFeeItem");
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
                clientFeeItemRepository.Update(clientFeeItem);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List", new { id = clientFeeItem.ClientFeeGroup.ClientFeeGroupId});
        }

        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id)
        {
            //Get Item From Database
            ClientFeeItem clientFeeItem = new ClientFeeItem();
            clientFeeItem = clientFeeItemRepository.GetItem(id);

            //Check Exists
            if (clientFeeItem == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientFeeGroup(clientFeeItem.ClientFeeGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ClientFeeItemVM clientFeeItemVM = new ClientFeeItemVM();
            clientFeeItemVM.ClientFeeItem = clientFeeItem;

            FeeTypeRepository feeTypeRepository = new FeeTypeRepository();
            FeeType feeType = new FeeType();
            feeType = feeTypeRepository.GetFeeType(clientFeeItem.ClientFee.FeeTypeId);
            if(feeType.FeeTypeDescription == "Client Fee"){
                feeType.FeeTypeDescription = "Transaction Fee";
            }
            clientFeeItemVM.FeeType = feeType;

            SelectList clientFees = new SelectList(clientFeeRepository.GetClientFeesByType(clientFeeItem.ClientFee.FeeTypeId).ToList(), "ClientFeeId", "ClientFeeDescription");
            clientFeeItemVM.ClientFees = clientFees;

            return View(clientFeeItemVM);
        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(ClientFeeItemVM clientFeeItemVM)
        {
            //Get Item
            ClientFeeItem clientFeeItem = new ClientFeeItem();
            clientFeeItem = clientFeeItemRepository.GetItem(clientFeeItemVM.ClientFeeItem.ClientFeeItemId);

            //Check Exists
            if (clientFeeItem == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }


            //Delete Item
            try
            {
                clientFeeItemRepository.Delete(clientFeeItemVM.ClientFeeItem);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/ClientFeeItem.mvc/Delete/" + clientFeeItem.ClientFeeItemId.ToString();
                    return View("VersionError");
                }
                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            //Return
            return RedirectToAction("List", new { id = clientFeeItem.ClientFeeGroup.ClientFeeGroupId });
        }
    }
}
