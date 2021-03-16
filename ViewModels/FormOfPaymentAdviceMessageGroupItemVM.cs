using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class FormOfPaymentAdviceMessageGroupItemVM : CWTBaseViewModel
    {
		public FormOfPaymentAdviceMessageGroup FormOfPaymentAdviceMessageGroup { get; set; }
		public FormOfPaymentAdviceMessageGroupItem FormOfPaymentAdviceMessageGroupItem { get; set; }

        public IEnumerable<SelectListItem> Countries { get; set; }
        public IEnumerable<SelectListItem> Products { get; set; }
		public IEnumerable<SelectListItem> TravelIndicators { get; set; }
		public IEnumerable<SelectListItem> FormOfPaymentTypes { get; set; }
		
        public FormOfPaymentAdviceMessageGroupItemVM()
        {
        }
        public FormOfPaymentAdviceMessageGroupItemVM(
			FormOfPaymentAdviceMessageGroupItem formOfPaymentAdviceMessageGroupItem, 
			IEnumerable<SelectListItem> countries,
			IEnumerable<SelectListItem> products,
			IEnumerable<SelectListItem> travelIndicators,
			IEnumerable<SelectListItem> formOfPaymentTypes)
        {
            FormOfPaymentAdviceMessageGroupItem = formOfPaymentAdviceMessageGroupItem;
            Countries = countries;
			Products = products;
			TravelIndicators = travelIndicators;
			FormOfPaymentTypes = formOfPaymentTypes;
        }
    }
}