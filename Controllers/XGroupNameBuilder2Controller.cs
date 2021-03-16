using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Repository;

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
namespace CWTDesktopDatabase.Controllers
{
    public class GroupNameBuilder2Controller : Controller
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
        //Build PolicyGroupName (except for ClientAccount)
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
        /*
        //on form submit - check if name already exists
        //returns boolean as Json
        [HttpPost]
        public JsonResult IsAvailableCDRGroupName(string groupName, string id)
        {
            if (groupName == "")
            {
                return Json(false);
            }
            var result = groupNameBuilderRepository.IsAvailableCDRGroupName(groupName, id);
            return Json(result);
        }
        */
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
