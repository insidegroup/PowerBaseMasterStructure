using CWTDesktopDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.Repository
{
	public class ClientSubUnitLineOfBusinessRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		public ClientSubUnitLineOfBusiness GetClientSubUnitLineOfBusiness(string clientSubUnitGuid)
        {
			return db.ClientSubUnitLineOfBusinesses.Where(x => x.ClientSubUnitGuid == clientSubUnitGuid).SingleOrDefault();
        }
    }
}
