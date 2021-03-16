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
    public class ClientSubUnitController : Controller
    {
        //main repository
        ClientTopUnitRepository clientTopUnitRepository = new ClientTopUnitRepository();
        ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();

        public ActionResult List(string id, int? page, string sortField, int? sortOrder, string filter)
        {

            ClientTopUnit clientTopUnit = new ClientTopUnit();
            clientTopUnit = clientTopUnitRepository.GetClientTopUnit(id);

            //Check Exists
            if (clientTopUnit == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }


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
                sortOrder = 0;
            }

            //extra data for page titles
            
            ViewData["ClientTopUnitName"] = clientTopUnit.ClientTopUnitName;
            ViewData["ClientTopUnitGuid"] = id;

			var paginatedView = clientTopUnitRepository.GetClientSubUnits(id, page ?? 1, sortField, sortOrder ?? 0, filter ?? "");

			return View(paginatedView);
        }

        public ActionResult ViewItem(string id)
        {
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(id);

            //Check Exists
            if (clientSubUnit == null)
            {
                ViewData["ActionMethod"] = "ListBySubUnitGet";
                return View("RecordDoesNotExistError");
            }
            clientSubUnitRepository.EditGroupForDisplay(clientSubUnit);
            return View(clientSubUnit);
        }

      
    }
}
