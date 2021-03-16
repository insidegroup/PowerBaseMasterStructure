using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Repository;

namespace CWTDesktopDatabase.Controllers
{
    public class GroupNameBuilderController : Controller
    {
        GroupNameBuilderRepository groupNameBuilderRepository = new GroupNameBuilderRepository();

        //Build PolicyGroupName (for ClientAccount)
        //returns a 3-digit number eg. "001" used to identify groups with the same properties
        [HttpPost]
        public JsonResult BuildGroupNameClientAccount(string clientAccountNumber, string sourceSystemCode, string group)
        {
            var result = groupNameBuilderRepository.ClientAccountIdentifier(sourceSystemCode, clientAccountNumber, group);
            return Json(result);
        }

		//Build ClientProfileAdminGroup (for ClientProfileAdminGroup)
        //returns a 3-digit number eg. "001" used to identify groups with the same properties
        //removed DMC 22 Jul 2014, no counter in ClientProfileAdminGroup
        /*[HttpPost]
		public JsonResult BuildGroupNameClientProfileAdminGroup(string hierarchyType, string hierarchyItem, string gdsCode, string backOffice)
        {
			var result = groupNameBuilderRepository.ClientProfileAdminGroup(hierarchyType, hierarchyItem, gdsCode, backOffice);
            return Json(result);
        }
         * */

		//Build PolicyGroupName (except for ClientAccount and ClientProfileAdminGroupId)
        //returns a 3-digit number eg. "001" used to identify groups with the same properties
        [HttpPost]
        public JsonResult BuildGroupName(string hierarchyType, string hierarchyItem, string group)
        {
            string result = "";

            if (hierarchyType == "ClientTopUnit")
            {
                result = groupNameBuilderRepository.ClientTopUnitIdentifier(hierarchyItem, group);
            }
            if (hierarchyType == "ClientSubUnit")
            {
                result = groupNameBuilderRepository.ClientSubUnitIdentifier(hierarchyItem, group);
            }
            if (hierarchyType == "ClientSubUnitTravelerType")
            {
                result = groupNameBuilderRepository.ClientSubUnitTravelerTypeIdentifier(hierarchyItem, group);
            }
            if (hierarchyType == "TravelerType")
            {
                result = groupNameBuilderRepository.TravelerTypeIdentifier(hierarchyItem, group);
            }
            if (hierarchyType == "Team")
            {
                result = groupNameBuilderRepository.GetTeamIdentifier(Convert.ToInt32(hierarchyItem), group);
            }
            if (hierarchyType == "Location")
            {
                result = groupNameBuilderRepository.GetLocationIdentifier(Convert.ToInt32(hierarchyItem), group);
            }
            if (hierarchyType == "CountryRegion")
            {
                result = groupNameBuilderRepository.GetCountryRegionIdentifier(Convert.ToInt32(hierarchyItem), group);
            }
            if (hierarchyType == "Country")
            {
                result = groupNameBuilderRepository.GetCountryIdentifier(hierarchyItem, group);
            }
            if (hierarchyType == "GlobalSubRegion")
            {
                result = groupNameBuilderRepository.GetGlobalSubRegionIdentifier(hierarchyItem, group);
            }
            if (hierarchyType == "GlobalRegion")
            {
                result = groupNameBuilderRepository.GetGlobalRegionIdentifier(hierarchyItem, group);
            }
            if (hierarchyType == "Global")
            {
                result = groupNameBuilderRepository.GetGlobalIdentifier(Convert.ToInt32(hierarchyItem), group);
            }

            return Json(result);

        }

		//on form submit - check if name already exists
		//returns boolean as Json
		[HttpPost]
		public JsonResult IsAvailableApprovalGroupName(string groupName, int id)
		{
			if (groupName == "")
			{
				return Json(false);
			}
			var result = groupNameBuilderRepository.IsAvailableApprovalGroupName(groupName, id);
			return Json(result);
		}

        //on form submit - check if name already exists
        //returns boolean as Json
        [HttpPost]
        public JsonResult IsAvailableApprovalGroupApprovalTypeId(int approvalGroupApprovalTypeId, int newApprovalGroupApprovalTypeId)
        {
            if (approvalGroupApprovalTypeId == newApprovalGroupApprovalTypeId)
            {
                return Json(true);
            }
            var result = groupNameBuilderRepository.IsAvailableApprovalGroupApprovalTypeId(approvalGroupApprovalTypeId, newApprovalGroupApprovalTypeId);
            return Json(result);
        }

