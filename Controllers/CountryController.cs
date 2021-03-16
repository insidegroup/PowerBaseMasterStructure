using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Repository;

namespace CWTDesktopDatabase.Controllers
{
    [AjaxTimeOutCheck]
    public class CountryController : Controller
    {
        CountryRepository countryRepository = new CountryRepository();

        //GET: A list of Cities
        public ActionResult List(string filter, int? page, string sortField, int? sortOrder)
        {

            //Set Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToReferenceInfo())
           {
                ViewData["Access"] = "WriteAccess";
            }

            //SortField + SortOrder settings
            if (sortField != "CountryCode")
            {
                sortField = "CountryName";
            }
            ViewData["CurrentSortField"] = sortField;

            if (sortOrder == 1)
            {
                ViewData["NewSortOrder"] = 0;
                ViewData["CurrentSortOrder"] = 1;
            }
            else
            {
                ViewData["NewSortOrder"] = 1;
                ViewData["CurrentSortOrder"] = 0;
            }

            var items = countryRepository.PageCountries(page ?? 1, filter ?? "", sortField, sortOrder ?? 0);
             
            return View(items);
        }

        // GET: A SIngle Country
        public ActionResult ViewItem(string id)
        {
            Country country = new Country();
            country = countryRepository.GetCountry(id);

            //Check Exists
            if (country == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            //countryRepository.EditItemForDisplay(country);
            return View(country);
        }

        // POST: AutoComplete Country
        [HttpPost]
        public JsonResult AutoCompleteCountries(string searchText)
        {
            HierarchyRepository hierarchyRepository = new HierarchyRepository();

            int maxResults = 15;
            var result = hierarchyRepository.LookUpCountries(searchText, maxResults);
            return Json(result);
        }

		// POST:  CountryRegions of a Country for SelectList
		[HttpPost]
		public JsonResult GetCountryRegions(string countryCode)
		{
			CountryRepository countryRepository = new CountryRepository();

			var result = countryRepository.LookUpCountryCountryRegions(countryCode);
			return Json(result);
		}

		// POST:  CountryRegions of a Country for SelectList
		[HttpPost]
		public JsonResult GetCountryGlobalRegions(string countryCode)
		{
			CountryRepository countryRepository = new CountryRepository();

			var result = countryRepository.GetCountryGlobalRegions(countryCode);
			return Json(result);
		}
    }
}
