using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientDefinedRuleGroupsVM : CWTBaseViewModel
    {
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientDefinedRuleGroups_v1Result> ClientDefinedRuleGroups { get; set; }
        public bool HasDomainWriteAccess { get; set; }

		//Search Fields
		public string Filter { get; set; }
		public string SearchTerm { get; set; }
		public string ClientTopUnitName { get; set; }
		public string ClientSubUnitName { get; set; }
		public string ClientAccountNumber { get; set; }
		public string SourceSystemCode { get; set; }
		public string TravelerTypeName { get; set; }
		public string ClientDefinedRuleGroupName { get; set; }
		public IEnumerable<SelectListItem> SearchListFilters { get; set; }

        public ClientDefinedRuleGroupsVM()
        {
            HasDomainWriteAccess = false;
        }

        public ClientDefinedRuleGroupsVM(
			CWTPaginatedList<spDesktopDataAdmin_SelectClientDefinedRuleGroups_v1Result> clientDefinedRuleGroups, 
			bool hasDomainWriteAccess,
			string filter,
			string clientTopUnitName,
			string clientSubUnitName,
			string clientAccountNumber,
			string sourceSystemCode,
			string travelerTypeName,
			string clientDefinedRuleGroupName,
			 IEnumerable<SelectListItem> searchListFilters)
        {
            ClientDefinedRuleGroups = clientDefinedRuleGroups;
            HasDomainWriteAccess = hasDomainWriteAccess;
			ClientTopUnitName = clientTopUnitName; 
			ClientSubUnitName = clientSubUnitName;
			ClientAccountNumber = clientAccountNumber;
			SourceSystemCode = sourceSystemCode;
			TravelerTypeName = travelerTypeName;
			ClientDefinedRuleGroupName = clientDefinedRuleGroupName;
			SearchListFilters = searchListFilters;
			Filter = filter;
        }
    }
}