using CWTDesktopDatabase.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
	[MetadataType(typeof(ThirdPartyUserValidation))]
	public partial class ThirdPartyUser : CWTBaseModel
	{
		public string InternalRemark { get; set; }
	
		//Nullable Fields
		public bool IsActiveFlagNonNullable { get; set; }
		public bool RoboticUserFlagNonNullable { get; set; }
		public bool CubaBookingAllowanceIndicatorNonNullable { get; set; }
		public bool MilitaryAndGovernmentUserFlagNonNullable { get; set; }

		//AutoComplets and Exports
		public string PartnerName { get; set; }
		public string CountryName { get; set; }
		public string ClientTopUnitName { get; set; }
		public string ClientSubUnitName { get; set; }

		public StateProvince StateProvince { get; set; }

		public List<Entitlement> Entitlements { get; set; }
	}

	public partial class ThirdPartyUserExport : ThirdPartyUser
	{
		public string GDSThirdPartyVendorName { get; set; }
		public string ThirdPartyUserTypeName { get; set; }

		public DateTime? CreationTimeStamp { get; set; }
		public DateTime? LastUpdateTimeStamp { get; set; }
	}
}
