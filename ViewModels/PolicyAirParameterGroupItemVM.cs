using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
    public class PolicyAirParameterGroupItemVM : CWTBaseViewModel
    {
        public PolicyGroup PolicyGroup { get; set; }
        public PolicyAirParameterGroupItem PolicyAirParameterGroupItem { get; set; }
        public PolicyRouting PolicyRouting { get; set; }
 
        public PolicyAirParameterGroupItemVM()
        {
        }

		public PolicyAirParameterGroupItemVM(PolicyGroup policyGroup, PolicyAirParameterGroupItem policyAirParameterGroupItem, PolicyRouting policyRouting)
        {
            PolicyGroup = policyGroup;
			PolicyAirParameterGroupItem = policyAirParameterGroupItem;
            PolicyRouting = policyRouting;
        }
    }
}
