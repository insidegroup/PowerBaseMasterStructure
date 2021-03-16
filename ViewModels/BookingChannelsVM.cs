using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
	public class BookingChannelsVM : CWTBaseViewModel
	{ 
        public ClientSubUnit ClientSubUnit { get; set; }
		public CWTPaginatedList<spDesktopDataAdmin_SelectClientSubUnitBookingChannels_v1Result> BookingChannels { get; set; }

        public BookingChannelsVM()
        {
        }

		public BookingChannelsVM(
			ClientSubUnit clientSubUnit, CWTPaginatedList<spDesktopDataAdmin_SelectClientSubUnitBookingChannels_v1Result> bookingChannels
			)
        {
            ClientSubUnit = clientSubUnit;
			BookingChannels = bookingChannels;
       }
	}
}