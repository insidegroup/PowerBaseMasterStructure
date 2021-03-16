using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Repository;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using System.Data.SqlClient;
using System.Web.Script.Serialization;

namespace CWTDesktopDatabase.Controllers
{
    public class TransactionFeeClientFeeGroupController : Controller
    {
        HierarchyRepository hierarchyRepository = new HierarchyRepository();
        TransactionFeeClientFeeGroupRepository transactionFeeClientFeeGroupRepository = new TransactionFeeClientFeeGroupRepository();
        TransactionFeeRepository transactionFeeRepository = new TransactionFeeRepository();
        TransactionFeeAirRepository transactionFeeAirRepository = new TransactionFeeAirRepository();
        TransactionFeeCarHotelRepository transactionFeeCarHotelRepository = new TransactionFeeCarHotelRepository();

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
            if (sortField != "ProductName" && sortField != "TravelIndicator" && sortField != "BookingOriginationCode" && sortField != "ChargeTypeCode" && sortField != "FeeCategory" && sortField != "FeeAmount" && sortField != "TransactionTypeCode")
            {
                sortField = "TransactionFeeDescription";
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

            ClientFeeGroupTransactionFeesVM clientFeeGroupTransactionFeesVM = new ClientFeeGroupTransactionFeesVM();
            clientFeeGroupTransactionFeesVM.ClientFeeGroup = clientFeeGroup;

            //Check Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                clientFeeGroupTransactionFeesVM.HasDomainWriteAccess = true;
            }

            clientFeeGroupTransactionFeesVM.ClientFeeGroupTransactionFees = transactionFeeClientFeeGroupRepository.PageTransactionFeeClientFeeGroups(id, page ?? 1, filter ?? "", sortField, sortOrder ?? 0);
            return View(clientFeeGroupTransactionFeesVM);
        }

        // GET: /Create
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

            ClientFeeGroupTransactionFeeVM clientFeeGroupTransactionFeesVM = new ClientFeeGroupTransactionFeeVM();
            clientFeeGroupTransactionFeesVM.ClientFeeGroup = clientFeeGroup;

            TransactionFeeClientFeeGroup transactionFeeClientFeeGroup = new TransactionFeeClientFeeGroup();
            transactionFeeClientFeeGroup.ClientFeeGroupId = id;
            clientFeeGroupTransactionFeesVM.TransactionFeeClientFeeGroup = transactionFeeClientFeeGroup;


            //TransactionFeeRepository transactionFeeRepository = new TransactionFeeRepository();
            //SelectList transactionFeeList = new SelectList(transactionFeeRepository.GetAllItems().ToList(), "TransactionFeeId", "TransactionFeeDescription");
            SelectList transactionFeeList = new SelectList(transactionFeeClientFeeGroupRepository.GetUnUsedTransactionFees(id, 0, 0).ToList(), "TransactionFeeId", "TransactionFeeDescription");
            clientFeeGroupTransactionFeesVM.TransactionFees = transactionFeeList;

            ProductRepository productRepository = new ProductRepository();
            SelectList productList = new SelectList(productRepository.GetTransactionFeeProducts().ToList(), "ProductId", "ProductName");
            clientFeeGroupTransactionFeesVM.Products = productList;


