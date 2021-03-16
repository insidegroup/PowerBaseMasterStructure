using CWTDesktopDatabase.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(PriceTrackingSetupGroupValidation))]
    public partial class PriceTrackingSetupGroup : CWTBaseModel
    {
        //True/False as Dropdowns
        public string SharedPseudoCityOrOfficeIdFlagSelectedValue { get; set; }
        public string MidOfficeUsedForQCTicketingFlagSelectedValue { get; set; }
        public string USGovernmentContractorFlagSelectedValue { get; set; }

        //XML
        public List<PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeId> PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeIdsXML { get; set; }
        public List<PriceTrackingSetupGroupExcludedTravelerType> PriceTrackingSetupGroupExcludedTravelerTypesXML { get; set; }

        //Hierarchies
        public string HierarchyType { get; set; }    //Link to Hierarchy     eg Location or Country
        public string HierarchyItem { get; set; }   //Text Value            eg London or UK
        public string HierarchyCode { get; set; }   //Code                  eg LON or GB

        //only used when ClientSubUnitTravelerType is chosen
        public string TravelerTypeGuid { get; set; }
        public string ClientSubUnitGuid { get; set; }
        public string TravelerTypeName { get; set; }
        public string ClientSubUnitName { get; set; }

        //only used when a ClientSubUnit is chosen
        public string ClientTopUnitName { get; set; }
        public string ClientTopUnitGuid { get; set; }

        //only used when a ClientAccount is chosen
        public string SourceSystemCode { get; set; }    //CLientAccountNumber is stored in HierarchyCode

        //does this item connect to multiple Hierarchy items
        public bool IsMultipleHierarchy { get; set; }
        public List<ClientSubUnit> ClientSubUnitsHierarchy { get; set; }

        //GDS
        public GDS GDS { get; set; }

        //TripType
        public TripType TripType { get; set; }

        //DesktopUsedType
        public DesktopUsedType DesktopUsedType { get; set; }

        //BackOfficeSystem
        public BackOfficeSystem BackOfficeSystem { get; set; }

        //PriceTrackingBillingModels
        public PriceTrackingBillingModel AirPriceTrackingBillingModel { get; set; }
        public PriceTrackingBillingModel HotelPriceTrackingBillingModel { get; set; }
        public PriceTrackingBillingModel PreTicketPriceTrackingBillingModel { get; set; }
    }

    public partial class PriceTrackingSetupGroupClientAccountJSON
    {
        public string ClientAccountName { get; set; }
        public string ClientAccountNumber { get; set; }
        public string SourceSystemCode { get; set; }
        public string ClientSubUnitGuid { get; set; }
        public string ClientSubUnitName { get; set; }
        public string ClientMasterCode { get; set; }
    }

    public partial class PriceTrackingSetupGroupTravelerTypeJSON
    {
        public string TravelerTypeName { get; set; }
        public string TravelerTypeGuid { get; set; }
        public string ParentName { get; set; }
        public string GrandParentName { get; set; }

    }
}
