using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;


namespace CWTDesktopDatabase.Repository
{
    public class GroupNameBuilderRepository
    {
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());
		private ClientDefinedRuleDC clientDefinedRuleDC = new ClientDefinedRuleDC(Settings.getConnectionString());
		private MeetingDC meetingDC = new MeetingDC(Settings.getConnectionString());
        private PriceTrackingDC priceTrackingDC = new PriceTrackingDC(Settings.getConnectionString());
        private PointOfSaleFeeLoadDataContext pointOfSaleFeeLoadDataContext = new PointOfSaleFeeLoadDataContext(Settings.getConnectionString());
		private ChatFAQResponseGroupDC chatFAQResponseGroupDC = new ChatFAQResponseGroupDC(Settings.getConnectionString());
		private FormOfPaymentAdviceMessageGroupDC formOfPaymentAdviceMessageGroupDC = new FormOfPaymentAdviceMessageGroupDC(Settings.getConnectionString());

        #region Get Identifier - return a 3 digit number

        //return a 3 digit number, this number is included as part of the groupname to make it unique
        public string ClientTopUnitIdentifier(string clientTopUnitId, string group)
        {
            int? Id = db.fnDesktopDataAdmin_GetFunctionalAreaNameClientTopUnitCounter_v1(clientTopUnitId, group);
            int nonNullId = (Id.HasValue) ? Id.Value : 0;
            string result = nonNullId.ToString();
            return result.PadLeft(3, '0');
        }
        
        //return a 3 digit number, this number is included as part of the groupname to make it unique
        public string ClientSubUnitIdentifier(string clientSubUnitId, string group)
        {
            int? Id = db.fnDesktopDataAdmin_GetFunctionalAreaNameClientSubUnitCounter_v1(clientSubUnitId, group);
            int nonNullId = (Id.HasValue) ? Id.Value : 0;
            string result = nonNullId.ToString();
            return result.PadLeft(3, '0');
        }
        
        //return a 3 digit number, this number is included as part of the groupname to make it unique
        public string ClientSubUnitTravelerTypeIdentifier(string clientSubUnitId, string group)
        {
            int? Id = db.fnDesktopDataAdmin_GetFunctionalAreaNameClientSubUnitTravelerTypeCounter_v1(clientSubUnitId, group);
            int nonNullId = (Id.HasValue) ? Id.Value : 0;
            string result = nonNullId.ToString();
            return result.PadLeft(3, '0');
        }
        
        //return a 3 digit number, this number is included as part of the groupname to make it unique
        public string TravelerTypeIdentifier(string travelerTypeId, string group)
        {
            int? Id = db.fnDesktopDataAdmin_GetFunctionalAreaNameTravelerTypeCounter_v1(travelerTypeId, group);
            int nonNullId = (Id.HasValue) ? Id.Value : 0;
            string result = nonNullId.ToString();
            return result.PadLeft(3, '0');
        }

        //return a 3 digit number, this number is included as part of the groupname to make it unique
        public string ClientAccountIdentifier(string sourceSystemCode, string clientAccountNumber, string group)
        {
            int? Id = db.fnDesktopDataAdmin_GetFunctionalAreaNameClientAccountCounter_v1(sourceSystemCode, clientAccountNumber, group);
            int nonNullId = (Id.HasValue) ? Id.Value : 0;
            string result = nonNullId.ToString();
            return result.PadLeft(3, '0');
        }
        
		//return a 3 digit number, this number is included as part of the groupname to make it unique
        //removed DMC 22 Jul 2014, no counter in ClientProfileAdminGroup
		/*public string ClientProfileAdminGroup(string hierarchyType, string hierarchyItem, string gdsCode, string backOffice)
        {
			int? Id = db.fnDesktopDataAdmin_GetFunctionalAreaNameClientProfileAdminGroupNameCounter_v1(hierarchyType, hierarchyItem, gdsCode, backOffice);
            int nonNullId = (Id.HasValue) ? Id.Value : 0;
            string result = nonNullId.ToString();
            return result.PadLeft(3, '0');
        }*/
		
        //return a 3 digit number, this number is included as part of the groupname to make it unique
        public string GetTeamIdentifier(int teamId, string group)
        {
            int? Id = db.fnDesktopDataAdmin_GetFunctionalAreaNameTeamCounter_v1(teamId, group);
            int nonNullId = (Id.HasValue) ? Id.Value : 0;
            string result = nonNullId.ToString();
            return result.PadLeft(3, '0');
        }
        
        //return a 3 digit number, this number is included as part of the groupname to make it unique
        public string GetLocationIdentifier(int locationId, string group)
        {
            int? Id = db.fnDesktopDataAdmin_GetFunctionalAreaNameLocationCounter_v1(locationId, group);
            int nonNullId = (Id.HasValue) ? Id.Value : 0;
            string result = nonNullId.ToString();
            return result.PadLeft(3, '0');
        }
        
        //return a 3 digit number, this number is included as part of the groupname to make it unique
        public string GetCountryRegionIdentifier(int countryRegionId, string group)
        {
            int? Id = db.fnDesktopDataAdmin_GetFunctionalAreaNameCountryRegionCounter_v1(countryRegionId, group);
            int nonNullId = (Id.HasValue) ? Id.Value : 0;
            string result = nonNullId.ToString();
            return result.PadLeft(3, '0');
        }
        
        //return a 3 digit number, this number is included as part of the groupname to make it unique
        public string GetCountryIdentifier(string countryCode, string group)
        {
            int? Id = db.fnDesktopDataAdmin_GetFunctionalAreaNameCountryCounter_v1(countryCode, group);
            int nonNullId = (Id.HasValue) ? Id.Value : 0;
            string result = nonNullId.ToString();
            return result.PadLeft(3, '0');
        }
        
        //return a 3 digit number, this number is included as part of the groupname to make it unique
        public string GetGlobalSubRegionIdentifier(string globalSubRegionCode, string group)
        {
            int? Id = db.fnDesktopDataAdmin_GetFunctionalAreaNameGlobalSubRegionCounter_v1(globalSubRegionCode, group);
            int nonNullId = (Id.HasValue) ? Id.Value : 0;
            string result = nonNullId.ToString();
            return result.PadLeft(3, '0');
        }
        
        //return a 3 digit number, this number is included as part of the groupname to make it unique
        public string GetGlobalRegionIdentifier(string globalRegionCode, string group)
        {
            int? Id = db.fnDesktopDataAdmin_GetFunctionalAreaNameGlobalRegionCounter_v1(globalRegionCode, group);
            int nonNullId = (Id.HasValue) ? Id.Value : 0;
            string result = nonNullId.ToString();
            return result.PadLeft(3, '0');
        }
        
