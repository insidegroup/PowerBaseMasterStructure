using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
	public class ClientProfileGroupsVM : CWTBaseViewModel
	{
		public CWTPaginatedList<spDesktopDataAdmin_SelectClientProfileGroups_v1Result> ClientProfileGroups { get; set; }
        public bool HasDomainWriteAccess { get; set; }
 
        public ClientProfileGroupsVM()
        {
            HasDomainWriteAccess = false;
        }
        public ClientProfileGroupsVM(
			CWTPaginatedList<spDesktopDataAdmin_SelectClientProfileGroups_v1Result> clientProfileGroups, 
            bool hasDomainWriteAccess)
        {
            ClientProfileGroups = clientProfileGroups;
            HasDomainWriteAccess = hasDomainWriteAccess;
        }
    }
}