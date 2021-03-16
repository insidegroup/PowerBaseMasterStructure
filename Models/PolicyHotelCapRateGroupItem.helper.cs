using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(PolicyHotelCapRateGroupItemValidation))]
	public partial class PolicyHotelCapRateGroupItem : CWTBaseModel
    {
        public string PolicyGroupName { get; set; }
        public string PolicyLocation { get; set; }
        public string CurrencyName { get; set; }
    }
}
