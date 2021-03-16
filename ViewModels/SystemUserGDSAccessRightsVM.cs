using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.ViewModels
{
    public class SystemUserGDSAccessRightsVM
    {
		public CWTPaginatedList<spDesktopDataAdmin_SelectSystemUserGDSAccessRights_v1Result> SystemUserGDSAccessRights { get; set; }
		public SystemUser SystemUser { get; set; }
    }
}