
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Repository;
using CWTDesktopDatabase.ViewModels;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Controllers
{
    public class TravelerTypeSponsorController : Controller
    {
        ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
        ClientSubUnitTravelerTypeRepository clientSubUnitTravelerTypeRepository = new ClientSubUnitTravelerTypeRepository();
        ClientTopUnitRepository clientTopUnitRepository = new ClientTopUnitRepository();
        TravelerTypeRepository travelerTypeRepository = new TravelerTypeRepository();
        TravelerTypeSponsorRepository travelerTypeSponsorRepository = new TravelerTypeSponsorRepository();

		HierarchyRepository hierarchyRepository = new HierarchyRepository();

        public ActionResult Edit(string csu, string tt)
        {
            ClientSubUnitTravelerType clientSubUnitTravelerType = new ClientSubUnitTravelerType();
            clientSubUnitTravelerType = clientSubUnitTravelerTypeRepository.GetClientSubUnitTravelerType(csu, tt);

			//Set Access Rights
			RolesRepository rolesRepository = new RolesRepository();
			if (rolesRepository.HasWriteAccessToClientSubUnit(csu))
			{
				ViewData["Access"] = "WriteAccess";
			}
			
			//Check Exists - Although TT exists independently of CSU, Access Rights are dependent on CSU
            //User can only edit a TT if it is linked to a CSU that the user has access to
            if (clientSubUnitTravelerType == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }
            TravelerTypeVM travelerTypeVM = new TravelerTypeVM();

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(csu);
            travelerTypeVM.ClientSubUnit = clientSubUnit;

            ClientTopUnit clientTopUnit = new ClientTopUnit();
            clientTopUnit = clientTopUnitRepository.GetClientTopUnit(clientSubUnit.ClientTopUnitGuid);
            travelerTypeVM.ClientTopUnit = clientTopUnit;

            TravelerTypeSponsor travelerTypeSponsor = new TravelerTypeSponsor();
            travelerTypeSponsor = travelerTypeSponsorRepository.GetTravelerTypeSponsor(tt);

            if (travelerTypeSponsor == null)
            {
                travelerTypeSponsor = new TravelerTypeSponsor();
                travelerTypeSponsor.TravelerTypeGuid = tt;
            }
            travelerTypeVM.TravelerTypeSponsor = travelerTypeSponsor;

            TravelerType travelerType = new TravelerType();
            travelerType = travelerTypeRepository.GetTravelerType(tt);
            travelerTypeVM.TravelerType = travelerType;

            return View(travelerTypeVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TravelerTypeVM travelerTypeVM)
        {
            TravelerType travelerType = new TravelerType();
            travelerType = travelerTypeRepository.GetTravelerType(travelerTypeVM.TravelerTypeSponsor.TravelerTypeGuid);

            //Check Exists - Although TT exists independently of CSU, Access Rights are dependent on CSU
            //User can only edit a TT if it is linked to a CSU that the user has access to
            if (travelerType == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            TravelerTypeSponsor travelerTypeSponsor = new TravelerTypeSponsor();
            travelerTypeSponsor = travelerTypeVM.TravelerTypeSponsor;

            if (travelerTypeSponsor.IsSponsorFlag)
                travelerTypeSponsor.RequiresSponsorFlag = false;

            if (travelerTypeSponsor.RequiresSponsorFlag)
                travelerTypeSponsor.IsSponsorFlag = false;

            try
            {
                if (travelerTypeSponsorRepository.GetTravelerTypeSponsor(travelerTypeSponsor.TravelerTypeGuid) == null)
                {
                    travelerTypeSponsorRepository.Add(travelerTypeSponsor);
                }
                else
                {
                    travelerTypeSponsorRepository.Edit(travelerTypeSponsor);
                }
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("ListByClientSubUnit", "ClientSubUnitTravelerType", new { id = travelerTypeVM.ClientSubUnit.ClientSubUnitGuid });
        }
    }
}