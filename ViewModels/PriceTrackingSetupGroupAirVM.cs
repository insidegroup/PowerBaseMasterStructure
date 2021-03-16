using System.Collections.Generic;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
	public class PriceTrackingSetupGroupItemAirVM : CWTBaseViewModel
	{
		public PriceTrackingSetupGroupItemAir PriceTrackingSetupGroupItemAir { get; set; }

        public PriceTrackingSetupGroupItemAirVM()
		{
		}

		public PriceTrackingSetupGroupItemAirVM(PriceTrackingSetupGroupItemAir priceTrackingSetupGroupItemAir)
		{
			PriceTrackingSetupGroupItemAir = priceTrackingSetupGroupItemAir;
		}
	}
}