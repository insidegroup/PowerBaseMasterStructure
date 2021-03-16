using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(ControlValueTranslationValidation))]
	public partial class ControlValueLanguage : CWTBaseModel
    {
        public string LanguageName { get; set; }
        //public string ControlValue { get; set; }

    }
}