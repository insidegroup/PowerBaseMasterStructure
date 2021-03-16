using System.Collections.Generic;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
	public class PriceTrackingContactVM : CWTBaseViewModel
	{
		public PriceTrackingContact PriceTrackingContact { get; set; }

		public IEnumerable<SelectListItem> ContactTypes { get; set; }
		public IEnumerable<SelectListItem> PriceTrackingContactUserTypes { get; set; }
        public IEnumerable<SelectListItem> PriceTrackingDashboardAccessTypes { get; set; }
        public IEnumerable<SelectListItem> PriceTrackingEmailAlertTypes { get; set; }

        public PriceTrackingContactVM()
		{
		}

		public PriceTrackingContactVM(PriceTrackingContact priceTrackingContact)
		{
			PriceTrackingContact = priceTrackingContact;
		}
	}
}