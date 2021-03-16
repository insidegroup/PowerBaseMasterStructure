using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CWTDesktopDatabase.ViewModels
{
	public class OptionalFieldGroupsVM : CWTBaseViewModel
	{
		public CWTPaginatedList<spDesktopDataAdmin_SelectOptionalFieldGroups_v1Result> OptionalFieldGroups { get; set; }
		public CWTPaginatedList<spDesktopDataAdmin_SelectOptionalFieldGroupsOrphaned_v1Result> OptionalFieldGroupsOrphaned { get; set; }
		public IEnumerable<SelectListItem> HierarchyTypes { get; set; }
		public bool HasDomainWriteAccess { get; set; }

		public OptionalFieldGroupsVM()
        {
			HasDomainWriteAccess = false;
        }

		public OptionalFieldGroupsVM(CWTPaginatedList<spDesktopDataAdmin_SelectOptionalFieldGroups_v1Result> optionalFieldGroups, 
									CWTPaginatedList<spDesktopDataAdmin_SelectOptionalFieldGroupsOrphaned_v1Result> optionalFieldGroupsOrphaned, 
									bool hasDomainWriteAccess)
        {
            OptionalFieldGroups = optionalFieldGroups;
            OptionalFieldGroupsOrphaned = optionalFieldGroupsOrphaned;
            HasDomainWriteAccess = hasDomainWriteAccess;
        }
	}
}