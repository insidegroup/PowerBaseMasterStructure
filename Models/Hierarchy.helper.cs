using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;

namespace CWTDesktopDatabase.Models
{
    /*
     * Tables located within Hierarchy have their own helper.cs
     *  eg Location, ClientTopUnit, Team
     */
	public partial class HierarchyJSON
	{
		public string HierarchyName { get; set; }       //London
		public string HierarchyCode { get; set; }       //LON
		public string HierarchyType { get; set; }       //Location
		public string ParentName { get; set; }          //United Kingdom
		public string GrandParentName { get; set; }     //Europe
		public string ClientSubUnitGuid { get; set; }
		public string ClientSubUnitName { get; set; }
		public string ClientTopUnitGuid { get; set; }
		public string ClientTopUnitName { get; set; }
	}

    public partial class ClientAccountJSON
    {
        public string HierarchyName { get; set; }       //John Smith
        public string ClientAccountNumber { get; set; } 
        public string SourceSystemCode { get; set; } 

    }

    public class HierarchyGroup
    {
        public string HierarchyType { get; set; }
        public string HierarchyCode { get; set; }
        public string HierarchyName { get; set; }
        public string HierarchyItem { get; set; }
        public string TravelerTypeGuid { get; set; }
        public string TravelerTypeName { get; set; }
        public string ClientSubUnitGuid { get; set; }
        public string ClientSubUnitName { get; set; }
        public string ClientTopUnitName { get; set; }
        public string SourceSystemCode { get; set; }
    }

    //p/ublic class ClientSubUnitTravelerTypeJSON
    //{
    //    public string HierarchyName { get; set; }
    //    public string ClientSubUnitGuid { get; set; }
    //    public string TravelerTypeGuid { get; set; }
    //}


}
