using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.Repository
{
    public class ServiceFundRoutingRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		public Dictionary<string, string> GetAllServiceFundRoutings()
        {
			Dictionary<string, string> serviceFundRoutings = new Dictionary<string, string>();
			serviceFundRoutings.Add("D", "D");
			serviceFundRoutings.Add("DI", "DI");
			serviceFundRoutings.Add("I", "I");
			return serviceFundRoutings;
        }
    }
}
