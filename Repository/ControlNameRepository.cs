using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using System.Linq.Dynamic;

namespace CWTDesktopDatabase.Repository
{
    public class ControlNameRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        public IQueryable<ControlName> GetAllControlNames()
        {
            return db.ControlNames.OrderBy(c => c.ControlName1);
        }
        public ControlName GetControlName(int controlNameId)
        {
            return db.ControlNames.SingleOrDefault(c => c.ControlNameId == controlNameId);
        }
    }
}
