using CWTDesktopDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.Repository
{
	public class BackOfficeSystemRepository
	{
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        public BackOfficeSystem GetBackOfficeSystem(int id)
        {
            return db.BackOfficeSystems.Where(c => c.BackOfficeSytemId == id).SingleOrDefault();
        }

        public List<BackOfficeSystem> GetAllBackOfficeSystems()
		{
			var result = from n in db.BackOfficeSystems select n;
			return result.ToList();
		}

        public List<BackOfficeSystem> GetBackOfficeSystemsExceptAll()
		{
			var result = from n in db.BackOfficeSystems.Where(x => x.BackOfficeSystemDescription != "All") select n;
			return result.ToList();
		}
	}
}