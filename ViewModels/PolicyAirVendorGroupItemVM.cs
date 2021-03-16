using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
    public class PolicyAirVendorGroupItemVM : CWTBaseViewModel
    {
        public PolicyGroup PolicyGroup { get; set; }
        public PolicyAirVendorGroupItem PolicyAirVendorGroupItem { get; set; }
        public PolicyRouting PolicyRouting { get; set; }
 
        public PolicyAirVendorGroupItemVM()
        {
        }
        public PolicyAirVendorGroupItemVM(PolicyGroup policyGroup, PolicyAirVendorGroupItem policyAirVendorGroupItem, PolicyRouting policyRouting)
        {
            PolicyGroup = policyGroup;
            PolicyAirVendorGroupItem = policyAirVendorGroupItem;
            PolicyRouting = policyRouting;
        }
    }
}
