using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
	[MetadataType(typeof(FareRedistributionValidation))]
	public partial class FareRedistribution : CWTBaseModel
    {
    }

	public partial class FareRedistributionReference
	{
		public string TableName { get; set; }
	}

	public partial class FareRedistributionJSON
	{
		public int FareRedistributionId { get; set; }
		public string FareRedistributionName { get; set; }
	}
}
