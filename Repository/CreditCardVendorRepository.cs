using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;


namespace CWTDesktopDatabase.Repository
{
    public class CreditCardVendorRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        public IQueryable<CreditCardVendor> GetAllCreditCardVendors()
        {
            return db.CreditCardVendors.OrderBy(c => c.CreditCardVendorName);
        }

        public CreditCardVendor GetCreditCardVendor(string creditCardVendorCode)
        {
            return db.CreditCardVendors.SingleOrDefault(c => c.CreditCardVendorCode == creditCardVendorCode);
        }
    }
}