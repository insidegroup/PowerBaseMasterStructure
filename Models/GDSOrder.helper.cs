using CWTDesktopDatabase.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.Models
{
	[MetadataType(typeof(GDSOrderValidation))]
	public partial class GDSOrder : CWTBaseModel
	{
		public string PseudoCityOrOfficeAddress { get; set; }
		public bool ExpediteFlagNullable { get; set; }
		public IEnumerable<int> GDSThirdPartyVendorIds { get; set; }

		public List<GDSOrderLineItem> GDSOrderLineItemsXML { get; set; }
		public List<GDSThirdPartyVendor> GDSThirdPartyVendors { get; set; }
		public List<GDSThirdPartyVendor> PseudoCityOrOfficeMaintenanceGDSThirdPartyVendors { get; set; }

		public Country OrderAnalystCountry { get; set; }
		public Country RequesterCountry { get; set; }

		public bool ShowDataFlag { get; set; }

		//PseudoCityOrOfficeMaintenance for separate Validation
		public string PseudoCityOrOfficeId { get; set; }
		public int? PseudoCityOrOfficeMaintenance_IATAId { get; set; }
		public string PseudoCityOrOfficeMaintenance_GDSCode { get; set; }
		public int? PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeAddressId { get; set; }
		public string PseudoCityOrOfficeMaintenance_LocationContactName { get; set; }
		public string PseudoCityOrOfficeMaintenance_LocationPhone { get; set; }
		public string PseudoCityOrOfficeMaintenance_CountryName { get; set; }
		public string PseudoCityOrOfficeMaintenance_GlobalRegionName { get; set; }
		public int? PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeDefinedRegionId { get; set; }

		public bool PseudoCityOrOfficeMaintenance_ActiveFlagNonNullable { get; set; }
		public string PseudoCityOrOfficeMaintenance_InternalSiteName { get; set; }
		
		public int? PseudoCityOrOfficeMaintenance_ExternalNameId { get; set; }
		public ExternalName PseudoCityOrOfficeMaintenance_ExternalName { get; set; }

		public int? PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeTypeId { get; set; }
		public PseudoCityOrOfficeType PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeType { get; set; }

		public int? PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeLocationTypeId { get; set; }
		public PseudoCityOrOfficeLocationType PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeLocationType { get; set; }

		public int? PseudoCityOrOfficeMaintenance_FareRedistributionId { get; set; }
		public FareRedistribution PseudoCityOrOfficeMaintenance_FareRedistribution { get; set; }
		
		public bool PseudoCityOrOfficeMaintenance_SharedPseudoCityOrOfficeFlagNonNullable { get; set; }
		public bool PseudoCityOrOfficeMaintenance_CWTOwnedPseudoCityOrOfficeFlagNonNullable { get; set; }
		public bool PseudoCityOrOfficeMaintenance_ClientDedicatedPseudoCityOrOfficeFlagNonNullable { get; set; }
		public bool PseudoCityOrOfficeMaintenance_ClientGDSAccessFlagNonNullable { get; set; }
		public bool PseudoCityOrOfficeMaintenance_DevelopmentOrInternalPseudoCityOrOfficeFlagNonNullable { get; set; }
		public bool PseudoCityOrOfficeMaintenance_CubaPseudoCityOrOfficeFlagNonNullable { get; set; }
		public bool PseudoCityOrOfficeMaintenance_GovernmentPseudoCityOrOfficeFlagNonNullable { get; set; }
		public bool PseudoCityOrOfficeMaintenance_GDSThirdPartyVendorFlagNonNullable { get; set; }

		//Email
		public string EmailFromAddress { get; set; }
		public string EmailToAddress { get; set; }
		public List<string> EmailToAddresses { get; set; }
	}

	public partial class GDSOrderExport : GDSOrder
	{
		public DateTime? CreationTimeStamp { get; set; }
		public DateTime? LastUpdateTimeStamp { get; set; }
	}

	public class GDSOrderAnalyst
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string CountryCode { get; set; }
	}
}
