using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(PseudoCityOrOfficeMaintenanceValidation))]
	public partial class PseudoCityOrOfficeMaintenance : CWTBaseModel
    {
		public string StateProvinceName { get; set; }
		
		//Readonly on UI
		public string CountryName { get; set; }
		public string CountryCode { get; set; }
		public string GlobalRegionName { get; set; }
		public string GlobalRegionCode { get; set; }

		public bool ActiveFlagNonNullable { get; set; }
		public bool SharedPseudoCityOrOfficeFlagNonNullable { get; set; }
		public bool CWTOwnedPseudoCityOrOfficeFlagNonNullable { get; set; }
		public bool ClientDedicatedPseudoCityOrOfficeFlagNonNullable { get; set; }
		public bool ClientGDSAccessFlagNonNullable { get; set; }
		public bool DevelopmentOrInternalPseudoCityOrOfficeFlagNonNullable { get; set; }
		public bool CubaPseudoCityOrOfficeFlagNonNullable { get; set; }
		public bool GovernmentPseudoCityOrOfficeFlagNonNullable { get; set; }
		public bool GDSThirdPartyVendorFlagNonNullable { get; set; }

		public List<GDSThirdPartyVendor> GDSThirdPartyVendorsList { get; set; }
		public List<ClientSubUnit> ClientSubUnitsList { get; set; }
}

	public partial class PseudoCityOrOfficeMaintenanceReference
	{
		public string TableName { get; set; }
	}

	public partial class PseudoCityOrOfficeMaintenanceJSON
	{
		public string PseudoCityOrOfficeId { get; set; }
		public int PseudoCityOrOfficeMaintenanceId { get; set; }
		public string PseudoCityOrOfficeMaintenanceName { get; set; }
        public bool GDSThirdPartyVendorFlag { get; set; }

        public int PseudoCityOrOfficeAddressId { get; set; }
		public string FirstAddressLine { get; set; }
	}
}
