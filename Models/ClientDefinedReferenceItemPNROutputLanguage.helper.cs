using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
	[MetadataType(typeof(ClientDefinedReferenceItemPNROutputLanguageValidation))]
	public partial class ClientDefinedReferenceItemPNROutputLanguage : CWTBaseModel
    {
		public string CurrentLanguageCode { get; set; }
    }
}
