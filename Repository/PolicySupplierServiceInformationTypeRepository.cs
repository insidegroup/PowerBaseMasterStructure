using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Repository
{
    public class PolicySupplierServiceInformationTypeRepository
    {
        private PolicySupplierServiceInformationTypeDC db = new PolicySupplierServiceInformationTypeDC(Settings.getConnectionString());

        public IQueryable<PolicySupplierServiceInformationType> GetAllPolicySupplierServiceInformationTypes()
        {
            return db.PolicySupplierServiceInformationTypes.OrderBy(c => c.PolicySupplierServiceInformationTypeDescription);
        }

        public PolicySupplierServiceInformationType GetPolicySupplierServiceInformationType(int id)
        {
            return db.PolicySupplierServiceInformationTypes.SingleOrDefault(c => c.PolicySupplierServiceInformationTypeId == id);
        }
    }
}
