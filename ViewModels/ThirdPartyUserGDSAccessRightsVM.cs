using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.ViewModels
{
    public class ThirdPartyUserGDSAccessRightsVM
    {
		public CWTPaginatedList<spDesktopDataAdmin_SelectThirdPartyUserGDSAccessRights_v1Result> ThirdPartyUserGDSAccessRights { get; set; }
		public ThirdPartyUser ThirdPartyUser { get; set; }
    }
}