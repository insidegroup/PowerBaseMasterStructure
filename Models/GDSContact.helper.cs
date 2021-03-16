using CWTDesktopDatabase.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.Models
{
	[MetadataType(typeof(GDSContactValidation))]
	public partial class GDSContact : CWTBaseModel
	{
		public IEnumerable<GDSRequestType> GDSRequestTypes { get; set; }
		public IEnumerable<int> GDSRequestTypeIds { get; set; }
		public IEnumerable<string> GDSRequestTypeNames { get; set; }
	}
}
