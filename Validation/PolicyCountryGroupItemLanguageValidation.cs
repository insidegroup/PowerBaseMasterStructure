using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using CWTDesktopDatabase.Validation;

namespace CWTDesktopDatabase.Validation
{
    public class PolicyCountryGroupItemLanguageValidation
    {
        [Required(ErrorMessage = "Country Advice Required")]
        public string CountryAdvice { get; set; }

        [Required(ErrorMessage = "Language Required")]
        public string LanguageCode { get; set; }
    }
}
