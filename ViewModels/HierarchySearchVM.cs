using System.Collections.Generic;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
    public class HierarchySearchVM : CWTBaseViewModel
    {
        public string FilterHierarchySearchProperty { get; set; }             //ClientSubUnitGuid, ClientSubUnitName
        public string FilterHierarchySearchText { get; set; }                       

        public string FeeTypeDisplayName { get; set; }
        public ClientFeeGroup ClientFeeGroup { get; set; } 

        public int GroupId { get; set; }                                //ClientFeeId, PolicyGoupId etc
        public string GroupType { get; set; }                           //"ClientFee", "Policy" etc..not the name of the Group
        
        public string HierarchyType { get; set; }                       //Standard Hierarchies or "Multiple"
        public IEnumerable<SelectListItem> HierarchyPropertyOptions { get; set; } //SearchBox with allowed Hierarchies for the Admin to Link to
        public List<spDesktopDataAdmin_SelectClientFeeGroupLinkedHierarchies_v1Result> LinkedHierarchies { get; set; }
        public List<spDesktopDataAdmin_SelectClientFeeGroupAvailableHierarchies_v1Result> AvailableHierarchies { get; set; }    //Hierarchies at a chosen level (or at ALL levels)
        public int LinkedHierarchiesTotal { get; set; }  //total amount of Hierarchies Linked
        public bool HasWriteAccess { get; set; }

        

        public HierarchySearchVM()
        {
        }
        public HierarchySearchVM(int linkedHierarchiesTotal, ClientFeeGroup clientFeeGroup, string feeTypeDisplayName, string filterHierarchySearchText, string filterHierarchySearchProperty, string hierarchyType, IEnumerable<SelectListItem> hierarchyPropertyOptions, int groupId, string groupType, bool hasWriteAccess, 
                                    List<spDesktopDataAdmin_SelectClientFeeGroupLinkedHierarchies_v1Result> linkedHierarchies,
                                    List<spDesktopDataAdmin_SelectClientFeeGroupAvailableHierarchies_v1Result> availableHierarchies)
        {
            LinkedHierarchiesTotal = linkedHierarchiesTotal;
            FilterHierarchySearchProperty = filterHierarchySearchProperty;
            FilterHierarchySearchText = filterHierarchySearchText;
            HierarchyType = hierarchyType;
            HasWriteAccess = hasWriteAccess;
            LinkedHierarchies = linkedHierarchies;
            AvailableHierarchies = availableHierarchies;
            FeeTypeDisplayName = feeTypeDisplayName;
            ClientFeeGroup = clientFeeGroup;
            GroupType = groupType;
            GroupId = groupId;
            HierarchyPropertyOptions = hierarchyPropertyOptions;
        }
    }
}