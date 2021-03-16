using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(PseudoCityOrOfficeTypeValidation))]
	public partial class PseudoCityOrOfficeType : CWTBaseModel
    {
    }

	public partial class PseudoCityOrOfficeTypeReference
	{
		public string TableName { get; set; }
	}
}
