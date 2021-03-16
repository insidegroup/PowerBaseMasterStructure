using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(LocalOperatingHoursGroupValidation))]
	public partial class LocalOperatingHoursGroup : CWTBaseModel
    {

        public string HierarchyType { get; set; }   //Link to Hierarchy     eg Location or Country
        public string HierarchyItem { get; set; }   //Text Value            eg London or UK
        public string HierarchyCode { get; set; }   //Code                  eg LON or GB

        //only used when ClientSubUnitTravelerType is chosen
        public string TravelerTypeGuid { get; set; }
        public string ClientSubUnitGuid { get; set; }
        public string TravelerTypeName { get; set; }
        public string ClientSubUnitName { get; set; }   //Name

        //only used when a ClientSubUnit is chosen
        public string ClientTopUnitName { get; set; }

        //only used when a ClientAccount is chosen
        public string SourceSystemCode { get; set; }    //CLientAccountNumber is stored in HierarchyCode
    }

    //DMC - JSON lookups use this
    public partial class LocalOperatingHoursGroupJSON
    {
        public int ReasonCodeGroupId { get; set; }
        public string ReasonCodeGroupName { get; set; }
    }



}
