using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class TravelerTypeSupplierProductsVM : CWTBaseViewModel
    {
        public ClientSubUnit ClientSubUnit { get; set; }
        public TravelerType TravelerType { get; set; }
        public ClientDetail ClientDetail { get; set; }
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientDetailSupplierProducts_v1Result> SupplierProducts { get; set; }
        
        public TravelerTypeSupplierProductsVM()
        {
        }
        public TravelerTypeSupplierProductsVM(ClientSubUnit clientSubUnit, TravelerType travelerType, ClientDetail clientDetail, CWTPaginatedList<spDesktopDataAdmin_SelectClientDetailSupplierProducts_v1Result> supplierProducts)
        {

            ClientSubUnit = clientSubUnit;
            TravelerType = travelerType;
            ClientDetail = clientDetail;
            SupplierProducts = supplierProducts;
        }
    }
}