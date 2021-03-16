using System.Collections.Generic;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
	public class PriceTrackingSetupGroupItemHotelVM : CWTBaseViewModel
	{
		public PriceTrackingSetupGroupItemHotel PriceTrackingSetupGroupItemHotel { get; set; }

        public PriceTrackingSetupGroupItemHotelVM()
		{
		}

		public PriceTrackingSetupGroupItemHotelVM(PriceTrackingSetupGroupItemHotel priceTrackingSetupGroupItemHotel)
		{
			PriceTrackingSetupGroupItemHotel = priceTrackingSetupGroupItemHotel;
		}
	}
}