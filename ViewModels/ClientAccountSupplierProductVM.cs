using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientAccountSupplierProductVM : CWTBaseViewModel
    {
        public ClientAccount ClientAccount { get; set; }
        public ClientDetail ClientDetail { get; set; }
        public ClientDetailSupplierProduct SupplierProduct { get; set; }
        public IEnumerable<SelectListItem> Products { get; set; }
        public IEnumerable<SelectListItem> Suppliers { get; set; }

        public ClientAccountSupplierProductVM()
        {
        }
        public ClientAccountSupplierProductVM(ClientAccount clientAccount, ClientDetail clientDetail, ClientDetailSupplierProduct supplierProduct , IEnumerable<SelectListItem> products, IEnumerable<SelectListItem> suppliers)
        {
            ClientAccount = clientAccount;
            ClientDetail = clientDetail;
            SupplierProduct = supplierProduct;
            Products = products;
            Suppliers = suppliers;
        }
    }
}