using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Repository
{
    public class AdministratorRoleHierarchyLevelTypeRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        public AdministratorRoleHierarchyLevelType GetAdministratorRoleHierarchyLevelType(int AdministratorRoleId, int hierarchyLevelTypeId)
        {
            return db.AdministratorRoleHierarchyLevelTypes.SingleOrDefault(c => (c.AdministratorRoleId == AdministratorRoleId && c.HierarchyLevelTypeId == hierarchyLevelTypeId));
        }
    }
}
