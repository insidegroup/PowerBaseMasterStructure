using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.ViewModels
{
	public class ThirdPartyUsersVM
    {
		public CWTPaginatedList<spDesktopDataAdmin_SelectThirdPartyUsers_v1Result> ThirdPartyUsers { get; set; }
    }
}