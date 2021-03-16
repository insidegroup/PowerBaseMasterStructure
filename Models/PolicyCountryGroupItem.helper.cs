using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(PolicyCountryGroupItemValidation))]
	public partial class PolicyCountryGroupItem : CWTBaseModel
    {
        public string PolicyGroupName { get; set; }
        public string PolicyCountryStatusDescription { get; set; }
        public string CountryName { get; set; }
    }
}
