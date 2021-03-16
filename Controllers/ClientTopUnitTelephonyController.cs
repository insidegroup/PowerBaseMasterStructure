using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Repository;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Models;


namespace CWTDesktopDatabase.Controllers
{
    public class ClientTopUnitTelephonyController : Controller
    {
        ClientTopUnitTelephonyRepository clientTopUnitTelephonyRepository = new ClientTopUnitTelephonyRepository();
        ClientTopUnitRepository clientTopUnitRepository = new ClientTopUnitRepository();

        // GET: /List
        public ActionResult List(int? page, string id, string sortField, int? sortOrder)
        {			
			//Set Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToReferenceInfo())
            {
                ViewData["Access"] = "WriteAccess";
            }
            ViewData["CurrentSortField"] = "Name";
            ViewData["CurrentSortOrder"] = 0;

            ClientTopUnitTelephoniesVM clientTopUnitTelephoniesVM = new ClientTopUnitTelephoniesVM();
            clientTopUnitTelephoniesVM.Telephonies = clientTopUnitTelephonyRepository.PageClientTopUnitTelephonies(page ?? 1, id, sortField, sortOrder);

            ClientTopUnit clientTopUnit = new ClientTopUnit();
            clientTopUnit = clientTopUnitRepository.GetClientTopUnit(id);

			//Check clientTopUnit
			if (clientTopUnit == null)
			{
				ViewData["ActionMethod"] = "List";
				return View("RecordDoesNotExistError");
			}

            clientTopUnitTelephoniesVM.ClientTopUnit = clientTopUnit;

            //return items
            return View(clientTopUnitTelephoniesVM);
        }

    }
}
