using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
	public class ClientProfileAdminGroupsVM : CWTBaseViewModel
	{
		public CWTPaginatedList<spDesktopDataAdmin_SelectClientProfileAdminGroups_v1Result> ClientProfileAdminGroups { get; set; }
        public bool HasDomainWriteAccess { get; set; }
 
        public ClientProfileAdminGroupsVM()
        {
            HasDomainWriteAccess = false;
        }
        public ClientProfileAdminGroupsVM(
			CWTPaginatedList<spDesktopDataAdmin_SelectClientProfileAdminGroups_v1Result> clientProfileAdminGroups, 
            bool hasDomainWriteAccess)
        {
            ClientProfileAdminGroups = clientProfileAdminGroups;
            HasDomainWriteAccess = hasDomainWriteAccess;
        }
    }
}