using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Repository;
using System.Data.SqlClient;

namespace CWTDesktopDatabase.Controllers
{
    public class CreateProfileAdviceController : Controller
    {
        //main repositories
        ClientSubUnitCreateProfileAdviceRepository clientSubUnitCreateProfileAdviceRepository = new ClientSubUnitCreateProfileAdviceRepository();
        ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
        ClientDetailRepository clientDetailRepository = new ClientDetailRepository();

        //GET:List
        public ActionResult List(string id, int? page, string sortField, int? sortOrder)
        {
            //Get PolicyAirVendorGroupItem
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(id);

            //Check Exists
            if (clientSubUnit == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }

            //Set Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnit.ClientSubUnitGuid))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //Parent Information
            ViewData["ClientSubUnitGuid"] = clientSubUnit.ClientSubUnitGuid;
            ViewData["ClientSubUnitName"] = clientSubUnit.ClientSubUnitName;

            //SortField+SortOrder settings
            if (sortField == null || sortField == "Name")
            {
                sortField = "LanguageName";
            }
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

            //Get data
			var paginatedView = clientSubUnitCreateProfileAdviceRepository.GetProfileAdviceLanguages(id, page ?? 1, sortField, sortOrder ?? 0);

            //Return View
            return View(paginatedView);
        }

        //GET: View
        public ActionResult ViewItem(string id, string languageCode)
        {
            ClientSubUnitCreateProfileAdvice clientSubUnitCreateProfileAdvice = new ClientSubUnitCreateProfileAdvice();
            clientSubUnitCreateProfileAdvice = clientSubUnitCreateProfileAdviceRepository.GetItem(id, languageCode);
            if (clientSubUnitCreateProfileAdvice == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }
            clientSubUnitCreateProfileAdviceRepository.EditItemForDisplay(clientSubUnitCreateProfileAdvice);
            return View(clientSubUnitCreateProfileAdvice);
        }

        // GET: /Create
        public ActionResult Create(string id)
        {
            //Get PolicyCarVendorGroupItem
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(id);

            //Check Exists
            if (clientSubUnit == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnit.ClientSubUnitGuid))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //New ClientSubUnitCreateProfileAdvice
            ClientSubUnitCreateProfileAdvice clientSubUnitCreateProfileAdvice = new ClientSubUnitCreateProfileAdvice();
            clientSubUnitCreateProfileAdvice.ClientSubUnitGuid = id;
            clientSubUnitCreateProfileAdviceRepository.EditItemForDisplay(clientSubUnitCreateProfileAdvice);

            //Language SelectList
            SelectList languageList = new SelectList(clientSubUnitCreateProfileAdviceRepository.GetUnUsedLanguages(id).ToList(), "LanguageCode", "LanguageName");
            ViewData["Languages"] = languageList;

            //Show Create Form
            return View(clientSubUnitCreateProfileAdvice);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClientSubUnitCreateProfileAdvice clientSubUnitCreateProfileAdvice)
        {

            //Get ClientSubUnit
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientSubUnitCreateProfileAdvice.ClientSubUnitGuid);

            //Check Exists
            if (clientSubUnit == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnit.ClientSubUnitGuid))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }


            //Update  Model from Form
            try
            {
                UpdateModel(clientSubUnitCreateProfileAdvice);
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
                clientSubUnitCreateProfileAdviceRepository.AddCreateProfileAdvice(clientSubUnitCreateProfileAdvice);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List", new { id = clientSubUnitCreateProfileAdvice.ClientSubUnitGuid });
        }

        // GET: /Edit
        public ActionResult Edit(string id, string languageCode)
        {
            //Get Item 
            ClientSubUnitCreateProfileAdvice clientSubUnitCreateProfileAdvice = new ClientSubUnitCreateProfileAdvice();
            clientSubUnitCreateProfileAdvice = clientSubUnitCreateProfileAdviceRepository.GetItem(id, languageCode);

            //Check Exists
            if (clientSubUnitCreateProfileAdvice == null)
            {
                return View("Error");
            }

            //Get ClientSubUnit
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientSubUnitCreateProfileAdvice.ClientSubUnitGuid);

            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnit.ClientSubUnitGuid))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Language SelectList
            SelectList languageList = new SelectList(clientSubUnitCreateProfileAdviceRepository.GetUnUsedLanguages(id).ToList(), "LanguageCode", "LanguageName");
            ViewData["Languages"] = languageList;

            clientSubUnitCreateProfileAdviceRepository.EditItemForDisplay(clientSubUnitCreateProfileAdvice);
            return View(clientSubUnitCreateProfileAdvice);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string clientSubUnitGuid, string languageCode, string createProfileAdvice, FormCollection collection)
        {
            //Get Item 
            ClientSubUnitCreateProfileAdvice clientSubUnitCreateProfileAdvice = new ClientSubUnitCreateProfileAdvice();
            clientSubUnitCreateProfileAdvice = clientSubUnitCreateProfileAdviceRepository.GetItem(clientSubUnitGuid, languageCode);

            //Check Exists
            if (clientSubUnitCreateProfileAdvice == null)
            {
                return View("Error");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnitGuid))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ////Update CarAdvice
            try
            {
                clientSubUnitCreateProfileAdviceRepository.UpdateCreateProfileAdvice(clientSubUnitGuid, languageCode, createProfileAdvice, Int32.Parse(collection["VersionNumber"]));
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/CreateProfileAdvice.mvc/Edit?languageCode=" + languageCode + "&id=" + clientSubUnitGuid.ToString();
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }




            return RedirectToAction("List", new { id = clientSubUnitCreateProfileAdvice.ClientSubUnitGuid });
        }

        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(string id, string languageCode)
        {
            //Get Item 
            ClientSubUnitCreateProfileAdvice clientSubUnitCreateProfileAdvice = new ClientSubUnitCreateProfileAdvice();
            clientSubUnitCreateProfileAdvice = clientSubUnitCreateProfileAdviceRepository.GetItem(id, languageCode);

            //Check Exists
            if (clientSubUnitCreateProfileAdvice == null)
            {
                return View("Error");
            }

            //Get ClientSubUnit
            ClientSubUnit clientSubUnit = new ClientSubUnit();
            clientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientSubUnitCreateProfileAdvice.ClientSubUnitGuid);

            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(clientSubUnit.ClientSubUnitGuid))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }


            //Add Linked Information
            clientSubUnitCreateProfileAdviceRepository.EditItemForDisplay(clientSubUnitCreateProfileAdvice);

            //Return View
            return View(clientSubUnitCreateProfileAdvice);

        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, string languageCode, FormCollection collection)
        {
            //Get Item 
            ClientSubUnitCreateProfileAdvice clientSubUnitCreateProfileAdvice = new ClientSubUnitCreateProfileAdvice();
            clientSubUnitCreateProfileAdvice = clientSubUnitCreateProfileAdviceRepository.GetItem(id, languageCode);

            //Check Exists
            if (clientSubUnitCreateProfileAdvice == null)
            {
                return View("Error");
            }

            //Check AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientSubUnit(id))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Delete
            try
            {
                clientSubUnitCreateProfileAdvice.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                clientSubUnitCreateProfileAdviceRepository.DeleteCreateProfileAdvice(clientSubUnitCreateProfileAdvice);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/CreateProfileAdvice.mvc/Delete?languageCode=" + languageCode + "&id=" + id.ToString();
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            //Return
            return RedirectToAction("List", new { id = clientSubUnitCreateProfileAdvice.ClientSubUnitGuid });
        }
    }
}
