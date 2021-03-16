using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(TeamValidation))]
	public partial class Team : CWTBaseModel
    {
        public string TeamTypeDescription { get; set; }  

        public string HierarchyType { get; set; }   //Link to Hierarchy     eg Location or Country
        public string HierarchyItem { get; set; }   //Text Value            eg London or UK
        public string HierarchyCode { get; set; }   //Code                  eg LON or GB

        //only used when ClientSubUnitTravelerType is chosen
        public string TravelerTypeGuid { get; set; }    //Code
        public string ClientSubUnitGuid { get; set; }   //Name
        public string TravelerTypeName { get; set; }    //Code
        public string ClientSubUnitName { get; set; }

        //only used when a ClientAccount is chosen
        public string SourceSystemCode { get; set; }    //CLientAccountNumber is stored in HierarchyCode

        //used when Deleting a team
        public int SystemUserCount { get; set; }
        public int ClientSubUnitCount { get; set; }
        public List<spDDAWizard_SelectTeamSystemUsers_v1Result> SystemUsers { get; set; }
        public List<spDDAWizard_SelectTeamClientSubUnits_v1Result> ClientSubUnits { get; set; }
    }

    //DMC - JSON lookups use this
    public partial class TeamJSON
    {
        public int TeamCode { get; set; }
        public string TeamName { get; set; }
    }



}
