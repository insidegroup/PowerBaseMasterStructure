using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Repository;
using CWTDesktopDatabase.Models;
using System.Data.SqlClient;

namespace CWTDesktopDatabase.Controllers
{
    public class MerchantFeeClientFeeGroupController : Controller
    {
        HierarchyRepository hierarchyRepository = new HierarchyRepository();
        MerchantFeeClientFeeGroupRepository merchantFeeClientFeeGroupRepository = new MerchantFeeClientFeeGroupRepository();
        MerchantFeeRepository merchantFeeRepository = new MerchantFeeRepository();
        ClientFeeGroupRepository clientFeeGroupRepository = new ClientFeeGroupRepository();
        private string groupName = "ClientFee";

		public ActionResult List(int id, string filter, int? page, string sortField, int? sortOrder)
        {
            //Get ClientFeeGroup
            ClientFeeGroup clientFeeGroup = new ClientFeeGroup();
            clientFeeGroup = clientFeeGroupRepository.GetGroup(id);

            //Check Exists
            if (clientFeeGroup == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }

            //SortField + SortOrder settings
            if (sortField != "CountryName" && sortField != "CreditCardVendorName" && sortField != "ProductName" && sortField != "SupplierName" && sortField != "MerchantFeePercent")
            {
                sortField = "MerchantFeeDescription";
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

            ClientFeeGroupMerchantFeesVM clientFeeGroupMerchantFeesVM = new ClientFeeGroupMerchantFeesVM();
            clientFeeGroupMerchantFeesVM.ClientFeeGroup = clientFeeGroup;

            //Check Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                clientFeeGroupMerchantFeesVM.HasDomainWriteAccess = true;
            }

			clientFeeGroupMerchantFeesVM.ClientFeeGroupMerchantFees = merchantFeeClientFeeGroupRepository.PageMerchantFeeClientFeeGroups(id, page ?? 1, filter ?? "", sortField, sortOrder ?? 0);
            return View(clientFeeGroupMerchantFeesVM);
        }

        // GET: Create a MerchantFeeClientFeeGroup
        public ActionResult Create(int id)
        {
            //Check Access Rights to Domain
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Get ClientFeeGroup
            ClientFeeGroup clientFeeGroup = new ClientFeeGroup();
            clientFeeGroup = clientFeeGroupRepository.GetGroup(id);

            //Check Exists
            if (clientFeeGroup == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }
            
            ClientFeeGroupMerchantFeeVM clientFeeGroupMerchantFeeVM = new ClientFeeGroupMerchantFeeVM();
            clientFeeGroupMerchantFeeVM.ClientFeeGroup = clientFeeGroup;

            MerchantFeeClientFeeGroup merchantFeeClientFeeGroup = new MerchantFeeClientFeeGroup();
            merchantFeeClientFeeGroup.ClientFeeGroupId = id;
            clientFeeGroupMerchantFeeVM.MerchantFeeClientFeeGroup = merchantFeeClientFeeGroup;

            SelectList merchantFeeList = new SelectList(merchantFeeClientFeeGroupRepository.GetUnUsedMerchantFees(id, 0).ToList(), "MerchantFeeId", "MerchantFeeDescription");
            clientFeeGroupMerchantFeeVM.MerchantFees = merchantFeeList;

            return View(clientFeeGroupMerchantFeeVM);
        }

