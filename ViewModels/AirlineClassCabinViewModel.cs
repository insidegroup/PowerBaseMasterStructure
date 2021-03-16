using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
	public class AirlineClassCabinViewModel : CWTBaseViewModel
   {
        public AirlineClassCabin AirlineClassCabin { get; set; }
        public PolicyRouting PolicyRouting { get; set; }
 
        public AirlineClassCabinViewModel()
        {
        }
        public AirlineClassCabinViewModel(AirlineClassCabin airlineClassCabin, PolicyRouting policyRouting)
        {
            AirlineClassCabin = airlineClassCabin;
            PolicyRouting = policyRouting;
        }
    }
}