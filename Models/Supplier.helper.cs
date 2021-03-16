using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
	[MetadataType(typeof(SupplierValidation))]
	public partial class Supplier : CWTBaseModel
	{
		public string ProductName { get; set; }
	}

	public partial class SupplierJSON
	{
		public string SupplierName { get; set; }
		public string SupplierCode { get; set; }
	}

	public partial class SupplierReference
	{
		public string TableName { get; set; }
	}
}
