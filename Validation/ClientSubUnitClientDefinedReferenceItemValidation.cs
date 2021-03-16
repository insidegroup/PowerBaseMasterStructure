using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Helpers;
using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
	public class ClientSubUnitClientDefinedReferenceItemValidation
    {
		[Required(ErrorMessage = "Validate Value Required")]
		public string RelatedToValue  { get; set; }
    }
}