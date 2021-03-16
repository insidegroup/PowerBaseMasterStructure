using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
	[MetadataType(typeof(CommissionableRouteItemValidation))]
	public partial class CommissionableRouteItem : CWTBaseModel
    {
        public string ProductName { get; set; }
        public string SupplierName { get; set; }
    }
}
