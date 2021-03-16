using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
	[MetadataType(typeof(IATAValidation))]
	public partial class IATA : CWTBaseModel
    {
    }

	public partial class IATAReference
	{
		public string ReferenceName { get; set; }
	}
}
