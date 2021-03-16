using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.Repository
{
	public class ContentBookedItemRepository
	{
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		public ContentBookedItem GetContentBookedItem(int id)
		{
			return db.ContentBookedItems.Where(c => c.ContentBookedItemId == id).SingleOrDefault();
		}

		public IQueryable<ContentBookedItem> GetBookingChannelContentBookedItems(int bookingChannelId)
		{
			return db.ContentBookedItems.Where(c => c.BookingChannelId == bookingChannelId).OrderBy(c => c.ContentBookedItemId);
		}
	}
}