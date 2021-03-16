using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using System.Web.Mvc;

namespace CWTDesktopDatabase.ViewModels
{
	[Bind(Include = "FareRestriction, PolicyRouting")]
	public class FareRestrictionViewModel : CWTBaseViewModel
     {
        public FareRestriction FareRestriction { get; set; }
        public PolicyRouting PolicyRouting { get; set; }
 
        public FareRestrictionViewModel()
        {
        }

        public FareRestrictionViewModel(FareRestriction fareRestriction, PolicyRouting policyRouting)
        {
            FareRestriction = fareRestriction;
            PolicyRouting = policyRouting;
        }
    }
}
