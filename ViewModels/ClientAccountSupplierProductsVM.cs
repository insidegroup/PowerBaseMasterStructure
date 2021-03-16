using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientAccountSupplierProductsVM : CWTBaseViewModel
    {
        public ClientAccount ClientAccount { get; set; }
        public ClientDetail ClientDetail { get; set; }
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientDetailSupplierProducts_v1Result> SupplierProducts { get; set; }
        
        public ClientAccountSupplierProductsVM()
        {
        }
        public ClientAccountSupplierProductsVM(ClientAccount clientAccount, ClientDetail clientDetail, CWTPaginatedList<spDesktopDataAdmin_SelectClientDetailSupplierProducts_v1Result> supplierProducts)
        {
            ClientAccount = clientAccount;
            ClientDetail = clientDetail;
            SupplierProducts = supplierProducts;
        }
    }
}