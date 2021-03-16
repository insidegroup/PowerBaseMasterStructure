using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using System.Data.SqlClient;
using System.Globalization;

namespace CWTDesktopDatabase.Repository
{
    public class ClientWizardRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());
        DateTimeFormatInfo myDTFI = new CultureInfo( "en-US", false ).DateTimeFormat;
        CultureInfo provider = CultureInfo.InvariantCulture;
        HierarchyRepository hierarchyRepository = new HierarchyRepository();

        //ClientTopUnits And ClientSubUnits - Filtered
        public List<spDDAWizard_SelectClientTopUnitSubUnitFiltered_v1Result> GetClientTopUnitAndSubUnits(string filter, string filterField)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            return db.spDDAWizard_SelectClientTopUnitSubUnitFiltered_v1(filter, filterField, adminUserGuid).ToList();
        }

        //List of Filtered Teams
        public List<spDDAWizard_SelectTeamsFiltered_v1Result> GetTeams(string filter, string filterField)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            return db.spDDAWizard_SelectTeamsFiltered_v1(adminUserGuid, filter, filterField).ToList();
        }

        //List of Filtered ClienAccounts
        public List<spDDAWizard_SelectClientAccountsFiltered_v1Result> GetClientAccountsFiltered(string filterField1, string filter1, string filterField2, string filter2, string filterField3, string filter3)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            return db.spDDAWizard_SelectClientAccountsFiltered_v1(filterField1, filter1, filterField2, filter2, filterField3, filter3).ToList();
        }

        //List of ClientAccounts for a ClientSubUnit
        public List<spDDAWizard_SelectClientSubUnitClientAccounts_v1Result> GetClientSubUnitClientAccounts(string clientSubUnitGuid)
        {
            return db.spDDAWizard_SelectClientSubUnitClientAccounts_v1(clientSubUnitGuid).ToList();
        }

        //List of ServicingOptions for a ClientSubUnit
        public List<spDDAWizard_SelectClientSubUnitServicingOptions_v1Result> GetClientSubUnitServicingOptions(string clientSubUnitGuid)
        {
            return db.spDDAWizard_SelectClientSubUnitServicingOptions_v1(clientSubUnitGuid).ToList();
        }

        //List of All ServicingOptions
        //public List<spDDAWizard_SelectServicingOptions_v1Result> GetServicingOptions()
        //{
        //    return db.spDDAWizard_SelectServicingOptions_v1().ToList();
		//}

		public ClientSubUnitReasonCodeProductGroup GetProductGroup(string clientSubUnitGuid, int ProductId, int ReasonCodeTypeId, string reasonCodeTypeName)
		{
			ProductRepository productRepository = new ProductRepository();
			ReasonCodeTypeRepository reasonCodeTypeRepository = new ReasonCodeTypeRepository();
			
			var ProductGroup = new ClientSubUnitReasonCodeProductGroup();
			
			//Product
			Product product = productRepository.GetProduct(ProductId);
			if (product != null)
			{
				ProductGroup.Product = product;
			}

			//ReasonCodeType
			ReasonCodeType reasonCodeType = reasonCodeTypeRepository.GetItem(ReasonCodeTypeId);
			if(reasonCodeType != null) {
				if(reasonCodeTypeName != null) {
					reasonCodeType.ReasonCodeTypeName = reasonCodeTypeName;
				}
				ProductGroup.ReasonCodeType = reasonCodeType;
			}

			if (ProductGroup.Product != null && ProductGroup.ReasonCodeType != null)
			{
				ProductGroup.ReasonCodeProductGroups = GetClientSubUnitReasonCodeItemsByProductAndType(
									clientSubUnitGuid, 
									ProductGroup.Product.ProductId, 
									ProductGroup.ReasonCodeType.ReasonCodeTypeId);
				
			}

			return ProductGroup;
		}

        //ReasonCodes of a ClientSubUnit
        public ClientSubUnitReasonCodesVM GetClientSubUnitReasonCodes(string clientSubUnitGuid)
        {

			ClientSubUnitReasonCodesVM clientSubUnitReasonCodesVM = new ClientSubUnitReasonCodesVM();
			clientSubUnitReasonCodesVM.ClientSubUnitReasonCodeProductGroup = new List<ClientSubUnitReasonCodeProductGroup>();

			List<spDDAWizard_SelectClientSubUnitReasonCodesTypes_v1Result> reasonCodeTypes = GetClientSubUnitReasonCodeItemTypes(clientSubUnitGuid);

			if (reasonCodeTypes != null)
			{
				foreach (spDDAWizard_SelectClientSubUnitReasonCodesTypes_v1Result reasonCodeType in reasonCodeTypes)
				{
					var reasonCodeItems = GetProductGroup(clientSubUnitGuid, reasonCodeType.ProductId, reasonCodeType.ReasonCodeTypeId, reasonCodeType.ReasonCodeTypeName);
					clientSubUnitReasonCodesVM.ClientSubUnitReasonCodeProductGroup.Add(reasonCodeItems);
				}
			}

            return clientSubUnitReasonCodesVM;
        }

		//List of All GetClientSubUnitReasonCodeItemTypes
		public List<spDDAWizard_SelectClientSubUnitReasonCodesTypes_v1Result> GetClientSubUnitReasonCodeItemTypes(string clientSubUnitGuid)
		{
			return db.spDDAWizard_SelectClientSubUnitReasonCodesTypes_v1(clientSubUnitGuid).ToList();
		}
		
		//List of All GetClientSubUnitReasonCodeItemsByProductAndType of a ClientSubUnit
		public List<spDDAWizard_SelectClientSubUnitReasonCodesByProductAndType_v1Result> GetClientSubUnitReasonCodeItemsByProductAndType(string clientSubUnitGuid, int productId, int reasonCodeTypeId)
        {
            return db.spDDAWizard_SelectClientSubUnitReasonCodesByProductAndType_v1(clientSubUnitGuid, productId, reasonCodeTypeId).ToList();
        }

		//List of All PolicyAirCabinGroupItems of a ClientSubUnit
		public List<spDDAWizard_SelectClientSubUnitPolicyAirCabinGroupItems_v1Result> GetClientSubUnitPolicyAirCabinGroupItems(string clientSubUnitGuid)
		{
			return db.spDDAWizard_SelectClientSubUnitPolicyAirCabinGroupItems_v1(clientSubUnitGuid).ToList();
		}

		//List of All PolicyAirParameterGroupItems of a ClientSubUnit
		public List<spDDAWizard_SelectClientSubUnitPolicyAirParameterGroupItems_v1Result> GetClientSubUnitPolicyAirTimeWindowGroupItems(string clientSubUnitGuid)
		{
			return db.spDDAWizard_SelectClientSubUnitPolicyAirParameterGroupItems_v1(clientSubUnitGuid, 1).ToList();
		}

		//List of All PolicyAirAdvancePurchaseGroupItems of a ClientSubUnit
		public List<spDDAWizard_SelectClientSubUnitPolicyAirParameterGroupItems_v1Result> GetClientSubUnitPolicyAirAdvancePurchaseGroupItems(string clientSubUnitGuid)
		{
			return db.spDDAWizard_SelectClientSubUnitPolicyAirParameterGroupItems_v1(clientSubUnitGuid, 2).ToList();
		}

		//List of All PolicyAirCabinGroupItems of a ClientSubUnit
		public List<spDDAWizard_SelectClientSubUnitPolicyAirMSTGroupItems_v1Result> GetClientSubUnitPolicyAirMSTGroupItems(string clientSubUnitGuid)
		{
			return db.spDDAWizard_SelectClientSubUnitPolicyAirMSTGroupItems_v1(clientSubUnitGuid).ToList();
		}

        //List of All PolicyAirVendorGroupItems of a ClientSubUnit
        public List<spDDAWizard_SelectClientSubUnitPolicyAirVendorGroupItems_v1Result> GetClientSubUnitPolicyAirVendorGroupItems(string clientSubUnitGuid)
        {
            return db.spDDAWizard_SelectClientSubUnitPolicyAirVendorGroupItems_v1(clientSubUnitGuid).ToList();
        }

        //List of All PolicyCarTypeGroupItems of a ClientSubUnit
        public List<spDDAWizard_SelectClientSubUnitPolicyCarTypeGroupItems_v1Result> GetClientSubUnitPolicyCarTypeGroupItems(string clientSubUnitGuid)
        {
           return db.spDDAWizard_SelectClientSubUnitPolicyCarTypeGroupItems_v1(clientSubUnitGuid).ToList();
        }

        //List of All PolicyCarVendorGroupItems of a ClientSubUnit
        public List<spDDAWizard_SelectClientSubUnitPolicyCarVendorGroupItems_v1Result> GetClientSubUnitPolicyCarVendorGroupItems(string clientSubUnitGuid)
        {
            return db.spDDAWizard_SelectClientSubUnitPolicyCarVendorGroupItems_v1(clientSubUnitGuid).ToList();
        }

        //List of All PolicyCountryGroupItems of a ClientSubUnit
        public List<spDDAWizard_SelectClientSubUnitPolicyCityGroupItems_v1Result> GetClientSubUnitPolicyCityGroupItems(string clientSubUnitGuid)
        {
            return db.spDDAWizard_SelectClientSubUnitPolicyCityGroupItems_v1(clientSubUnitGuid).ToList();
        }

        //List of All PolicyCountryGroupItems of a ClientSubUnit
        public List<spDDAWizard_SelectClientSubUnitPolicyCountryGroupItems_v1Result> GetClientSubUnitPolicyCountryGroupItems(string clientSubUnitGuid)
        {
            return db.spDDAWizard_SelectClientSubUnitPolicyCountryGroupItems_v1(clientSubUnitGuid).ToList();
        }

        //Single PolicyGroup of a ClientSubUnit
        public PolicyGroup GetClientSubUnitPolicyGroup(string clientSubUnitGuid)
        {
            PolicyGroup policyGroup = new PolicyGroup();
            spDDAWizard_SelectClientSubUnitPolicyGroup_v1Result clientSubUnitPolicyGroup = new spDDAWizard_SelectClientSubUnitPolicyGroup_v1Result();
            clientSubUnitPolicyGroup = db.spDDAWizard_SelectClientSubUnitPolicyGroup_v1(clientSubUnitGuid).FirstOrDefault();
            if (clientSubUnitPolicyGroup != null)
            {
                int policyGroupId = clientSubUnitPolicyGroup.PolicyGroupId;
                PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();
                policyGroup = policyGroupRepository.GetGroup(policyGroupId);
            }

            return policyGroup;
        }

        //Single ServicingOptionGroup of a ClientSubUnit 
        //Removed 24.03.2012, Readded Oct 2013 (v2.05.1)
        public ServicingOptionGroup GetClientSubUnitServicingOptionGroup(string clientSubUnitGuid)
        {
            ServicingOptionGroup servicingOptionGroup = new ServicingOptionGroup();
            spDDAWizard_SelectClientSubUnitServicingOptionGroup_v1Result clientSubUnitServicingOptionGroup = new spDDAWizard_SelectClientSubUnitServicingOptionGroup_v1Result();
            clientSubUnitServicingOptionGroup = db.spDDAWizard_SelectClientSubUnitServicingOptionGroup_v1(clientSubUnitGuid).FirstOrDefault();
            if (clientSubUnitServicingOptionGroup != null)
            {
                int servicingOptionGroupId = clientSubUnitServicingOptionGroup.ServicingOptionGroupId;
                ServicingOptionGroupRepository ServicingOptionGroupRepository = new ServicingOptionGroupRepository();
                servicingOptionGroup = ServicingOptionGroupRepository.GetGroup(servicingOptionGroupId);
            }

            return servicingOptionGroup;
        }

        //Policies of a ClientSubUnit
        public ClientSubUnitPoliciesVM GetClientSubUnitPolicies(string clientSubUnitGuid)
        {
            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = GetClientSubUnitPolicyGroup(clientSubUnitGuid);
            ClientSubUnitPoliciesVM clientSubUnitPoliciesVM = new ClientSubUnitPoliciesVM();
            clientSubUnitPoliciesVM.PolicyGroup = policyGroup;

            //Check Access Rights to ClientSubUnit's Policies
            clientSubUnitPoliciesVM.HasPolicyGroupWriteAccess = false;
            if (hierarchyRepository.AdminHasDomainHierarchyWriteAccess("ClientSubUnit", clientSubUnitGuid, "", "Policy Hierarchy"))
            {
                clientSubUnitPoliciesVM.HasPolicyGroupWriteAccess = true;
            }

            clientSubUnitPoliciesVM.Inherited = policyGroup.InheritFromParentFlag;
            PolicyRoutingRepository policyRoutingRepository = new PolicyRoutingRepository();

			#region Get PolicyAirTimeWindowGroupItems

			List<spDDAWizard_SelectClientSubUnitPolicyAirParameterGroupItems_v1Result> clientSubUnitPolicyAirTimeWindowGroupItems = new List<spDDAWizard_SelectClientSubUnitPolicyAirParameterGroupItems_v1Result>();
			clientSubUnitPolicyAirTimeWindowGroupItems = GetClientSubUnitPolicyAirTimeWindowGroupItems(clientSubUnitGuid);
			clientSubUnitPoliciesVM.PolicyAirTimeWindowGroupItems = clientSubUnitPolicyAirTimeWindowGroupItems;

			#endregion

			#region Get PolicyAirAdvancePurchaseGroupItems

			List<spDDAWizard_SelectClientSubUnitPolicyAirParameterGroupItems_v1Result> clientSubUnitPolicyAirAdvancePurchaseGroupItems = new List<spDDAWizard_SelectClientSubUnitPolicyAirParameterGroupItems_v1Result>();
			clientSubUnitPolicyAirAdvancePurchaseGroupItems = GetClientSubUnitPolicyAirAdvancePurchaseGroupItems(clientSubUnitGuid);
			clientSubUnitPoliciesVM.PolicyAirAdvancePurchaseGroupItems = clientSubUnitPolicyAirAdvancePurchaseGroupItems;

			#endregion

			#region Get PolicyAirCabinGroupItems

			List<spDDAWizard_SelectClientSubUnitPolicyAirCabinGroupItems_v1Result> clientSubUnitPolicyAirCabinGroupItems = new List<spDDAWizard_SelectClientSubUnitPolicyAirCabinGroupItems_v1Result>();
			clientSubUnitPolicyAirCabinGroupItems = GetClientSubUnitPolicyAirCabinGroupItems(clientSubUnitGuid);
			clientSubUnitPoliciesVM.PolicyAirCabinGroupItems = clientSubUnitPolicyAirCabinGroupItems;

			#endregion

            #region Get PolicyAirMissedSavingsThresholdGroupItems

            List<spDDAWizard_SelectClientSubUnitPolicyAirMSTGroupItems_v1Result> clientSubUnitPolicyAirMSTGroupItems = new List<spDDAWizard_SelectClientSubUnitPolicyAirMSTGroupItems_v1Result>();
            clientSubUnitPolicyAirMSTGroupItems = GetClientSubUnitPolicyAirMSTGroupItems(clientSubUnitGuid);
            clientSubUnitPoliciesVM.PolicyAirMissedSavingsThresholdGroupItems = clientSubUnitPolicyAirMSTGroupItems;

            #endregion

            #region Get PolicyAirVendorGroupItems

            List<spDDAWizard_SelectClientSubUnitPolicyAirVendorGroupItems_v1Result> clientSubUnitPolicyAirVendorGroupItems = new List<spDDAWizard_SelectClientSubUnitPolicyAirVendorGroupItems_v1Result>();
            clientSubUnitPolicyAirVendorGroupItems = GetClientSubUnitPolicyAirVendorGroupItems(clientSubUnitGuid);
            clientSubUnitPoliciesVM.PolicyAirVendorGroupItems = clientSubUnitPolicyAirVendorGroupItems;

            #endregion

            #region Get PolicyCarTypeGroupItems

            List<spDDAWizard_SelectClientSubUnitPolicyCarTypeGroupItems_v1Result> clientSubUnitPolicyCarTypeGroupItems = new List<spDDAWizard_SelectClientSubUnitPolicyCarTypeGroupItems_v1Result>();
            clientSubUnitPolicyCarTypeGroupItems = GetClientSubUnitPolicyCarTypeGroupItems(clientSubUnitGuid);
            clientSubUnitPoliciesVM.PolicyCarTypeGroupItems = clientSubUnitPolicyCarTypeGroupItems;

            #endregion

            #region Get PolicyCarVendorGroupItems

            List<spDDAWizard_SelectClientSubUnitPolicyCarVendorGroupItems_v1Result> clientSubUnitPolicyCarVendorGroupItems = new List<spDDAWizard_SelectClientSubUnitPolicyCarVendorGroupItems_v1Result>();
            clientSubUnitPolicyCarVendorGroupItems = GetClientSubUnitPolicyCarVendorGroupItems(clientSubUnitGuid);
            clientSubUnitPoliciesVM.PolicyCarVendorGroupItems = clientSubUnitPolicyCarVendorGroupItems;

            #endregion

            #region Get PolicyCityGroupItems

            List<spDDAWizard_SelectClientSubUnitPolicyCityGroupItems_v1Result> clientSubUnitPolicyCityGroupItems = new List<spDDAWizard_SelectClientSubUnitPolicyCityGroupItems_v1Result>();
            clientSubUnitPolicyCityGroupItems = GetClientSubUnitPolicyCityGroupItems(clientSubUnitGuid);
            clientSubUnitPoliciesVM.PolicyCityGroupItems = clientSubUnitPolicyCityGroupItems;

            #endregion

            #region Get PolicyCountryGroupItems

            List<spDDAWizard_SelectClientSubUnitPolicyCountryGroupItems_v1Result> clientSubUnitPolicyCountryGroupItems = new List<spDDAWizard_SelectClientSubUnitPolicyCountryGroupItems_v1Result>();
            clientSubUnitPolicyCountryGroupItems = GetClientSubUnitPolicyCountryGroupItems(clientSubUnitGuid);
            clientSubUnitPoliciesVM.PolicyCountryGroupItems = clientSubUnitPolicyCountryGroupItems;

            #endregion

            #region Get PolicyHotelCapRateGroupItems

            List<spDDAWizard_SelectClientSubUnitPolicyHotelCapRateGroupItems_v1Result> clientSubUnitPolicyHotelCapRateGroupItems = new List<spDDAWizard_SelectClientSubUnitPolicyHotelCapRateGroupItems_v1Result>();
            clientSubUnitPolicyHotelCapRateGroupItems = GetClientSubUnitPolicyHotelCapRateGroupItems(clientSubUnitGuid);
            clientSubUnitPoliciesVM.PolicyHotelCapRateGroupItems = clientSubUnitPolicyHotelCapRateGroupItems;

            #endregion

            #region Get PolicyHotelPropertyGroupItems

            List<spDDAWizard_SelectClientSubUnitPolicyHotelPropertyGroupItems_v1Result> clientSubUnitPolicyHotelPropertyGroupItems = new List<spDDAWizard_SelectClientSubUnitPolicyHotelPropertyGroupItems_v1Result>();
            clientSubUnitPolicyHotelPropertyGroupItems = GetClientSubUnitPolicyHotelPropertyGroupItems(clientSubUnitGuid);
            clientSubUnitPoliciesVM.PolicyHotelPropertyGroupItems = clientSubUnitPolicyHotelPropertyGroupItems;

            #endregion

            #region Get PolicyHotelVendorGroupItems

            List<spDDAWizard_SelectClientSubUnitPolicyHotelVendorGroupItems_v1Result> clientSubUnitPolicyHotelVendorGroupItems = new List<spDDAWizard_SelectClientSubUnitPolicyHotelVendorGroupItems_v1Result>();
            clientSubUnitPolicyHotelVendorGroupItems = GetClientSubUnitPolicyHotelVendorGroupItems(clientSubUnitGuid);
            clientSubUnitPoliciesVM.PolicyHotelVendorGroupItems = clientSubUnitPolicyHotelVendorGroupItems;

            #endregion

            #region Get PolicySupplierDealCodes

            List<spDDAWizard_SelectClientSubUnitPolicySupplierDealCodes_v1Result> clientSubUnitPolicySupplierDealCodes = new List<spDDAWizard_SelectClientSubUnitPolicySupplierDealCodes_v1Result>();
            clientSubUnitPolicySupplierDealCodes = GetClientSubUnitPolicySupplierDealCodes(clientSubUnitGuid);
            clientSubUnitPoliciesVM.PolicySupplierDealCodes = clientSubUnitPolicySupplierDealCodes;

            #endregion

            #region Get PolicySupplierServiceInformations

            List<spDDAWizard_SelectClientSubUnitPolicySupplierServiceInformations_v1Result> clientSubUnitPolicySupplierServiceInformations = new List<spDDAWizard_SelectClientSubUnitPolicySupplierServiceInformations_v1Result>();
            clientSubUnitPolicySupplierServiceInformations = GetClientSubUnitPolicySupplierServiceInformations(clientSubUnitGuid);
            clientSubUnitPoliciesVM.PolicySupplierServiceInformations = clientSubUnitPolicySupplierServiceInformations;

            #endregion;

			#region

			//Display Flags
			clientSubUnitPoliciesVM.Policy24HSCOtherGroupItemDisplayFlag = GetPolicyTypeDisplayFlag("Policy24HSCOtherGroupItem");
			clientSubUnitPoliciesVM.PolicyAirCabinGroupItemDisplayFlag = GetPolicyTypeDisplayFlag("PolicyAirCabinGroupItem");
			clientSubUnitPoliciesVM.PolicyAirMissedSavingsThresholdGroupItemDisplayFlag = GetPolicyTypeDisplayFlag("PolicyAirMissedSavingsThresholdGroupItem");
			clientSubUnitPoliciesVM.PolicyAirTimeWindowGroupItemDisplayFlag = GetPolicyTypeDisplayFlag("PolicyAirTimeWindowGroupItem");
			clientSubUnitPoliciesVM.PolicyAirOtherGroupItemDisplayFlag = GetPolicyTypeDisplayFlag("PolicyAirOtherGroupItem");
			clientSubUnitPoliciesVM.PolicyAirAdvancePurchaseGroupItemDisplayFlag = GetPolicyTypeDisplayFlag("PolicyAirAdvancePurchaseGroupItem");
			clientSubUnitPoliciesVM.PolicyAirVendorGroupItemDisplayFlag = GetPolicyTypeDisplayFlag("PolicyAirVendorGroupItem");
			clientSubUnitPoliciesVM.PolicyAllOtherGroupItemDisplayFlag = GetPolicyTypeDisplayFlag("PolicyAllOtherGroupItem");
			clientSubUnitPoliciesVM.PolicyCarOtherGroupItemDisplayFlag = GetPolicyTypeDisplayFlag("PolicyCarOtherGroupItem");
			clientSubUnitPoliciesVM.PolicyCarTypeGroupItemDisplayFlag = GetPolicyTypeDisplayFlag("PolicyCarTypeGroupItem");
			clientSubUnitPoliciesVM.PolicyCarVendorGroupItemDisplayFlag = GetPolicyTypeDisplayFlag("PolicyCarVendorGroupItem");
			clientSubUnitPoliciesVM.PolicyCityGroupItemDisplayFlag = GetPolicyTypeDisplayFlag("PolicyCityGroupItem");
			clientSubUnitPoliciesVM.PolicyCountryGroupItemDisplayFlag = GetPolicyTypeDisplayFlag("PolicyCountryGroupItem");
			clientSubUnitPoliciesVM.PolicyHotelCapRateGroupItemDisplayFlag = GetPolicyTypeDisplayFlag("PolicyHotelCapRateGroupItem");
			clientSubUnitPoliciesVM.PolicyHotelOtherGroupItemDisplayFlag = GetPolicyTypeDisplayFlag("PolicyHotelOtherGroupItem");
			clientSubUnitPoliciesVM.PolicyHotelPropertyGroupItemDisplayFlag = GetPolicyTypeDisplayFlag("PolicyHotelPropertyGroupItem");
			clientSubUnitPoliciesVM.PolicyHotelVendorGroupItemDisplayFlag = GetPolicyTypeDisplayFlag("PolicyHotelVendorGroupItem");
			clientSubUnitPoliciesVM.PolicyMessageGroupItemDisplayFlag = GetPolicyTypeDisplayFlag("PolicyMessageGroupItem");
			clientSubUnitPoliciesVM.PolicyOtherGroupItemDisplayFlag = GetPolicyTypeDisplayFlag("PolicyOtherGroupItem");
			clientSubUnitPoliciesVM.PolicySupplierDealCodeDisplayFlag = GetPolicyTypeDisplayFlag("PolicySupplierDealCode");
			clientSubUnitPoliciesVM.PolicySupplierServiceInformationDisplayFlag = GetPolicyTypeDisplayFlag("PolicySupplierServiceInformation");

			//Display Titles
			clientSubUnitPoliciesVM.Policy24HSCOtherGroupItemDisplayTitle = GetPolicyTypeDisplayTitle("Policy24HSCOtherGroupItem");
			clientSubUnitPoliciesVM.PolicyAirCabinGroupItemDisplayTitle = GetPolicyTypeDisplayTitle("PolicyAirCabinGroupItem");
			clientSubUnitPoliciesVM.PolicyAirMissedSavingsThresholdGroupItemDisplayTitle = GetPolicyTypeDisplayTitle("PolicyAirMissedSavingsThresholdGroupItem");
			clientSubUnitPoliciesVM.PolicyAirTimeWindowGroupItemDisplayTitle = GetPolicyTypeDisplayTitle("PolicyAirTimeWindowGroupItem");
			clientSubUnitPoliciesVM.PolicyAirOtherGroupItemDisplayTitle = GetPolicyTypeDisplayTitle("PolicyAirOtherGroupItem");
			clientSubUnitPoliciesVM.PolicyAirAdvancePurchaseGroupItemDisplayTitle = GetPolicyTypeDisplayTitle("PolicyAirAdvancePurchaseGroupItem");
			clientSubUnitPoliciesVM.PolicyAirVendorGroupItemDisplayTitle = GetPolicyTypeDisplayTitle("PolicyAirVendorGroupItem");
			clientSubUnitPoliciesVM.PolicyAllOtherGroupItemDisplayTitle = GetPolicyTypeDisplayTitle("PolicyAllOtherGroupItem");
			clientSubUnitPoliciesVM.PolicyCarOtherGroupItemDisplayTitle = GetPolicyTypeDisplayTitle("PolicyCarOtherGroupItem");
			clientSubUnitPoliciesVM.PolicyCarTypeGroupItemDisplayTitle = GetPolicyTypeDisplayTitle("PolicyCarTypeGroupItem");
			clientSubUnitPoliciesVM.PolicyCarVendorGroupItemDisplayTitle = GetPolicyTypeDisplayTitle("PolicyCarVendorGroupItem");
			clientSubUnitPoliciesVM.PolicyCityGroupItemDisplayTitle = GetPolicyTypeDisplayTitle("PolicyCityGroupItem");
			clientSubUnitPoliciesVM.PolicyCountryGroupItemDisplayTitle = GetPolicyTypeDisplayTitle("PolicyCountryGroupItem");
			clientSubUnitPoliciesVM.PolicyHotelCapRateGroupItemDisplayTitle = GetPolicyTypeDisplayTitle("PolicyHotelCapRateGroupItem");
			clientSubUnitPoliciesVM.PolicyHotelOtherGroupItemDisplayTitle = GetPolicyTypeDisplayTitle("PolicyHotelOtherGroupItem");
			clientSubUnitPoliciesVM.PolicyHotelPropertyGroupItemDisplayTitle = GetPolicyTypeDisplayTitle("PolicyHotelPropertyGroupItem");
			clientSubUnitPoliciesVM.PolicyHotelVendorGroupItemDisplayTitle = GetPolicyTypeDisplayTitle("PolicyHotelVendorGroupItem");
			clientSubUnitPoliciesVM.PolicyMessageGroupItemDisplayTitle = GetPolicyTypeDisplayTitle("PolicyMessageGroupItem");
			clientSubUnitPoliciesVM.PolicyOtherGroupItemDisplayTitle = GetPolicyTypeDisplayTitle("PolicyOtherGroupItem");
			clientSubUnitPoliciesVM.PolicySupplierDealCodeDisplayTitle = GetPolicyTypeDisplayTitle("PolicySupplierDealCode");
			clientSubUnitPoliciesVM.PolicySupplierServiceInformationDisplayTitle = GetPolicyTypeDisplayTitle("PolicySupplierServiceInformation");

			#endregion

			return clientSubUnitPoliciesVM;
        }

		public bool? GetPolicyTypeDisplayFlag(string policyTypeTableName)
		{
			return db.fnDesktopDataAdmin_SelectPolicyTypeDisplay_v1(policyTypeTableName);
		}

		public string GetPolicyTypeDisplayTitle(string policyTypeTableName)
		{
			return db.fnDesktopDataAdmin_SelectPolicyTypeDisplayTitle_v1(policyTypeTableName);
		}

        //List of All PolicyHotelCapRateGroupItems of a ClientSubUnit
        public List<spDDAWizard_SelectClientSubUnitPolicyHotelCapRateGroupItems_v1Result> GetClientSubUnitPolicyHotelCapRateGroupItems(string clientSubUnitGuid)
        {
            return db.spDDAWizard_SelectClientSubUnitPolicyHotelCapRateGroupItems_v1(clientSubUnitGuid).ToList();
        }
        
        //List of All PolicyHotelPropertyGroupItems of a ClientSubUnit
        public List<spDDAWizard_SelectClientSubUnitPolicyHotelPropertyGroupItems_v1Result> GetClientSubUnitPolicyHotelPropertyGroupItems(string clientSubUnitGuid)
        {
            return db.spDDAWizard_SelectClientSubUnitPolicyHotelPropertyGroupItems_v1(clientSubUnitGuid).ToList();
        }

        //List of All PolicyHotelVendorGroupItems of a ClientSubUnit
        public List<spDDAWizard_SelectClientSubUnitPolicyHotelVendorGroupItems_v1Result> GetClientSubUnitPolicyHotelVendorGroupItems(string clientSubUnitGuid)
        {
            return db.spDDAWizard_SelectClientSubUnitPolicyHotelVendorGroupItems_v1(clientSubUnitGuid).ToList();
        }

        //List of All PolicySupplierDealCodes of a ClientSubUnit
        public List<spDDAWizard_SelectClientSubUnitPolicySupplierDealCodes_v1Result> GetClientSubUnitPolicySupplierDealCodes(string clientSubUnitGuid)
        {
            return db.spDDAWizard_SelectClientSubUnitPolicySupplierDealCodes_v1(clientSubUnitGuid).ToList();
        }

        //List of All PolicySupplierServiceInformations of a ClientSubUnit
        public List<spDDAWizard_SelectClientSubUnitPolicySupplierServiceInformations_v1Result> GetClientSubUnitPolicySupplierServiceInformations(string clientSubUnitGuid)
        {
            return db.spDDAWizard_SelectClientSubUnitPolicySupplierServiceInformations_v1(clientSubUnitGuid).ToList();
        }

        //List of All ReasonCodesItems associated to a Group
        public List<spDDAWizard_SelectReasonCodeGroupReasonCodeItems_v1Result> GetReasonCodeGroupReasonCodeItems(int groupId, int productId, int reasonCodeTypeId)
        {
            return db.spDDAWizard_SelectReasonCodeGroupReasonCodeItems_v1(groupId, productId, reasonCodeTypeId).ToList();
        }

        //List of All ReasonCodesItems Not associated to a Group
        public List<spDDAWizard_SelectReasonCodeGroupAvailableReasonCodes_v1Result> GetReasonCodeGroupAvailableReasonCodeItems(int? groupId, int productId, int reasonCodeTypeId)
        {
            return db.spDDAWizard_SelectReasonCodeGroupAvailableReasonCodes_v1(groupId, productId, reasonCodeTypeId).ToList();
        }
    
        //List of All ReasonCodes
        public List<spDDAWizard_SelectAllReasonCodeItems_v1Result> GetReasonCodes()
        {
            return db.spDDAWizard_SelectAllReasonCodeItems_v1().ToList();
        }

        //List of All ReasonCode Groups
        public List<spDDAWizard_SelectClientSubUnitReasonCodeGroups_v1Result> GetReasonCodeGroups(string clientSubUnitGuid)
        {
            return db.spDDAWizard_SelectClientSubUnitReasonCodeGroups_v1(clientSubUnitGuid).ToList();
        }

        //Compare two Clients and return a list of messages about changes
        public WizardMessages BuildClientChangeMessages(WizardMessages wizardMessages, ClientWizardVM originalClient, ClientWizardVM updatedClient)
        {

            #region ClientSubUnit Changes
            ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
            clientSubUnitRepository.EditGroupForDisplay(updatedClient.ClientSubUnit);

            //PortraitStatusId
            if (originalClient.ClientSubUnit.PortraitStatusId != updatedClient.ClientSubUnit.PortraitStatusId)
            {
                PortraitStatusRepository portraitStatusRepository = new PortraitStatusRepository();
                PortraitStatus portraitStatus = new PortraitStatus();
                portraitStatus = portraitStatusRepository.GetPortraitStatus(updatedClient.ClientSubUnit.PortraitStatusId);
                wizardMessages.AddMessage("Client SubUnit Portrait Status will be updated to \"" + portraitStatus.PortraitStatusDescription + "\".", true);
            }

            //PortraitStatusDescription

            if (originalClient.ClientSubUnit.PortraitStatusDescription == null)
            {
                originalClient.ClientSubUnit.PortraitStatusDescription = string.Empty;
            }

            if (updatedClient.ClientSubUnit.PortraitStatusDescription == null)
            {
                updatedClient.ClientSubUnit.PortraitStatusDescription = string.Empty;
            }

            if (originalClient.ClientSubUnit.PortraitStatusDescription != updatedClient.ClientSubUnit.PortraitStatusDescription)
            {
                wizardMessages.AddMessage("Portrait Status Description will be updated to \"" + updatedClient.ClientSubUnit.PortraitStatusDescription + "\".", true);
            }

            //ClientBusinessDescription

            if (originalClient.ClientSubUnit.ClientBusinessDescription == null)
            {
                originalClient.ClientSubUnit.ClientBusinessDescription = string.Empty;
            }

            if (updatedClient.ClientSubUnit.ClientBusinessDescription == null)
            {
                updatedClient.ClientSubUnit.ClientBusinessDescription = string.Empty;
            }

            if (originalClient.ClientSubUnit.ClientBusinessDescription != updatedClient.ClientSubUnit.ClientBusinessDescription)
            {
                wizardMessages.AddMessage("Client Business Description will be updated to \"" + updatedClient.ClientSubUnit.ClientBusinessDescription + "\".", true);
            }

            //LineOfBusinessId

            if (originalClient.ClientSubUnit.LineOfBusinessId != updatedClient.ClientSubUnit.LineOfBusinessId)
			{
				LineOfBusinessRepository lineOfBusinessRepository = new LineOfBusinessRepository();
				LineOfBusiness lineOfBusiness = new LineOfBusiness();
				lineOfBusiness = lineOfBusinessRepository.GetLineOfBusiness(updatedClient.ClientSubUnit.LineOfBusinessId);
				wizardMessages.AddMessage("Client SubUnit Line of Business will be updated to \"" + lineOfBusiness.LineofBusinessDescription + "\".", true);
			}
            
			if (originalClient.ClientSubUnit.ClientSubUnitDisplayName != updatedClient.ClientSubUnit.ClientSubUnitDisplayName)
            {
                wizardMessages.AddMessage("Client SubUnit Display Name will be updated to \"" + updatedClient.ClientSubUnit.ClientSubUnitDisplayName + "\".", true);
            }
			
			if (originalClient.ClientSubUnit.RestrictedClient != updatedClient.ClientSubUnit.RestrictedClient)
			{
				wizardMessages.AddMessage("Client SubUnit Restricted Client will be updated to \"" + updatedClient.ClientSubUnit.RestrictedClient + "\".", true);
			}

			if (originalClient.ClientSubUnit.PrivateClient != updatedClient.ClientSubUnit.PrivateClient)
			{
				wizardMessages.AddMessage("Client SubUnit Private Client will be updated to \"" + updatedClient.ClientSubUnit.PrivateClient + "\".", true);
			}

			if (originalClient.ClientSubUnit.CubaBookingAllowed != updatedClient.ClientSubUnit.CubaBookingAllowed)
			{
				wizardMessages.AddMessage("Client SubUnit Cuba Booking Allowed will be updated to \"" + updatedClient.ClientSubUnit.CubaBookingAllowed + "\".", true);
			}

			if (originalClient.ClientSubUnit.InCountryServiceOnly != updatedClient.ClientSubUnit.InCountryServiceOnly)
			{
				wizardMessages.AddMessage("Client SubUnit In Country Service Only will be updated to \"" + updatedClient.ClientSubUnit.InCountryServiceOnly + "\".", true);
			}

			//Dialled Number

			if (originalClient.ClientSubUnit.DialledNumber24HSC == null)
			{
				originalClient.ClientSubUnit.DialledNumber24HSC = string.Empty;
			}

			if (updatedClient.ClientSubUnit.DialledNumber24HSC == null)
			{
				updatedClient.ClientSubUnit.DialledNumber24HSC = string.Empty;
			}

			if (originalClient.ClientSubUnit.DialledNumber24HSC != updatedClient.ClientSubUnit.DialledNumber24HSC)
			{	
				wizardMessages.AddMessage("Client SubUnit 24HSC Dialled Number will be updated to \"" + updatedClient.ClientSubUnit.DialledNumber24HSC + "\".", true);
			}

			//Branch Contact Number

			if (originalClient.ClientSubUnit.BranchContactNumber == null)
			{
				originalClient.ClientSubUnit.BranchContactNumber = string.Empty;
			}

			if (updatedClient.ClientSubUnit.BranchContactNumber == null)
			{
				updatedClient.ClientSubUnit.BranchContactNumber = string.Empty;
			}

			if (originalClient.ClientSubUnit.BranchContactNumber != updatedClient.ClientSubUnit.BranchContactNumber)
			{
				wizardMessages.AddMessage("Client SubUnit Branch Contact Number will be updated to \"" + updatedClient.ClientSubUnit.BranchContactNumber + "\".", true);
			}

			//Branch Fax Number

			if (originalClient.ClientSubUnit.BranchFaxNumber == null)
			{
				originalClient.ClientSubUnit.BranchFaxNumber = string.Empty;
			}

			if (updatedClient.ClientSubUnit.BranchFaxNumber == null)
			{
				updatedClient.ClientSubUnit.BranchFaxNumber = string.Empty;
			} 
			
			if (originalClient.ClientSubUnit.BranchFaxNumber != updatedClient.ClientSubUnit.BranchFaxNumber)
			{
				wizardMessages.AddMessage("Client SubUnit Branch Fax Number will be updated to \"" + updatedClient.ClientSubUnit.BranchFaxNumber + "\".", true);
			}

			//Branch Email

			if (originalClient.ClientSubUnit.BranchEmail == null)
			{
				originalClient.ClientSubUnit.BranchEmail = string.Empty;
			}

			if (updatedClient.ClientSubUnit.BranchEmail == null)
			{
				updatedClient.ClientSubUnit.BranchEmail = string.Empty;
			} 
			
			if (originalClient.ClientSubUnit.BranchEmail != updatedClient.ClientSubUnit.BranchEmail)
			{
				wizardMessages.AddMessage("Client SubUnit Branch Email will be updated to \"" + updatedClient.ClientSubUnit.BranchEmail + "\".", true);
			}

			//Client IATA

			if (originalClient.ClientSubUnit.ClientIATA == null)
			{
				originalClient.ClientSubUnit.ClientIATA = string.Empty;
			}

			if (updatedClient.ClientSubUnit.ClientIATA == null)
			{
				updatedClient.ClientSubUnit.ClientIATA = string.Empty;
			} 
			
			if (originalClient.ClientSubUnit.ClientIATA != updatedClient.ClientSubUnit.ClientIATA)
			{
				wizardMessages.AddMessage("Client SubUnit Client IATA will be updated to \"" + updatedClient.ClientSubUnit.ClientIATA + "\".", true);
			}

			//Telephonies

			if (updatedClient.ClientSubUnitTelephoniesAdded != null && updatedClient.ClientSubUnitTelephoniesAdded.Count > 0)
            {
                wizardMessages.AddMessage("Client SubUnit Telephonies will be Added.", true);
            }
            
			if (updatedClient.ClientSubUnitTelephoniesRemoved != null && updatedClient.ClientSubUnitTelephoniesRemoved.Count > 0)
            {
                wizardMessages.AddMessage("Client SubUnit Telephonies will be Removed.", true);
            }
            
			#endregion

            #region Team Changes
            TeamRepository teamRepository = new TeamRepository();
            if (updatedClient.TeamsAdded != null)
            {
                if (updatedClient.TeamsAdded.Count > 0)
                {
                    foreach (ClientSubUnitTeam item in updatedClient.TeamsAdded)
                    {
                        Team team = new Team();
                        team = teamRepository.GetTeam(item.TeamId);
                        if (team != null)
                        {
                            wizardMessages.AddMessage("You will add Team \"" + team.TeamName + "\" to the ClientSubUnit.", true);
                        }
                    }
                }
            }
            if (updatedClient.TeamsRemoved != null)
            {
                if (updatedClient.TeamsRemoved.Count > 0)
                {
                    foreach (ClientSubUnitTeam item in updatedClient.TeamsRemoved)
                    {
                        Team team = new Team();
                        team = teamRepository.GetTeam(item.TeamId);
                        if (team != null)
                        {
                            wizardMessages.AddMessage("You will remove Team \"" + team.TeamName + "\" from the ClientSubUnit.", true);
                        }
                    }
                }
            }
            if (updatedClient.TeamsAltered != null)
            {
                if (updatedClient.TeamsAltered.Count > 0)
                {
                    foreach (ClientSubUnitTeam item in updatedClient.TeamsAltered)
                    {
                        Team team = new Team();
                        team = teamRepository.GetTeam(item.TeamId);
                        if (team != null)
                        {
                            wizardMessages.AddMessage("You will alter Team \"" + team.TeamName + "\".", true);
                        }
                    }
                }
            }
            #endregion

            #region ClientAccount Changes
            ClientAccountRepository clientAccountRepository = new ClientAccountRepository();
            if (updatedClient.ClientAccountsAdded != null)
            {
                if (updatedClient.ClientAccountsAdded.Count > 0)
                {
                    foreach (ClientSubUnitClientAccount item in updatedClient.ClientAccountsAdded)
                    {
                        ClientAccount clientAccount = new ClientAccount();
                        clientAccount = clientAccountRepository.GetClientAccount(item.ClientAccountNumber, item.SourceSystemCode);
                        if (clientAccount != null)
                        {
                            wizardMessages.AddMessage("You will add Client Account \"" + clientAccount.ClientAccountName + "\" to the ClientSubUnit.", true);
                        }
                    }
                }
            }
            if (updatedClient.ClientAccountsRemoved != null)
            {
                if (updatedClient.ClientAccountsRemoved.Count > 0)
                {
                    foreach (ClientSubUnitClientAccount item in updatedClient.ClientAccountsRemoved)
                    {
                        ClientAccount clientAccount = new ClientAccount();
                        clientAccount = clientAccountRepository.GetClientAccount(item.ClientAccountNumber, item.SourceSystemCode);
                        if (clientAccount != null)
                        {
                            wizardMessages.AddMessage("You will remove Client Account \"" + clientAccount.ClientAccountName + "\" from the ClientSubUnit.", true);
                        }
                    }
                }

            }
            #endregion

            #region Servicing Option Changes

            if (hierarchyRepository.AdminHasDomainHierarchyWriteAccess("ClientSubUnit", originalClient.ClientSubUnit.ClientSubUnitGuid, "", "Servicing Option"))
            {

                ServicingOptionRepository servicingOptionRepository = new ServicingOptionRepository();
                if (updatedClient.ServicingOptionItemsAdded != null)
                {
                    if (updatedClient.ServicingOptionItemsAdded.Count > 0)
                    {
                        foreach (ServicingOptionItem item in updatedClient.ServicingOptionItemsAdded)
                        {
                            ServicingOption servicingOption = new ServicingOption();
                            servicingOption = servicingOptionRepository.GetServicingOption(item.ServicingOptionId);
                            if (servicingOption != null)
                            {
                                wizardMessages.AddMessage("You will add Servicing Option Item \"" + servicingOption.ServicingOptionName + "\" to the ClientSubUnit.", true);
                            }
                        }
                    }

                }
                if (updatedClient.ServicingOptionItemsRemoved != null)
                {
                    if (updatedClient.ServicingOptionItemsRemoved.Count > 0)
                    {
                        foreach (ServicingOptionItem item in updatedClient.ServicingOptionItemsRemoved)
                        {
                            ServicingOption servicingOption = new ServicingOption();
                            servicingOption = servicingOptionRepository.GetServicingOption(item.ServicingOptionId);
                            if (servicingOption != null)
                            {
                                wizardMessages.AddMessage("You will remove Servicing Option Item \"" + servicingOption.ServicingOptionName + "\" from the ClientSubUnit.", true);
                            }
                        }
                    }

                }
                if (updatedClient.ServicingOptionItemsAltered != null)
                {
                    if (updatedClient.ServicingOptionItemsAltered.Count > 0)
                    {
                        foreach (ServicingOptionItem item in updatedClient.ServicingOptionItemsAltered)
                        {
                            ServicingOption servicingOption = new ServicingOption();
                            servicingOption = servicingOptionRepository.GetServicingOption(item.ServicingOptionId);
                            if (servicingOption != null)
                            {
                                wizardMessages.AddMessage("You will alter Servicing Option Item \"" + servicingOption.ServicingOptionName + "\".", true);
                            }
                        }
                    }

                }
            }
            #endregion

            #region ReasonCode Changes

			if (hierarchyRepository.AdminHasDomainHierarchyWriteAccess("ClientSubUnit", originalClient.ClientSubUnit.ClientSubUnitGuid, "", "Reason Code"))
			{
				ReasonCodeItemRepository reasonCodeItemRepository = new ReasonCodeItemRepository();
				if (updatedClient.ReasonCodesAdded != null)
				{
					if (updatedClient.ReasonCodesAdded.Count > 0)
					{
						foreach (ReasonCodeItem item in updatedClient.ReasonCodesAdded)
						{
							ReasonCodeProductTypeRepository reasonCodeProductTypeRepository = new ReasonCodeProductTypeRepository();
							ReasonCodeProductType reasonCodeProductType = new ReasonCodeProductType();
							reasonCodeProductType = reasonCodeProductTypeRepository.GetReasonCodeProductType(item.ReasonCode, item.ProductId, item.ReasonCodeTypeId);

							//now we get the description (may not exist)
							ReasonCodeProductTypeDescriptionRepository reasonCodeProductTypeDescriptionRepository = new ReasonCodeProductTypeDescriptionRepository();
							ReasonCodeProductTypeDescription reasonCodeProductTypeDescription = new ReasonCodeProductTypeDescription();
							reasonCodeProductTypeDescription = reasonCodeProductTypeDescriptionRepository.GetItem("en-GB", item.ReasonCode, item.ProductId, item.ReasonCodeTypeId);

							if (reasonCodeProductTypeDescription != null)
							{
								wizardMessages.AddMessage("You will add ReasonCode ProductType \"" + reasonCodeProductTypeDescription.ReasonCodeProductTypeDescription1 + "\" to the ClientSubUnit.", true);
							}
							else
							{
								//reasonCodeProductTypeDescription is null
								//instead use description of individual members
								ProductRepository productRepository = new ProductRepository();
								Product product = new Product();
								product = productRepository.GetProduct(item.ProductId);

								ReasonCodeTypeRepository reasonCodeTypeRepository = new ReasonCodeTypeRepository();
								ReasonCodeType reasonCodeType = new ReasonCodeType();
								reasonCodeType = reasonCodeTypeRepository.GetItem(reasonCodeProductType.ReasonCodeTypeId);

								wizardMessages.AddMessage("You will add ReasonCode \"" + item.ReasonCode + "\" to " + product.ProductName + "\\" + reasonCodeType.ReasonCodeTypeDescription + ".", true);

							}
						}
					}
				}
				if (updatedClient.ReasonCodeItemsRemoved != null)
				{
					if (updatedClient.ReasonCodeItemsRemoved.Count > 0)
					{
						foreach (ReasonCodeItem item in updatedClient.ReasonCodeItemsRemoved)
						{
							//only have ID, need to get PK properties
							ReasonCodeItem reasonCodeItem = new ReasonCodeItem();
							reasonCodeItem = reasonCodeItemRepository.GetItem(item.ReasonCodeItemId);

							//use PK to get full object
							ReasonCodeProductTypeRepository reasonCodeProductTypeRepository = new ReasonCodeProductTypeRepository();
							ReasonCodeProductType reasonCodeProductType = new ReasonCodeProductType();
							reasonCodeProductType = reasonCodeProductTypeRepository.GetReasonCodeProductType(reasonCodeItem.ReasonCode, reasonCodeItem.ProductId, reasonCodeItem.ReasonCodeTypeId);

							//now we get the description (may not exist)
							ReasonCodeProductTypeDescriptionRepository reasonCodeProductTypeDescriptionRepository = new ReasonCodeProductTypeDescriptionRepository();
							ReasonCodeProductTypeDescription reasonCodeProductTypeDescription = new ReasonCodeProductTypeDescription();
							reasonCodeProductTypeDescription = reasonCodeProductTypeDescriptionRepository.GetItem("en-GB", reasonCodeItem.ReasonCode, reasonCodeItem.ProductId, reasonCodeItem.ReasonCodeTypeId);

							if (reasonCodeProductTypeDescription != null)
							{
								wizardMessages.AddMessage("You will remove ReasonCode ProductType \"" + reasonCodeProductTypeDescription.ReasonCodeProductTypeDescription1 + "\" from the ClientSubUnit.", true);
							}
							else
							{
								//reasonCodeProductTypeDescription is null
								//instead use description of individual members
								ProductRepository productRepository = new ProductRepository();
								Product product = new Product();
								product = productRepository.GetProduct(reasonCodeItem.ProductId);

								ReasonCodeTypeRepository reasonCodeTypeRepository = new ReasonCodeTypeRepository();
								ReasonCodeType reasonCodeType = new ReasonCodeType();
								reasonCodeType = reasonCodeTypeRepository.GetItem(reasonCodeProductType.ReasonCodeTypeId);

								wizardMessages.AddMessage("You will remove ReasonCode \"" + reasonCodeItem.ReasonCode + "\" from " + product.ProductName + "\\" + reasonCodeType.ReasonCodeTypeDescription + ".", true);

							}
						}
					}
				}

				if (updatedClient.ReasonCodeItemsAltered != null)
				{
					if (updatedClient.ReasonCodeItemsAltered.Count > 0)
					{
						foreach (ReasonCodeItem item in updatedClient.ReasonCodeItemsAltered)
						{
							//only have ID, need to get PK properties
							ReasonCodeItem reasonCodeItem = new ReasonCodeItem();
							reasonCodeItem = reasonCodeItemRepository.GetItem(item.ReasonCodeItemId);

							//use PK to get full object
							ReasonCodeProductTypeRepository reasonCodeProductTypeRepository = new ReasonCodeProductTypeRepository();
							ReasonCodeProductType reasonCodeProductType = new ReasonCodeProductType();
							reasonCodeProductType = reasonCodeProductTypeRepository.GetReasonCodeProductType(reasonCodeItem.ReasonCode, reasonCodeItem.ProductId, reasonCodeItem.ReasonCodeTypeId);

							//now we get the description (may not exist)
							ReasonCodeProductTypeDescriptionRepository reasonCodeProductTypeDescriptionRepository = new ReasonCodeProductTypeDescriptionRepository();
							ReasonCodeProductTypeDescription reasonCodeProductTypeDescription = new ReasonCodeProductTypeDescription();
							reasonCodeProductTypeDescription = reasonCodeProductTypeDescriptionRepository.GetItem("en-GB", reasonCodeItem.ReasonCode, reasonCodeItem.ProductId, reasonCodeItem.ReasonCodeTypeId);

							if (reasonCodeProductTypeDescription != null)
							{
								wizardMessages.AddMessage("You will update ReasonCode ProductType \"" + reasonCodeProductTypeDescription.ReasonCodeProductTypeDescription1 + "\".", true);
							}
							else
							{
								//reasonCodeProductTypeDescription is null
								//instead use description of individual members
								ProductRepository productRepository = new ProductRepository();
								Product product = new Product();
								product = productRepository.GetProduct(reasonCodeItem.ProductId);

								ReasonCodeTypeRepository reasonCodeTypeRepository = new ReasonCodeTypeRepository();
								ReasonCodeType reasonCodeType = new ReasonCodeType();
								reasonCodeType = reasonCodeTypeRepository.GetItem(reasonCodeProductType.ReasonCodeTypeId);

								wizardMessages.AddMessage("You will update ReasonCode \"" + reasonCodeItem.ReasonCode + ".", true);
							}
						}
					}
				}
			}
            #endregion

            #region PolicyGroup + Policies Changes

            //Check Access Rights to ClientSubUnit's Policies
             if (hierarchyRepository.AdminHasDomainHierarchyWriteAccess("ClientSubUnit", originalClient.ClientSubUnit.ClientSubUnitGuid, "", "Policy Hierarchy"))
             {
                 #region PolicyGroup Changes

                 //There is no existing Policygroup, we are adding a PolicyGroup           
                 if (originalClient.PolicyGroup == null && updatedClient.PolicyGroup != null)
                 {
                     wizardMessages.AddMessage("You will create a Policy Group.", true);
                 }
                 //We are changing the InheritFrom Parent flag on existing PolicyGroup
                 if (originalClient.PolicyGroup != null && updatedClient.PolicyGroup != null)
                 {
                     if (updatedClient.PolicyGroup.InheritFromParentFlag != originalClient.PolicyGroup.InheritFromParentFlag)
                     {
                         wizardMessages.AddMessage("You will alter Policy Group Inheritance.", true);
                     }
                 }
                 #endregion

				 #region Policy Air Parameter Group Item Changes
				 if (updatedClient.PolicyAirParameterGroupItemsAdded != null)
				 {
					 if (updatedClient.PolicyAirParameterGroupItemsAdded.Count > 0)
					 {
						 if (updatedClient.PolicyAirParameterGroupItemsAdded.Where(x => x.PolicyAirParameterGroupItem.PolicyAirParameterTypeId == 1).Count() > 0)
						 {
							 wizardMessages.AddMessage("You will add Policy Air Time Window Group Item(s) to the ClientSubUnit.", true);
						 }
						 if (updatedClient.PolicyAirParameterGroupItemsAdded.Where(x => x.PolicyAirParameterGroupItem.PolicyAirParameterTypeId == 2).Count() > 0)
						 {
							 wizardMessages.AddMessage("You will add Policy Air Advance Purchase Group Item(s) to the ClientSubUnit.", true);
						 }
					 }
				 }
				 if (updatedClient.PolicyAirParameterGroupItemsAltered != null)
				 {
					 if (updatedClient.PolicyAirParameterGroupItemsAltered.Count > 0)
					 {
						 if (updatedClient.PolicyAirParameterGroupItemsAltered.Where(x => x.PolicyAirParameterGroupItem.PolicyAirParameterTypeId == 1).Count() > 0)
						 {
							 wizardMessages.AddMessage("You will alter Policy Air Time Window Group Item(s) of the ClientSubUnit.", true);
						 }
						 if (updatedClient.PolicyAirParameterGroupItemsAltered.Where(x => x.PolicyAirParameterGroupItem.PolicyAirParameterTypeId == 2).Count() > 0)
						 {
							 wizardMessages.AddMessage("You will alter Policy Air Advance Purchase Group Item(s) of the ClientSubUnit.", true);
						 }
					 }
				 }
				 if (updatedClient.PolicyAirParameterGroupItemsRemoved != null)
				 {
					 if (updatedClient.PolicyAirParameterGroupItemsRemoved.Count > 0)
					 {
						 if (updatedClient.PolicyAirParameterGroupItemsRemoved.Where(x => x.PolicyAirParameterTypeId == 1).Count() > 0)
						 {
							 wizardMessages.AddMessage("You will remove Policy Air Time Window Group Item(s) from the ClientSubUnit.", true);
						 }
						 if (updatedClient.PolicyAirParameterGroupItemsRemoved.Where(x => x.PolicyAirParameterTypeId == 2).Count() > 0)
						 {
							 wizardMessages.AddMessage("You will remove Policy Air Advance Purchase Group Item(s) from the ClientSubUnit.", true);
						 }
					 }
				 }
				 #endregion

				 #region Policy Air Cabin Group Item Changes
				 if (updatedClient.PolicyAirCabinGroupItemsAdded != null)
				 {
					 if (updatedClient.PolicyAirCabinGroupItemsAdded.Count > 0)
					 {
						 wizardMessages.AddMessage("You will add Policy Air Cabin Group Item(s) to the ClientSubUnit.", true);
					 }

				 }
				 if (updatedClient.PolicyAirCabinGroupItemsAltered != null)
				 {
					 if (updatedClient.PolicyAirCabinGroupItemsAltered.Count > 0)
					 {
						 wizardMessages.AddMessage("You will alter Policy Air Cabin Group Item(s) of the ClientSubUnit.", true);
					 }
				 }
				 if (updatedClient.PolicyAirCabinGroupItemsRemoved != null)
				 {
					 if (updatedClient.PolicyAirCabinGroupItemsRemoved.Count > 0)
					 {
						 wizardMessages.AddMessage("You will remove Policy Air Cabin Group Item(s) from the ClientSubUnit.", true);
					 }
				 }
				 #endregion

                 #region Policy Air Missed Savings Threshold Group Item Changes
                 if (updatedClient.PolicyAirMissedSavingsThresholdGroupItemsAdded != null)
                 {
                     if (updatedClient.PolicyAirMissedSavingsThresholdGroupItemsAdded.Count > 0)
                     {
                         wizardMessages.AddMessage("You will add Policy Air Missed Savings Threshold Item(s) to the ClientSubUnit.", true);
                     }

                 }
                 if (updatedClient.PolicyAirMissedSavingsThresholdGroupItemsAltered != null)
                 {
                     if (updatedClient.PolicyAirMissedSavingsThresholdGroupItemsAltered.Count > 0)
                     {
                         wizardMessages.AddMessage("You will alter Policy Air Missed Savings Threshold Group Item(s) of the ClientSubUnit.", true);
                     }
                 }
                 if (updatedClient.PolicyAirMissedSavingsThresholdGroupItemsRemoved != null)
                 {
                     if (updatedClient.PolicyAirMissedSavingsThresholdGroupItemsRemoved.Count > 0)
                     {
                         wizardMessages.AddMessage("You will remove Policy Air Missed Savings Threshold Group Item(s) from the ClientSubUnit.", true);
                     }
                 }
                 #endregion

                 #region Policy Air Vendor Group Item Changes
                 if (updatedClient.PolicyAirVendorGroupItemsAdded != null)
                 {
                     if (updatedClient.PolicyAirVendorGroupItemsAdded.Count > 0)
                     {
                         wizardMessages.AddMessage("You will add Policy Air Vendor Group Item(s) to the ClientSubUnit.", true);
                     }
                 }
                 if (updatedClient.PolicyAirVendorGroupItemsAltered != null)
                 {
                     if (updatedClient.PolicyAirVendorGroupItemsAltered.Count > 0)
                     {
                         wizardMessages.AddMessage("You will alter Policy Air Vendor Group Item(s) of the ClientSubUnit.", true);
                     }
                 }
                 if (updatedClient.PolicyAirVendorGroupItemsRemoved != null)
                 {
                     if (updatedClient.PolicyAirVendorGroupItemsRemoved.Count > 0)
                     {
                         wizardMessages.AddMessage("You will remove Policy Air Vendor Group Item(s) from the ClientSubUnit.", true);
                     }
                 }
                 #endregion

                 #region  Policy Car Type Group Item Changes
                 if (updatedClient.PolicyCarTypeGroupItemsAdded != null)
                 {
                     if (updatedClient.PolicyCarTypeGroupItemsAdded.Count > 0)
                     {
                         wizardMessages.AddMessage("You will add Policy Car Type Group Item(s) to the ClientSubUnit.", true);
                     }
                 }
                 if (updatedClient.PolicyCarTypeGroupItemsAltered != null)
                 {
                     if (updatedClient.PolicyCarTypeGroupItemsAltered.Count > 0)
                     {
                         wizardMessages.AddMessage("You will alter Policy Car Type Group Item(s) of the ClientSubUnit.", true);
                     }
                 }
                 if (updatedClient.PolicyCarTypeGroupItemsRemoved != null)
                 {
                     if (updatedClient.PolicyCarTypeGroupItemsRemoved.Count > 0)
                     {
                         wizardMessages.AddMessage("You will remove Policy Car Type Group Item(s) from the ClientSubUnit.", true);
                     }
                 }
                 #endregion

                 #region Policy Car Vendor Group Item Changes
                 if (updatedClient.PolicyCarVendorGroupItemsAdded != null)
                 {
                     if (updatedClient.PolicyCarVendorGroupItemsAdded.Count > 0)
                     {
                         wizardMessages.AddMessage("You will add Policy Car Vendor Group Item(s) to the ClientSubUnit.", true);
                     }
                 }
                 if (updatedClient.PolicyCarVendorGroupItemsAltered != null)
                 {
                     if (updatedClient.PolicyCarVendorGroupItemsAltered.Count > 0)
                     {
                         wizardMessages.AddMessage("You will alter Policy Car Vendor Group Item(s) of the ClientSubUnit.", true);
                     }
                 }
                 if (updatedClient.PolicyCarVendorGroupItemsRemoved != null)
                 {
                     if (updatedClient.PolicyCarVendorGroupItemsRemoved.Count > 0)
                     {
                         wizardMessages.AddMessage("You will remove Policy Car Vendor Group Item(s) of the ClientSubUnit.", true);
                     }
                 }
                 #endregion

                 #region Policy City Group Item Changes
                 if (updatedClient.PolicyCityGroupItemsAdded != null)
                 {
                     if (updatedClient.PolicyCityGroupItemsAdded.Count > 0)
                     {
                         wizardMessages.AddMessage("You will add Policy City Group Item(s) to the ClientSubUnit.", true);
                     }
                 }
                 if (updatedClient.PolicyCityGroupItemsAltered != null)
                 {
                     if (updatedClient.PolicyCityGroupItemsAltered.Count > 0)
                     {
                         wizardMessages.AddMessage("You will alter Policy City Group Item(s) of the ClientSubUnit.", true);
                     }
                 }
                 if (updatedClient.PolicyCityGroupItemsRemoved != null)
                 {
                     if (updatedClient.PolicyCityGroupItemsRemoved.Count > 0)
                     {
                         wizardMessages.AddMessage("You will remove Policy City Group Item(s) from the ClientSubUnit.", true);
                     }
                 }
                 #endregion

                 #region Policy Country Group Item Changes
                 if (updatedClient.PolicyCountryGroupItemsAdded != null)
                 {
                     if (updatedClient.PolicyCountryGroupItemsAdded.Count > 0)
                     {
                         wizardMessages.AddMessage("You will add Policy Country Group Item(s) to the ClientSubUnit.", true);
                     }
                 }
                 if (updatedClient.PolicyCountryGroupItemsAltered != null)
                 {
                     if (updatedClient.PolicyCountryGroupItemsAltered.Count > 0)
                     {
                         wizardMessages.AddMessage("You will alter Policy Country Group Item(s) of the ClientSubUnit.", true);
                     }
                 }
                 if (updatedClient.PolicyCountryGroupItemsRemoved != null)
                 {
                     if (updatedClient.PolicyCountryGroupItemsRemoved.Count > 0)
                     {
                         wizardMessages.AddMessage("You will remove Policy Country Group Item(s) from the ClientSubUnit.", true);
                     }
                 }
                 #endregion

                 #region Policy Hotel Cap Rate Group Item Changes
                 if (updatedClient.PolicyHotelCapRateGroupItemsAdded != null)
                 {
                     if (updatedClient.PolicyHotelCapRateGroupItemsAdded.Count > 0)
                     {
                         wizardMessages.AddMessage("You will add Policy Hotel Cap Rate Group Item(s) to the ClientSubUnit.", true);
                     }
                 }
                 if (updatedClient.PolicyHotelCapRateGroupItemsAltered != null)
                 {
                     if (updatedClient.PolicyHotelCapRateGroupItemsAltered.Count > 0)
                     {
                         wizardMessages.AddMessage("You will alter Policy Hotel Cap Rate Group Item(s) of the ClientSubUnit.", true);
                     }
                 }
                 if (updatedClient.PolicyHotelCapRateGroupItemsRemoved != null)
                 {
                     if (updatedClient.PolicyHotelCapRateGroupItemsRemoved.Count > 0)
                     {
                         wizardMessages.AddMessage("You will remove Policy Hotel Cap Rate Group Item(s) from the ClientSubUnit.", true);
                     }
                 }
                 #endregion

                 #region Policy Hotel Property Group Item Changes
                 if (updatedClient.PolicyHotelPropertyGroupItemsAdded != null)
                 {
                     if (updatedClient.PolicyHotelPropertyGroupItemsAdded.Count > 0)
                     {
                         wizardMessages.AddMessage("You will add Policy Hotel Property Group Item(s) to the ClientSubUnit.", true);
                     }
                 }
                 if (updatedClient.PolicyHotelPropertyGroupItemsAltered != null)
                 {
                     if (updatedClient.PolicyHotelPropertyGroupItemsAltered.Count > 0)
                     {
                         wizardMessages.AddMessage("You will alter Policy Hotel Property Group Item(s) of the ClientSubUnit.", true);
                     }
                 }
                 if (updatedClient.PolicyHotelPropertyGroupItemsRemoved != null)
                 {
                     if (updatedClient.PolicyHotelPropertyGroupItemsRemoved.Count > 0)
                     {
                         wizardMessages.AddMessage("You will remove Policy Hotel Property Group Item(s) from the ClientSubUnit.", true);
                     }
                 }
                 #endregion

                 #region Policy Hotel Vendor Group Item Changes
                 if (updatedClient.PolicyHotelVendorGroupItemsAdded != null)
                 {
                     if (updatedClient.PolicyHotelVendorGroupItemsAdded.Count > 0)
                     {
                         wizardMessages.AddMessage("You will add Policy Hotel Vendor Group Item(s) to the ClientSubUnit.", true);
                     }
                 }
                 if (updatedClient.PolicyHotelVendorGroupItemsAltered != null)
                 {
                     if (updatedClient.PolicyHotelVendorGroupItemsAltered.Count > 0)
                     {
                         wizardMessages.AddMessage("You will alter Policy Hotel Vendor Group Item(s) of the ClientSubUnit.", true);
                     }
                 }
                 if (updatedClient.PolicyHotelVendorGroupItemsRemoved != null)
                 {
                     if (updatedClient.PolicyHotelVendorGroupItemsRemoved.Count > 0)
                     {
                         wizardMessages.AddMessage("You will remove Policy Hotel Vendor Group Item(s) from the ClientSubUnit.", true);
                     }
                 }
                 #endregion

                 #region Policy Supplier Deal Codes Changes
                 if (updatedClient.PolicySupplierDealCodesAdded != null)
                 {
                     if (updatedClient.PolicySupplierDealCodesAdded.Count > 0)
                     {
                         wizardMessages.AddMessage("You will add Policy Supplier Deal Code Item(s) to the ClientSubUnit.", true);
                     }
                 }
                 if (updatedClient.PolicySupplierDealCodesAltered != null)
                 {
                     if (updatedClient.PolicySupplierDealCodesAltered.Count > 0)
                     {
                         wizardMessages.AddMessage("You will alter Policy Supplier Deal Code Item(s) of the ClientSubUnit.", true);
                     }
                 }
                 if (updatedClient.PolicySupplierDealCodesRemoved != null)
                 {
                     if (updatedClient.PolicySupplierDealCodesRemoved.Count > 0)
                     {
                         wizardMessages.AddMessage("You will remove Policy Supplier Deal Code Item(s) from the ClientSubUnit.", true);
                     }
                 }
                 #endregion

                 #region Policy Supplier Service Information Changes
                 if (updatedClient.PolicySupplierServiceInformationsAdded != null)
                 {
                     if (updatedClient.PolicySupplierServiceInformationsAdded.Count > 0)
                     {
                         wizardMessages.AddMessage("You will add Policy Supplier Service Information Item(s) to the ClientSubUnit.", true);
                     }
                 }
                 if (updatedClient.PolicySupplierServiceInformationsAltered != null)
                 {
                     if (updatedClient.PolicySupplierServiceInformationsAltered.Count > 0)
                     {
                         wizardMessages.AddMessage("You will alter Policy Supplier Service Information Item(s) of the ClientSubUnit.", true);
                     }
                 }
                 if (updatedClient.PolicySupplierServiceInformationsRemoved != null)
                 {
                     if (updatedClient.PolicySupplierServiceInformationsRemoved.Count > 0)
                     {
                         wizardMessages.AddMessage("You will remove Policy Supplier Service Information Item(s) from the ClientSubUnit.", true);
                     }
                 }
                 #endregion
             }
             #endregion

            //Return a list of messages to the User
            return wizardMessages;
        }

        //Update ClientSubUnit
        public void UpdateClientSubUnit(ClientSubUnit clientSubUnit)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			
			LogRepository logRepository = new LogRepository(); 
			string computerName = logRepository.GetComputerName();

            db.spDDAWizard_UpdateClientSubUnit_v1(
                clientSubUnit.ClientSubUnitGuid,
                clientSubUnit.PortraitStatusId,
                clientSubUnit.LineOfBusinessId,
                clientSubUnit.ClientSubUnitDisplayName,
                adminUserGuid,
				clientSubUnit.RestrictedClient,
				clientSubUnit.PrivateClient,
				clientSubUnit.CubaBookingAllowed,
				clientSubUnit.InCountryServiceOnly,
				clientSubUnit.DialledNumber24HSC,
				clientSubUnit.BranchContactNumber,
				clientSubUnit.BranchFaxNumber,
				clientSubUnit.BranchEmail,
				clientSubUnit.ClientIATA,
                clientSubUnit.PortraitStatusDescription,
                clientSubUnit.ClientBusinessDescription,
                Settings.ApplicationName(),
				Settings.ApplicationVersion(),
				computerName,
                clientSubUnit.VersionNumber
            );
        }

        //Update ClientSubUnit Teams
        public WizardMessages UpdateClientSubUnitTeams(ClientWizardVM updatedClient, WizardMessages wizardMessages)
        {
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = updatedClient.ClientSubUnit;
        
            // Create the xml document container
            XmlDocument doc = new XmlDocument();// Create the XML Declaration, and append it to XML document
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(dec);
            XmlElement root = doc.CreateElement("ClientSubUnitTeams");
            doc.AppendChild(root);

            bool changesExist = false;
            TeamRepository teamRepository = new TeamRepository();

            if (updatedClient.TeamsAdded != null)
            {
                if (updatedClient.TeamsAdded.Count > 0)
                {
                    changesExist = true;
                    XmlElement xmlTeamsAdded = doc.CreateElement("TeamsAdded");

                    foreach (ClientSubUnitTeam item in updatedClient.TeamsAdded)
                    {
                        Team team = new Team();
                        team = teamRepository.GetTeam(item.TeamId);

                        XmlElement xmlTeam = doc.CreateElement("Team");
                        xmlTeamsAdded.AppendChild(xmlTeam);

                        XmlElement xmlTeamName = doc.CreateElement("TeamName");
                        xmlTeamName.InnerText = team.TeamName;
                        xmlTeam.AppendChild(xmlTeamName);

						XmlElement xmlTeamId = doc.CreateElement("TeamId");
						xmlTeamId.InnerText = item.TeamId.ToString();
						xmlTeam.AppendChild(xmlTeamId);

						XmlElement xmlIsPrimaryTeamForSub = doc.CreateElement("IsPrimaryTeamForSub");
						xmlIsPrimaryTeamForSub.InnerText = item.IsPrimaryTeamForSub.ToString();
						xmlTeam.AppendChild(xmlIsPrimaryTeamForSub); 

                        XmlElement xmlIncludeInClientDroplistFlag = doc.CreateElement("IncludeInClientDroplistFlag");
                        xmlIncludeInClientDroplistFlag.InnerText = item.IncludeInClientDroplistFlag == true ? "1" : "0";
                        xmlTeam.AppendChild(xmlIncludeInClientDroplistFlag);

                    }
                    root.AppendChild(xmlTeamsAdded);
                }
            }
            if (updatedClient.TeamsRemoved != null)
            {
                if (updatedClient.TeamsRemoved.Count > 0)
                {
                    changesExist = true;
                    XmlElement xmlTeamsRemoved = doc.CreateElement("TeamsRemoved");

                    foreach (ClientSubUnitTeam item in updatedClient.TeamsRemoved)
                    {
                        Team team = new Team();
                        team = teamRepository.GetTeam(item.TeamId);

                        XmlElement xmlTeam = doc.CreateElement("Team");
                        xmlTeamsRemoved.AppendChild(xmlTeam);

                        XmlElement xmlTeamName = doc.CreateElement("TeamName");
                        xmlTeamName.InnerText = team.TeamName;
                        xmlTeam.AppendChild(xmlTeamName);

                        XmlElement xmlTeamId = doc.CreateElement("TeamId");
                        xmlTeamId.InnerText = item.TeamId.ToString();
                        xmlTeam.AppendChild(xmlTeamId);

						XmlElement xmlIsPrimaryTeamForSub = doc.CreateElement("IsPrimaryTeamForSub");
						xmlIsPrimaryTeamForSub.InnerText = item.IsPrimaryTeamForSub.ToString();
						xmlTeam.AppendChild(xmlIsPrimaryTeamForSub); 

                        XmlElement xmlVersionNumber = doc.CreateElement("VersionNumber");
                        xmlVersionNumber.InnerText = item.VersionNumber.ToString();
                        xmlTeam.AppendChild(xmlVersionNumber);

                    }
                    root.AppendChild(xmlTeamsRemoved);
                }
            }
            if (updatedClient.TeamsAltered != null)
            {
                if (updatedClient.TeamsAltered.Count > 0)
                {
                    changesExist = true;
                    XmlElement xmlTeamsAltered = doc.CreateElement("TeamsAltered");

                    foreach (ClientSubUnitTeam item in updatedClient.TeamsAltered)
                    {
                        Team team = new Team();
                        team = teamRepository.GetTeam(item.TeamId);

                        XmlElement xmlTeam = doc.CreateElement("Team");
                        xmlTeamsAltered.AppendChild(xmlTeam);

                        XmlElement xmlTeamName = doc.CreateElement("TeamName");
                        xmlTeamName.InnerText = team.TeamName;
                        xmlTeam.AppendChild(xmlTeamName);

                        XmlElement xmlTeamId = doc.CreateElement("TeamId");
                        xmlTeamId.InnerText = item.TeamId.ToString();
                        xmlTeam.AppendChild(xmlTeamId);

						XmlElement xmlIsPrimaryTeamForSub = doc.CreateElement("IsPrimaryTeamForSub");
						xmlIsPrimaryTeamForSub.InnerText = item.IsPrimaryTeamForSub.ToString();
						xmlTeam.AppendChild(xmlIsPrimaryTeamForSub); 
						
						XmlElement xmlIncludeInClientDroplistFlag = doc.CreateElement("IncludeInClientDroplistFlag");
                        xmlIncludeInClientDroplistFlag.InnerText = item.IncludeInClientDroplistFlag == true ? "1" : "0";
                        xmlTeam.AppendChild(xmlIncludeInClientDroplistFlag);

                        XmlElement xmlVersionNumber = doc.CreateElement("VersionNumber");
                        xmlVersionNumber.InnerText = item.VersionNumber.ToString();
                        xmlTeam.AppendChild(xmlVersionNumber);

                    }
                    root.AppendChild(xmlTeamsAltered);
                }
            }

            if (changesExist)
            {
                string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

                var output = (from n in db.spDDAWizard_UpdateClientSubUnitTeams_v1(
                    clientSubUnit.ClientSubUnitGuid,
                    System.Xml.Linq.XElement.Parse(doc.OuterXml),
                    adminUserGuid)
                              select n).ToList();

                foreach (spDDAWizard_UpdateClientSubUnitTeams_v1Result message in output)
                {
                    wizardMessages.AddMessage(message.MessageText.ToString(), (bool)message.Success);
                }
            }
            return wizardMessages;
        }

         //Update ClientSubUnit Telephonies
        public WizardMessages UpdateClientSubUnitTelephonies(ClientWizardVM updatedClient, WizardMessages wizardMessages)
        {
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = updatedClient.ClientSubUnit;
        
            // Create the xml document container
            XmlDocument doc = new XmlDocument();// Create the XML Declaration, and append it to XML document
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(dec);
            XmlElement root = doc.CreateElement("ClientSubUnitTelephonies");
            doc.AppendChild(root);

            bool changesExist = false;
            if (updatedClient.ClientSubUnitTelephoniesAdded != null)
            {
                if (updatedClient.ClientSubUnitTelephoniesAdded.Count > 0)
                {
                    changesExist = true;
                    XmlElement xmlClientSubUnitTelephoniesAdded = doc.CreateElement("ClientSubUnitTelephoniesAdded");

                    foreach (ClientSubUnitTelephony item in updatedClient.ClientSubUnitTelephoniesAdded)
                    {
                        XmlElement xmlClientSubUnitTelephony = doc.CreateElement("ClientSubUnitTelephony");
                        xmlClientSubUnitTelephoniesAdded.AppendChild(xmlClientSubUnitTelephony);

                        XmlElement xmlDialedNumber = doc.CreateElement("DialedNumber");
                        xmlDialedNumber.InnerText = item.DialedNumber.ToString();
                        xmlClientSubUnitTelephony.AppendChild(xmlDialedNumber);

                        XmlElement xmlIncludeInClientDroplistFlag = doc.CreateElement("CallerEnteredDigitDefinitionTypeId");
                        xmlIncludeInClientDroplistFlag.InnerText = item.CallerEnteredDigitDefinitionTypeId.ToString();
                        xmlClientSubUnitTelephony.AppendChild(xmlIncludeInClientDroplistFlag);
                    }
                    root.AppendChild(xmlClientSubUnitTelephoniesAdded);
                }
            }

            if (updatedClient.ClientSubUnitTelephoniesRemoved !=null)
            {
                if (updatedClient.ClientSubUnitTelephoniesRemoved.Count > 0)
                {
                    changesExist = true;
                    XmlElement xmlClientSubUnitTelephoniesRemoved = doc.CreateElement("ClientSubUnitTelephoniesRemoved");

                    foreach (ClientSubUnitTelephony item in updatedClient.ClientSubUnitTelephoniesRemoved)
                    {
                        XmlElement xmlClientSubUnitTelephony = doc.CreateElement("ClientSubUnitTelephony");
                        xmlClientSubUnitTelephoniesRemoved.AppendChild(xmlClientSubUnitTelephony);

                        XmlElement xmlDialedNumber = doc.CreateElement("DialedNumber");
                        xmlDialedNumber.InnerText = item.DialedNumber.ToString();
                        xmlClientSubUnitTelephony.AppendChild(xmlDialedNumber);

                        XmlElement xmlVersionNumber = doc.CreateElement("VersionNumber");
                        xmlVersionNumber.InnerText = item.VersionNumber.ToString();
                        xmlClientSubUnitTelephony.AppendChild(xmlVersionNumber);

                    }
                    root.AppendChild(xmlClientSubUnitTelephoniesRemoved);
                }
            }

            if (changesExist)
            {
                string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

                var output = (from n in db.spDDAWizard_UpdateClientSubUnitTelephonies_v1(
                    clientSubUnit.ClientSubUnitGuid,
                    System.Xml.Linq.XElement.Parse(doc.OuterXml),
                    adminUserGuid)
                              select n).ToList();

                foreach (spDDAWizard_UpdateClientSubUnitTelephonies_v1Result message in output)
                {
                    wizardMessages.AddMessage(message.MessageText.ToString(), (bool)message.Success);
                }
            }
            return wizardMessages;
        }
        
        //Update ClientSubUnit ClientAccounts
        public WizardMessages UpdateClientSubUnitClientAccounts(ClientWizardVM updatedClient, WizardMessages wizardMessages)
        {
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = updatedClient.ClientSubUnit;

            // Create the xml document container
            XmlDocument doc = new XmlDocument();// Create the XML Declaration, and append it to XML document
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(dec);
            XmlElement root = doc.CreateElement("ClientSubUnitClientAccounts");
            doc.AppendChild(root);

            bool changesExist = false;
            ClientAccountRepository clientAccountRepository = new ClientAccountRepository();

            if (updatedClient.ClientAccountsAdded != null)
            {
                if (updatedClient.ClientAccountsAdded.Count > 0)
                {
                    changesExist = true;
                    XmlElement xmlClientAccountsAdded = doc.CreateElement("ClientAccountsAdded");
                    //xml = xml + "<ClientAccountsAdded>";

                    foreach (ClientSubUnitClientAccount item in updatedClient.ClientAccountsAdded)
                    {
                        ClientAccount clientAccount = new ClientAccount();
                        clientAccount = clientAccountRepository.GetClientAccount(item.ClientAccountNumber, item.SourceSystemCode);
                        //xml = xml + "<ClientAccount>";
                        //xml = xml + "<SourceSystemCode>" + item.SourceSystemCode + "</SourceSystemCode>";
                        //xml = xml + "<ClientAccountNumber>" + item.ClientAccountNumber + "</ClientAccountNumber>";
                        //xml = xml + "<ClientAccountName>" + clientAccount.ClientAccountName + "</ClientAccountName>";
                        //xml = xml + "<ConfidenceLevelForLoadId>" + item.ConfidenceLevelForLoadId + "</ConfidenceLevelForLoadId>";
                        //xml = xml + "</ClientAccount>";

                        XmlElement xmlClientAccount = doc.CreateElement("ClientAccount");
                        xmlClientAccountsAdded.AppendChild(xmlClientAccount);

                        XmlElement xmlSourceSystemCode = doc.CreateElement("SourceSystemCode");
                        xmlSourceSystemCode.InnerText = item.SourceSystemCode;
                        xmlClientAccount.AppendChild(xmlSourceSystemCode);

                        XmlElement xmlClientAccountNumber = doc.CreateElement("ClientAccountNumber");
                        xmlClientAccountNumber.InnerText = item.ClientAccountNumber;
                        xmlClientAccount.AppendChild(xmlClientAccountNumber);

                        XmlElement xmlClientAccountName = doc.CreateElement("ClientAccountName");
                        xmlClientAccountName.InnerText = clientAccount.ClientAccountName;
                        xmlClientAccount.AppendChild(xmlClientAccountName);

                        XmlElement xmlConfidenceLevelForLoadId = doc.CreateElement("ConfidenceLevelForLoadId");
                        xmlConfidenceLevelForLoadId.InnerText = item.ConfidenceLevelForLoadId.ToString();
                        xmlClientAccount.AppendChild(xmlConfidenceLevelForLoadId);

                    }
                    root.AppendChild(xmlClientAccountsAdded);
                    //xml = xml + "</ClientAccountsAdded>";
                }
            }
            if (updatedClient.ClientAccountsRemoved != null)
            {
                if (updatedClient.ClientAccountsRemoved.Count > 0)
                {
                    changesExist = true;
                    XmlElement xmlClientAccountsRemoved = doc.CreateElement("ClientAccountsRemoved");

                    foreach (ClientSubUnitClientAccount item in updatedClient.ClientAccountsRemoved)
                    {
                        ClientAccount clientAccount = new ClientAccount();
                        clientAccount = clientAccountRepository.GetClientAccount(item.ClientAccountNumber, item.SourceSystemCode);
                        //xml = xml + "<ClientAccount>";
                        //xml = xml + "<SourceSystemCode>" + item.SourceSystemCode + "</SourceSystemCode>";
                        //xml = xml + "<ClientAccountNumber>" + item.ClientAccountNumber + "</ClientAccountNumber>";
                        //xml = xml + "<ClientAccountName>" + clientAccount.ClientAccountName + "</ClientAccountName>";
                        //xml = xml + "<VersionNumber>" + item.VersionNumber + "</VersionNumber>";
                        //xml = xml + "</ClientAccount>";

                        XmlElement xmlClientAccount = doc.CreateElement("ClientAccount");
                        xmlClientAccountsRemoved.AppendChild(xmlClientAccount);

                        XmlElement xmlSourceSystemCode = doc.CreateElement("SourceSystemCode");
                        xmlSourceSystemCode.InnerText = item.SourceSystemCode;
                        xmlClientAccount.AppendChild(xmlSourceSystemCode);

                        XmlElement xmlClientAccountNumber = doc.CreateElement("ClientAccountNumber");
                        xmlClientAccountNumber.InnerText = item.ClientAccountNumber;
                        xmlClientAccount.AppendChild(xmlClientAccountNumber);

                        XmlElement xmlClientAccountName = doc.CreateElement("ClientAccountName");
                        xmlClientAccountName.InnerText = clientAccount.ClientAccountName;
                        xmlClientAccount.AppendChild(xmlClientAccountName);

                        XmlElement xmlVersionNumber = doc.CreateElement("VersionNumber");
                        xmlVersionNumber.InnerText = item.VersionNumber.ToString();
                        xmlClientAccount.AppendChild(xmlVersionNumber);
                    }
                    root.AppendChild(xmlClientAccountsRemoved);
                    //xml = xml + "</ClientAccountsRemoved>";
                }
            }
            if (updatedClient.ClientAccountsAltered != null)
            {
                if (updatedClient.ClientAccountsAltered.Count > 0)
                {
                    changesExist = true;
                    XmlElement xmlClientAccountsAltered = doc.CreateElement("ClientAccountsAltered");

                    foreach (ClientSubUnitClientAccount item in updatedClient.ClientAccountsAltered)
                    {
                        ClientAccount clientAccount = new ClientAccount();
                        clientAccount = clientAccountRepository.GetClientAccount(item.ClientAccountNumber, item.SourceSystemCode);
                        //xml = xml + "<ClientAccount>";
                        //xml = xml + "<SourceSystemCode>" + item.SourceSystemCode + "</SourceSystemCode>";
                        //xml = xml + "<ClientAccountNumber>" + item.ClientAccountNumber + "</ClientAccountNumber>";
                        //xml = xml + "<ClientAccountName>" + clientAccount.ClientAccountName + "</ClientAccountName>";
                        //xml = xml + "<ConfidenceLevelForLoadId>" + item.ConfidenceLevelForLoadId + "</ConfidenceLevelForLoadId>";
                        //xml = xml + "<VersionNumber>" + item.VersionNumber + "</VersionNumber>";
                        //xml = xml + "</ClientAccount>";

                        XmlElement xmlClientAccount = doc.CreateElement("ClientAccount");
                        xmlClientAccountsAltered.AppendChild(xmlClientAccount);

                        XmlElement xmlSourceSystemCode = doc.CreateElement("SourceSystemCode");
                        xmlSourceSystemCode.InnerText = item.SourceSystemCode;
                        xmlClientAccount.AppendChild(xmlSourceSystemCode);

                        XmlElement xmlClientAccountNumber = doc.CreateElement("ClientAccountNumber");
                        xmlClientAccountNumber.InnerText = item.ClientAccountNumber;
                        xmlClientAccount.AppendChild(xmlClientAccountNumber);

                        XmlElement xmlClientAccountName = doc.CreateElement("ClientAccountName");
                        xmlClientAccountName.InnerText = clientAccount.ClientAccountName;
                        xmlClientAccount.AppendChild(xmlClientAccountName);

                        XmlElement xmlConfidenceLevelForLoadId = doc.CreateElement("ConfidenceLevelForLoadId");
                        xmlConfidenceLevelForLoadId.InnerText = item.ConfidenceLevelForLoadId.ToString();
                        xmlClientAccount.AppendChild(xmlConfidenceLevelForLoadId);

                        XmlElement xmlVersionNumber = doc.CreateElement("VersionNumber");
                        xmlVersionNumber.InnerText = item.VersionNumber.ToString();
                        xmlClientAccount.AppendChild(xmlVersionNumber);

                    }
                    //xml = xml + "</ClientAccountsAltered>";
                    root.AppendChild(xmlClientAccountsAltered);
                }
            }
            //xml = xml + "</ClientSubUnitClientAccounts>";

            if (changesExist)
            {
                string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

                var output = (from n in db.spDDAWizard_UpdateClientSubUnitClientAccounts_v1(
                    clientSubUnit.ClientSubUnitGuid,
                    System.Xml.Linq.XElement.Parse(doc.OuterXml),
                    adminUserGuid)
                              select n).ToList();

                foreach (spDDAWizard_UpdateClientSubUnitClientAccounts_v1Result message in output)
                {
                    wizardMessages.AddMessage(message.MessageText.ToString(), (bool)message.Success);
                }
                 
            }
            return wizardMessages;
        }

        //Update ClientSubUnit Reason Codes
        public WizardMessages UpdateClientSubUnitReasonCodes(ClientWizardVM updatedClient, WizardMessages wizardMessages)
        {
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = updatedClient.ClientSubUnit;

            // Create the xml document container
            XmlDocument doc = new XmlDocument();// Create the XML Declaration, and append it to XML document
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(dec);
            XmlElement root = doc.CreateElement("ClientSubUnitReasonCodes");
            doc.AppendChild(root);

            bool changesExist = false;
            XmlElement xmlClientSubUnitReasonCodes = doc.CreateElement("ClientSubUnitReasonCodes");

            ReasonCodeItemRepository reasonCodeItemRepository = new ReasonCodeItemRepository();

            if (updatedClient.ReasonCodesAdded != null)
            {
                if (updatedClient.ReasonCodesAdded.Count > 0)
                {
                    changesExist = true;
                    XmlElement xmlReasonCodeItemsAdded = doc.CreateElement("ReasonCodeItemsAdded");
                    foreach (ReasonCodeItem item in updatedClient.ReasonCodesAdded)
                    {

                        XmlElement xmlReasonCodeItem = doc.CreateElement("ReasonCodeItem");
                        xmlReasonCodeItemsAdded.AppendChild(xmlReasonCodeItem);

                        XmlElement xmlReasonCode = doc.CreateElement("ReasonCode");
                        xmlReasonCode.InnerText = item.ReasonCode;
                        xmlReasonCodeItem.AppendChild(xmlReasonCode);

                        XmlElement xmlProductId = doc.CreateElement("ProductId");
                        xmlProductId.InnerText = item.ProductId.ToString();
                        xmlReasonCodeItem.AppendChild(xmlProductId);

                        XmlElement xmlReasonCodeTypeId = doc.CreateElement("ReasonCodeTypeId");
                        xmlReasonCodeTypeId.InnerText = item.ReasonCodeTypeId.ToString();
                        xmlReasonCodeItem.AppendChild(xmlReasonCodeTypeId);

                        XmlElement xmlReasonCodeGroupId = doc.CreateElement("ReasonCodeGroupId");
                        xmlReasonCodeGroupId.InnerText = item.ReasonCodeGroupId.ToString();
                        xmlReasonCodeItem.AppendChild(xmlReasonCodeGroupId);
        
                    }
                    root.AppendChild(xmlReasonCodeItemsAdded);
                }
            }
            
			if (updatedClient.ReasonCodeItemsRemoved != null)
            {
                if (updatedClient.ReasonCodeItemsRemoved.Count > 0)
                {
                    changesExist = true;
                    XmlElement xmlReasonCodeItemsRemoved = doc.CreateElement("ReasonCodeItemsRemoved");

                    foreach (ReasonCodeItem item in updatedClient.ReasonCodeItemsRemoved)
                    {

                        XmlElement xmlReasonCodeItem = doc.CreateElement("ReasonCodeItem");
                        xmlReasonCodeItemsRemoved.AppendChild(xmlReasonCodeItem);

                        XmlElement xmlReasonCodeItemId = doc.CreateElement("ReasonCodeItemId");
                        xmlReasonCodeItemId.InnerText = item.ReasonCodeItemId.ToString();
                        xmlReasonCodeItem.AppendChild(xmlReasonCodeItemId);

                        XmlElement xmlVersionNumber = doc.CreateElement("VersionNumber");
                        xmlVersionNumber.InnerText = item.VersionNumber.ToString();
                        xmlReasonCodeItem.AppendChild(xmlVersionNumber);
                    }
                    root.AppendChild(xmlReasonCodeItemsRemoved);
                }
            }

			if (updatedClient.ReasonCodeItemsAltered != null)
			{
				if (updatedClient.ReasonCodeItemsAltered.Count > 0)
				{
					changesExist = true;
					XmlElement xmlReasonCodeItemsAltered = doc.CreateElement("ReasonCodeItemsAltered");

					foreach (ReasonCodeItem item in updatedClient.ReasonCodeItemsAltered)
					{

						XmlElement xmlReasonCodeItem = doc.CreateElement("ReasonCodeItem");
						xmlReasonCodeItemsAltered.AppendChild(xmlReasonCodeItem);

						XmlElement xmlReasonCodeItemId = doc.CreateElement("ReasonCodeItemId");
						xmlReasonCodeItemId.InnerText = item.ReasonCodeItemId.ToString();
						xmlReasonCodeItem.AppendChild(xmlReasonCodeItemId);

						XmlElement xmlVersionNumber = doc.CreateElement("VersionNumber");
						xmlVersionNumber.InnerText = item.VersionNumber.ToString();
						xmlReasonCodeItem.AppendChild(xmlVersionNumber); 
						
						XmlElement xmlReasonCode = doc.CreateElement("ReasonCode");
						xmlReasonCode.InnerText = item.ReasonCode;
						xmlReasonCodeItem.AppendChild(xmlReasonCode);

						XmlElement xmlProductId = doc.CreateElement("ProductId");
						xmlProductId.InnerText = item.ProductId.ToString();
						xmlReasonCodeItem.AppendChild(xmlProductId);

						XmlElement xmlReasonCodeTypeId = doc.CreateElement("ReasonCodeTypeId");
						xmlReasonCodeTypeId.InnerText = item.ReasonCodeTypeId.ToString();
						xmlReasonCodeItem.AppendChild(xmlReasonCodeTypeId);

						XmlElement xmlReasonCodeGroupId = doc.CreateElement("ReasonCodeGroupId");
						xmlReasonCodeGroupId.InnerText = item.ReasonCodeGroupId.ToString();
						xmlReasonCodeItem.AppendChild(xmlReasonCodeGroupId);
					}
					root.AppendChild(xmlReasonCodeItemsAltered);
				}
			}

            if (changesExist)
            {
                string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

                var output = (from n in db.spDDAWizard_UpdateClientSubUnitReasonCodes_v1(
                    clientSubUnit.ClientSubUnitGuid,
                    System.Xml.Linq.XElement.Parse(doc.OuterXml),
                    adminUserGuid)
                              select n).ToList();

                foreach (spDDAWizard_UpdateClientSubUnitReasonCodes_v1Result message in output)
                {
                    ReasonCodeProductTypeDescription reasonCodeProductTypeDescription = new ReasonCodeProductTypeDescription();
                    ReasonCodeProductType reasonCodeProductType = new ReasonCodeProductType();

                    if (message.ReasonCode != null)
                    {
                        ReasonCodeProductTypeRepository reasonCodeProductTypeRepository = new ReasonCodeProductTypeRepository();      
                        reasonCodeProductType = reasonCodeProductTypeRepository.GetReasonCodeProductType(message.ReasonCode, message.ProductId, message.ReasonCodeTypeId);

                        //now we get the description (may not exist)
                        ReasonCodeProductTypeDescriptionRepository reasonCodeProductTypeDescriptionRepository = new ReasonCodeProductTypeDescriptionRepository();                        
                        reasonCodeProductTypeDescription = reasonCodeProductTypeDescriptionRepository.GetItem("en-GB", message.ReasonCode, (int)message.ProductId, (int)message.ReasonCodeTypeId);


                    }
                    if (message.MessageText == "ADDED")
                    {
                        if (reasonCodeProductTypeDescription != null)
                        {
                            wizardMessages.AddMessage("You have added ReasonCode ProductType \"" + reasonCodeProductTypeDescription.ReasonCodeProductTypeDescription1 + "\" to the ClientSubUnit.", true);
                        }
                        else
                        {
                            //reasonCodeProductTypeDescription is null
                            //instead use description of individual members
                            ProductRepository productRepository = new ProductRepository();
                            Product product = new Product();
                            product = productRepository.GetProduct((int)message.ProductId);

                            ReasonCodeTypeRepository reasonCodeTypeRepository = new ReasonCodeTypeRepository();
                            ReasonCodeType reasonCodeType = new ReasonCodeType();
                            reasonCodeType = reasonCodeTypeRepository.GetItem(reasonCodeProductType.ReasonCodeTypeId);

                            wizardMessages.AddMessage("You have added ReasonCode \"" + message.ReasonCode + "\" to " + product.ProductName + "\\" + reasonCodeType.ReasonCodeTypeDescription + ".", true);

                        }
                    }
					else if (message.MessageText == "DELETED")
					{
						if (reasonCodeProductTypeDescription != null)
						{
							wizardMessages.AddMessage("You have removed ReasonCode ProductType \"" + reasonCodeProductTypeDescription.ReasonCodeProductTypeDescription1 + "\" from the ClientSubUnit.", true);
						}
						else
						{
							//reasonCodeProductTypeDescription is null
							//instead use description of individual members
							ProductRepository productRepository = new ProductRepository();
							Product product = new Product();
							product = productRepository.GetProduct((int)message.ProductId);

							ReasonCodeTypeRepository reasonCodeTypeRepository = new ReasonCodeTypeRepository();
							ReasonCodeType reasonCodeType = new ReasonCodeType();
							reasonCodeType = reasonCodeTypeRepository.GetItem(reasonCodeProductType.ReasonCodeTypeId);

							wizardMessages.AddMessage("You have removed ReasonCode \"" + message.ReasonCode + "\" from " + product.ProductName + "\\" + reasonCodeType.ReasonCodeTypeDescription + ".", true);

						}
					}
					else if (message.MessageText == "UPDATED")
					{
						if (reasonCodeProductTypeDescription != null)
						{
							wizardMessages.AddMessage("You have updated ReasonCode ProductType \"" + reasonCodeProductTypeDescription.ReasonCodeProductTypeDescription1 + "\".", true);
						}
						else
						{
							//reasonCodeProductTypeDescription is null
							//instead use description of individual members
							ProductRepository productRepository = new ProductRepository();
							Product product = new Product();
							product = productRepository.GetProduct((int)message.ProductId);

							ReasonCodeTypeRepository reasonCodeTypeRepository = new ReasonCodeTypeRepository();
							ReasonCodeType reasonCodeType = new ReasonCodeType();
							reasonCodeType = reasonCodeTypeRepository.GetItem(reasonCodeProductType.ReasonCodeTypeId);

							wizardMessages.AddMessage("You have updated ReasonCode ProductType \"" + message.ReasonCode + "\" at " + product.ProductName + "\\" + reasonCodeType.ReasonCodeTypeDescription + "\".", true);

						}
					}
                    else if (message.MessageText == "ALREADY EXISTS")
                    {
                        if (reasonCodeProductTypeDescription != null)
                        {
                            wizardMessages.AddMessage("ReasonCode ProductType \"" + reasonCodeProductTypeDescription.ReasonCodeProductTypeDescription1 + "\" already exists.", true);
                        }
                        else
                        {
                            //reasonCodeProductTypeDescription is null
                            //instead use description of individual members
                            ProductRepository productRepository = new ProductRepository();
                            Product product = new Product();
                            product = productRepository.GetProduct((int)message.ProductId);

                            ReasonCodeTypeRepository reasonCodeTypeRepository = new ReasonCodeTypeRepository();
                            ReasonCodeType reasonCodeType = new ReasonCodeType();
                            reasonCodeType = reasonCodeTypeRepository.GetItem(reasonCodeProductType.ReasonCodeTypeId);

                            wizardMessages.AddMessage("ReasonCode \"" + message.ReasonCode + "\" at " + product.ProductName + "\\" + reasonCodeType.ReasonCodeTypeDescription + " already exists.", true);

                        }
                    }
                    else
                    {
                        wizardMessages.AddMessage(message.MessageText, (bool)message.Success);
                    }

                }
            }
            return wizardMessages;
        }

        //Update ClientSubUnit Servicing Options
        public WizardMessages UpdateClientSubUnitServicingOptions(ClientWizardVM updatedClient, WizardMessages wizardMessages)
        {
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = updatedClient.ClientSubUnit;

            bool changesExist = false;

            // Create the xml document container
            XmlDocument doc = new XmlDocument();// Create the XML Declaration, and append it to XML document
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(dec);
            XmlElement root = doc.CreateElement("ClientSubUnitServicingOptions");
            doc.AppendChild(root);

            ServicingOptionRepository servicingOptionRepository = new ServicingOptionRepository();

            if (updatedClient.ServicingOptionItemsAdded != null)
            {
                if (updatedClient.ServicingOptionItemsAdded.Count > 0)
                {
                    changesExist = true;
                    XmlElement xmlServicingOptionItemsAdded = doc.CreateElement("ServicingOptionItemsAdded"); 
                    foreach (ServicingOptionItem item in updatedClient.ServicingOptionItemsAdded)
                    {
                        ServicingOption servicingOption = new ServicingOption();
                        servicingOption = servicingOptionRepository.GetServicingOption(item.ServicingOptionId);

                        XmlElement xmlServicingOptionItem = doc.CreateElement("ServicingOptionItem");
                        xmlServicingOptionItemsAdded.AppendChild(xmlServicingOptionItem);

                        XmlElement xmlServicingOptionName = doc.CreateElement("ServicingOptionName");
                        xmlServicingOptionName.InnerText = servicingOption.ServicingOptionName;
                        xmlServicingOptionItem.AppendChild(xmlServicingOptionName);

                        XmlElement xmlServicingOptionId = doc.CreateElement("ServicingOptionId");
                        xmlServicingOptionId.InnerText = item.ServicingOptionId.ToString();
                        xmlServicingOptionItem.AppendChild(xmlServicingOptionId);

                        XmlElement xmlGDSCode = doc.CreateElement("GDSCode");
                        if (item.GDSCode != null)
                        {
                            xmlGDSCode.InnerText = item.GDSCode.ToString();
                            xmlServicingOptionItem.AppendChild(xmlGDSCode);
                        }

                        XmlElement xmlServicingOptionItemValue = doc.CreateElement("ServicingOptionItemValue");
                        xmlServicingOptionItemValue.InnerText = item.ServicingOptionItemValue;
                        xmlServicingOptionItem.AppendChild(xmlServicingOptionItemValue);
                        string n = xmlServicingOptionItemValue.InnerText;

                        XmlElement xmlServicingOptionItemInstruction = doc.CreateElement("ServicingOptionItemInstruction");
                        xmlServicingOptionItemInstruction.InnerText = item.ServicingOptionItemInstruction;
                        xmlServicingOptionItem.AppendChild(xmlServicingOptionItemInstruction);

                        XmlElement xmlDisplayInApplicationFlag = doc.CreateElement("DisplayInApplicationFlag");
                        xmlDisplayInApplicationFlag.InnerText = item.DisplayInApplicationFlag == true ? "1" : "0";
                        xmlServicingOptionItem.AppendChild(xmlDisplayInApplicationFlag);

						XmlElement xmlDepartureTimeWindow = doc.CreateElement("DepartureTimeWindowMinutes");
						xmlDepartureTimeWindow.InnerText = item.DepartureTimeWindowMinutes.ToString();
						xmlServicingOptionItem.AppendChild(xmlDepartureTimeWindow);

						XmlElement xmlArrivalTimeWindow = doc.CreateElement("ArrivalTimeWindowMinutes");
						xmlArrivalTimeWindow.InnerText = item.ArrivalTimeWindowMinutes.ToString();
						xmlServicingOptionItem.AppendChild(xmlArrivalTimeWindow);

						XmlElement xmlMaximumStops = doc.CreateElement("MaximumStops");
						xmlMaximumStops.InnerText = item.MaximumStops.ToString();
						xmlServicingOptionItem.AppendChild(xmlMaximumStops);

						XmlElement xmlMaximumConnectionTime = doc.CreateElement("MaximumConnectionTimeMinutes");
						xmlMaximumConnectionTime.InnerText = item.MaximumConnectionTimeMinutes.ToString();
						xmlServicingOptionItem.AppendChild(xmlMaximumConnectionTime);

						XmlElement xmlUseAlternateAirportFlag = doc.CreateElement("UseAlternateAirportFlag");
                        xmlUseAlternateAirportFlag.InnerText = item.UseAlternateAirportFlag == true ? "1" : "0";
                        xmlServicingOptionItem.AppendChild(xmlUseAlternateAirportFlag);

						XmlElement xmlNoPenaltyFlag = doc.CreateElement("NoPenaltyFlag");
                        xmlNoPenaltyFlag.InnerText = item.NoPenaltyFlag == true ? "1" : "0";
                        xmlServicingOptionItem.AppendChild(xmlNoPenaltyFlag);

						XmlElement xmlNoRestrictionsFlag = doc.CreateElement("NoRestrictionsFlag");
                        xmlNoRestrictionsFlag.InnerText = item.NoRestrictionsFlag == true ? "1" : "0";
                        xmlServicingOptionItem.AppendChild(xmlNoRestrictionsFlag);

					
                    }
                    root.AppendChild(xmlServicingOptionItemsAdded);
                }
            }
            if (updatedClient.ServicingOptionItemsAltered != null)
            {

                if (updatedClient.ServicingOptionItemsAltered.Count > 0)
                {
                    changesExist = true;
                    XmlElement xmlServicingOptionItemsAltered = doc.CreateElement("ServicingOptionItemsAltered"); 
                    foreach (ServicingOptionItem item in updatedClient.ServicingOptionItemsAltered)
                    {
                        ServicingOption servicingOption = new ServicingOption();
                        servicingOption = servicingOptionRepository.GetServicingOption(item.ServicingOptionId);

                        XmlElement xmlServicingOptionItem = doc.CreateElement("ServicingOptionItem");
                        xmlServicingOptionItemsAltered.AppendChild(xmlServicingOptionItem);

                        XmlElement xmlServicingOptionName = doc.CreateElement("ServicingOptionName");
                        xmlServicingOptionName.InnerText = servicingOption.ServicingOptionName;
                        xmlServicingOptionItem.AppendChild(xmlServicingOptionName);

						XmlElement xmlServicingOptionId = doc.CreateElement("ServicingOptionId");
						xmlServicingOptionId.InnerText = item.ServicingOptionId.ToString();
						xmlServicingOptionItem.AppendChild(xmlServicingOptionId);

                        XmlElement xmlServicingOptionItemId = doc.CreateElement("ServicingOptionItemId");
                        xmlServicingOptionItemId.InnerText = item.ServicingOptionItemId.ToString();
                        xmlServicingOptionItem.AppendChild(xmlServicingOptionItemId);

                        XmlElement xmlGDSCode = doc.CreateElement("GDSCode");
                        if (item.GDSCode != null)
                        {
                            xmlGDSCode.InnerText = item.GDSCode.ToString();
                            xmlServicingOptionItem.AppendChild(xmlGDSCode);
                        }

                        XmlElement xmlServicingOptionItemValue = doc.CreateElement("ServicingOptionItemValue");
                        xmlServicingOptionItemValue.InnerText = item.ServicingOptionItemValue;
                        xmlServicingOptionItem.AppendChild(xmlServicingOptionItemValue);

                        XmlElement xmlServicingOptionItemInstruction = doc.CreateElement("ServicingOptionItemInstruction");
                        xmlServicingOptionItemInstruction.InnerText = item.ServicingOptionItemInstruction;
                        xmlServicingOptionItem.AppendChild(xmlServicingOptionItemInstruction);

                        XmlElement xmlDisplayInApplicationFlag = doc.CreateElement("DisplayInApplicationFlag");
                        xmlDisplayInApplicationFlag.InnerText = item.DisplayInApplicationFlag == true ? "1" : "0";
                        xmlServicingOptionItem.AppendChild(xmlDisplayInApplicationFlag);

						XmlElement xmlDepartureTimeWindow = doc.CreateElement("DepartureTimeWindowMinutes");
						xmlDepartureTimeWindow.InnerText = item.DepartureTimeWindowMinutes.ToString();
						xmlServicingOptionItem.AppendChild(xmlDepartureTimeWindow);

						XmlElement xmlArrivalTimeWindow = doc.CreateElement("ArrivalTimeWindowMinutes");
						xmlArrivalTimeWindow.InnerText = item.ArrivalTimeWindowMinutes.ToString();
						xmlServicingOptionItem.AppendChild(xmlArrivalTimeWindow);

						XmlElement xmlMaximumStops = doc.CreateElement("MaximumStops");
						xmlMaximumStops.InnerText = item.MaximumStops.ToString();
						xmlServicingOptionItem.AppendChild(xmlMaximumStops);

						XmlElement xmlMaximumConnectionTime = doc.CreateElement("MaximumConnectionTimeMinutes");
						xmlMaximumConnectionTime.InnerText = item.MaximumConnectionTimeMinutes.ToString();
						xmlServicingOptionItem.AppendChild(xmlMaximumConnectionTime);

						XmlElement xmlUseAlternateAirportFlag = doc.CreateElement("UseAlternateAirportFlag");
						xmlUseAlternateAirportFlag.InnerText = item.UseAlternateAirportFlag == true ? "1" : "0";
						xmlServicingOptionItem.AppendChild(xmlUseAlternateAirportFlag);

						XmlElement xmlNoPenaltyFlag = doc.CreateElement("NoPenaltyFlag");
						xmlNoPenaltyFlag.InnerText = item.NoPenaltyFlag == true ? "1" : "0";
						xmlServicingOptionItem.AppendChild(xmlNoPenaltyFlag);

						XmlElement xmlNoRestrictionsFlag = doc.CreateElement("NoRestrictionsFlag");
						xmlNoRestrictionsFlag.InnerText = item.NoRestrictionsFlag == true ? "1" : "0";
						xmlServicingOptionItem.AppendChild(xmlNoRestrictionsFlag);

                        XmlElement xmlVersionNumber = doc.CreateElement("VersionNumber");
                        xmlVersionNumber.InnerText = item.VersionNumber.ToString();
                        xmlServicingOptionItem.AppendChild(xmlVersionNumber);
                    }
                    root.AppendChild(xmlServicingOptionItemsAltered);
                    //xml = xml + "</ServicingOptionItemsAltered>";
                }
            }
            if (updatedClient.ServicingOptionItemsRemoved != null)
            {
                if (updatedClient.ServicingOptionItemsRemoved.Count > 0)
                {
                    changesExist = true;
                    //xml = xml + "<ServicingOptionItemsRemoved>";
                    XmlElement xmlServicingOptionItemsRemoved = doc.CreateElement("ServicingOptionItemsRemoved"); 

                    foreach (ServicingOptionItem item in updatedClient.ServicingOptionItemsRemoved)
                    {
                        ServicingOption servicingOption = new ServicingOption();
                        servicingOption = servicingOptionRepository.GetServicingOption(item.ServicingOptionId);

                        XmlElement xmlServicingOptionItem = doc.CreateElement("ServicingOptionItem");
                        xmlServicingOptionItemsRemoved.AppendChild(xmlServicingOptionItem);

                        XmlElement xmlServicingOptionName = doc.CreateElement("ServicingOptionName");
                        xmlServicingOptionName.InnerText = servicingOption.ServicingOptionName;
                        xmlServicingOptionItem.AppendChild(xmlServicingOptionName);

                        XmlElement xmlServicingOptionItemId = doc.CreateElement("ServicingOptionItemId");
                        xmlServicingOptionItemId.InnerText = item.ServicingOptionItemId.ToString();
                        xmlServicingOptionItem.AppendChild(xmlServicingOptionItemId);

                        XmlElement xmlVersionNumber = doc.CreateElement("VersionNumber");
                        xmlVersionNumber.InnerText = item.VersionNumber.ToString() ;
                        xmlServicingOptionItem.AppendChild(xmlVersionNumber);

                    }
                    root.AppendChild(xmlServicingOptionItemsRemoved);
                }
            } 

            if (changesExist)
            {
                GroupNameBuilderRepository groupNameBuilderRepository = new GroupNameBuilderRepository();
                string data = groupNameBuilderRepository.ClientSubUnitIdentifier(clientSubUnit.ClientSubUnitGuid, "Servicing Option");
                string servicingOptionGroupName = "";
                if (clientSubUnit.ClientSubUnitName.Length > 16)
                {
                    servicingOptionGroupName = clientSubUnit.ClientSubUnitName.Substring(0, 16) + "_" + data + "_ClientSubUnit_ServicingOption";
                }
                else
                {
                    servicingOptionGroupName = clientSubUnit.ClientSubUnitName + "_" + data + "_ClientSubUnit_ServicingOption";
                }
                servicingOptionGroupName = servicingOptionGroupName.Replace(" ", "_");

                string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];


                var output = (from n in db.spDDAWizard_UpdateClientSubUnitServicingOptions_v1(
                    clientSubUnit.ClientSubUnitGuid,
                    servicingOptionGroupName,
                    System.Xml.Linq.XElement.Parse(doc.OuterXml),
                    adminUserGuid)
                              select n).ToList();

                foreach (spDDAWizard_UpdateClientSubUnitServicingOptions_v1Result message in output)
                {
                    wizardMessages.AddMessage(message.MessageText.ToString(), (bool)message.Success);
                }
            }
            return wizardMessages;
        }

        //Update ClientSubUnit POlicygroup Inheritance
        public WizardMessages UpdateClientSubUnitPolicyGroup(ClientWizardVM updatedClient, WizardMessages wizardMessages)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            ClientWizardRepository clientWizardRepository = new ClientWizardRepository();
            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = GetClientSubUnitPolicyGroup(updatedClient.ClientSubUnit.ClientSubUnitGuid);

            if (policyGroup.PolicyGroupId == 0) //no existing PolicyGroup
            {
                policyGroup = updatedClient.PolicyGroup;
                //if (updatedClient.PolicyGroup != null)
                //{
                    policyGroup.HierarchyType = "ClientSubUnit";
                    policyGroup.HierarchyCode = updatedClient.ClientSubUnit.ClientSubUnitGuid;

                    PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();
                    policyGroupRepository.Add(policyGroup);
                    wizardMessages.AddMessage("Policy Group Added", true);
                //}
            }
            else
            {
                if (policyGroup.InheritFromParentFlag != updatedClient.PolicyGroup.InheritFromParentFlag)
                {
                    db.spDDAWizard_UpdatePolicyGroup_v1(
                        updatedClient.PolicyGroup.PolicyGroupId,
                        updatedClient.PolicyGroup.InheritFromParentFlag,
                        adminUserGuid,
                        updatedClient.PolicyGroup.VersionNumber
                    );
                    wizardMessages.AddMessage("Policy Group Inheritance updated", true);
                }
            }

            
            return wizardMessages;
        }

		//Update ClientSubUnit PolicyAirParameterGroupItems
		public WizardMessages UpdateClientSubUnitPolicyAirParameterGroupItems(ClientWizardVM updatedClient, WizardMessages wizardMessages)
		{
			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = updatedClient.ClientSubUnit;

			PolicyRoutingRepository policyRoutingRepository = new PolicyRoutingRepository();
			TravelPortTypeRepository travelPortTypeRepository = new TravelPortTypeRepository();
			TravelPortType travelPortType = new TravelPortType();
			bool changesExist = false;

			// Create the xml document container
			XmlDocument doc = new XmlDocument();// Create the XML Declaration, and append it to XML document
			XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
			doc.AppendChild(dec);
			XmlElement root = doc.CreateElement("ClientSubUnitPolicyAirParameterGroupItems");
			doc.AppendChild(root);

			if (updatedClient.PolicyAirParameterGroupItemsAdded != null)
			{
				if (updatedClient.PolicyAirParameterGroupItemsAdded.Count > 0)
				{
					changesExist = true;
					XmlElement xmlPolicyAirParameterGroupItemsAdded = doc.CreateElement("PolicyAirParameterGroupItemsAdded");

					foreach (PolicyAirParameterGroupItemVM item in updatedClient.PolicyAirParameterGroupItemsAdded)
					{
						//PolicyAirParameterGroupItem
						XmlElement xmlPolicyAirParameterGroupItem = doc.CreateElement("PolicyAirParameterGroupItem");
						xmlPolicyAirParameterGroupItemsAdded.AppendChild(xmlPolicyAirParameterGroupItem);

						XmlElement xmlPolicyGroupId = doc.CreateElement("PolicyGroupId");
						xmlPolicyGroupId.InnerText = updatedClient.PolicyGroup.PolicyGroupId.ToString();
						xmlPolicyAirParameterGroupItem.AppendChild(xmlPolicyGroupId);

						XmlElement xmlPolicyAirParameterTypeId = doc.CreateElement("PolicyAirParameterTypeId");
						xmlPolicyAirParameterTypeId.InnerText = item.PolicyAirParameterGroupItem.PolicyAirParameterTypeId.ToString();
						xmlPolicyAirParameterGroupItem.AppendChild(xmlPolicyAirParameterTypeId);

						XmlElement xmlPolicyAirParameterValue = doc.CreateElement("PolicyAirParameterValue");
						xmlPolicyAirParameterValue.InnerText = item.PolicyAirParameterGroupItem.PolicyAirParameterValue.ToString();
						xmlPolicyAirParameterGroupItem.AppendChild(xmlPolicyAirParameterValue);

						XmlElement xmlEnabledFlag = doc.CreateElement("EnabledFlag");
						xmlEnabledFlag.InnerText = item.PolicyAirParameterGroupItem.EnabledFlag == true ? "1" : "0";
						xmlPolicyAirParameterGroupItem.AppendChild(xmlEnabledFlag);

						XmlElement xmlEnabledDate = doc.CreateElement("EnabledDate");
						if (item.PolicyAirParameterGroupItem.EnabledDate != null)
						{
							DateTime x = new DateTime();
							x = (DateTime)item.PolicyAirParameterGroupItem.EnabledDate;
							xmlEnabledDate.InnerText = x.ToString("yyyyMMdd");
						}
						xmlPolicyAirParameterGroupItem.AppendChild(xmlEnabledDate);


						XmlElement xmlExpiryDate = doc.CreateElement("ExpiryDate");
						if (item.PolicyAirParameterGroupItem.ExpiryDate != null)
						{
							DateTime x = new DateTime();
							x = (DateTime)item.PolicyAirParameterGroupItem.ExpiryDate;
							xmlExpiryDate.InnerText = x.ToString("yyyyMMdd");
						}
						xmlPolicyAirParameterGroupItem.AppendChild(xmlExpiryDate);

						XmlElement xmlTravelDateValidFrom = doc.CreateElement("TravelDateValidFrom");
						if (item.PolicyAirParameterGroupItem.TravelDateValidFrom != null)
						{
							DateTime x = new DateTime();
							x = (DateTime)item.PolicyAirParameterGroupItem.TravelDateValidFrom;
							xmlTravelDateValidFrom.InnerText = x.ToString("yyyyMMdd");
						}
						xmlPolicyAirParameterGroupItem.AppendChild(xmlTravelDateValidFrom);

						XmlElement xmlTravelDateValidTo = doc.CreateElement("TravelDateValidTo");
						if (item.PolicyAirParameterGroupItem.TravelDateValidTo != null)
						{
							DateTime x = new DateTime();
							x = (DateTime)item.PolicyAirParameterGroupItem.TravelDateValidTo;
							xmlTravelDateValidTo.InnerText = x.ToString("yyyyMMdd");
						}
						xmlPolicyAirParameterGroupItem.AppendChild(xmlTravelDateValidTo);

						//PolicyRouting
						PolicyRouting policyRouting = new PolicyRouting();
						policyRouting = item.PolicyRouting;
						policyRoutingRepository.EditPolicyRouting(policyRouting);
						item.PolicyRouting = policyRouting;

						if (item.PolicyRouting.FromCodeType == "TravelPortType")
						{
							travelPortType = travelPortTypeRepository.GetTravelPortTypeByDescription(item.PolicyRouting.FromCode);
							item.PolicyRouting.FromTraverlPortTypeId = travelPortType.TravelPortTypeId;
						}
						if (item.PolicyRouting.ToCodeType == "TravelPortType")
						{
							travelPortType = travelPortTypeRepository.GetTravelPortTypeByDescription(item.PolicyRouting.ToCode);
							item.PolicyRouting.ToTravelPortTypeId = travelPortType.TravelPortTypeId.ToString();
						}

						XmlElement xmlPolicyRouting = doc.CreateElement("PolicyRouting");
						xmlPolicyAirParameterGroupItem.AppendChild(xmlPolicyRouting);

						XmlElement xmlName = doc.CreateElement("Name");
						xmlName.InnerText = item.PolicyRouting.Name;
						xmlPolicyRouting.AppendChild(xmlName);


						XmlElement xmlFromGlobalFlag = doc.CreateElement("FromGlobalFlag");
						xmlFromGlobalFlag.InnerText = item.PolicyRouting.FromGlobalFlag == true ? "1" : "0";
						xmlPolicyRouting.AppendChild(xmlFromGlobalFlag);

						XmlElement xmlFromGlobalRegionCode = doc.CreateElement("FromGlobalRegionCode");
						xmlFromGlobalRegionCode.InnerText = item.PolicyRouting.FromGlobalRegionCode;
						xmlPolicyRouting.AppendChild(xmlFromGlobalRegionCode);

						XmlElement xmlFromGlobalSubRegionCode = doc.CreateElement("FromGlobalSubRegionCode");
						xmlFromGlobalSubRegionCode.InnerText = item.PolicyRouting.FromGlobalSubRegionCode;
						xmlPolicyRouting.AppendChild(xmlFromGlobalSubRegionCode);

						XmlElement xmlFromCountryCode = doc.CreateElement("FromCountryCode");
						xmlFromCountryCode.InnerText = item.PolicyRouting.FromCountryCode;
						xmlPolicyRouting.AppendChild(xmlFromCountryCode);

						XmlElement xmlFromCityCode = doc.CreateElement("FromCityCode");
						xmlFromCityCode.InnerText = item.PolicyRouting.FromCityCode;
						xmlPolicyRouting.AppendChild(xmlFromCityCode);

						XmlElement xmlFromTravelPortCode = doc.CreateElement("FromTravelPortCode");
						xmlFromTravelPortCode.InnerText = item.PolicyRouting.FromTravelPortCode;
						xmlPolicyRouting.AppendChild(xmlFromTravelPortCode);

						XmlElement xmlFromTraverlPortTypeId = doc.CreateElement("FromTraverlPortTypeId");
						if (item.PolicyRouting.FromTraverlPortTypeId != null)
						{
							xmlFromTraverlPortTypeId.InnerText = item.PolicyRouting.FromTraverlPortTypeId.ToString();
						}
						xmlPolicyRouting.AppendChild(xmlFromTraverlPortTypeId);

						XmlElement xmlToGlobalFlag = doc.CreateElement("ToGlobalFlag");
						xmlToGlobalFlag.InnerText = item.PolicyRouting.ToGlobalFlag == true ? "1" : "0";
						xmlPolicyRouting.AppendChild(xmlToGlobalFlag);

						XmlElement xmlToGlobalRegionCode = doc.CreateElement("ToGlobalRegionCode");
						xmlToGlobalRegionCode.InnerText = item.PolicyRouting.ToGlobalRegionCode;
						xmlPolicyRouting.AppendChild(xmlToGlobalRegionCode);

						XmlElement xmlToGlobalSubRegionCode = doc.CreateElement("ToGlobalSubRegionCode");
						xmlToGlobalSubRegionCode.InnerText = item.PolicyRouting.ToGlobalSubRegionCode;
						xmlPolicyRouting.AppendChild(xmlToGlobalSubRegionCode);

						XmlElement xmlToCountryCode = doc.CreateElement("ToCountryCode");
						xmlToCountryCode.InnerText = item.PolicyRouting.ToCountryCode;
						xmlPolicyRouting.AppendChild(xmlToCountryCode);

						XmlElement xmlToCityCode = doc.CreateElement("ToCityCode");
						xmlToCityCode.InnerText = item.PolicyRouting.ToCityCode;
						xmlPolicyRouting.AppendChild(xmlToCityCode);

						XmlElement xmlToTravelPortCode = doc.CreateElement("ToTravelPortCode");
						xmlToTravelPortCode.InnerText = item.PolicyRouting.ToTravelPortCode;
						xmlPolicyRouting.AppendChild(xmlToTravelPortCode);

						XmlElement xmlToTravelPortTypeId = doc.CreateElement("ToTravelPortTypeId");
						if (item.PolicyRouting.ToTravelPortTypeId != null)
						{
							xmlToTravelPortTypeId.InnerText = item.PolicyRouting.ToTravelPortTypeId.ToString();
						}
						xmlPolicyRouting.AppendChild(xmlToTravelPortTypeId);

						XmlElement xmlRoutingViceVersaFlag = doc.CreateElement("RoutingViceVersaFlag");
						xmlRoutingViceVersaFlag.InnerText = item.PolicyRouting.RoutingViceVersaFlag == true ? "1" : "0";
						xmlPolicyRouting.AppendChild(xmlRoutingViceVersaFlag);
					}
					root.AppendChild(xmlPolicyAirParameterGroupItemsAdded);
				}
			}


			if (updatedClient.PolicyAirParameterGroupItemsAltered != null)
			{
				if (updatedClient.PolicyAirParameterGroupItemsAltered.Count > 0)
				{
					changesExist = true;
					XmlElement xmlPolicyAirParameterGroupItemsAltered = doc.CreateElement("PolicyAirParameterGroupItemsAltered");

					foreach (PolicyAirParameterGroupItemVM item in updatedClient.PolicyAirParameterGroupItemsAltered)
					{

						//PolicyAirParameterGroupItem
						XmlElement xmlPolicyAirParameterGroupItem = doc.CreateElement("PolicyAirParameterGroupItem");
						xmlPolicyAirParameterGroupItemsAltered.AppendChild(xmlPolicyAirParameterGroupItem);

						XmlElement xmlPolicyAirParameterGroupItemId = doc.CreateElement("PolicyAirParameterGroupItemId");
						xmlPolicyAirParameterGroupItemId.InnerText = item.PolicyAirParameterGroupItem.PolicyAirParameterGroupItemId.ToString();
						xmlPolicyAirParameterGroupItem.AppendChild(xmlPolicyAirParameterGroupItemId);

						XmlElement xmlPolicyGroupId = doc.CreateElement("PolicyGroupId");
						xmlPolicyGroupId.InnerText = updatedClient.PolicyGroup.PolicyGroupId.ToString();
						xmlPolicyAirParameterGroupItem.AppendChild(xmlPolicyGroupId);

						XmlElement xmlPolicyAirParameterTypeId = doc.CreateElement("PolicyAirParameterTypeId");
						xmlPolicyAirParameterTypeId.InnerText = item.PolicyAirParameterGroupItem.PolicyAirParameterTypeId.ToString();
						xmlPolicyAirParameterGroupItem.AppendChild(xmlPolicyAirParameterTypeId);

						XmlElement xmlPolicyAirParameterValue = doc.CreateElement("PolicyAirParameterValue");
						xmlPolicyAirParameterValue.InnerText = item.PolicyAirParameterGroupItem.PolicyAirParameterValue.ToString();
						xmlPolicyAirParameterGroupItem.AppendChild(xmlPolicyAirParameterValue);

						XmlElement xmlEnabledFlag = doc.CreateElement("EnabledFlag");
						xmlEnabledFlag.InnerText = item.PolicyAirParameterGroupItem.EnabledFlag == true ? "1" : "0";
						xmlPolicyAirParameterGroupItem.AppendChild(xmlEnabledFlag);

						XmlElement xmlEnabledDate = doc.CreateElement("EnabledDate");
						if (item.PolicyAirParameterGroupItem.EnabledDate != null)
						{
							DateTime x = new DateTime();
							x = (DateTime)item.PolicyAirParameterGroupItem.EnabledDate;
							xmlEnabledDate.InnerText = x.ToString("yyyyMMdd");
						}
						xmlPolicyAirParameterGroupItem.AppendChild(xmlEnabledDate);


						XmlElement xmlExpiryDate = doc.CreateElement("ExpiryDate");
						if (item.PolicyAirParameterGroupItem.ExpiryDate != null)
						{
							DateTime x = new DateTime();
							x = (DateTime)item.PolicyAirParameterGroupItem.ExpiryDate;
							xmlExpiryDate.InnerText = x.ToString("yyyyMMdd");
						}
						xmlPolicyAirParameterGroupItem.AppendChild(xmlExpiryDate);

						XmlElement xmlTravelDateValidFrom = doc.CreateElement("TravelDateValidFrom");
						if (item.PolicyAirParameterGroupItem.TravelDateValidFrom != null)
						{
							DateTime x = new DateTime();
							x = (DateTime)item.PolicyAirParameterGroupItem.TravelDateValidFrom;
							xmlTravelDateValidFrom.InnerText = x.ToString("yyyyMMdd");
						}
						xmlPolicyAirParameterGroupItem.AppendChild(xmlTravelDateValidFrom);

						XmlElement xmlTravelDateValidTo = doc.CreateElement("TravelDateValidTo");
						if (item.PolicyAirParameterGroupItem.TravelDateValidTo != null)
						{
							DateTime x = new DateTime();
							x = (DateTime)item.PolicyAirParameterGroupItem.TravelDateValidTo;
							xmlTravelDateValidTo.InnerText = x.ToString("yyyyMMdd");
						}
						xmlPolicyAirParameterGroupItem.AppendChild(xmlTravelDateValidTo);

						XmlElement xmlVersionNumber = doc.CreateElement("VersionNumber");
						xmlVersionNumber.InnerText = item.PolicyAirParameterGroupItem.VersionNumber.ToString();
						xmlPolicyAirParameterGroupItem.AppendChild(xmlVersionNumber);

						//PolicyRouting
						PolicyRouting policyRouting = new PolicyRouting();
						policyRouting = item.PolicyRouting;
						policyRoutingRepository.EditPolicyRouting(policyRouting);
						item.PolicyRouting = policyRouting;

						if (item.PolicyRouting.FromCodeType == "TravelPortType")
						{
							travelPortType = travelPortTypeRepository.GetTravelPortTypeByDescription(item.PolicyRouting.FromCode);
							item.PolicyRouting.FromTraverlPortTypeId = travelPortType.TravelPortTypeId;
						}
						if (item.PolicyRouting.ToCodeType == "TravelPortType")
						{
							travelPortType = travelPortTypeRepository.GetTravelPortTypeByDescription(item.PolicyRouting.ToCode);
							item.PolicyRouting.ToTravelPortTypeId = travelPortType.TravelPortTypeId.ToString();
						}

						XmlElement xmlPolicyRouting = doc.CreateElement("PolicyRouting");
						xmlPolicyAirParameterGroupItem.AppendChild(xmlPolicyRouting);

						XmlElement xmlName = doc.CreateElement("Name");
						xmlName.InnerText = item.PolicyRouting.Name;
						xmlPolicyRouting.AppendChild(xmlName);

						XmlElement xmlFromGlobalFlag = doc.CreateElement("FromGlobalFlag");
						xmlFromGlobalFlag.InnerText = item.PolicyRouting.FromGlobalFlag == true ? "1" : "0";
						xmlPolicyRouting.AppendChild(xmlFromGlobalFlag);

						XmlElement xmlFromGlobalRegionCode = doc.CreateElement("FromGlobalRegionCode");
						xmlFromGlobalRegionCode.InnerText = item.PolicyRouting.FromGlobalRegionCode;
						xmlPolicyRouting.AppendChild(xmlFromGlobalRegionCode);

						XmlElement xmlFromGlobalSubRegionCode = doc.CreateElement("FromGlobalSubRegionCode");
						xmlFromGlobalSubRegionCode.InnerText = item.PolicyRouting.FromGlobalSubRegionCode;
						xmlPolicyRouting.AppendChild(xmlFromGlobalSubRegionCode);

						XmlElement xmlFromCountryCode = doc.CreateElement("FromCountryCode");
						xmlFromCountryCode.InnerText = item.PolicyRouting.FromCountryCode;
						xmlPolicyRouting.AppendChild(xmlFromCountryCode);

						XmlElement xmlFromCityCode = doc.CreateElement("FromCityCode");
						xmlFromCityCode.InnerText = item.PolicyRouting.FromCityCode;
						xmlPolicyRouting.AppendChild(xmlFromCityCode);

						XmlElement xmlFromTravelPortCode = doc.CreateElement("FromTravelPortCode");
						xmlFromTravelPortCode.InnerText = item.PolicyRouting.FromTravelPortCode;
						xmlPolicyRouting.AppendChild(xmlFromTravelPortCode);

						XmlElement xmlFromTraverlPortTypeId = doc.CreateElement("FromTraverlPortTypeId");
						if (item.PolicyRouting.FromTraverlPortTypeId != null)
						{
							xmlFromTraverlPortTypeId.InnerText = item.PolicyRouting.FromTraverlPortTypeId.ToString();
						}
						xmlPolicyRouting.AppendChild(xmlFromTraverlPortTypeId);

						XmlElement xmlToGlobalFlag = doc.CreateElement("ToGlobalFlag");
						xmlToGlobalFlag.InnerText = item.PolicyRouting.ToGlobalFlag == true ? "1" : "0";
						xmlPolicyRouting.AppendChild(xmlToGlobalFlag);

						XmlElement xmlToGlobalRegionCode = doc.CreateElement("ToGlobalRegionCode");
						xmlToGlobalRegionCode.InnerText = item.PolicyRouting.ToGlobalRegionCode;
						xmlPolicyRouting.AppendChild(xmlToGlobalRegionCode);

						XmlElement xmlToGlobalSubRegionCode = doc.CreateElement("ToGlobalSubRegionCode");
						xmlToGlobalSubRegionCode.InnerText = item.PolicyRouting.ToGlobalSubRegionCode;
						xmlPolicyRouting.AppendChild(xmlToGlobalSubRegionCode);

						XmlElement xmlToCountryCode = doc.CreateElement("ToCountryCode");
						xmlToCountryCode.InnerText = item.PolicyRouting.ToCountryCode;
						xmlPolicyRouting.AppendChild(xmlToCountryCode);

						XmlElement xmlToCityCode = doc.CreateElement("ToCityCode");
						xmlToCityCode.InnerText = item.PolicyRouting.ToCityCode;
						xmlPolicyRouting.AppendChild(xmlToCityCode);

						XmlElement xmlToTravelPortCode = doc.CreateElement("ToTravelPortCode");
						xmlToTravelPortCode.InnerText = item.PolicyRouting.ToTravelPortCode;
						xmlPolicyRouting.AppendChild(xmlToTravelPortCode);

						XmlElement xmlToTravelPortTypeId = doc.CreateElement("ToTravelPortTypeId");
						if (item.PolicyRouting.ToTravelPortTypeId != null)
						{
							xmlToTravelPortTypeId.InnerText = item.PolicyRouting.ToTravelPortTypeId.ToString();
						}
						xmlPolicyRouting.AppendChild(xmlToTravelPortTypeId);

						XmlElement xmlRoutingViceVersaFlag = doc.CreateElement("RoutingViceVersaFlag");
						xmlRoutingViceVersaFlag.InnerText = item.PolicyRouting.RoutingViceVersaFlag == true ? "1" : "0";
						xmlPolicyRouting.AppendChild(xmlRoutingViceVersaFlag);
					}
					root.AppendChild(xmlPolicyAirParameterGroupItemsAltered);
				}
			}


			if (updatedClient.PolicyAirParameterGroupItemsRemoved != null)
			{
				if (updatedClient.PolicyAirParameterGroupItemsRemoved.Count > 0)
				{
					changesExist = true;
					XmlElement xmlPolicyAirParameterGroupItemsRemoved = doc.CreateElement("PolicyAirParameterGroupItemsRemoved");

					foreach (PolicyAirParameterGroupItem item in updatedClient.PolicyAirParameterGroupItemsRemoved)
					{
						//PolicyAirParameterGroupItem
						XmlElement xmlPolicyAirParameterGroupItem = doc.CreateElement("PolicyAirParameterGroupItem");
						xmlPolicyAirParameterGroupItemsRemoved.AppendChild(xmlPolicyAirParameterGroupItem);

						XmlElement xmlPolicyAirParameterGroupItemId = doc.CreateElement("PolicyAirParameterGroupItemId");
						xmlPolicyAirParameterGroupItemId.InnerText = item.PolicyAirParameterGroupItemId.ToString();
						xmlPolicyAirParameterGroupItem.AppendChild(xmlPolicyAirParameterGroupItemId);

						XmlElement xmlPolicyAirParameterTypeId = doc.CreateElement("PolicyAirParameterTypeId");
						xmlPolicyAirParameterTypeId.InnerText = item.PolicyAirParameterTypeId.ToString();
						xmlPolicyAirParameterGroupItem.AppendChild(xmlPolicyAirParameterTypeId);

						XmlElement xmlVersionNumber = doc.CreateElement("VersionNumber");
						xmlVersionNumber.InnerText = item.VersionNumber.ToString();
						xmlPolicyAirParameterGroupItem.AppendChild(xmlVersionNumber);

					}
					root.AppendChild(xmlPolicyAirParameterGroupItemsRemoved);
				}
			}

			if (changesExist)
			{
				string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

				var output = (from n in db.spDDAWizard_UpdateClientSubUnitPolicyAirParameterGroupItems_v1(
					clientSubUnit.ClientSubUnitGuid,
					System.Xml.Linq.XElement.Parse(doc.OuterXml),
					adminUserGuid)
							  select n).ToList();

				foreach (spDDAWizard_UpdateClientSubUnitPolicyAirParameterGroupItems_v1Result message in output)
				{
					wizardMessages.AddMessage(message.MessageText.ToString(), (bool)message.Success);
				}

			}
			return wizardMessages;
		}
		
		//Update ClientSubUnit PolicyAirCabinGroupItems
        public WizardMessages UpdateClientSubUnitPolicyAirCabinGroupItems(ClientWizardVM updatedClient, WizardMessages wizardMessages)
        {

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = updatedClient.ClientSubUnit;

            PolicyRoutingRepository policyRoutingRepository = new PolicyRoutingRepository();
            TravelPortTypeRepository travelPortTypeRepository = new TravelPortTypeRepository();
            TravelPortType travelPortType = new TravelPortType();
            bool changesExist = false;

            // Create the xml document container
            XmlDocument doc = new XmlDocument();// Create the XML Declaration, and append it to XML document
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(dec);
            XmlElement root = doc.CreateElement("ClientSubUnitPolicyAirCabinGroupItems");
            doc.AppendChild(root);
            

            if (updatedClient.PolicyAirCabinGroupItemsAdded != null)
            {
                if (updatedClient.PolicyAirCabinGroupItemsAdded.Count > 0)
                {
                    changesExist = true;
                    XmlElement xmlPolicyAirCabinGroupItemsAdded = doc.CreateElement("PolicyAirCabinGroupItemsAdded");

                    foreach (PolicyAirCabinGroupItemViewModel item in updatedClient.PolicyAirCabinGroupItemsAdded)
                    {
                        //PolicyAirCabinGroupItem
                        XmlElement xmlPolicyAirCabinGroupItem = doc.CreateElement("PolicyAirCabinGroupItem");
                        xmlPolicyAirCabinGroupItemsAdded.AppendChild(xmlPolicyAirCabinGroupItem);

                        XmlElement xmlPolicyGroupId = doc.CreateElement("PolicyGroupId");
                        xmlPolicyGroupId.InnerText = updatedClient.PolicyGroup.PolicyGroupId.ToString();
                        xmlPolicyAirCabinGroupItem.AppendChild(xmlPolicyGroupId);

                        XmlElement xmlAirlineCabinCode = doc.CreateElement("AirlineCabinCode");
                        xmlAirlineCabinCode.InnerText = item.PolicyAirCabinGroupItem.AirlineCabinCode;
                        xmlPolicyAirCabinGroupItem.AppendChild(xmlAirlineCabinCode);

                        XmlElement xmlFlightDurationAllowedMin = doc.CreateElement("FlightDurationAllowedMin");
                        xmlFlightDurationAllowedMin.InnerText = item.PolicyAirCabinGroupItem.FlightDurationAllowedMin.ToString();
                        xmlPolicyAirCabinGroupItem.AppendChild(xmlFlightDurationAllowedMin);

                        XmlElement xmlFlightDurationAllowedMax = doc.CreateElement("FlightDurationAllowedMax");
                        xmlFlightDurationAllowedMax.InnerText = item.PolicyAirCabinGroupItem.FlightDurationAllowedMax.ToString();
                        xmlPolicyAirCabinGroupItem.AppendChild(xmlFlightDurationAllowedMax);

                        XmlElement xmlFlightMileageAllowedMin = doc.CreateElement("FlightMileageAllowedMin");
                        xmlFlightMileageAllowedMin.InnerText = item.PolicyAirCabinGroupItem.FlightMileageAllowedMin.ToString();
                        xmlPolicyAirCabinGroupItem.AppendChild(xmlFlightMileageAllowedMin);

                        XmlElement xmlFlightMileageAllowedMax = doc.CreateElement("FlightMileageAllowedMax");
                        xmlFlightMileageAllowedMax.InnerText = item.PolicyAirCabinGroupItem.FlightMileageAllowedMax.ToString();
                        xmlPolicyAirCabinGroupItem.AppendChild(xmlFlightMileageAllowedMax);

                        XmlElement xmlPolicyProhibitedFlag = doc.CreateElement("PolicyProhibitedFlag");
                        xmlPolicyProhibitedFlag.InnerText = item.PolicyAirCabinGroupItem.PolicyProhibitedFlag == true ? "1" : "0";
                        xmlPolicyAirCabinGroupItem.AppendChild(xmlPolicyProhibitedFlag);

                        XmlElement xmlEnabledFlag = doc.CreateElement("EnabledFlag");
                        xmlEnabledFlag.InnerText = item.PolicyAirCabinGroupItem.EnabledFlag == true ? "1" : "0";
                        xmlPolicyAirCabinGroupItem.AppendChild(xmlEnabledFlag);

                        XmlElement xmlEnabledDate = doc.CreateElement("EnabledDate");
                        if (item.PolicyAirCabinGroupItem.EnabledDate != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.PolicyAirCabinGroupItem.EnabledDate;
                            xmlEnabledDate.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyAirCabinGroupItem.AppendChild(xmlEnabledDate);


                        XmlElement xmlExpiryDate = doc.CreateElement("ExpiryDate");
                        if (item.PolicyAirCabinGroupItem.ExpiryDate != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.PolicyAirCabinGroupItem.ExpiryDate;
                            xmlExpiryDate.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyAirCabinGroupItem.AppendChild(xmlExpiryDate);

                        XmlElement xmlTravelDateValidFrom = doc.CreateElement("TravelDateValidFrom");
                        if (item.PolicyAirCabinGroupItem.TravelDateValidFrom != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.PolicyAirCabinGroupItem.TravelDateValidFrom;
                            xmlTravelDateValidFrom.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyAirCabinGroupItem.AppendChild(xmlTravelDateValidFrom);

                        XmlElement xmlTravelDateValidTo = doc.CreateElement("TravelDateValidTo");
                        if (item.PolicyAirCabinGroupItem.TravelDateValidTo != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.PolicyAirCabinGroupItem.TravelDateValidTo;
                            xmlTravelDateValidTo.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyAirCabinGroupItem.AppendChild(xmlTravelDateValidTo);

                        //PolicyRouting
                        PolicyRouting policyRouting = new PolicyRouting();
                        policyRouting = item.PolicyRouting;
                        policyRoutingRepository.EditPolicyRouting(policyRouting);
                        item.PolicyRouting = policyRouting;

                        if (item.PolicyRouting.FromCodeType == "TravelPortType")
                        {
                            travelPortType = travelPortTypeRepository.GetTravelPortTypeByDescription(item.PolicyRouting.FromCode);
                            item.PolicyRouting.FromTraverlPortTypeId = travelPortType.TravelPortTypeId;
                        }
                        if (item.PolicyRouting.ToCodeType == "TravelPortType")
                        {
                            travelPortType = travelPortTypeRepository.GetTravelPortTypeByDescription(item.PolicyRouting.ToCode);
                            item.PolicyRouting.ToTravelPortTypeId = travelPortType.TravelPortTypeId.ToString();
                        }

                        XmlElement xmlPolicyRouting = doc.CreateElement("PolicyRouting");
                        xmlPolicyAirCabinGroupItem.AppendChild(xmlPolicyRouting);

                        XmlElement xmlName = doc.CreateElement("Name");
                        xmlName.InnerText = policyRouting.Name;
                        xmlPolicyRouting.AppendChild(xmlName);

                        XmlElement xmlFromGlobalFlag = doc.CreateElement("FromGlobalFlag");
                        xmlFromGlobalFlag.InnerText = policyRouting.FromGlobalFlag == true ? "1" : "0";
                        xmlPolicyRouting.AppendChild(xmlFromGlobalFlag);

                        XmlElement xmlFromGlobalRegionCode = doc.CreateElement("FromGlobalRegionCode");
                        xmlFromGlobalRegionCode.InnerText = policyRouting.FromGlobalRegionCode;
                        xmlPolicyRouting.AppendChild(xmlFromGlobalRegionCode);

                        XmlElement xmlFromGlobalSubRegionCode = doc.CreateElement("FromGlobalSubRegionCode");
                        xmlFromGlobalSubRegionCode.InnerText = policyRouting.FromGlobalSubRegionCode;
                        xmlPolicyRouting.AppendChild(xmlFromGlobalSubRegionCode);

                        XmlElement xmlFromCountryCode = doc.CreateElement("FromCountryCode");
                        xmlFromCountryCode.InnerText = item.PolicyRouting.FromCountryCode;
                        xmlPolicyRouting.AppendChild(xmlFromCountryCode);

                        XmlElement xmlFromCityCode = doc.CreateElement("FromCityCode");
                        xmlFromCityCode.InnerText = policyRouting.FromCityCode;
                        xmlPolicyRouting.AppendChild(xmlFromCityCode);

                        XmlElement xmlFromTravelPortCode = doc.CreateElement("FromTravelPortCode");
                        xmlFromTravelPortCode.InnerText = policyRouting.FromTravelPortCode;
                        xmlPolicyRouting.AppendChild(xmlFromTravelPortCode);

                        XmlElement xmlFromTraverlPortTypeId = doc.CreateElement("FromTraverlPortTypeId");
                        if (policyRouting.FromTraverlPortTypeId != null)
                        {
                            xmlFromTraverlPortTypeId.InnerText = policyRouting.FromTraverlPortTypeId.ToString();
                        }
                        xmlPolicyRouting.AppendChild(xmlFromTraverlPortTypeId);

                        XmlElement xmlToGlobalFlag = doc.CreateElement("ToGlobalFlag");
                        xmlToGlobalFlag.InnerText = policyRouting.ToGlobalFlag == true ? "1" : "0";
                        xmlPolicyRouting.AppendChild(xmlToGlobalFlag);

                        XmlElement xmlToGlobalRegionCode = doc.CreateElement("ToGlobalRegionCode");
                        xmlToGlobalRegionCode.InnerText = policyRouting.ToGlobalRegionCode;
                        xmlPolicyRouting.AppendChild(xmlToGlobalRegionCode);

                        XmlElement xmlToGlobalSubRegionCode = doc.CreateElement("ToGlobalSubRegionCode");
                        xmlToGlobalSubRegionCode.InnerText = policyRouting.ToGlobalSubRegionCode;
                        xmlPolicyRouting.AppendChild(xmlToGlobalSubRegionCode);

                        XmlElement xmlToCountryCode = doc.CreateElement("ToCountryCode");
                        xmlToCountryCode.InnerText = policyRouting.ToCountryCode;
                        xmlPolicyRouting.AppendChild(xmlToCountryCode);

                        XmlElement xmlToCityCode = doc.CreateElement("ToCityCode");
                        xmlToCityCode.InnerText = policyRouting.ToCityCode;
                        xmlPolicyRouting.AppendChild(xmlToCityCode);

                        XmlElement xmlToTravelPortCode = doc.CreateElement("ToTravelPortCode");
                        xmlToTravelPortCode.InnerText = policyRouting.ToTravelPortCode;
                        xmlPolicyRouting.AppendChild(xmlToTravelPortCode);

                        XmlElement xmlToTravelPortTypeId = doc.CreateElement("ToTravelPortTypeId");
                        if (policyRouting.ToTravelPortTypeId != null)
                        {
                            xmlToTravelPortTypeId.InnerText = policyRouting.ToTravelPortTypeId.ToString();
                        }
                        xmlPolicyRouting.AppendChild(xmlToTravelPortTypeId);

                        XmlElement xmlRoutingViceVersaFlag = doc.CreateElement("RoutingViceVersaFlag");
                        xmlRoutingViceVersaFlag.InnerText = policyRouting.RoutingViceVersaFlag == true ? "1" : "0";
                        xmlPolicyRouting.AppendChild(xmlRoutingViceVersaFlag);

                       
                    }
                    root.AppendChild(xmlPolicyAirCabinGroupItemsAdded);
                }
            }


            if (updatedClient.PolicyAirCabinGroupItemsAltered != null)
            {
                if (updatedClient.PolicyAirCabinGroupItemsAltered.Count > 0)
                {
                    changesExist = true;
                    XmlElement xmlPolicyAirCabinGroupItemsAltered = doc.CreateElement("PolicyAirCabinGroupItemsAltered");

                    foreach (PolicyAirCabinGroupItemViewModel item in updatedClient.PolicyAirCabinGroupItemsAltered)
                    {
                        //PolicyAirCabinGroupItem
                        XmlElement xmlPolicyAirCabinGroupItem = doc.CreateElement("PolicyAirCabinGroupItem");
                        xmlPolicyAirCabinGroupItemsAltered.AppendChild(xmlPolicyAirCabinGroupItem);

                        XmlElement xmlPolicyAirCabinGroupItemId = doc.CreateElement("PolicyAirCabinGroupItemId");
                        xmlPolicyAirCabinGroupItemId.InnerText = item.PolicyAirCabinGroupItem.PolicyAirCabinGroupItemId.ToString();
                        xmlPolicyAirCabinGroupItem.AppendChild(xmlPolicyAirCabinGroupItemId);

                        XmlElement xmlPolicyGroupId = doc.CreateElement("PolicyGroupId");
                        xmlPolicyGroupId.InnerText = updatedClient.PolicyGroup.PolicyGroupId.ToString();
                        xmlPolicyAirCabinGroupItem.AppendChild(xmlPolicyGroupId);

                        XmlElement xmlAirlineCabinCode = doc.CreateElement("AirlineCabinCode");
                        xmlAirlineCabinCode.InnerText = item.PolicyAirCabinGroupItem.AirlineCabinCode;
                        xmlPolicyAirCabinGroupItem.AppendChild(xmlAirlineCabinCode);

                        XmlElement xmlFlightDurationAllowedMin = doc.CreateElement("FlightDurationAllowedMin");
                        xmlFlightDurationAllowedMin.InnerText = item.PolicyAirCabinGroupItem.FlightDurationAllowedMin.ToString();
                        xmlPolicyAirCabinGroupItem.AppendChild(xmlFlightDurationAllowedMin);

                        XmlElement xmlFlightDurationAllowedMax = doc.CreateElement("FlightDurationAllowedMax");
                        xmlFlightDurationAllowedMax.InnerText = item.PolicyAirCabinGroupItem.FlightDurationAllowedMax.ToString();
                        xmlPolicyAirCabinGroupItem.AppendChild(xmlFlightDurationAllowedMax);

                        XmlElement xmlFlightMileageAllowedMin = doc.CreateElement("FlightMileageAllowedMin");
                        xmlFlightMileageAllowedMin.InnerText = item.PolicyAirCabinGroupItem.FlightMileageAllowedMin.ToString();
                        xmlPolicyAirCabinGroupItem.AppendChild(xmlFlightMileageAllowedMin);

                        XmlElement xmlFlightMileageAllowedMax = doc.CreateElement("FlightMileageAllowedMax");
                        xmlFlightMileageAllowedMax.InnerText = item.PolicyAirCabinGroupItem.FlightMileageAllowedMax.ToString();
                        xmlPolicyAirCabinGroupItem.AppendChild(xmlFlightMileageAllowedMax);

                        XmlElement xmlPolicyProhibitedFlag = doc.CreateElement("PolicyProhibitedFlag");
                        xmlPolicyProhibitedFlag.InnerText = item.PolicyAirCabinGroupItem.PolicyProhibitedFlag == true ? "1" : "0";
                        xmlPolicyAirCabinGroupItem.AppendChild(xmlPolicyProhibitedFlag);

                        XmlElement xmlEnabledFlag = doc.CreateElement("EnabledFlag");
                        xmlEnabledFlag.InnerText = item.PolicyAirCabinGroupItem.EnabledFlag == true ? "1" : "0";
                        xmlPolicyAirCabinGroupItem.AppendChild(xmlEnabledFlag);

                        XmlElement xmlEnabledDate = doc.CreateElement("EnabledDate");
                        if (item.PolicyAirCabinGroupItem.EnabledDate != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.PolicyAirCabinGroupItem.EnabledDate;
                            xmlEnabledDate.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyAirCabinGroupItem.AppendChild(xmlEnabledDate);


                        XmlElement xmlExpiryDate = doc.CreateElement("ExpiryDate");
                        if (item.PolicyAirCabinGroupItem.ExpiryDate != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.PolicyAirCabinGroupItem.ExpiryDate;
                            xmlExpiryDate.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyAirCabinGroupItem.AppendChild(xmlExpiryDate);

                        XmlElement xmlTravelDateValidFrom = doc.CreateElement("TravelDateValidFrom");
                        if (item.PolicyAirCabinGroupItem.TravelDateValidFrom != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.PolicyAirCabinGroupItem.TravelDateValidFrom;
                            xmlTravelDateValidFrom.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyAirCabinGroupItem.AppendChild(xmlTravelDateValidFrom);

                        XmlElement xmlTravelDateValidTo = doc.CreateElement("TravelDateValidTo");
                        if (item.PolicyAirCabinGroupItem.TravelDateValidTo != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.PolicyAirCabinGroupItem.TravelDateValidTo;
                            xmlTravelDateValidTo.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyAirCabinGroupItem.AppendChild(xmlTravelDateValidTo);

                        XmlElement xmlVersionNumber = doc.CreateElement("VersionNumber");
                        xmlVersionNumber.InnerText = item.PolicyAirCabinGroupItem.VersionNumber.ToString();
                        xmlPolicyAirCabinGroupItem.AppendChild(xmlVersionNumber);

                        //PolicyRouting
                        PolicyRouting policyRouting = new PolicyRouting();
                        policyRouting = item.PolicyRouting;
                        policyRoutingRepository.EditPolicyRouting(policyRouting);
                        item.PolicyRouting = policyRouting;

                        if (item.PolicyRouting.FromCodeType == "TravelPortType")
                        {
                            travelPortType = travelPortTypeRepository.GetTravelPortTypeByDescription(item.PolicyRouting.FromCode);
                            item.PolicyRouting.FromTraverlPortTypeId = travelPortType.TravelPortTypeId;
                        }
                        if (item.PolicyRouting.ToCodeType == "TravelPortType")
                        {
                            travelPortType = travelPortTypeRepository.GetTravelPortTypeByDescription(item.PolicyRouting.ToCode);
                            item.PolicyRouting.ToTravelPortTypeId = travelPortType.TravelPortTypeId.ToString();
                        }
                        XmlElement xmlPolicyRouting = doc.CreateElement("PolicyRouting");
                        xmlPolicyAirCabinGroupItem.AppendChild(xmlPolicyRouting);

                        XmlElement xmlName = doc.CreateElement("Name");
                        xmlName.InnerText = policyRouting.Name;
                        xmlPolicyRouting.AppendChild(xmlName);

                        XmlElement xmlFromGlobalFlag = doc.CreateElement("FromGlobalFlag");
                        xmlFromGlobalFlag.InnerText = policyRouting.FromGlobalFlag == true ? "1" : "0";
                        xmlPolicyRouting.AppendChild(xmlFromGlobalFlag);

                        XmlElement xmlFromGlobalRegionCode = doc.CreateElement("FromGlobalRegionCode");
                        xmlFromGlobalRegionCode.InnerText = policyRouting.FromGlobalRegionCode;
                        xmlPolicyRouting.AppendChild(xmlFromGlobalRegionCode);

                        XmlElement xmlFromGlobalSubRegionCode = doc.CreateElement("FromGlobalSubRegionCode");
                        xmlFromGlobalSubRegionCode.InnerText = policyRouting.FromGlobalSubRegionCode;
                        xmlPolicyRouting.AppendChild(xmlFromGlobalSubRegionCode);

                        XmlElement xmlFromCountryCode = doc.CreateElement("FromCountryCode");
                        xmlFromCountryCode.InnerText = item.PolicyRouting.FromCountryCode;
                        xmlPolicyRouting.AppendChild(xmlFromCountryCode);

                        XmlElement xmlFromCityCode = doc.CreateElement("FromCityCode");
                        xmlFromCityCode.InnerText = policyRouting.FromCityCode;
                        xmlPolicyRouting.AppendChild(xmlFromCityCode);

                        XmlElement xmlFromTravelPortCode = doc.CreateElement("FromTravelPortCode");
                        xmlFromTravelPortCode.InnerText = policyRouting.FromTravelPortCode;
                        xmlPolicyRouting.AppendChild(xmlFromTravelPortCode);

                        XmlElement xmlFromTraverlPortTypeId = doc.CreateElement("FromTraverlPortTypeId");
                        if (policyRouting.FromTraverlPortTypeId != null)
                        {
                            xmlFromTraverlPortTypeId.InnerText = policyRouting.FromTraverlPortTypeId.ToString();
                        }
                        xmlPolicyRouting.AppendChild(xmlFromTraverlPortTypeId);

                        XmlElement xmlToGlobalFlag = doc.CreateElement("ToGlobalFlag");
                        xmlToGlobalFlag.InnerText = policyRouting.ToGlobalFlag == true ? "1" : "0";
                        xmlPolicyRouting.AppendChild(xmlToGlobalFlag);

                        XmlElement xmlToGlobalRegionCode = doc.CreateElement("ToGlobalRegionCode");
                        xmlToGlobalRegionCode.InnerText = policyRouting.ToGlobalRegionCode;
                        xmlPolicyRouting.AppendChild(xmlToGlobalRegionCode);

                        XmlElement xmlToGlobalSubRegionCode = doc.CreateElement("ToGlobalSubRegionCode");
                        xmlToGlobalSubRegionCode.InnerText = policyRouting.ToGlobalSubRegionCode;
                        xmlPolicyRouting.AppendChild(xmlToGlobalSubRegionCode);

                        XmlElement xmlToCountryCode = doc.CreateElement("ToCountryCode");
                        xmlToCountryCode.InnerText = policyRouting.ToCountryCode;
                        xmlPolicyRouting.AppendChild(xmlToCountryCode);

                        XmlElement xmlToCityCode = doc.CreateElement("ToCityCode");
                        xmlToCityCode.InnerText = policyRouting.ToCityCode;
                        xmlPolicyRouting.AppendChild(xmlToCityCode);

                        XmlElement xmlToTravelPortCode = doc.CreateElement("ToTravelPortCode");
                        xmlToTravelPortCode.InnerText = policyRouting.ToTravelPortCode;
                        xmlPolicyRouting.AppendChild(xmlToTravelPortCode);

                        XmlElement xmlToTravelPortTypeId = doc.CreateElement("ToTravelPortTypeId");
                        if (policyRouting.ToTravelPortTypeId != null)
                        {
                            xmlToTravelPortTypeId.InnerText = policyRouting.ToTravelPortTypeId.ToString();
                        }
                        xmlPolicyRouting.AppendChild(xmlToTravelPortTypeId);

                        XmlElement xmlRoutingViceVersaFlag = doc.CreateElement("RoutingViceVersaFlag");
                        xmlRoutingViceVersaFlag.InnerText = policyRouting.RoutingViceVersaFlag == true ? "1" : "0";
                        xmlPolicyRouting.AppendChild(xmlRoutingViceVersaFlag);
                    }
                    root.AppendChild(xmlPolicyAirCabinGroupItemsAltered);
                }
            }


            if (updatedClient.PolicyAirCabinGroupItemsRemoved != null)
            {
                if (updatedClient.PolicyAirCabinGroupItemsRemoved.Count > 0)
                {
                    changesExist = true;
                    XmlElement xmlPolicyAirCabinGroupItemsRemoved = doc.CreateElement("PolicyAirCabinGroupItemsRemoved");

                    foreach (PolicyAirCabinGroupItem item in updatedClient.PolicyAirCabinGroupItemsRemoved)
                    {
                        //PolicyAirCabinGroupItem
                        XmlElement xmlPolicyAirCabinGroupItem = doc.CreateElement("PolicyAirCabinGroupItem");
                        xmlPolicyAirCabinGroupItemsRemoved.AppendChild(xmlPolicyAirCabinGroupItem);

                        XmlElement xmlPolicyAirCabinGroupItemId = doc.CreateElement("PolicyAirCabinGroupItemId");
                        xmlPolicyAirCabinGroupItemId.InnerText = item.PolicyAirCabinGroupItemId.ToString();
                        xmlPolicyAirCabinGroupItem.AppendChild(xmlPolicyAirCabinGroupItemId);

                        XmlElement xmlVersionNumber = doc.CreateElement("VersionNumber");
                        xmlVersionNumber.InnerText = item.VersionNumber.ToString();
                        xmlPolicyAirCabinGroupItem.AppendChild(xmlVersionNumber);
                 
                    }
                    root.AppendChild(xmlPolicyAirCabinGroupItemsRemoved);
                }
            }

            if (changesExist)
            {
                string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

                var output = (from n in db.spDDAWizard_UpdateClientSubUnitPolicyAirCabinGroupItems_v1(
                    clientSubUnit.ClientSubUnitGuid,
                    System.Xml.Linq.XElement.Parse(doc.OuterXml),
                    adminUserGuid)
                              select n).ToList();

                foreach (spDDAWizard_UpdateClientSubUnitPolicyAirCabinGroupItems_v1Result message in output)
                {
                    wizardMessages.AddMessage(message.MessageText.ToString(), (bool)message.Success);
                }
                
            }
            return wizardMessages;

           
        }

        //Update ClientSubUnit PolicyAirMissedSavingsThresholdGroupItem
        public WizardMessages UpdateClientSubUnitPolicyAirMissedSavingsThresholdGroupItems(ClientWizardVM updatedClient, WizardMessages wizardMessages)
        {
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = updatedClient.ClientSubUnit;
            bool changesExist = false;

            // Create the xml document container
            XmlDocument doc = new XmlDocument();// Create the XML Declaration, and append it to XML document
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(dec);
            XmlElement root = doc.CreateElement("ClientSubUnitPolicyAirMSTGroupItems");
            doc.AppendChild(root);

            if (updatedClient.PolicyAirMissedSavingsThresholdGroupItemsAdded != null)
            {
                if (updatedClient.PolicyAirMissedSavingsThresholdGroupItemsAdded.Count > 0)
                {
                    changesExist = true;
                    XmlElement xmlPolicyAirMissedSavingsThresholdGroupItemsAdded = doc.CreateElement("PolicyAirMSTGroupItemsAdded");

                    foreach (PolicyAirMissedSavingsThresholdGroupItem item in updatedClient.PolicyAirMissedSavingsThresholdGroupItemsAdded)
                    {
                        //PolicyHotelCapRateGroupItem
                        XmlElement xmlPolicyAirMissedSavingsThresholdGroupItem = doc.CreateElement("PolicyAirMissedSavingsThresholdGroupItem");
                        xmlPolicyAirMissedSavingsThresholdGroupItemsAdded.AppendChild(xmlPolicyAirMissedSavingsThresholdGroupItem);

                        XmlElement xmlPolicyGroupId = doc.CreateElement("PolicyGroupId");
                        xmlPolicyGroupId.InnerText = updatedClient.PolicyGroup.PolicyGroupId.ToString();
                        xmlPolicyAirMissedSavingsThresholdGroupItem.AppendChild(xmlPolicyGroupId);

                        XmlElement xmlMissedThresholdAmount = doc.CreateElement("MissedThresholdAmount");
                        xmlMissedThresholdAmount.InnerText = item.MissedThresholdAmount.ToString();
                        xmlPolicyAirMissedSavingsThresholdGroupItem.AppendChild(xmlMissedThresholdAmount);

                        XmlElement xmlCurrencyCode = doc.CreateElement("CurrencyCode");
                        xmlCurrencyCode.InnerText = item.CurrencyCode.ToString();
                        xmlPolicyAirMissedSavingsThresholdGroupItem.AppendChild(xmlCurrencyCode);

                        XmlElement xmlRoutingCode = doc.CreateElement("RoutingCode");
                        xmlRoutingCode.InnerText = item.RoutingCode.ToString();
                        xmlPolicyAirMissedSavingsThresholdGroupItem.AppendChild(xmlRoutingCode);

                        XmlElement xmlEnabledFlag = doc.CreateElement("EnabledFlag");
                        xmlEnabledFlag.InnerText = item.EnabledFlag == true ? "1" : "0";
                        xmlPolicyAirMissedSavingsThresholdGroupItem.AppendChild(xmlEnabledFlag);

                        XmlElement xmlEnabledDate = doc.CreateElement("EnabledDate");
                        if (item.EnabledDate != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.EnabledDate;
                            xmlEnabledDate.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyAirMissedSavingsThresholdGroupItem.AppendChild(xmlEnabledDate);

                        XmlElement xmlExpiryDate = doc.CreateElement("ExpiryDate");
                        if (item.ExpiryDate != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.ExpiryDate;
                            xmlExpiryDate.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyAirMissedSavingsThresholdGroupItem.AppendChild(xmlExpiryDate);

                        XmlElement xmlTravelDateValidFrom = doc.CreateElement("TravelDateValidFrom");
                        if (item.TravelDateValidFrom != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.TravelDateValidFrom;
                            xmlTravelDateValidFrom.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyAirMissedSavingsThresholdGroupItem.AppendChild(xmlTravelDateValidFrom);

                        XmlElement xmlTravelDateValidTo = doc.CreateElement("TravelDateValidTo");
                        if (item.TravelDateValidTo != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.TravelDateValidTo;
                            xmlTravelDateValidTo.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyAirMissedSavingsThresholdGroupItem.AppendChild(xmlTravelDateValidTo);
                    }
                    root.AppendChild(xmlPolicyAirMissedSavingsThresholdGroupItemsAdded);
                }
            }


            if (updatedClient.PolicyAirMissedSavingsThresholdGroupItemsAltered != null)
            {
                if (updatedClient.PolicyAirMissedSavingsThresholdGroupItemsAltered.Count > 0)
                {
                    changesExist = true;
                    XmlElement xmlPolicyAirMSTGroupItemsAltered = doc.CreateElement("PolicyAirMSTGroupItemsAltered");

                    foreach (PolicyAirMissedSavingsThresholdGroupItem item in updatedClient.PolicyAirMissedSavingsThresholdGroupItemsAltered)
                    {

                        //PolicyHotelCapRateGroupItem
                        XmlElement xmlPolicyAirMissedSavingsThresholdGroupItem = doc.CreateElement("PolicyAirMissedSavingsThresholdGroupItem");
                        xmlPolicyAirMSTGroupItemsAltered.AppendChild(xmlPolicyAirMissedSavingsThresholdGroupItem);

                        XmlElement xmlPolicyAirMissedSavingsThresholdGroupItemId = doc.CreateElement("PolicyAirMissedSavingsThresholdGroupItemId");
                        xmlPolicyAirMissedSavingsThresholdGroupItemId.InnerText = item.PolicyAirMissedSavingsThresholdGroupItemId.ToString();
                        xmlPolicyAirMissedSavingsThresholdGroupItem.AppendChild(xmlPolicyAirMissedSavingsThresholdGroupItemId);

                        XmlElement xmlPolicyGroupId = doc.CreateElement("PolicyGroupId");
                        xmlPolicyGroupId.InnerText = updatedClient.PolicyGroup.PolicyGroupId.ToString();
                        xmlPolicyAirMissedSavingsThresholdGroupItem.AppendChild(xmlPolicyGroupId);

                        XmlElement xmlMissedThresholdAmount = doc.CreateElement("MissedThresholdAmount");
                        xmlMissedThresholdAmount.InnerText = item.MissedThresholdAmount.ToString();
                        xmlPolicyAirMissedSavingsThresholdGroupItem.AppendChild(xmlMissedThresholdAmount);

                        XmlElement xmlCurrencyCode = doc.CreateElement("CurrencyCode");
                        xmlCurrencyCode.InnerText = item.CurrencyCode.ToString();
                        xmlPolicyAirMissedSavingsThresholdGroupItem.AppendChild(xmlCurrencyCode);

                        XmlElement xmlRoutingCode = doc.CreateElement("RoutingCode");
                        xmlRoutingCode.InnerText = item.RoutingCode.ToString();
                        xmlPolicyAirMissedSavingsThresholdGroupItem.AppendChild(xmlRoutingCode);

                        XmlElement xmlEnabledFlag = doc.CreateElement("EnabledFlag");
                        xmlEnabledFlag.InnerText = item.EnabledFlag == true ? "1" : "0";
                        xmlPolicyAirMissedSavingsThresholdGroupItem.AppendChild(xmlEnabledFlag);

                        XmlElement xmlEnabledDate = doc.CreateElement("EnabledDate");
                        if (item.EnabledDate != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.EnabledDate;
                            xmlEnabledDate.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyAirMissedSavingsThresholdGroupItem.AppendChild(xmlEnabledDate);

                        XmlElement xmlExpiryDate = doc.CreateElement("ExpiryDate");
                        if (item.ExpiryDate != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.ExpiryDate;
                            xmlExpiryDate.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyAirMissedSavingsThresholdGroupItem.AppendChild(xmlExpiryDate);

                        XmlElement xmlTravelDateValidFrom = doc.CreateElement("TravelDateValidFrom");
                        if (item.TravelDateValidFrom != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.TravelDateValidFrom;
                            xmlTravelDateValidFrom.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyAirMissedSavingsThresholdGroupItem.AppendChild(xmlTravelDateValidFrom);

                        XmlElement xmlTravelDateValidTo = doc.CreateElement("TravelDateValidTo");
                        if (item.TravelDateValidTo != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.TravelDateValidTo;
                            xmlTravelDateValidTo.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyAirMissedSavingsThresholdGroupItem.AppendChild(xmlTravelDateValidTo);

                        XmlElement xmlVersionNumber = doc.CreateElement("VersionNumber");
                        xmlVersionNumber.InnerText = item.VersionNumber.ToString();
                        xmlPolicyAirMissedSavingsThresholdGroupItem.AppendChild(xmlVersionNumber);
                    }
                    root.AppendChild(xmlPolicyAirMSTGroupItemsAltered);
                }
            }


            if (updatedClient.PolicyAirMissedSavingsThresholdGroupItemsRemoved != null)
            {
                if (updatedClient.PolicyAirMissedSavingsThresholdGroupItemsRemoved.Count > 0)
                {
                    changesExist = true;
                    XmlElement xmlPolicyAirMSTGroupItemsRemoved = doc.CreateElement("PolicyAirMSTGroupItemsRemoved");

                    foreach (PolicyAirMissedSavingsThresholdGroupItem item in updatedClient.PolicyAirMissedSavingsThresholdGroupItemsRemoved)
                    {
                        //PolicyAirMissedSavingsThresholdGroupItem
                        XmlElement xmlPolicyAirMissedSavingsThresholdGroupItem = doc.CreateElement("PolicyAirMissedSavingsThresholdGroupItem");
                        xmlPolicyAirMSTGroupItemsRemoved.AppendChild(xmlPolicyAirMissedSavingsThresholdGroupItem);

                        XmlElement xmlPolicyAirMissedSavingsThresholdGroupItemId = doc.CreateElement("PolicyAirMissedSavingsThresholdGroupItemId");
                        xmlPolicyAirMissedSavingsThresholdGroupItemId.InnerText = item.PolicyAirMissedSavingsThresholdGroupItemId.ToString();
                        xmlPolicyAirMissedSavingsThresholdGroupItem.AppendChild(xmlPolicyAirMissedSavingsThresholdGroupItemId);

                        XmlElement xmlVersionNumber = doc.CreateElement("VersionNumber");
                        xmlVersionNumber.InnerText = item.VersionNumber.ToString();
                        xmlPolicyAirMissedSavingsThresholdGroupItem.AppendChild(xmlVersionNumber);

                    }
                    root.AppendChild(xmlPolicyAirMSTGroupItemsRemoved);
                }
            }

            if (changesExist)
            {
                string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

                var output = (from n in db.spDDAWizard_UpdateClientSubUnitPolicyAirMSTGroupItems_v1(
                    clientSubUnit.ClientSubUnitGuid,
                    System.Xml.Linq.XElement.Parse(doc.OuterXml),
                    adminUserGuid)
                              select n).ToList();

                foreach (spDDAWizard_UpdateClientSubUnitPolicyAirMSTGroupItems_v1Result message in output)
                {
                    wizardMessages.AddMessage(message.MessageText.ToString(), (bool)message.Success);
                }

            }
            return wizardMessages;
        }

        //Update ClientSubUnit PolicyAirVendorGroupItems
        public WizardMessages UpdateClientSubUnitPolicyAirVendorGroupItems(ClientWizardVM updatedClient, WizardMessages wizardMessages)
        {
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = updatedClient.ClientSubUnit;

            PolicyRoutingRepository policyRoutingRepository = new PolicyRoutingRepository();
            TravelPortTypeRepository travelPortTypeRepository = new TravelPortTypeRepository();
            TravelPortType travelPortType = new TravelPortType();
            bool changesExist = false;

            // Create the xml document container
            XmlDocument doc = new XmlDocument();// Create the XML Declaration, and append it to XML document
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(dec);
            XmlElement root = doc.CreateElement("ClientSubUnitPolicyAirVendorGroupItems");
            doc.AppendChild(root);

            if (updatedClient.PolicyAirVendorGroupItemsAdded != null)
            {
                if (updatedClient.PolicyAirVendorGroupItemsAdded.Count > 0)
                {
                    changesExist = true;
                    XmlElement xmlPolicyAirVendorGroupItemsAdded = doc.CreateElement("PolicyAirVendorGroupItemsAdded");

                    foreach (PolicyAirVendorGroupItemVM item in updatedClient.PolicyAirVendorGroupItemsAdded)
                    {
                        //PolicyAirVendorGroupItem
                        XmlElement xmlPolicyAirVendorGroupItem = doc.CreateElement("PolicyAirVendorGroupItem");
                        xmlPolicyAirVendorGroupItemsAdded.AppendChild(xmlPolicyAirVendorGroupItem);

                        XmlElement xmlPolicyGroupId = doc.CreateElement("PolicyGroupId");
                        xmlPolicyGroupId.InnerText = updatedClient.PolicyGroup.PolicyGroupId.ToString();
                        xmlPolicyAirVendorGroupItem.AppendChild(xmlPolicyGroupId);

                        XmlElement xmlPolicyAirStatusId = doc.CreateElement("PolicyAirStatusId");
                        xmlPolicyAirStatusId.InnerText = item.PolicyAirVendorGroupItem.PolicyAirStatusId.ToString();
                        xmlPolicyAirVendorGroupItem.AppendChild(xmlPolicyAirStatusId);

                        XmlElement xmlEnabledFlag = doc.CreateElement("EnabledFlag");
                        xmlEnabledFlag.InnerText = item.PolicyAirVendorGroupItem.EnabledFlag == true ? "1" : "0";
                        xmlPolicyAirVendorGroupItem.AppendChild(xmlEnabledFlag);

                        XmlElement xmlEnabledDate = doc.CreateElement("EnabledDate");
                        if (item.PolicyAirVendorGroupItem.EnabledDate != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.PolicyAirVendorGroupItem.EnabledDate;
                            xmlEnabledDate.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyAirVendorGroupItem.AppendChild(xmlEnabledDate);


                        XmlElement xmlExpiryDate = doc.CreateElement("ExpiryDate");
                        if (item.PolicyAirVendorGroupItem.ExpiryDate != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.PolicyAirVendorGroupItem.ExpiryDate;
                            xmlExpiryDate.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyAirVendorGroupItem.AppendChild(xmlExpiryDate);

                        XmlElement xmlTravelDateValidFrom = doc.CreateElement("TravelDateValidFrom");
                        if (item.PolicyAirVendorGroupItem.TravelDateValidFrom != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.PolicyAirVendorGroupItem.TravelDateValidFrom;
                            xmlTravelDateValidFrom.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyAirVendorGroupItem.AppendChild(xmlTravelDateValidFrom);

                        XmlElement xmlTravelDateValidTo = doc.CreateElement("TravelDateValidTo");
                        if (item.PolicyAirVendorGroupItem.TravelDateValidTo != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.PolicyAirVendorGroupItem.TravelDateValidTo;
                            xmlTravelDateValidTo.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyAirVendorGroupItem.AppendChild(xmlTravelDateValidTo);

                        XmlElement xmlSupplierCode = doc.CreateElement("SupplierCode");
                        xmlSupplierCode.InnerText = item.PolicyAirVendorGroupItem.SupplierCode;
                        xmlPolicyAirVendorGroupItem.AppendChild(xmlSupplierCode);

                        XmlElement xmlProductId = doc.CreateElement("ProductId");
                        xmlProductId.InnerText = item.PolicyAirVendorGroupItem.ProductId.ToString();
                        xmlPolicyAirVendorGroupItem.AppendChild(xmlProductId);

                        XmlElement xmlAirVendorRanking = doc.CreateElement("AirVendorRanking");
                        if (item.PolicyAirVendorGroupItem.AirVendorRanking != null)
                        {
                            xmlAirVendorRanking.InnerText = item.PolicyAirVendorGroupItem.AirVendorRanking.ToString();
                        }
                        xmlPolicyAirVendorGroupItem.AppendChild(xmlAirVendorRanking);

                        //PolicyRouting
                        PolicyRouting policyRouting = new PolicyRouting();
                        policyRouting = item.PolicyRouting;
                        policyRoutingRepository.EditPolicyRouting(policyRouting);
                        item.PolicyRouting = policyRouting;

                        if (item.PolicyRouting.FromCodeType == "TravelPortType")
                        {
                            travelPortType = travelPortTypeRepository.GetTravelPortTypeByDescription(item.PolicyRouting.FromCode);
                            item.PolicyRouting.FromTraverlPortTypeId = travelPortType.TravelPortTypeId;
                        }
                        if (item.PolicyRouting.ToCodeType == "TravelPortType")
                        {
                            travelPortType = travelPortTypeRepository.GetTravelPortTypeByDescription(item.PolicyRouting.ToCode);
                            item.PolicyRouting.ToTravelPortTypeId = travelPortType.TravelPortTypeId.ToString();
                        }

                        XmlElement xmlPolicyRouting = doc.CreateElement("PolicyRouting");
                        xmlPolicyAirVendorGroupItem.AppendChild(xmlPolicyRouting);

                        XmlElement xmlName = doc.CreateElement("Name");
                        xmlName.InnerText = item.PolicyRouting.Name;
                        xmlPolicyRouting.AppendChild(xmlName);
                        

                        XmlElement xmlFromGlobalFlag = doc.CreateElement("FromGlobalFlag");
                        xmlFromGlobalFlag.InnerText = item.PolicyRouting.FromGlobalFlag == true ? "1" : "0";
                        xmlPolicyRouting.AppendChild(xmlFromGlobalFlag);

                        XmlElement xmlFromGlobalRegionCode = doc.CreateElement("FromGlobalRegionCode");
                        xmlFromGlobalRegionCode.InnerText = item.PolicyRouting.FromGlobalRegionCode;
                        xmlPolicyRouting.AppendChild(xmlFromGlobalRegionCode);

                        XmlElement xmlFromGlobalSubRegionCode = doc.CreateElement("FromGlobalSubRegionCode");
                        xmlFromGlobalSubRegionCode.InnerText = item.PolicyRouting.FromGlobalSubRegionCode;
                        xmlPolicyRouting.AppendChild(xmlFromGlobalSubRegionCode);

                        XmlElement xmlFromCountryCode = doc.CreateElement("FromCountryCode");
                        xmlFromCountryCode.InnerText = item.PolicyRouting.FromCountryCode;
                        xmlPolicyRouting.AppendChild(xmlFromCountryCode);

                        XmlElement xmlFromCityCode = doc.CreateElement("FromCityCode");
                        xmlFromCityCode.InnerText = item.PolicyRouting.FromCityCode;
                        xmlPolicyRouting.AppendChild(xmlFromCityCode);

                        XmlElement xmlFromTravelPortCode = doc.CreateElement("FromTravelPortCode");
                        xmlFromTravelPortCode.InnerText = item.PolicyRouting.FromTravelPortCode;
                        xmlPolicyRouting.AppendChild(xmlFromTravelPortCode);

                        XmlElement xmlFromTraverlPortTypeId = doc.CreateElement("FromTraverlPortTypeId");
                        if (item.PolicyRouting.FromTraverlPortTypeId != null)
                        {
                            xmlFromTraverlPortTypeId.InnerText = item.PolicyRouting.FromTraverlPortTypeId.ToString();
                        }
                        xmlPolicyRouting.AppendChild(xmlFromTraverlPortTypeId);

                        XmlElement xmlToGlobalFlag = doc.CreateElement("ToGlobalFlag");
                        xmlToGlobalFlag.InnerText = item.PolicyRouting.ToGlobalFlag == true ? "1" : "0";
                        xmlPolicyRouting.AppendChild(xmlToGlobalFlag);

                        XmlElement xmlToGlobalRegionCode = doc.CreateElement("ToGlobalRegionCode");
                        xmlToGlobalRegionCode.InnerText = item.PolicyRouting.ToGlobalRegionCode;
                        xmlPolicyRouting.AppendChild(xmlToGlobalRegionCode);

                        XmlElement xmlToGlobalSubRegionCode = doc.CreateElement("ToGlobalSubRegionCode");
                        xmlToGlobalSubRegionCode.InnerText = item.PolicyRouting.ToGlobalSubRegionCode;
                        xmlPolicyRouting.AppendChild(xmlToGlobalSubRegionCode);

                        XmlElement xmlToCountryCode = doc.CreateElement("ToCountryCode");
                        xmlToCountryCode.InnerText = item.PolicyRouting.ToCountryCode;
                        xmlPolicyRouting.AppendChild(xmlToCountryCode);

                        XmlElement xmlToCityCode = doc.CreateElement("ToCityCode");
                        xmlToCityCode.InnerText = item.PolicyRouting.ToCityCode;
                        xmlPolicyRouting.AppendChild(xmlToCityCode);

                        XmlElement xmlToTravelPortCode = doc.CreateElement("ToTravelPortCode");
                        xmlToTravelPortCode.InnerText = item.PolicyRouting.ToTravelPortCode;
                        xmlPolicyRouting.AppendChild(xmlToTravelPortCode);

                        XmlElement xmlToTravelPortTypeId = doc.CreateElement("ToTravelPortTypeId");
                        if (item.PolicyRouting.ToTravelPortTypeId != null)
                        {
                            xmlToTravelPortTypeId.InnerText = item.PolicyRouting.ToTravelPortTypeId.ToString();
                        }
                        xmlPolicyRouting.AppendChild(xmlToTravelPortTypeId);

                        XmlElement xmlRoutingViceVersaFlag = doc.CreateElement("RoutingViceVersaFlag");
                        xmlRoutingViceVersaFlag.InnerText = item.PolicyRouting.RoutingViceVersaFlag == true ? "1" : "0";
                        xmlPolicyRouting.AppendChild(xmlRoutingViceVersaFlag);
                    }
                    root.AppendChild(xmlPolicyAirVendorGroupItemsAdded);
                }
            }


            if (updatedClient.PolicyAirVendorGroupItemsAltered != null)
            {
                if (updatedClient.PolicyAirVendorGroupItemsAltered.Count > 0)
                {
                    changesExist = true;
                    XmlElement xmlPolicyAirVendorGroupItemsAltered = doc.CreateElement("PolicyAirVendorGroupItemsAltered");

                    foreach (PolicyAirVendorGroupItemVM item in updatedClient.PolicyAirVendorGroupItemsAltered)
                    {

                        //PolicyAirVendorGroupItem
                        XmlElement xmlPolicyAirVendorGroupItem = doc.CreateElement("PolicyAirVendorGroupItem");
                        xmlPolicyAirVendorGroupItemsAltered.AppendChild(xmlPolicyAirVendorGroupItem);

                        XmlElement xmlPolicyAirVendorGroupItemId = doc.CreateElement("PolicyAirVendorGroupItemId");
                        xmlPolicyAirVendorGroupItemId.InnerText = item.PolicyAirVendorGroupItem.PolicyAirVendorGroupItemId.ToString();
                        xmlPolicyAirVendorGroupItem.AppendChild(xmlPolicyAirVendorGroupItemId);

                        XmlElement xmlPolicyGroupId = doc.CreateElement("PolicyGroupId");
                        xmlPolicyGroupId.InnerText = updatedClient.PolicyGroup.PolicyGroupId.ToString();
                        xmlPolicyAirVendorGroupItem.AppendChild(xmlPolicyGroupId);

                        XmlElement xmlPolicyAirStatusId = doc.CreateElement("PolicyAirStatusId");
                        xmlPolicyAirStatusId.InnerText = item.PolicyAirVendorGroupItem.PolicyAirStatusId.ToString();
                        xmlPolicyAirVendorGroupItem.AppendChild(xmlPolicyAirStatusId);

                        XmlElement xmlEnabledFlag = doc.CreateElement("EnabledFlag");
                        xmlEnabledFlag.InnerText = item.PolicyAirVendorGroupItem.EnabledFlag == true ? "1" : "0";
                        xmlPolicyAirVendorGroupItem.AppendChild(xmlEnabledFlag);

                        XmlElement xmlEnabledDate = doc.CreateElement("EnabledDate");
                        if (item.PolicyAirVendorGroupItem.EnabledDate != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.PolicyAirVendorGroupItem.EnabledDate;
                            xmlEnabledDate.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyAirVendorGroupItem.AppendChild(xmlEnabledDate);


                        XmlElement xmlExpiryDate = doc.CreateElement("ExpiryDate");
                        if (item.PolicyAirVendorGroupItem.ExpiryDate != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.PolicyAirVendorGroupItem.ExpiryDate;
                            xmlExpiryDate.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyAirVendorGroupItem.AppendChild(xmlExpiryDate);

                        XmlElement xmlTravelDateValidFrom = doc.CreateElement("TravelDateValidFrom");
                        if (item.PolicyAirVendorGroupItem.TravelDateValidFrom != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.PolicyAirVendorGroupItem.TravelDateValidFrom;
                            xmlTravelDateValidFrom.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyAirVendorGroupItem.AppendChild(xmlTravelDateValidFrom);

                        XmlElement xmlTravelDateValidTo = doc.CreateElement("TravelDateValidTo");
                        if (item.PolicyAirVendorGroupItem.TravelDateValidTo != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.PolicyAirVendorGroupItem.TravelDateValidTo;
                            xmlTravelDateValidTo.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyAirVendorGroupItem.AppendChild(xmlTravelDateValidTo);

                        XmlElement xmlSupplierCode = doc.CreateElement("SupplierCode");
                        xmlSupplierCode.InnerText = item.PolicyAirVendorGroupItem.SupplierCode;
                        xmlPolicyAirVendorGroupItem.AppendChild(xmlSupplierCode);

                        XmlElement xmlProductId = doc.CreateElement("ProductId");
                        xmlProductId.InnerText = item.PolicyAirVendorGroupItem.ProductId.ToString();
                        xmlPolicyAirVendorGroupItem.AppendChild(xmlProductId);

                        XmlElement xmlAirVendorRanking = doc.CreateElement("AirVendorRanking");
                        if (item.PolicyAirVendorGroupItem.AirVendorRanking != null)
                        {
                            xmlAirVendorRanking.InnerText = item.PolicyAirVendorGroupItem.AirVendorRanking.ToString();
                        }
                        xmlPolicyAirVendorGroupItem.AppendChild(xmlAirVendorRanking);

                        XmlElement xmlVersionNumber = doc.CreateElement("VersionNumber");
                        xmlVersionNumber.InnerText = item.PolicyAirVendorGroupItem.VersionNumber.ToString();
                        xmlPolicyAirVendorGroupItem.AppendChild(xmlVersionNumber);

                        //PolicyRouting
                        PolicyRouting policyRouting = new PolicyRouting();
                        policyRouting = item.PolicyRouting;
                        policyRoutingRepository.EditPolicyRouting(policyRouting);
                        item.PolicyRouting = policyRouting;

                        if (item.PolicyRouting.FromCodeType == "TravelPortType")
                        {
                            travelPortType = travelPortTypeRepository.GetTravelPortTypeByDescription(item.PolicyRouting.FromCode);
                            item.PolicyRouting.FromTraverlPortTypeId = travelPortType.TravelPortTypeId;
                        }
                        if (item.PolicyRouting.ToCodeType == "TravelPortType")
                        {
                            travelPortType = travelPortTypeRepository.GetTravelPortTypeByDescription(item.PolicyRouting.ToCode);
                            item.PolicyRouting.ToTravelPortTypeId = travelPortType.TravelPortTypeId.ToString();
                        }

                        XmlElement xmlPolicyRouting = doc.CreateElement("PolicyRouting");
                        xmlPolicyAirVendorGroupItem.AppendChild(xmlPolicyRouting);

                        XmlElement xmlName = doc.CreateElement("Name");
                        xmlName.InnerText = item.PolicyRouting.Name;
                        xmlPolicyRouting.AppendChild(xmlName);

                        XmlElement xmlFromGlobalFlag = doc.CreateElement("FromGlobalFlag");
                        xmlFromGlobalFlag.InnerText = item.PolicyRouting.FromGlobalFlag == true ? "1" : "0";
                        xmlPolicyRouting.AppendChild(xmlFromGlobalFlag);

                        XmlElement xmlFromGlobalRegionCode = doc.CreateElement("FromGlobalRegionCode");
                        xmlFromGlobalRegionCode.InnerText = item.PolicyRouting.FromGlobalRegionCode;
                        xmlPolicyRouting.AppendChild(xmlFromGlobalRegionCode);

                        XmlElement xmlFromGlobalSubRegionCode = doc.CreateElement("FromGlobalSubRegionCode");
                        xmlFromGlobalSubRegionCode.InnerText = item.PolicyRouting.FromGlobalSubRegionCode;
                        xmlPolicyRouting.AppendChild(xmlFromGlobalSubRegionCode);

                        XmlElement xmlFromCountryCode = doc.CreateElement("FromCountryCode");
                        xmlFromCountryCode.InnerText = item.PolicyRouting.FromCountryCode;
                        xmlPolicyRouting.AppendChild(xmlFromCountryCode);

                        XmlElement xmlFromCityCode = doc.CreateElement("FromCityCode");
                        xmlFromCityCode.InnerText = item.PolicyRouting.FromCityCode;
                        xmlPolicyRouting.AppendChild(xmlFromCityCode);

                        XmlElement xmlFromTravelPortCode = doc.CreateElement("FromTravelPortCode");
                        xmlFromTravelPortCode.InnerText = item.PolicyRouting.FromTravelPortCode;
                        xmlPolicyRouting.AppendChild(xmlFromTravelPortCode);

                        XmlElement xmlFromTraverlPortTypeId = doc.CreateElement("FromTraverlPortTypeId");
                        if (item.PolicyRouting.FromTraverlPortTypeId != null)
                        {
                            xmlFromTraverlPortTypeId.InnerText = item.PolicyRouting.FromTraverlPortTypeId.ToString();
                        }
                        xmlPolicyRouting.AppendChild(xmlFromTraverlPortTypeId);

                        XmlElement xmlToGlobalFlag = doc.CreateElement("ToGlobalFlag");
                        xmlToGlobalFlag.InnerText = item.PolicyRouting.ToGlobalFlag == true ? "1" : "0";
                        xmlPolicyRouting.AppendChild(xmlToGlobalFlag);

                        XmlElement xmlToGlobalRegionCode = doc.CreateElement("ToGlobalRegionCode");
                        xmlToGlobalRegionCode.InnerText = item.PolicyRouting.ToGlobalRegionCode;
                        xmlPolicyRouting.AppendChild(xmlToGlobalRegionCode);

                        XmlElement xmlToGlobalSubRegionCode = doc.CreateElement("ToGlobalSubRegionCode");
                        xmlToGlobalSubRegionCode.InnerText = item.PolicyRouting.ToGlobalSubRegionCode;
                        xmlPolicyRouting.AppendChild(xmlToGlobalSubRegionCode);

                        XmlElement xmlToCountryCode = doc.CreateElement("ToCountryCode");
                        xmlToCountryCode.InnerText = item.PolicyRouting.ToCountryCode;
                        xmlPolicyRouting.AppendChild(xmlToCountryCode);

                        XmlElement xmlToCityCode = doc.CreateElement("ToCityCode");
                        xmlToCityCode.InnerText = item.PolicyRouting.ToCityCode;
                        xmlPolicyRouting.AppendChild(xmlToCityCode);

                        XmlElement xmlToTravelPortCode = doc.CreateElement("ToTravelPortCode");
                        xmlToTravelPortCode.InnerText = item.PolicyRouting.ToTravelPortCode;
                        xmlPolicyRouting.AppendChild(xmlToTravelPortCode);

                        XmlElement xmlToTravelPortTypeId = doc.CreateElement("ToTravelPortTypeId");
                        if (item.PolicyRouting.ToTravelPortTypeId != null)
                        {
                            xmlToTravelPortTypeId.InnerText = item.PolicyRouting.ToTravelPortTypeId.ToString();
                        }
                        xmlPolicyRouting.AppendChild(xmlToTravelPortTypeId);

                        XmlElement xmlRoutingViceVersaFlag = doc.CreateElement("RoutingViceVersaFlag");
                        xmlRoutingViceVersaFlag.InnerText = item.PolicyRouting.RoutingViceVersaFlag == true ? "1" : "0";
                        xmlPolicyRouting.AppendChild(xmlRoutingViceVersaFlag);
                    }
                    root.AppendChild(xmlPolicyAirVendorGroupItemsAltered);
                }
            }


            if (updatedClient.PolicyAirVendorGroupItemsRemoved != null)
            {
                if (updatedClient.PolicyAirVendorGroupItemsRemoved.Count > 0)
                {
                    changesExist = true;
                    XmlElement xmlPolicyAirVendorGroupItemsRemoved = doc.CreateElement("PolicyAirVendorGroupItemsRemoved");

                    foreach (PolicyAirVendorGroupItem item in updatedClient.PolicyAirVendorGroupItemsRemoved)
                    {
                        //PolicyAirVendorGroupItem
                        XmlElement xmlPolicyAirVendorGroupItem = doc.CreateElement("PolicyAirVendorGroupItem");
                        xmlPolicyAirVendorGroupItemsRemoved.AppendChild(xmlPolicyAirVendorGroupItem);

                        XmlElement xmlPolicyAirVendorGroupItemId = doc.CreateElement("PolicyAirVendorGroupItemId");
                        xmlPolicyAirVendorGroupItemId.InnerText = item.PolicyAirVendorGroupItemId.ToString();
                        xmlPolicyAirVendorGroupItem.AppendChild(xmlPolicyAirVendorGroupItemId);

                        XmlElement xmlVersionNumber = doc.CreateElement("VersionNumber");
                        xmlVersionNumber.InnerText = item.VersionNumber.ToString();
                        xmlPolicyAirVendorGroupItem.AppendChild(xmlVersionNumber);

                    }
                    root.AppendChild(xmlPolicyAirVendorGroupItemsRemoved);
                }
            }

            if (changesExist)
            {
                string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

                var output = (from n in db.spDDAWizard_UpdateClientSubUnitPolicyAirVendorGroupItems_v1(
                    clientSubUnit.ClientSubUnitGuid,
                    System.Xml.Linq.XElement.Parse(doc.OuterXml),
                    adminUserGuid)
                              select n).ToList();

                foreach (spDDAWizard_UpdateClientSubUnitPolicyAirVendorGroupItems_v1Result message in output)
                {
                    wizardMessages.AddMessage(message.MessageText.ToString(), (bool)message.Success);
                }
                
            }
            return wizardMessages;
        }

        //Update ClientSubUnit PolicyCarTypeGroupItems
        public WizardMessages UpdateClientSubUnitPolicyCarTypeGroupItems(ClientWizardVM updatedClient, WizardMessages wizardMessages)
        {
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = updatedClient.ClientSubUnit;
            bool changesExist = false;

            // Create the xml document container
            XmlDocument doc = new XmlDocument();// Create the XML Declaration, and append it to XML document
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(dec);
            XmlElement root = doc.CreateElement("ClientSubUnitPolicyCarTypeGroupItems");
            doc.AppendChild(root);

            if (updatedClient.PolicyCarTypeGroupItemsAdded != null)
            {
                if (updatedClient.PolicyCarTypeGroupItemsAdded.Count > 0)
                {
                    changesExist = true;
                    XmlElement xmlPolicyCarTypeGroupItemsAdded = doc.CreateElement("PolicyCarTypeGroupItemsAdded");

                    foreach (PolicyCarTypeGroupItem item in updatedClient.PolicyCarTypeGroupItemsAdded)
                    {
                        //PolicyCarTypeGroupItem
                        XmlElement xmlPolicyCarTypeGroupItem = doc.CreateElement("PolicyCarTypeGroupItem");
                        xmlPolicyCarTypeGroupItemsAdded.AppendChild(xmlPolicyCarTypeGroupItem);

                        XmlElement xmlPolicyGroupId = doc.CreateElement("PolicyGroupId");
                        xmlPolicyGroupId.InnerText = updatedClient.PolicyGroup.PolicyGroupId.ToString();
                        xmlPolicyCarTypeGroupItem.AppendChild(xmlPolicyGroupId);

                        XmlElement xmlPolicyLocationId = doc.CreateElement("PolicyLocationId");
                        xmlPolicyLocationId.InnerText = item.PolicyLocationId.ToString();
                        xmlPolicyCarTypeGroupItem.AppendChild(xmlPolicyLocationId);

                        XmlElement xmlPolicyCarStatusId = doc.CreateElement("PolicyCarStatusId");
                        xmlPolicyCarStatusId.InnerText = item.PolicyCarStatusId.ToString();
                        xmlPolicyCarTypeGroupItem.AppendChild(xmlPolicyCarStatusId);

                        XmlElement xmlCarTypeCategoryId = doc.CreateElement("CarTypeCategoryId");
                        xmlCarTypeCategoryId.InnerText = item.CarTypeCategoryId.ToString();
                        xmlPolicyCarTypeGroupItem.AppendChild(xmlCarTypeCategoryId);

                        XmlElement xmlEnabledFlag = doc.CreateElement("EnabledFlag");
                        xmlEnabledFlag.InnerText = item.EnabledFlag == true ? "1" : "0";
                        xmlPolicyCarTypeGroupItem.AppendChild(xmlEnabledFlag);

                        XmlElement xmlEnabledDate = doc.CreateElement("EnabledDate");
                        if (item.EnabledDate != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.EnabledDate;
                            xmlEnabledDate.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyCarTypeGroupItem.AppendChild(xmlEnabledDate);


                        XmlElement xmlExpiryDate = doc.CreateElement("ExpiryDate");
                        if (item.ExpiryDate != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.ExpiryDate;
                            xmlExpiryDate.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyCarTypeGroupItem.AppendChild(xmlExpiryDate);

                        XmlElement xmlTravelDateValidFrom = doc.CreateElement("TravelDateValidFrom");
                        if (item.TravelDateValidFrom != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.TravelDateValidFrom;
                            xmlTravelDateValidFrom.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyCarTypeGroupItem.AppendChild(xmlTravelDateValidFrom);

                        XmlElement xmlTravelDateValidTo = doc.CreateElement("TravelDateValidTo");
                        if (item.TravelDateValidTo != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.TravelDateValidTo;
                            xmlTravelDateValidTo.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyCarTypeGroupItem.AppendChild(xmlTravelDateValidTo);                  
                    }
                    root.AppendChild(xmlPolicyCarTypeGroupItemsAdded);
                }
            }


            if (updatedClient.PolicyCarTypeGroupItemsAltered != null)
            {
                if (updatedClient.PolicyCarTypeGroupItemsAltered.Count > 0)
                {
                    changesExist = true;
                    XmlElement xmlPolicyCarTypeGroupItemsAltered = doc.CreateElement("PolicyCarTypeGroupItemsAltered");

                    foreach (PolicyCarTypeGroupItem item in updatedClient.PolicyCarTypeGroupItemsAltered)
                    {

                        //PolicyCarTypeGroupItem
                        XmlElement xmlPolicyCarTypeGroupItem = doc.CreateElement("PolicyCarTypeGroupItem");
                        xmlPolicyCarTypeGroupItemsAltered.AppendChild(xmlPolicyCarTypeGroupItem);

                        XmlElement xmlPolicyCarTypeGroupItemId = doc.CreateElement("PolicyCarTypeGroupItemId");
                        xmlPolicyCarTypeGroupItemId.InnerText = item.PolicyCarTypeGroupItemId.ToString();
                        xmlPolicyCarTypeGroupItem.AppendChild(xmlPolicyCarTypeGroupItemId);

                        XmlElement xmlPolicyGroupId = doc.CreateElement("PolicyGroupId");
                        xmlPolicyGroupId.InnerText = updatedClient.PolicyGroup.PolicyGroupId.ToString();
                        xmlPolicyCarTypeGroupItem.AppendChild(xmlPolicyGroupId);

                        XmlElement xmlPolicyLocationId = doc.CreateElement("PolicyLocationId");
                        xmlPolicyLocationId.InnerText = item.PolicyLocationId.ToString();
                        xmlPolicyCarTypeGroupItem.AppendChild(xmlPolicyLocationId);

                        XmlElement xmlPolicyCarStatusId = doc.CreateElement("PolicyCarStatusId");
                        xmlPolicyCarStatusId.InnerText = item.PolicyCarStatusId.ToString();
                        xmlPolicyCarTypeGroupItem.AppendChild(xmlPolicyCarStatusId);

                        XmlElement xmlCarTypeCategoryId = doc.CreateElement("CarTypeCategoryId");
                        xmlCarTypeCategoryId.InnerText = item.CarTypeCategoryId.ToString();
                        xmlPolicyCarTypeGroupItem.AppendChild(xmlCarTypeCategoryId);

                        XmlElement xmlEnabledFlag = doc.CreateElement("EnabledFlag");
                        xmlEnabledFlag.InnerText = item.EnabledFlag == true ? "1" : "0";
                        xmlPolicyCarTypeGroupItem.AppendChild(xmlEnabledFlag);

                        XmlElement xmlEnabledDate = doc.CreateElement("EnabledDate");
                        if (item.EnabledDate != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.EnabledDate;
                            xmlEnabledDate.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyCarTypeGroupItem.AppendChild(xmlEnabledDate);


                        XmlElement xmlExpiryDate = doc.CreateElement("ExpiryDate");
                        if (item.ExpiryDate != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.ExpiryDate;
                            xmlExpiryDate.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyCarTypeGroupItem.AppendChild(xmlExpiryDate);

                        XmlElement xmlTravelDateValidFrom = doc.CreateElement("TravelDateValidFrom");
                        if (item.TravelDateValidFrom != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.TravelDateValidFrom;
                            xmlTravelDateValidFrom.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyCarTypeGroupItem.AppendChild(xmlTravelDateValidFrom);

                        XmlElement xmlTravelDateValidTo = doc.CreateElement("TravelDateValidTo");
                        if (item.TravelDateValidTo != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.TravelDateValidTo;
                            xmlTravelDateValidTo.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyCarTypeGroupItem.AppendChild(xmlTravelDateValidTo);

                        XmlElement xmlVersionNumber = doc.CreateElement("VersionNumber");
                        xmlVersionNumber.InnerText = item.VersionNumber.ToString();
                        xmlPolicyCarTypeGroupItem.AppendChild(xmlVersionNumber);
                    }
                    root.AppendChild(xmlPolicyCarTypeGroupItemsAltered);
                }
            }


            if (updatedClient.PolicyCarTypeGroupItemsRemoved != null)
            {
                if (updatedClient.PolicyCarTypeGroupItemsRemoved.Count > 0)
                {
                    changesExist = true;
                    XmlElement xmlPolicyCarTypeGroupItemsRemoved = doc.CreateElement("PolicyCarTypeGroupItemsRemoved");

                    foreach (PolicyCarTypeGroupItem item in updatedClient.PolicyCarTypeGroupItemsRemoved)
                    {
                        //PolicyCarTypeGroupItem
                        XmlElement xmlPolicyCarTypeGroupItem = doc.CreateElement("PolicyCarTypeGroupItem");
                        xmlPolicyCarTypeGroupItemsRemoved.AppendChild(xmlPolicyCarTypeGroupItem);

                        XmlElement xmlPolicyCarTypeGroupItemId = doc.CreateElement("PolicyCarTypeGroupItemId");
                        xmlPolicyCarTypeGroupItemId.InnerText = item.PolicyCarTypeGroupItemId.ToString();
                        xmlPolicyCarTypeGroupItem.AppendChild(xmlPolicyCarTypeGroupItemId);

                        XmlElement xmlVersionNumber = doc.CreateElement("VersionNumber");
                        xmlVersionNumber.InnerText = item.VersionNumber.ToString();
                        xmlPolicyCarTypeGroupItem.AppendChild(xmlVersionNumber);

                    }
                    root.AppendChild(xmlPolicyCarTypeGroupItemsRemoved);
                }
            }

            if (changesExist)
            {
                string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

                var output = (from n in db.spDDAWizard_UpdateClientSubUnitPolicyCarTypeGroupItems_v1(
                    clientSubUnit.ClientSubUnitGuid,
                    System.Xml.Linq.XElement.Parse(doc.OuterXml),
                    adminUserGuid)
                              select n).ToList();

                foreach (spDDAWizard_UpdateClientSubUnitPolicyCarTypeGroupItems_v1Result message in output)
                {
                    wizardMessages.AddMessage(message.MessageText.ToString(), (bool)message.Success);
                }
                
            }
            return wizardMessages;
        }

        //Update ClientSubUnit PolicyCarVendorGroupItems
        public WizardMessages UpdateClientSubUnitPolicyCarVendorGroupItems(ClientWizardVM updatedClient, WizardMessages wizardMessages)
        {
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = updatedClient.ClientSubUnit;
            bool changesExist = false;

            // Create the xml document container
            XmlDocument doc = new XmlDocument();// Create the XML Declaration, and append it to XML document
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(dec);
            XmlElement root = doc.CreateElement("ClientSubUnitPolicyCarVendorGroupItems");
            doc.AppendChild(root);

            if (updatedClient.PolicyCarVendorGroupItemsAdded != null)
            {
                if (updatedClient.PolicyCarVendorGroupItemsAdded.Count > 0)
                {
                    changesExist = true;
                    XmlElement xmlPolicyCarVendorGroupItemsAdded = doc.CreateElement("PolicyCarVendorGroupItemsAdded");

                    foreach (PolicyCarVendorGroupItem item in updatedClient.PolicyCarVendorGroupItemsAdded)
                    {
                        //PolicyCarVendorGroupItem
                        XmlElement xmlPolicyCarVendorGroupItem = doc.CreateElement("PolicyCarVendorGroupItem");
                        xmlPolicyCarVendorGroupItemsAdded.AppendChild(xmlPolicyCarVendorGroupItem);

                        XmlElement xmlPolicyGroupId = doc.CreateElement("PolicyGroupId");
                        xmlPolicyGroupId.InnerText = updatedClient.PolicyGroup.PolicyGroupId.ToString();
                        xmlPolicyCarVendorGroupItem.AppendChild(xmlPolicyGroupId);

                        XmlElement xmlPolicyLocationId = doc.CreateElement("PolicyLocationId");
                        xmlPolicyLocationId.InnerText = item.PolicyLocationId.ToString();
                        xmlPolicyCarVendorGroupItem.AppendChild(xmlPolicyLocationId);

                        XmlElement xmlPolicyCarStatusId = doc.CreateElement("PolicyCarStatusId");
                        xmlPolicyCarStatusId.InnerText = item.PolicyCarStatusId.ToString();
                        xmlPolicyCarVendorGroupItem.AppendChild(xmlPolicyCarStatusId);

                        XmlElement xmlEnabledFlag = doc.CreateElement("EnabledFlag");
                        xmlEnabledFlag.InnerText = item.EnabledFlag == true ? "1" : "0";
                        xmlPolicyCarVendorGroupItem.AppendChild(xmlEnabledFlag);

                        XmlElement xmlEnabledDate = doc.CreateElement("EnabledDate");
                        if (item.EnabledDate != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.EnabledDate;
                            xmlEnabledDate.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyCarVendorGroupItem.AppendChild(xmlEnabledDate);

                        XmlElement xmlExpiryDate = doc.CreateElement("ExpiryDate");
                        if (item.ExpiryDate != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.ExpiryDate;
                            xmlExpiryDate.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyCarVendorGroupItem.AppendChild(xmlExpiryDate);

                        XmlElement xmlTravelDateValidFrom = doc.CreateElement("TravelDateValidFrom");
                        if (item.TravelDateValidFrom != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.TravelDateValidFrom;
                            xmlTravelDateValidFrom.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyCarVendorGroupItem.AppendChild(xmlTravelDateValidFrom);

                        XmlElement xmlTravelDateValidTo = doc.CreateElement("TravelDateValidTo");
                        if (item.TravelDateValidTo != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.TravelDateValidTo;
                            xmlTravelDateValidTo.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyCarVendorGroupItem.AppendChild(xmlTravelDateValidTo);

                        XmlElement xmlSupplierCode = doc.CreateElement("SupplierCode");
                        xmlSupplierCode.InnerText = item.SupplierCode;
                        xmlPolicyCarVendorGroupItem.AppendChild(xmlSupplierCode);

                        XmlElement xmlProductId = doc.CreateElement("ProductId");
                        xmlProductId.InnerText = item.ProductId.ToString();
                        xmlPolicyCarVendorGroupItem.AppendChild(xmlProductId);
                    }
                    root.AppendChild(xmlPolicyCarVendorGroupItemsAdded);
                }
            }


            if (updatedClient.PolicyCarVendorGroupItemsAltered != null)
            {
                if (updatedClient.PolicyCarVendorGroupItemsAltered.Count > 0)
                {
                    changesExist = true;
                    XmlElement xmlPolicyCarVendorGroupItemsAltered = doc.CreateElement("PolicyCarVendorGroupItemsAltered");

                    foreach (PolicyCarVendorGroupItem item in updatedClient.PolicyCarVendorGroupItemsAltered)
                    {

                        //PolicyCarVendorGroupItem
                        XmlElement xmlPolicyCarVendorGroupItem = doc.CreateElement("PolicyCarVendorGroupItem");
                        xmlPolicyCarVendorGroupItemsAltered.AppendChild(xmlPolicyCarVendorGroupItem);

                        XmlElement xmlPolicyCarVendorGroupItemId = doc.CreateElement("PolicyCarVendorGroupItemId");
                        xmlPolicyCarVendorGroupItemId.InnerText = item.PolicyCarVendorGroupItemId.ToString();
                        xmlPolicyCarVendorGroupItem.AppendChild(xmlPolicyCarVendorGroupItemId);

                        XmlElement xmlPolicyGroupId = doc.CreateElement("PolicyGroupId");
                        xmlPolicyGroupId.InnerText = updatedClient.PolicyGroup.PolicyGroupId.ToString();
                        xmlPolicyCarVendorGroupItem.AppendChild(xmlPolicyGroupId);

                        XmlElement xmlPolicyLocationId = doc.CreateElement("PolicyLocationId");
                        xmlPolicyLocationId.InnerText = item.PolicyLocationId.ToString();
                        xmlPolicyCarVendorGroupItem.AppendChild(xmlPolicyLocationId);

                        XmlElement xmlPolicyCarStatusId = doc.CreateElement("PolicyCarStatusId");
                        xmlPolicyCarStatusId.InnerText = item.PolicyCarStatusId.ToString();
                        xmlPolicyCarVendorGroupItem.AppendChild(xmlPolicyCarStatusId);

                        XmlElement xmlEnabledFlag = doc.CreateElement("EnabledFlag");
                        xmlEnabledFlag.InnerText = item.EnabledFlag == true ? "1" : "0";
                        xmlPolicyCarVendorGroupItem.AppendChild(xmlEnabledFlag);

                        XmlElement xmlEnabledDate = doc.CreateElement("EnabledDate");
                        if (item.EnabledDate != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.EnabledDate;
                            xmlEnabledDate.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyCarVendorGroupItem.AppendChild(xmlEnabledDate);

                        XmlElement xmlExpiryDate = doc.CreateElement("ExpiryDate");
                        if (item.ExpiryDate != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.ExpiryDate;
                            xmlExpiryDate.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyCarVendorGroupItem.AppendChild(xmlExpiryDate);

                        XmlElement xmlTravelDateValidFrom = doc.CreateElement("TravelDateValidFrom");
                        if (item.TravelDateValidFrom != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.TravelDateValidFrom;
                            xmlTravelDateValidFrom.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyCarVendorGroupItem.AppendChild(xmlTravelDateValidFrom);

                        XmlElement xmlTravelDateValidTo = doc.CreateElement("TravelDateValidTo");
                        if (item.TravelDateValidTo != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.TravelDateValidTo;
                            xmlTravelDateValidTo.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyCarVendorGroupItem.AppendChild(xmlTravelDateValidTo);

                        XmlElement xmlSupplierCode = doc.CreateElement("SupplierCode");
                        xmlSupplierCode.InnerText = item.SupplierCode;
                        xmlPolicyCarVendorGroupItem.AppendChild(xmlSupplierCode);

                        XmlElement xmlProductId = doc.CreateElement("ProductId");
                        xmlProductId.InnerText = item.ProductId.ToString();
                        xmlPolicyCarVendorGroupItem.AppendChild(xmlProductId);

                        XmlElement xmlVersionNumber = doc.CreateElement("VersionNumber");
                        xmlVersionNumber.InnerText = item.VersionNumber.ToString();
                        xmlPolicyCarVendorGroupItem.AppendChild(xmlVersionNumber);
                    }
                    root.AppendChild(xmlPolicyCarVendorGroupItemsAltered);
                }
            }


            if (updatedClient.PolicyCarVendorGroupItemsRemoved != null)
            {
                if (updatedClient.PolicyCarVendorGroupItemsRemoved.Count > 0)
                {
                    changesExist = true;
                    XmlElement xmlPolicyCarVendorGroupItemsRemoved = doc.CreateElement("PolicyCarVendorGroupItemsRemoved");

                    foreach (PolicyCarVendorGroupItem item in updatedClient.PolicyCarVendorGroupItemsRemoved)
                    {
                        //PolicyCarVendorGroupItem
                        XmlElement xmlPolicyCarVendorGroupItem = doc.CreateElement("PolicyCarVendorGroupItem");
                        xmlPolicyCarVendorGroupItemsRemoved.AppendChild(xmlPolicyCarVendorGroupItem);

                        XmlElement xmlVersionNumber = doc.CreateElement("VersionNumber");
                        xmlVersionNumber.InnerText = item.VersionNumber.ToString();
                        xmlPolicyCarVendorGroupItem.AppendChild(xmlVersionNumber);

                        XmlElement xmlPolicyCarVendorGroupItemId = doc.CreateElement("PolicyCarVendorGroupItemId");
                        xmlPolicyCarVendorGroupItemId.InnerText = item.PolicyCarVendorGroupItemId.ToString();
                        xmlPolicyCarVendorGroupItem.AppendChild(xmlPolicyCarVendorGroupItemId);
                    }
                    root.AppendChild(xmlPolicyCarVendorGroupItemsRemoved);
                }
            }

            if (changesExist)
            {
                string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

                var output = (from n in db.spDDAWizard_UpdateClientSubUnitPolicyCarVendorGroupItems_v1(
                    clientSubUnit.ClientSubUnitGuid,
                    System.Xml.Linq.XElement.Parse(doc.OuterXml),
                    adminUserGuid)
                              select n).ToList();

                foreach (spDDAWizard_UpdateClientSubUnitPolicyCarVendorGroupItems_v1Result message in output)
                {
                    wizardMessages.AddMessage(message.MessageText.ToString(), (bool)message.Success);
                }
                
            }
            return wizardMessages;
        }

        //Update ClientSubUnit PolicyCityGroupItems
        public WizardMessages UpdateClientSubUnitPolicyCityGroupItems(ClientWizardVM updatedClient, WizardMessages wizardMessages)
        {
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = updatedClient.ClientSubUnit;
            bool changesExist = false;

            // Create the xml document container
            XmlDocument doc = new XmlDocument();// Create the XML Declaration, and append it to XML document
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(dec);
            XmlElement root = doc.CreateElement("ClientSubUnitPolicyCityGroupItems");
            doc.AppendChild(root);

            if (updatedClient.PolicyCityGroupItemsAdded != null)
            {
                if (updatedClient.PolicyCityGroupItemsAdded.Count > 0)
                {
                    changesExist = true;
                    XmlElement xmlPolicyCityGroupItemsAdded = doc.CreateElement("PolicyCityGroupItemsAdded");

                    foreach (PolicyCityGroupItem item in updatedClient.PolicyCityGroupItemsAdded)
                    {
                        //PolicyCityGroupItem
                        XmlElement xmlPolicyCityGroupItem = doc.CreateElement("PolicyCityGroupItem");
                        xmlPolicyCityGroupItemsAdded.AppendChild(xmlPolicyCityGroupItem);

                        XmlElement xmlPolicyGroupId = doc.CreateElement("PolicyGroupId");
                        xmlPolicyGroupId.InnerText = updatedClient.PolicyGroup.PolicyGroupId.ToString();
                        xmlPolicyCityGroupItem.AppendChild(xmlPolicyGroupId);

                        XmlElement xmlPolicyCityStatusId = doc.CreateElement("PolicyCityStatusId");
                        xmlPolicyCityStatusId.InnerText = item.PolicyCityStatusId.ToString();
                        xmlPolicyCityGroupItem.AppendChild(xmlPolicyCityStatusId);

                        XmlElement xmlCityCode = doc.CreateElement("CityCode");
                        xmlCityCode.InnerText = item.CityCode;
                        xmlPolicyCityGroupItem.AppendChild(xmlCityCode);

                        XmlElement xmlInheritFromParentFlag = doc.CreateElement("InheritFromParentFlag");
                        xmlInheritFromParentFlag.InnerText = item.InheritFromParentFlag == true ? "1" : "0";
                        xmlPolicyCityGroupItem.AppendChild(xmlInheritFromParentFlag);

                        XmlElement xmlEnabledFlag = doc.CreateElement("EnabledFlag");
                        xmlEnabledFlag.InnerText = item.EnabledFlag == true ? "1" : "0";
                        xmlPolicyCityGroupItem.AppendChild(xmlEnabledFlag);

                        XmlElement xmlEnabledDate = doc.CreateElement("EnabledDate");
                        if (item.EnabledDate != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.EnabledDate;
                            xmlEnabledDate.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyCityGroupItem.AppendChild(xmlEnabledDate);

                        XmlElement xmlExpiryDate = doc.CreateElement("ExpiryDate");
                        if (item.ExpiryDate != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.ExpiryDate;
                            xmlExpiryDate.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyCityGroupItem.AppendChild(xmlExpiryDate);

                        XmlElement xmlTravelDateValidFrom = doc.CreateElement("TravelDateValidFrom");
                        if (item.TravelDateValidFrom != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.TravelDateValidFrom;
                            xmlTravelDateValidFrom.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyCityGroupItem.AppendChild(xmlTravelDateValidFrom);

                        XmlElement xmlTravelDateValidTo = doc.CreateElement("TravelDateValidTo");
                        if (item.TravelDateValidTo != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.TravelDateValidTo;
                            xmlTravelDateValidTo.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyCityGroupItem.AppendChild(xmlTravelDateValidTo);

                    }
                    root.AppendChild(xmlPolicyCityGroupItemsAdded);
                }
            }


            if (updatedClient.PolicyCityGroupItemsAltered != null)
            {
                if (updatedClient.PolicyCityGroupItemsAltered.Count > 0)
                {
                    changesExist = true;
                    XmlElement xmlPolicyCityGroupItemsAltered = doc.CreateElement("PolicyCityGroupItemsAltered");

                    foreach (PolicyCityGroupItem item in updatedClient.PolicyCityGroupItemsAltered)
                    {

                        //PolicyCityGroupItem
                        XmlElement xmlPolicyCityGroupItem = doc.CreateElement("PolicyCityGroupItem");
                        xmlPolicyCityGroupItemsAltered.AppendChild(xmlPolicyCityGroupItem);

                        XmlElement xmlPolicyCityGroupItemId = doc.CreateElement("PolicyCityGroupItemId");
                        xmlPolicyCityGroupItemId.InnerText = item.PolicyCityGroupItemId.ToString();
                        xmlPolicyCityGroupItem.AppendChild(xmlPolicyCityGroupItemId);

                        XmlElement xmlPolicyGroupId = doc.CreateElement("PolicyGroupId");
                        xmlPolicyGroupId.InnerText = updatedClient.PolicyGroup.PolicyGroupId.ToString();
                        xmlPolicyCityGroupItem.AppendChild(xmlPolicyGroupId);

                        XmlElement xmlPolicyCityStatusId = doc.CreateElement("PolicyCityStatusId");
                        xmlPolicyCityStatusId.InnerText = item.PolicyCityStatusId.ToString();
                        xmlPolicyCityGroupItem.AppendChild(xmlPolicyCityStatusId);

                        XmlElement xmlCityCode = doc.CreateElement("CityCode");
                        xmlCityCode.InnerText = item.CityCode;
                        xmlPolicyCityGroupItem.AppendChild(xmlCityCode);

                        XmlElement xmlInheritFromParentFlag = doc.CreateElement("InheritFromParentFlag");
                        xmlInheritFromParentFlag.InnerText = item.InheritFromParentFlag == true ? "1" : "0";
                        xmlPolicyCityGroupItem.AppendChild(xmlInheritFromParentFlag);

                        XmlElement xmlEnabledFlag = doc.CreateElement("EnabledFlag");
                        xmlEnabledFlag.InnerText = item.EnabledFlag == true ? "1" : "0";
                        xmlPolicyCityGroupItem.AppendChild(xmlEnabledFlag);

                        XmlElement xmlEnabledDate = doc.CreateElement("EnabledDate");
                        if (item.EnabledDate != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.EnabledDate;
                            xmlEnabledDate.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyCityGroupItem.AppendChild(xmlEnabledDate);

                        XmlElement xmlExpiryDate = doc.CreateElement("ExpiryDate");
                        if (item.ExpiryDate != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.ExpiryDate;
                            xmlExpiryDate.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyCityGroupItem.AppendChild(xmlExpiryDate);

                        XmlElement xmlTravelDateValidFrom = doc.CreateElement("TravelDateValidFrom");
                        if (item.TravelDateValidFrom != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.TravelDateValidFrom;
                            xmlTravelDateValidFrom.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyCityGroupItem.AppendChild(xmlTravelDateValidFrom);

                        XmlElement xmlTravelDateValidTo = doc.CreateElement("TravelDateValidTo");
                        if (item.TravelDateValidTo != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.TravelDateValidTo;
                            xmlTravelDateValidTo.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyCityGroupItem.AppendChild(xmlTravelDateValidTo);

                        XmlElement xmlVersionNumber = doc.CreateElement("VersionNumber");
                        xmlVersionNumber.InnerText = item.VersionNumber.ToString();
                        xmlPolicyCityGroupItem.AppendChild(xmlVersionNumber);
                    }
                    root.AppendChild(xmlPolicyCityGroupItemsAltered);
                }
            }


            if (updatedClient.PolicyCityGroupItemsRemoved != null)
            {
                if (updatedClient.PolicyCityGroupItemsRemoved.Count > 0)
                {
                    changesExist = true;
                    XmlElement xmlPolicyCityGroupItemsRemoved = doc.CreateElement("PolicyCityGroupItemsRemoved");

                    foreach (PolicyCityGroupItem item in updatedClient.PolicyCityGroupItemsRemoved)
                    {
                        //PolicyCityGroupItem
                        XmlElement xmlPolicyCityGroupItem = doc.CreateElement("PolicyCityGroupItem");
                        xmlPolicyCityGroupItemsRemoved.AppendChild(xmlPolicyCityGroupItem);

                        XmlElement xmlPolicyCityGroupItemId = doc.CreateElement("PolicyCityGroupItemId");
                        xmlPolicyCityGroupItemId.InnerText = item.PolicyCityGroupItemId.ToString();
                        xmlPolicyCityGroupItem.AppendChild(xmlPolicyCityGroupItemId);

                        XmlElement xmlVersionNumber = doc.CreateElement("VersionNumber");
                        xmlVersionNumber.InnerText = item.VersionNumber.ToString();
                        xmlPolicyCityGroupItem.AppendChild(xmlVersionNumber);

                    }
                    root.AppendChild(xmlPolicyCityGroupItemsRemoved);
                }
            }

            if (changesExist)
            {
                string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

                var output = (from n in db.spDDAWizard_UpdateClientSubUnitPolicyCityGroupItems_v1(
                    clientSubUnit.ClientSubUnitGuid,
                    System.Xml.Linq.XElement.Parse(doc.OuterXml),
                    adminUserGuid)
                              select n).ToList();

                foreach (spDDAWizard_UpdateClientSubUnitPolicyCityGroupItems_v1Result message in output)
                {
                    wizardMessages.AddMessage(message.MessageText.ToString(), (bool)message.Success);
                }

            }
            return wizardMessages;
        }

        //Update ClientSubUnit PolicyCountryGroupItems
        public WizardMessages UpdateClientSubUnitPolicyCountryGroupItems(ClientWizardVM updatedClient, WizardMessages wizardMessages)
        {
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = updatedClient.ClientSubUnit;
            bool changesExist = false;

            // Create the xml document container
            XmlDocument doc = new XmlDocument();// Create the XML Declaration, and append it to XML document
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(dec);
            XmlElement root = doc.CreateElement("ClientSubUnitPolicyCountryGroupItems");
            doc.AppendChild(root);

            if (updatedClient.PolicyCountryGroupItemsAdded != null)
            {
                if (updatedClient.PolicyCountryGroupItemsAdded.Count > 0)
                {
                    changesExist = true;
                    XmlElement xmlPolicyCountryGroupItemsAdded = doc.CreateElement("PolicyCountryGroupItemsAdded");

                    foreach (PolicyCountryGroupItem item in updatedClient.PolicyCountryGroupItemsAdded)
                    {
                        //PolicyCountryGroupItem
                        XmlElement xmlPolicyCountryGroupItem = doc.CreateElement("PolicyCountryGroupItem");
                        xmlPolicyCountryGroupItemsAdded.AppendChild(xmlPolicyCountryGroupItem);

                        XmlElement xmlPolicyGroupId = doc.CreateElement("PolicyGroupId");
                        xmlPolicyGroupId.InnerText = updatedClient.PolicyGroup.PolicyGroupId.ToString();
                        xmlPolicyCountryGroupItem.AppendChild(xmlPolicyGroupId);

                        XmlElement xmlPolicyCountryStatusId = doc.CreateElement("PolicyCountryStatusId");
                        xmlPolicyCountryStatusId.InnerText = item.PolicyCountryStatusId.ToString();
                        xmlPolicyCountryGroupItem.AppendChild(xmlPolicyCountryStatusId);

                        XmlElement xmlCountryCode = doc.CreateElement("CountryCode");
                        xmlCountryCode.InnerText = item.CountryCode;
                        xmlPolicyCountryGroupItem.AppendChild(xmlCountryCode);

                        XmlElement xmlInheritFromParentFlag = doc.CreateElement("InheritFromParentFlag");
                        xmlInheritFromParentFlag.InnerText = item.InheritFromParentFlag == true ? "1" : "0";
                        xmlPolicyCountryGroupItem.AppendChild(xmlInheritFromParentFlag);

                        XmlElement xmlEnabledFlag = doc.CreateElement("EnabledFlag");
                        xmlEnabledFlag.InnerText = item.EnabledFlag == true ? "1" : "0";
                        xmlPolicyCountryGroupItem.AppendChild(xmlEnabledFlag);

                        XmlElement xmlEnabledDate = doc.CreateElement("EnabledDate");
                        if (item.EnabledDate != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.EnabledDate;
                            xmlEnabledDate.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyCountryGroupItem.AppendChild(xmlEnabledDate);

                        XmlElement xmlExpiryDate = doc.CreateElement("ExpiryDate");
                        if (item.ExpiryDate != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.ExpiryDate;
                            xmlExpiryDate.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyCountryGroupItem.AppendChild(xmlExpiryDate);

                        XmlElement xmlTravelDateValidFrom = doc.CreateElement("TravelDateValidFrom");
                        if (item.TravelDateValidFrom != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.TravelDateValidFrom;
                            xmlTravelDateValidFrom.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyCountryGroupItem.AppendChild(xmlTravelDateValidFrom);

                        XmlElement xmlTravelDateValidTo = doc.CreateElement("TravelDateValidTo");
                        if (item.TravelDateValidTo != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.TravelDateValidTo;
                            xmlTravelDateValidTo.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyCountryGroupItem.AppendChild(xmlTravelDateValidTo);
                    }
                    root.AppendChild(xmlPolicyCountryGroupItemsAdded);
                }
            }


            if (updatedClient.PolicyCountryGroupItemsAltered != null)
            {
                if (updatedClient.PolicyCountryGroupItemsAltered.Count > 0)
                {
                    changesExist = true;
                    XmlElement xmlPolicyCountryGroupItemsAltered = doc.CreateElement("PolicyCountryGroupItemsAltered");

                    foreach (PolicyCountryGroupItem item in updatedClient.PolicyCountryGroupItemsAltered)
                    {

                        //PolicyCountryGroupItem
                        XmlElement xmlPolicyCountryGroupItem = doc.CreateElement("PolicyCountryGroupItem");
                        xmlPolicyCountryGroupItemsAltered.AppendChild(xmlPolicyCountryGroupItem);

                        XmlElement xmlPolicyCountryGroupItemId = doc.CreateElement("PolicyCountryGroupItemId");
                        xmlPolicyCountryGroupItemId.InnerText = item.PolicyCountryGroupItemId.ToString();
                        xmlPolicyCountryGroupItem.AppendChild(xmlPolicyCountryGroupItemId);

                        XmlElement xmlPolicyGroupId = doc.CreateElement("PolicyGroupId");
                        xmlPolicyGroupId.InnerText = updatedClient.PolicyGroup.PolicyGroupId.ToString();
                        xmlPolicyCountryGroupItem.AppendChild(xmlPolicyGroupId);

                        XmlElement xmlPolicyCountryStatusId = doc.CreateElement("PolicyCountryStatusId");
                        xmlPolicyCountryStatusId.InnerText = item.PolicyCountryStatusId.ToString();
                        xmlPolicyCountryGroupItem.AppendChild(xmlPolicyCountryStatusId);

                        XmlElement xmlCountryCode = doc.CreateElement("CountryCode");
                        xmlCountryCode.InnerText = item.CountryCode;
                        xmlPolicyCountryGroupItem.AppendChild(xmlCountryCode);

                        XmlElement xmlInheritFromParentFlag = doc.CreateElement("InheritFromParentFlag");
                        xmlInheritFromParentFlag.InnerText = item.InheritFromParentFlag == true ? "1" : "0";
                        xmlPolicyCountryGroupItem.AppendChild(xmlInheritFromParentFlag);

                        XmlElement xmlEnabledFlag = doc.CreateElement("EnabledFlag");
                        xmlEnabledFlag.InnerText = item.EnabledFlag == true ? "1" : "0";
                        xmlPolicyCountryGroupItem.AppendChild(xmlEnabledFlag);

                        XmlElement xmlEnabledDate = doc.CreateElement("EnabledDate");
                        if (item.EnabledDate != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.EnabledDate;
                            xmlEnabledDate.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyCountryGroupItem.AppendChild(xmlEnabledDate);

                        XmlElement xmlExpiryDate = doc.CreateElement("ExpiryDate");
                        if (item.ExpiryDate != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.ExpiryDate;
                            xmlExpiryDate.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyCountryGroupItem.AppendChild(xmlExpiryDate);

                        XmlElement xmlTravelDateValidFrom = doc.CreateElement("TravelDateValidFrom");
                        if (item.TravelDateValidFrom != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.TravelDateValidFrom;
                            xmlTravelDateValidFrom.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyCountryGroupItem.AppendChild(xmlTravelDateValidFrom);

                        XmlElement xmlTravelDateValidTo = doc.CreateElement("TravelDateValidTo");
                        if (item.TravelDateValidTo != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.TravelDateValidTo;
                            xmlTravelDateValidTo.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyCountryGroupItem.AppendChild(xmlTravelDateValidTo);

                        XmlElement xmlVersionNumber = doc.CreateElement("VersionNumber");
                        xmlVersionNumber.InnerText = item.VersionNumber.ToString();
                        xmlPolicyCountryGroupItem.AppendChild(xmlVersionNumber);
                    }
                    root.AppendChild(xmlPolicyCountryGroupItemsAltered);
                }
            }


            if (updatedClient.PolicyCountryGroupItemsRemoved != null)
            {
                if (updatedClient.PolicyCountryGroupItemsRemoved.Count > 0)
                {
                    changesExist = true;
                    XmlElement xmlPolicyCountryGroupItemsRemoved = doc.CreateElement("PolicyCountryGroupItemsRemoved");

                    foreach (PolicyCountryGroupItem item in updatedClient.PolicyCountryGroupItemsRemoved)
                    {
                        //PolicyCountryGroupItem
                        XmlElement xmlPolicyCountryGroupItem = doc.CreateElement("PolicyCountryGroupItem");
                        xmlPolicyCountryGroupItemsRemoved.AppendChild(xmlPolicyCountryGroupItem);

                        XmlElement xmlPolicyCountryGroupItemId = doc.CreateElement("PolicyCountryGroupItemId");
                        xmlPolicyCountryGroupItemId.InnerText = item.PolicyCountryGroupItemId.ToString();
                        xmlPolicyCountryGroupItem.AppendChild(xmlPolicyCountryGroupItemId);

                        XmlElement xmlVersionNumber = doc.CreateElement("VersionNumber");
                        xmlVersionNumber.InnerText = item.VersionNumber.ToString();
                        xmlPolicyCountryGroupItem.AppendChild(xmlVersionNumber);

                    }
                    root.AppendChild(xmlPolicyCountryGroupItemsRemoved);
                }
            }

            if (changesExist)
            {
                string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

                var output = (from n in db.spDDAWizard_UpdateClientSubUnitPolicyCountryGroupItems_v1(
                    clientSubUnit.ClientSubUnitGuid,
                    System.Xml.Linq.XElement.Parse(doc.OuterXml),
                    adminUserGuid)
                              select n).ToList();

                foreach (spDDAWizard_UpdateClientSubUnitPolicyCountryGroupItems_v1Result message in output)
                {
                    wizardMessages.AddMessage(message.MessageText.ToString(), (bool)message.Success);
                }
                
            }
            return wizardMessages;
        }

        //Update ClientSubUnit PolicyHotelCapRateGroupItems
        public WizardMessages UpdateClientSubUnitPolicyHotelCapRateGroupItems(ClientWizardVM updatedClient, WizardMessages wizardMessages)
        {
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = updatedClient.ClientSubUnit;
            bool changesExist = false;

            // Create the xml document container
            XmlDocument doc = new XmlDocument();// Create the XML Declaration, and append it to XML document
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(dec);
            XmlElement root = doc.CreateElement("ClientSubUnitPolicyHotelCapRateGroupItems");
            doc.AppendChild(root);

            if (updatedClient.PolicyHotelCapRateGroupItemsAdded != null)
            {
                if (updatedClient.PolicyHotelCapRateGroupItemsAdded.Count > 0)
                {
                    changesExist = true;
                    XmlElement xmlPolicyHotelCapRateGroupItemsAdded = doc.CreateElement("PolicyHotelCapRateGroupItemsAdded");

                    foreach (PolicyHotelCapRateGroupItem item in updatedClient.PolicyHotelCapRateGroupItemsAdded)
                    {
                        //PolicyHotelCapRateGroupItem
                        XmlElement xmlPolicyHotelCapRateGroupItem = doc.CreateElement("PolicyHotelCapRateGroupItem");
                        xmlPolicyHotelCapRateGroupItemsAdded.AppendChild(xmlPolicyHotelCapRateGroupItem);

                        XmlElement xmlPolicyGroupId = doc.CreateElement("PolicyGroupId");
                        xmlPolicyGroupId.InnerText = updatedClient.PolicyGroup.PolicyGroupId.ToString();
                        xmlPolicyHotelCapRateGroupItem.AppendChild(xmlPolicyGroupId);

                        XmlElement xmlPolicyLocationId = doc.CreateElement("PolicyLocationId");
                        xmlPolicyLocationId.InnerText = item.PolicyLocationId.ToString();
                        xmlPolicyHotelCapRateGroupItem.AppendChild(xmlPolicyLocationId);

						//XmlElement xmlPolicyProhibitedFlag = doc.CreateElement("PolicyProhibitedFlag");
						//xmlPolicyProhibitedFlag.InnerText = item.PolicyProhibitedFlag == true ? "1" : "0";
						//xmlPolicyHotelCapRateGroupItem.AppendChild(xmlPolicyProhibitedFlag);

						XmlElement xmlTaxInclusiveFlag = doc.CreateElement("TaxInclusiveFlag");
						xmlTaxInclusiveFlag.InnerText = item.TaxInclusiveFlag == true ? "1" : "0";
						xmlPolicyHotelCapRateGroupItem.AppendChild(xmlTaxInclusiveFlag);

                        XmlElement xmlCapRate = doc.CreateElement("CapRate");
                        xmlCapRate.InnerText = item.CapRate.ToString();
                        xmlPolicyHotelCapRateGroupItem.AppendChild(xmlCapRate);

                        XmlElement xmlCurrencyCode = doc.CreateElement("CurrencyCode");
                        xmlCurrencyCode.InnerText = item.CurrencyCode;
                        xmlPolicyHotelCapRateGroupItem.AppendChild(xmlCurrencyCode);

                        XmlElement xmlEnabledFlag = doc.CreateElement("EnabledFlag");
                        xmlEnabledFlag.InnerText = item.EnabledFlag == true ? "1" : "0";
                        xmlPolicyHotelCapRateGroupItem.AppendChild(xmlEnabledFlag);

                        XmlElement xmlEnabledDate = doc.CreateElement("EnabledDate");
                        if (item.EnabledDate != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.EnabledDate;
                            xmlEnabledDate.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyHotelCapRateGroupItem.AppendChild(xmlEnabledDate);

                        XmlElement xmlExpiryDate = doc.CreateElement("ExpiryDate");
                        if (item.ExpiryDate != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.ExpiryDate;
                            xmlExpiryDate.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyHotelCapRateGroupItem.AppendChild(xmlExpiryDate);

                        XmlElement xmlTravelDateValidFrom = doc.CreateElement("TravelDateValidFrom");
                        if (item.TravelDateValidFrom != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.TravelDateValidFrom;
                            xmlTravelDateValidFrom.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyHotelCapRateGroupItem.AppendChild(xmlTravelDateValidFrom);

                        XmlElement xmlTravelDateValidTo = doc.CreateElement("TravelDateValidTo");
                        if (item.TravelDateValidTo != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.TravelDateValidTo;
                            xmlTravelDateValidTo.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyHotelCapRateGroupItem.AppendChild(xmlTravelDateValidTo);
                    }
                    root.AppendChild(xmlPolicyHotelCapRateGroupItemsAdded);
                }
            }


            if (updatedClient.PolicyHotelCapRateGroupItemsAltered != null)
            {
                if (updatedClient.PolicyHotelCapRateGroupItemsAltered.Count > 0)
                {
                    changesExist = true;
                    XmlElement xmlPolicyHotelCapRateGroupItemsAltered = doc.CreateElement("PolicyHotelCapRateGroupItemsAltered");

                    foreach (PolicyHotelCapRateGroupItem item in updatedClient.PolicyHotelCapRateGroupItemsAltered)
                    {

                        //PolicyHotelCapRateGroupItem
                        XmlElement xmlPolicyHotelCapRateGroupItem = doc.CreateElement("PolicyHotelCapRateGroupItem");
                        xmlPolicyHotelCapRateGroupItemsAltered.AppendChild(xmlPolicyHotelCapRateGroupItem);

                        XmlElement xmlPolicyHotelCapRateItemId = doc.CreateElement("PolicyHotelCapRateItemId");
                        xmlPolicyHotelCapRateItemId.InnerText = item.PolicyHotelCapRateItemId.ToString();
                        xmlPolicyHotelCapRateGroupItem.AppendChild(xmlPolicyHotelCapRateItemId);

                        XmlElement xmlPolicyGroupId = doc.CreateElement("PolicyGroupId");
                        xmlPolicyGroupId.InnerText = updatedClient.PolicyGroup.PolicyGroupId.ToString();
                        xmlPolicyHotelCapRateGroupItem.AppendChild(xmlPolicyGroupId);

                        XmlElement xmlPolicyLocationId = doc.CreateElement("PolicyLocationId");
                        xmlPolicyLocationId.InnerText = item.PolicyLocationId.ToString();
                        xmlPolicyHotelCapRateGroupItem.AppendChild(xmlPolicyLocationId);

						//XmlElement xmlPolicyProhibitedFlag = doc.CreateElement("PolicyProhibitedFlag");
						//xmlPolicyProhibitedFlag.InnerText = item.PolicyProhibitedFlag == true ? "1" : "0";
						//xmlPolicyHotelCapRateGroupItem.AppendChild(xmlPolicyProhibitedFlag);

						XmlElement xmlTaxInclusiveFlag = doc.CreateElement("TaxInclusiveFlag");
						xmlTaxInclusiveFlag.InnerText = item.TaxInclusiveFlag == true ? "1" : "0";
						xmlPolicyHotelCapRateGroupItem.AppendChild(xmlTaxInclusiveFlag);

                        XmlElement xmlCapRate = doc.CreateElement("CapRate");
                        xmlCapRate.InnerText = item.CapRate.ToString();
                        xmlPolicyHotelCapRateGroupItem.AppendChild(xmlCapRate);

                        XmlElement xmlCurrencyCode = doc.CreateElement("CurrencyCode");
                        xmlCurrencyCode.InnerText = item.CurrencyCode;
                        xmlPolicyHotelCapRateGroupItem.AppendChild(xmlCurrencyCode);

                        XmlElement xmlEnabledFlag = doc.CreateElement("EnabledFlag");
                        xmlEnabledFlag.InnerText = item.EnabledFlag == true ? "1" : "0";
                        xmlPolicyHotelCapRateGroupItem.AppendChild(xmlEnabledFlag);

                        XmlElement xmlEnabledDate = doc.CreateElement("EnabledDate");
                        if (item.EnabledDate != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.EnabledDate;
                            xmlEnabledDate.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyHotelCapRateGroupItem.AppendChild(xmlEnabledDate);

                        XmlElement xmlExpiryDate = doc.CreateElement("ExpiryDate");
                        if (item.ExpiryDate != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.ExpiryDate;
                            xmlExpiryDate.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyHotelCapRateGroupItem.AppendChild(xmlExpiryDate);

                        XmlElement xmlTravelDateValidFrom = doc.CreateElement("TravelDateValidFrom");
                        if (item.TravelDateValidFrom != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.TravelDateValidFrom;
                            xmlTravelDateValidFrom.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyHotelCapRateGroupItem.AppendChild(xmlTravelDateValidFrom);

                        XmlElement xmlTravelDateValidTo = doc.CreateElement("TravelDateValidTo");
                        if (item.TravelDateValidTo != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.TravelDateValidTo;
                            xmlTravelDateValidTo.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyHotelCapRateGroupItem.AppendChild(xmlTravelDateValidTo);

                        XmlElement xmlVersionNumber = doc.CreateElement("VersionNumber");
                        xmlVersionNumber.InnerText = item.VersionNumber.ToString();
                        xmlPolicyHotelCapRateGroupItem.AppendChild(xmlVersionNumber);
                    }
                    root.AppendChild(xmlPolicyHotelCapRateGroupItemsAltered);
                }
            }


            if (updatedClient.PolicyHotelCapRateGroupItemsRemoved != null)
            {
                if (updatedClient.PolicyHotelCapRateGroupItemsRemoved.Count > 0)
                {
                    changesExist = true;
                    XmlElement xmlPolicyHotelCapRateGroupItemsRemoved = doc.CreateElement("PolicyHotelCapRateGroupItemsRemoved");

                    foreach (PolicyHotelCapRateGroupItem item in updatedClient.PolicyHotelCapRateGroupItemsRemoved)
                    {
                        //PolicyHotelCapRateGroupItem
                        XmlElement xmlPolicyHotelCapRateGroupItem = doc.CreateElement("PolicyHotelCapRateGroupItem");
                        xmlPolicyHotelCapRateGroupItemsRemoved.AppendChild(xmlPolicyHotelCapRateGroupItem);

                        XmlElement xmlPolicyHotelCapRateItemId = doc.CreateElement("PolicyHotelCapRateItemId");
                        xmlPolicyHotelCapRateItemId.InnerText = item.PolicyHotelCapRateItemId.ToString();
                        xmlPolicyHotelCapRateGroupItem.AppendChild(xmlPolicyHotelCapRateItemId);

                        XmlElement xmlVersionNumber = doc.CreateElement("VersionNumber");
                        xmlVersionNumber.InnerText = item.VersionNumber.ToString();
                        xmlPolicyHotelCapRateGroupItem.AppendChild(xmlVersionNumber);

                    }
                    root.AppendChild(xmlPolicyHotelCapRateGroupItemsRemoved);
                }
            }

            if (changesExist)
            {
                string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

                var output = (from n in db.spDDAWizard_UpdateClientSubUnitPolicyHotelCapRateGroupItems_v1(
                    clientSubUnit.ClientSubUnitGuid,
                    System.Xml.Linq.XElement.Parse(doc.OuterXml),
                    adminUserGuid)
                              select n).ToList();

                foreach (spDDAWizard_UpdateClientSubUnitPolicyHotelCapRateGroupItems_v1Result message in output)
                {
                    wizardMessages.AddMessage(message.MessageText.ToString(), (bool)message.Success);
                }
                
            }
            return wizardMessages;
        }

        //Update ClientSubUnit PolicyHotelPropertyGroupItems
        public WizardMessages UpdateClientSubUnitPolicyHotelPropertyGroupItems(ClientWizardVM updatedClient, WizardMessages wizardMessages)
        {
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = updatedClient.ClientSubUnit;
            bool changesExist = false;

            // Create the xml document container
            XmlDocument doc = new XmlDocument();// Create the XML Declaration, and append it to XML document
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(dec);
            XmlElement root = doc.CreateElement("ClientSubUnitPolicyHotelPropertyGroupItems");
            doc.AppendChild(root);

            if (updatedClient.PolicyHotelPropertyGroupItemsAdded != null)
            {
                if (updatedClient.PolicyHotelPropertyGroupItemsAdded.Count > 0)
                {
                    changesExist = true;
                    XmlElement xmlPolicyHotelPropertyGroupItemsAdded = doc.CreateElement("PolicyHotelPropertyGroupItemsAdded");

                    foreach (PolicyHotelPropertyGroupItem item in updatedClient.PolicyHotelPropertyGroupItemsAdded)
                    {
                        //PolicyHotelPropertyGroupItem
                        XmlElement xmlPolicyHotelPropertyGroupItem = doc.CreateElement("PolicyHotelPropertyGroupItem");
                        xmlPolicyHotelPropertyGroupItemsAdded.AppendChild(xmlPolicyHotelPropertyGroupItem);

                        XmlElement xmlPolicyGroupId = doc.CreateElement("PolicyGroupId");
                        xmlPolicyGroupId.InnerText = updatedClient.PolicyGroup.PolicyGroupId.ToString();
                        xmlPolicyHotelPropertyGroupItem.AppendChild(xmlPolicyGroupId);

                        XmlElement xmlPolicyHotelStatusId = doc.CreateElement("PolicyHotelStatusId");
                        xmlPolicyHotelStatusId.InnerText = item.PolicyHotelStatusId.ToString();
                        xmlPolicyHotelPropertyGroupItem.AppendChild(xmlPolicyHotelStatusId);

                        XmlElement xmlHarpHotelId = doc.CreateElement("HarpHotelId");
                        xmlHarpHotelId.InnerText = item.HarpHotelId.ToString();
                        xmlPolicyHotelPropertyGroupItem.AppendChild(xmlHarpHotelId);

                        XmlElement xmlEnabledFlag = doc.CreateElement("EnabledFlag");
                        xmlEnabledFlag.InnerText = item.EnabledFlag == true ? "1" : "0";
                        xmlPolicyHotelPropertyGroupItem.AppendChild(xmlEnabledFlag);

                        XmlElement xmlEnabledDate = doc.CreateElement("EnabledDate");
                        if (item.EnabledDate != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.EnabledDate;
                            xmlEnabledDate.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyHotelPropertyGroupItem.AppendChild(xmlEnabledDate);

                        XmlElement xmlExpiryDate = doc.CreateElement("ExpiryDate");
                        if (item.ExpiryDate != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.ExpiryDate;
                            xmlExpiryDate.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyHotelPropertyGroupItem.AppendChild(xmlExpiryDate);

                        XmlElement xmlTravelDateValidFrom = doc.CreateElement("TravelDateValidFrom");
                        if (item.TravelDateValidFrom != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.TravelDateValidFrom;
                            xmlTravelDateValidFrom.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyHotelPropertyGroupItem.AppendChild(xmlTravelDateValidFrom);

                        XmlElement xmlTravelDateValidTo = doc.CreateElement("TravelDateValidTo");
                        if (item.TravelDateValidTo != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.TravelDateValidTo;
                            xmlTravelDateValidTo.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyHotelPropertyGroupItem.AppendChild(xmlTravelDateValidTo);
                    }
                    root.AppendChild(xmlPolicyHotelPropertyGroupItemsAdded);
                }
            }


            if (updatedClient.PolicyHotelPropertyGroupItemsAltered != null)
            {
                if (updatedClient.PolicyHotelPropertyGroupItemsAltered.Count > 0)
                {
                    changesExist = true;
                    XmlElement xmlPolicyHotelPropertyGroupItemsAltered = doc.CreateElement("PolicyHotelPropertyGroupItemsAltered");

                    foreach (PolicyHotelPropertyGroupItem item in updatedClient.PolicyHotelPropertyGroupItemsAltered)
                    {

                        //PolicyHotelPropertyGroupItem
                        XmlElement xmlPolicyHotelPropertyGroupItem = doc.CreateElement("PolicyHotelPropertyGroupItem");
                        xmlPolicyHotelPropertyGroupItemsAltered.AppendChild(xmlPolicyHotelPropertyGroupItem);

                        XmlElement xmlPolicyHotelPropertyGroupItemId = doc.CreateElement("PolicyHotelPropertyGroupItemId");
                        xmlPolicyHotelPropertyGroupItemId.InnerText = item.PolicyHotelPropertyGroupItemId.ToString();
                        xmlPolicyHotelPropertyGroupItem.AppendChild(xmlPolicyHotelPropertyGroupItemId);

                        XmlElement xmlPolicyGroupId = doc.CreateElement("PolicyGroupId");
                        xmlPolicyGroupId.InnerText = updatedClient.PolicyGroup.PolicyGroupId.ToString();
                        xmlPolicyHotelPropertyGroupItem.AppendChild(xmlPolicyGroupId);

                        XmlElement xmlPolicyHotelStatusId = doc.CreateElement("PolicyHotelStatusId");
                        xmlPolicyHotelStatusId.InnerText = item.PolicyHotelStatusId.ToString();
                        xmlPolicyHotelPropertyGroupItem.AppendChild(xmlPolicyHotelStatusId);

                        XmlElement xmlHarpHotelId = doc.CreateElement("HarpHotelId");
                        xmlHarpHotelId.InnerText = item.HarpHotelId.ToString();
                        xmlPolicyHotelPropertyGroupItem.AppendChild(xmlHarpHotelId);

                        XmlElement xmlEnabledFlag = doc.CreateElement("EnabledFlag");
                        xmlEnabledFlag.InnerText = item.EnabledFlag == true ? "1" : "0";
                        xmlPolicyHotelPropertyGroupItem.AppendChild(xmlEnabledFlag);

                        XmlElement xmlEnabledDate = doc.CreateElement("EnabledDate");
                        if (item.EnabledDate != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.EnabledDate;
                            xmlEnabledDate.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyHotelPropertyGroupItem.AppendChild(xmlEnabledDate);

                        XmlElement xmlExpiryDate = doc.CreateElement("ExpiryDate");
                        if (item.ExpiryDate != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.ExpiryDate;
                            xmlExpiryDate.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyHotelPropertyGroupItem.AppendChild(xmlExpiryDate);

                        XmlElement xmlTravelDateValidFrom = doc.CreateElement("TravelDateValidFrom");
                        if (item.TravelDateValidFrom != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.TravelDateValidFrom;
                            xmlTravelDateValidFrom.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyHotelPropertyGroupItem.AppendChild(xmlTravelDateValidFrom);

                        XmlElement xmlTravelDateValidTo = doc.CreateElement("TravelDateValidTo");
                        if (item.TravelDateValidTo != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.TravelDateValidTo;
                            xmlTravelDateValidTo.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyHotelPropertyGroupItem.AppendChild(xmlTravelDateValidTo);

                        XmlElement xmlVersionNumber = doc.CreateElement("VersionNumber");
                        xmlVersionNumber.InnerText = item.VersionNumber.ToString();
                        xmlPolicyHotelPropertyGroupItem.AppendChild(xmlVersionNumber);
                    }
                    root.AppendChild(xmlPolicyHotelPropertyGroupItemsAltered);
                }
            }


            if (updatedClient.PolicyHotelPropertyGroupItemsRemoved != null)
            {
                if (updatedClient.PolicyHotelPropertyGroupItemsRemoved.Count > 0)
                {
                    changesExist = true;
                    XmlElement xmlPolicyHotelPropertyGroupItemsRemoved = doc.CreateElement("PolicyHotelPropertyGroupItemsRemoved");

                    foreach (PolicyHotelPropertyGroupItem item in updatedClient.PolicyHotelPropertyGroupItemsRemoved)
                    {
                        //PolicyHotelPropertyGroupItem
                        XmlElement xmlPolicyHotelPropertyGroupItem = doc.CreateElement("PolicyHotelPropertyGroupItem");
                        xmlPolicyHotelPropertyGroupItemsRemoved.AppendChild(xmlPolicyHotelPropertyGroupItem);

                        XmlElement xmlPolicyHotelPropertyGroupItemId = doc.CreateElement("PolicyHotelPropertyGroupItemId");
                        xmlPolicyHotelPropertyGroupItemId.InnerText = item.PolicyHotelPropertyGroupItemId.ToString();
                        xmlPolicyHotelPropertyGroupItem.AppendChild(xmlPolicyHotelPropertyGroupItemId);

                        XmlElement xmlVersionNumber = doc.CreateElement("VersionNumber");
                        xmlVersionNumber.InnerText = item.VersionNumber.ToString();
                        xmlPolicyHotelPropertyGroupItem.AppendChild(xmlVersionNumber);

                    }
                    root.AppendChild(xmlPolicyHotelPropertyGroupItemsRemoved);
                }
            }

            if (changesExist)
            {
                string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

                var output = (from n in db.spDDAWizard_UpdateClientSubUnitPolicyHotelPropertyGroupItems_v1(
                    clientSubUnit.ClientSubUnitGuid,
                    System.Xml.Linq.XElement.Parse(doc.OuterXml),
                    adminUserGuid)
                              select n).ToList();

                foreach (spDDAWizard_UpdateClientSubUnitPolicyHotelPropertyGroupItems_v1Result message in output)
                {
                    wizardMessages.AddMessage(message.MessageText.ToString(), (bool)message.Success);
                }
                
            }
            return wizardMessages;
        }

        //Update ClientSubUnit PolicyHotelVendorGroupItems
        public WizardMessages UpdateClientSubUnitPolicyHotelVendorGroupItems(ClientWizardVM updatedClient, WizardMessages wizardMessages)
        {
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = updatedClient.ClientSubUnit;
            bool changesExist = false;

            // Create the xml document container
            XmlDocument doc = new XmlDocument();// Create the XML Declaration, and append it to XML document
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(dec);
            XmlElement root = doc.CreateElement("ClientSubUnitPolicyHotelVendorGroupItems");
            doc.AppendChild(root);

            if (updatedClient.PolicyHotelVendorGroupItemsAdded != null)
            {
                if (updatedClient.PolicyHotelVendorGroupItemsAdded.Count > 0)
                {
                    changesExist = true;
                    XmlElement xmlPolicyHotelVendorGroupItemsAdded = doc.CreateElement("PolicyHotelVendorGroupItemsAdded");

                    foreach (PolicyHotelVendorGroupItem item in updatedClient.PolicyHotelVendorGroupItemsAdded)
                    {
                        //PolicyHotelVendorGroupItem
                        XmlElement xmlPolicyHotelVendorGroupItem = doc.CreateElement("PolicyHotelVendorGroupItem");
                        xmlPolicyHotelVendorGroupItemsAdded.AppendChild(xmlPolicyHotelVendorGroupItem);

                        XmlElement xmlPolicyGroupId = doc.CreateElement("PolicyGroupId");
                        xmlPolicyGroupId.InnerText = updatedClient.PolicyGroup.PolicyGroupId.ToString();
                        xmlPolicyHotelVendorGroupItem.AppendChild(xmlPolicyGroupId);

                        XmlElement xmlPolicyLocationId  = doc.CreateElement("PolicyLocationId");
                        xmlPolicyLocationId.InnerText = item.PolicyLocationId.ToString();
                        xmlPolicyHotelVendorGroupItem.AppendChild(xmlPolicyLocationId);

                        XmlElement xmlPolicyHotelStatusId = doc.CreateElement("PolicyHotelStatusId");
                        xmlPolicyHotelStatusId.InnerText = item.PolicyHotelStatusId.ToString();
                        xmlPolicyHotelVendorGroupItem.AppendChild(xmlPolicyHotelStatusId);

                        XmlElement xmlEnabledFlag = doc.CreateElement("EnabledFlag");
                        xmlEnabledFlag.InnerText = item.EnabledFlag == true ? "1" : "0";
                        xmlPolicyHotelVendorGroupItem.AppendChild(xmlEnabledFlag);

                        XmlElement xmlEnabledDate = doc.CreateElement("EnabledDate");
                        if (item.EnabledDate != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.EnabledDate;
                            xmlEnabledDate.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyHotelVendorGroupItem.AppendChild(xmlEnabledDate);

                        XmlElement xmlExpiryDate = doc.CreateElement("ExpiryDate");
                        if (item.ExpiryDate != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.ExpiryDate;
                            xmlExpiryDate.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyHotelVendorGroupItem.AppendChild(xmlExpiryDate);

                        XmlElement xmlTravelDateValidFrom = doc.CreateElement("TravelDateValidFrom");
                        if (item.TravelDateValidFrom != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.TravelDateValidFrom;
                            xmlTravelDateValidFrom.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyHotelVendorGroupItem.AppendChild(xmlTravelDateValidFrom);

                        XmlElement xmlTravelDateValidTo = doc.CreateElement("TravelDateValidTo");
                        if (item.TravelDateValidTo != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.TravelDateValidTo;
                            xmlTravelDateValidTo.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyHotelVendorGroupItem.AppendChild(xmlTravelDateValidTo);

                        XmlElement xmlSupplierCode = doc.CreateElement("SupplierCode");
                        xmlSupplierCode.InnerText = item.SupplierCode;
                        xmlPolicyHotelVendorGroupItem.AppendChild(xmlSupplierCode);

                        XmlElement xmlProductId = doc.CreateElement("ProductId");
                        xmlProductId.InnerText = item.ProductId.ToString();
                        xmlPolicyHotelVendorGroupItem.AppendChild(xmlProductId);
                    }
                    root.AppendChild(xmlPolicyHotelVendorGroupItemsAdded);
                }
            }


            if (updatedClient.PolicyHotelVendorGroupItemsAltered != null)
            {
                if (updatedClient.PolicyHotelVendorGroupItemsAltered.Count > 0)
                {
                    changesExist = true;
                    XmlElement xmlPolicyHotelVendorGroupItemsAltered = doc.CreateElement("PolicyHotelVendorGroupItemsAltered");

                    foreach (PolicyHotelVendorGroupItem item in updatedClient.PolicyHotelVendorGroupItemsAltered)
                    {

                        //PolicyHotelVendorGroupItem
                        XmlElement xmlPolicyHotelVendorGroupItem = doc.CreateElement("PolicyHotelVendorGroupItem");
                        xmlPolicyHotelVendorGroupItemsAltered.AppendChild(xmlPolicyHotelVendorGroupItem);

                        XmlElement xmlPolicyHotelVendorGroupItemId = doc.CreateElement("PolicyHotelVendorGroupItemId");
                        xmlPolicyHotelVendorGroupItemId.InnerText = item.PolicyHotelVendorGroupItemId.ToString();
                        xmlPolicyHotelVendorGroupItem.AppendChild(xmlPolicyHotelVendorGroupItemId);

                        XmlElement xmlPolicyGroupId = doc.CreateElement("PolicyGroupId");
                        xmlPolicyGroupId.InnerText = updatedClient.PolicyGroup.PolicyGroupId.ToString();
                        xmlPolicyHotelVendorGroupItem.AppendChild(xmlPolicyGroupId);

                        XmlElement xmlPolicyLocationId = doc.CreateElement("PolicyLocationId");
                        xmlPolicyLocationId.InnerText = item.PolicyLocationId.ToString();
                        xmlPolicyHotelVendorGroupItem.AppendChild(xmlPolicyLocationId);

                        XmlElement xmlPolicyHotelStatusId = doc.CreateElement("PolicyHotelStatusId");
                        xmlPolicyHotelStatusId.InnerText = item.PolicyHotelStatusId.ToString();
                        xmlPolicyHotelVendorGroupItem.AppendChild(xmlPolicyHotelStatusId);

                        XmlElement xmlEnabledFlag = doc.CreateElement("EnabledFlag");
                        xmlEnabledFlag.InnerText = item.EnabledFlag == true ? "1" : "0";
                        xmlPolicyHotelVendorGroupItem.AppendChild(xmlEnabledFlag);

                        XmlElement xmlEnabledDate = doc.CreateElement("EnabledDate");
                        if (item.EnabledDate != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.EnabledDate;
                            xmlEnabledDate.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyHotelVendorGroupItem.AppendChild(xmlEnabledDate);

                        XmlElement xmlExpiryDate = doc.CreateElement("ExpiryDate");
                        if (item.ExpiryDate != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.ExpiryDate;
                            xmlExpiryDate.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyHotelVendorGroupItem.AppendChild(xmlExpiryDate);

                        XmlElement xmlTravelDateValidFrom = doc.CreateElement("TravelDateValidFrom");
                        if (item.TravelDateValidFrom != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.TravelDateValidFrom;
                            xmlTravelDateValidFrom.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyHotelVendorGroupItem.AppendChild(xmlTravelDateValidFrom);

                        XmlElement xmlTravelDateValidTo = doc.CreateElement("TravelDateValidTo");
                        if (item.TravelDateValidTo != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.TravelDateValidTo;
                            xmlTravelDateValidTo.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicyHotelVendorGroupItem.AppendChild(xmlTravelDateValidTo);

                        XmlElement xmlSupplierCode = doc.CreateElement("SupplierCode");
                        xmlSupplierCode.InnerText = item.SupplierCode;
                        xmlPolicyHotelVendorGroupItem.AppendChild(xmlSupplierCode);

                        XmlElement xmlProductId = doc.CreateElement("ProductId");
                        xmlProductId.InnerText = item.ProductId.ToString();
                        xmlPolicyHotelVendorGroupItem.AppendChild(xmlProductId);

                        XmlElement xmlVersionNumber = doc.CreateElement("VersionNumber");
                        xmlVersionNumber.InnerText = item.VersionNumber.ToString();
                        xmlPolicyHotelVendorGroupItem.AppendChild(xmlVersionNumber);
                    }
                    root.AppendChild(xmlPolicyHotelVendorGroupItemsAltered);
                }
            }


            if (updatedClient.PolicyHotelVendorGroupItemsRemoved != null)
            {
                if (updatedClient.PolicyHotelVendorGroupItemsRemoved.Count > 0)
                {
                    changesExist = true;
                    XmlElement xmlPolicyHotelVendorGroupItemsRemoved = doc.CreateElement("PolicyHotelVendorGroupItemsRemoved");

                    foreach (PolicyHotelVendorGroupItem item in updatedClient.PolicyHotelVendorGroupItemsRemoved)
                    {
                        //PolicyHotelVendorGroupItem
                        XmlElement xmlPolicyHotelVendorGroupItem = doc.CreateElement("PolicyHotelVendorGroupItem");
                        xmlPolicyHotelVendorGroupItemsRemoved.AppendChild(xmlPolicyHotelVendorGroupItem);

                        XmlElement xmlPolicyHotelVendorGroupItemId = doc.CreateElement("PolicyHotelVendorGroupItemId");
                        xmlPolicyHotelVendorGroupItemId.InnerText = item.PolicyHotelVendorGroupItemId.ToString();
                        xmlPolicyHotelVendorGroupItem.AppendChild(xmlPolicyHotelVendorGroupItemId);

                        XmlElement xmlVersionNumber = doc.CreateElement("VersionNumber");
                        xmlVersionNumber.InnerText = item.VersionNumber.ToString();
                        xmlPolicyHotelVendorGroupItem.AppendChild(xmlVersionNumber);

                    }
                    root.AppendChild(xmlPolicyHotelVendorGroupItemsRemoved);
                }
            }

            if (changesExist)
            {
                string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

                var output = (from n in db.spDDAWizard_UpdateClientSubUnitPolicyHotelVendorGroupItems_v1(
                    clientSubUnit.ClientSubUnitGuid,
                    System.Xml.Linq.XElement.Parse(doc.OuterXml),
                    adminUserGuid)
                              select n).ToList();

                foreach (spDDAWizard_UpdateClientSubUnitPolicyHotelVendorGroupItems_v1Result message in output)
                {
                    wizardMessages.AddMessage(message.MessageText.ToString(), (bool)message.Success);
                }
                
            }
            return wizardMessages;
        }

        //Update ClientSubUnit PolicySupplierDealCodes
        public WizardMessages UpdateClientSubUnitPolicySupplierDealCodes(ClientWizardVM updatedClient, WizardMessages wizardMessages)
        {
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = updatedClient.ClientSubUnit;
            bool changesExist = false;

            // Create the xml document container
            XmlDocument doc = new XmlDocument();// Create the XML Declaration, and append it to XML document
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(dec);
            XmlElement root = doc.CreateElement("ClientSubUnitPolicySupplierDealCodes");
            doc.AppendChild(root);

            if (updatedClient.PolicySupplierDealCodesAdded != null)
            {
                if (updatedClient.PolicySupplierDealCodesAdded.Count > 0)
                {
                    changesExist = true;
                    XmlElement xmlPolicySupplierDealCodesAdded = doc.CreateElement("PolicySupplierDealCodesAdded");

                    foreach (PolicySupplierDealCode item in updatedClient.PolicySupplierDealCodesAdded)
                    {
                        //PolicySupplierDealCode
                        XmlElement xmlPolicySupplierDealCode = doc.CreateElement("PolicySupplierDealCode");
                        xmlPolicySupplierDealCodesAdded.AppendChild(xmlPolicySupplierDealCode);

                        XmlElement xmlPolicyGroupId = doc.CreateElement("PolicyGroupId");
                        xmlPolicyGroupId.InnerText = updatedClient.PolicyGroup.PolicyGroupId.ToString();
                        xmlPolicySupplierDealCode.AppendChild(xmlPolicyGroupId);
                        XmlElement xmlPolicySupplierDealCodeValue = doc.CreateElement("PolicySupplierDealCodeValue");
                        xmlPolicySupplierDealCodeValue.InnerText = System.Web.HttpUtility.UrlDecode(item.PolicySupplierDealCodeValue);
                        xmlPolicySupplierDealCode.AppendChild(xmlPolicySupplierDealCodeValue);

                        XmlElement xmlPolicySupplierDealCodeDescription = doc.CreateElement("PolicySupplierDealCodeDescription");
                        xmlPolicySupplierDealCodeDescription.InnerText = item.PolicySupplierDealCodeDescription;
                        xmlPolicySupplierDealCode.AppendChild(xmlPolicySupplierDealCodeDescription);

                        XmlElement xmlEnabledFlag = doc.CreateElement("EnabledFlag");
                        xmlEnabledFlag.InnerText = item.EnabledFlag == true ? "1" : "0";
                        xmlPolicySupplierDealCode.AppendChild(xmlEnabledFlag);

                        XmlElement xmlEnabledDate = doc.CreateElement("EnabledDate");
                        if (item.EnabledDate != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.EnabledDate;
                            xmlEnabledDate.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicySupplierDealCode.AppendChild(xmlEnabledDate);

                        XmlElement xmlExpiryDate = doc.CreateElement("ExpiryDate");
                        if (item.ExpiryDate != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.ExpiryDate;
                            xmlExpiryDate.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicySupplierDealCode.AppendChild(xmlExpiryDate);

                        XmlElement xmlGDSCode = doc.CreateElement("GDSCode");
                        xmlGDSCode.InnerText = item.GDSCode;
                        xmlPolicySupplierDealCode.AppendChild(xmlGDSCode);

                        XmlElement xmlPolicySupplierDealCodeTypeId = doc.CreateElement("PolicySupplierDealCodeTypeId");
                        xmlPolicySupplierDealCodeTypeId.InnerText = item.PolicySupplierDealCodeTypeId.ToString();
                        xmlPolicySupplierDealCode.AppendChild(xmlPolicySupplierDealCodeTypeId);

                        XmlElement xmlSupplierCode = doc.CreateElement("SupplierCode");
                        xmlSupplierCode.InnerText = item.SupplierCode;
                        xmlPolicySupplierDealCode.AppendChild(xmlSupplierCode);

                        XmlElement xmlProductId = doc.CreateElement("ProductId");
                        xmlProductId.InnerText = item.ProductId.ToString();
                        xmlPolicySupplierDealCode.AppendChild(xmlProductId);

                        XmlElement xmlPolicyLocationId = doc.CreateElement("PolicyLocationId");
                        xmlPolicyLocationId.InnerText = item.PolicyLocationId.ToString();
                        xmlPolicySupplierDealCode.AppendChild(xmlPolicyLocationId);
                    }
                    root.AppendChild(xmlPolicySupplierDealCodesAdded);
                }
            }


            if (updatedClient.PolicySupplierDealCodesAltered != null)
            {
                if (updatedClient.PolicySupplierDealCodesAltered.Count > 0)
                {
                    changesExist = true;
                    XmlElement xmlPolicySupplierDealCodesAltered = doc.CreateElement("PolicySupplierDealCodesAltered");

                    foreach (PolicySupplierDealCode item in updatedClient.PolicySupplierDealCodesAltered)
                    {

                        //PolicySupplierDealCode
                        XmlElement xmlPolicySupplierDealCode = doc.CreateElement("PolicySupplierDealCode");
                        xmlPolicySupplierDealCodesAltered.AppendChild(xmlPolicySupplierDealCode);

                        XmlElement xmlPolicySupplierDealCodeId = doc.CreateElement("PolicySupplierDealCodeId");
                        xmlPolicySupplierDealCodeId.InnerText = item.PolicySupplierDealCodeId.ToString();
                        xmlPolicySupplierDealCode.AppendChild(xmlPolicySupplierDealCodeId);

                        XmlElement xmlPolicyGroupId = doc.CreateElement("PolicyGroupId");
                        xmlPolicyGroupId.InnerText = updatedClient.PolicyGroup.PolicyGroupId.ToString();
                        xmlPolicySupplierDealCode.AppendChild(xmlPolicyGroupId);

                        XmlElement xmlPolicySupplierDealCodeValue = doc.CreateElement("PolicySupplierDealCodeValue");
                        xmlPolicySupplierDealCodeValue.InnerText = item.PolicySupplierDealCodeValue;
                        xmlPolicySupplierDealCode.AppendChild(xmlPolicySupplierDealCodeValue);

                        XmlElement xmlPolicySupplierDealCodeDescription = doc.CreateElement("PolicySupplierDealCodeDescription");
                        xmlPolicySupplierDealCodeDescription.InnerText = item.PolicySupplierDealCodeDescription;
                        xmlPolicySupplierDealCode.AppendChild(xmlPolicySupplierDealCodeDescription);

                        XmlElement xmlEnabledFlag = doc.CreateElement("EnabledFlag");
                        xmlEnabledFlag.InnerText = item.EnabledFlag == true ? "1" : "0";
                        xmlPolicySupplierDealCode.AppendChild(xmlEnabledFlag);

                        XmlElement xmlEnabledDate = doc.CreateElement("EnabledDate");
                        if (item.EnabledDate != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.EnabledDate;
                            xmlEnabledDate.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicySupplierDealCode.AppendChild(xmlEnabledDate);

                        XmlElement xmlExpiryDate = doc.CreateElement("ExpiryDate");
                        if (item.ExpiryDate != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.ExpiryDate;
                            xmlExpiryDate.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicySupplierDealCode.AppendChild(xmlExpiryDate);

                        XmlElement xmlGDSCode = doc.CreateElement("GDSCode");
                        xmlGDSCode.InnerText = item.GDSCode;
                        xmlPolicySupplierDealCode.AppendChild(xmlGDSCode);

                        XmlElement xmlPolicySupplierDealCodeTypeId = doc.CreateElement("PolicySupplierDealCodeTypeId");
                        xmlPolicySupplierDealCodeTypeId.InnerText = item.PolicySupplierDealCodeTypeId.ToString();
                        xmlPolicySupplierDealCode.AppendChild(xmlPolicySupplierDealCodeTypeId);

                        XmlElement xmlSupplierCode = doc.CreateElement("SupplierCode");
                        xmlSupplierCode.InnerText = item.SupplierCode;
                        xmlPolicySupplierDealCode.AppendChild(xmlSupplierCode);

                        XmlElement xmlProductId = doc.CreateElement("ProductId");
                        xmlProductId.InnerText = item.ProductId.ToString();
                        xmlPolicySupplierDealCode.AppendChild(xmlProductId);

                        XmlElement xmlPolicyLocationId = doc.CreateElement("PolicyLocationId");
                        xmlPolicyLocationId.InnerText = item.PolicyLocationId.ToString();
                        xmlPolicySupplierDealCode.AppendChild(xmlPolicyLocationId);

                        XmlElement xmlVersionNumber = doc.CreateElement("VersionNumber");
                        xmlVersionNumber.InnerText = item.VersionNumber.ToString();
                        xmlPolicySupplierDealCode.AppendChild(xmlVersionNumber);
                    }
                    root.AppendChild(xmlPolicySupplierDealCodesAltered);
                }
            }


            if (updatedClient.PolicySupplierDealCodesRemoved != null)
            {
                if (updatedClient.PolicySupplierDealCodesRemoved.Count > 0)
                {
                    changesExist = true;
                    XmlElement xmlPolicySupplierDealCodesRemoved = doc.CreateElement("PolicySupplierDealCodesRemoved");

                    foreach (PolicySupplierDealCode item in updatedClient.PolicySupplierDealCodesRemoved)
                    {
                        //PolicySupplierDealCode
                        XmlElement xmlPolicySupplierDealCode = doc.CreateElement("PolicySupplierDealCode");
                        xmlPolicySupplierDealCodesRemoved.AppendChild(xmlPolicySupplierDealCode);

                        XmlElement xmlPolicySupplierDealCodeId = doc.CreateElement("PolicySupplierDealCodeId");
                        xmlPolicySupplierDealCodeId.InnerText = item.PolicySupplierDealCodeId.ToString();
                        xmlPolicySupplierDealCode.AppendChild(xmlPolicySupplierDealCodeId);

                        XmlElement xmlVersionNumber = doc.CreateElement("VersionNumber");
                        xmlVersionNumber.InnerText = item.VersionNumber.ToString();
                        xmlPolicySupplierDealCode.AppendChild(xmlVersionNumber);

                    }
                    root.AppendChild(xmlPolicySupplierDealCodesRemoved);
                }
            }

            if (changesExist)
            {
                string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

                var output = (from n in db.spDDAWizard_UpdateClientSubUnitPolicySupplierDealCodes_v1(
                    clientSubUnit.ClientSubUnitGuid,
                    System.Xml.Linq.XElement.Parse(doc.OuterXml),
                    adminUserGuid)
                              select n).ToList();

                foreach (spDDAWizard_UpdateClientSubUnitPolicySupplierDealCodes_v1Result message in output)
                {
                    wizardMessages.AddMessage(message.MessageText.ToString(), (bool)message.Success);
                }
                
            }
            return wizardMessages;
        }

        //Update ClientSubUnit PolicySupplierServiceInformations
        public WizardMessages UpdateClientSubUnitPolicySupplierServiceInformations(ClientWizardVM updatedClient, WizardMessages wizardMessages)
        {
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = updatedClient.ClientSubUnit;
            bool changesExist = false;

            // Create the xml document container
            XmlDocument doc = new XmlDocument();// Create the XML Declaration, and append it to XML document
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(dec);
            XmlElement root = doc.CreateElement("ClientSubUnitPolicySupplierServiceInformations");
            doc.AppendChild(root);

            if (updatedClient.PolicySupplierServiceInformationsAdded != null)
            {
                if (updatedClient.PolicySupplierServiceInformationsAdded.Count > 0)
                {
                    changesExist = true;
                    XmlElement xmlPolicySupplierServiceInformationsAdded = doc.CreateElement("PolicySupplierServiceInformationsAdded");

                    foreach (PolicySupplierServiceInformation item in updatedClient.PolicySupplierServiceInformationsAdded)
                    {
                        //PolicySupplierServiceInformation
                        XmlElement xmlPolicySupplierServiceInformation = doc.CreateElement("PolicySupplierServiceInformation");
                        xmlPolicySupplierServiceInformationsAdded.AppendChild(xmlPolicySupplierServiceInformation);

                        XmlElement xmlPolicyGroupId = doc.CreateElement("PolicyGroupId");
                        xmlPolicyGroupId.InnerText = updatedClient.PolicyGroup.PolicyGroupId.ToString();
                        xmlPolicySupplierServiceInformation.AppendChild(xmlPolicyGroupId);

                        XmlElement xmlPolicySupplierServiceInformationValue = doc.CreateElement("PolicySupplierServiceInformationValue");
                        xmlPolicySupplierServiceInformationValue.InnerText = System.Web.HttpUtility.UrlDecode(item.PolicySupplierServiceInformationValue);
                        xmlPolicySupplierServiceInformation.AppendChild(xmlPolicySupplierServiceInformationValue);

                        XmlElement xmlPolicySupplierServiceInformationTypeId = doc.CreateElement("PolicySupplierServiceInformationTypeId");
                        xmlPolicySupplierServiceInformationTypeId.InnerText = item.PolicySupplierServiceInformationTypeId.ToString();
                        xmlPolicySupplierServiceInformation.AppendChild(xmlPolicySupplierServiceInformationTypeId);

                        XmlElement xmlEnabledFlag = doc.CreateElement("EnabledFlag");
                        xmlEnabledFlag.InnerText = item.EnabledFlag == true ? "1" : "0";
                        xmlPolicySupplierServiceInformation.AppendChild(xmlEnabledFlag);

                        XmlElement xmlEnabledDate = doc.CreateElement("EnabledDate");
                        if (item.EnabledDate != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.EnabledDate;
                            xmlEnabledDate.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicySupplierServiceInformation.AppendChild(xmlEnabledDate);

                        XmlElement xmlExpiryDate = doc.CreateElement("ExpiryDate");
                        if (item.ExpiryDate != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.ExpiryDate;
                            xmlExpiryDate.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicySupplierServiceInformation.AppendChild(xmlExpiryDate);

                        XmlElement xmlSupplierCode = doc.CreateElement("SupplierCode");
                        xmlSupplierCode.InnerText = item.SupplierCode;
                        xmlPolicySupplierServiceInformation.AppendChild(xmlSupplierCode);

                        XmlElement xmlProductId = doc.CreateElement("ProductId");
                        xmlProductId.InnerText = item.ProductId.ToString();
                        xmlPolicySupplierServiceInformation.AppendChild(xmlProductId);
                    }
                    root.AppendChild(xmlPolicySupplierServiceInformationsAdded);
                }
            }


            if (updatedClient.PolicySupplierServiceInformationsAltered != null)
            {
                if (updatedClient.PolicySupplierServiceInformationsAltered.Count > 0)
                {
                    changesExist = true;
                    XmlElement xmlPolicySupplierServiceInformationsAltered = doc.CreateElement("PolicySupplierServiceInformationsAltered");

                    foreach (PolicySupplierServiceInformation item in updatedClient.PolicySupplierServiceInformationsAltered)
                    {

                        //PolicySupplierServiceInformation
                        XmlElement xmlPolicySupplierServiceInformation = doc.CreateElement("PolicySupplierServiceInformation");
                        xmlPolicySupplierServiceInformationsAltered.AppendChild(xmlPolicySupplierServiceInformation);

                        XmlElement xmlPolicySupplierServiceInformationId = doc.CreateElement("PolicySupplierServiceInformationId");
                        xmlPolicySupplierServiceInformationId.InnerText = item.PolicySupplierServiceInformationId.ToString();
                        xmlPolicySupplierServiceInformation.AppendChild(xmlPolicySupplierServiceInformationId);

                        XmlElement xmlPolicyGroupId = doc.CreateElement("PolicyGroupId");
                        xmlPolicyGroupId.InnerText = updatedClient.PolicyGroup.PolicyGroupId.ToString();
                        xmlPolicySupplierServiceInformation.AppendChild(xmlPolicyGroupId);

                        XmlElement xmlPolicySupplierServiceInformationValue = doc.CreateElement("PolicySupplierServiceInformationValue");
                        xmlPolicySupplierServiceInformationValue.InnerText = System.Web.HttpUtility.UrlDecode(item.PolicySupplierServiceInformationValue);
                        xmlPolicySupplierServiceInformation.AppendChild(xmlPolicySupplierServiceInformationValue);

                        XmlElement xmlPolicySupplierServiceInformationTypeId = doc.CreateElement("PolicySupplierServiceInformationTypeId");
                        xmlPolicySupplierServiceInformationTypeId.InnerText = item.PolicySupplierServiceInformationTypeId.ToString();
                        xmlPolicySupplierServiceInformation.AppendChild(xmlPolicySupplierServiceInformationTypeId);

                        XmlElement xmlEnabledFlag = doc.CreateElement("EnabledFlag");
                        xmlEnabledFlag.InnerText = item.EnabledFlag == true ? "1" : "0";
                        xmlPolicySupplierServiceInformation.AppendChild(xmlEnabledFlag);

                        XmlElement xmlEnabledDate = doc.CreateElement("EnabledDate");
                        if (item.EnabledDate != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.EnabledDate;
                            xmlEnabledDate.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicySupplierServiceInformation.AppendChild(xmlEnabledDate);

                        XmlElement xmlExpiryDate = doc.CreateElement("ExpiryDate");
                        if (item.ExpiryDate != null)
                        {
                            DateTime x = new DateTime();
                            x = (DateTime)item.ExpiryDate;
                            xmlExpiryDate.InnerText = x.ToString("yyyyMMdd");
                        }
                        xmlPolicySupplierServiceInformation.AppendChild(xmlExpiryDate);

                        XmlElement xmlSupplierCode = doc.CreateElement("SupplierCode");
                        xmlSupplierCode.InnerText = item.SupplierCode;
                        xmlPolicySupplierServiceInformation.AppendChild(xmlSupplierCode);

                        XmlElement xmlProductId = doc.CreateElement("ProductId");
                        xmlProductId.InnerText = item.ProductId.ToString();
                        xmlPolicySupplierServiceInformation.AppendChild(xmlProductId);

                        XmlElement xmlVersionNumber = doc.CreateElement("VersionNumber");
                        xmlVersionNumber.InnerText = item.VersionNumber.ToString();
                        xmlPolicySupplierServiceInformation.AppendChild(xmlVersionNumber);
                    }
                    root.AppendChild(xmlPolicySupplierServiceInformationsAltered);
                }
            }


            if (updatedClient.PolicySupplierServiceInformationsRemoved != null)
            {
                if (updatedClient.PolicySupplierServiceInformationsRemoved.Count > 0)
                {
                    changesExist = true;
                    XmlElement xmlPolicySupplierServiceInformationsRemoved = doc.CreateElement("PolicySupplierServiceInformationsRemoved");

                    foreach (PolicySupplierServiceInformation item in updatedClient.PolicySupplierServiceInformationsRemoved)
                    {
                        //PolicySupplierServiceInformation
                        XmlElement xmlPolicySupplierServiceInformation = doc.CreateElement("PolicySupplierServiceInformation");
                        xmlPolicySupplierServiceInformationsRemoved.AppendChild(xmlPolicySupplierServiceInformation);

                        XmlElement xmlPolicySupplierServiceInformationId = doc.CreateElement("PolicySupplierServiceInformationId");
                        xmlPolicySupplierServiceInformationId.InnerText = item.PolicySupplierServiceInformationId.ToString();
                        xmlPolicySupplierServiceInformation.AppendChild(xmlPolicySupplierServiceInformationId);

                        XmlElement xmlVersionNumber = doc.CreateElement("VersionNumber");
                        xmlVersionNumber.InnerText = item.VersionNumber.ToString();
                        xmlPolicySupplierServiceInformation.AppendChild(xmlVersionNumber);

                    }
                    root.AppendChild(xmlPolicySupplierServiceInformationsRemoved);
                }
            }

            if (changesExist)
            {
                string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

                var output = (from n in db.spDDAWizard_UpdateClientSubUnitPolicySupplierServiceInformations_v1(
                    clientSubUnit.ClientSubUnitGuid,
                    System.Xml.Linq.XElement.Parse(doc.OuterXml),
                    adminUserGuid)
                              select n).ToList();

                foreach (spDDAWizard_UpdateClientSubUnitPolicySupplierServiceInformations_v1Result message in output)
                {
                    wizardMessages.AddMessage(message.MessageText.ToString(), (bool)message.Success);
                }
                
            }
            return wizardMessages;
        }

    }
}
        