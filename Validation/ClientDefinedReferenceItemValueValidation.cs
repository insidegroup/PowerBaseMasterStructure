using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Helpers;
using System.ComponentModel.DataAnnotations;
using Foolproof;

namespace CWTDesktopDatabase.Validation
{
	public class ClientDefinedReferenceItemValueValidation
    {
		[Required(ErrorMessage = "Field Required")]
		public string Value { get; set; }

		[Required(ErrorMessage = "Field Required")]
		public string ValueDescription { get; set; }
	}
}