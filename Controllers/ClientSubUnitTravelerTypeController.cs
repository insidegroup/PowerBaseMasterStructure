using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Repository;

namespace CWTDesktopDatabase.Controllers
{
    public class ClientSubUnitTravelerTypeController : Controller
    {
        ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
        ClientSubUnitTravelerTypeRepository clientSubUnitTravelerTypeRepository = new ClientSubUnitTravelerTypeRepository();

        // GET: /ListByClientSubUnit/
        public ActionResult ListByClientSubUnit(string id, int? page)
        {

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(id);

            //Check Exists
            if (clientSubUnit == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }

			//Access Rights
			ViewData["Access"] = "";
			RolesRepository rolesRepository = new RolesRepository();
			if (rolesRepository.HasWriteAccessToClientSubUnit(id))
			{
				ViewData["Access"] = "WriteAccess";
			}

            ClientSubUnitTravelerTypesVM clientSubUnitTravelerTypesVM = new ClientSubUnitTravelerTypesVM();
            clientSubUnitTravelerTypesVM.ClientSubUnit = clientSubUnit;

            var items = clientSubUnitTravelerTypeRepository.GetClientSubUnitTravelerTypes(clientSubUnit.ClientSubUnitGuid, page ?? 1);
            clientSubUnitTravelerTypesVM.TravelerTypes = items;
            return View(clientSubUnitTravelerTypesVM);
        }

    }
}