        //on form submit - check if name already exists
        //returns boolean as Json
        [HttpPost]
        public JsonResult IsAvailableApprovalGroupApprovalTypeName(string groupName, int id)
        {
            if (groupName == "")
            {
                return Json(false);
            }
            var result = groupNameBuilderRepository.IsAvailableApprovalGroupApprovalTypeName(groupName, id);
            return Json(result);
        }

        //on form submit - check if name already exists
        //returns boolean as Json
        [HttpPost]
        public JsonResult IsAvailableChatFAQResponseGroupName(string groupName, int id)
        {
            if (groupName == "")
            {
                return Json(false);
            }
            var result = groupNameBuilderRepository.IsAvailableChatFAQResponseGroupName(groupName, id);
            return Json(result);
        }

        //on form submit - check if name already exists
        //returns boolean as Json
        [HttpPost]
        public JsonResult IsAvailableChatMessageFAQName(string groupName, int id)
        {
            if (groupName == "")
            {
                return Json(false);
            }
            var result = groupNameBuilderRepository.IsAvailableChatMessageFAQName(groupName, id);
            return Json(result);
        }

        //on form submit - check if name already exists
        //returns boolean as Json
        [HttpPost]
		public JsonResult IsAvailableClientDefinedReferenceValueItem(string name, string id, string currentId)
		{
			if (name == "")
			{
				return Json(false);
			}
			var result = groupNameBuilderRepository.IsAvailableClientDefinedReferenceItemValue(name, id, currentId);
			return Json(result);
		}

		//on form submit - check if name already exists
		//returns boolean as Json
		[HttpPost]
		public JsonResult IsAvailableClientDetailName(string groupName, int id)
		{
			if (groupName == "")
			{
				return Json(false);
			}
			var result = groupNameBuilderRepository.IsAvailableClientDetailName(groupName, id);
			return Json(result);
		}

		//on form submit - check if name already exists
		//returns boolean as Json
		[HttpPost]
		public JsonResult IsAvailableClientDefinedRuleGroupName(string groupName, string hierarchyType, string hierarchyItem, int id)
		{
			if (groupName == "")
			{
				return Json(false);
			}
			var result = groupNameBuilderRepository.IsAvailableClientDefinedRuleGroupName(groupName, hierarchyType, hierarchyItem, id);
			return Json(result);
		}

		//on form submit - check if name already exists
		//returns boolean as Json
		[HttpPost]
		public JsonResult IsAvailableClientDefinedBusinessRuleGroupName(string groupName, string category, int id)
		{
			if (groupName == "")
			{
				return Json(false);
			}
			var result = groupNameBuilderRepository.IsAvailableClientDefinedBusinessRuleGroupName(groupName, category, id);
			return Json(result);
		}

		//on form submit - check if name already exists
		//returns boolean as Json
		[HttpPost]
		public JsonResult IsAvailableClientFeeGroupName(string groupName, int id)
		{
			if (groupName == "")
			{
				return Json(false);
			}
			var result = groupNameBuilderRepository.IsAvailableClientFeeGroupName(groupName, id);
			return Json(result);
		}

        //on form submit - check if name already exists
        //returns boolean as Json
        [HttpPost]
        public JsonResult IsAvailableClientProfileAdminGroupName(string groupName, int id)
        {
            if (groupName == "")
            {
                return Json(false);
            }
            var result = groupNameBuilderRepository.IsAvailableClientProfileAdminGroupName(groupName, id);
            return Json(result);
        }

		//on form submit - check if name already exists
		//returns boolean as Json
		[HttpPost]
		public JsonResult IsAvailableClientProfileName(string groupName, int? id)
		{
			if (groupName == "")
			{
				return Json(false);
			}
			var result = groupNameBuilderRepository.IsAvailableClientProfileName(groupName, id);
			return Json(result);
		}
		
		//on form submit - check if name already exists
		//returns boolean as Json
		[HttpPost]
		public JsonResult IsAvailableCommissionableRouteGroupName(string groupName, int id)
		{
			if (groupName == "")
			{
				return Json(false);
			}
			var result = groupNameBuilderRepository.IsAvailableCommissionableRouteGroupName(groupName, id);
			return Json(result);
		}

