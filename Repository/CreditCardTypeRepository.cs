using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;


namespace CWTDesktopDatabase.Repository
{
    public class CreditCardTypeRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        public IQueryable<CreditCardType> GetAllCreditCardTypes()
        {
            return db.CreditCardTypes.OrderBy(c => c.CreditCardTypeDescription);
        }

        public CreditCardType GetCreditCardType(int creditCardTypeId)
        {
            return db.CreditCardTypes.SingleOrDefault(c => c.CreditCardTypeId == creditCardTypeId);
        }
    }
}