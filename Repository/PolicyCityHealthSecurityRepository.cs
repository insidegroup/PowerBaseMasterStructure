using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Repository
{
	public class PolicyCityHealthSecurityRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//Get PolicyCityHealthSecurityItemsByCityCode
		public List<PolicyCityHealthSecurity> GetPolicyCityHealthSecurityItemsByCityCode(string cityCode)
        {
			return db.PolicyCityHealthSecurities.Where(c => c.CityCode == cityCode).ToList();
        }

    }
}
