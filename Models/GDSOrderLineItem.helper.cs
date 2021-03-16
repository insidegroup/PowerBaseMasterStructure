using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
	[MetadataType(typeof(GDSOrderDetailValidation))]
	public partial class GDSOrderLineItem : CWTBaseModel
    {
		public IEnumerable<SelectListItem> GDSOrderLineItemActions { get; set; }
		public IEnumerable<SelectListItem> GDSOrderDetails { get; set; }
    }
}
