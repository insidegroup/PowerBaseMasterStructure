using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.Repository
{
	public class TelephonyTypeRepository
	{
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		public TelephonyType GetTelephonyType(int id)
		{
			return db.TelephonyTypes.Where(c => c.TelephonyTypeId == id).SingleOrDefault();
		}

		public IQueryable<TelephonyType> GetAllTelephonyTypes()
		{
			return db.TelephonyTypes.OrderBy(c => c.TelephonyTypeDescription);
		}
	}
}