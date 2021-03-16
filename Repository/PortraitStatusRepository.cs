using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;


namespace CWTDesktopDatabase.Repository
{
    public class PortraitStatusRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        public PortraitStatus GetPortraitStatus(int portraitStatusId)
        {
            return db.PortraitStatus.SingleOrDefault(c => c.PortraitStatusId == portraitStatusId);
        }
        public IQueryable<PortraitStatus> GetAllPortraitStatuses()
        {
            return db.PortraitStatus.OrderBy(c => c.PortraitStatusDescription);
        }
    }
}
