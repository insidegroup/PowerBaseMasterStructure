using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientTopUnitSupplierProductsVM : CWTBaseViewModel
    {
        public ClientTopUnit ClientTopUnit { get; set; }
        public ClientDetail ClientDetail { get; set; }
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientDetailSupplierProducts_v1Result> SupplierProducts { get; set; }
        
        public ClientTopUnitSupplierProductsVM()
        {
        }
        public ClientTopUnitSupplierProductsVM(ClientTopUnit clientTopUnit, ClientDetail clientDetail, CWTPaginatedList<spDesktopDataAdmin_SelectClientDetailSupplierProducts_v1Result> supplierProducts)
        {
            ClientTopUnit = clientTopUnit;
            ClientDetail = clientDetail;
            SupplierProducts = supplierProducts;
        }
    }
}