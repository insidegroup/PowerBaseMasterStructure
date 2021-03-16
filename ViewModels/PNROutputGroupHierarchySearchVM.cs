using System.Collections.Generic;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
    public class PNROutputGroupHierarchySearchVM : CWTBaseViewModel
    {
        public string FilterHierarchySearchProperty { get; set; }             //ClientSubUnitGuid, ClientSubUnitName
        public string FilterHierarchySearchText { get; set; }
		public string FilterHierarchyCSUSearchText { get; set; }
		public string FilterHierarchyTTSearchText { get; set; }
		public string AvailableHierarchyTypeDisplayName { get; set; }

		public string PNROutputGroupName { get; set; }
        public PNROutputGroup PNROutputGroup { get; set; } 

        public int GroupId { get; set; }                                //ClientFeeId, PolicyGoupId etc
        public string GroupType { get; set; }                           //"ClientFee", "Policy" etc..not the name of the Group
        
        public string HierarchyType { get; set; }                       //Standard Hierarchies or "Multiple"
        public IEnumerable<SelectListItem> HierarchyPropertyOptions { get; set; } //SearchBox with allowed Hierarchies for the Admin to Link to
        public List<spDesktopDataAdmin_SelectPNROutputGroupLinkedHierarchies_v1Result> LinkedHierarchies { get; set; }
        public List<spDesktopDataAdmin_SelectPNROutputGroupAvailableHierarchies_v1Result> AvailableHierarchies { get; set; }    //Hierarchies at a chosen level (or at ALL levels)
        public int LinkedHierarchiesTotal { get; set; }  //total amount of Hierarchies Linked
        public bool HasWriteAccess { get; set; }

        public PNROutputGroupHierarchySearchVM()
        {
        }

		public PNROutputGroupHierarchySearchVM(
			int linkedHierarchiesTotal, 
			PNROutputGroup pnrOutputGroup,
			string pnrOutputGroupName, 
			string filterHierarchySearchText, 
			string filterHierarchySearchProperty,
			string filterHierarchyCSUSearchText,
			string filterHierarchyTTSearchText,
			string hierarchyType, IEnumerable<SelectListItem> hierarchyPropertyOptions, 
			int groupId, 
			string groupType,
			bool hasWriteAccess, 
            List<spDesktopDataAdmin_SelectPNROutputGroupLinkedHierarchies_v1Result> linkedHierarchies,
            List<spDesktopDataAdmin_SelectPNROutputGroupAvailableHierarchies_v1Result> availableHierarchies)
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
			PNROutputGroupName = pnrOutputGroupName;
            PNROutputGroup = pnrOutputGroup;
            GroupType = groupType;
            GroupId = groupId;
            HierarchyPropertyOptions = hierarchyPropertyOptions;
        }
    }
}