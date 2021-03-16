using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;


namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(ClientDetailValidation))]
	public partial class ClientDetail : CWTBaseModel
     {
        public string TripType { get; set; }

         public string HierarchyType { get; set; }   //Link to Hierarchy     eg Location, CountryRegion, Country, ClientSubUnit etc
         public string HierarchyItem { get; set; }   //Text Value            eg London or UK
         public string HierarchyCode { get; set; }   //Code                  eg LON or GB

         //only used when ClientSubUnitTravelerType is chosen
         public string TravelerTypeGuid { get; set; }    //Code
         public string ClientSubUnitGuid { get; set; }   //Name
         public string TravelerTypeName { get; set; }    //Code
         public string ClientSubUnitName { get; set; }   //Name

         //only used when a ClientAccount is chosen
         public string SourceSystemCode { get; set; }    //CLientAccountNumber is stored in HierarchyCode
     }

    //JSON lookups use this - NOT USED?
    //public partial class ClientDetailJSON
    //{
    //    public string ClientDetailId { get; set; }
    //    public string ClientDetailName { get; set; }
    //}



}

