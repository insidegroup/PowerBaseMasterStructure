using System.Linq;
using CWTDesktopDatabase.Models;
using System.Linq.Dynamic;

namespace CWTDesktopDatabase.Repository
{
    public class PolicyCarStatusRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        public IQueryable<PolicyCarStatus> GetAllPolicyCarStatuses()
        {
            return db.PolicyCarStatus.OrderBy("PolicyCarStatusDescription");
        }

        public PolicyCarStatus GetPolicyCarStatus(int policyCarStatusId)
        {
            return db.PolicyCarStatus.SingleOrDefault(c => c.PolicyCarStatusId == policyCarStatusId);
        }
    }
}
