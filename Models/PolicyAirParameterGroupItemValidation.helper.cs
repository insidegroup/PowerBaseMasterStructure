using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
	[MetadataType(typeof(PolicyAirParameterGroupItemValidation))]
	public partial class PolicyAirParameterGroupItem : CWTBaseModel
    {
        public string PolicyGroupName { get; set; }
    }
}
