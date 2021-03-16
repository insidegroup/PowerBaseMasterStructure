using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
	[Bind(Exclude = "CreationTimestamp")]
	public class CWTBaseModel
	{
	}
}