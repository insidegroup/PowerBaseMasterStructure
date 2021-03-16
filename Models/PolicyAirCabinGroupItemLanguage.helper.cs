using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(PolicyAirCabinGroupItemLanguageValidation))]
	public partial class PolicyAirCabinGroupItemLanguage : CWTBaseModel
    {
        public string PolicyGroupName { get; set; }
        public int PolicyGroupId { get; set; }
        public string LanguageName { get; set; }
    }
}
