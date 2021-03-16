using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(PolicySupplierServiceInformationValidation))]
    public partial class PolicySupplierServiceInformation : CWTBaseModel
    {
        public string PolicyGroupName { get; set; }
        public string PolicySupplierServiceInformationTypeDescription { get; set; }
        public string ProductName { get; set; }
        public string SupplierName { get; set; }
        public bool EnabledFlagNonNullable { get; set; } //EnabledFlag is nullable
    }
}
