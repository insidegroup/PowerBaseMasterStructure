using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
	public partial class ExternalSystemLogin : CWTBaseModel
	{
		public IEnumerable<SelectListItem> Countries { get; set; }
	}
}