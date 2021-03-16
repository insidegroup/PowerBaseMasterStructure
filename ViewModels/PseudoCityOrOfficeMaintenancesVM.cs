using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.ViewModels
{
    public class PseudoCityOrOfficeMaintenancesVM
    {
        public CWTPaginatedList<spDesktopDataAdmin_SelectPseudoCityOrOfficeMaintenance_v1Result> PseudoCityOrOfficeMaintenances { get; set; }
    }
}