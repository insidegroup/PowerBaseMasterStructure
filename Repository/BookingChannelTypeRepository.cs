using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.Repository
{
	public class BookingChannelTypeRepository
	{
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		public BookingChannelType GetBookingChannelType(int id)
		{
			return db.BookingChannelTypes.Where(c => c.BookingChannelTypeId == id).SingleOrDefault();
		}

        public IQueryable<BookingChannelType> GetAllBookingChannelTypes()
        {
            return db.BookingChannelTypes.OrderBy(c => c.BookingChannelTypeDescription);
        }

        public BookingChannelType GetBookingChannelTypeByDescription(string bookingChannelTypeDescription)
        {
            return db.BookingChannelTypes.Where(c => c.BookingChannelTypeDescription == bookingChannelTypeDescription).SingleOrDefault();
        }
    }
}