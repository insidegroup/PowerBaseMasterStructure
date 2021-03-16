using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientTopUnitSupplierProductVM : CWTBaseViewModel
   {
        public ClientTopUnit ClientTopUnit { get; set; }
        public ClientDetail ClientDetail { get; set; }
        public ClientDetailSupplierProduct SupplierProduct { get; set; }
        public IEnumerable<SelectListItem> Products { get; set; }
        public IEnumerable<SelectListItem> Suppliers { get; set; }

        public ClientTopUnitSupplierProductVM()
        {
        }
        public ClientTopUnitSupplierProductVM(ClientTopUnit clientTopUnit, ClientDetail clientDetail, ClientDetailSupplierProduct supplierProduct, Address address, IEnumerable<SelectListItem> products, IEnumerable<SelectListItem> suppliers)
        {
            ClientTopUnit = clientTopUnit;
            ClientDetail = clientDetail;
            SupplierProduct = supplierProduct;
            Products = products;
            Suppliers = suppliers;
        }
    }
}