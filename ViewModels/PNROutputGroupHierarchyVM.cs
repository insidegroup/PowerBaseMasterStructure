using System.Collections.Generic;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
	public class PNROutputGroupHierarchyVM : CWTBaseViewModel
    {
        public string FilterHierarchySearchProperty { get; set; }             //ClientSubUnitGuid, ClientSubUnitName
        public string FilterHierarchySearchText { get; set; }
		public string FilterHierarchyCSUSearchText { get; set; }
		public string FilterHierarchyTTSearchText { get; set; }
		
		public int GroupId { get; set; }       //Id of group
        public string GroupType { get; set; }     //ClientFee, Policy


        public string HierarchyName { get; set; }       //London, "CWT US SubuUnit"
        public string HierarchyCode { get; set; }       //LON, "A:1234" etc
        public string HierarchyType { get; set; }       //Location, ClientSubUnit etc

        //only used when ClientSubUnitTravelerType is chosen (HierarchyCode, HierarchyName not used)
        public string TravelerTypeGuid { get; set; }    //Code
        public string ClientSubUnitGuid { get; set; }   //Name
        public string TravelerTypeName { get; set; }    //Code
        public string ClientSubUnitName { get; set; }   //Name

        //only used when a ClientAccount is chosen (HierarchyCode not used)
        public string SourceSystemCode { get; set; }  
        public string ClientAccountNumber { get; set; } 


        public PNROutputGroupHierarchyVM()
        {
        }

		public PNROutputGroupHierarchyVM(
			string filterHierarchySearchText,
			string filterHierarchySearchProperty,
			string filterHierarchyCSUSearchText,
			string filterHierarchyTTSearchText,
			string hierarchyName,
            string hierarchyCode,
            string hierarchyType,
            string travelerTypeGuid,
            string clientSubUnitGuid,
            string travelerTypeName,
            string clientSubUnitName,
            string sourceSystemCode,
            string clientAccountNumber)
        {
			FilterHierarchySearchProperty = filterHierarchySearchProperty;
			FilterHierarchySearchText = filterHierarchySearchText;
			FilterHierarchyCSUSearchText =filterHierarchyCSUSearchText;
			FilterHierarchyTTSearchText = filterHierarchyTTSearchText;
            HierarchyType = hierarchyType;
            HierarchyName = hierarchyName;
            HierarchyCode = hierarchyCode;
            HierarchyType = hierarchyType;
            TravelerTypeGuid = travelerTypeGuid;
            ClientSubUnitGuid = clientSubUnitGuid;
            TravelerTypeName = travelerTypeName;
            ClientSubUnitName = clientSubUnitName;
            SourceSystemCode = sourceSystemCode;
            ClientAccountNumber = clientAccountNumber;
        }
    }
}