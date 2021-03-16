using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Repository
{
    public class CurrencyRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());
        public IQueryable<Currency> GetAllCurrencies()
        {
            return db.Currencies.OrderBy(c => c.Name);
        }

        public Currency GetCurrency(string currencyCode)
        {
            return db.Currencies.SingleOrDefault(c => c.CurrencyCode == currencyCode);
        }

        public string GetCurrencyName(string currencyCode)
        {
            string currencyName = string.Empty;

            Currency currency = GetCurrency(currencyCode);

            if(currency != null)
            {
                currencyName = currency.Name;
            }
            return currencyName;
        }
    }
}

