using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.Repository
{
	public class PseudoCityOrOfficeBusinessRepository
	{
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		public PseudoCityOrOfficeBusiness GetPseudoCityOrOfficeBusiness(int id)
		{
			return db.PseudoCityOrOfficeBusinesses.Where(c => c.PseudoCityOrOfficeBusinessId == id).SingleOrDefault();
		}

		public IQueryable<PseudoCityOrOfficeBusiness> GetAllPseudoCityOrOfficeBusinesses()
		{
			return db.PseudoCityOrOfficeBusinesses.OrderBy(c => c.PseudoCityOrOfficeBusinessName);
		}
	}
}