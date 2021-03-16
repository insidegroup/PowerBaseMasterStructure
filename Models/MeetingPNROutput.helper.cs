using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
	[MetadataType(typeof(MeetingPNROutputValidation))]
	public partial class MeetingPNROutput : CWTBaseModel
    {
		public GDS GDS { get; set; }
		public Language Language { get; set; }
		public Country Country { get; set; }

		public PNROutputRemarkType PNROutputRemarkType { get; set; }
    }
}
