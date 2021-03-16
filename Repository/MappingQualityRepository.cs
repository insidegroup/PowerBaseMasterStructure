using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Repository
{
    public class MappingQualityRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        public IQueryable<MappingQuality> GetAllMappingQualities()
        {
            return db.MappingQualities.OrderBy(c => c.MappingQualityDescription);
        }
    }
}