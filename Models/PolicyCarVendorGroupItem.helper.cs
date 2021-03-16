using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;

namespace CWTDesktopDatabase.Models
{
    //extra fields not in PolicyCarVendorGroupItem Table
    [MetadataType(typeof(PolicyCarVendorGroupItemValidation))]
	public partial class PolicyCarVendorGroupItem : CWTBaseModel
    {
        public string PolicyGroupName { get; set; }
        public string SupplierName { get; set; }
        public string ProductName { get; set; }
        public string PolicyCarStatus { get; set; }
        public string PolicyLocationName { get; set; }

    }
}
