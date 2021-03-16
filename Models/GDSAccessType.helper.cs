using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
	[MetadataType(typeof(GDSAccessTypeValidation))]
	public partial class GDSAccessType : CWTBaseModel
    {
    }

	public class ValidGDSAccessTypeJSON
	{
		public int GDSAccessTypeId { get; set; }
		public string GDSAccessTypeName { get; set; }
	}

	public partial class GDSAccessTypeReference
	{
		public string TableName { get; set; }
	}
}
