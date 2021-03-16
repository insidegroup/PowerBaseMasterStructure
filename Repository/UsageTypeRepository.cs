using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.Repository
{
	public class UsageTypeRepository
	{
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		public UsageType GetUsageType(int id)
		{
			return db.UsageTypes.Where(c => c.UsageTypeId == id).SingleOrDefault();
		}

		public IQueryable<UsageType> GetAllUsageTypes()
		{
			return db.UsageTypes.OrderBy(c => c.UsageTypeDescription);
		}

		public List<UsageType> GetAvailableUsageTypes(string clientSubUnitGuid, int bookingTypeId = 0)
		{
			List<UsageType> result = (from n in db.spDesktopDataAdmin_SelectUsageTypeAvailableUsageTypes_v1(clientSubUnitGuid)
										 select new UsageType
										{
											UsageTypeId = n.UsageTypeId,
											UsageTypeDescription = n.UsageTypeDescription
										}).ToList();
			
			//Add selected item into list
			if (bookingTypeId > 0)
			{
				UsageType selectedItem = GetUsageType(bookingTypeId);
				if (selectedItem != null)
				{
					result.Add(selectedItem);
				}
			}

			return result.OrderBy(x => x.UsageTypeDescription).ToList();
		}
	}
}