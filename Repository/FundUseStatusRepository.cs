using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.Repository
{
    public class FundUseStatusRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		public Dictionary<string, string> GetAllFundUseStatuses()
        {
			Dictionary<string, string> fundUseStatuses = new Dictionary<string, string>();
			fundUseStatuses.Add("Off", "Off");
			fundUseStatuses.Add("On", "On");
			return fundUseStatuses;
        }
    }
}
