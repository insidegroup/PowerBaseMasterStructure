using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;

/*
 * Tnis is a new version introduced in US570, initially just for ClientFeeGroups
 * Names now use the following
 * v1                           v2
 * ClientTopUnit                CTU
 * ClientSubUnit                CSU
 * ClietnSubUnitTravellerType   CSUTT
 * Global                       Global
 * GlobalSubRegion              GSR
 * GlobalRegion                 GR
 * Country                      Country
 * CountryRegion                CR
 * Location                     Location
 * Team                         Team
 */
namespace CWTDesktopDatabase.Repository
{
    public class GroupNameBuilder2Repository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

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
            WorkFlowGroupDC dbWorkFlowGroup = new WorkFlowGroupDC(Settings.getConnectionString());
            int count = 0;

            if (groupId.HasValue)
            {
                var result = from n in dbWorkFlowGroup.WorkFlowGroups
                             where n.WorkFlowGroupName.Trim().Equals(groupName) && n.WorkFlowGroupId != groupId
                             select n.WorkFlowGroupName;
                count = result.Count();
            }
            else
            {
                var result = from n in dbWorkFlowGroup.WorkFlowGroups
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
