using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(PartnerValidation))]
	public partial class Partner : CWTBaseModel
    {
    }

	public partial class PartnerReference
	{
		public string TableName { get; set; }
	}

	public partial class PartnerJSON
	{
		public int PartnerId { get; set; }
		public string PartnerName { get; set; }
		public string CountryName { get; set; }
	}
}
