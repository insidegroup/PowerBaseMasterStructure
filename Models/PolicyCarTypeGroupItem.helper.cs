using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;
namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(PolicyCarTypeGroupItemValidation))]
	public partial class PolicyCarTypeGroupItem : CWTBaseModel
    {
        public string PolicyGroupName { get; set; }
        public string PolicyLocation { get; set; }
        public string PolicyCarStatusDescription { get; set; }
        public string CarTypeCategoryName { get; set; }
    }
}
