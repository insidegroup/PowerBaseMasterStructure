using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Repository;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Models;
using System.Data.SqlClient;


namespace CWTDesktopDatabase.Controllers
{
    public class ClientSubUnitTelephonyController : Controller
    {
        ClientSubUnitTelephonyRepository clientSubUnitTelephonyRepository = new ClientSubUnitTelephonyRepository();
        ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();

        // GET: /List
        public ActionResult List(int? page, string id, string sortField, int? sortOrder)
        {
            //Set Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToClientSubUnit(id))
            {
                ViewData["Access"] = "WriteAccess";
            }
            ViewData["CurrentSortField"] = "Name";
            ViewData["CurrentSortOrder"] = 0;

            ClientSubUnitTelephoniesVM clientSubUnitTelephoniesVM = new ClientSubUnitTelephoniesVM();
            clientSubUnitTelephoniesVM.Telephonies = clientSubUnitTelephonyRepository.PageClientSubUnitTelephonies(page ?? 1, id, sortField, sortOrder);
            
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(id);
			
			//Check clientSubUnit
			if (clientSubUnit == null)
			{
				ViewData["ActionMethod"] = "List";
				return View("RecordDoesNotExistError");
			}
			
			clientSubUnitTelephoniesVM.ClientSubUnit = clientSubUnit;

            //return items
            return View(clientSubUnitTelephoniesVM);
        }

        // GET: /Create
        public ActionResult Create(string id)
        {
            //Get ClientSubUnit
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(id);

            //Check Exists
            if (clientSubUnit == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(id))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ClientSubUnitTelephonyVM clientSubUnitTelephonyVM = new ClientSubUnitTelephonyVM();
            clientSubUnitTelephonyVM.ClientSubUnit = clientSubUnit;

            ClientSubUnitTelephony clientSubUnitTelephony = new ClientSubUnitTelephony();
            clientSubUnitTelephony.ClientSubUnitGuid = id;
            clientSubUnitTelephonyVM.ClientSubUnitTelephony = clientSubUnitTelephony;

            CallerEnteredDigitDefinitionTypeRepository callerEnteredDigitDefinitionTypeRepository = new CallerEnteredDigitDefinitionTypeRepository();
            clientSubUnitTelephonyVM.CallerEnteredDigitDefinitionTypes = new SelectList(callerEnteredDigitDefinitionTypeRepository.GetAllCallerEnteredDigitDefinitionTypes().ToList(), "CallerEnteredDigitDefinitionTypeId", "CallerEnteredDigitDefinitionTypeDescription");

            //Show Create Form
            return View(clientSubUnitTelephonyVM);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClientSubUnitTelephonyVM clientSubUnitTelephonyVM)
        {
            string clientSubUnitGuid = clientSubUnitTelephonyVM.ClientSubUnitTelephony.ClientSubUnitGuid;

            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientSubUnitGuid);

            //Check Exists
            if (clientSubUnit == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnitGuid))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ClientSubUnitTelephony clientSubUnitTelephony = new ClientSubUnitTelephony();
            clientSubUnitTelephony = clientSubUnitTelephonyRepository.GetClientSubUnitTelephony(clientSubUnitGuid, clientSubUnitTelephonyVM.ClientSubUnitTelephony.DialedNumber);
            if(clientSubUnitTelephony !=null){
                ModelState.AddModelError("ClientSubUnitTelephony.DialedNumber", "This number already exists");

                CallerEnteredDigitDefinitionTypeRepository callerEnteredDigitDefinitionTypeRepository = new CallerEnteredDigitDefinitionTypeRepository();
                clientSubUnitTelephonyVM.CallerEnteredDigitDefinitionTypes = new SelectList(callerEnteredDigitDefinitionTypeRepository.GetAllCallerEnteredDigitDefinitionTypes().ToList(), "CallerEnteredDigitDefinitionTypeId", "CallerEnteredDigitDefinitionTypeDescription");


                clientSubUnitTelephonyVM.ClientSubUnit = clientSubUnit;
                return View(clientSubUnitTelephonyVM);
            }

