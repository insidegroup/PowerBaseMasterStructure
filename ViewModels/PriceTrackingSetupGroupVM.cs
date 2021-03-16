using System.Collections.Generic;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
	public class PriceTrackingSetupGroupVM : CWTBaseViewModel
	{
		public PriceTrackingSetupGroup PriceTrackingSetupGroup { get; set; }

        public PriceTrackingSetupGroupVM()
		{
		}

		public PriceTrackingSetupGroupVM(PriceTrackingSetupGroup priceTrackingSetupGroup)
		{
			PriceTrackingSetupGroup = priceTrackingSetupGroup;
		}
	}
}