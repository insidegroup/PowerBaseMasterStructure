using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(PseudoCityOrOfficeAddressValidation))]
	public partial class PseudoCityOrOfficeAddress : CWTBaseModel
    {
		public string StateProvinceName { get; set; }
    }

	public partial class PseudoCityOrOfficeAddressReference
	{
		public string TableName { get; set; }
	}

	public partial class PseudoCityOrOfficeAddressJSON
	{
		public int PseudoCityOrOfficeAddressId { get; set; }
		public string PseudoCityOrOfficeAddressName { get; set; }
	}

	public partial class PseudoCityOrOfficeAddressCountryGlobalRegionJSON
	{
		public string CountryCode { get; set; }
		public string CountryName { get; set; }
		public string GlobalRegionCode { get; set; }
		public string GlobalRegionName { get; set; }
	}
}
