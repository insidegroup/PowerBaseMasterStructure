using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Repository;
using CWTDesktopDatabase.ViewModels;
using System.Data.SqlClient;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Controllers
{
    public class TransactionFeeController : Controller
    {
        HierarchyRepository hierarchyRepository = new HierarchyRepository();
        TransactionFeeRepository transactionFeeRepository = new TransactionFeeRepository();
        private string groupName = "ClientFee";

        public ActionResult List(string filter, int? page, string sortField, int? sortOrder)
        {

            //SortField + SortOrder settings
            if (sortField != "TravelIndicator" && sortField != "BookingSourceCode" && sortField != "BookingOriginationCode" && sortField != "FeeAmount" && sortField != "FeePercent" && sortField != "FeeCurrencyCode")
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

            TransactionFeesVM transactionFeesVM = new TransactionFeesVM();

            //Check Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                transactionFeesVM.HasWriteAccess = true;
            }

            transactionFeesVM.TransactionFees = transactionFeeRepository.PageTransactionFees(page ?? 1, filter ?? "", sortField, sortOrder ?? 0);
            return View(transactionFeesVM);
        }

        public ActionResult SelectProduct()
        {
            //Check Access Rights to Domain
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            TransactionFeeProductSelectListVM transactionFeeProductSelectListVM = new TransactionFeeProductSelectListVM();

            ProductRepository productRepository = new ProductRepository();
            transactionFeeProductSelectListVM.Products = new SelectList(productRepository.GetTransactionFeeProducts().ToList(), "ProductId", "ProductName");

            return View(transactionFeeProductSelectListVM);
        }
        // POST:
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelectProduct(int productId)
        {
            ProductRepository productRepository = new ProductRepository();
            Product product = new Product();
            product = productRepository.GetProduct(productId);
            if (product == null || productId < 1 || productId > 3)
            {
                ViewData["ActionMethod"] = "SelectTypePost";
                return View("RecordDoesNotExistError");
            }

            if (productId == 1)
            {
                return RedirectToAction("Create", "TransactionFeeAir");
            }
            else
            {
                return RedirectToAction("Create", "TransactionFeeCarHotel", new { productId = productId });
            }
        }
    }
}
