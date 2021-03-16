using CWTDesktopDatabase.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
	[MetadataType(typeof(PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeIdValidation))]
	public partial class PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeId : CWTBaseModel
	{
        //True/False as Dropdowns
        public string SharedPseudoCityOrOfficeIdFlagSelectedValue { get; set; }
        public IEnumerable<SelectListItem> SharedPseudoCityOrOfficeIdFlagList { get; set; }

    }
}
