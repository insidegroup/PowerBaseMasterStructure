using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Repository
{
    public class PolicyHotelStatusRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        public IQueryable<PolicyHotelStatus> GetAllPolicyHotelStatuses()
        {
            return db.PolicyHotelStatus.OrderBy(c => c.PolicyHotelStatusDescription);
        }

        public PolicyHotelStatus GetPolicyHotelStatus(int id)
        {
            return db.PolicyHotelStatus.SingleOrDefault(c => c.PolicyHotelStatusId == id);
        }
    }
}
