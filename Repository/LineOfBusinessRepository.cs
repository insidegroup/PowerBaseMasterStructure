using CWTDesktopDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.Repository
{
	public class LineOfBusinessRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		public LineOfBusiness GetLineOfBusiness(int? lineOfBusinessId)
        {
			return db.LineOfBusinesses.SingleOrDefault(c => c.LineofBusinessId == lineOfBusinessId);
        }

		public IQueryable<LineOfBusiness> GetAllLineOfBusinesses()
        {
			return db.LineOfBusinesses.OrderBy(c => c.LineofBusinessDescription);
        }
    }
}
