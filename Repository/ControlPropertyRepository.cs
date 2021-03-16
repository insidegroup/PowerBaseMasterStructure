using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using System.Linq.Dynamic;
namespace CWTDesktopDatabase.Repository
{
    public class ControlPropertyRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        public IQueryable<ControlProperty> GetAllControlProperties()
        {
            return db.ControlProperties.OrderBy(c=> c.ControlPropertyDescription);
        }
        public ControlProperty GetControlProperty(int controlPropertyId)
        {
            return db.ControlProperties.SingleOrDefault(c => c.ControlPropertyId == controlPropertyId);
        }
    }
}
