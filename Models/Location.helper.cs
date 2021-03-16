using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(LocationValidation))]
	public partial class Location : CWTBaseModel
    {
        public string CountryRegionName { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }

    }
}
