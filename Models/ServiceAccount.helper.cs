using CWTDesktopDatabase.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
	[MetadataType(typeof(ServiceAccountValidation))]
	public partial class ServiceAccount : CWTBaseModel
    {
		public string InternalRemark { get; set; }

        //Nullable Fields
        public bool CubaBookingAllowanceIndicatorNonNullable { get; set; }
        public bool MilitaryAndGovernmentUserFlagNonNullable { get; set; }
    }
}