        //return a 3 digit number, this number is included as part of the groupname to make it unique
        public string GetGlobalIdentifier(int globalId, string group)
        {
            int? Id = db.fnDesktopDataAdmin_GetFunctionalAreaNameGlobalCounter_v1(globalId, group);
            int nonNullId = (Id.HasValue) ? Id.Value : 0;
            string result = nonNullId.ToString();
            return result.PadLeft(3, '0');
        }

        
        #endregion

		//on form submit - check user input against existing items
		//if editing, id is passed to ignore current item
		public bool IsAvailableApprovalGroupName(string groupName, int? groupId)
		{
			int count = 0;

			if (groupId.HasValue)
			{
				var result = from n in db.ApprovalGroups
							 where n.ApprovalGroupName.Trim().Equals(groupName) && n.ApprovalGroupId != groupId
							 select n.ApprovalGroupName;
				count = result.Count();
			}
			else
			{
				var result = from n in db.ApprovalGroups
							 where n.ApprovalGroupName.Trim().Equals(groupName)
							 select n.ApprovalGroupName;
				count = result.Count();
			}
			if (count == 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
        
        //on form submit - check user input against existing items
		//if editing, id is passed to ignore current item
		public bool IsAvailableApprovalGroupApprovalTypeId(int approvalGroupApprovalTypeId, int? newApprovalGroupApprovalTypeId)
		{
			int count = 0;

			if (newApprovalGroupApprovalTypeId.HasValue && newApprovalGroupApprovalTypeId != 0 && approvalGroupApprovalTypeId != newApprovalGroupApprovalTypeId.Value)
			{
				var result = from n in db.ApprovalGroupApprovalTypes
							 where n.ApprovalGroupApprovalTypeId.Equals(newApprovalGroupApprovalTypeId)
							 select n.ApprovalGroupApprovalTypeDescription;
				count = result.Count();
			}
			else
			{
				var result = from n in db.ApprovalGroupApprovalTypes
							 where n.ApprovalGroupApprovalTypeId.Equals(approvalGroupApprovalTypeId)
							 select n.ApprovalGroupApprovalTypeDescription;
				count = result.Count();
			}
			if (count == 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

        //on form submit - check user input against existing items
        //if editing, id is passed to ignore current item
        public bool IsAvailableApprovalGroupApprovalTypeName(string groupName, int? groupId)
        {
            int count = 0;

            if (groupId.HasValue)
            {
                var result = from n in db.ApprovalGroupApprovalTypes
                             where n.ApprovalGroupApprovalTypeDescription.Trim().Equals(groupName) && n.ApprovalGroupApprovalTypeId != groupId
                             select n.ApprovalGroupApprovalTypeDescription;
                count = result.Count();
            }
            else
            {
                var result = from n in db.ApprovalGroupApprovalTypes
                             where n.ApprovalGroupApprovalTypeDescription.Trim().Equals(groupName)
                             select n.ApprovalGroupApprovalTypeDescription;
                count = result.Count();
            }
            if (count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //on form submit - check user input against existing items
        //if editing, id is passed to ignore current item
        public bool IsAvailableChatFAQResponseGroupName(string groupName, int? groupId)
        {
            int count = 0;

            if (groupId.HasValue)
            {
                var result = from n in chatFAQResponseGroupDC.ChatFAQResponseGroups
                             where n.ChatFAQResponseGroupName.Trim().Equals(groupName) && n.ChatFAQResponseGroupId != groupId
                             select n.ChatFAQResponseGroupName;
                count = result.Count();
            }
            else
            {
                var result = from n in chatFAQResponseGroupDC.ChatFAQResponseGroups
                             where n.ChatFAQResponseGroupName.Trim().Equals(groupName)
                             select n.ChatFAQResponseGroupName;
                count = result.Count();
            }
            if (count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        //on form submit - check user input against existing items
        //if editing, id is passed to ignore current item
        public bool IsAvailableChatMessageFAQName(string groupName, int? groupId)
        {
            int count = 0;

            if (groupId.HasValue)
            {
                var result = from n in db.ChatMessageFAQs
                             where n.ChatMessageFAQName.Trim().Equals(groupName) && n.ChatMessageFAQId != groupId
                             select n.ChatMessageFAQName;
                count = result.Count();
            }
            else
            {
                var result = from n in db.ChatMessageFAQs
                             where n.ChatMessageFAQName.Trim().Equals(groupName)
                             select n.ChatMessageFAQName;
                count = result.Count();
            }
            if (count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //on form submit - check user input against existing items
        //if editing, id is passed to ignore current item
        public bool IsAvailableClientDetailName(string groupName, int? groupId)
		{
			ClientDetailDC dbClientDetail = new ClientDetailDC(Settings.getConnectionString());
			int count = 0;

			if (groupId.HasValue)
			{
				var result = from n in dbClientDetail.ClientDetails
							 where n.ClientDetailName.Trim().Equals(groupName) && n.ClientDetailId != groupId
							 select n.ClientDetailName;
				count = result.Count();
			}
			else
			{
				var result = from n in dbClientDetail.ClientDetails
							 where n.ClientDetailName.Trim().Equals(groupName)
							 select n.ClientDetailName;
				count = result.Count();
			}
			if (count == 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		//on form submit - check user input against existing items
		//if editing, id is passed to ignore current item
		public bool IsAvailableClientDefinedReferenceItemValue(string name, string id, string currentId)
		{
			int count = 0;

			if (!string.IsNullOrEmpty(currentId))
			{
				var result = from n in db.ClientDefinedReferenceItemValues
							 where n.Value.Trim().Equals(name) && n.ClientDefinedReferenceItemId == id && n.ClientDefinedReferenceItemValueId != currentId
							 select n.Value;
				count = result.Count();
			}
			else
			{
				var result = from n in db.ClientDefinedReferenceItemValues
							 where n.Value.Trim().Equals(name) && n.ClientDefinedReferenceItemId == id
							 select n.Value;
				count = result.Count();
			}
			if (count == 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		//on form submit - check user input against existing items
		//if editing, id is passed to ignore current item
		public bool IsAvailableClientDefinedRuleGroupName(string groupName, string hierarchyType, string hierarchyItem, int? groupId)
		{
			ClientDefinedRuleGroupRepository clientDefinedRuleGroupRepository = new ClientDefinedRuleGroupRepository();

			int count = 0;

			List<ClientDefinedRuleGroup> clientDefinedRuleGroups = new List<ClientDefinedRuleGroup>();

			if (groupId.HasValue && groupId != 0)
			{
				clientDefinedRuleGroups = clientDefinedRuleDC.ClientDefinedRuleGroups.Where(c => c.ClientDefinedRuleGroupName.Trim().Equals(groupName) && c.ClientDefinedRuleGroupId != groupId).ToList();
				count = clientDefinedRuleGroups.Count();
			}
			else
			{
				clientDefinedRuleGroups = clientDefinedRuleDC.ClientDefinedRuleGroups.Where(c => c.ClientDefinedRuleGroupName.Trim().Equals(groupName)).ToList();
				count = clientDefinedRuleGroups.Count();
			}

			//Set up the hierarchies
			foreach (ClientDefinedRuleGroup clientDefinedRuleGroup in clientDefinedRuleGroups)
			{
				clientDefinedRuleGroupRepository.EditGroupForDisplay(clientDefinedRuleGroup);
			}

			//Check if any items have same hierarchy
			if (count > 0)
			{
				foreach (ClientDefinedRuleGroup clientDefinedRuleGroup in clientDefinedRuleGroups)
				{
					if (clientDefinedRuleGroup.HierarchyType == hierarchyType && clientDefinedRuleGroup.HierarchyItem == hierarchyItem)
					{
						return false;
					}
					else
					{
						count--;
					}
				}
			}

			if (count == 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		//on form submit - check user input against existing items
		//if editing, id is passed to ignore current item
		public bool IsAvailableClientDefinedBusinessRuleGroupName(string groupName, string category, int? groupId)
		{
			int count = 0;

			List<ClientDefinedRuleGroup> clientDefinedRuleGroups = new List<ClientDefinedRuleGroup>();

			if (groupId.HasValue && groupId != 0)
			{
				clientDefinedRuleGroups = clientDefinedRuleDC.ClientDefinedRuleGroups.Where(c => c.ClientDefinedRuleGroupName.Trim().Equals(groupName) && c.ClientDefinedRuleGroupId != groupId).ToList();
				count = clientDefinedRuleGroups.Count();
			}
			else
			{
				clientDefinedRuleGroups = clientDefinedRuleDC.ClientDefinedRuleGroups.Where(c => c.ClientDefinedRuleGroupName.Trim().Equals(groupName)).ToList();
				count = clientDefinedRuleGroups.Count();
			}

			//Check if any items have same category
			if (count > 0)
			{
				foreach (ClientDefinedRuleGroup clientDefinedRuleGroup in clientDefinedRuleGroups)
				{
					if (clientDefinedRuleGroup.Category == category)
					{
						return false;
					}
					else
					{
						count--;
					}
				}
			}

			if (count == 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		//on form submit - check user input against existing items
		//if editing, id is passed to ignore current item
		public bool IsAvailableClientFeeGroupName(string groupName, int? groupId)
		{
			int count = 0;

			if (groupId.HasValue)
			{
				var result = from n in db.ClientFeeGroups
							 where n.ClientFeeGroupName.Trim().Equals(groupName) && n.ClientFeeGroupId != groupId
							 select n.ClientFeeGroupName;
				count = result.Count();
			}
			else
			{
				var result = from n in db.ClientFeeGroups
							 where n.ClientFeeGroupName.Trim().Equals(groupName)
							 select n.ClientFeeGroupName;
				count = result.Count();
			}
			if (count == 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

        //on form submit - check user input against existing items
        //if editing, id is passed to ignore current item
        public bool IsAvailableClientProfileAdminGroupName(string groupName, int? groupId)
        {
            int count = 0;

            if (groupId.HasValue)
            {
                var result = from n in db.ClientProfileAdminGroups
                             where n.ClientProfileGroupName.Trim().Equals(groupName) && n.ClientProfileAdminGroupId != groupId
                             select n.ClientProfileGroupName;
                count = result.Count();
            }
            else
            {
                var result = from n in db.ClientProfileAdminGroups
                             where n.ClientProfileGroupName.Trim().Equals(groupName)
                             select n.ClientProfileGroupName;
                count = result.Count();
            }

            return (count == 0);
        }

		//on form submit - check user input against existing items
		//if editing, id is passed to ignore current item
		public bool IsAvailableClientProfileName(string groupName, int? groupId)
		{
			int count = 0;

			if (groupId.HasValue)
			{
				var result = from n in db.ClientProfileGroups
							 where n.UniqueName.Trim().Equals(groupName) && n.ClientProfileGroupId != groupId
							 select n.UniqueName;
				count = result.Count();
			}
			else
			{
				var result = from n in db.ClientProfileGroups
							 where n.UniqueName.Trim().Equals(groupName)
							 select n.UniqueName;
				count = result.Count();
			}
			if (count == 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		//on form submit - check user input against existing items
		//if editing, id is passed to ignore current item
		public bool IsAvailableCommissionableRouteGroupName(string groupName, int? groupId)
		{
			int count = 0;

			if (groupId.HasValue)
			{
				var result = from n in db.CommissionableRouteGroups
							 where n.CommissionableRouteGroupName.Trim().Equals(groupName) && n.CommissionableRouteGroupId != groupId
							 select n.CommissionableRouteGroupName;
				count = result.Count();
			}
			else
			{
				var result = from n in db.CommissionableRouteGroups
							 where n.CommissionableRouteGroupName.Trim().Equals(groupName)
							 select n.CommissionableRouteGroupName;
				count = result.Count();
			}
			if (count == 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		// Check if user has selected a valid PCC / GDS combination
		public bool IsValidPccGDS(string pcc, string gds)
		{
			int count = 0;

			var result = from n in db.ValidPseudoCityOrOfficeIds
							 where n.GDSCode.Trim().Equals(gds) && n.PseudoCityOrOfficeId.Trim().Equals(pcc)
							 select n.GDSCode;
				count = result.Count();

			return (count != 0);
		}
        
        //on form submit - check user input against existing items
        //if editing, id is passed to ignore current item
        public bool IsAvailableLocalOperatingHoursGroupName(string groupName, int? groupId)
        {
            int count = 0;

            if (groupId.HasValue)
            {
                var result = from n in db.LocalOperatingHoursGroups
                             where n.LocalOperatingHoursGroupName.Trim().Equals(groupName) && n.LocalOperatingHoursGroupId != groupId
                             select n.LocalOperatingHoursGroupName;
                count = result.Count();
            }
            else
            {
                var result = from n in db.LocalOperatingHoursGroups
                             where n.LocalOperatingHoursGroupName.Trim().Equals(groupName)
                             select n.LocalOperatingHoursGroupName;
                count = result.Count();
            }
            if (count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

		//on form submit - check user input against existing items
		//if editing, id is passed to ignore current item
		public bool IsAvailableExternalName(string groupName, int? groupId)
		{
			int count = 0;

			if (groupId.HasValue)
			{
				var result = from n in db.ExternalNames
							 where n.ExternalName1.Trim().Equals(groupName) && n.ExternalNameId != groupId
							 select n.ExternalName1;
				count = result.Count();
			}
			else
			{
				var result = from n in db.ExternalNames
							 where n.ExternalName1.Trim().Equals(groupName)
							 select n.ExternalName1;
				count = result.Count();
			}
			if (count == 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		
		//on form submit - check user input against existing items
		//if editing, id is passed to ignore current item
		public bool IsAvailableFareRedistributionName(string groupName, int? groupId, string gdsCode)
		{
			int count = 0;

			if (groupId.HasValue)
			{
				var result = from n in db.FareRedistributions
							 where n.FareRedistributionName.Trim().Equals(groupName) && n.GDSCode.Trim().Equals(gdsCode) && n.FareRedistributionId != groupId
							 select n.FareRedistributionName;
				count = result.Count();
			}
			else
			{
				var result = from n in db.FareRedistributions
							 where n.FareRedistributionName.Trim().Equals(groupName) && n.GDSCode.Trim().Equals(gdsCode)
							 select n.FareRedistributionName;
				count = result.Count();
			}
			if (count == 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		
		//if editing, id is passed to ignore current item
		public bool IsAvailableFormOfPaymentAdviceMessageGroupName(string groupName, int? groupId)
		{
			int count = 0;

			if (groupId.HasValue)
			{
				var result = from n in formOfPaymentAdviceMessageGroupDC.FormOfPaymentAdviceMessageGroups
							 where n.FormOfPaymentAdviceMessageGroupName.Trim().Equals(groupName) && n.FormOfPaymentAdviceMessageGroupID != groupId
							 select n.FormOfPaymentAdviceMessageGroupName;
				count = result.Count();
			}
			else
			{
				var result = from n in formOfPaymentAdviceMessageGroupDC.FormOfPaymentAdviceMessageGroups
							 where n.FormOfPaymentAdviceMessageGroupName.Trim().Equals(groupName)
							 select n.FormOfPaymentAdviceMessageGroupName;
				count = result.Count();
			}
			if (count == 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		//on form submit - check user input against existing items
		//if editing, id is passed to ignore current item
		public bool IsAvailableGDSAccessTypeName(string groupName, int? groupId, string gdsCode)
		{
			int count = 0;

			if (groupId.HasValue)
			{
				var result = from n in db.GDSAccessTypes
							 where n.GDSAccessTypeName.Trim().Equals(groupName) && n.GDSAccessTypeId != groupId && n.GDSCode.Equals(gdsCode)
							 select n.GDSAccessTypeName;
				count = result.Count();
			}
			else
			{
				var result = from n in db.GDSAccessTypes
							 where n.GDSAccessTypeName.Trim().Equals(groupName) && n.GDSCode.Equals(gdsCode)
							 select n.GDSAccessTypeName;
				count = result.Count();
			}
			if (count == 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		//on form submit - check user input against existing items
		//if editing, id is passed to ignore current item
		public bool IsAvailableGDSOrderTypeName(string groupName, int? groupId)
		{
			int count = 0;

			if (groupId.HasValue)
			{
				var result = from n in db.GDSOrderTypes
							 where n.GDSOrderTypeName.Trim().Equals(groupName) && n.GDSOrderTypeId != groupId
							 select n.GDSOrderTypeName;
				count = result.Count();
			}
			else
			{
				var result = from n in db.GDSOrderTypes
							 where n.GDSOrderTypeName.Trim().Equals(groupName)
							 select n.GDSOrderTypeName;
				count = result.Count();
			}
			if (count == 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		
		//on form submit - check user input against existing items
		//if editing, id is passed to ignore current item
		public bool IsAvailableGDSOrderDetailName(string groupName, int? groupId)
		{
			int count = 0;

			if (groupId.HasValue)
			{
				var result = from n in db.GDSOrderDetails
							 where n.GDSOrderDetailName.Trim().Equals(groupName) && n.GDSOrderDetailId != groupId
							 select n.GDSOrderDetailName;
				count = result.Count();
			}
			else
			{
				var result = from n in db.GDSOrderDetails
							 where n.GDSOrderDetailName.Trim().Equals(groupName)
							 select n.GDSOrderDetailName;
				count = result.Count();
			}
			if (count == 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		//on form submit - check user input against existing items
		//if editing, id is passed to ignore current item
		public bool IsAvailableGDSRequestTypeName(string groupName, int? groupId)
		{
			int count = 0;

			if (groupId.HasValue)
			{
				var result = from n in db.GDSRequestTypes
							 where n.GDSRequestTypeName.Trim().Equals(groupName) && n.GDSRequestTypeId != groupId
							 select n.GDSRequestTypeName;
				count = result.Count();
			}
			else
			{
				var result = from n in db.GDSRequestTypes
							 where n.GDSRequestTypeName.Trim().Equals(groupName)
							 select n.GDSRequestTypeName;
				count = result.Count();
			}
			if (count == 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		//on form submit - check user input against existing items
        //if editing, id is passed to ignore current item
        public bool IsAvailableMeetingName(string groupName, int? groupId)
        {
            int count = 0;

            if (groupId.HasValue)
            {
				var result = from n in meetingDC.Meetings
                             where n.MeetingName.Trim().Equals(groupName) && n.MeetingID != groupId
							 select n.MeetingName;
                count = result.Count();
            }
            else
            {
				var result = from n in meetingDC.Meetings
							 where n.MeetingName.Trim().Equals(groupName)
							 select n.MeetingName;
                count = result.Count();
            }
            if (count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //on form submit - check user input against existing items
        //if editing, id is passed to ignore current item
        public bool IsAvailableOptionalFieldGroupName(string groupName, int? groupId)
        {
            int count = 0;

            if (groupId.HasValue)
            {
                var result = from n in db.OptionalFieldGroups
                             where n.OptionalFieldGroupName.Trim().Equals(groupName) && n.OptionalFieldGroupId != groupId
                             select n.OptionalFieldGroupName;
                count = result.Count();
            }
            else
            {
                var result = from n in db.OptionalFieldGroups
                             where n.OptionalFieldGroupName.Trim().Equals(groupName)
                             select n.OptionalFieldGroupName;
                count = result.Count();
            }
            if (count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

		//on form submit - check user input against existing items
		//if editing, id is passed to ignore current item
		public bool IsAvailablePartner(string groupName, string countryCode, int? groupId)
		{
			int count = 0;

			if (groupId.HasValue)
			{
				var result = from n in db.Partners
							 where n.PartnerName.Trim().Equals(groupName) && n.CountryCode == countryCode && n.PartnerId != groupId
							 select n.PartnerName;
				count = result.Count();
			}
			else
			{
				var result = from n in db.Partners
							 where n.PartnerName.Trim().Equals(groupName) && n.CountryCode == countryCode
							 select n.PartnerName;
				count = result.Count();
			}
			if (count == 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

        //on form submit - check user input against existing items
        //if editing, id is passed to ignore current item
        public bool IsAvailablePNROutputGroupName(string groupName, int? groupId)
        {
            int count = 0;

            if (groupId.HasValue)
            {
                var result = from n in db.PNROutputGroups
                             where n.PNROutputGroupName.Trim().Equals(groupName) && n.PNROutputGroupId != groupId
                             select n.PNROutputGroupName;
                count = result.Count();
            }
            else
            {
                var result = from n in db.PNROutputGroups
                             where n.PNROutputGroupName.Trim().Equals(groupName)
                             select n.PNROutputGroupName;
                count = result.Count();
            }
            if (count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        //on form submit - check user input against existing items
        //if editing, id is passed to ignore current item
        public bool IsAvailablePointOfSaleFeeLoad(int? groupId, string clientTopUnitGuid, string clientSubUnitGuid, string travelerTypeGuid, string feeLoadDescriptionTypeCode, int productId, bool agentInitiatedFlag, string travelIndicator)
        {
            int count = 0;

            if (groupId.HasValue)
            {
                var result = from n in pointOfSaleFeeLoadDataContext.PointOfSaleFeeLoads
                             where
                                n.PointOfSaleFeeLoadId != groupId &&
								(!string.IsNullOrEmpty(clientTopUnitGuid) ? n.ClientTopUnitGuid.Trim().Equals(clientTopUnitGuid) : n.ClientTopUnitGuid == null) &&
								(!string.IsNullOrEmpty(clientSubUnitGuid) ? n.ClientSubUnitGuid.Trim().Equals(clientSubUnitGuid) : n.ClientSubUnitGuid == null) &&
								(!string.IsNullOrEmpty(travelerTypeGuid) ? n.TravelerTypeGuid.Trim().Equals(travelerTypeGuid) : n.TravelerTypeGuid == null) &&
								(!string.IsNullOrEmpty(feeLoadDescriptionTypeCode) ? n.FeeLoadDescriptionTypeCode.Trim().Equals(feeLoadDescriptionTypeCode) : n.FeeLoadDescriptionTypeCode == null) &&
                                n.ProductId.Equals(productId) &&
                                n.AgentInitiatedFlag.Equals(agentInitiatedFlag) &&
								(!string.IsNullOrEmpty(travelIndicator) ? n.TravelIndicator.Trim().Equals(travelIndicator) : n.TravelIndicator == null)
                             select n.ClientTopUnitGuid;
                count = result.Count();
            }
            else
            {
                var result = from n in pointOfSaleFeeLoadDataContext.PointOfSaleFeeLoads
                             where
                                (!string.IsNullOrEmpty(clientTopUnitGuid) ? n.ClientTopUnitGuid.Trim().Equals(clientTopUnitGuid) : n.ClientTopUnitGuid == null) &&
                                (!string.IsNullOrEmpty(clientSubUnitGuid) ? n.ClientSubUnitGuid.Trim().Equals(clientSubUnitGuid) : n.ClientSubUnitGuid == null) &&
                                (!string.IsNullOrEmpty(travelerTypeGuid) ? n.TravelerTypeGuid.Trim().Equals(travelerTypeGuid) : n.TravelerTypeGuid == null) &&
								(!string.IsNullOrEmpty(feeLoadDescriptionTypeCode) ? n.FeeLoadDescriptionTypeCode.Trim().Equals(feeLoadDescriptionTypeCode) : n.FeeLoadDescriptionTypeCode == null) &&
                                n.ProductId.Equals(productId) &&
                                n.AgentInitiatedFlag.Equals(agentInitiatedFlag) &&
								(!string.IsNullOrEmpty(travelIndicator) ? n.TravelIndicator.Trim().Equals(travelIndicator) : n.TravelIndicator == null)
                             select n.ClientTopUnitGuid;
                count = result.Count();
            }
            if (count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //on form submit - check user input against existing items
        //if editing, id is passed to ignore current item
        public bool IsAvailableProductGroupName(string groupName, int? groupId)
        {
            int count = 0;

            if (groupId.HasValue)
            {
                var result = from n in db.ProductGroups
                             where n.ProductGroupName.Trim().Equals(groupName) && n.ProductGroupId != groupId
                             select n.ProductGroupName;
                count = result.Count();
            }
            else
            {
                var result = from n in db.ProductGroups
                             where n.ProductGroupName.Trim().Equals(groupName)
                             select n.ProductGroupName;
                count = result.Count();
            }
            if (count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

		//on form submit - check user input against existing items
		//if editing, id is passed to ignore current item
		public bool IsAvailablePolicyGroupName(string groupName, int? groupId)
		{
			PolicyGroupDC dbPolicyGroup = new PolicyGroupDC(Settings.getConnectionString());
			int count = 0;

			if (groupId.HasValue)
			{
				var result = from n in dbPolicyGroup.PolicyGroups
							 where n.PolicyGroupName.Trim().Equals(groupName) && n.PolicyGroupId != groupId
							 select n.PolicyGroupName;
				count = result.Count();
			}
			else
			{
				var result = from n in dbPolicyGroup.PolicyGroups
							 where n.PolicyGroupName.Trim().Equals(groupName)
							 select n.PolicyGroupName;
				count = result.Count();
			}
			if (count == 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		//on form submit - check user input against existing items
		//if editing, id is passed to ignore current item
		public bool IsAvailablePriceTrackingHandlingFeeGroupName(string groupName, int? groupId)
		{
			int count = 0;

			if (groupId.HasValue)
			{
				var result = from n in priceTrackingDC.PriceTrackingHandlingFeeGroups
							 where n.PriceTrackingHandlingFeeGroupName.Trim().Equals(groupName) && n.PriceTrackingHandlingFeeGroupId != groupId
							 select n.PriceTrackingHandlingFeeGroupName;
				count = result.Count();
			}
			else
			{
				var result = from n in priceTrackingDC.PriceTrackingHandlingFeeGroups
							 where n.PriceTrackingHandlingFeeGroupName.Trim().Equals(groupName)
							 select n.PriceTrackingHandlingFeeGroupName;
				count = result.Count();
			}
			if (count == 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

        //on form submit - check user input against existing items
        // values on the group for GDS, PCC/OID, Hierarchy Type and Hierachy Item
        //if editing, id is passed to ignore current item
        public bool IsAvailablePriceTrackingSetupGroup(string gdsCode, string pseudoCityOrOfficeId, string hierarchyType, string hierarchyCode,  int? groupId)
        {
            int count = 0;

            if (groupId.HasValue && hierarchyType == "ClientTopUnit")
             {
                var result = from n in priceTrackingDC.PriceTrackingSetupGroups
                             join x in priceTrackingDC.PriceTrackingSetupGroupClientTopUnits on n.PriceTrackingSetupGroupId equals x.PriceTrackingSetupGroupId
                             where
                                 n.GDSCode.Trim().Equals(gdsCode) &&
                                 n.PseudoCityOrOfficeId.Trim().Equals(pseudoCityOrOfficeId) &&
                                 x.ClientTopUnitGuid.Trim().Equals(hierarchyCode) &&
                                 n.PriceTrackingSetupGroupId != groupId
                             select n.PriceTrackingSetupGroupName;

                count = result.Count();
            }
            else if (groupId.HasValue && hierarchyType == "ClientSubUnit")
            {
                var result = from n in priceTrackingDC.PriceTrackingSetupGroups
                             join x in priceTrackingDC.PriceTrackingSetupGroupClientSubUnits on n.PriceTrackingSetupGroupId equals x.PriceTrackingSetupGroupId
                             where
                                 n.GDSCode.Trim().Equals(gdsCode) &&
                                 n.PseudoCityOrOfficeId.Trim().Equals(pseudoCityOrOfficeId) &&
                                 x.ClientSubUnitGuid.Trim().Equals(hierarchyCode) &&
                                 n.PriceTrackingSetupGroupId != groupId
                             select n.PriceTrackingSetupGroupName;

                count = result.Count();
            }
            else if (!groupId.HasValue && hierarchyType == "ClientTopUnit")
            {
                var result = from n in priceTrackingDC.PriceTrackingSetupGroups
                             join x in priceTrackingDC.PriceTrackingSetupGroupClientTopUnits on n.PriceTrackingSetupGroupId equals x.PriceTrackingSetupGroupId
                             where
                                 n.GDSCode.Trim().Equals(gdsCode) &&
                                 n.PseudoCityOrOfficeId.Trim().Equals(pseudoCityOrOfficeId) &&
                                 x.ClientTopUnitGuid.Trim().Equals(hierarchyCode)
                             select n.PriceTrackingSetupGroupName;

                count = result.Count();
            }
            else if (!groupId.HasValue && hierarchyType == "ClientSubUnit")
            {
                var result = from n in priceTrackingDC.PriceTrackingSetupGroups
                             join x in priceTrackingDC.PriceTrackingSetupGroupClientSubUnits on n.PriceTrackingSetupGroupId equals x.PriceTrackingSetupGroupId
                             where
                                 n.GDSCode.Trim().Equals(gdsCode) &&
                                 n.PseudoCityOrOfficeId.Trim().Equals(pseudoCityOrOfficeId) &&
                                 x.ClientSubUnitGuid.Trim().Equals(hierarchyCode)
                             select n.PriceTrackingSetupGroupName;

                count = result.Count();
            }
            
            if (count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //on form submit - check user input against existing items
        //if editing, id is passed to ignore current item
        public bool IsAvailablePublicHolidayGroupName(string groupName, int? groupId)
        {
            PublicHolidayGroupDC dbPublicHolidayGroup = new PublicHolidayGroupDC(Settings.getConnectionString());
            int count = 0;

            if (groupId.HasValue)
            {
                var result = from n in dbPublicHolidayGroup.PublicHolidayGroups
                             where n.PublicHolidayGroupName.Trim().Equals(groupName) && n.PublicHolidayGroupId != groupId
                             select n.PublicHolidayGroupName;
                count = result.Count();
            }
            else
            {
                var result = from n in dbPublicHolidayGroup.PublicHolidayGroups
                             where n.PublicHolidayGroupName.Trim().Equals(groupName)
                             select n.PublicHolidayGroupName;
                count = result.Count();
            }
            if (count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

		//on form submit - check user input against existing items
		//if editing, id is passed to ignore current item
		public bool IsAvailablePseudoCityOrOfficeAddressName(string cityName, int? groupId, string firstLineAddress)
		{
			int count = 0;

			if (groupId.HasValue)
			{
				var result = from n in db.PseudoCityOrOfficeAddresses
							 where n.CityName.Trim().Equals(cityName) && n.FirstAddressLine.Equals(firstLineAddress) && n.PseudoCityOrOfficeAddressId != groupId
							 select n.FirstAddressLine;
				count = result.Count();
			}
			else
			{
				var result = from n in db.PseudoCityOrOfficeAddresses
							 where n.CityName.Trim().Equals(cityName) && n.FirstAddressLine.Equals(firstLineAddress)
							 select n.FirstAddressLine;
				count = result.Count();
			}
			if (count == 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		
		//on form submit - check user input against existing items
		//if editing, id is passed to ignore current item
		public bool IsAvailablePseudoCityOrOfficeTypeName(string groupName, int? groupId)
		{
			int count = 0;

			if (groupId.HasValue)
			{
				var result = from n in db.PseudoCityOrOfficeTypes
							 where n.PseudoCityOrOfficeTypeName.Trim().Equals(groupName) && n.PseudoCityOrOfficeTypeId != groupId
							 select n.PseudoCityOrOfficeTypeName;
				count = result.Count();
			}
			else
			{
				var result = from n in db.PseudoCityOrOfficeTypes
							 where n.PseudoCityOrOfficeTypeName.Trim().Equals(groupName)
							 select n.PseudoCityOrOfficeTypeName;
				count = result.Count();
			}
			if (count == 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		//on form submit - check user input against existing items
		//if editing, id is passed to ignore current item
		public bool IsAvailablePseudoCityOrOfficeDefinedRegionName(string groupName, int? groupId, string globalRegionCode)
		{
			int count = 0;

			if (groupId.HasValue)
			{
				var result = from n in db.PseudoCityOrOfficeDefinedRegions
							 where n.PseudoCityOrOfficeDefinedRegionName.Trim().Equals(groupName) && n.GlobalRegionCode.Equals(globalRegionCode) && n.PseudoCityOrOfficeDefinedRegionId != groupId
							 select n.PseudoCityOrOfficeDefinedRegionName;
				count = result.Count();
			}
			else
			{
				var result = from n in db.PseudoCityOrOfficeDefinedRegions
							 where n.PseudoCityOrOfficeDefinedRegionName.Trim().Equals(groupName) && n.GlobalRegionCode.Equals(globalRegionCode)
							 select n.PseudoCityOrOfficeDefinedRegionName;
				count = result.Count();
			}
			if (count == 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		//on form submit - check user input against existing items
		//if editing, id is passed to ignore current item
		public bool IsAvailablePseudoCityOrOfficeLocationTypeName(string groupName, int? groupId)
		{
			int count = 0;

			if (groupId.HasValue)
			{
				var result = from n in db.PseudoCityOrOfficeLocationTypes
							 where n.PseudoCityOrOfficeLocationTypeName.Trim().Equals(groupName) && n.PseudoCityOrOfficeLocationTypeId != groupId
							 select n.PseudoCityOrOfficeLocationTypeName;
				count = result.Count();
			}
			else
			{
				var result = from n in db.PseudoCityOrOfficeLocationTypes
							 where n.PseudoCityOrOfficeLocationTypeName.Trim().Equals(groupName)
							 select n.PseudoCityOrOfficeLocationTypeName;
				count = result.Count();
			}
			if (count == 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

        //on form submit - check user input against existing items
        //if editing, id is passed to ignore current item
        public bool IsAvailableQueueMinderGroupName(string groupName, int? groupId)
        {
            int count = 0;

            if (groupId.HasValue)
            {
                var result = from n in db.QueueMinderGroups
                             where n.QueueMinderGroupName.Trim().Equals(groupName) && n.QueueMinderGroupId != groupId
                             select n.QueueMinderGroupName;
                count = result.Count();
            }
            else
            {
                var result = from n in db.QueueMinderGroups
                             where n.QueueMinderGroupName.Trim().Equals(groupName)
                             select n.QueueMinderGroupName;
                count = result.Count();
            }
            if (count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //on form submit - check user input against existing items
        //if editing, id is passed to ignore current item
        public bool IsAvailableReasonCodeGroupName(string groupName, int? groupId)
        {
            ReasonCodeGroupDC dbReasonCodeGroup = new ReasonCodeGroupDC(Settings.getConnectionString());
            int count = 0;

            if (groupId.HasValue)
            {
                var result = from n in dbReasonCodeGroup.ReasonCodeGroups
                             where n.ReasonCodeGroupName.Trim().Equals(groupName) && n.ReasonCodeGroupId != groupId
                             select n.ReasonCodeGroupName;
                count = result.Count();
            }
            else
            {
                var result = from n in dbReasonCodeGroup.ReasonCodeGroups
                             where n.ReasonCodeGroupName.Trim().Equals(groupName)
                             select n.ReasonCodeGroupName;
                count = result.Count();
            }
            if (count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //on form submit - check user input against existing items
        //if editing, id is passed to ignore current item
        public bool IsAvailableServicingOptionGroupName(string groupName, int? groupId)
        {
            ServicingOptionGroupDC dbServicingOptionGroup = new ServicingOptionGroupDC(Settings.getConnectionString());
            int count = 0;

            if (groupId.HasValue)
            {
                var result = from n in dbServicingOptionGroup.ServicingOptionGroups
                             where n.ServicingOptionGroupName.Trim().Equals(groupName) && n.ServicingOptionGroupId != groupId
                             select n.ServicingOptionGroupName;
                count = result.Count();
            }
            else
            {
                var result = from n in dbServicingOptionGroup.ServicingOptionGroups
                             where n.ServicingOptionGroupName.Trim().Equals(groupName)
                             select n.ServicingOptionGroupName;
                count = result.Count();
            }
            if (count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

		//on form submit - check user input against existing items
		//if editing, id is passed to ignore current item
		public bool IsAvailableThirdPartyUserName(string groupName, int? groupId)
		{
			int count = 0;

			if (groupId.HasValue)
			{
				var result = from n in db.ThirdPartyUsers
							 where n.ThirdPartyName.Trim().Equals(groupName) && n.ThirdPartyUserId != groupId
							 select n.ThirdPartyName;
				count = result.Count();
			}
			else
			{
				var result = from n in db.ThirdPartyUsers
							 where n.ThirdPartyName.Trim().Equals(groupName)
							 select n.ThirdPartyName;
				count = result.Count();
			}
			if (count == 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

        //on form submit - check user input against existing items
        //if editing, id is passed to ignore current item
        public bool IsAvailableServiceAccountName(string groupName, string groupId)
        {
            int count = 0;

            if (!string.IsNullOrEmpty(groupId))
            {
                var result = from n in db.ServiceAccounts
                             where n.ServiceAccountName.Trim().Equals(groupName) && n.ServiceAccountId != groupId
                             select n.ServiceAccountName;
                count = result.Count();
            }
            else
            {
                var result = from n in db.ServiceAccounts
                             where n.ServiceAccountName.Trim().Equals(groupName)
                             select n.ServiceAccountName;
                count = result.Count();
            }
            if (count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //on form submit - check user input against existing items
        //if editing, id is passed to ignore current item
		public bool IsAvailableServiceAccountGDSAccessRightGDSSignOnID(string groupName, int groupId, string gdsSignOnID, string pseudoCityOrOfficeId)
        {
            int count = 0;

			if (groupId > 0 && groupName == "ServiceAccount")
            {
                var result = from n in db.ServiceAccountGDSAccessRights
							 where n.GDSSignOnID.Trim().Equals(gdsSignOnID) && n.PseudoCityOrOfficeId.Trim().Equals(pseudoCityOrOfficeId) && n.ServiceAccountGDSAccessRightId != groupId
                             select n.GDSSignOnID;
                count = result.Count();
            }
            else
            {
                var result = from n in db.ServiceAccountGDSAccessRights
							 where n.GDSSignOnID.Trim().Equals(gdsSignOnID) && n.PseudoCityOrOfficeId.Trim().Equals(pseudoCityOrOfficeId)
                             select n.GDSSignOnID;
                count = result.Count();
            }
            if (count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //on form submit - check user input against existing items
        //if editing, id is passed to ignore current item
        public bool IsAvailableSystemUserGDSAccessRightGDSSignOnID(string groupName, int groupId, string gdsSignOnID, string pseudoCityOrOfficeId)
        {
            int count = 0;

            if (groupId > 0 && groupName == "SystemUser")
            {
                var result = from n in db.SystemUserGDSAccessRights
                             where n.GDSSignOnID.Trim().Equals(gdsSignOnID) && n.PseudoCityOrOfficeId.Trim().Equals(pseudoCityOrOfficeId) && n.SystemUserGDSAccessRightId != groupId
                             select n.GDSSignOnID;
                count = result.Count();
            }
            else
            {
                var result = from n in db.SystemUserGDSAccessRights
                             where n.GDSSignOnID.Trim().Equals(gdsSignOnID) && n.PseudoCityOrOfficeId.Trim().Equals(pseudoCityOrOfficeId)
                             select n.GDSSignOnID;
                count = result.Count();
            }
            if (count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //on form submit - check user input against existing items
        //if editing, id is passed to ignore current item
        public bool IsAvailableTeamOutOfOfficeGroupName(string groupName, int? groupId)
        {
            int count = 0;

            if (groupId.HasValue)
            {
                var result = from n in db.TeamOutOfOfficeGroups
                             where n.TeamOutOfOfficeGroupName.Trim().Equals(groupName) && n.TeamOutOfOfficeGroupId != groupId
                             select n.TeamOutOfOfficeGroupName;
                count = result.Count();
            }
            else
            {
                var result = from n in db.TeamOutOfOfficeGroups
                             where n.TeamOutOfOfficeGroupName.Trim().Equals(groupName)
                             select n.TeamOutOfOfficeGroupName;
                count = result.Count();
            }
            if (count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        //on form submit - check user input against existing items
        //if editing, id is passed to ignore current item
        public bool IsAvailableThirdPartyUserGDSAccessRightGDSSignOnID(string groupName, int groupId, string gdsSignOnID, string pseudoCityOrOfficeId)
        {
            int count = 0;

            if (groupId > 0 && groupName == "ThirdPartyUser")
            {
                var result = from n in db.ThirdPartyUserGDSAccessRights
                             where n.GDSSignOnID.Trim().Equals(gdsSignOnID) && n.PseudoCityOrOfficeId.Trim().Equals(pseudoCityOrOfficeId) && n.ThirdPartyUserGDSAccessRightId != groupId
                             select n.GDSSignOnID;
                count = result.Count();
            }
            else
            {
                var result = from n in db.ThirdPartyUserGDSAccessRights
                             where n.GDSSignOnID.Trim().Equals(gdsSignOnID) && n.PseudoCityOrOfficeId.Trim().Equals(pseudoCityOrOfficeId)
                             select n.GDSSignOnID;
                count = result.Count();
            }
            if (count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //on form submit - check user input against existing items
        //if editing, id is passed to ignore current item
        public bool IsAvailableGDSAccessRightGDSSignOnID(string groupName, int groupId, string gdsSignOnID, string pseudoCityOrOfficeId)
        {
            bool availableServiceAccountGDSAccessRightGDSSignOnID = IsAvailableServiceAccountGDSAccessRightGDSSignOnID(groupName, groupId, gdsSignOnID, pseudoCityOrOfficeId);
            bool availableSystemUserGDSAccessRight = IsAvailableSystemUserGDSAccessRightGDSSignOnID(groupName, groupId, gdsSignOnID, pseudoCityOrOfficeId);
            bool availableThirdPartyUserGDSAccessRight = IsAvailableThirdPartyUserGDSAccessRightGDSSignOnID(groupName, groupId, gdsSignOnID, pseudoCityOrOfficeId);

            if (availableServiceAccountGDSAccessRightGDSSignOnID && availableSystemUserGDSAccessRight && availableThirdPartyUserGDSAccessRight)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //on form submit - check user input against existing items
        //if editing, id is passed to ignore current item
        public bool IsAvailableTicketQueueGroupName(string groupName, int? groupId)
        {
            TicketQueueGroupDC dbTicketQueueGroup = new TicketQueueGroupDC(Settings.getConnectionString());
            int count = 0;

            if (groupId.HasValue)
            {
                var result = from n in dbTicketQueueGroup.TicketQueueGroups
                             where n.TicketQueueGroupName.Trim().Equals(groupName) && n.TicketQueueGroupId != groupId
                             select n.TicketQueueGroupName;
                count = result.Count();
            }
            else
            {
                var result = from n in dbTicketQueueGroup.TicketQueueGroups
                             where n.TicketQueueGroupName.Trim().Equals(groupName)
                             select n.TicketQueueGroupName;
                count = result.Count();
            }
            if (count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //on form submit - check user input against existing items
        //if editing, id is passed to ignore current item
        public bool IsAvailableTripTypeGroupName(string groupName, int? groupId)
        {
            TripTypeGroupDC dbTripTypeGroup = new TripTypeGroupDC(Settings.getConnectionString());
            int count = 0;

            if (groupId.HasValue)
            {
                var result = from n in dbTripTypeGroup.TripTypeGroups
                             where n.TripTypeGroupName.Trim().Equals(groupName) && n.TripTypeGroupId != groupId
                             select n.TripTypeGroupName;
                count = result.Count();
            }
            else
            {
                var result = from n in dbTripTypeGroup.TripTypeGroups
                             where n.TripTypeGroupName.Trim().Equals(groupName)
                             select n.TripTypeGroupName;
                count = result.Count();
            }
            if (count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //on form submit - check user input against existing items
        //if editing, id is passed to ignore current item
        public bool IsAvailableWorkFlowGroupName(string groupName, int? groupId)
        {
            int count = 0;

            if (groupId.HasValue)
            {
                var result = from n in db.WorkFlowGroups
                             where n.WorkFlowGroupName.Trim().Equals(groupName) && n.WorkFlowGroupId != groupId
                             select n.WorkFlowGroupName;
                count = result.Count();
            }
            else
            {
                var result = from n in db.WorkFlowGroups
                             where n.WorkFlowGroupName.Trim().Equals(groupName)
                             select n.WorkFlowGroupName;
                count = result.Count();
            }
            if (count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
