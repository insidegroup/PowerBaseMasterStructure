using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(PolicyHotelVendorGroupItemLanguageValidation))]
	public partial class PolicyHotelVendorGroupItemLanguage : CWTBaseModel
    {
        public int PolicyGroupId { get; set; }
        public string PolicyGroupName { get; set; }
        public string LanguageName { get; set; }
    }
}
