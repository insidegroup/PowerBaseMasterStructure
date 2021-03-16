using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using System.Linq.Dynamic;

namespace CWTDesktopDatabase.Repository
{
    public class PolicyCountryStatusRepository
    {
        private PolicyCountryStatusDC db = new PolicyCountryStatusDC(Settings.getConnectionString());

        public IQueryable<PolicyCountryStatus> GetAllPolicyCountryStatuses()
        {
            return db.PolicyCountryStatus.OrderBy(c => c.PolicyCountryStatusDescription); ;
        }

        public PolicyCountryStatus GetPolicyCountryStatus(int policyCountryStatusId)
        {
            return db.PolicyCountryStatus.SingleOrDefault(c => c.PolicyCountryStatusId == policyCountryStatusId);
        }

        public PolicyCountryStatus GetPolicyCountryStatusByDescription(string policyCountryStatusDescription) => db.PolicyCountryStatus.SingleOrDefault(c => c.PolicyCountryStatusDescription == policyCountryStatusDescription);
    }
}
