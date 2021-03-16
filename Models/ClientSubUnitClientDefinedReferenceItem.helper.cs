using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Validation;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
	[MetadataType(typeof(ClientSubUnitClientDefinedReferenceItemValidation))]
	public partial class ClientSubUnitClientDefinedReferenceItem : CWTBaseModel
	{
		public ClientSubUnit ClientSubUnit { get; set; }
    }
}