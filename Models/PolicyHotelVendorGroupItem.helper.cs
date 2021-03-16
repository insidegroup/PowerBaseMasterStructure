using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;

namespace CWTDesktopDatabase.Models
{
	//extra fields not in PolicyCarVendorGroupItem Table
    [MetadataType(typeof(PolicyHotelVendorGroupItemValidation))]
	public partial class PolicyHotelVendorGroupItem : CWTBaseModel
    {
        public string PolicyGroupName { get; set; }
        public string SupplierName { get; set; }
        public string ProductName { get; set; }
        public string PolicyHotelStatus { get; set; }
        public string PolicyLocationName { get; set; }

    }
}
