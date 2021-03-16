using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
	public class ClientDefinedBusinessRuleGroupsVM : CWTBaseViewModel
	{
		public CWTPaginatedList<spDesktopDataAdmin_SelectClientDefinedBusinessRuleGroups_v1Result> ClientDefinedBusinessRuleGroups { get; set; }
		public bool HasDomainWriteAccess { get; set; }

		//Search Fields
		public string Filter { get; set; }
		public string SearchTerm { get; set; }
		public string Category { get; set; }
		public string ClientDefinedRuleGroupName { get; set; }
		public IEnumerable<SelectListItem> SearchListFilters { get; set; }
		public IEnumerable<SelectListItem> ClientDefinedRuleBusinessEntityCategories { get; set; }

		public ClientDefinedBusinessRuleGroupsVM()
		{
			HasDomainWriteAccess = false;
		}

		public ClientDefinedBusinessRuleGroupsVM(
			CWTPaginatedList<spDesktopDataAdmin_SelectClientDefinedBusinessRuleGroups_v1Result> clientDefinedBusinessRuleGroups,
			bool hasDomainWriteAccess,
			string filter,
			string category,
			string clientDefinedRuleGroupName,
			 IEnumerable<SelectListItem> searchListFilters)
		{
			ClientDefinedBusinessRuleGroups = clientDefinedBusinessRuleGroups;
			HasDomainWriteAccess = hasDomainWriteAccess;
			Category = category;
			ClientDefinedRuleGroupName = clientDefinedRuleGroupName;
			SearchListFilters = searchListFilters;
			Filter = filter;
		}
	}
}