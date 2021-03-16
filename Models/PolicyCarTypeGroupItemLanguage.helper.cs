using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(PolicyCarTypeGroupItemLanguageValidation))]
	public partial class PolicyCarTypeGroupItemLanguage : CWTBaseModel
    {
        public int PolicyGroupId { get; set; }
        public string PolicyGroupName { get; set; }
        public string LanguageName { get; set; }

    }
}
