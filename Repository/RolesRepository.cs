using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Repository
{
    public class RolesRepository
    {
        private RolesDC db = new RolesDC(Settings.getConnectionString());
        private string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];


		//Check if current user has Write Access to an ApprovalGroup 
		public bool HasWriteAccessToApprovalGroup(int approvalGroupId)
		{
			var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToApprovalGroup_v1(approvalGroupId, adminUserGuid);
			return result;
		}

		//Check if current user has Write Access to Configuration Parameters
        public bool HasWriteAccessToGroupHierarchyLevel(string roleName, string hierarchyLevel)
        {
            var result = (bool)db.fnDesktopDataAdmin_GetAdminUserDomainHierarchyLevelWriteAccess_v1(adminUserGuid, roleName, hierarchyLevel);
            return result;
        }


        //Check if current user has Write Access to Configuration Parameters
        public bool HasWriteAccessToCDRLinkImport()
        {
            var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToCDRLinkImport_v1(adminUserGuid);
            return result;
        }

		//Check if current user has Write Access to a ChatFAQResponseGroup
		public bool HasWriteAccessToChatFAQResponseGroup(int chatFAQResponseGroupId)
		{
			var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToChatFAQResponseGroup_v1(chatFAQResponseGroupId, adminUserGuid);
			return result;
		}

		//Check if current user has Write Access to a ChatFAQResponseItem Import
		public bool HasWriteAccessToChatFAQResponseItemImport()
		{
			var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToChatFAQResponseItemImport_v1(adminUserGuid);
			return result;
		}
		
        //Check if current user has Write Access to a ClientAccount
		public bool HasWriteAccessToClientAccount(string clientAccountNumber, string sourceCodeSystem)
		{
			var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToClientAccount_v1(clientAccountNumber, sourceCodeSystem, adminUserGuid);
			return result;
		}

		//Check if user is allow to edit order / create new ClientDefinedReferenceItems
		public bool HasWriteAccessToClientDefinedReferenceItem(string clientSubUnitGuid, string clientAccountNumber, string sourceCodeSystem)
		{
			var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToClientDefinedReferenceItem_v1(clientSubUnitGuid, clientAccountNumber, sourceCodeSystem, adminUserGuid);
			return result;
		}

		//Check if user is allow to edit order / create new ClientDefinedReferenceItems
		public bool HasWriteAccessToClientSubUnitClientDefinedReferenceItem(string clientSubUnitGuid)
		{
			var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToClientSubUnitClientDefinedReferenceItem_v1(clientSubUnitGuid, adminUserGuid);
			return result;
		}

		//Check if user is allow to edit order / create new ClientDefinedReferenceItems
		public bool HasWriteAccessToClientSubUnitClientDefinedReferenceItemValue(string clientDefinedReferenceItemId)
		{
			var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToClientSubUnitClientDefinedReferenceItemValue_v1(clientDefinedReferenceItemId, adminUserGuid);
			return result;
		}
        
		//Check if current user has Write Access to ClientAccount CreditCards 
        public bool HasWriteAccessToClientAccountCreditCards(string clientAccountNumber, string sourceCodeSystem)
        {
            var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToClientAccountCreditCard_v1(clientAccountNumber, sourceCodeSystem, adminUserGuid);
            return result;
        }

		//Check if current user has Write Access to a ClientDefinedRuleGroup 
		public bool HasWriteAccessToClientDefinedRuleGroup(int clientDefinedRuleGroupId)
		{
			var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToClientDefinedRuleGroup_v1(clientDefinedRuleGroupId, adminUserGuid);
			return result;
		}

		//Check if current user has Write Access to a ClientFeeGroup 
		public bool HasWriteAccessToClientFeeGroup(int clientFeeGroupid)
		{
			var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToClientFeeGroup_v1(clientFeeGroupid, adminUserGuid);
			return result;
		}

		//Check if current user has Write Access to a ClientProfileGroup 
		public bool HasWriteAccessToClientProfileGroup(int clientProfileGroupId)
		{
            var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToClientProfileGroup_v1(clientProfileGroupId, adminUserGuid);
			return result;
		}

		//Check if current user has Write Access to a ClientProfileAdminGroup 
		public bool HasWriteAccessToClientProfileAdminGroup(int clientProfileAdminGroupId)
		{
            var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToClientProfileAdminGroup_v1(clientProfileAdminGroupId, adminUserGuid);
			return result;
		}

		//Check if current user has Write Access to a ClientSubUnit
        public bool HasWriteAccessToClientSubUnit(string clientSubUnitGuid)
        {
            var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToClientSubUnit_v1(clientSubUnitGuid, adminUserGuid);
            return result;
        }

        //Check if current user has Write Access to ClientSubUnit CreditCards 
        public bool HasWriteAccessToClientSubUnitCreditCards(string clientSubUnitGuid)
        {
            var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToClientSubUnitCreditCard_v1(clientSubUnitGuid, adminUserGuid);
            return result;
        }

        //Check if current user has Write Access to a ClientTopUnit
        public bool HasWriteAccessToClientTopUnit(string clientTopUnitGuid)
        {
            var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToClientTopUnit_v1(clientTopUnitGuid, adminUserGuid);
            return result;
        }

        //Check if current user has Write Access to ClientTopUnit CreditCards 
        public bool HasWriteAccessToClientTopUnitCreditCards(string clientTopUnitGuid)
        {
            var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToClientTopUnitCreditCard_v1(clientTopUnitGuid, adminUserGuid);
            return result;
        }
		
		//Check if current user has Write Access to ClientTopUnit Location Import 
        public bool HasWriteAccessToClientTopUnitClientLocationImport(string clientTopUnitGuid)
        {
            var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToClientTopUnitClientLocationImport_v1(clientTopUnitGuid, adminUserGuid);
            return result;
        }

		//Check if current user has Write Access to a CommissionableRouteGroup 
		public bool HasWriteAccessToCommissionableRouteGroup(int commissionableRouteGroupId)
		{
			var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToCommissionableRouteGroup_v1(commissionableRouteGroupId, adminUserGuid);
			return result;
		}
		
		//Check if current user has Write Access to Configuration Parameters
        public bool HasWriteAccessToConfigurationParameters()
        {
            var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToConfigurationParameters_v1(adminUserGuid);
            return result;
        }

        //Check if current user has Write Access to a ExternalSystemParameter 
        public bool HasWriteAccessToExternalSystemParameter(int externalSystemParameterid)
        {
            var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToExternalSystemParameter_v1(externalSystemParameterid, adminUserGuid);
            return result;
        }

		//Check if current user has Write Access to a GDSAdditionalEntry 
		public bool HasWriteAccessToGDSAdditionalEntry(int gdsAdditionalEntryid)
		{
			var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToGDSAdditionalEntry_v1(gdsAdditionalEntryid, adminUserGuid);
			return result;
		}

		//Check if current user has Write Access to a GDSEndWarningConfiguration 
		public bool HasWriteAccessToGDSEndWarningConfiguration()
		{
			var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToGDSEndWarningConfiguration_v1(adminUserGuid);
			return result;
		}

		//Check if current user has Write Access to a GDSOrderType 
		public bool HasWriteAccessToGDSOrderType()
		{
			var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToGDSOrderType_v1(adminUserGuid);
			return result;
		}

		//Check if current user has Write Access to a GDSOrderDetail
		public bool HasWriteAccessToGDSOrderDetail()
		{
			var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToGDSOrderDetail_v1(adminUserGuid);
			return result;
		}

        //Check if current user has Write Access to a LocalOperatingHoursGroup 
        public bool HasWriteAccessToLocation(int? locationId)
        {
            var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToLocation_v1(locationId, adminUserGuid);
            return result;
        }

        //Check if current user has Write Access to a LocalOperatingHoursGroup 
        public bool HasWriteAccessToLocalOperatingHoursGroup(int localOperatingHoursGroupid)
        {
            var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToLocalOperatingHoursGroup_v1(localOperatingHoursGroupid, adminUserGuid);
            return result;
        }

        //Check if current user has Write Access to OptionalField 
        public bool HasWriteAccessToOptionalFieldGroup(int optionalFieldGroupid)
        {
            var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToOptionalFieldGroup_v1(optionalFieldGroupid, adminUserGuid);
            return result;
        }

        //Check if current user has Write Access to a PNROutputGroup 
        public bool HasWriteAccessToPNROutputGroup(int pnrOutputGroupid)
        {
            var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToPNROutputGroup_v1(pnrOutputGroupid, adminUserGuid);
            return result;
        }

        //Check if current user has Write Access to PointOfSaleFeeLoad 
        public bool HasWriteAccessToPointOfSaleFeeLoad(int pointOfSaleFeeLoadId)
        {
            var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToPointOfSaleFeeLoad_v1(pointOfSaleFeeLoadId, adminUserGuid);

            return result;
        }
        
        //Check if current user has Write Access to PolicyGroup 
        public bool HasWriteAccessToPolicyGroup(int priceTrackingSetupGroupid)
        {
            var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToPolicyGroup_v1(priceTrackingSetupGroupid, adminUserGuid);
            return result;
        }

         //Check if current user has Write Access to PolicyGroup 
        public bool HasWriteAccessToPolicyGroupMessages(int priceTrackingSetupGroupid)
        {
            var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToPolicyGroupMessages_v1(priceTrackingSetupGroupid, adminUserGuid);
            return result;
        }

		//Check if current user has Write Access to Policy HotelCapRate Import
		public bool HasWriteAccessToPolicyHotelCapRateImport(int priceTrackingSetupGroupid)
		{
			var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToPolicyHotelCapRateImport_v1(priceTrackingSetupGroupid, adminUserGuid);
			return result;
		}
        
        //Check if current user has Write Access to Policy Online Other Group Items
		public bool HasWriteAccessToPolicyOnlineOtherGroupItemRepository()
		{
			var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToPolicyOnlineOtherGroupItem_v1(adminUserGuid);
			return result;
		}

        //Check if current user has Write Access to an PriceTrackingHandlingFeeGroup 
        public bool HasWriteAccessToPriceTrackingHandlingFeeGroup(int priceTrackingHandlingFeeGroupId)
        {
            var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToPriceTrackingHandlingFeeGroup_v1(priceTrackingHandlingFeeGroupId, adminUserGuid);
            return result;
        }

        //Check if current user has Write Access to an PriceTrackingSetupGroup 
        public bool HasWriteAccessToPriceTrackingSetupGroup(int priceTrackingHandlingFeeGroupId)
        {
            var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToPriceTrackingSetupGroup_v1(priceTrackingHandlingFeeGroupId, adminUserGuid);
            return result;
        }

        //Check if current user has Write Access to ProductGroup 
        public bool HasWriteAccessToProductGroup(int productGroupid)
        {
            var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToProductGroup_v1(productGroupid, adminUserGuid);
            return result;
        }

		//Check if current user has Write Access to edit PseudoCityOrOfficeMaintenance_CubaPseudoCityOrOfficeFlag
		public bool PseudoCityOrOfficeMaintenance_CubaPseudoCityOrOfficeFlag()
        {
			var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToPseudoCityOrOfficeMaintenance_CubaPseudoCityOrOfficeFlag_v1(adminUserGuid);
            return result;
        }

        //Check if current user has Write Access to a PublicHolidayGroup 
        public bool HasWriteAccessToPublicHolidayGroup(int publicHolidayGroupId)
        {
            var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToPublicHolidayGroup_v1(publicHolidayGroupId, adminUserGuid);
            return result;
        }

        //Check if current user has Write Access to a QueueMinderGroup 
        public bool HasWriteAccessToQueueMinderGroup(int queueMinderGroupId)
        {
            var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToQueueMinderGroup_v1(queueMinderGroupId, adminUserGuid);
            return result;
        }

        //Check if current user has Write Access to a QueueMinderItem
        public bool HasWriteAccessToQueueMinderItem(int queueMinderItemId)
        {
            var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToQueueMinderItem_v1(queueMinderItemId, adminUserGuid);
            return result;
        }
		
        //Check if current user has Write Access to a ReasonCodeGroup 
        public bool HasWriteAccessToReasonCodeGroup(int reasonCodeGroupId)
        {
            var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToReasonCodeGroup_v1(reasonCodeGroupId, adminUserGuid);
            return result;
        }

        //Check if current user has Write Access to Reference Info
        public bool HasWriteAccessToReferenceInfo()
        {
            var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToReferenceInfo_v1(adminUserGuid);
            return result;
        }

		//Check if current user has Write Access to Service Accounts 
		public bool HasWriteAccessToServiceAccounts()
		{
			var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToServiceAccounts_v1(adminUserGuid);
			return result;
		}

		//Check if current user has Write Access to a ServiceAccount CubaBookingAllowanceIndicator
		public bool HasWriteAccessToServiceAccountCubaBookingAllowanceIndicator()
		{
			var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToServiceAccount_CubaBookingAllowanceIndicator_v1(adminUserGuid);
			return result;
		}

		//Check if current user has Write Access to a ServiceAccount MilitaryAndGovernmentUserFlag
		public bool HasWriteAccessToServiceAccountMilitaryAndGovernmentUserFlag()
		{
			var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToServiceAccount_MilitaryAndGovernmentUserFlag_v1(adminUserGuid);
			return result;
		}
        
		//Check if current user has Write Access to a ServicingOptionGroup 
        public bool HasWriteAccessToServicingOptionGroup(int servicingOptionGroupid)
        {
            var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToServicingOptionGroup_v1(servicingOptionGroupid, adminUserGuid);
            return result;
        }

        //Check if current user has Write Access to a SystemUser Roles 
        public bool HasWriteAccessToSystemUserRoles(string systemuserGuid)
        {
            var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToSystemUserRoles_v1(systemuserGuid, adminUserGuid);
            return result;
        }

		//Check if current user has Write Access to a SystemUser GDSAccessRight
		public bool HasWriteAccessToSystemUserGDSAccessRight(int systemUserGDSAccessRightId)
		{
			var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToSystemUserGDSAccessRight_v1(systemUserGDSAccessRightId, adminUserGuid);
			return result;
		}

		//Check if current user has Write Access to a SystemUser GDSAccessRights
		public bool HasWriteAccessToSystemUserGDSAccessRights(string systemuserGuid)
		{
			var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToSystemUserGDSAccessRights_v1(systemuserGuid, adminUserGuid);
			return result;
		}
        
        //Check if current user has Write Access to a SystemUser RestrictedFlag
		public bool HasWriteAccessToSystemUserOnlineUserFlag()
		{
			var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToSystemUserOnlineUserFlag_v1(adminUserGuid);
			return result;
		}

		//Check if current user has Write Access to a SystemUser RestrictedFlag
		public bool HasWriteAccessToSystemUserRestrictedFlag(string systemuserGuid)
		{
			var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToSystemUser_RestrictedFlag_v1(systemuserGuid, adminUserGuid);
			return result;
		}

		//Check if current user has Write Access to a SystemUser MilitaryAndGovernmentUserFlag
		public bool HasWriteAccessToSystemUserMilitaryAndGovernmentUserFlag(string systemuserGuid)
		{
			var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToSystemUserMilitaryAndGovernmentUserFlag_v1(systemuserGuid, adminUserGuid);
			return result;
		}

		//Check if current user has Write Access to a Team 
		public bool HasWriteAccessToTeam(int teamId)
        {
            var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToTeam_v1(teamId, adminUserGuid);
            return result;
        }

        //Check if current user has Write Access to a TeamOutOfOfficeGroup 
        public bool HasWriteAccessToTeamOutOfOfficeGroup(int teamOutOfOfficeGroupId)
        {
            var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToTeamOutOfOfficeGroup_v1(teamOutOfOfficeGroupId, adminUserGuid);
            return result;
        }

		//Check if current user has Write Access to TeamOutOfOfficeGroup Import
		public bool HasWriteAccessToTeamOutOfOfficeGroup()
		{
			var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToTeamOutOfOfficeGroupImport_v1(adminUserGuid);
			return result;
		}

        //Check if current user has Write Access to a ThirdPartyUser GDSAccessRights
        public bool HasWriteAccessToThirdPartyGDSAccessRight(int thirdPartyUserGDSAccessRightId)
		{
			var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToThirdPartyGDSAccessRight_v1(thirdPartyUserGDSAccessRightId, adminUserGuid);
			return result;
		}

		//Check if current user has Write Access to a ThirdPartyUser GDSAccessRights
		public bool HasWriteAccessToThirdPartyGDSAccessRights(int thirdPartyUserId)
		{
			var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToThirdPartyGDSAccessRights_v1(thirdPartyUserId, adminUserGuid);
			return result;
		}
		
		//Check if current user has Write Access to a ThirdPartyUser CubaBookingAllowanceIndicator
		public bool HasWriteAccessToThirdPartyUserCubaBookingAllowanceIndicator()
		{
			var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToThirdPartyUser_CubaBookingAllowanceIndicator_v1(adminUserGuid);
			return result;
		}

		//Check if current user has Write Access to a ThirdPartyUser MilitaryAndGovernmentUserFlag
		public bool HasWriteAccessToThirdPartyUserMilitaryAndGovernmentUserFlag()
		{
			var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToThirdPartyUser_MilitaryAndGovernmentUserFlag_v1(adminUserGuid);
			return result;
		}

        //Check if current user has Write Access to a TicketQueueGroup 
        public bool HasWriteAccessToTicketQueueGroup(int ticketQueueGroupid)
        {
            var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToTicketQueueGroup_v1(ticketQueueGroupid, adminUserGuid);
            return result;
        }

        //Check if current user has Write Access to a TripTypeGroup 
        public bool HasWriteAccessToTripTypeGroup(int tripTypeGroupId)
        {
            var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToTripTypeGroup_v1(tripTypeGroupId, adminUserGuid);
            return result;
        }

        //Check if current user has Write Access to a WorkflowGroup 
        public bool HasWriteAccessToWorkflowGroup(int workflowGroupid)
        {
            var result = (bool)db.fnDesktopDataAdmin_GetWriteAccessToWorkflowGroup_v1(workflowGroupid, adminUserGuid);
            return result;
        }
    }
}