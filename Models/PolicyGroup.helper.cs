using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(PolicyGroupValidation))]
	public partial class PolicyGroup : CWTBaseModel
    {
        public int SequenceNumber { get; set; }  //from Hierarchy Link Table
        public string TripType { get; set; }        //Text Value of TripTypeId
        
        public string HierarchyType { get; set; }    //Link to Hierarchy     eg Location or Country
        public string HierarchyItem { get; set; }   //Text Value            eg London or UK
        public string HierarchyCode { get; set; }   //Code                  eg LON or GB

        //only used when ClientSubUnitTravelerType is chosen
        public string TravelerTypeGuid { get; set; }
        public string ClientSubUnitGuid { get; set; }
        public string TravelerTypeName { get; set; }
        public string ClientSubUnitName { get; set; }

        //only used when a ClientSubUnit is chosen
        public string ClientTopUnitName { get; set; }

        //only used when a ClientAccount is chosen
        public string SourceSystemCode { get; set; }    //CLientAccountNumber is stored in HierarchyCode

        //does this item connect to multiple Hierarchy items
        public bool IsMultipleHierarchy { get; set; }
        public List<ClientSubUnit> ClientSubUnitsHierarchy { get; set; }

        public Meeting Meeting { get; set; }
    }

    //DMC - JSON lookups use this
    public partial class PolicyGroupJSON
    {
        public int PolicyGroupId { get; set; }
        public string PolicyGroupName { get; set; }
    }

     

}
