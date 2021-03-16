using CWTDesktopDatabase.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.Models
{
	[MetadataType(typeof(GDSThirdPartyVendorValidation))]
	public partial class GDSThirdPartyVendor : CWTBaseModel
	{
	}

	public partial class GDSThirdPartyVendorReference
	{
		public string TableName { get; set; }
	}
}
