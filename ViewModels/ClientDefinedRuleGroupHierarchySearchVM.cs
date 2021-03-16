using System.Collections.Generic;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientDefinedRuleGroupHierarchySearchVM : CWTBaseViewModel
    {
        public string FilterHierarchySearchProperty { get; set; }             //ClientSubUnitGuid, ClientSubUnitName
        public string FilterHierarchySearchText { get; set; }
		public string FilterHierarchyCSUSearchText { get; set; }
		public string FilterHierarchyTTSearchText { get; set; }
		public string AvailableHierarchyTypeDisplayName { get; set; }

		public string ClientDefinedRuleGroupName { get; set; }
        public ClientDefinedRuleGroup ClientDefinedRuleGroup { get; set; } 

        public int GroupId { get; set; }                                //ClientFeeId, PolicyGoupId etc
        public string GroupType { get; set; }                           //"ClientFee", "Policy" etc..not the name of the Group
        
        public string HierarchyType { get; set; }                       //Standard Hierarchies or "Multiple"
        public IEnumerable<SelectListItem> HierarchyPropertyOptions { get; set; } //SearchBox with allowed Hierarchies for the Admin to Link to
        public List<spDesktopDataAdmin_SelectClientDefinedRuleGroupLinkedHierarchies_v1Result> LinkedHierarchies { get; set; }
        public List<spDesktopDataAdmin_SelectClientDefinedRuleGroupAvailableHierarchies_v1Result> AvailableHierarchies { get; set; }    //Hierarchies at a chosen level (or at ALL levels)
        public int LinkedHierarchiesTotal { get; set; }  //total amount of Hierarchies Linked
        public bool HasWriteAccess { get; set; }

        public ClientDefinedRuleGroupHierarchySearchVM()
        {
        }

		public ClientDefinedRuleGroupHierarchySearchVM(
			int linkedHierarchiesTotal, 
			ClientDefinedRuleGroup clientDefinedRuleGroup,
			string clientDefinedRuleGroupName, 
			string filterHierarchySearchText, 
			string filterHierarchySearchProperty,
			string filterHierarchyCSUSearchText,
			string filterHierarchyTTSearchText,
			string hierarchyType, IEnumerable<SelectListItem> hierarchyPropertyOptions, 
			int groupId, 
			string groupType,
			bool hasWriteAccess, 
            List<spDesktopDataAdmin_SelectClientDefinedRuleGroupLinkedHierarchies_v1Result> linkedHierarchies,
            List<spDesktopDataAdmin_SelectClientDefinedRuleGroupAvailableHierarchies_v1Result> availableHierarchies)
        {
            LinkedHierarchiesTotal = linkedHierarchiesTotal;
            FilterHierarchySearchProperty = filterHierarchySearchProperty;
            FilterHierarchySearchText = filterHierarchySearchText;
			FilterHierarchyCSUSearchText = filterHierarchyCSUSearchText;
			FilterHierarchyTTSearchText = filterHierarchyTTSearchText;
			HierarchyType = hierarchyType;
            HasWriteAccess = hasWriteAccess;
            LinkedHierarchies = linkedHierarchies;
            AvailableHierarchies = availableHierarchies;
			ClientDefinedRuleGroupName = clientDefinedRuleGroupName;
            ClientDefinedRuleGroup = clientDefinedRuleGroup;
            GroupType = groupType;
            GroupId = groupId;
            HierarchyPropertyOptions = hierarchyPropertyOptions;
        }
    }
}