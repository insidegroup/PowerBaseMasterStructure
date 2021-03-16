using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(PseudoCityOrOfficeDefinedRegionValidation))]
	public partial class PseudoCityOrOfficeDefinedRegion : CWTBaseModel
    {
    }

	public partial class PseudoCityOrOfficeDefinedRegionReference
	{
		public string TableName { get; set; }
	}

	public partial class PseudoCityOrOfficeDefinedRegionJSON
	{
		public int PseudoCityOrOfficeDefinedRegionId { get; set; }
		public string PseudoCityOrOfficeDefinedRegionName { get; set; }
	}
}
