using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Repository
{
    public class PolicySupplierDealCodeTypeRepository
    {
        private PolicySupplierDealCodeTypeDC db = new PolicySupplierDealCodeTypeDC(Settings.getConnectionString());

        public IQueryable<PolicySupplierDealCodeType> GetAllPolicySupplierDealCodeTypes()
        {
            return db.PolicySupplierDealCodeTypes.OrderBy(c => c.PolicySupplierDealCodeTypeDescription);
        }

        public PolicySupplierDealCodeType GetPolicySupplierDealCodeType(int id)
        {
            return db.PolicySupplierDealCodeTypes.SingleOrDefault(c => c.PolicySupplierDealCodeTypeId == id);
        }
    }
}
