using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
	[MetadataType(typeof(PriceTrackingHandlingFeeItemValidation))]
	public partial class PriceTrackingHandlingFeeItem : CWTBaseModel
	{
		public Product Product { get; set; }
	}
}

