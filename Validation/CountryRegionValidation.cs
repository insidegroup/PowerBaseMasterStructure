using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using CWTDesktopDatabase.Validation;

namespace CWTDesktopDatabase.Validation
{
    public class CountryRegionValidation
    {
        [Required(ErrorMessage = "CountryRegion Required")]
        public string CountryRegionName { get; set; }

        [Required(ErrorMessage = "Country Required")]
        public string CountryCode { get; set; }
    }
}
