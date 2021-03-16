using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(PseudoCityOrOfficeLocationTypeValidation))]
	public partial class PseudoCityOrOfficeLocationType : CWTBaseModel
    {
    }

	public partial class PseudoCityOrOfficeLocationTypeReference
	{
		public string TableName { get; set; }
	}
}
