using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Repository
{
    public class AirlineCabinRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        public IQueryable<AirlineCabin> GetAllAirlineCabins()
        {
            return db.AirlineCabins.OrderBy(c => c.AirlineCabinRank);
        }

        public AirlineCabin GetAirlineCabin(string airlineCabinCode)
        {
            return db.AirlineCabins.SingleOrDefault(c => c.AirlineCabinCode == airlineCabinCode);
        }
    }
}