		//on form submit - check if GDS PCC combo is correct
		//returns boolean as Json
		[HttpPost]
		public JsonResult IsValidPccGDS(string pcc, string gds)
		{
			if (pcc == "" || gds == "")
			{
				return Json(false);
			}
			var result = groupNameBuilderRepository.IsValidPccGDS(pcc, gds);
			return Json(result);
		}

        //on form submit - check if name already exists
        //returns boolean as Json
        [HttpPost]
        public JsonResult IsAvailableLocalOperatingHoursGroupName(string groupName, int id)
        {
            if (groupName == "")
            {
                return Json(false);
            }
            var result = groupNameBuilderRepository.IsAvailableLocalOperatingHoursGroupName(groupName, id);
            return Json(result);
        }

		//on form submit - check if name already exists
		//returns boolean as Json
		[HttpPost]
		public JsonResult IsAvailableExternalName(string groupName, int id)
		{
			if (groupName == "")
			{
				return Json(false);
			}
			var result = groupNameBuilderRepository.IsAvailableExternalName(groupName, id);
			return Json(result);
		}

		//on form submit - check if name already exists
		//returns boolean as Json
		[HttpPost]
		public JsonResult IsAvailableFareRedistributionName(string groupName, int id, string gdsCode)
		{
			if (groupName == "")
			{
				return Json(false);
			}
			var result = groupNameBuilderRepository.IsAvailableFareRedistributionName(groupName, id, gdsCode);
			return Json(result);
		}

		//on form submit - check if name already exists
		//returns boolean as Json
		[HttpPost]
		public JsonResult IsAvailableFormOfPaymentAdviceMessageGroupName(string groupName, int id)
		{
			if (groupName == "")
			{
				return Json(false);
			}
			var result = groupNameBuilderRepository.IsAvailableFormOfPaymentAdviceMessageGroupName(groupName, id);
			return Json(result);
		}
        
		//on form submit - check if name already exists
		//returns boolean as Json
		[HttpPost]
		public JsonResult IsAvailableGDSAccessTypeName(string groupName, int id, string gdsCode)
		{
			if (groupName == "")
			{
				return Json(false);
			}
			var result = groupNameBuilderRepository.IsAvailableGDSAccessTypeName(groupName, id, gdsCode);
			return Json(result);
		}

		//on form submit - check if name already exists
		//returns boolean as Json
		[HttpPost]
		public JsonResult IsAvailableGDSOrderDetailName(string groupName, int id)
		{
			if (groupName == "")
			{
				return Json(false);
			}
			var result = groupNameBuilderRepository.IsAvailableGDSOrderDetailName(groupName, id);
			return Json(result);
		}
		
		//on form submit - check if name already exists
		//returns boolean as Json
		[HttpPost]
		public JsonResult IsAvailableGDSOrderTypeName(string groupName, int id)
		{
			if (groupName == "")
			{
				return Json(false);
			}
			var result = groupNameBuilderRepository.IsAvailableGDSOrderTypeName(groupName, id);
			return Json(result);
		}
		
		//on form submit - check if name already exists
		//returns boolean as Json
		[HttpPost]
		public JsonResult IsAvailableGDSRequestTypeName(string groupName, int id)
		{
			if (groupName == "")
			{
				return Json(false);
			}
			var result = groupNameBuilderRepository.IsAvailableGDSRequestTypeName(groupName, id);
			return Json(result);
		}

		//on form submit - check if name already exists
        //returns boolean as Json
        [HttpPost]
		public JsonResult IsAvailableMeetingName(string groupName, int? id)
        {
            if (groupName == "")
            {
                return Json(false);
            }
			var result = groupNameBuilderRepository.IsAvailableMeetingName(groupName, id);
            return Json(result);
        }

		//on form submit - check if name already exists
        //returns boolean as Json
        [HttpPost]
        public JsonResult IsAvailableOptionalFieldGroupName(string groupName, int id)
        {
            if (groupName == "")
            {
                return Json(false);
            }
            var result = groupNameBuilderRepository.IsAvailableOptionalFieldGroupName(groupName, id);
            return Json(result);
        }

