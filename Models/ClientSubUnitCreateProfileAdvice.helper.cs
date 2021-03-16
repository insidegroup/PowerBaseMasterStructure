using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(ClientSubUnitCreateProfileAdviceValidation))]
	public partial class ClientSubUnitCreateProfileAdvice : CWTBaseModel
    {
        public string ClientSubUnitDisplayName { get; set; }
        public string LanguageName { get; set; }
    }
}

