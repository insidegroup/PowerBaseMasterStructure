using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.Helpers
{
	public class Security
	{
		public static bool IsHttps()
		{
			return HttpContext.Current.Request.IsSecureConnection;
		}
	}
}