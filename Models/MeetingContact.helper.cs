using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(MeetingContactValidation))]
	public partial class MeetingContact : CWTBaseModel
    {
		public ContactType ContactType { get; set; }
		public Country Country { get; set; }
    }
}