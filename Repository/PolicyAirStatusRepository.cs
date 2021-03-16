using System.Linq;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Repository
{
    public class PolicyAirStatusRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        public IQueryable<PolicyAirStatus> GetAllPolicyAirStatuses()
        {
            return db.PolicyAirStatus;
        }

        public PolicyAirStatus GetPolicyAirStatus(int policyAirStatusId)
        {
            return db.PolicyAirStatus.SingleOrDefault(c => c.PolicyAirStatusId == policyAirStatusId);
        }

        public PolicyAirStatus GetPolicyAirStatusByDescription(string policyAirStatusDescription)
        {
            return db.PolicyAirStatus.SingleOrDefault(c => c.PolicyAirStatusDescription == policyAirStatusDescription);
        }
    }
}
