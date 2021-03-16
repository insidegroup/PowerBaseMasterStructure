using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Repository
{
    public class PNROutputTypeRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());
        public IQueryable<PNROutputType> GetAllPNROutputTypes()
        {
            return db.PNROutputTypes;
        }

        public PNROutputType GetPNROutputType(int? id)
        {
            return db.PNROutputTypes.SingleOrDefault(c => c.PNROutputTypeId == id);
        }
    }
}
