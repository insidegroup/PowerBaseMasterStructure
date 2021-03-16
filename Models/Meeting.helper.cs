using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(MeetingValidation))]
	public partial class Meeting : CWTBaseModel
    {
		public string MeetingArriveByTime { get; set; }
		public string MeetingLeaveAfterTime { get; set; }

		public City City { get; set; }
		public ClientTopUnit ClientTopUnit { get; set; }

		public string HierarchyType { get; set; }   //Link to Hierarchy     eg Location, CountryRegion, Country, ClientSubUnit etc
        public string HierarchyItem { get; set; }   //Text Value            eg London or UK
        public string HierarchyCode { get; set; }   //Code                  eg LON or GB

		//does this item connect to multiple Hierarchy items
        public bool IsMultipleHierarchy { get; set; }

		public string MeetingDisplayName { get; set; }
    }

    //DMC - JSON lookups use this
    public partial class MeetingJSON
    {
        public int MeetingId { get; set; }
        public string MeetingName { get; set; }
    }



}
