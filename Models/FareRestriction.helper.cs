using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(FareRestrictionValidation))]
	public partial class FareRestriction : CWTBaseModel
    {
        public string LanguageName { get; set; }
        public string ProductName { get; set; }
        public string SupplierName { get; set; }
    }
}