        //on form submit - check if name already exists
        //returns boolean as Json
        [HttpPost]
        public JsonResult IsAvailablePointOfSaleFeeLoad(int? id, string clientTopUnitGuid, string clientSubUnitGuid, string travelerTypeGuid, string feeLoadDescriptionTypeCode, int productId, bool agentInitiatedFlag, string travelIndicator)
        {
            var result = groupNameBuilderRepository.IsAvailablePointOfSaleFeeLoad(id, clientTopUnitGuid, clientSubUnitGuid, travelerTypeGuid, feeLoadDescriptionTypeCode, productId, agentInitiatedFlag, travelIndicator);
            return Json(result);
        }

        //on form submit - check if name already exists
        //returns boolean as Json
        [HttpPost]
        public JsonResult IsAvailableProductGroupName(string groupName, int id)
        {
            if (groupName == "")
            {
                return Json(false);
            }
            var result = groupNameBuilderRepository.IsAvailableProductGroupName(groupName, id);
            return Json(result);
        }

        //on form submit - check if name already exists
        //returns boolean as Json
        [HttpPost]
		public JsonResult IsAvailablePartner(string groupName, string countryCode, int id)
		{
			if (groupName == "")
			{
				return Json(false);
			}
			var result = groupNameBuilderRepository.IsAvailablePartner(groupName, countryCode, id);
			return Json(result);
		}

        //on form submit - check if name already exists
        //returns boolean as Json
        [HttpPost]
        public JsonResult IsAvailablePNROutputGroupName(string groupName, int id)
        {
            if (groupName == "")
            {
                return Json(false);
            }
            var result = groupNameBuilderRepository.IsAvailablePNROutputGroupName(groupName, id);
            return Json(result);
        }

        //on form submit - check if name already exists
        //returns boolean as Json
        [HttpPost]
        public JsonResult IsAvailablePolicyGroupName(string groupName, int id)
        {
            if (groupName == "")
            {
                return Json(false);
            }
            var result = groupNameBuilderRepository.IsAvailablePolicyGroupName(groupName, id);
            return Json(result);
        }
        
        //on form submit - check if name already exists
        //returns boolean as Json
        [HttpPost]
		public JsonResult IsAvailablePriceTrackingHandlingFeeGroupName(string groupName, int id)
		{
			if (groupName == "")
			{
				return Json(false);
			}
			var result = groupNameBuilderRepository.IsAvailablePriceTrackingHandlingFeeGroupName(groupName, id);
			return Json(result);
		}
        
        //on form submit - check if name already exists
        //returns boolean as Json
        [HttpPost]
        public JsonResult IsAvailablePriceTrackingSetupGroup(string gdsCode, string pseudoCityOrOfficeId, string hierarchyType, string hierarchyCode, int id)
        {
            if (gdsCode == "" || pseudoCityOrOfficeId == "")
            {
                return Json(false);
            }
            var result = groupNameBuilderRepository.IsAvailablePriceTrackingSetupGroup(gdsCode, pseudoCityOrOfficeId, hierarchyType, hierarchyCode, id);
            return Json(result);
        }

        //on form submit - check if name already exists
        //returns boolean as Json
        [HttpPost]
        public JsonResult IsAvailablePublicHolidayGroupName(string groupName, int id)
        {
            if (groupName == "")
            {
                return Json(false);
            }
            var result = groupNameBuilderRepository.IsAvailablePublicHolidayGroupName(groupName, id);
            return Json(result);
        }

		//on form submit - check if name already exists
		//returns boolean as Json
		[HttpPost]
		public JsonResult IsAvailablePseudoCityOrOfficeAddressName(string cityName, int id, string firstLineAddress)
		{
			if (cityName == "" || firstLineAddress == "")
			{
				return Json(false);
			}
			var result = groupNameBuilderRepository.IsAvailablePseudoCityOrOfficeAddressName(cityName, id, firstLineAddress);
			return Json(result);
		}
		
		//on form submit - check if name already exists
		//returns boolean as Json
		[HttpPost]
		public JsonResult IsAvailablePseudoCityOrOfficeTypeName(string groupName, int id)
		{
			if (groupName == "")
			{
				return Json(false);
			}
			var result = groupNameBuilderRepository.IsAvailablePseudoCityOrOfficeTypeName(groupName, id);
			return Json(result);
		}

		//on form submit - check if name already exists
		//returns boolean as Json
		[HttpPost]
		public JsonResult IsAvailablePseudoCityOrOfficeDefinedRegionName(string groupName, int id, string globalRegionCode)
		{
			if (groupName == "")
			{
				return Json(false);
			}
			var result = groupNameBuilderRepository.IsAvailablePseudoCityOrOfficeDefinedRegionName(groupName, id, globalRegionCode);
			return Json(result);
		}
		
