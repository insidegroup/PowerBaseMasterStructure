using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(PolicyAirCabinGroupItemValidation))]
	public partial class PolicyAirCabinGroupItem : CWTBaseModel
    {
        public string PolicyGroupName { get; set; }
        public string AirlineCabinDefaultDescription { get; set; }

    }
     
}
