using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Repository;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.Controllers
{
    [AjaxTimeOutCheck]
    public class HierarchyController : Controller
    {
        HierarchyRepository hierarchyRepository = new HierarchyRepository();

        #region IsValidHierarchy - checks if valid item for a hierarchy - doesn't check user rights
        // POST: Check If 
        [HttpPost]
        public JsonResult IsValidGlobal(string searchText)
        {
            searchText = System.Web.HttpUtility.UrlDecode(searchText);
            var result = hierarchyRepository.GetGlobalByName(searchText);
            return Json(result);
        }
        [HttpPost]
        public JsonResult IsValidGlobalRegion(string searchText)
        {
            searchText = System.Web.HttpUtility.UrlDecode(searchText);
            var result = hierarchyRepository.GetGlobalRegionByName(searchText);
            return Json(result);
        }
        [HttpPost]
        public JsonResult IsValidGlobalSubRegion(string searchText)
        {
            searchText = System.Web.HttpUtility.UrlDecode(searchText);
            var result = hierarchyRepository.GetGlobalSubRegionByName(searchText);
            return Json(result);
        }
        [HttpPost]
        public JsonResult IsValidCountry(string searchText)
        {
            searchText = System.Web.HttpUtility.UrlDecode(searchText);
            var result = hierarchyRepository.GetCountryByName(searchText);
            return Json(result);
        }
        [HttpPost]
        public JsonResult IsValidCountryRegion(string searchText)
        {
            searchText = System.Web.HttpUtility.UrlDecode(searchText);
            var result = hierarchyRepository.GetCountryRegionByName(searchText);
            return Json(result);
        }
        [HttpPost]
        public JsonResult IsValidTeam(string searchText)
        {
            TeamRepository teamRepository = new TeamRepository();

            searchText = System.Web.HttpUtility.UrlDecode(searchText);
            var result = teamRepository.GetTeamByName(searchText);
            return Json(result);
        }

        public JsonResult IsValidLocation(string searchText)
        {
            searchText = System.Web.HttpUtility.UrlDecode(searchText);
            var result = hierarchyRepository.GetLocationByName(searchText);
            return Json(result);
        }
        [HttpPost]
        public JsonResult IsValidClientAccount(string searchText)
        {
            searchText = System.Web.HttpUtility.UrlDecode(searchText);
            var result = hierarchyRepository.GetClientAccountByName(searchText);
            return Json(result);
        }
        [HttpPost]
        public JsonResult IsValidClientSubUnit(string searchText)
        {
            searchText = System.Web.HttpUtility.UrlDecode(searchText);
            var result = hierarchyRepository.GetClientSubUnitByName(searchText);
            return Json(result);
        }
        [HttpPost]
        public JsonResult IsValidClientSubUnitTravelerType(string searchText)
        {
            searchText = System.Web.HttpUtility.UrlDecode(searchText);
            var result = hierarchyRepository.GetClientSubUnitByName(searchText);
            return Json(result);
        }
        [HttpPost]
        public JsonResult IsValidClientTopUnit(string searchText)
        {
            searchText = System.Web.HttpUtility.UrlDecode(searchText);
            var result = hierarchyRepository.GetClientTopUnitByName(searchText);
            return Json(result);
        }
		[HttpPost]
		public JsonResult IsValidTravelerType(string searchText)
		{
			searchText = System.Web.HttpUtility.UrlDecode(searchText);
			var result = hierarchyRepository.GetTravelerTypeByName(searchText);
			return Json(result);
		}
		[HttpPost]
		public JsonResult IsValidClientTelephonyMainNumber(string hierarchyType, string hierarchyItem, int clientTelephonyId)
		{
			hierarchyItem = System.Web.HttpUtility.UrlDecode(hierarchyItem);
			var result = Json(hierarchyRepository.GetClientTelephonyClientByMainNumber(hierarchyType, hierarchyItem, clientTelephonyId));
			return result;
		}

        #endregion
    }
}
