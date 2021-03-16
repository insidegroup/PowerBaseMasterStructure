using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
	[MetadataType(typeof(ReasonCodeTravelerDescriptionValidation))]
	public partial class ReasonCodeTravelerDescription : CWTBaseModel
    {
        public string ReasonCodeGroupName { get; set; }
        public int ReasonCodeItemDisplayOrder { get; set; }
        public string LanguageName { get; set; }

		public ReasonCodeProductTypeTravelerDescription ReasonCodeProductTypeTravelerDescription { get; set; }
    }
}
