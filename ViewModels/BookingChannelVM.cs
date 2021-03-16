using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Web.Mvc;

namespace CWTDesktopDatabase.ViewModels
{
	[Bind(Exclude = "GDSList")]
	public class BookingChannelVM : CWTBaseViewModel
	{
		public BookingChannel BookingChannel { get; set; }
		
		public ClientSubUnit ClientSubUnit { get; set; }
		
		public IEnumerable<SelectListItem> UsageTypes { get; set; }
		public IEnumerable<SelectListItem> BookingChannelTypes { get; set; }
		public IEnumerable<SelectListItem> ProductChannelTypes { get; set; }
		public IEnumerable<SelectListItem> DesktopUsedTypes { get; set; }
		public IEnumerable<SelectListItem> Products { get; set; }
		public IEnumerable<SelectListItem> GDSList { get; set; }
		public GDS GDS { get; set; }

		public List<string> ContentBookedItems { get; set; }
		public IEnumerable<SelectListItem> ContentBookedItemsSelected { get; set; }
		public string ContentBookedItemsList { get; set; }

        public BookingChannelVM()
        {
        }

		public BookingChannelVM(
			BookingChannel bookingChannel,
			ClientSubUnit clientSubUnit,
			IEnumerable<SelectListItem> bookingTypes
			)
        {
			BookingChannel = bookingChannel;
			ClientSubUnit = clientSubUnit;
			UsageTypes = bookingTypes;
       }
	}
}