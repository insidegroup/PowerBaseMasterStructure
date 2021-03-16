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
    public class ClientTopUnitController : Controller
    {
        //main repository
        ClientTopUnitRepository clientTopUnitRepository = new ClientTopUnitRepository();
        
        // GET: /ClientTopUnit/
        public ActionResult List(string filter, int? page, string sortField, int? sortOrder)
        {
            if (sortField != "URLIdentifier" && sortField != "ClientTopUnitGuid")
            {
                sortField = "ClientTopUnitName";
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

            var items = clientTopUnitRepository.GetClientTopUnits(filter ?? "", page ?? 1);
            return View(items);
        }

        public ActionResult ListSubUnits(string id, int? page, string sortField, int? sortOrder, string filter)
        {
            //Sorting
            if (sortField == null || sortField == "Name")
            {
                sortField = "ClientSubUnitDisplayName";
            }
            ViewData["CurrentSortField"] = sortField;            

            //Ordering
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

            //extra data for page titles
            ClientTopUnit clientTopUnit = new ClientTopUnit();
            clientTopUnit = clientTopUnitRepository.GetClientTopUnit(id);
            ViewData["ClientTopUnitName"] = clientTopUnit.ClientTopUnitName;
            ViewData["ClientTopUnitGuid"] = id;

			var paginatedView = clientTopUnitRepository.GetClientSubUnits(id, page ?? 1, sortField, sortOrder ?? 0, filter ?? "");

			return View(paginatedView);
        }
        // GET: /View
        public ActionResult ViewItem(string id)
        {
            ClientTopUnit clientTopUnit = new ClientTopUnit();
            clientTopUnit = clientTopUnitRepository.GetClientTopUnit(id);
            //clientTopUnitRepository.EditGroupForDisplay(clientTopUnit);

			//Check clientTopUnit
			if (clientTopUnit == null)
			{
				ViewData["ActionMethod"] = "List";
				return View("RecordDoesNotExistError");
			}

            return View(clientTopUnit);
        }
    }
}
