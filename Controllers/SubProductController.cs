using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Repository;
namespace CWTDesktopDatabase.Controllers
{
    public class SubProductController : Controller
    {
        SubProductRepository subProductRepository = new SubProductRepository();

        // POST: AutoComplete Suppliers
        [HttpPost]
        public JsonResult AutoCompleteSubProducts(string searchText, int maxResults)
        {
            
            var result = subProductRepository.LookUpSubProducts(searchText, maxResults);
            return Json(result);
        }
        [HttpPost]
        public JsonResult IsValidSubProduct(string searchText)
        {

            var result = subProductRepository.GetSubProductByName(searchText);
            return Json(result);
        }

        // POST: List SubProducts of a Product
        [HttpPost]
        public JsonResult ProductSubProducts(int productId)
        {
            SubProductRepository subProductRepository = new SubProductRepository();
            var result = subProductRepository.ListProductSubProducts(productId);
            return Json(result);
        }
    }
}
