using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(ServicingOptionItemValidation))]
	public partial class ServicingOptionItem : CWTBaseModel
    {
        public string ServicingOptionGroupName { get; set; }
        public string ServicingOptionName { get; set; }
		public string GDSName { get; set; }
		public bool? GDSRequiredFlag { get; set; }

		//ServicingOptionFareCalculations
		public int? DepartureTimeWindowMinutes { get; set; }
		public int? ArrivalTimeWindowMinutes { get; set; }
		public int? MaximumConnectionTimeMinutes { get; set; }
		public int? MaximumStops { get; set; }
		public bool? UseAlternateAirportFlag { get; set; }
		public bool? NoPenaltyFlag { get; set; }
		public bool? NoRestrictionsFlag { get; set; }
        public string Source { get; internal set; }
        public string SourceName { get; internal set; }
    }

    public partial class ServicingOptionItemValueJSON
    {
        public int ServicingOptionId { get; set; }
        public string ServicingOptionItemValue { get; set; }     
    }
}
