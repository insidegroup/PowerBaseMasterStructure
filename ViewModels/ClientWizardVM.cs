using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
    
    public class ClientWizardVM : CWTBaseViewModel
    {
        public ClientTopUnit ClientTopUnit { get; set; }
        public ClientSubUnit ClientSubUnit { get; set; }
		public IEnumerable<SelectListItem> ClientSubUnitPortraitStatuses { get; set; }
		public IEnumerable<SelectListItem> ClientSubUnitLineOfBusinesses { get; set; }

        public List<ClientSubUnitTelephony> ClientSubUnitTelephonies { get; set; }
        public List<ClientSubUnitTelephony> ClientSubUnitTelephoniesRemoved { get; set; }
        public List<ClientSubUnitTelephony> ClientSubUnitTelephoniesAdded { get; set; }
        public IEnumerable<SelectListItem> CallerEnteredDigitDefinitionTypes { get; set; }

        public List<ClientSubUnitTeam> TeamsAdded { get; set; }
        public List<ClientSubUnitTeam> TeamsRemoved { get; set; }
        public List<ClientSubUnitTeam> TeamsAltered { get; set; }
        public List<ClientSubUnitClientAccount> ClientAccountsAdded { get; set; }
        public List<ClientSubUnitClientAccount> ClientAccountsRemoved { get; set; }
        public List<ClientSubUnitClientAccount> ClientAccountsAltered { get; set; }
        public List<ServicingOptionItem> ServicingOptionItemsAdded { get; set; }
        public List<ServicingOptionItem> ServicingOptionItemsRemoved { get; set; }
        public List<ServicingOptionItem> ServicingOptionItemsAltered { get; set; }
        public List<ReasonCodeItem> ReasonCodesAdded { get; set; }
		public List<ReasonCodeItem> ReasonCodeItemsRemoved { get; set; }
		public List<ReasonCodeItem> ReasonCodeItemsAltered { get; set; }
        public List<PolicyAirCabinGroupItemViewModel> PolicyAirCabinGroupItemsAdded { get; set; }
        public List<PolicyAirCabinGroupItem> PolicyAirCabinGroupItemsRemoved { get; set; }
        public List<PolicyAirCabinGroupItemViewModel> PolicyAirCabinGroupItemsAltered { get; set; }
        public List<PolicyAirMissedSavingsThresholdGroupItem> PolicyAirMissedSavingsThresholdGroupItemsAdded { get; set; }
        public List<PolicyAirMissedSavingsThresholdGroupItem> PolicyAirMissedSavingsThresholdGroupItemsRemoved { get; set; }
        public List<PolicyAirMissedSavingsThresholdGroupItem> PolicyAirMissedSavingsThresholdGroupItemsAltered { get; set; }
		public List<PolicyAirParameterGroupItemVM> PolicyAirParameterGroupItemsAdded { get; set; }
		public List<PolicyAirParameterGroupItemVM> PolicyAirParameterGroupItemsAltered { get; set; }
		public List<PolicyAirParameterGroupItem> PolicyAirParameterGroupItemsRemoved { get; set; }
		public List<PolicyAirVendorGroupItemVM> PolicyAirVendorGroupItemsAdded { get; set; }
        public List<PolicyAirVendorGroupItem> PolicyAirVendorGroupItemsRemoved { get; set; }
        public List<PolicyAirVendorGroupItemVM> PolicyAirVendorGroupItemsAltered { get; set; }
        public List<PolicyCarTypeGroupItem> PolicyCarTypeGroupItemsAdded { get; set; }
        public List<PolicyCarTypeGroupItem> PolicyCarTypeGroupItemsRemoved { get; set; }
        public List<PolicyCarTypeGroupItem> PolicyCarTypeGroupItemsAltered { get; set; }
        public List<PolicyCarVendorGroupItem> PolicyCarVendorGroupItemsAdded { get; set; }
        public List<PolicyCarVendorGroupItem> PolicyCarVendorGroupItemsRemoved { get; set; }
        public List<PolicyCarVendorGroupItem> PolicyCarVendorGroupItemsAltered { get; set; }
        public List<PolicyCityGroupItem> PolicyCityGroupItemsAdded { get; set; }
        public List<PolicyCityGroupItem> PolicyCityGroupItemsRemoved { get; set; }
        public List<PolicyCityGroupItem> PolicyCityGroupItemsAltered { get; set; }
        public List<PolicyCountryGroupItem> PolicyCountryGroupItemsAdded { get; set; }
        public List<PolicyCountryGroupItem> PolicyCountryGroupItemsRemoved { get; set; }
        public List<PolicyCountryGroupItem> PolicyCountryGroupItemsAltered { get; set; }
        public List<PolicyHotelCapRateGroupItem> PolicyHotelCapRateGroupItemsAdded { get; set; }
        public List<PolicyHotelCapRateGroupItem> PolicyHotelCapRateGroupItemsRemoved { get; set; }
        public List<PolicyHotelCapRateGroupItem> PolicyHotelCapRateGroupItemsAltered { get; set; }
        public List<PolicyHotelPropertyGroupItem> PolicyHotelPropertyGroupItemsAdded { get; set; }
        public List<PolicyHotelPropertyGroupItem> PolicyHotelPropertyGroupItemsRemoved { get; set; }
        public List<PolicyHotelPropertyGroupItem> PolicyHotelPropertyGroupItemsAltered { get; set; }
        public List<PolicyHotelVendorGroupItem> PolicyHotelVendorGroupItemsAdded { get; set; }
        public List<PolicyHotelVendorGroupItem> PolicyHotelVendorGroupItemsRemoved { get; set; }
        public List<PolicyHotelVendorGroupItem> PolicyHotelVendorGroupItemsAltered { get; set; }
        public List<PolicySupplierDealCode> PolicySupplierDealCodesAdded { get; set; }
        public List<PolicySupplierDealCode> PolicySupplierDealCodesRemoved { get; set; }
        public List<PolicySupplierDealCode> PolicySupplierDealCodesAltered { get; set; }
        public List<PolicySupplierServiceInformation> PolicySupplierServiceInformationsAdded { get; set; }
        public List<PolicySupplierServiceInformation> PolicySupplierServiceInformationsRemoved { get; set; }
        public List<PolicySupplierServiceInformation> PolicySupplierServiceInformationsAltered { get; set; }
        public PolicyGroup PolicyGroup { get; set; }
        public bool ReasonCodesInherited { get; set; }
		public bool RestrictedClientAccess { get; set; }
		public bool ComplianceAdministratorAccess { get; set; }

        public ClientWizardVM()
        {
        }
        public ClientWizardVM(
                    ClientTopUnit clientTopUnit,
                    ClientSubUnit clientSubUnit,
                    IEnumerable<SelectListItem> clientSubUnitPortraitStatuses,
                    List<ClientSubUnitTelephony> clientSubUnitTelephonies,
                    List<ClientSubUnitTelephony> clientSubUnitTelephoniesAdded,
                    List<ClientSubUnitTelephony> clientSubUnitTelephoniesRemoved,
                    IEnumerable<SelectListItem> callerEnteredDigitDefinitionTypes,
                    List<ClientSubUnitTeam> teamsAdded,
                    List<ClientSubUnitTeam> teamsRemoved,
                    List<ClientSubUnitTeam> teamsAltered,
                    List<ClientSubUnitClientAccount> clientAccountsAdded,
                    List<ClientSubUnitClientAccount> clientAccountsRemoved,
                    List<ClientSubUnitClientAccount> clientAccountsAltered,
                    List<ServicingOptionItem> servicingOptionItemsAdded,
                    List<ServicingOptionItem> servicingOptionItemsRemoved,
                    List<ServicingOptionItem> servicingOptionItemsAltered,
                    List<ReasonCodeItem> reasonCodesAdded,
                    List<ReasonCodeItem> reasonCodeItemsRemoved,
					List<ReasonCodeItem> reasonCodesItemsAltered,
                    List<PolicyAirCabinGroupItemViewModel> policyAirCabinGroupItemsAdded,
                    List<PolicyAirCabinGroupItem> policyAirCabinGroupItemsRemoved,
                    List<PolicyAirCabinGroupItemViewModel> policyAirCabinGroupItemsAltered,
                    List<PolicyAirMissedSavingsThresholdGroupItem> policyAirMissedSavingsThresholdGroupItemsAdded,
                    List<PolicyAirMissedSavingsThresholdGroupItem> policyAirMissedSavingsThresholdGroupItemsRemoved,
                    List<PolicyAirMissedSavingsThresholdGroupItem> policyAirMissedSavingsThresholdGroupItemsAltered,
					List<PolicyAirParameterGroupItemVM> policyAirParameterGroupItemsAdded,
					List<PolicyAirParameterGroupItemVM> policyAirParameterGroupItemsAltered,
					List<PolicyAirParameterGroupItem> policyAirParameterGroupItemsRemoved,
                    List<PolicyAirVendorGroupItemVM> policyAirVendorGroupItemsAdded,
                    List<PolicyAirVendorGroupItem> policyAirVendorGroupItemsRemoved,
                    List<PolicyAirVendorGroupItemVM> policyAirVendorGroupItemsAltered,
                    List<PolicyCarTypeGroupItem> policyCarTypeGroupItemsAdded,
                    List<PolicyCarTypeGroupItem> policyCarTypeGroupItemsRemoved,
                    List<PolicyCarTypeGroupItem> policyCarTypeGroupItemsAltered,
                    List<PolicyCarVendorGroupItem> policyCarVendorGroupItemsAdded,
                    List<PolicyCarVendorGroupItem> policyCarVendorGroupItemsRemoved,
                    List<PolicyCarVendorGroupItem> policyCarVendorGroupItemsAltered,
                    List<PolicyCityGroupItem> policyCityGroupItemsAdded,
                    List<PolicyCityGroupItem> policyCityGroupItemsRemoved,
                    List<PolicyCityGroupItem> policyCityGroupItemsAltered,
                    List<PolicyCountryGroupItem> policyCountryGroupItemsAdded,
                    List<PolicyCountryGroupItem> policyCountryGroupItemsRemoved,
                    List<PolicyCountryGroupItem> policyCountryGroupItemsAltered,
                    List<PolicyHotelCapRateGroupItem> policyHotelCapRateGroupItemsAdded,
                    List<PolicyHotelCapRateGroupItem> policyHotelCapRateGroupItemsRemoved,
                    List<PolicyHotelCapRateGroupItem> policyHotelCapRateGroupItemsAltered,
                    List<PolicyHotelPropertyGroupItem> policyHotelPropertyGroupItemsAdded,
                    List<PolicyHotelPropertyGroupItem> policyHotelPropertyGroupItemsRemoved,
                    List<PolicyHotelPropertyGroupItem> policyHotelPropertyGroupItemsAltered,
                    List<PolicyHotelVendorGroupItem> policyHotelVendorGroupItemsAdded,
                    List<PolicyHotelVendorGroupItem> policyHotelVendorGroupItemsRemoved,
                    List<PolicyHotelVendorGroupItem> policyHotelVendorGroupItemsAltered,
                    List<PolicySupplierDealCode> policySupplierDealCodesAdded,
                    List<PolicySupplierDealCode> policySupplierDealCodesRemoved,
                    List<PolicySupplierDealCode> policySupplierDealCodesAltered,
                    List<PolicySupplierServiceInformation> policySupplierServiceInformationsAdded,
                    List<PolicySupplierServiceInformation> policySupplierServiceInformationsRemoved,
                    List<PolicySupplierServiceInformation> policySupplierServiceInformationsAltered,
                    PolicyGroup policyGroup,
                    bool reasonCodesInherited,
					bool restrictedClientAccess,
					bool complianceAdministratorAccess
            
            )
        {
            ClientTopUnit = clientTopUnit;
            ClientSubUnit = clientSubUnit;
            ClientSubUnitPortraitStatuses = clientSubUnitPortraitStatuses;
            ClientSubUnitTelephonies = clientSubUnitTelephonies;
            ClientSubUnitTelephoniesAdded = clientSubUnitTelephoniesAdded;
            ClientSubUnitTelephoniesRemoved = clientSubUnitTelephoniesRemoved;
            CallerEnteredDigitDefinitionTypes = callerEnteredDigitDefinitionTypes;
            TeamsAdded = teamsAdded;
            TeamsRemoved = teamsRemoved;
            TeamsAltered = teamsAltered;
            ClientAccountsAdded = clientAccountsAdded;
            ClientAccountsRemoved = clientAccountsRemoved;
            ClientAccountsAltered = clientAccountsAltered;
            ServicingOptionItemsAdded = servicingOptionItemsAdded;
            ServicingOptionItemsRemoved = servicingOptionItemsRemoved;
            ServicingOptionItemsAltered = servicingOptionItemsAltered;
            ReasonCodesAdded = reasonCodesAdded;
            ReasonCodeItemsRemoved = reasonCodeItemsRemoved;
            PolicyAirCabinGroupItemsAdded = policyAirCabinGroupItemsAdded;
            PolicyAirCabinGroupItemsRemoved = policyAirCabinGroupItemsRemoved;
            PolicyAirCabinGroupItemsAltered = policyAirCabinGroupItemsAltered;
            PolicyAirMissedSavingsThresholdGroupItemsAdded = policyAirMissedSavingsThresholdGroupItemsAdded;
            PolicyAirMissedSavingsThresholdGroupItemsRemoved = policyAirMissedSavingsThresholdGroupItemsRemoved;
            PolicyAirMissedSavingsThresholdGroupItemsAltered = policyAirMissedSavingsThresholdGroupItemsAltered;
			PolicyAirParameterGroupItemsAdded = policyAirParameterGroupItemsAdded;
			PolicyAirParameterGroupItemsAltered = policyAirParameterGroupItemsAltered;
			PolicyAirParameterGroupItemsRemoved = policyAirParameterGroupItemsRemoved;
            PolicyAirVendorGroupItemsAdded = policyAirVendorGroupItemsAdded;
            PolicyAirVendorGroupItemsRemoved = policyAirVendorGroupItemsRemoved;
            PolicyAirVendorGroupItemsAltered = policyAirVendorGroupItemsAltered;
            PolicyCarTypeGroupItemsAdded = policyCarTypeGroupItemsAdded;
            PolicyCarTypeGroupItemsRemoved = policyCarTypeGroupItemsRemoved;
            PolicyCarTypeGroupItemsAltered = policyCarTypeGroupItemsAltered;
            PolicyCarVendorGroupItemsAdded = policyCarVendorGroupItemsAdded;
            PolicyCarVendorGroupItemsRemoved = policyCarVendorGroupItemsRemoved;
            PolicyCarVendorGroupItemsAltered = policyCarVendorGroupItemsAltered;
            PolicyCityGroupItemsAdded = policyCityGroupItemsAdded;
            PolicyCityGroupItemsRemoved = policyCityGroupItemsRemoved;
            PolicyCityGroupItemsAltered = policyCityGroupItemsAltered;
            PolicyCountryGroupItemsAdded = policyCountryGroupItemsAdded;
            PolicyCountryGroupItemsRemoved = policyCountryGroupItemsRemoved;
            PolicyCountryGroupItemsAltered = policyCountryGroupItemsAltered;
            PolicyHotelCapRateGroupItemsAdded = policyHotelCapRateGroupItemsAdded;
            PolicyHotelCapRateGroupItemsRemoved = policyHotelCapRateGroupItemsRemoved;
            PolicyHotelCapRateGroupItemsAltered = policyHotelCapRateGroupItemsAltered;
            PolicyHotelPropertyGroupItemsAdded = policyHotelPropertyGroupItemsAdded;
            PolicyHotelPropertyGroupItemsRemoved = policyHotelPropertyGroupItemsRemoved;
            PolicyHotelPropertyGroupItemsAltered = policyHotelPropertyGroupItemsAltered;
            PolicyHotelVendorGroupItemsAdded = policyHotelVendorGroupItemsAdded;
            PolicyHotelVendorGroupItemsRemoved = policyHotelVendorGroupItemsRemoved;
            PolicyHotelVendorGroupItemsAltered = policyHotelVendorGroupItemsAltered;
            PolicySupplierDealCodesAdded = policySupplierDealCodesAdded;
            PolicySupplierDealCodesRemoved = policySupplierDealCodesRemoved;
            PolicySupplierDealCodesAltered = policySupplierDealCodesAltered;
            PolicySupplierServiceInformationsAdded = policySupplierServiceInformationsAdded;
            PolicySupplierServiceInformationsRemoved = policySupplierServiceInformationsRemoved;
            PolicySupplierServiceInformationsAltered = policySupplierServiceInformationsAltered;
            PolicyGroup = policyGroup;
            ReasonCodesInherited = reasonCodesInherited;
			RestrictedClientAccess = restrictedClientAccess;
			ComplianceAdministratorAccess = complianceAdministratorAccess;
        }
    }
}
