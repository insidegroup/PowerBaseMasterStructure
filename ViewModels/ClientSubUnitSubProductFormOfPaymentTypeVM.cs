using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientSubUnitSubProductFormOfPaymentTypeVM : CWTBaseViewModel
    {
        public ClientSubUnit ClientSubUnit { get; set; }
        public ClientDetail ClientDetail { get; set; }
        public ClientDetailSubProductFormOfPaymentType ClientDetailSubProductFormOfPaymentType { get; set; }
        public IEnumerable<SelectListItem> FormOfPaymentTypes { get; set; }
        public IEnumerable<SelectListItem> SubProducts { get; set; }

        public ClientSubUnitSubProductFormOfPaymentTypeVM()
        {
        }
        public ClientSubUnitSubProductFormOfPaymentTypeVM(
                            ClientSubUnit clientSubUnit, 
                            ClientDetail clientDetail,
                            ClientDetailSubProductFormOfPaymentType clientDetailSubProductFormOfPaymentType,
                            IEnumerable<SelectListItem> subProducts,
                            IEnumerable<SelectListItem> formOfPaymentTypes)
        {
            ClientSubUnit = clientSubUnit;
            ClientDetail = clientDetail;
            ClientDetailSubProductFormOfPaymentType = clientDetailSubProductFormOfPaymentType;
            FormOfPaymentTypes = formOfPaymentTypes;
            SubProducts = subProducts;
        }
    }
}