            //Update  Model from Form
            try
            {
                UpdateModel<ClientSubUnitTelephony>(clientSubUnitTelephonyVM.ClientSubUnitTelephony, "ClientSubUnitTelephony");
            }
            catch
            {
                string n = "";
                foreach (ModelState modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        n += error.ErrorMessage;
                    }
                }
                ViewData["Message"] = "ValidationError : " + n;
                return View("Error");
            }



            try
            {
                clientSubUnitTelephonyRepository.Add(clientSubUnitTelephonyVM.ClientSubUnitTelephony);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            return RedirectToAction("List", new { id = clientSubUnitGuid });
        }

        // GET: /Edit
        public ActionResult Edit(string id, string dn)
        {
            //Get ClientSubUnitTelephony
            ClientSubUnitTelephony clientSubUnitTelephony = new ClientSubUnitTelephony();
            clientSubUnitTelephony = clientSubUnitTelephonyRepository.GetClientSubUnitTelephony(id, dn);

            //Check Exists
            if (clientSubUnitTelephony == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(id))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ClientSubUnitTelephonyVM clientSubUnitTelephonyVM = new ClientSubUnitTelephonyVM();
            clientSubUnitTelephonyVM.ClientSubUnitTelephony = clientSubUnitTelephony;

            //Get ClientSubUnit
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(id);
            clientSubUnitTelephonyVM.ClientSubUnit = clientSubUnit;

            //Identifier Drop-down
            CallerEnteredDigitDefinitionTypeRepository callerEnteredDigitDefinitionTypeRepository = new CallerEnteredDigitDefinitionTypeRepository();
            clientSubUnitTelephonyVM.CallerEnteredDigitDefinitionTypes = new SelectList(callerEnteredDigitDefinitionTypeRepository.GetAllCallerEnteredDigitDefinitionTypes().ToList(), "CallerEnteredDigitDefinitionTypeId", "CallerEnteredDigitDefinitionTypeDescription", clientSubUnitTelephony.CallerEnteredDigitDefinitionTypeId);

            //Show Create Form
            return View(clientSubUnitTelephonyVM);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ClientSubUnitTelephonyVM clientSubUnitTelephonyVM, string OriginalDialedNumber)
        {
            //Get ClientSubUnitTelephony (clientSubUnitTelephony1 = already existing item)
            ClientSubUnitTelephony clientSubUnitTelephony1 = new ClientSubUnitTelephony();
            clientSubUnitTelephony1 = clientSubUnitTelephonyRepository.GetClientSubUnitTelephony(
                clientSubUnitTelephonyVM.ClientSubUnitTelephony.ClientSubUnitGuid, OriginalDialedNumber);
            

            //Check Exists
            if (clientSubUnitTelephony1 == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }
            string clientSubUnitGuid = clientSubUnitTelephony1.ClientSubUnitGuid;

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnitGuid))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }


            //here we use the exisitng CSUGuid and the new Dialled Number to check if the new item already exists
            ClientSubUnitTelephony clientSubUnitTelephony2 = new ClientSubUnitTelephony();
            clientSubUnitTelephony2 = clientSubUnitTelephonyRepository.GetClientSubUnitTelephony(clientSubUnitGuid, clientSubUnitTelephonyVM.ClientSubUnitTelephony.DialedNumber);
            if (clientSubUnitTelephony2 != null)
            {
                if (OriginalDialedNumber != clientSubUnitTelephonyVM.ClientSubUnitTelephony.DialedNumber)
                {
                    ModelState.AddModelError("ClientSubUnitTelephony.DialedNumber", "This number already exists");

                    CallerEnteredDigitDefinitionTypeRepository callerEnteredDigitDefinitionTypeRepository = new CallerEnteredDigitDefinitionTypeRepository();
                    clientSubUnitTelephonyVM.CallerEnteredDigitDefinitionTypes = new SelectList(callerEnteredDigitDefinitionTypeRepository.GetAllCallerEnteredDigitDefinitionTypes().ToList(), "CallerEnteredDigitDefinitionTypeId", "CallerEnteredDigitDefinitionTypeDescription");

                    //Get ClientSubUnit
                    ClientSubUnit clientSubUnit = new ClientSubUnit();
                    clientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientSubUnitGuid);
                    clientSubUnitTelephonyVM.ClientSubUnit = clientSubUnit;

                    return View(clientSubUnitTelephonyVM);
                }
            }

            //Update  Model from Form
            try
            {
                UpdateModel<ClientSubUnitTelephony>(clientSubUnitTelephonyVM.ClientSubUnitTelephony, "ClientSubUnitTelephony");
            }
            catch
            {
                string n = "";
                foreach (ModelState modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        n += error.ErrorMessage;
                    }
                }
                ViewData["Message"] = "ValidationError : " + n;
                return View("Error");
            }



            try
            {
                clientSubUnitTelephonyRepository.Update(clientSubUnitTelephonyVM.ClientSubUnitTelephony, OriginalDialedNumber);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            return RedirectToAction("List", new { id = clientSubUnitGuid });
        }

        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(string id, string dn)
        {
            //Get ClientSubUnitTelephony
            ClientSubUnitTelephony clientSubUnitTelephony = new ClientSubUnitTelephony();
            clientSubUnitTelephony = clientSubUnitTelephonyRepository.GetClientSubUnitTelephony(id, dn);

            //Check Exists
            if (clientSubUnitTelephony == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(id))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ClientSubUnitTelephonyVM clientSubUnitTelephonyVM = new ClientSubUnitTelephonyVM();
            clientSubUnitTelephonyVM.ClientSubUnitTelephony = clientSubUnitTelephony;

            //Get ClientSubUnit
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(id);
            clientSubUnitTelephonyVM.ClientSubUnit = clientSubUnit;

            //Show Form
            return View(clientSubUnitTelephonyVM);
        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(ClientSubUnitTelephonyVM clientSubUnitTelephonyVM)
        {
            //Get ClientSubUnitTelephony
            ClientSubUnitTelephony clientSubUnitTelephony = new ClientSubUnitTelephony();
            clientSubUnitTelephony = clientSubUnitTelephonyRepository.GetClientSubUnitTelephony(clientSubUnitTelephonyVM.ClientSubUnitTelephony.ClientSubUnitGuid, clientSubUnitTelephonyVM.ClientSubUnitTelephony.DialedNumber);

            //Check Exists
            if (clientSubUnitTelephony == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            //Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnitTelephony.ClientSubUnitGuid))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Delete Item
            try
            {
                clientSubUnitTelephonyRepository.Delete(clientSubUnitTelephonyVM.ClientSubUnitTelephony);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/ClientSubUnitTelephony.mvc/Delete/id=" + clientSubUnitTelephony.ClientSubUnitGuid + "&dn=" + clientSubUnitTelephony.DialedNumber;
                    return View("VersionError");
                }
                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            //Return
            return RedirectToAction("List", new { id = clientSubUnitTelephony.ClientSubUnitGuid });
        }
    }
}
