using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using System.Linq.Dynamic;
using System.Web.Security;

namespace CWTDesktopDatabase.Repository
{
    public class WorkFlowTypeRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());
        public IQueryable<WorkFlowType> GetAllWorkflowTypes()
        {
            return db.WorkFlowTypes;
        }

        public WorkFlowType GetWorkFlowTypeType(int? id)
        {
            return db.WorkFlowTypes.SingleOrDefault(c => c.WorkFlowTypeId == id);
        }
    }
}
