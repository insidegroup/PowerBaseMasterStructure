using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Linq.Dynamic;
using System.Text.RegularExpressions;

namespace CWTDesktopDatabase.Repository
{
    public class PriceTrackingSystemRepository
    {
        private PriceTrackingDC db = new PriceTrackingDC(Settings.getConnectionString());

		//Get One PriceTrackingSystem
		public PriceTrackingSystem GetPriceTrackingSystem(int id)
		{
			return db.PriceTrackingSystems.SingleOrDefault(c => c.PriceTrackingSystemId == id);
		}

		//Get All PriceTrackingSystems
		public List<PriceTrackingSystem> GetAllPriceTrackingSystems()
		{
			return db.PriceTrackingSystems.OrderBy(x => x.PriceTrackingSystemName).ToList();
		}
	}
}
