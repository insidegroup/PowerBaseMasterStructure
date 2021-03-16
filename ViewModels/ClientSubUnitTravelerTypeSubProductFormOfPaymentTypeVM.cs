using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientSubUnitTravelerTypeSubProductFormOfPaymentTypeVM : CWTBaseViewModel
    {
        public ClientSubUnit ClientSubUnit { get; set; }
        public TravelerType TravelerType { get; set; }
        public ClientDetail ClientDetail { get; set; }
        public ClientDetailSubProductFormOfPaymentType ClientDetailSubProductFormOfPaymentType { get; set; }
        public IEnumerable<SelectListItem> FormOfPaymentTypes { get; set; }
        public IEnumerable<SelectListItem> SubProducts { get; set; }

        public ClientSubUnitTravelerTypeSubProductFormOfPaymentTypeVM()
        {
        }
        public ClientSubUnitTravelerTypeSubProductFormOfPaymentTypeVM(
                            ClientSubUnit clientSubUnit, 
                            TravelerType travelerType, 
                            ClientDetail clientDetail,
                            ClientDetailSubProductFormOfPaymentType clientDetailSubProductFormOfPaymentType,
                            IEnumerable<SelectListItem> subProducts,
                            IEnumerable<SelectListItem> formOfPaymentTypes)
        {
            ClientSubUnit = clientSubUnit;
            TravelerType = travelerType;
            ClientDetail = clientDetail;
            ClientDetailSubProductFormOfPaymentType = clientDetailSubProductFormOfPaymentType;
            FormOfPaymentTypes = formOfPaymentTypes;
            SubProducts = subProducts;
        }
    }
}