using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Web.Mvc;

namespace CWTDesktopDatabase.ViewModels
{
	public class AdditionalBookingCommentsVM : CWTBaseViewModel
	{ 
        public ClientSubUnit ClientSubUnit { get; set; }
		public CWTPaginatedList<spDesktopDataAdmin_SelectAdditionalBookingComments_v1Result> AdditionalBookingComments { get; set; }
		public BookingChannel BookingChannel {get; set;}

        public AdditionalBookingCommentsVM()
        {
        }

		public AdditionalBookingCommentsVM(
			ClientSubUnit clientSubUnit,
			CWTPaginatedList<spDesktopDataAdmin_SelectAdditionalBookingComments_v1Result> additionalBookingComments,
			BookingChannel bookingChannel
			)
        {
            ClientSubUnit = clientSubUnit;
			AdditionalBookingComments = additionalBookingComments;
			BookingChannel = bookingChannel;
       }
	}
}