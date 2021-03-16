using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(ReasonCodeItemValidation))]
	public partial class ReasonCodeItem : CWTBaseModel
    {
		public string ReasonCodeDescription { get; set; }
        public string ReasonCodeGroupName { get; set; }
        public string ReasonCodeTypeDescription { get; set; }
        public string ProductName { get; set; }

    }
    public partial class ReasonCode
    {
        public string ReasonCodeValue { get; set; }

    }
}