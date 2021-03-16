using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(ReasonCodeGroupValidation))]
	public partial class ReasonCodeGroup : CWTBaseModel
    {
        public string TripType { get; set; }        //Text Value ofTripTypeId
        
        public string HierarchyType { get; set; }   //Link to Hierarchy     eg Location or Country
        public string HierarchyItem { get; set; }   //Text Value            eg London or UK
        public string HierarchyCode { get; set; }   //Code                  eg LON or GB

        //only used when ClientSubUnitTravelerType is chosen
        public string TravelerTypeGuid { get; set; }    //Code
        public string ClientSubUnitGuid { get; set; }   //Name
        public string TravelerTypeName { get; set; }    //Code
        public string ClientSubUnitName { get; set; }   //Name

        //only used when a ClientSubUnit is chosen
        public string ClientTopUnitName { get; set; }

        //only used when a ClientAccount is chosen
        public string SourceSystemCode { get; set; }    //CLientAccountNumber is stored in HierarchyCode
        
        //does this item connect to multiple Hierarchy items
        public bool IsMultipleHierarchy { get; set; }
        public List<ClientSubUnit> ClientSubUnitsHierarchy { get; set; }
    }

    //DMC - JSON lookups use this
    public partial class ReasonCodeGroupJSON
    {
        public int ReasonCodeGroupId { get; set; }
        public string ReasonCodeGroupName { get; set; }
    }

     

}
