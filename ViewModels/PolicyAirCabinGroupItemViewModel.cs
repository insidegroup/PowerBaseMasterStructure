using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.ViewModels
{
	[Bind(Include = "PolicyGroup, PolicyAirCabinGroupItem, PolicyRouting")]
	public class PolicyAirCabinGroupItemViewModel : CWTBaseViewModel
     {
        public PolicyGroup PolicyGroup { get; set; }
        public PolicyAirCabinGroupItem PolicyAirCabinGroupItem { get; set; }
        public PolicyRouting PolicyRouting { get; set; }
 
        public PolicyAirCabinGroupItemViewModel()
        {
        }
        public PolicyAirCabinGroupItemViewModel(PolicyGroup policyGroup, PolicyAirCabinGroupItem policyAirCabinGroupItem, PolicyRouting policyRouting)
        {
            PolicyGroup = policyGroup;
            PolicyAirCabinGroupItem = policyAirCabinGroupItem;
            PolicyRouting = policyRouting;
        }
    }
}
