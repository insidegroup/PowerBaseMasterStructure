using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(GDSRequestTypeValidation))]
	public partial class GDSRequestType : CWTBaseModel
    {
    }

	public partial class GDSRequestTypeReference
	{
		public string TableName { get; set; }
	}
}
