using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Helpers;
using System.ComponentModel.DataAnnotations;
using Foolproof;

namespace CWTDesktopDatabase.Validation
{
	public class ClientDefinedReferenceItemValidation
    {

		[Required(ErrorMessage = "Field Required")]
		public string DisplayName { get; set; }
		
		[Required(ErrorMessage = "Field Required")]
		public string DisplayNameAlias { get; set; }

		public string EntryFormat { get; set; }

		[Required(ErrorMessage = "Field Required")]
		public int? MinLength { get; set; }

		[Required(ErrorMessage = "Field Required")]
		public int? MaxLength { get; set; }
		[RegularExpression(@"^([\w\s]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string OnlineDefaultValue { get; set; }
	}
}