using CWTDesktopDatabase.Models;
using System.Linq;

namespace CWTDesktopDatabase.Repository
{
    public class PolicyCityStatusRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        public IQueryable<PolicyCityStatus> GetAllPolicyCityStatuses()
        {
            return db.PolicyCityStatus.OrderBy(c => c.PolicyCityStatusDescription);
        }
        public PolicyCityStatus GetPolicyCityStatus(int policyCityStatusId)
        {
            return db.PolicyCityStatus.SingleOrDefault(c => c.PolicyCityStatusId == policyCityStatusId);
        }

        public PolicyCityStatus GetPolicyCityStatusByDescrition(string policyCityStatusDescription) => db.PolicyCityStatus.SingleOrDefault(c => c.PolicyCityStatusDescription == policyCityStatusDescription);
    }
}