            return View(clientFeeGroupTransactionFeesVM);
        }

        // POST: Create a TransactionFeeClientFeeGroup
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TransactionFeeClientFeeGroup transactionFeeClientFeeGroup)
        {
            //Check Access Rights to Domain
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
           
            //Get ClientFeeGroup
            ClientFeeGroup clientFeeGroup = new ClientFeeGroup();
            clientFeeGroup = clientFeeGroupRepository.GetGroup(transactionFeeClientFeeGroup.ClientFeeGroupId);

            //Check Exists
            if (clientFeeGroup == null)
            {
                ViewData["ActionMethod"] = "CreateGet1";
                return View("RecordDoesNotExistError");
            }
            //Get TransactionFee
            TransactionFee transactionFee = new TransactionFee();
            int productId = transactionFeeClientFeeGroup.ProductId;
            if (productId == 1)
            {
                transactionFee = transactionFeeAirRepository.GetItem(transactionFeeClientFeeGroup.TransactionFeeId);
            }
            else
            {
                transactionFee = transactionFeeCarHotelRepository.GetItem(transactionFeeClientFeeGroup.TransactionFeeId);
            }
            

            //Check Exists
            if (transactionFee == null)
            {
                ViewData["ActionMethod"] = "CreateGet2";
                return View("RecordDoesNotExistError");
            }


            //Database Update
            try
            {
                transactionFeeClientFeeGroupRepository.Add(transactionFeeClientFeeGroup);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List", new { id = transactionFeeClientFeeGroup.ClientFeeGroupId });
        }

        // GET: Edit A TransactionFeeClientFeeGroup
        public ActionResult Edit(int cid, int tId)
        {
            //Get Item From Database
            TransactionFeeClientFeeGroup transactionFeeClientFeeGroup = new TransactionFeeClientFeeGroup();
            transactionFeeClientFeeGroup = transactionFeeClientFeeGroupRepository.GetItem(cid, tId);

            //Check Exists
            if (transactionFeeClientFeeGroup == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            ClientFeeGroupTransactionFeeVM clientFeeGroupTransactionFeeVM = new ClientFeeGroupTransactionFeeVM();
            clientFeeGroupTransactionFeeVM.TransactionFeeClientFeeGroup = transactionFeeClientFeeGroup;

            //Get ClientFeeGroup
            ClientFeeGroup clientFeeGroup = new ClientFeeGroup();
            clientFeeGroup = clientFeeGroupRepository.GetGroup(cid);
            clientFeeGroupTransactionFeeVM.ClientFeeGroup = clientFeeGroup;
            clientFeeGroupTransactionFeeVM.OriginalTransactionFeeId = tId;

            TransactionFee transactionFee = new TransactionFee();
            transactionFee = transactionFeeRepository.GetItem(tId);

            int productId = (int)transactionFee.ProductId;
            transactionFeeClientFeeGroup.ProductId = productId;



            SelectList transactionFeeList = new SelectList(transactionFeeClientFeeGroupRepository.GetUnUsedTransactionFees(cid, productId, transactionFee.TransactionFeeId).ToList(), "TransactionFeeId", "TransactionFeeDescription");
            clientFeeGroupTransactionFeeVM.TransactionFees = transactionFeeList;

            ProductRepository productRepository = new ProductRepository();
            SelectList productList = new SelectList(productRepository.GetTransactionFeeProducts().ToList(), "ProductId", "ProductName", productId);
            clientFeeGroupTransactionFeeVM.Products = productList;

            return View(clientFeeGroupTransactionFeeVM);
        }

        // POST: Edit a TransactionFeeClientFeeGroup
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ClientFeeGroupTransactionFeeVM clientFeeGroupTransactionFeeVM)
        {
            //Check Access Rights to Domain
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Get ClientFeeGroup
            TransactionFeeClientFeeGroup transactionFeeClientFeeGroup = new TransactionFeeClientFeeGroup();
            transactionFeeClientFeeGroup = clientFeeGroupTransactionFeeVM.TransactionFeeClientFeeGroup;

            ClientFeeGroup clientFeeGroup = new ClientFeeGroup();
            clientFeeGroup = clientFeeGroupRepository.GetGroup(transactionFeeClientFeeGroup.ClientFeeGroupId);

            //Check Exists
            if (clientFeeGroup == null)
            {
                ViewData["ActionMethod"] = "EditGet1";
                return View("RecordDoesNotExistError");
            }
            //Get TransactionFee
            TransactionFee transactionFee = new TransactionFee();
            transactionFee = transactionFeeRepository.GetItem(transactionFeeClientFeeGroup.TransactionFeeId);

            //Check Exists
            if (transactionFee == null)
            {
                ViewData["ActionMethod"] = "EditGet2";
                return View("RecordDoesNotExistError");
            }


            //Database Update
            try
            {
                transactionFeeClientFeeGroupRepository.Update(transactionFeeClientFeeGroup, clientFeeGroupTransactionFeeVM.OriginalTransactionFeeId);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List", new { id = transactionFeeClientFeeGroup.ClientFeeGroupId });
        }
        
		// POST:  TransactionFees available for a TransactionFeeClientFeeGroup
		// Data returned on live as JSON string caused an error:
		/*
		 * Error during serialization or deserialization using the JSON JavaScriptSerializer. 
		 * The length of the string exceeds the value set on the maxJsonLength property
		 */

		//[HttpPost]
		//public JsonResult GetUnUsedTransactionFees(int clientFeeGroupId, int productid, int? transactionFeeId)
		//{
		//	var result = transactionFeeClientFeeGroupRepository.GetUnUsedTransactionFees(clientFeeGroupId, productid, transactionFeeId);
		//	return Json(result);
		//}

		[HttpPost]
		public ContentResult GetUnUsedTransactionFees(int clientFeeGroupId, int productid, int? transactionFeeId)
        {
			//Get a list of Unused Transaction Fees
			List<TransactionFee> transactionFeeList = transactionFeeClientFeeGroupRepository.GetUnUsedTransactionFees(clientFeeGroupId, productid, transactionFeeId);

			//Using TransactionFeeJson which just has an Id and Description to reduce Json Output
			List<TransactionFeeJson> transactionFeeJson = new List<TransactionFeeJson>();

			//Loop though the list of TransactionFees and populate new list of TransactionFeeJson
			foreach (TransactionFee value in transactionFeeList)
			{
				transactionFeeJson.Add(new TransactionFeeJson()
				{
					TransactionFeeId = value.TransactionFeeId,
					TransactionFeeDescription = value.TransactionFeeDescription
				});
			}

			//Setting the max value will allow more data to be returned by the application
			var serializer = new JavaScriptSerializer()
			{
				MaxJsonLength = int.MaxValue
			};

			//Return the serialized list as Json
			var result = new ContentResult()
			{
				Content = serializer.Serialize(transactionFeeJson),
				ContentType = "application/json"
			};
			
			return result;
        }
    }
}
