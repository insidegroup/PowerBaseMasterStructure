using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Repository
{
    public class PolicyAirMissedSavingsThresholdRoutingRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());
        public IQueryable<PolicyAirMissedSavingsThresholdRoutingCode> GetAllPolicyAirMissedSavingsThresholdRoutings()
        {
            return db.PolicyAirMissedSavingsThresholdRoutingCodes.OrderBy(c => c.RoutingCode);
        }

        public PolicyAirMissedSavingsThresholdRoutingCode GetPolicyAirMissedSavingsThresholdRoutings(string routingCode)
        {
            return db.PolicyAirMissedSavingsThresholdRoutingCodes.SingleOrDefault(c => c.RoutingCode == routingCode);
        }
    }
}

