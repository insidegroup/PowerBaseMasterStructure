using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
  
    public class ClientSubUnitPoliciesVM : CWTBaseViewModel
    {
        public ClientSubUnit ClientSubUnit { get; set; }
        public PolicyGroup PolicyGroup { get; set; }
        public bool Inherited { get; set; }
        public bool HasPolicyGroupWriteAccess { get; set; }
        public List<spDDAWizard_SelectClientSubUnitPolicyAirCabinGroupItems_v1Result> PolicyAirCabinGroupItems { get; set; }
        public List<spDDAWizard_SelectClientSubUnitPolicyAirMSTGroupItems_v1Result> PolicyAirMissedSavingsThresholdGroupItems { get; set; }
        public List<spDDAWizard_SelectClientSubUnitPolicyAirVendorGroupItems_v1Result> PolicyAirVendorGroupItems { get; set; }
        public List<spDDAWizard_SelectClientSubUnitPolicyCarTypeGroupItems_v1Result> PolicyCarTypeGroupItems { get; set; }
        public List<spDDAWizard_SelectClientSubUnitPolicyCarVendorGroupItems_v1Result> PolicyCarVendorGroupItems { get; set; }
        public List<spDDAWizard_SelectClientSubUnitPolicyCityGroupItems_v1Result> PolicyCityGroupItems { get; set; }
        public List<spDDAWizard_SelectClientSubUnitPolicyCountryGroupItems_v1Result> PolicyCountryGroupItems { get; set; }
        public List<spDDAWizard_SelectClientSubUnitPolicyHotelCapRateGroupItems_v1Result> PolicyHotelCapRateGroupItems { get; set; }
        public List<spDDAWizard_SelectClientSubUnitPolicyHotelPropertyGroupItems_v1Result> PolicyHotelPropertyGroupItems { get; set; }
        public List<spDDAWizard_SelectClientSubUnitPolicyHotelVendorGroupItems_v1Result> PolicyHotelVendorGroupItems { get; set; }
        public List<spDDAWizard_SelectClientSubUnitPolicySupplierDealCodes_v1Result> PolicySupplierDealCodes { get; set; }
        public List<spDDAWizard_SelectClientSubUnitPolicySupplierServiceInformations_v1Result> PolicySupplierServiceInformations { get; set; }
		public List<spDDAWizard_SelectClientSubUnitPolicyAirParameterGroupItems_v1Result> PolicyAirAdvancePurchaseGroupItems { get; set; }
		public List<spDDAWizard_SelectClientSubUnitPolicyAirParameterGroupItems_v1Result> PolicyAirTimeWindowGroupItems { get; set; }

		//Policies Display Order
		public bool? Policy24HSCOtherGroupItemDisplayFlag { get; set; }
		public bool? PolicyAirCabinGroupItemDisplayFlag { get; set; }
		public bool? PolicyAirMissedSavingsThresholdGroupItemDisplayFlag { get; set; }
		public bool? PolicyAirTimeWindowGroupItemDisplayFlag { get; set; }
		public bool? PolicyAirAdvancePurchaseGroupItemDisplayFlag { get; set; }
		public bool? PolicyAirOtherGroupItemDisplayFlag { get; set; }
		public bool? PolicyAirParameterGroupItemDisplayFlag { get; set; }
		public bool? PolicyAirVendorGroupItemDisplayFlag { get; set; }
		public bool? PolicyAllOtherGroupItemDisplayFlag { get; set; }
		public bool? PolicyCarOtherGroupItemDisplayFlag { get; set; }
		public bool? PolicyCarTypeGroupItemDisplayFlag { get; set; }
		public bool? PolicyCarVendorGroupItemDisplayFlag { get; set; }
		public bool? PolicyCityGroupItemDisplayFlag { get; set; }
		public bool? PolicyCountryGroupItemDisplayFlag { get; set; }
		public bool? PolicyHotelCapRateGroupItemDisplayFlag { get; set; }
		public bool? PolicyHotelOtherGroupItemDisplayFlag { get; set; }
		public bool? PolicyHotelPropertyGroupItemDisplayFlag { get; set; }
		public bool? PolicyHotelVendorGroupItemDisplayFlag { get; set; }
		public bool? PolicyMessageGroupItemDisplayFlag { get; set; }
		public bool? PolicyOtherGroupItemDisplayFlag { get; set; }
		public bool? PolicySupplierDealCodeDisplayFlag { get; set; }
		public bool? PolicySupplierServiceInformationDisplayFlag { get; set; }

		//Policies Titles
		public string Policy24HSCOtherGroupItemDisplayTitle { get; set; }
		public string PolicyAirCabinGroupItemDisplayTitle { get; set; }
		public string PolicyAirMissedSavingsThresholdGroupItemDisplayTitle { get; set; }
		public string PolicyAirTimeWindowGroupItemDisplayTitle { get; set; }
		public string PolicyAirAdvancePurchaseGroupItemDisplayTitle { get; set; }
		public string PolicyAirOtherGroupItemDisplayTitle { get; set; }
		public string PolicyAirParameterGroupItemDisplayTitle { get; set; }
		public string PolicyAirVendorGroupItemDisplayTitle { get; set; }
		public string PolicyAllOtherGroupItemDisplayTitle { get; set; }
		public string PolicyCarOtherGroupItemDisplayTitle { get; set; }
		public string PolicyCarTypeGroupItemDisplayTitle { get; set; }
		public string PolicyCarVendorGroupItemDisplayTitle { get; set; }
		public string PolicyCityGroupItemDisplayTitle { get; set; }
		public string PolicyCountryGroupItemDisplayTitle { get; set; }
		public string PolicyHotelCapRateGroupItemDisplayTitle { get; set; }
		public string PolicyHotelOtherGroupItemDisplayTitle { get; set; }
		public string PolicyHotelPropertyGroupItemDisplayTitle { get; set; }
		public string PolicyHotelVendorGroupItemDisplayTitle { get; set; }
		public string PolicyMessageGroupItemDisplayTitle { get; set; }
		public string PolicyOtherGroupItemDisplayTitle { get; set; }
		public string PolicySupplierDealCodeDisplayTitle { get; set; }
		public string PolicySupplierServiceInformationDisplayTitle { get; set; }
        
        public ClientSubUnitPoliciesVM()
        {
            this.PolicyAirCabinGroupItems = new List<spDDAWizard_SelectClientSubUnitPolicyAirCabinGroupItems_v1Result>();
            this.PolicyAirMissedSavingsThresholdGroupItems = new List<spDDAWizard_SelectClientSubUnitPolicyAirMSTGroupItems_v1Result>();
            this.PolicyAirVendorGroupItems = new List<spDDAWizard_SelectClientSubUnitPolicyAirVendorGroupItems_v1Result>();
            this.PolicyCarTypeGroupItems = new List<spDDAWizard_SelectClientSubUnitPolicyCarTypeGroupItems_v1Result>();
            this.PolicyCarVendorGroupItems = new List<spDDAWizard_SelectClientSubUnitPolicyCarVendorGroupItems_v1Result>();
            this.PolicyCityGroupItems = new List<spDDAWizard_SelectClientSubUnitPolicyCityGroupItems_v1Result>();
            this.PolicyCountryGroupItems = new List<spDDAWizard_SelectClientSubUnitPolicyCountryGroupItems_v1Result>();
            this.PolicyHotelCapRateGroupItems = new List<spDDAWizard_SelectClientSubUnitPolicyHotelCapRateGroupItems_v1Result>();
            this.PolicyHotelPropertyGroupItems = new List<spDDAWizard_SelectClientSubUnitPolicyHotelPropertyGroupItems_v1Result>();
            this.PolicyHotelVendorGroupItems = new List<spDDAWizard_SelectClientSubUnitPolicyHotelVendorGroupItems_v1Result>();
            this.PolicySupplierDealCodes = new List<spDDAWizard_SelectClientSubUnitPolicySupplierDealCodes_v1Result>();
            this.PolicySupplierServiceInformations = new List<spDDAWizard_SelectClientSubUnitPolicySupplierServiceInformations_v1Result>();
			this.PolicyAirAdvancePurchaseGroupItems = new List<spDDAWizard_SelectClientSubUnitPolicyAirParameterGroupItems_v1Result>();
			this.PolicyAirTimeWindowGroupItems = new List<spDDAWizard_SelectClientSubUnitPolicyAirParameterGroupItems_v1Result>();
        }

        public ClientSubUnitPoliciesVM(
            ClientSubUnit clientSubUnit,
            PolicyGroup policyGroup,
            bool hasPolicyGroupWriteAccess,
            bool inherited,
            List<spDDAWizard_SelectClientSubUnitPolicyAirCabinGroupItems_v1Result> policyAirCabinGroupItems,
            List<spDDAWizard_SelectClientSubUnitPolicyAirMSTGroupItems_v1Result> policyAirMissedSavingsThresholdGroupItems,
            List<spDDAWizard_SelectClientSubUnitPolicyAirVendorGroupItems_v1Result> policyAirVendorGroupItems,
            List<spDDAWizard_SelectClientSubUnitPolicyCarTypeGroupItems_v1Result> policyCarTypeGroupItems,
            List<spDDAWizard_SelectClientSubUnitPolicyCarVendorGroupItems_v1Result> policyCarVendorGroupItems,
            List<spDDAWizard_SelectClientSubUnitPolicyCityGroupItems_v1Result> policyCityGroupItems,
            List<spDDAWizard_SelectClientSubUnitPolicyCountryGroupItems_v1Result> policyCountryGroupItems,
            List<spDDAWizard_SelectClientSubUnitPolicyHotelCapRateGroupItems_v1Result> policyHotelCapRateGroupItems,
            List<spDDAWizard_SelectClientSubUnitPolicyHotelPropertyGroupItems_v1Result> policyHotelPropertyGroupItems,
            List<spDDAWizard_SelectClientSubUnitPolicyHotelVendorGroupItems_v1Result> policyHotelVendorGroupItems,
            List<spDDAWizard_SelectClientSubUnitPolicySupplierDealCodes_v1Result> policySupplierDealCodes,
            List<spDDAWizard_SelectClientSubUnitPolicySupplierServiceInformations_v1Result> policySupplierServiceInformations,
    		List<spDDAWizard_SelectClientSubUnitPolicyAirParameterGroupItems_v1Result> policyAirAdvancePurchaseGroupItems,
			List<spDDAWizard_SelectClientSubUnitPolicyAirParameterGroupItems_v1Result> policyAirTimeWindowGroupItems
        )
        {
            ClientSubUnit = clientSubUnit;
            PolicyGroup = policyGroup;
            HasPolicyGroupWriteAccess = hasPolicyGroupWriteAccess;
            Inherited = inherited;
            PolicyAirCabinGroupItems = policyAirCabinGroupItems;
            PolicyAirMissedSavingsThresholdGroupItems = policyAirMissedSavingsThresholdGroupItems;
            PolicyAirVendorGroupItems = policyAirVendorGroupItems;
            PolicyCarTypeGroupItems = policyCarTypeGroupItems;
            PolicyCarVendorGroupItems = policyCarVendorGroupItems;
            PolicyCityGroupItems = policyCityGroupItems;
            PolicyCountryGroupItems = policyCountryGroupItems;
            PolicyHotelCapRateGroupItems = policyHotelCapRateGroupItems;
            PolicyHotelPropertyGroupItems = policyHotelPropertyGroupItems;
            PolicyHotelVendorGroupItems = policyHotelVendorGroupItems;
            PolicySupplierDealCodes = policySupplierDealCodes;
            PolicySupplierServiceInformations = policySupplierServiceInformations;
			PolicyAirAdvancePurchaseGroupItems = policyAirAdvancePurchaseGroupItems;
			PolicyAirTimeWindowGroupItems = policyAirTimeWindowGroupItems;
        }
    }
}
