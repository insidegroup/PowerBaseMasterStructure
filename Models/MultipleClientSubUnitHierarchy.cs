using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
    public class MultipleClientSubUnitHierarchy
    {
        public string ClientSubUnitGuid { get; set; }
        public string ClientSubUnitName { get; set; }
        public string Location { get; set; }
    }

    public class MultipleHierarchy
    {
        public string Name { get; set; }
        public string ParentName { get; set; }
        public string GrandParentName { get; set; }
    }

    public class MultipleHierarchyDefinition
    {
        public string HierarchyType { get; set; }   //Link to Hierarchy     eg Location, CountryRegion, Country, ClientSubUnit etc
        public string HierarchyItem { get; set; }   //Text Value            eg London or UK
        public string HierarchyCode { get; set; }   //Code                  eg LON or GB

        public string TravelerTypeGuid { get; set; }    //Code
        public string ClientSubUnitGuid { get; set; }   //Name
        public string TravelerTypeName { get; set; }    //Code
        public string ClientSubUnitName { get; set; }   //Name

        //only used when a ClientAccount is chosen
        public string SourceSystemCode { get; set; }    //CLientAccountNumber is stored in HierarchyCode

        //only used when a ClientSubUnit is chosen
        public string ClientTopUnitName { get; set; }
    }
}