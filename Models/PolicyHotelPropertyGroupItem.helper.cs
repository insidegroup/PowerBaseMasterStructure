using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(PolicyHotelPropertyGroupItemValidation))]
	public partial class PolicyHotelPropertyGroupItem : CWTBaseModel
    {
        public string PolicyGroupName { get; set; }
        public string PolicyHotelStatus { get; set; }
        public string HarpHotelName { get; set; }
    }
}
