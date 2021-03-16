using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientSubUnitTravelerTypeSupplierProductVM : CWTBaseViewModel
    {
        public ClientSubUnit ClientSubUnit { get; set; }
        public TravelerType TravelerType { get; set; }
        public ClientDetail ClientDetail { get; set; }
        public ClientDetailSupplierProduct SupplierProduct { get; set; }
        public IEnumerable<SelectListItem> Products { get; set; }
        public IEnumerable<SelectListItem> Suppliers { get; set; }

        public ClientSubUnitTravelerTypeSupplierProductVM()
        {
        }
        public ClientSubUnitTravelerTypeSupplierProductVM(ClientSubUnit clientSubUnit, TravelerType travelerType, ClientDetail clientDetail, ClientDetailSupplierProduct supplierProduct, IEnumerable<SelectListItem> products, IEnumerable<SelectListItem> suppliers)
        {
            ClientSubUnit = clientSubUnit;
            TravelerType = travelerType;
            ClientDetail = clientDetail;
            SupplierProduct = supplierProduct;
            Products = products;
            Suppliers = suppliers;
        }
    }
}