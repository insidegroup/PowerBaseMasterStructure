using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.Models
{
    public class ReasonCodeClientSubUnitCountryVM
    {
        public string ClientSubUnitName { get; set; }
        public string ClientSubUnitGuid { get; set; }
        public string CountryName { get; set; }
        public bool HasWriteAccess { get; set; }
        public int IsClientExpiredFlag { get; set; }

        public ReasonCodeClientSubUnitCountryVM()
        {
        }
        public ReasonCodeClientSubUnitCountryVM(string clientSubUnitName, string clientSubUnitGuid, string countryName, bool hasWriteAccess)
        {
            ClientSubUnitName = clientSubUnitName;
            ClientSubUnitGuid = clientSubUnitGuid;
            CountryName = countryName;
            HasWriteAccess = hasWriteAccess;
        }
    }
}