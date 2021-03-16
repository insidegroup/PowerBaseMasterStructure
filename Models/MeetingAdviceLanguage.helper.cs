using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
	[MetadataType(typeof(MeetingAdviceLanguageValidation))]
	public partial class MeetingAdviceLanguage : CWTBaseModel
    {
		public Language Language { get; set; }
    }
}
