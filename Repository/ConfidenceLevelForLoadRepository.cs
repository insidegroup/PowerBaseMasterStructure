using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Repository
{
    public class ConfidenceLevelForLoadRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        public IQueryable<ConfidenceLevelForLoad> GetAllConfidenceLevelForLoads()
        {
            return db.ConfidenceLevelForLoads.OrderBy(c => c.ConfidenceLevelForLoadDescription);
        }

        public ConfidenceLevelForLoad GetConfidenceLevelForLoad(int confidenceLevelForLoadId)
        {
            return db.ConfidenceLevelForLoads.SingleOrDefault(c => c.ConfidenceLevelForLoadId == confidenceLevelForLoadId);
        }
    }
}
