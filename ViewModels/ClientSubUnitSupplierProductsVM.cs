using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientSubUnitSupplierProductsVM : CWTBaseViewModel
    {
        public ClientSubUnit ClientSubUnit { get; set; }
        public ClientDetail ClientDetail { get; set; }
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientDetailSupplierProducts_v1Result> SupplierProducts { get; set; }
        
        public ClientSubUnitSupplierProductsVM()
        {
        }
        public ClientSubUnitSupplierProductsVM(ClientSubUnit clientSubUnit, ClientDetail clientDetail, CWTPaginatedList<spDesktopDataAdmin_SelectClientDetailSupplierProducts_v1Result> supplierProducts)
        {
            ClientSubUnit = clientSubUnit;
            ClientDetail = clientDetail;
            SupplierProducts = supplierProducts;
        }
    }
}