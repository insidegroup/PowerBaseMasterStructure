using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.Repository
{
	public class ProductChannelTypeRepository
	{
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		public ProductChannelType GetProductChannelType(int id)
		{
			return db.ProductChannelTypes.Where(c => c.ProductChannelTypeId == id).SingleOrDefault();
		}

        public ProductChannelType GetProductChannelTypeByDescription(string productChannelTypeDescription)
        {
            return db.ProductChannelTypes.Where(c => c.ProductChannelTypeDescription == productChannelTypeDescription).SingleOrDefault();
        }

        public ProductChannelType GetProductChannelTypeBookingChannelType(int bookingChannelTypeId, int productChannelTypeId)
        {
            return db.ProductChannelTypes.Where(c => c.BookingChannelTypeId == bookingChannelTypeId && c.ProductChannelTypeId == productChannelTypeId).SingleOrDefault();
        }

        public IQueryable<ProductChannelType> GetAllProductChannelTypes()
		{
			return db.ProductChannelTypes.OrderBy(c => c.ProductChannelTypeDescription);
		}

		public List<ProductChannelType> GetProductChannelTypesForBookingChannel(int bookingChannelTypeId)
		{
			IEnumerable<ProductChannelType> productChannelTypes = db.ProductChannelTypes
														.Where(x => x.BookingChannelTypeId == bookingChannelTypeId)
														.OrderBy(c => c.ProductChannelTypeDescription);
			
			var result = productChannelTypes.Select(
				x => new ProductChannelType { 
					ProductChannelTypeId = x.ProductChannelTypeId, 
					ProductChannelTypeDescription = x.ProductChannelTypeDescription 
				}
			);

			return result.ToList();
		}
	}
}