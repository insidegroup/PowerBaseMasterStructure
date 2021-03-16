using CWTDesktopDatabase.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.Models
{
	[MetadataType(typeof(PriceTrackingContactValidation))]
	public partial class PriceTrackingContact : CWTBaseModel
	{
		public ContactType ContactType { get; set; }
        public string PriceTrackingDashboardAccessFlagSelectedValue { get;  set; }

    }
}
