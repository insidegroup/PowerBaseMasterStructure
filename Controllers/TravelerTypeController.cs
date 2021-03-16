using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Repository;

namespace CWTDesktopDatabase.Controllers
{
    public class TravelerTypeController : Controller
    {
        TravelerTypeRepository travelerTypeRepository = new TravelerTypeRepository();
		ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();

        // GET: /ViewItem/
		public ActionResult ViewItem(string id, string csu)
        {

            TravelerType travelerType = new TravelerType();
            travelerType = travelerTypeRepository.GetTravelerType(id);

            //Check Exists
            if (travelerType == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }
            travelerTypeRepository.EditForDisplay(travelerType);

			ClientSubUnit clientSubUnit = new ClientSubUnit();
			clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);
			travelerType.ClientSubUnit = clientSubUnit;

			ClientTopUnit clientTopUnit = clientSubUnit.ClientTopUnit;
			travelerType.ClientTopUnit = clientTopUnit;

			return View(travelerType);
        }

    }
}
