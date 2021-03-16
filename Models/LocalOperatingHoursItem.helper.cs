using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(LocalOperatingHoursItemValidation))]
	public partial class LocalOperatingHoursItem : CWTBaseModel
    {
        public string LocalOperatingHoursGroupName { get; set; }
        public string WeekdayName { get; set; }
        public string OpeningTime { get; set; }
        public string ClosingTime { get; set; }
    }
}
