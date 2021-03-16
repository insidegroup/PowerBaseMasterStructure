using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(TeamOutOfOfficeGroupValidation))]
	public partial class TeamOutOfOfficeGroup : CWTBaseModel
    {        
        public string HierarchyType { get; set; }   //Link to Hierarchy     eg Location or Country
        public string HierarchyItem { get; set; }   //Text Value            eg London or UK
        public string HierarchyCode { get; set; }   //Code                  eg LON or GB

        public string ClientSubUnitGuid { get; set; }   //Name
        public string ClientSubUnitName { get; set; }   //Name

        //only used when a ClientSubUnit is chosen
        public string ClientTopUnitName { get; set; }   //Name
    }

    //DMC - JSON lookups use this
    public partial class TeamOutOfOfficeGroupJSON
    {
        public int TeamOutOfOfficeGroupId { get; set; }
        public string TeamOutOfOfficeGroupName { get; set; }
    }
}