        // POST: Create a MerchantFeeClientFeeGroup
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MerchantFeeClientFeeGroup merchantFeeClientFeeGroup)
        {
            //Check Access Rights to Domain
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Get ClientFeeGroup
            ClientFeeGroup clientFeeGroup = new ClientFeeGroup();
            clientFeeGroup = clientFeeGroupRepository.GetGroup(merchantFeeClientFeeGroup.ClientFeeGroupId);

            //Check Exists
            if (clientFeeGroup == null)
            {
                ViewData["ActionMethod"] = "CreateGet1";
                return View("RecordDoesNotExistError");
            }
            //Get MerchantFee
            MerchantFee merchantFee = new MerchantFee();
            merchantFee = merchantFeeRepository.GetItem(merchantFeeClientFeeGroup.MerchantFeeId);

            //Check Exists
            if (merchantFee == null)
            {
                ViewData["ActionMethod"] = "CreateGet2";
                return View("RecordDoesNotExistError");
            }


            //Database Update
            try
            {
                merchantFeeClientFeeGroupRepository.Add(merchantFeeClientFeeGroup);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List", new {id = merchantFeeClientFeeGroup.ClientFeeGroupId});
         }

        // GET: Edit A MerchantFeeClientFeeGroup
        public ActionResult Edit(int id, int mId)
        {
            //Get Item From Database
            MerchantFeeClientFeeGroup merchantFeeClientFeeGroup = new MerchantFeeClientFeeGroup();
            merchantFeeClientFeeGroup = merchantFeeClientFeeGroupRepository.GetItem(id, mId);

            //Check Exists
            if (merchantFeeClientFeeGroup == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            ClientFeeGroupMerchantFeeVM clientFeeGroupMerchantFeeVM  = new ClientFeeGroupMerchantFeeVM();
            clientFeeGroupMerchantFeeVM.MerchantFeeClientFeeGroup = merchantFeeClientFeeGroup;

            MerchantFee merchantFee = new MerchantFee();
            merchantFee = merchantFeeRepository.GetItem(mId);
            merchantFeeRepository.EditForDisplay(merchantFee);
            clientFeeGroupMerchantFeeVM.MerchantFee = merchantFee;

            ClientFeeGroup clientFeeGroup = new ClientFeeGroup();
            clientFeeGroup = clientFeeGroupRepository.GetGroup(id);
            clientFeeGroupMerchantFeeVM.ClientFeeGroup = clientFeeGroup;
            clientFeeGroupMerchantFeeVM.OriginalMerchantFeeId = mId;

            SelectList merchantFeeList = new SelectList(merchantFeeClientFeeGroupRepository.GetUnUsedMerchantFees(id, mId).ToList(), "MerchantFeeId", "MerchantFeeDescription", mId);
            clientFeeGroupMerchantFeeVM.MerchantFees = merchantFeeList;

            return View(clientFeeGroupMerchantFeeVM);
        }


        // POST: Edit a MerchantFeeClientFeeGroup
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ClientFeeGroupMerchantFeeVM clientFeeGroupMerchantFeeVM)
        {
            //Check Access Rights to Domain
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Get ClientFeeGroup
            MerchantFeeClientFeeGroup merchantFeeClientFeeGroup = new MerchantFeeClientFeeGroup();
            merchantFeeClientFeeGroup = clientFeeGroupMerchantFeeVM.MerchantFeeClientFeeGroup;

            ClientFeeGroup clientFeeGroup = new ClientFeeGroup();
            clientFeeGroup = clientFeeGroupRepository.GetGroup(merchantFeeClientFeeGroup.ClientFeeGroupId);

            //Check Exists
            if (clientFeeGroup == null)
            {
                ViewData["ActionMethod"] = "EditGet1";
                return View("RecordDoesNotExistError");
            }
            //Get MerchantFee
            MerchantFee merchantFee = new MerchantFee();
            merchantFee = merchantFeeRepository.GetItem(merchantFeeClientFeeGroup.MerchantFeeId);

            //Check Exists
            if (merchantFee == null)
            {
                ViewData["ActionMethod"] = "EditGet2";
                return View("RecordDoesNotExistError");
            }


            //Database Update
            try
            {
                merchantFeeClientFeeGroupRepository.Update(merchantFeeClientFeeGroup, clientFeeGroupMerchantFeeVM.OriginalMerchantFeeId);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List", new { id = merchantFeeClientFeeGroup.ClientFeeGroupId });
        }

        // GET: View A MerchantFeeClientFeeGroup
        public ActionResult View(int id, int mId)
         {
             //Get Item From Database
             MerchantFeeClientFeeGroup merchantFeeClientFeeGroup = new MerchantFeeClientFeeGroup();
             merchantFeeClientFeeGroup = merchantFeeClientFeeGroupRepository.GetItem(id, mId);

             //Check Exists
             if (merchantFeeClientFeeGroup == null)
             {
                 ViewData["ActionMethod"] = "ViewGet";
                 return View("RecordDoesNotExistError");
             }

             ClientFeeGroupMerchantFeeVM clientFeeGroupMerchantFeeVM = new ClientFeeGroupMerchantFeeVM();

             MerchantFee merchantFee = new MerchantFee();
             merchantFee = merchantFeeRepository.GetItem(mId);
             merchantFeeRepository.EditForDisplay(merchantFee);
             clientFeeGroupMerchantFeeVM.MerchantFee = merchantFee;

             ClientFeeGroup clientFeeGroup = new ClientFeeGroup();
             clientFeeGroup = clientFeeGroupRepository.GetGroup(id);
             clientFeeGroupMerchantFeeVM.ClientFeeGroup = clientFeeGroup;

             return View(clientFeeGroupMerchantFeeVM);
         }

        // GET: Delete A MerchantFeeClientFeeGroup
		[HttpGet]
		public ActionResult Delete(int id, int mId)
         {
             //Get Item From Database
             MerchantFeeClientFeeGroup merchantFeeClientFeeGroup = new MerchantFeeClientFeeGroup();
             merchantFeeClientFeeGroup = merchantFeeClientFeeGroupRepository.GetItem(id, mId);

             //Check Exists
             if (merchantFeeClientFeeGroup == null)
             {
                 ViewData["ActionMethod"] = "DeleteGet";
                 return View("RecordDoesNotExistError");
             }

             ClientFeeGroupMerchantFeeVM clientFeeGroupMerchantFeeVM = new ClientFeeGroupMerchantFeeVM();
             clientFeeGroupMerchantFeeVM.MerchantFeeClientFeeGroup = merchantFeeClientFeeGroup;

             MerchantFee merchantFee = new MerchantFee();
             merchantFee = merchantFeeRepository.GetItem(mId);
             merchantFeeRepository.EditForDisplay(merchantFee);
             clientFeeGroupMerchantFeeVM.MerchantFee = merchantFee;

             ClientFeeGroup clientFeeGroup = new ClientFeeGroup();
             clientFeeGroup = clientFeeGroupRepository.GetGroup(id);
             clientFeeGroupMerchantFeeVM.ClientFeeGroup = clientFeeGroup;

             return View(clientFeeGroupMerchantFeeVM);
         }

        // POST: Delete A MerchantFeeClientFeeGroup
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(ClientFeeGroupMerchantFeeVM clientFeeGroupMerchantFeeVM, FormCollection collection)
        {
            //Check Access Rights to Domain
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Get Item From Database
            int clientFeeGroupId = clientFeeGroupMerchantFeeVM.MerchantFeeClientFeeGroup.ClientFeeGroupId;
            int merchantFeeId = clientFeeGroupMerchantFeeVM.MerchantFeeClientFeeGroup.MerchantFeeId;
            
             MerchantFeeClientFeeGroup merchantFeeClientFeeGroup = new MerchantFeeClientFeeGroup();
             merchantFeeClientFeeGroup = merchantFeeClientFeeGroupRepository.GetItem(clientFeeGroupId, merchantFeeId);

             //Check Exists
             if (merchantFeeClientFeeGroup == null)
             {
                 ViewData["ActionMethod"] = "DeletePost";
                 return View("RecordDoesNotExistError");
             }

            //Delete Item
            try
            {
                merchantFeeClientFeeGroup.VersionNumber = Int32.Parse(collection["MerchantFeeClientFeeGroup.VersionNumber"]);
                merchantFeeClientFeeGroupRepository.Delete(merchantFeeClientFeeGroup);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/MerchantFeeClientFeeGroup.mvc/Delete/" + clientFeeGroupId.ToString() + "?mid=" + merchantFeeId.ToString();
                    return View("VersionError");
                }
                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            //Return
            return RedirectToAction("List", new { id = clientFeeGroupId });
        }
    }
}
