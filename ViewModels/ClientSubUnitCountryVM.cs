using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientSubUnitCountryVM : CWTBaseViewModel
    {
        public string ClientSubUnitName { get; set; }
        public string ClientSubUnitGuid { get; set; }
        public string CountryName { get; set; }
        public bool HasWriteAccess { get; set; }
        public int IsClientExpiredFlag { get; set; }

        public ClientSubUnitCountryVM()
        {
        }
        public ClientSubUnitCountryVM(string clientSubUnitName, string clientSubUnitGuid, string countryName, bool hasWriteAccess)
        {
            ClientSubUnitName = clientSubUnitName;
            ClientSubUnitGuid = clientSubUnitGuid;
            CountryName = countryName;
            HasWriteAccess = hasWriteAccess;
        }
    }
}