		//on form submit - check if name already exists
		//returns boolean as Json
		[HttpPost]
		public JsonResult IsAvailablePseudoCityOrOfficeLocationTypeName(string groupName, int id)
		{
			if (groupName == "")
			{
				return Json(false);
			}
			var result = groupNameBuilderRepository.IsAvailablePseudoCityOrOfficeLocationTypeName(groupName, id);
			return Json(result);
		}

        //on form submit - check if name already exists
        //returns boolean as Json
        [HttpPost]
        public JsonResult IsAvailableQueueMinderGroupName(string groupName, int id)
        {
            if (groupName == "")
            {
                return Json(false);
            }
            var result = groupNameBuilderRepository.IsAvailableQueueMinderGroupName(groupName, id);
            return Json(result);
        }

        //on form submit - check if name already exists
        //returns boolean as Json
        [HttpPost]
        public JsonResult IsAvailableReasonCodeGroupName(string groupName, int id)
        {
            if (groupName == "")
            {
                return Json(false);
            }
            var result = groupNameBuilderRepository.IsAvailableReasonCodeGroupName(groupName, id);
            return Json(result);
        }

        //on form submit - check if name already exists
        //returns boolean as Json
        [HttpPost]
        public JsonResult IsAvailableServicingOptionGroupName(string groupName, int id)
        {
            if (groupName == "")
            {
                return Json(false);
            }
            var result = groupNameBuilderRepository.IsAvailableServicingOptionGroupName(groupName, id);
            return Json(result);
        }

        //on form submit - check if name already exists
        //returns boolean as Json
        [HttpPost]
        public JsonResult IsAvailableTeamOutOfOfficeGroupName(string groupName, int id)
        {
            if (groupName == "")
            {
                return Json(false);
            }
            var result = groupNameBuilderRepository.IsAvailableTeamOutOfOfficeGroupName(groupName, id);
            return Json(result);
        }
        
        //on form submit - check if name already exists
         //returns boolean as Json
        [HttpPost]
		public JsonResult IsAvailableThirdPartyUserName(string groupName, int id)
		{
			if (groupName == "")
			{
				return Json(false);
			}
			var result = groupNameBuilderRepository.IsAvailableThirdPartyUserName(groupName, id);
			return Json(result);
		}

        //on form submit - check if name already exists
        //returns boolean as Json
        [HttpPost]
        public JsonResult IsAvailableServiceAccountName(string groupName, string id)
        {
            if (groupName == "")
            {
                return Json(false);
            }
            var result = groupNameBuilderRepository.IsAvailableServiceAccountName(groupName, id);
            return Json(result);
        }

        //on form submit - check if name already exists
        //returns boolean as Json
        [HttpPost]
        public JsonResult IsAvailableGDSAccessRightGDSSignOnID(string groupName, int id, string gdsSignOnID, string pseudoCityOrOfficeId)
        {
            if (gdsSignOnID == "")
            {
                return Json(false);
            }
            var result = groupNameBuilderRepository.IsAvailableGDSAccessRightGDSSignOnID(groupName, id, gdsSignOnID, pseudoCityOrOfficeId);
            return Json(result);
        }

        //on form submit - check if name already exists
        //returns boolean as Json
        [HttpPost]
        public JsonResult IsAvailableTicketQueueGroupName(string groupName, int id)
        {
            if (groupName == "")
            {
                return Json(false);
            }
            var result = groupNameBuilderRepository.IsAvailableTicketQueueGroupName(groupName, id);
            return Json(result);
        }

        //on form submit - check if name already exists
        //returns boolean as Json
        [HttpPost]
        public JsonResult IsAvailableTripTypeGroupName(string groupName, int id)
        {
            if (groupName == "")
            {
                return Json(false);
            }
            var result = groupNameBuilderRepository.IsAvailableTripTypeGroupName(groupName, id);
            return Json(result);
        }

        //on form submit - check if name already exists
        //returns boolean as Json
        [HttpPost]
        public JsonResult IsAvailableWorkFlowGroupName(string groupName, int id)
        {
            if (groupName == "")
            {
                return Json(false);
            }
            var result = groupNameBuilderRepository.IsAvailableWorkFlowGroupName(groupName, id);
            return Json(result);
        }
    }
}
