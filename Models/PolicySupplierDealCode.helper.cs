using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(PolicySupplierDealCodeValidation))]
	public partial class PolicySupplierDealCode : CWTBaseModel
    {
        public string PolicyGroupName { get; set; }
        public string PolicySupplierDealCodeTypeDescription { get; set; }
        public string SupplierName { get; set; }
        public string PolicyLocationName { get; set; }
        public string ProductName { get; set; }
        public string GDSName { get; set; }
		public bool EnabledFlagNonNullable { get; set; } //EnabledFlag is nullable
		public bool OSIFlagNonNullable { get; set; } //OSIFlag is nullable

		public TourCodeType TourCodeType { get; set; }
	}
}
