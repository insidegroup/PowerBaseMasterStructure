using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(PolicyAirVendorGroupItemValidation))]
	public partial class PolicyAirVendorGroupItem : CWTBaseModel
    {
        public string PolicyGroupName { get; set; }
        public string SupplierName { get; set; }
        public string ProductName { get; set; }
        public string PolicyAirStatus { get; set; }
    }
}
