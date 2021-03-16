using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
	[MetadataType(typeof(ClientDefinedReferenceItemValidation))]
	[Bind(Exclude = "CreationTimestamp")]
	public partial class ClientDefinedReferenceItem : CWTBaseModel
    {
		public bool MandatoryFlagNullable { get; set; }
		public bool TableDrivenFlagNullable { get; set; }

		public string ClientDefinedReferenceItemProductIds { get; set; }
		public string ClientDefinedReferenceItemContextIds { get; set; }

		public List<Product> Products { get; set; }
		public List<Context> Contexts { get; set; }
    }
}
