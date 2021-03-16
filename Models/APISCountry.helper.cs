using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(APISCountryValidation))]
	public partial class APISCountry : CWTBaseModel
    {
        public string OriginCountryName { get; set; }
        public string DestinationCountryName { get; set; }
    }
}
