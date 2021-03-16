using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Repository;

namespace CWTDesktopDatabase.Controllers
{
    public class ProductController : Controller
    {
        // POST: AutoComplete Suppliers of a Product
        [HttpPost]
        public JsonResult AutoCompleteProductSuppliers(string searchText, int productId)
        {
            SupplierRepository supplierRepository = new SupplierRepository();
            var result = supplierRepository.LookUpProductSuppliers(searchText, productId);
            return Json(result);
        }

       
    }
}
