using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using System.Web.Mvc;

namespace CWTDesktopDatabase.ViewModels
{
    public class PolicyAirMissedSavingsThresholdGroupItemVM : CWTBaseViewModel
    {
        public PolicyGroup PolicyGroup { get; set; }
        public PolicyAirMissedSavingsThresholdGroupItem PolicyAirMissedSavingsThresholdGroupItem { get; set; }
        public IEnumerable<SelectListItem> Currencies { get; set; }
        public IEnumerable<SelectListItem> RoutingCodes { get; set; }
 
        public PolicyAirMissedSavingsThresholdGroupItemVM()
        {
        }
        public PolicyAirMissedSavingsThresholdGroupItemVM(PolicyGroup policyGroup, PolicyAirMissedSavingsThresholdGroupItem policyAirMissedSavingsThresholdGroupItem,
            IEnumerable<SelectListItem> currencies,
            IEnumerable<SelectListItem> routingCodes)
        {
            PolicyGroup = policyGroup;
            PolicyAirMissedSavingsThresholdGroupItem = policyAirMissedSavingsThresholdGroupItem;
            RoutingCodes = routingCodes;
        }
    }
}
