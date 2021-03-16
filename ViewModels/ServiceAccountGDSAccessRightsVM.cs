using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.ViewModels
{
    public class ServiceAccountGDSAccessRightsVM
    {
		public CWTPaginatedList<spDesktopDataAdmin_SelectServiceAccountGDSAccessRights_v1Result> ServiceAccountGDSAccessRights { get; set; }
		public ServiceAccount ServiceAccount { get; set; }
    }
}