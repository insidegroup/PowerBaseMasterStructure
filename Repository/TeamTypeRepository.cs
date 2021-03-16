using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Repository
{
    public class TeamTypeRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());
        public IQueryable<TeamType> GetAllTeamTypes()
        {
            return db.TeamTypes.OrderBy(t => t.TeamTypeDescription);
        }

        public TeamType GetTeamType(string code)
        {
            return db.TeamTypes.SingleOrDefault(c => c.TeamTypeCode == code);
        }
    }
}
