using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class PointOfSaleFeeLoadsVM : CWTBaseViewModel
    {
        public CWTPaginatedList<spDesktopDataAdmin_SelectPointOfSaleFeeLoads_v1Result> PointOfSaleFeeLoads { get; set; }
        public bool HasDomainWriteAccess { get; set; }

        //Search Fields
        public string ClientTopUnitName { get; set; }
        public string ClientTopUnitGuid { get; set; }

        public string ClientSubUnitName { get; set; }
        public string ClientSubUnitGuid { get; set; }

        public string TravelerTypeName { get; set; }
        public string TravelerTypeGuid { get; set; }

        public PointOfSaleFeeLoadsVM()
        {
            HasDomainWriteAccess = false;
        }
    }
}